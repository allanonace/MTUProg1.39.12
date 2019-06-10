// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using aclara_meters.Helpers;
using aclara_meters.Models;
using Acr.UserDialogs;
using Library;
using MTUComm;
using nexus.protocols.ble.scan;
using Plugin.Settings;


using System.Security.Cryptography.X509Certificates;
using Xamarin.Forms;

using ActionType = MTUComm.Action.ActionType;
using ble_library;

namespace aclara_meters.view
{
    public partial class AclaraViewMainMenu
    {
        private bool autoConnect;
        private bool conectarDevice;
        private bool bAlertBatt = true;
        private bool bAlertBatt10 = true;

        private ActionType actionType;

        public DeviceItem last_item;

        private List<PageItem> MenuList { get; set; }
        private IUserDialogs dialogsSaved;
        private ObservableCollection<DeviceItem> listPucks;

        private int peripheralConnected = ble_library.BlePort.NO_CONNECTED;
        private Boolean peripheralManualDisconnection = false;
        private Thread printer;
        private int TimeOutSeconds = 5;


        private Command refresh_command;

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private bool GetDebugVar()
        {
            return false;
        }
        public AclaraViewMainMenu()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            RefreshPuckData();
        }

        public AclaraViewMainMenu(IUserDialogs dialogs)
        {
            InitializeComponent();
            
            PrintToConsole($"-------------------------------   AclaraViewMainMenu     , thread: { Thread.CurrentThread.ManagedThreadId}");
            Settings.IsConnectedBLE = false;
            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar
            TappedListeners();
            LoadPreUIGFX();

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                LoadTabletUI();
            }
            else
            {
                LoadPhoneUI();
            }

            dialogsSaved = dialogs;
            LoadPostUIGFX();

            //Change username textview to Prefs. String
            if (FormsApp.credentialsService.UserName != null)
            {
                userName.Text = FormsApp.credentialsService.UserName; //"Kartik";
                CrossSettings.Current.AddOrUpdateValue("session_username", FormsApp.credentialsService.UserName);
            }

            LoadSideMenuElements();

            if (Device.Idiom == TargetIdiom.Phone)
            {
                background_scan_page.Opacity = 0;
                background_scan_page.FadeTo(1, 250);
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                backmenu.Scale = 1.42;

            }

