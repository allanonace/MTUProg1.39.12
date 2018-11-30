// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using aclara_meters.Helpers;
using aclara_meters.Models;
using Xamarin.Forms;
using System.Threading;
using nexus.protocols.ble.scan;
using System.Collections.ObjectModel;
using Plugin.Settings;
using System.Linq;
using MTUComm;

namespace aclara_meters.view
{
    public partial class AclaraViewMainMenu
    {
        private List<PageItem> MenuList { get; set; }
        private IUserDialogs dialogsSaved;
        private ObservableCollection<DeviceItem> employees;
        private IBlePeripheral peripheral = null;
        private int peripheralConnected = ble_library.BlePort.NO_CONNECTED;
        private Boolean peripheralManualDisconnection = false;
        private Thread printer;

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public AclaraViewMainMenu()
        {
           InitializeComponent();
        }
      
        public AclaraViewMainMenu(IUserDialogs dialogs )
        {
            InitializeComponent();
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
            if (FormsApp.CredentialsService.UserName != null)
            {
                userName.Text = FormsApp.CredentialsService.UserName; //"Kartik";
                CrossSettings.Current.AddOrUpdateValue("session_username", FormsApp.CredentialsService.UserName);           
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

            printer = new Thread(new ThreadStart(InvokeMethod));
            printer.Start();

            employees = new ObservableCollection<DeviceItem>();

            DeviceList.RefreshCommand = new Command(async () =>
            {
                // Hace un resume si se ha hecho un suspend (al pasar a config o logout)
                // Problema: solo se hace si se refresca DeviceList
                // TO-DO: eliminar el hilo o eliminar el suspend
                if (printer.ThreadState == ThreadState.Suspended)
                {
                    try
                    {
                        printer.Resume();
                    }
                    catch (Exception e11)
                    {
                        Console.WriteLine(e11.StackTrace);
                    }
                } 
                DeviceList.IsRefreshing = true;

                employees = new ObservableCollection<DeviceItem>();

                await FormsApp.ble_interface.Scan();
                await ChangeListViewData();
                DeviceList.IsRefreshing = false;

                if (employees.Count != 0)
                {
                    DeviceList.ItemsSource = employees;
                }
            });
            DeviceList.RefreshCommand.Execute(true);
            if(employees.Count != 0)
            {
                DeviceList.ItemsSource = employees;
            }
           
        }

        private void LoadSideMenuElements()
        {

            MenuList = new List<PageItem>
            {
                // Creating our pages for menu navigation
                // Here you can define title for item, 
                // icon on the left side, and page that you want to open after selection

                // Adding menu items to MenuList
                new PageItem()
                {
                    Title = "Read MTU",
                    Icon = "readmtu_icon.png",
                    TargetType = "ReadMTU"
                },

                new PageItem()
                {
                    Title = "Turn Off MTU",
                    Icon = "turnoff_icon.png",
                    TargetType = "turnOff"
                },

                new PageItem()
                {
                    Title = "Add MTU",
                    Icon = "addMTU.png",
                    TargetType = "AddMTU"
                },

                new PageItem()
                {
                    Title = "Replace MTU",
                    Icon = "replaceMTU2.png",
                    TargetType = "replaceMTU"
                },

                new PageItem()
                {
                    Title = "Replace Meter",
                    Icon = "replaceMeter.png",
                    TargetType = "replaceMeter"
                },

                new PageItem()
                {
                    Title = "Add MTU / Add meter",
                    Icon = "addMTUaddmeter.png",
                    TargetType = "AddMTUAddMeter"
                },

                new PageItem()
                {
                    Title = "Add MTU / Rep. Meter",
                    Icon = "addMTUrepmeter.png",
                    TargetType = "AddMTUReplaceMeter"
                },

                new PageItem()
                {
                    Title = "Rep.MTU / Rep. Meter",
                    Icon = "repMTUrepmeter.png",
                    TargetType = "ReplaceMTUReplaceMeter"
                },

                new PageItem()
                {
                    Title = "Install Confirmation",
                    Icon = "installConfirm.png",
                    TargetType = "InstallConfirm"
                }
            };

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = MenuList;
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
        }

        private void TappedListeners()
        {
            turnoffmtu_ok.Tapped += TurnOffMTUOkTapped;
            turnoffmtu_no.Tapped += TurnOffMTUNoTapped;
            turnoffmtu_ok_close.Tapped += TurnOffMTUCloseTapped;
            replacemeter_ok.Tapped += ReplaceMeterOkTapped;
            replacemeter_cancel.Tapped += ReplaceMeterCancelTapped;
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


        private void InvokeMethod()
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // printer = new Thread(new ThreadStart(InvokeMethod));
        // printer.Start();
        {
            int timeout_connecting = 0;
            while (true)
            {
                      
                //if (!FormsApp.ble_interface.GetPairingStatusOk())
                int status = FormsApp.ble_interface.GetConnectionStatus();

                if (status != peripheralConnected)
                {
                    if (peripheralConnected == ble_library.BlePort.NO_CONNECTED)
                    {
                        // status DEBERIA SER SIEMPRE ble_library.BlePort.CONNECTING
                        peripheralConnected = status;
                        timeout_connecting = 0;
                    }
                    else if (peripheralConnected == ble_library.BlePort.CONNECTING)
                    {
                        if (status == ble_library.BlePort.NO_CONNECTED)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                switch (FormsApp.ble_interface.GetConnectionError())
                                {
                                    case ble_library.BlePort.NO_ERROR:
                                        break;
                                    case ble_library.BlePort.CONECTION_ERRROR:
                                        Application.Current.MainPage.DisplayAlert("Alert", "Connection error. Please, retry", "Ok");
                                        break;
                                    case ble_library.BlePort.DYNAMIC_KEY_ERROR:
                                        Application.Current.MainPage.DisplayAlert("Alert", "Please, press the button to change PAIRING mode", "Ok");
                                        break;
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
                            peripheral = null;
                        }
                        else // status == ble_library.BlePort.CONNECTED
                        {
                            DeviceList.IsEnabled = true;
                            // status DEBERIA SER SIEMPRE ble_library.BlePort.CONNECTED
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
                        // status DEBERIA SER SIEMPRE ble_library.BlePort.NO_CONNECTED
                        peripheralConnected = status;
                        peripheral = null;
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            fondo.Opacity = 1;
                            background_scan_page.Opacity = 1;
                            background_scan_page.IsEnabled = true;

                            IsConnectedUIChange(false);
                        });

                    }

                }
                if (peripheralConnected == ble_library.BlePort.CONNECTING)
                {
                    
                    timeout_connecting++;
                    if (timeout_connecting >= 2*10) // 10 seconds
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage.DisplayAlert("Timeout", "Connection Timeout", "Ok");
                            DeviceList.IsEnabled = true;
                            fondo.Opacity = 1;
                            background_scan_page.Opacity = 1;
                            background_scan_page.IsEnabled = true;

                        });
                        peripheralConnected = ble_library.BlePort.NO_CONNECTED;
                        timeout_connecting = 0;
                        FormsApp.ble_interface.Close();
                    }
                }

