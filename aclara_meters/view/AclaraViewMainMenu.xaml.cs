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
using Xamarin.Forms;

using ActionType = MTUComm.Action.ActionType;
using ValidationResult = MTUComm.MTUComm.ValidationResult;


namespace aclara_meters.view
{
    public partial class AclaraViewMainMenu
    {
        private bool autoConnect;
        private bool conectarDevice;
        private bool bAlertBatt = true;
        private bool bAlertBatt10 = true;
        private MenuView menuOptions;
        private DialogsView dialogView;

        private ActionType actionType;

        private IUserDialogs dialogsSaved;
        private ObservableCollection<DeviceItem> listPucks;

        private int peripheralConnected = ble_library.BlePort.NO_CONNECTED;
        private Boolean peripheralManualDisconnection = false;
        private Thread connectionThread;
        private int TimeOutSeconds = 5;


        private Command refresh_command;

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public AclaraViewMainMenu()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshPuckData();
         
            background_scan_page.IsEnabled = true;
            background_scan_page_detail.IsEnabled = true;
            DeviceList.IsRefreshing = false;
            backdark_bg.IsVisible = false;
            indicator.IsVisible = false;
           
        }

        public AclaraViewMainMenu(IUserDialogs dialogs)
        {
            InitializeComponent();
            
            Utils.PrintDeep($"-------------------------------   AclaraViewMainMenu     , thread: { Thread.CurrentThread.ManagedThreadId}");
            Settings.IsConnectedBLE = false;
            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            menuOptions = this.MenuOptions;
            dialogView = this.DialogView;

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
                CrossSettings.Current.AddOrUpdateValue("session_username", FormsApp.credentialsService.UserName);
            }

            if (Device.Idiom == TargetIdiom.Phone)
            {
                background_scan_page.Opacity = 0;
                background_scan_page.FadeTo(1, 250);
            }    