            // Upload log files and then start pucks detection
            this.UpdateFiles ();
        }

        public async void UpdateFiles ()
        {
            // Upload log files
            if (FormsApp.config.Global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();
            
            // Init pucks detection
            InitRefreshCommand();
            Interface_background_scan_page();
        }

        public string GZipCompress ( string input )
        {
            using ( var outStream = new System.IO.MemoryStream () )
            {
                using ( var gzipStream = new System.IO.Compression.GZipStream ( outStream, System.IO.Compression.CompressionMode.Compress ) )
                {
                    using (var ms = new System.IO.MemoryStream ( System.Text.Encoding.UTF8.GetBytes ( input ) ) )
                    {
                        ms.CopyTo ( gzipStream );
                    }
                }
    
                return System.Text.Encoding.UTF8.GetString ( outStream.ToArray() );
            }
        }

        private void InitRefreshCommand()
        {
            refresh_command = new Command(async () =>
            {
                PrintToConsole($"----------------------REFRESH command dispositivos encontrados : {FormsApp.ble_interface.GetBlePeripheralList().Count}");
                PrintToConsole($"-------------------------------        REFRESH command, thread: { Thread.CurrentThread.ManagedThreadId}");

                if (!GetAutoConnectStatus())
                {
                    Esperando();

                    if (FormsApp.ble_interface.IsOpen()) FormsApp.ble_interface.Close();
                    
                    //FormsApp.ble_interface= new BleSerial();

                    Utils.PrintDeep("----------------------------------------------  init Ble_iterface");
                    if (printer.ThreadState == ThreadState.Suspended)
                    {
                        try
                        {
                            Utils.Print("---------------  printer resume");
                            //printer.Interrupt();
                            printer.Resume();
                        }
                        catch (Exception e11)
                        {
                            Utils.Print(e11.StackTrace);
                        }
                    }
                    //DeviceList.IsRefreshing = true;
                    listPucks = new ObservableCollection<DeviceItem>();
                   
                    FormsApp.ble_interface.SetTimeOutSeconds(TimeOutSeconds);
                    await FormsApp.ble_interface.Scan();
                    TimeOutSeconds = 3; // los siguientes escaneos son de 5 sec

                    if (FormsApp.ble_interface.GetBlePeripheralList().Count>0)
                    {

                        //await ChangeListViewData();
                        ChangeListViewData();

                        //DeviceList.IsRefreshing = false;
                        if (listPucks.Count != 0)
                        {
                            DeviceList.ItemsSource = listPucks;
                        }
                        if (conectarDevice)
                        {
                            PairWithKnowDevice();
                        }
                    }
                    else
                    {
                        DeviceList.ItemsSource = null;
                        Application.Current.MainPage.DisplayAlert("Alert", "No device found, please, press the button to turn on the device and refresh", "Ok");
                        Terminado();
                    }
                }
            });
        }

        private void PairWithKnowDevice()
        {
            autoConnect = false;
            conectarDevice = false;
            #region Autoconnect to stored device 

            //Utils.Print($"-----------------------------------va a conectar con : { Singleton.Get.Puck.Name }");
            //Task.Factory.StartNew(NewOpenConnectionWithDevice);
            NewOpenConnectionWithDevice();
            #endregion

        }
        private void Esperando()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                #region New Circular Progress bar Animations    
                DeviceList.IsRefreshing = false;
                backdark_bg.IsVisible = true;
                indicator.IsVisible = true;
                background_scan_page.IsEnabled = true;
                #endregion
            });
        }

        private void Terminado()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                #region Disable Circular Progress bar Animations when done

                backdark_bg.IsVisible = false;
                indicator.IsVisible = false;
                background_scan_page.IsEnabled = true;

                //DeviceList.IsEnabled = true;
                //fondo.Opacity = 1;
                //background_scan_page.Opacity = 1;
                //background_scan_page.IsEnabled = true;
                #endregion
            });
        }
        /*--------------------------------------------------*/
        /*          Device List Interface Contenview
        /---------------------------------------------------*/

        private bool GetAutoConnectStatus()
        {
            return autoConnect;
        }

       
        private void Interface_background_scan_page()
        {
            PrintToConsole($"-------------------------------    Interface_background_scan_page, thread: { Thread.CurrentThread.ManagedThreadId}");

            printer = new Thread(new ThreadStart(InvokeMethod));

            printer.Start();

            DeviceList.RefreshCommand = refresh_command;

            FirstRefreshSearchPucs();
               
        }



        public void FirstRefreshSearchPucs()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                refresh_command.Execute(true);
            });
        }

        private void LoadSideMenuElements()
        {
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection

            MenuList = new List<PageItem>();

            // Adding menu items to MenuList

            MenuList.Add(new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png",Color= "White", TargetType = ActionType.ReadMtu }); 

            if (FormsApp.config.Global.ShowTurnOff)
                MenuList.Add(new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", Color = "White", TargetType = ActionType.TurnOffMtu });

            if (FormsApp.config.Global.ShowAddMTU)
                MenuList.Add(new PageItem() { Title = "Add MTU", Icon = "addMTU.png", Color = "White", TargetType = ActionType.AddMtu });

            if (FormsApp.config.Global.ShowReplaceMTU)
                MenuList.Add(new PageItem() { Title = "Replace MTU", Icon = "replaceMTU2.png", Color = "White", TargetType = ActionType.ReplaceMTU });

            if (FormsApp.config.Global.ShowReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Replace Meter", Icon = "replaceMeter.png", Color = "White", TargetType = ActionType.ReplaceMeter });

            if (FormsApp.config.Global.ShowAddMTUMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Add Meter", Icon = "addMTUaddmeter.png", Color = "White", TargetType = ActionType.AddMtuAddMeter });

            if (FormsApp.config.Global.ShowAddMTUReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Rep. Meter", Icon = "addMTUrepmeter.png", Color = "White", TargetType = ActionType.AddMtuReplaceMeter });

            if (FormsApp.config.Global.ShowReplaceMTUMeter)
                MenuList.Add(new PageItem() { Title = "Rep.MTU / Rep. Meter", Icon = "repMTUrepmeter.png", Color = "White", TargetType = ActionType.ReplaceMtuReplaceMeter });

            if (FormsApp.config.Global.ShowInstallConfirmation)
                MenuList.Add(new PageItem() { Title = "Install Confirmation", Icon = "installConfirm.png", Color = "White", TargetType = ActionType.MtuInstallationConfirmation });

#if DEBUG
           // MenuList.Add(new PageItem() { Title = "Read Fabric", Icon = "readmtu_icon.png", Color = "White", TargetType = ActionType.ReadFabric });
#endif


            // ListView needs to be at least  elements for UI Purposes, even empty ones
            while (MenuList.Count < 9)
                MenuList.Add(new PageItem() { Title = "", Color = "#6aa2b8", Icon = "" });

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = MenuList;

        }

        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            if (Device.Idiom == TargetIdiom.Tablet)
                return;

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    SideMenuClose(sender, e);

                    //fondo.Opacity = 1;
                    //ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                    //shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    //background_scan_page.Opacity = 1;
                    //background_scan_page_detail.Opacity = 1;

                    //Task.Delay(200).ContinueWith(t =>
                    //Device.BeginInvokeOnMainThread(() =>
                    //{
                    //    ContentNav.Opacity = 0;
                    //    shadoweffect.IsVisible = false;
                    //    ContentNav.IsVisible = false;
                    //    background_scan_page.IsEnabled = true;
                    //    background_scan_page_detail.IsEnabled = true;
                    //}));

                    break;
                case SwipeDirection.Right:
                    SideMenuOpen(sender, e);
                    //fondo.Opacity = 0;
                    //ContentNav.IsVisible = true;
                    //shadoweffect.IsVisible = true;
                    //background_scan_page.Opacity = 0.5;
                    //background_scan_page_detail.Opacity = 0.5;
                    //ContentNav.Opacity = 1;
                    //ContentNav.TranslateTo(0, 0, 175, Easing.SinIn);
                    //shadoweffect.TranslateTo(0, 0, 175, Easing.SinIn);
                    //background_scan_page.IsEnabled = false;
                    //background_scan_page_detail.IsEnabled = false;
                    break;

            }
        }

        private void LoadPreUIGFX()
        {
            shadoweffect.IsVisible = false;
            background_scan_page_detail.IsVisible = true;
            background_scan_page_detail.IsVisible = false;
        }

        private void LoadPostUIGFX()
        {
            background_scan_page_detail.IsVisible = true;
            background_scan_page_detail.IsVisible = false;
            background_scan_page.IsVisible = true;
            navigationDrawerList.IsEnabled = true;
            navigationDrawerList.Opacity = 0.65;

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                ContentNav.Opacity = 1;
                ContentNav.IsVisible = true;
            }
            else
            {
                ContentNav.Opacity = 0;
                ContentNav.IsVisible = false;
            }



            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;
        }

        private void LoadPhoneUI()
        {
            background_scan_page.Margin = new Thickness(0, 0, 0, 0);
            background_scan_page_detail.Margin = new Thickness(0, 0, 0, 0);
            close_menu_icon.Opacity = 1;
            hamburger_icon.IsVisible = true;
            hamburger_icon_detail.IsVisible = true;
            aclara_detail_logo.IsVisible = true;
            aclara_logo.IsVisible = true;
            tablet_user_view.TranslationY = 0;
            tablet_user_view.Scale = 1;
            aclara_logo.IsVisible = true;
            logo_tablet_aclara.Opacity = 0;
            aclara_detail_logo.IsVisible = true;
            tablet_user_view.TranslationY = -22;
            tablet_user_view.Scale = 1.2;
            ContentNav.TranslationX = -310;
            shadoweffect.TranslationX = -310;
            ContentNav.IsVisible = true;
            shadoweffect.IsVisible = true;
            ContentNav.IsVisible = false;
            shadoweffect.IsVisible = false;
        }

        private void LoadTabletUI()
        {
            ContentNav.IsVisible = true;
            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;
            close_menu_icon.Opacity = 0;
            hamburger_icon.IsVisible = false;
            hamburger_icon_detail.IsVisible = false;
            background_scan_page.Margin = new Thickness(310, 0, 0, 0);
            background_scan_page_detail.Margin = new Thickness(310, 0, 0, 0);
            aclara_logo.IsVisible = true;
            logo_tablet_aclara.Opacity = 0;
            aclara_detail_logo.IsVisible = true;
            tablet_user_view.TranslationY = -22;
            tablet_user_view.Scale = 1.2;
            shadoweffect.IsVisible = true;
            aclara_logo.Scale = 1.2;
            aclara_detail_logo.Scale = 1.2;
            aclara_detail_logo.TranslationX = 42;
            aclara_logo.TranslationX = 42;

            shadoweffect.Source = "shadow_effect_tablet";

        }

        private void TappedListeners()
        {
            turnoffmtu_ok.Tapped += TurnOffMTUOkTapped;
            turnoffmtu_no.Tapped += TurnOffMTUNoTapped;
            turnoffmtu_ok_close.Tapped += TurnOffMTUCloseTapped;
            replacemeter_ok.Tapped += ReplaceMtuOkTapped;
            replacemeter_cancel.Tapped += ReplaceMtuCancelTapped;
            meter_ok.Tapped += MeterOkTapped;
            meter_cancel.Tapped += MeterCancelTapped;

            dialog_AddMTUAddMeter_ok.Tapped += dialog_AddMTUAddMeter_okTapped;
            dialog_AddMTUAddMeter_cancel.Tapped += dialog_AddMTUAddMeter_cancelTapped;

            dialog_AddMTUReplaceMeter_ok.Tapped += dialog_AddMTUReplaceMeter_okTapped;
            dialog_AddMTUReplaceMeter_cancel.Tapped += dialog_AddMTUReplaceMeter_cancelTapped;

            dialog_ReplaceMTUReplaceMeter_ok.Tapped += dialog_ReplaceMTUReplaceMeter_okTapped;
            dialog_ReplaceMTUReplaceMeter_cancel.Tapped += dialog_ReplaceMTUReplaceMeter_cancelTapped;


            dialog_AddMTU_ok.Tapped += dialog_AddMTU_okTapped;
            dialog_AddMTU_cancel.Tapped += dialog_AddMTU_cancelTapped;



            disconnectDevice.Tapped += BluetoothPeripheralDisconnect;
            back_button.Tapped += SideMenuOpen;
            back_button_menu.Tapped += SideMenuClose;
            logout_button.Tapped += LogoutTapped;
            back_button_detail.Tapped += SideMenuOpen;
            settings_button.Tapped += OpenSettingsTapped;

            logoff_no.Tapped += LogOffNoTapped;
            logoff_ok.Tapped += LogOffOkTapped;


            if (Device.Idiom == TargetIdiom.Tablet)
            {
                hamburger_icon_home.IsVisible = true;
                hamburger_icon_home_detail.IsVisible = true;

                hamburger_icon_home.Opacity = 0;
                hamburger_icon_home_detail.Opacity = 0;
            }

            refresh_signal.Tapped += refreshBleData;



        }

        private void refreshBleData(object sender, EventArgs e)
        {
            DeviceList.RefreshCommand.Execute(true);
        }


        /***
         * 
         *  //Dynamic battery detection when connected
         * 
         * 
            try
            {
                battery = FormsApp.ble_interface.GetBatteryLevel();

                if(battery != null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        
                        if(battery[0] < 101 && battery[0] > 1 )
                        {
                            batteryLevel.Text = battery[0].ToString() + " %";

                            if (battery[0] > 75)
                            {

                                imageBattery.Source = "battery_toolbar_high";
                                battery_level.Source = "battery_toolbar_high_white";
                                battery_level_detail.Source = "battery_toolbar_high_white";
                            }

                            if (battery[0] > 45 && battery[0] < 75)
                            {

                                imageBattery.Source = "battery_toolbar_mid";
                                battery_level.Source = "battery_toolbar_mid_white";
                                battery_level_detail.Source = "battery_toolbar_mid_white";
                            }

                            if (battery[0] > 15 && battery[0] < 45)
                            {

                                imageBattery.Source = "battery_toolbar_low";
                                battery_level.Source = "battery_toolbar_low_white";
                                battery_level_detail.Source = "battery_toolbar_low_white";
                            }

                            if (battery[0] < 15)
                            {

                                imageBattery.Source = "battery_toolbar_empty";
                                battery_level.Source = "battery_toolbar_empty_white";
                                battery_level_detail.Source = "battery_toolbar_empty_white";
                            }

                        }
                    });
                }
            }catch (Exception e5){
                
            }
         *
         ***/

        private void RefreshPuckData(bool Firtstime = false)
        {
            if (!Singleton.Has<Puck>()) return;

            Puck puck = Singleton.Get.Puck;
            int battery;
            string batteryIcon;

            deviceID.Text = puck.Name;
            macAddress.Text = puck.SerialNumber;

            if (Firtstime)
            {
                battery = puck.BatteryLevelFix;
                batteryIcon = puck.BatteryLevelIconFix;
            }
            else {
                battery = puck.BatteryLevel;
                batteryIcon = puck.BatteryLevelIcon;
            }

            int rssi = puck.RSSI;
            string rssiIcon = puck.RSSIIcon;

            Device.BeginInvokeOnMainThread(async() =>
            {
                batteryLevel.Text = battery.ToString() + " %";

                imageBattery.Source = batteryIcon;
                battery_level_detail.Source = batteryIcon + "_white";

                imageRssi.Source = rssiIcon;
                rssi_level_detail.Source = rssiIcon + "_white";

                if (battery == 20 && bAlertBatt)
                {
                    await Application.Current.MainPage.DisplayAlert("Attention", "The battery level is at 20%", "OK");
                    bAlertBatt = false;
                }
                if (battery == 10 && bAlertBatt10)
                {
                    await Application.Current.MainPage.DisplayAlert("Attention", "The battery level is at 10%, soon the puck will turn off", "OK");
                    bAlertBatt10 = false;
                }
            });

            //Save Battery & Rssi info for the next windows
            CrossSettings.Current.AddOrUpdateValue("battery_icon_topbar", batteryIcon + "_white");
            CrossSettings.Current.AddOrUpdateValue("rssi_icon_topbar", rssiIcon + "_white");
        }

        private void InvokeMethod()
        {
            //PrintToConsole("dentro del metodo - InvokeMethod");

            int timeout_connecting = 0;
            int cont = 0;
            int refresh = 0;

            //bAlertBatt = false;
            //PrintToConsole("se va a ejecutar un bucle (WHILE TRUE) - InvokeMethod");

            while (true)
            {
                //Utils.Print($"---------------------------------Invoke method while ----dispositivos encontados : {FormsApp.ble_interface.GetBlePeripheralList().Count}");
                //Utils.Print($"---------------------------------Invoke method while ---- Thread: {Thread.CurrentThread.ManagedThreadId}");

                if (Settings.IsLoggedIn == false) 
                    break;

                int status = FormsApp.ble_interface.GetConnectionStatus();

                //Utils.Print("se obtiene el estado de la conexion - InvokeMethod");

                if (cont == 2000)
                {
                    if (refresh == 4)
                    {
                        refresh = 0;
                        bAlertBatt = true;
                        bAlertBatt10 = true;
                    }
                    else refresh += 1;

                    RefreshPuckData();
                    cont = 0;
                }
                else cont += 1;

                if (status != peripheralConnected)
                {

                   // Utils.Print($"---------------------------------Invoke method ----estado : {status} , Perifericoconnected: {peripheralConnected}");
                   // Utils.Print($"---------------------------------Invoke method ---- Thread: {Thread.CurrentThread.ManagedThreadId}");
                    
                    //PrintToConsole("¿ES NO_CONNECTED? - InvokeMethod");

                    if (peripheralConnected == ble_library.BlePort.NO_CONNECTED)
                    {
                        //Utils.Print("    NO_CONNECTED - InvokeMethod");
                        peripheralConnected = status;
                        timeout_connecting = 0;
                    }
                    else if (peripheralConnected == ble_library.BlePort.CONNECTING)
                    {
                        //Utils.Print("Nop, es CONNECTING - InvokeMethod");

                        if (status == ble_library.BlePort.NO_CONNECTED)
                        {
                            //Utils.Print("Se va a ejecutar algo en la UI - InvokeMethod");

                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //Utils.Print("Se va a detectar el estado de la conexion - InvokeMethod");

                                switch (FormsApp.ble_interface.GetConnectionError())
                                {
                                    case ble_library.BlePort.NO_ERROR:
                                        //Utils.Print("Estado conexion: NO_ERROR - InvokeMethod");
                                        break;
                                    case ble_library.BlePort.CONECTION_ERRROR:
                                        //Utils.Print("Estado conexion: CONECTION_ERRROR - InvokeMethod");

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            #region New Circular Progress bar Animations    

                                            DeviceList.IsRefreshing = false;
                                            backdark_bg.IsVisible = false;
                                            indicator.IsVisible = false;
                                            background_scan_page.IsEnabled = true;

                                            #endregion
                                        });

                                        //Utils.Print("Desactivar barra de progreso - InvokeMethod");

                                        Application.Current.MainPage.DisplayAlert("Alert", "Connection error. Please, retry", "Ok");
                                        break;
                                    case ble_library.BlePort.DYNAMIC_KEY_ERROR:
                                        //Utils.Print("Estado conexion: DYNAMIC_KEY_ERROR - InvokeMethod");

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            #region New Circular Progress bar Animations    

                                            DeviceList.IsRefreshing = false;
                                            backdark_bg.IsVisible = false;
                                            indicator.IsVisible = false;
                                            background_scan_page.IsEnabled = true;

                                            #endregion
                                        });

                                        //Utils.Print("Desactivar barra de progreso - InvokeMethod");
                                        Application.Current.MainPage.DisplayAlert("Alert", "Please, press the button to change PAIRING mode", "Ok");
                                        break;
                                    case ble_library.BlePort.NO_DYNAMIC_KEY_ERROR:
                                        //Utils.Print("Estado conexion: NO_DYNAMIC_KEY_ERROR - InvokeMethod");

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            #region New Circular Progress bar Animations    

                                            DeviceList.IsRefreshing = false;
                                            backdark_bg.IsVisible = false;
                                            indicator.IsVisible = false;
                                            background_scan_page.IsEnabled = true;

                                            #endregion

                                        });
                                        //Utils.Print("Desactivar barra de progreso - InvokeMethod");
                                        Application.Current.MainPage.DisplayAlert("Alert", "Please, press the button to change PAIRING mode", "Ok");
                                        break;
                                }
                                DeviceList.IsEnabled = true;
                                
                                fondo.Opacity = 1;
                                background_scan_page.Opacity = 1;
                                background_scan_page.IsEnabled = true;

                            });
                            peripheralConnected = status;
                            Singleton.Remove<Puck> ();
                           
                            bAlertBatt = true;
                            bAlertBatt10 = true;
                        }
                        else // status == ble_library.BlePort.CONNECTED
                        {
                            //Utils.Print("Estas Conectado - InvokeMethod");

                            DeviceList.IsEnabled = true;
                           
                            peripheralConnected = status;
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                fondo.Opacity = 1;
                                background_scan_page.Opacity = 1;
                                background_scan_page.IsEnabled = true;

                                IsConnectedUIChange(true);
                            });
                        }
                    }
                    else if (peripheralConnected == ble_library.BlePort.CONNECTED)
                    {
                        //Utils.Print("Nop, es CONNECTED - InvokeMethod");

                        DeviceList.IsEnabled = true;
                       
                        peripheralConnected = status;
                        Singleton.Remove<Puck> ();
                        bAlertBatt = true;
                        bAlertBatt10 = true;

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            fondo.Opacity = 1;
                            background_scan_page.Opacity = 1;
                            background_scan_page.IsEnabled = true;
                            //desconectar disp
                            IsConnectedUIChange(false);
                        });
                    }
                }

                //Utils.Print("¿Está en CONNECTING? - InvokeMethod");
                if (peripheralConnected == ble_library.BlePort.CONNECTING)
                {
                    //Utils.Print("Si, es CONNECTING - InvokeMethod");
                    timeout_connecting++;
                    if (timeout_connecting >= 2 * 12) // 10 seconds
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            //Utils.Print("Un Timeout que te llevas - InvokeMethod");
                            Application.Current.MainPage.DisplayAlert("Timeout", "Connection Timeout", "Ok");

                            listPucks = new ObservableCollection<DeviceItem>();
                            DeviceList.ItemsSource = listPucks;

                            DeviceList.IsEnabled = true;
                            fondo.Opacity = 1;
                            background_scan_page.Opacity = 1;
                            background_scan_page.IsEnabled = true;

                            autoConnect = false;

                            #region Disable Circular Progress bar Animations when done

                                backdark_bg.IsVisible = false;
                                indicator.IsVisible = false;
                                background_scan_page.IsEnabled = true;

                            #endregion

                            try
                            {
                                Utils.Print("---------------  printer suspend");
                                printer.Suspend();
                            }
                            catch (Exception e5)
                            {
                                Utils.Print($"------ {e5.StackTrace}, {e5.Message}");
                            }


                        });
                        peripheralConnected = ble_library.BlePort.NO_CONNECTED;
                        timeout_connecting = 0;

                        FormsApp.ble_interface.Close();
                    }
                }
                else
                {
                    //Utils.Print("Nop, no es CONNECTING - InvokeMethod");
                }

                
                //Utils.Print("Esperamos 300 ms - InvokeMethod");
                Thread.Sleep(300); // 0.5 Second
                
                //Utils.Print("¿Se va a realizar reconexion? - InvokeMethod");

            }

        }

        private void IsConnectedUIChange(bool v)
        {
            //Utils.Print($"---------------------------------IsConnectedUIChange param: {v} ---- Thread: {Thread.CurrentThread.ManagedThreadId}");
            if (v)
            {
                try
                {
                    RefreshPuckData(true);

                }
                catch (Exception e)
                {
                    Utils.Print(e.StackTrace);
                }

                background_scan_page_detail.IsVisible = true;
                block_ble_disconnect.Opacity = 0;
                block_ble_disconnect.FadeTo(1, 250);
                block_ble.Opacity = 0;
                block_ble.FadeTo(1, 250);
                background_scan_page.IsVisible = false;
                navigationDrawerList.IsEnabled = true;
                navigationDrawerList.Opacity = 1;

                #region Disable Circular Progress bar Animations when done

                backdark_bg.IsVisible = false;
                indicator.IsVisible = false;

                #endregion



            }
            else
            {
                background_scan_page_detail.IsVisible = false;
                navigationDrawerList.Opacity = 0.65;
                navigationDrawerList.IsEnabled = true;
                background_scan_page.IsVisible = true;
                refresh_command.Execute(true);
                Navigation.PopToRootAsync();


            }
        }

        //private async Task ChangeListViewData()
        private  void ChangeListViewData()
        {
            //await Task.Factory.StartNew(() =>
            // {
            // wait until scan finish
            //Utils.Print($"-------------------------------    ChangeListViewData, thread: {Thread.CurrentThread.ManagedThreadId}");
              //  while (FormsApp.ble_interface.IsScanning())
              //  {
                    try
                    {
                       // Utils.Print($"------------------------------- ChangeListViewData while IsScanning, thread: {Thread.CurrentThread.ManagedThreadId}");
                        List<IBlePeripheral> blePeripherals;
                        blePeripherals = FormsApp.ble_interface.GetBlePeripheralList();

                        // YOU CAN RETURN THE PASS BY GETTING THE STRING AND CONVERTING IT TO BYTE ARRAY TO AUTO-PAIR
                        byte[] bytes = System.Convert.FromBase64String(CrossSettings.Current.GetValueOrDefault("session_peripheral_DeviceId", string.Empty));

                        byte[] byte_now = new byte[] { };

                        int sizeList = blePeripherals.Count;

                        for (int i = 0; i < sizeList; i++)
                        {
                            try
                            {
                                if (blePeripherals[i] != null)
                                {
                                    Puck puck = new Puck ( blePeripherals[ i ] );
                                    //puck.BlInterfaz= FormsApp.ble_interface;

                                    byte_now = puck.ManofacturerData;

                                    bool enc = false;
                                    int sizeListTemp = listPucks.Count;

                                    for (int j = 0; j < sizeListTemp; j++)
                                    {
                                        if ( listPucks[j].Peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray()
                                                .SequenceEqual ( puck.ManofacturerData ) )
                                            enc = true;
                                    }

                                    if (!enc)
                                    {
                                        int    bateria     = puck.BatteryLevel;
                                        string iconBattery = puck.BatteryLevelIcon;

                                        int    rssi     = puck.RSSI;
                                        string iconRSSI = puck.RSSIIcon;

                                        DeviceItem device = new DeviceItem
                                        {
                                            deviceMacAddress  = puck.SerialNumber,
                                            deviceName        = puck.Name,
                                            deviceBattery     = bateria + "%",
                                            deviceRssi        = rssi + " dBm",
                                            deviceBatteryIcon = iconBattery,
                                            deviceRssiIcon    = iconRSSI,
                                            Peripheral        = puck.Device
                                        };

                                        listPucks.Add(device);

                                        //VERIFY IF PREVIOUSLY BOUNDED DEVICES WITH THE RIGHT USERNAME
                                        if (CrossSettings.Current.GetValueOrDefault("session_dynamicpass", string.Empty) != string.Empty &&
                                            FormsApp.credentialsService.UserName.Equals(CrossSettings.Current.GetValueOrDefault("session_username", string.Empty)) &&
                                            bytes.Take(4).ToArray().SequenceEqual(byte_now) &&
                                            puck.Name.Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral", string.Empty)) &&
                                            ! peripheralManualDisconnection &&
                                            ! Singleton.Has<Puck>() )
                                        {
                                            if (!FormsApp.ble_interface.IsOpen())
                                            {
                                                try
                                                {
                                                    Singleton.Set = new Puck ();
                                                    Singleton.Get.Puck.Device = blePeripherals[ i ];
                                                    Singleton.Get.Puck.BlInterfaz = FormsApp.ble_interface;
                                                    
                                                    peripheralConnected = ble_library.BlePort.NO_CONNECTED;
                                                    peripheralManualDisconnection = false;

                                                    #region Autoconnect to stored device 

                                                    conectarDevice = true;

                                                    autoConnect = true;

                                                    #endregion
                                                }
                                                catch (Exception e)
                                                {
                                                    Utils.Print(e.StackTrace);
                                                }

                                            }
                                            else
                                            {
                                                if (autoConnect)
                                                {
                                                    Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        #region Disable Circular Progress bar Animations when done

                                                        backdark_bg.IsVisible = false;
                                                        indicator.IsVisible = false;
                                                        background_scan_page.IsEnabled = true;

                                                        #endregion
                                                    });
                                                }
                                            }
                                        }
                                        else
                                        {
                                            // if (autoConnect)
                                            //  {

                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                #region Disable Circular Progress bar Animations when done

                                                DeviceList.IsRefreshing = false;
                                                backdark_bg.IsVisible = false;
                                                indicator.IsVisible = false;
                                                background_scan_page.IsEnabled = true;

                                                #endregion

                                            });

                                            //  }

                                        }
                                    }
                                }
                            }
                            catch (Exception er)
                            {
                                Utils.Print(er.StackTrace); //2018-09-21 13:08:25.918 aclara_meters.iOS[505:190980] System.NullReferenceException: Object reference not set to an instance of an object
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Utils.Print(e);
                    }
                //}
           // });
        }


        #region We want to connect to the device if there is not scanning running

        private void NewOpenConnectionWithDevice()
        {

            //Utils.Print("Se va a entrar en un bucle mientras esté Escaneando bluetooth - NewOpenConnectionWithDevice");

            while (FormsApp.ble_interface.IsScanning())
            {
               // Utils.Print("A esperar 100 ms mientras escanea... - NewOpenConnectionWithDevice");
                Thread.Sleep(100);
            }

            //Utils.Print("Se va a ejecutar algo en el UI - NewOpenConnectionWithDevice");

            Device.BeginInvokeOnMainThread(() =>
            {
                var seconds = TimeSpan.FromSeconds(1); // Don't execute it asap!

                Device.StartTimer(seconds, () =>
                {
                    //Utils.Print("Cada 1 segundo, se ejectua lo siguinete en el UI - NewOpenConnectionWithDevice");
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        //Utils.Print("¿Esta la conexion abierta ? - NewOpenConnectionWithDevice");


                        if (!FormsApp.ble_interface.IsOpen())
                        {
                            //Utils.Print("¿Esta escaneando perifericos ? - NewOpenConnectionWithDevice");
                            while (FormsApp.ble_interface.IsScanning())
                            {
                                //Utils.Print("A esperar 100 ms en bucle - NewOpenConnectionWithDevice");
                                Thread.Sleep(100);
                            }
                            /* MRA
                            while(FormsApp.ble_interface.GetConnectionStatus() != ble_library.BlePort.CONNECTING)
                            {

                            }*/
                            // call your method to check for notifications here
                            FormsApp.ble_interface.Open ( Singleton.Get.Puck.Device, true );
                        }
                        else
                        {
                            //Utils.Print("NOPE, no lo esta - NewOpenConnectionWithDevice");
                        }
                    });

                    return false;
                });
            });
        }

        #endregion

        private void DoBasicRead()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Task.Factory.StartNew(BasicReadThread);
            });
        }

        private async void LogOffOkTapped(object sender, EventArgs e)
        {
            // Upload log files
            if (FormsApp.config.Global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();

            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            printer.Abort(); //.Suspend();

            FormsApp.DoLogOff();

            background_scan_page.IsEnabled = true;
            background_scan_page_detail.IsEnabled = true;
            
            Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogsSaved));
            //Navigation.PopAsync();

        }

        private void LogOffNoTapped(object sender, EventArgs e)
        {
            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void ReplaceMtuCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void ReplaceMtuOkTapped(object sender, EventArgs e)
        {
            dialog_replacemeter_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();

        }




        private void TurnOffMTUCloseTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void TurnOffMTUNoTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void TurnOffMTUOkTapped(object sender, EventArgs e)
        {
            dialog_turnoff_one.IsVisible = false;
            dialog_turnoff_two.IsVisible = true;

            Task.Factory.StartNew(TurnOffMethod);
        }

        private void TurnOffMethod()
        {

            MTUComm.Action turnOffAction = new MTUComm.Action(
                config: FormsApp.config,
                serial: FormsApp.ble_interface,
                type: MTUComm.Action.ActionType.TurnOffMtu,
                user: FormsApp.credentialsService.UserName);

            turnOffAction.OnFinish += ((s, args) =>
            {
                ActionResult actionResult = args.Result;

                Task.Delay(2000).ContinueWith(t =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       this.dialog_turnoff_text.Text = "MTU turned off Successfully";

                       dialog_turnoff_two.IsVisible = false;
                       dialog_turnoff_three.IsVisible = true;
                   }));
            });

            turnOffAction.OnError += (() =>
            {
                Task.Delay(2000).ContinueWith(t =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
                       this.dialog_turnoff_text.Text = "MTU turned off Unsuccessfully";

                       dialog_turnoff_two.IsVisible = false;
                       dialog_turnoff_three.IsVisible = true;
                   }));
            });

            turnOffAction.Run();
        }

        void MeterCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_meter_replace_one.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void MeterOkTapped(object sender, EventArgs e)
        {
            dialog_meter_replace_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();


        }

        void dialog_AddMTUAddMeter_cancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_AddMTUAddMeter.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void dialog_AddMTUAddMeter_okTapped(object sender, EventArgs e)
        {
            dialog_AddMTUAddMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();


        }

        void dialog_AddMTUReplaceMeter_cancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_AddMTUReplaceMeter.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void dialog_AddMTUReplaceMeter_okTapped(object sender, EventArgs e)
        {
            dialog_AddMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();

        }

        void dialog_ReplaceMTUReplaceMeter_cancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_ReplaceMTUReplaceMeter.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void dialog_ReplaceMTUReplaceMeter_okTapped(object sender, EventArgs e)
        {
            dialog_ReplaceMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();


        }

        void dialog_AddMTU_cancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_AddMTU.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void dialog_AddMTU_okTapped(object sender, EventArgs e)
        {
            dialog_AddMTU.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();

        }

        void BasicReadThread()
        {
            MTUComm.Action basicRead = new MTUComm.Action(
               config: FormsApp.config,
               serial: FormsApp.ble_interface,
               type: MTUComm.Action.ActionType.BasicRead,
               user: FormsApp.credentialsService.UserName);

            /*
            basicRead.OnFinish += ((s, args) =>
            { });
            */

            basicRead.OnFinish += ((s, e) =>
            {
                Task.Delay(100).ContinueWith(t =>
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved,  this.actionType), false);

                        #region New Circular Progress bar Animations    

                        DeviceList.IsRefreshing = false;
                        backdark_bg.IsVisible = false;
                        indicator.IsVisible = false;
                        background_scan_page.IsEnabled = true;

                        #endregion

                    })
                );
            });

            basicRead.OnError += (() =>
            {
                Task.Delay(100).ContinueWith(t =>
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            #region New Circular Progress bar Animations    

                            DeviceList.IsRefreshing = false;
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            background_scan_page.IsEnabled = true;

                            #endregion
                        });
                    })
                );
            });

            Device.BeginInvokeOnMainThread(() =>
            {
                #region New Circular Progress bar Animations    

                DeviceList.IsRefreshing = false;
                backdark_bg.IsVisible = true;
                indicator.IsVisible = true;
                background_scan_page.IsEnabled = true;

                #endregion

            });

            basicRead.Run();

           

        }


        private void BluetoothPeripheralDisconnect(object sender, EventArgs e)
        {

            FormsApp.ble_interface.Close();
            //peripheralConnected = ble_library.BlePort.NO_CONNECTED;
            //Singleton.Remove<Puck>();

            peripheralManualDisconnection = true;

            CrossSettings.Current.AddOrUpdateValue("session_dynamicpass", string.Empty);

        }

        private void LogoutTapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                dialog_turnoff_one.IsVisible = false;
                dialog_open_bg.IsVisible = true;
                dialog_meter_replace_one.IsVisible = false;
                dialog_turnoff_two.IsVisible = false;
                dialog_turnoff_three.IsVisible = false;
                dialog_replacemeter_one.IsVisible = false;
                dialog_logoff.IsVisible = true;
                dialog_open_bg.IsVisible = true;
                turnoff_mtu_background.IsVisible = true;
            });

        }

        public void externalReconnect(Boolean reassociate)
        {
            try
            {
                FormsApp.ble_interface.Open ( Singleton.Get.Puck.Device, reassociate );
            }
            catch (Exception e5)
            {
                Utils.Print(e5.StackTrace);
            }
           
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelectedListDevices(object sender, ItemTappedEventArgs e)
        {
            var item = (DeviceItem)e.Item;
            //fondo.Opacity = 0;
            //background_scan_page.Opacity = 0.5;
            background_scan_page.IsEnabled = false;

            #region New Circular Progress bar Animations    

            DeviceList.IsRefreshing = false;
            backdark_bg.IsVisible = true;
            indicator.IsVisible = true;

            #endregion

            bool reassociate = false;

            if (CrossSettings.Current.GetValueOrDefault("session_dynamicpass", string.Empty) != string.Empty &&
                FormsApp.credentialsService.UserName.Equals(CrossSettings.Current.GetValueOrDefault("session_username", string.Empty)) &&
                System.Convert.ToBase64String(item.Peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray())
                    .Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral_DeviceId", string.Empty)) &&
                item.Peripheral.Advertisement.DeviceName.Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral", string.Empty)))
            {
                reassociate = true;
            }

            last_item = item;

            try
            {
                Singleton.Set = new Puck ();
                Singleton.Get.Puck.Device = item.Peripheral;
                bAlertBatt = true;
                bAlertBatt10 = true;
                externalReconnect(reassociate);
                Singleton.Get.Puck.BlInterfaz = FormsApp.ble_interface;

                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        deviceID.Text = item.deviceName;
                        macAddress.Text = item.deviceMacAddress;
                        imageBattery.Source = item.deviceBatteryIcon;
                        imageRssi.Source = item.deviceRssiIcon;
                        batteryLevel.Text = item.deviceBattery;
                        rssiLevel.Text = item.deviceRssi;
                    }
                    catch (Exception e4)
                    {
                        Utils.Print(e4.StackTrace);
                    }
                });

            }
            catch (Exception e22)
            {
                Utils.Print(e22.StackTrace);
            }

        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, ItemTappedEventArgs e)
        {
            var item = (PageItem)e.Item;
            if (item.Title == String.Empty )
            {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;
                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;
                return;
            }
            if (!FormsApp.ble_interface.IsOpen())
            {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;
                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;
               
            }
            if (FormsApp.ble_interface.IsOpen())
            {
                navigationDrawerList.SelectedItem = null;
                try
                {
                    //var item = (PageItem)e.Item;
                    ActionType page = item.TargetType;
                    ((ListView)sender).SelectedItem = null;

                    this.actionType = page;

                    NavigationController(page);
                }
                catch (Exception w1)
                {
                    Utils.Print(w1.StackTrace);
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Connect to a device and retry", "Ok");
            }
        }

        private void NavigationController(ActionType page)
        {
            switch (page)
            {
                case ActionType.ReadFabric:

                    #region New Circular Progress bar Animations    

                    DeviceList.IsRefreshing = false;
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;

                    #endregion

                    #region Read Mtu Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            navigationDrawerList.SelectedItem = null;

                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved,page), false);

                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }
                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;

                            #region New Circular Progress bar Animations    

                            DeviceList.IsRefreshing = false;
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;

                            #endregion

                        })
                    );

                    #endregion

                    break;
                case ActionType.ReadMtu:

                    #region New Circular Progress bar Animations    

                    DeviceList.IsRefreshing = false;
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;

                    #endregion

                    #region Read Mtu Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            navigationDrawerList.SelectedItem = null;

                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved, page), false);

                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }
                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;

                            #region New Circular Progress bar Animations    

                            DeviceList.IsRefreshing = false;
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;

                            #endregion

                        })
                    );

                    #endregion

                    break;

                case ActionType.AddMtu:

                    #region Add Mtu Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;
                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            dialog_open_bg.IsVisible = true;
                            turnoff_mtu_background.IsVisible = true;
                            dialog_turnoff_one.IsVisible = false;
                            dialog_turnoff_two.IsVisible = false;
                            dialog_turnoff_three.IsVisible = false;
                            dialog_replacemeter_one.IsVisible = false;
                            dialog_meter_replace_one.IsVisible = false;

                            dialog_AddMTUAddMeter.IsVisible = false;
                            dialog_AddMTUReplaceMeter.IsVisible = false;
                            dialog_ReplaceMTUReplaceMeter.IsVisible = false;

                            #region Check ActionVerify

                            if (FormsApp.config.Global.ActionVerify)
                                dialog_AddMTU.IsVisible = true;
                            else
                                CallLoadViewAddMtu();

                            #endregion

                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }

                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone;
                        })
                    );

                    #endregion

                    break;

                case ActionType.TurnOffMtu:

                    #region Turn Off Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            dialog_open_bg.IsVisible = true;
                            turnoff_mtu_background.IsVisible = true;
                            dialog_meter_replace_one.IsVisible = false;

                            #region Check ActionVerify

                            if (FormsApp.config.Global.ActionVerify)
                                dialog_turnoff_one.IsVisible = true;
                            else
                                CallLoadViewTurnOff();

                            #endregion

                            dialog_turnoff_two.IsVisible = false;
                            dialog_turnoff_three.IsVisible = false;
                            dialog_replacemeter_one.IsVisible = false;

                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }

                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone;
                        })
                    );

                    #endregion

                    break;

                case ActionType.MtuInstallationConfirmation:

                    #region Install Confirm Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            navigationDrawerList.SelectedItem = null;

                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewInstallConfirmation(dialogsSaved), false);

                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }
                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone;
                        })
                    );

                    #endregion

                    break;

                case ActionType.ReplaceMTU:

                    #region Replace Mtu Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            dialog_open_bg.IsVisible = true;
                            turnoff_mtu_background.IsVisible = true;
                            dialog_meter_replace_one.IsVisible = false;
                            dialog_turnoff_one.IsVisible = false;
                            dialog_turnoff_two.IsVisible = false;
                            dialog_turnoff_three.IsVisible = false;

                            #region Check ActionVerify

                            if (FormsApp.config.Global.ActionVerify)
                                dialog_replacemeter_one.IsVisible = true;
                            else
                                CallLoadViewReplaceMtu();

                            #endregion

                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }

                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; //if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                        })
                    );

                    #endregion

                    break;

                case ActionType.ReplaceMeter:

                    #region Replace Meter Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }
                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            dialog_open_bg.IsVisible = true;
                            turnoff_mtu_background.IsVisible = true;
                            dialog_turnoff_one.IsVisible = false;
                            dialog_turnoff_two.IsVisible = false;
                            dialog_turnoff_three.IsVisible = false;
                            dialog_replacemeter_one.IsVisible = false;


                            #region Check ActionVerify

                            if (FormsApp.config.Global.ActionVerify)
                                dialog_meter_replace_one.IsVisible = true;
                            else
                                CallLoadViewReplaceMeter();

                            #endregion

                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }
                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                        })
                    );

                    #endregion

                    break;

                case ActionType.AddMtuAddMeter:

                    #region Add Mtu | Add Meter Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            dialog_open_bg.IsVisible = true;
                            turnoff_mtu_background.IsVisible = true;
                            dialog_turnoff_one.IsVisible = false;
                            dialog_turnoff_two.IsVisible = false;
                            dialog_turnoff_three.IsVisible = false;
                            dialog_replacemeter_one.IsVisible = false;
                            dialog_meter_replace_one.IsVisible = false;

                            #region Check ActionVerify

                            if (FormsApp.config.Global.ActionVerify)
                                dialog_AddMTUAddMeter.IsVisible = true;
                            else
                                CallLoadViewAddMTUAddMeter();

                            #endregion

                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }
                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                        })
                    );

                    #endregion

                    break;

                case ActionType.AddMtuReplaceMeter:

                    #region Add Mtu | Replace Meter Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            dialog_open_bg.IsVisible = true;
                            turnoff_mtu_background.IsVisible = true;
                            dialog_turnoff_one.IsVisible = false;
                            dialog_turnoff_two.IsVisible = false;
                            dialog_turnoff_three.IsVisible = false;
                            dialog_replacemeter_one.IsVisible = false;
                            dialog_meter_replace_one.IsVisible = false;
                            dialog_AddMTUAddMeter.IsVisible = false;

                            #region Check ActionVerify

                            if (FormsApp.config.Global.ActionVerify)
                                dialog_AddMTUReplaceMeter.IsVisible = true;
                            else
                                CallLoadViewAddMTUReplaceMeter();

                            #endregion

                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }
                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                        })
                    );

                    #endregion

                    break;

                case ActionType.ReplaceMtuReplaceMeter:

                    #region Replace Mtu | Replace Meter Controller

                    background_scan_page.Opacity = 1;
                    background_scan_page_detail.Opacity = 1;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            dialog_open_bg.IsVisible = true;
                            turnoff_mtu_background.IsVisible = true;
                            dialog_turnoff_one.IsVisible = false;
                            dialog_turnoff_two.IsVisible = false;
                            dialog_turnoff_three.IsVisible = false;
                            dialog_replacemeter_one.IsVisible = false;
                            dialog_meter_replace_one.IsVisible = false;
                            dialog_AddMTUAddMeter.IsVisible = false;
                            dialog_AddMTUReplaceMeter.IsVisible = false;

                            #region Check ActionVerify

                            if (FormsApp.config.Global.ActionVerify)
                                dialog_ReplaceMTUReplaceMeter.IsVisible = true;
                            else
                                CallLoadViewReplaceMTUReplaceMeter();

                            #endregion


                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            if (Device.Idiom == TargetIdiom.Tablet)
                            {
                                ContentNav.Opacity = 1;
                                ContentNav.IsVisible = true;
                            }
                            else
                            {
                                ContentNav.Opacity = 0;
                                ContentNav.IsVisible = false;
                            }
                            shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                        })
                    );

                    #endregion

                    break;

            }
        }

        private void CallLoadViewReplaceMTUReplaceMeter()
        {
            dialog_ReplaceMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();

        }

        private void CallLoadViewAddMTUReplaceMeter()
        {
            dialog_AddMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();
        }

        private void CallLoadViewAddMTUAddMeter()
        {

            dialog_AddMTUAddMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();

        }

        private void CallLoadViewReplaceMeter()
        {
            dialog_meter_replace_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();
        }

        private void CallLoadViewReplaceMtu()
        {
            dialog_replacemeter_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;


            DoBasicRead();

        }

        private void CallLoadViewTurnOff()
        {
            dialog_turnoff_one.IsVisible = false;
            dialog_turnoff_two.IsVisible = true;

            Task.Factory.StartNew(TurnOffMethod);
        }

        private void CallLoadViewAddMtu()
        {
            dialog_AddMTU.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();
        }



        private void OpenSettingsTapped(object sender, EventArgs e)
        {
            //printer.Suspend();
            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;
            background_scan_page.IsEnabled = true;
            background_scan_page_detail.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

           
            Task.Delay(200).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        #region New Circular Progress bar Animations    

                        DeviceList.IsRefreshing = false;
                        backdark_bg.IsVisible = true;
                        indicator.IsVisible = true;
                        background_scan_page.IsEnabled = false;

                        #endregion

                    });

                    if (FormsApp.ble_interface.IsOpen())
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new AclaraViewSettings(dialogsSaved), false);
                        if (Device.Idiom == TargetIdiom.Tablet)
                        {
                            ContentNav.Opacity = 1;
                            ContentNav.IsVisible = true;
                        }
                        else
                        {
                            ContentNav.Opacity = 0;
                            ContentNav.IsVisible = false;
                        }
                        background_scan_page.Opacity = 1;
                        background_scan_page_detail.Opacity = 1;

                        shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; //   if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;

                        Device.BeginInvokeOnMainThread(() =>
                        {

                            #region New Circular Progress bar Animations    

                            DeviceList.IsRefreshing = false;
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            background_scan_page.IsEnabled = true;

                            #endregion


                        });

                        return;
                    }
                    else
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new AclaraViewSettings(true), false);

                        if (Device.Idiom == TargetIdiom.Tablet)
                        {
                            ContentNav.Opacity = 1;
                            ContentNav.IsVisible = true;
                        }
                        else
                        {
                            ContentNav.Opacity = 0;
                            ContentNav.IsVisible = false;
                        }

                        background_scan_page.Opacity = 1;
                        background_scan_page_detail.Opacity = 1;

                        shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false; 

                        Device.BeginInvokeOnMainThread(() =>
                        {

                            #region New Circular Progress bar Animations    

                            DeviceList.IsRefreshing = false;
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            background_scan_page.IsEnabled = true;

                            #endregion


                        });

                    }
                }
                catch (Exception i2)
                {
                    Utils.Print(i2.StackTrace);
                }
            }));
        }

        private void SideMenuOpen(object sender, EventArgs e)
        {
            fondo.Opacity = 0;
            ContentNav.IsVisible = true;
            shadoweffect.IsVisible = true;
            background_scan_page.Opacity = 0.5;
            background_scan_page_detail.Opacity = 0.5;
            ContentNav.Opacity = 1;
            ContentNav.TranslateTo(0, 0, 175, Easing.SinIn);
            shadoweffect.TranslateTo(0, 0, 175, Easing.SinIn);
            background_scan_page.IsEnabled = false;
            background_scan_page_detail.IsEnabled = false;
        }

        private void SideMenuClose(object sender, EventArgs e)
        {
            fondo.Opacity = 1;
            ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
            shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;

            Task.Delay(200).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                ContentNav.Opacity = 0;
                shadoweffect.IsVisible = false;
                ContentNav.IsVisible = false;
                background_scan_page.IsEnabled = true;
                background_scan_page_detail.IsEnabled = true;
            }));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // todo: this is a hack - hopefully Xamarin adds the ability to name a Pushed Page.
            //MainMenu.IsSegmentShowing = false;
            bool value = FormsApp.ble_interface.IsOpen();
            value &= Navigation.NavigationStack.Count >= 3; //  if(Navigation.NavigationStack.Count < 3) Settings.IsLoggedIn = false;
        }


        public void PrintToConsole(string printConsole)
        {

            if (GetDebugVar())
            {
                Utils.Print("DEBUG_ACL: " + printConsole);
            }
        }


    }
}