                Thread.Sleep(500); // 0.5 Second
            }
        }

        private void IsConnectedUIChange(bool v)
        {
            if(v)
            {
                try
                {
                    // TODO: la siguente linea siempre da error xq peripheral es null
                    deviceID.Text = peripheral.Advertisement.DeviceName;
                    macAddress.Text = DecodeId(peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray());
                   
                    //imageBattery.Source = "battery_toolbar_high";
                    // imageRssi.Source = "rssi_toolbar_high";
                    // batteryLevel.Text = "100%";
                    // rssiLevel.Text = peripheral.Rssi.ToString() + " dBm";

                    byte[] battery_ui = peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Skip(4).Take(1).ToArray();

                    if (battery_ui[0] < 101 && battery_ui[0] > 1)
                    {
                        batteryLevel.Text = battery_ui[0].ToString() + " %";

                        if (battery_ui[0] >= 75)
                        {
                            imageBattery.Source = "battery_toolbar_high";
                            battery_level.Source = "battery_toolbar_high_white";
                            battery_level_detail.Source = "battery_toolbar_high_white";
                        }
                        else if (battery_ui[0] >= 45 && battery_ui[0] < 75)
                        {
                            imageBattery.Source = "battery_toolbar_mid";
                            battery_level.Source = "battery_toolbar_mid_white";
                            battery_level_detail.Source = "battery_toolbar_mid_white";
                        }
                        else if (battery_ui[0] >= 15 && battery_ui[0] < 45)
                        {
                            imageBattery.Source = "battery_toolbar_low";
                            battery_level.Source = "battery_toolbar_low_white";
                            battery_level_detail.Source = "battery_toolbar_low_white";
                        }
                        else // battery_ui[0] < 15
                        {
                            imageBattery.Source = "battery_toolbar_empty";
                            battery_level.Source = "battery_toolbar_empty_white";
                            battery_level_detail.Source = "battery_toolbar_empty_white";
                        }
                    }

                    /*** RSSI ICONS UPDATE ***/
                    if (peripheral.Rssi <= -90)
                    {
                        imageRssi.Source = "rssi_toolbar_empty";
                        rssi_level.Source = "rssi_toolbar_empty_white";
                        rssi_level_detail.Source = "rssi_toolbar_empty_white";
                    }
                    else if (peripheral.Rssi <= -80 && peripheral.Rssi > -90)
                    {
                        imageRssi.Source = "rssi_toolbar_low";
                        rssi_level.Source = "rssi_toolbar_low_white";
                        rssi_level_detail.Source = "rssi_toolbar_low_white";
                    }
                    else if (peripheral.Rssi <= -60 && peripheral.Rssi > -80)
                    {
                        imageRssi.Source = "rssi_toolbar_mid";
                        rssi_level.Source = "rssi_toolbar_mid_white";
                        rssi_level_detail.Source = "rssi_toolbar_mid_white";
                    }
                    else // (peripheral.Rssi > -60) 
                    {
                        imageRssi.Source = "rssi_toolbar_high";
                        rssi_level.Source = "rssi_toolbar_high_white";
                        rssi_level_detail.Source = "rssi_toolbar_high_white";
                    }

                    //Save Battery & Rssi info for the next windows
                    CrossSettings.Current.AddOrUpdateValue("battery_icon_topbar", battery_level.Source.ToString().Substring(6));
                    CrossSettings.Current.AddOrUpdateValue("rssi_icon_topbar", rssi_level.Source.ToString().Substring(6));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                background_scan_page_detail.IsVisible = true;
                block_ble_disconnect.Opacity = 0;
                block_ble_disconnect.FadeTo(1, 250);
                block_ble.Opacity = 0;
                block_ble.FadeTo(1, 250);
                background_scan_page.IsVisible = false;
                navigationDrawerList.IsEnabled = true;
                navigationDrawerList.Opacity = 1;

            }else{
                background_scan_page_detail.IsVisible = false;
                navigationDrawerList.Opacity = 0.65;
                navigationDrawerList.IsEnabled = true;
                background_scan_page.IsVisible = true;
                DeviceList.RefreshCommand.Execute(true);
            }
        }

        private string DecodeId(byte[] id)
        {
            string s;
            try
            {
                s = System.Text.Encoding.ASCII.GetString(id.Take(2).ToArray());
                byte[] byte_aux = new byte[4];
                byte_aux[0] = id[3];
                byte_aux[1] = id[2];
                byte_aux[2] = 0;
                byte_aux[3] = 0;
                s += BitConverter.ToInt32(byte_aux, 0);
            }
            catch (Exception e)
            {
                s = BitConverter.ToString(id);
            }
            return s;
        }

        private async Task ChangeListViewData()
        {
            await Task.Factory.StartNew(() =>
            {
                // wait until scan finish
                while (FormsApp.ble_interface.IsScanning())
                {
                    try
                    {
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
                                if(blePeripherals[i] != null)
                                {
                                    byte_now = blePeripherals[i].Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray();
                                   
                                    bool enc = false;
                                    int sizeListTemp = employees.Count;

                                    for (int j = 0; j < sizeListTemp; j++)
                                    {
                                        if (employees[j].Peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray()
                                            .SequenceEqual(blePeripherals[i].Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray()))
                                        {
                                            enc = true;
                                        }
                                    }

                                    string icono_bateria;

                                    byte[] bateria;

                                    if (!enc)
                                    {
                                        bateria = blePeripherals[i].Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Skip(4).Take(1).ToArray();

                                        icono_bateria = "battery_toolbar_high";

                                        if (bateria[0] >= 75)
                                        {
                                            icono_bateria = "battery_toolbar_high";
                                        }
                                        else if (bateria[0] >= 45 && bateria[0] < 75)
                                        {
                                            icono_bateria = "battery_toolbar_mid";
                                        }
                                        else if (bateria[0] >= 15 && bateria[0] < 45)
                                        {
                                            icono_bateria = "battery_toolbar_low";
                                        }
                                        else // bateria[0] < 15
                                        {
                                            icono_bateria = "battery_toolbar_empty";
                                        }

                                        string rssiIcono = "rssi_toolbar_high";

                                        /*** RSSI ICONS UPDATE ***/

                                        if (blePeripherals[i].Rssi <= -90)
                                        {
                                            rssiIcono = "rssi_toolbar_empty";
                                        }
                                        else if (blePeripherals[i].Rssi <= -80 && blePeripherals[i].Rssi > -90)
                                        {
                                            rssiIcono = "rssi_toolbar_low";
                                        }
                                        else if (blePeripherals[i].Rssi <= -60 && blePeripherals[i].Rssi > -80)
                                        {
                                            rssiIcono = "rssi_toolbar_mid";
                                        }
                                        else // (blePeripherals[i].Rssi > -60) 
                                        {
                                            rssiIcono = "rssi_toolbar_high";
                                        }

                                        DeviceItem device = new DeviceItem
                                        {
                                            deviceMacAddress = DecodeId(byte_now),
                                            deviceName = blePeripherals[i].Advertisement.DeviceName,
                                            deviceBattery = bateria[0].ToString() + "%",
                                            deviceRssi = blePeripherals[i].Rssi.ToString() + " dBm",
                                            deviceBatteryIcon = icono_bateria,
                                            deviceRssiIcon = rssiIcono,
                                            Peripheral = blePeripherals[i]
                                        };

                                        employees.Add(device);

                                        //VERIFY IF PREVIOUSLY BOUNDED DEVICES WITH THE RIGHT USERNAME
                                        if (CrossSettings.Current.GetValueOrDefault("session_dynamicpass", string.Empty) != string.Empty &&
                                            FormsApp.CredentialsService.UserName.Equals(CrossSettings.Current.GetValueOrDefault("session_username", string.Empty)) &&
                                            bytes.Take(4).ToArray().SequenceEqual(byte_now) &&
                                            blePeripherals[i].Advertisement.DeviceName.Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral", string.Empty)) &&
                                            !peripheralManualDisconnection &&
                                            peripheral == null)
                                        {
                                            if (!FormsApp.ble_interface.IsOpen())
                                            {
                                                try
                                                {
                                                    peripheral = blePeripherals[i];
                                                    peripheralConnected = ble_library.BlePort.NO_CONNECTED;
                                                    peripheralManualDisconnection = false;
                                                    FormsApp.ble_interface.Open(peripheral, true);
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine(e.StackTrace);
                                                }
                                   
                                            }
                                        }
                                    }
                                }
                            }catch (Exception er){
                               
                                Console.WriteLine(er.StackTrace); //2018-09-21 13:08:25.918 aclara_meters.iOS[505:190980] System.NullReferenceException: Object reference not set to an instance of an object
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            });
        } 



        private void LogOffOkTapped(object sender, EventArgs e)
        {
            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            printer.Suspend();
            Settings.IsLoggedIn = false;
            FormsApp.CredentialsService.DeleteCredentials();
            FormsApp.ble_interface.Close();
            background_scan_page.IsEnabled = true;
            background_scan_page_detail.IsEnabled = true;
            Navigation.PopAsync();

        }

        private void LogOffNoTapped(object sender, EventArgs e)
        {
            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }


        private void ReplaceMeterCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void ReplaceMeterOkTapped(object sender, EventArgs e)
        {
            dialog_replacemeter_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;


            //Bug fix Android UI Animation
            Task.Delay(200).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMTU(dialogsSaved), false);             
            }));



                 
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

            MTUComm.Action turnOffAction = new MTUComm.Action(config: FormsApp.config, serial: FormsApp.ble_interface, actiontype: MTUComm.Action.ActionType.TurnOffMtu, user: FormsApp.CredentialsService.UserName);

            turnOffAction.OnFinish += ((s, args) =>
            {
                ActionResult actionResult = args.Result;

                Task.Delay(2000).ContinueWith(t =>
                   Device.BeginInvokeOnMainThread(() =>
                   {
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

            //Bug fix Android UI Animation
            Task.Delay(200).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMeter(dialogsSaved), false);
            }));
            
           
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

            //Bug fix Android UI Animation
            Task.Delay(200).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTUAddMeter(dialogsSaved), false);
            })
            );
            
           
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

            //Bug fix Android UI Animation
            Task.Delay(200).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTUReplaceMeter(dialogsSaved), false);
            })
            );


          
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


            //Bug fix Android UI Animation
            Task.Delay(200).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMTUReplaceMeter(dialogsSaved), false);
            })
            );
            

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

            Device.BeginInvokeOnMainThread(() =>
            {
                // TODO: cambiar usuario
                // TODO: BasicRead no loguea
                Task.Factory.StartNew(BasicReadThread);
            });

            //Bug fix Android UI Animation

        }

        void BasicReadThread()
        {
            MTUComm.Action basicRead = new MTUComm.Action(config: FormsApp.config, serial: FormsApp.ble_interface, actiontype: MTUComm.Action.ActionType.BasicRead, user: FormsApp.CredentialsService.UserName);
            basicRead.OnFinish += ((s, args) =>
            {
            });
            basicRead.Run();
            Task.Delay(200).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved), false);
            })
            ); 
        }

        private void BluetoothPeripheralDisconnect(object sender, EventArgs e)
        {
            FormsApp.ble_interface.Close();
        
            peripheralManualDisconnection = true;

            CrossSettings.Current.AddOrUpdateValue("session_dynamicpass", string.Empty);
            /*
            try
            {
                printer.Start();
            }
            catch (Exception t12)
            {
                Console.WriteLine(t12.StackTrace);
            }
            */
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

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelectedListDevices(object sender, ItemTappedEventArgs e)
        {
            var item = (DeviceItem)e.Item;
            fondo.Opacity = 0;
            background_scan_page.Opacity = 0.5;
            background_scan_page.IsEnabled = false;
            bool reassociate = false;
            if (CrossSettings.Current.GetValueOrDefault("session_dynamicpass", string.Empty) != string.Empty &&
                FormsApp.CredentialsService.UserName.Equals(CrossSettings.Current.GetValueOrDefault("session_username", string.Empty)) &&
                System.Convert.ToBase64String(item.Peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray()).Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral_DeviceId", string.Empty)) &&
                item.Peripheral.Advertisement.DeviceName.Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral", string.Empty)))
            {
                reassociate = true;
            }

            FormsApp.ble_interface.Open(item.Peripheral, reassociate);
            peripheral = item.Peripheral;

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
                    Console.WriteLine(e4.StackTrace);
                }
            });
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView

        private void OnMenuItemSelected(object sender, ItemTappedEventArgs e)
        {
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
                    var item = (PageItem)e.Item;
                    String page = item.TargetType;

                    ((ListView)sender).SelectedItem = null;

                    switch (page)
                    {
                        case "ReadMTU":
                            OnCaseReadMTU();
                            break;

                        case "AddMTU":
                            OnCaseAddMTU();
                            break;

                        case "turnOff":
                            OnCaseTurnOff();
                            break;

                        case "replaceMTU":
                            OnCaseReplaceMTU();
                            break;

                        case "replaceMeter":
                            OnCaseReplaceMeter();
                            break;

                        case "AddMTUAddMeter":
                            OnCaseAddMTUAddMeter();
                            break;

                        case "AddMTUReplaceMeter":
                            OnCaseAddMTUReplaceMeter();
                            break;


                        case "ReplaceMTUReplaceMeter":
                            OnCaseReplaceMTUReplaceMeter();
                            break;

						case "InstallConfirm":
							OnCaseInstallConfirm();
                            break;
                     
                    }
                }
                catch (Exception w1)
                {
                    Console.WriteLine(w1.StackTrace);
                }
            }else{
                Application.Current.MainPage.DisplayAlert("Alert", "Connect to a device and retry", "Ok");
            }
        }

        private void OnCaseInstallConfirm()
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
                shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
            }));

        }

        private void OnCaseReplaceMTUReplaceMeter()
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
                dialog_open_bg.IsVisible = true;
                turnoff_mtu_background.IsVisible = true;
                dialog_turnoff_one.IsVisible = false;
                dialog_turnoff_two.IsVisible = false;
                dialog_turnoff_three.IsVisible = false;
                dialog_replacemeter_one.IsVisible = false;
                dialog_meter_replace_one.IsVisible = false;

                dialog_AddMTUAddMeter.IsVisible = false;
                dialog_AddMTUReplaceMeter.IsVisible = false;
                dialog_ReplaceMTUReplaceMeter.IsVisible = true;

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
            }));
        }

        private void OnCaseAddMTUReplaceMeter()
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
                dialog_open_bg.IsVisible = true;
                turnoff_mtu_background.IsVisible = true;
                dialog_turnoff_one.IsVisible = false;
                dialog_turnoff_two.IsVisible = false;
                dialog_turnoff_three.IsVisible = false;
                dialog_replacemeter_one.IsVisible = false;
                dialog_meter_replace_one.IsVisible = false;

                dialog_AddMTUAddMeter.IsVisible = false;
                dialog_AddMTUReplaceMeter.IsVisible = true;

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
            }));
        }

        private void OnCaseAddMTUAddMeter()
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
                dialog_open_bg.IsVisible = true;
                turnoff_mtu_background.IsVisible = true;
                dialog_turnoff_one.IsVisible = false;
                dialog_turnoff_two.IsVisible = false;
                dialog_turnoff_three.IsVisible = false;
                dialog_replacemeter_one.IsVisible = false;
                dialog_meter_replace_one.IsVisible = false;

                dialog_AddMTUAddMeter.IsVisible = true;

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
            }));
        }

        private void OnCaseReplaceMeter()
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
                dialog_open_bg.IsVisible = true;
                turnoff_mtu_background.IsVisible = true;
                dialog_turnoff_one.IsVisible = false;
                dialog_turnoff_two.IsVisible = false;
                dialog_turnoff_three.IsVisible = false;
                dialog_replacemeter_one.IsVisible = false;
                dialog_meter_replace_one.IsVisible = true;
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
            }));
         
        }

        private void OnCaseReplaceMTU()
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
               dialog_open_bg.IsVisible = true;
               turnoff_mtu_background.IsVisible = true;
               dialog_meter_replace_one.IsVisible = false;
               dialog_turnoff_one.IsVisible = false;
               dialog_turnoff_two.IsVisible = false;
               dialog_turnoff_three.IsVisible = false;
               dialog_replacemeter_one.IsVisible = true;
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
            }));


         
        }

        private void OnCaseTurnOff()
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
               dialog_open_bg.IsVisible = true;
               turnoff_mtu_background.IsVisible = true;
               dialog_meter_replace_one.IsVisible = false;
               dialog_turnoff_one.IsVisible = true;
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

               shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; //if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
            }));
        }

        private void OnCaseAddMTU()
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

                dialog_AddMTU.IsVisible = true;

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
            }));
        }

        private void OnCaseReadMTU()
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
                navigationDrawerList.SelectedItem = null;
                Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved), false);
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
            }));
        }
       
        private void OpenSettingsTapped(object sender, EventArgs e)
        {

            printer.Suspend();
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
                    }
                }catch(Exception i2){
                    Console.WriteLine(i2.StackTrace);
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
    }
}