            // Upload log files and then start pucks detection
            UploadFilesAndCheckCertificate ();
        }

        public async Task UploadFilesAndCheckCertificate()
        {
            if (Mobile.configData.IsCertLoaded)
            {
               if (Mobile.configData.certificate.NotAfter.AddMonths(-2) <= DateTime.Today)
                {
                   await DisplayAlert("Alert", $"The installed certificate will expire on: {Mobile.configData.certificate.NotAfter.ToShortDateString()}", "OK");
                }
            }
            // Upload log files
            if (Singleton.Get.Configuration.Global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();
            
            // Init pucks detection
            InitRefreshCommand();
            Interface_background_scan_page();
           
        }

        private void InitRefreshCommand()
        {
            //Asign the action to command refresh_command
            refresh_command = new Command(async () =>
            {
                Utils.PrintDeep($"----------------------REFRESH command dispositivos encontrados : {FormsApp.ble_interface.GetBlePeripheralList().Count}");
                Utils.PrintDeep($"-------------------------------        REFRESH command, thread: { Thread.CurrentThread.ManagedThreadId}");

                if (!GetAutoConnectStatus())
                {
                    Esperando();

                    if (FormsApp.ble_interface.IsOpen()) FormsApp.ble_interface.Close();

                    Utils.PrintDeep("----------------------------------------------  init Ble_iterface");
                    if (connectionThread.ThreadState == ThreadState.WaitSleepJoin)
                    {
                       Utils.PrintDeep($"____________________________________________________________________Thread '{Thread.CurrentThread.Name}' waiting.");
                       connectionThread.Interrupt(); 
                    }
                    
                    listPucks = new ObservableCollection<DeviceItem>();

                    //start scan
                    FormsApp.ble_interface.SetTimeOutSeconds(TimeOutSeconds);
                    await FormsApp.ble_interface.Scan();

                    TimeOutSeconds = 3; //next scan timeout 3 sec

                    if (FormsApp.ble_interface.GetBlePeripheralList().Count > 0)
                    {
                        ChangeListViewData();

                        if (listPucks.Count != 0)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                DeviceList.ItemsSource = listPucks;
                            });

                            if (conectarDevice)
                            {
                                PairWithKnowDevice();
                            }
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DeviceList.ItemsSource = null;
                            Application.Current.MainPage.DisplayAlert("Alert", "No device found, please, press the button to turn on the device and refresh", "Ok");
                            Terminado();
                        });
                    }
                    
                }
            });
        }

        private void PairWithKnowDevice()
        {
            autoConnect = false;
            conectarDevice = false;
        
            NewOpenConnectionWithDevice();
  
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
            Utils.PrintDeep($"-------------------------------    Interface_background_scan_page, thread: { Thread.CurrentThread.ManagedThreadId}");

            // this task controls the connect and disconnect for the devices
            connectionThread = new Thread(new ThreadStart(ConnectionMethod))
            {
                Name = "ConnectionThread"
            };
            connectionThread.Start();

            // asign the command to the list
            DeviceList.RefreshCommand = refresh_command;

            //start first scan
            FirstRefreshSearchPucs();
               
        }

        public void FirstRefreshSearchPucs()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                refresh_command.Execute(true);
            });
        }


        private void OnSwiped(object sender, SwipedEventArgs e)
        {
            if (Device.Idiom == TargetIdiom.Tablet)
                return;

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    SideMenuClose(sender, e);

                    break;
                case SwipeDirection.Right:
                    SideMenuOpen(sender, e);
    
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
            menuOptions.GetListElement("navigationDrawerList").IsEnabled = true;
            menuOptions.GetListElement("navigationDrawerList").Opacity = 0.65;

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
 
            hamburger_icon.IsVisible = true;
            hamburger_icon_detail.IsVisible = true;
            aclara_detail_logo.IsVisible = true;
            aclara_logo.IsVisible = true;

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
          
            hamburger_icon.IsVisible = false;
            hamburger_icon_detail.IsVisible = false;
            background_scan_page.Margin = new Thickness(310, 0, 0, 0);
            background_scan_page_detail.Margin = new Thickness(310, 0, 0, 0);
            aclara_logo.IsVisible = true;
           
            aclara_detail_logo.IsVisible = true;
 
            shadoweffect.IsVisible = true;
            aclara_logo.Scale = 1.2;
            aclara_detail_logo.Scale = 1.2;
       
            shadoweffect.Source = "shadow_effect_tablet";

        }

        private void TappedListeners()
        {

            dialogView.GetTGRElement("turnoffmtu_ok").Tapped += TurnOffMTUOkTapped;
            dialogView.GetTGRElement("turnoffmtu_no").Tapped += TurnOffMTUNoTapped;
            dialogView.GetTGRElement("turnoffmtu_ok_close").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("replacemeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("replacemeter_cancel").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("meter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("meter_cancel").Tapped += dialog_cancelTapped;
                 

            menuOptions.GetListElement("navigationDrawerList").ItemTapped += OnMenuItemSelected;


            dialogView.GetTGRElement("logoff_no").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("logoff_ok").Tapped += LogOffOkTapped;

            dialogView.GetTGRElement("dialog_AddMTUAddMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTUAddMeter_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_AddMTUReplaceMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTUReplaceMeter_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_ReplaceMTUReplaceMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_ReplaceMTUReplaceMeter_cancel").Tapped += dialog_cancelTapped;


            dialogView.GetTGRElement("dialog_AddMTU_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTU_cancel").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("dialog_NoAction_ok").Tapped += dialog_cancelTapped;

            disconnectDevice.Tapped += BluetoothPeripheralDisconnect;
            back_button.Tapped += SideMenuOpen;
            menuOptions.GetTGRElement("back_button_menu").Tapped += SideMenuClose;
            menuOptions.GetTGRElement("logout_button").Tapped += LogoutTapped;
            back_button_detail.Tapped += SideMenuOpen;
            menuOptions.GetTGRElement("settings_button").Tapped += OpenSettingsTapped;

   
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                hamburger_icon_home.IsVisible = true;
                hamburger_icon_home_detail.IsVisible = true;

                hamburger_icon_home.Opacity = 0;
                hamburger_icon_home_detail.Opacity = 0;
            }

            refresh_signal.Tapped += refreshBleData;



        }
        void dialog_cancelTapped(object sender, EventArgs e)
        {
            Label obj = (Label)sender;
            StackLayout parent = (StackLayout)obj.Parent;
            StackLayout dialog = (StackLayout)parent.Parent;
            dialog.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            backdark_bg.IsVisible = false;
            indicator.IsVisible = false;
            background_scan_page.IsEnabled = true;
  
        }
        private void dialog_OKBasicTapped(object sender, EventArgs e)
        {
            Label obj = (Label)sender;
            StackLayout parent = (StackLayout)obj.Parent;
            StackLayout dialog = (StackLayout)parent.Parent;
            dialog.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
 
            this.GoToPage ();
        }

        private void GoToPage ()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (actionType == ActionType.DataRead)
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewDataRead(dialogsSaved,  this.actionType), false);
                else if(actionType == ActionType.RemoteDisconnect)
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewRemoteDisconnect(dialogsSaved,  this.actionType), false);
                else
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved,  this.actionType), false);
            });         
        }

        private void refreshBleData(object sender, EventArgs e)
        {
            DeviceList.RefreshCommand.Execute(true);
        }
                

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

        private void ConnectionMethod()
        {
 
            int timeout_connecting = 0;
            int cont = 0;
            int refresh = 0;

     
            while (true)
            {
                try
                { 

                    if (Settings.IsLoggedIn == false)
                        break;

                    int status = FormsApp.ble_interface.GetConnectionStatus();

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
                        if (peripheralConnected == ble_library.BlePort.NO_CONNECTED)
                        {
                            peripheralConnected = status;
                            timeout_connecting = 0;
                        }
                        else if (peripheralConnected == ble_library.BlePort.CONNECTING)
                        {

                            if (status == ble_library.BlePort.NO_CONNECTED)
                            {
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
                                    switch (FormsApp.ble_interface.GetConnectionError())
                                    {
                                        case ble_library.BlePort.NO_ERROR:

                                            Application.Current.MainPage.DisplayAlert("Alert", "Please, press again the button to change PAIRING mode", "Ok");
                                            break;
                                        case ble_library.BlePort.CONECTION_ERRROR:

                                            Application.Current.MainPage.DisplayAlert("Alert", "Connection error. Please, retry", "Ok");
                                            break;
                                        case ble_library.BlePort.DYNAMIC_KEY_ERROR:
                                        case ble_library.BlePort.NO_DYNAMIC_KEY_ERROR:

                                            Application.Current.MainPage.DisplayAlert("Alert", "Please, press the button to change PAIRING mode", "Ok");
                                            break;
                                    }
                                    DeviceList.IsEnabled = true;

                                    fondo.Opacity = 1;
                                    background_scan_page.Opacity = 1;
                                    background_scan_page.IsEnabled = true;

                                });
                                peripheralConnected = status;
                                Singleton.Remove<Puck>();

                                bAlertBatt = true;
                                bAlertBatt10 = true;
                            }
                            else // status == ble_library.BlePort.CONNECTED
                            {
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
                            DeviceList.IsEnabled = true;

                            peripheralConnected = status;
                            Singleton.Remove<Puck>();
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
                    
                    if (peripheralConnected == ble_library.BlePort.CONNECTING)
                    {
                        timeout_connecting++;
                        
                        if (timeout_connecting >= 2 * 12) // 10 seconds
                        {                           
                            Device.BeginInvokeOnMainThread(() =>
                            {
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

                                peripheralConnected = ble_library.BlePort.NO_CONNECTED;
                                timeout_connecting = 0;

                                FormsApp.ble_interface.Close();
                      
                            });
                           
                            Thread.Sleep(Timeout.Infinite);
                        }
                    }
                    else
                    {
                        //No es connecting
                    }

                    Thread.Sleep(300); // 0.5 Second
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine($"____________________________________________________________________Thread '{Thread.CurrentThread.Name}' awoken.");
                }

            }
           
        }

        private void IsConnectedUIChange(bool puckConnected)
        {

            if (puckConnected)
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
                menuOptions.GetListElement("navigationDrawerList").IsEnabled = true;
                menuOptions.GetListElement("navigationDrawerList").Opacity = 1;

                #region Disable Circular Progress bar Animations when done

                backdark_bg.IsVisible = false;
                indicator.IsVisible = false;

                #endregion
            }
            else
            {
                background_scan_page_detail.IsVisible = false;
                menuOptions.GetListElement("navigationDrawerList").Opacity = 0.65;
                menuOptions.GetListElement("navigationDrawerList").IsEnabled = true;
                background_scan_page.IsVisible = true;
                refresh_command.Execute(true);
                Navigation.PopToRootAsync();


            }
        }

       
        private  void ChangeListViewData()
        {
 
            try
            {
 
                List<IBlePeripheral> blePeripherals;
                blePeripherals = FormsApp.ble_interface.GetBlePeripheralList();

                // YOU CAN RETURN THE PASS BY GETTING THE STRING AND CONVERTING IT TO BYTE ARRAY TO AUTO-PAIR
                byte[] bytesDev = System.Convert.FromBase64String(CrossSettings.Current.GetValueOrDefault("session_peripheral_DeviceId", string.Empty));

                byte[] byte_now;

                int sizeList = blePeripherals.Count;

                for (int i = 0; i < sizeList; i++)
                {
                    try
                    {
                        if (blePeripherals[i] != null)
                        {
                            Puck puck = new Puck ( blePeripherals[ i ] );

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
                                string iconBattery = puck.BatteryLevelIcon;                                 
                                string iconRSSI = puck.RSSIIcon;

                                DeviceItem device = new DeviceItem
                                {
                                    deviceMacAddress  = puck.SerialNumber,
                                    deviceName        = puck.Name,
                                    deviceBattery     = puck.BatteryLevel + "%",
                                    deviceRssi        = puck.RSSI + " dBm",
                                    deviceBatteryIcon = iconBattery,
                                    deviceRssiIcon    = iconRSSI,
                                    Peripheral        = puck.Device
                                };

                                listPucks.Add(device);

                                //VERIFY IF PREVIOUSLY BOUNDED DEVICES WITH THE RIGHT USERNAME
                                if (CrossSettings.Current.GetValueOrDefault("session_dynamicpass", string.Empty) != string.Empty &&
                                    FormsApp.credentialsService.UserName.Equals(CrossSettings.Current.GetValueOrDefault("session_username", string.Empty)) &&
                                    bytesDev.Take(4).ToArray().SequenceEqual(byte_now) &&
                                    puck.Name.Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral", string.Empty)) &&
                                    ! peripheralManualDisconnection &&
                                    ! Singleton.Has<Puck>() )
                                {
                                    if (!FormsApp.ble_interface.IsOpen())
                                    {
                                        try
                                        {
                                            Singleton.Set = new Puck ( blePeripherals[ i ], FormsApp.ble_interface );
                                                    
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
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        Utils.Print(er.StackTrace); 
                    }
                }
                if (!autoConnect)
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
            catch (Exception e)
            {
                Utils.Print(e);
            }

        }


        #region We want to connect to the device if there is not scanning running

        private void NewOpenConnectionWithDevice()
        {
            while (FormsApp.ble_interface.IsScanning())
            {
                 Thread.Sleep(100);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                var seconds = TimeSpan.FromSeconds(1); // Don't execute it asap!

                Device.StartTimer(seconds, () =>
                {
 
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        if (!FormsApp.ble_interface.IsOpen())
                        {
                            while (FormsApp.ble_interface.IsScanning())
                            {
                                Thread.Sleep(100);
                            }

                            // call your method to check for notifications here
                            FormsApp.ble_interface.Open ( Singleton.Get.Puck.Device, true );
                        }

                    });

                    return false;
                });
            });
        }

        #endregion

        private async void LogOffOkTapped(object sender, EventArgs e)
        {
            // Upload log files
            if (Singleton.Get.Configuration.Global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();

            dialogView.OpenCloseDialog("dialog_logoff", false);
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            connectionThread.Abort();

            FormsApp.DoLogOff();

            background_scan_page.IsEnabled = true;
            background_scan_page_detail.IsEnabled = true;
            
            Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogsSaved));
         
        }

        private void TurnOffMTUCloseTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void TurnOffMTUNoTapped(object sender, EventArgs e)
        {
            backdark_bg.IsVisible = false;
            indicator.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void TurnOffMTUOkTapped(object sender, EventArgs e)
        {
            CallLoadViewTurnOff();
        }

        private async Task TurnOffMethod()
        {

            MTUComm.Action turnOffAction = new MTUComm.Action (
                FormsApp.ble_interface,
                MTUComm.Action.ActionType.TurnOffMtu,
                FormsApp.credentialsService.UserName );

            turnOffAction.OnFinish -= TurnOff_OnFinish;
            turnOffAction.OnFinish += TurnOff_OnFinish;

            turnOffAction.OnError  -= TurnOff_OnError;
            turnOffAction.OnError  += TurnOff_OnError;

            await turnOffAction.Run();
        }

        public async Task TurnOff_OnFinish ( object sender, Delegates.ActionFinishArgs args )
        {
             await Task.Delay(2000).ContinueWith(t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label textResult = (Label)dialogView.FindByName("dialog_turnoff_text");
                    textResult.Text = "MTU turned off Successfully";

                    dialogView.OpenCloseDialog("dialog_turnoff_two", false);
                    dialogView.OpenCloseDialog("dialog_turnoff_three", true);
                }));
        }

        public void TurnOff_OnError ()
        {
            Task.Delay(2000).ContinueWith(t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label textResult = (Label)dialogView.FindByName("dialog_turnoff_text");
                    textResult.Text = "MTU turned off Unsuccessfully";

                    dialogView.OpenCloseDialog("dialog_turnoff_two", false);
                    dialogView.OpenCloseDialog("dialog_turnoff_three", true);
                }));
        }


        private void BluetoothPeripheralDisconnect(object sender, EventArgs e)
        {

            FormsApp.ble_interface.Close();
            peripheralManualDisconnection = true;

            CrossSettings.Current.AddOrUpdateValue("session_dynamicpass", string.Empty);

        }

        private void LogoutTapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                dialogView.CloseDialogs();
                dialogView.OpenCloseDialog("dialog_logoff", true);

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

            try
            {
                Singleton.Set = new Puck ( item.Peripheral, FormsApp.ble_interface );

                bAlertBatt   = true;
                bAlertBatt10 = true;
                externalReconnect(reassociate);

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
                menuOptions.GetListElement("navigationDrawerList").SelectedItem = null;
                try
                {
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

        private async Task NavigationController (
            ActionType actionTarget )
        {
            #region New Circular Progress bar Animations    

            DeviceList.IsRefreshing = false;
            backdark_bg.IsVisible = true;
            indicator.IsVisible = true;

            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;

            background_scan_page.IsEnabled = true;
            background_scan_page_detail.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

            #endregion
            
            switch ( await base.ValidateNavigation ( actionTarget ) )
            {
                case ValidationResult.EXCEPTION:
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                    return;
                case ValidationResult.FAIL:
                    dialog_open_bg.IsVisible = true;
                    turnoff_mtu_background.IsVisible = true;
                    dialogView.CloseDialogs();
                    dialogView.OpenCloseDialog("dialog_NoAction", true);
                    return;
            }

            switch (actionTarget)
            {
                    case ActionType.DataRead:
                    case ActionType.RemoteDisconnect:
                    #region DataRead  
                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
  
                            this.GoToPage ();
                        })
                    );

                    #endregion

                    break;
                case ActionType.ReadFabric:
                    #region ReadFabric
                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
 
                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved,actionTarget), false);

                        })
                    );

                    #endregion

                    break;
                case ActionType.ReadMtu:

                    #region Read Mtu Controller

                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved, actionTarget), false);


                        })
                    );

                    #endregion

                    break;

                case ActionType.AddMtu:

                    #region Add Mtu Controller

                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            
                            dialogView.CloseDialogs();

                            #region Check ActionVerify

                            if (Singleton.Get.Configuration.Global.ActionVerify)
                            {
                                dialog_open_bg.IsVisible = true;
                                turnoff_mtu_background.IsVisible = true;
                                dialogView.GetStackLayoutElement("dialog_AddMTU").IsVisible = true;
                            }
                            else
                                GoToPage();

                            #endregion

                        })
                    );

                    #endregion

                    break;

                case ActionType.TurnOffMtu:

                    #region Turn Off Controller

                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                         
                            dialogView.CloseDialogs();

                            #region Check ActionVerify

                            if (Singleton.Get.Configuration.Global.ActionVerify)
                            {
                                dialog_open_bg.IsVisible = true;
                                turnoff_mtu_background.IsVisible = true;
                                dialogView.OpenCloseDialog("dialog_turnoff_one", true);
                            }
                            else
                                CallLoadViewTurnOff();

                            #endregion
                        

                        })
                    );

                    #endregion

                    break;

                case ActionType.MtuInstallationConfirmation:

                    #region Install Confirm Controller

                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewInstallConfirmation(dialogsSaved), false);

                        })
                    );

                    #endregion

                    break;

                case ActionType.ReplaceMTU:

                    #region Replace Mtu Controller
                    await ControllerAction(actionTarget, "dialog_replacemeter_one");
 

                    #endregion

                    break;

                case ActionType.ReplaceMeter:

                    #region Replace Meter Controller
                    await ControllerAction(actionTarget, "dialog_meter_replace_one");
                    

                    #endregion

                    break;

                case ActionType.AddMtuAddMeter:

                    #region Add Mtu | Add Meter Controller
                    await ControllerAction(actionTarget, "dialog_AddMTUAddMeter");
                    

                    #endregion

                    break;

                case ActionType.AddMtuReplaceMeter:

                    #region Add Mtu | Replace Meter Controller
                    await ControllerAction(actionTarget, "dialog_AddMTUReplaceMeter");
                    

                    #endregion

                    break;

                case ActionType.ReplaceMtuReplaceMeter:

                    #region Replace Mtu | Replace Meter Controller
                    await ControllerAction(actionTarget, "dialog_ReplaceMTUReplaceMeter");
                    

                    #endregion

                    break;

            }
        }

        private async Task ControllerAction(ActionType page, string nameDialog)
        {

            await Task.Delay(200).ContinueWith(t =>

                Device.BeginInvokeOnMainThread(() =>
                {

                    dialogView.CloseDialogs();

                    #region Check ActionVerify
                    if (Singleton.Get.Configuration.Global.ActionVerify)
                    {
                        dialog_open_bg.IsVisible = true;
                        turnoff_mtu_background.IsVisible = true;
                        dialogView.OpenCloseDialog(nameDialog, true);
                    }
                    else
                    {
                        this.actionType = page;
                        GoToPage();
                    }
                    #endregion


                })
            );
        }

        private void CallLoadViewTurnOff()
        {
            dialogView.OpenCloseDialog("dialog_turnoff_one", false);
            dialogView.OpenCloseDialog("dialog_turnoff_two", true);

            Task.Factory.StartNew(TurnOffMethod);
        }

        private void OpenSettingsTapped(object sender, EventArgs e)
        {
            
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
     
                        return;
                    }
                    else
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new AclaraViewSettings(true), false);

                        return;
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
        
        }
            
    }
}
