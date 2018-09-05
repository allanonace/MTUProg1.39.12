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
using System.Text;
using System.ComponentModel;

namespace aclara_meters.view
{
    public partial class AclaraViewMainMenu
    {
		private List<PageItem> MenuList { get; set; }
        private IUserDialogs dialogsSaved;
		private List<ReadMTUItem> MenuListReadMTU { get; set; }
        private Boolean changedStatus;
        private List<IBlePeripheral> blePeripherals;
        private List<IBlePeripheral> temporal_ble_peripherals;
        private ObservableCollection<DeviceItem> employees;
        private Boolean disconnectedButton = false;
        private IBlePeripheral peripheral;
        private byte[] btdata; 

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
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(LoadTabletUI);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(LoadPhoneUI);
                });
            }
          
            dialogsSaved = dialogs;
            LoadPostUIGFX();

            //Change username textview to Prefs. String
            if (FormsApp.CredentialsService.UserName != null)
            {
                userName.Text = FormsApp.CredentialsService.UserName;
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

            Thread printer = new Thread(new ThreadStart(InvokeMethod));
            printer.Start();

            employees = new ObservableCollection<DeviceItem>();
            temporal_ble_peripherals = new List<IBlePeripheral> { };
                
            DeviceList.RefreshCommand = new Command(() =>
            {
                try
                {
                    employees.Clear();
                    FormsApp.ble_interface.Scan();
                    DeviceList.IsRefreshing = true;
                    temporal_ble_peripherals.Clear();
                    try
                    {
                        ChangeListViewData();
                    }
                    catch (Exception c1)
                    {
                        Console.WriteLine(c1.StackTrace);
                    }
                }catch (Exception c2){
                    Console.WriteLine(c2.StackTrace);
                }
                DeviceList.IsRefreshing = false;
            });
            DeviceList.RefreshCommand.Execute(true);








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
                    TargetType = ""
                },

                new PageItem()
                {
                    Title = "Add MTU / Rep. Meter",
                    Icon = "addMTUrepmeter.png",
                    TargetType = ""
                },

                new PageItem()
                {
                    Title = "Rep.MTU / Rep. Meter",
                    Icon = "repMTUrepmeter.png",
                    TargetType = ""
                },

                new PageItem()
                {
                    Title = "Install Confirmation",
                    Icon = "installConfirm.png",
                    TargetType = ""
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
            ContentNav.IsVisible = false;
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
            disconnectDevice.Tapped += BluetoothPeripheralDisconnect;
            back_button.Tapped += SideMenuOpen;
            back_button_menu.Tapped += SideMenuClose;
            logout_button.Tapped += LogoutTapped;
            back_button_detail.Tapped += SideMenuOpen;
            settings_button.Tapped += OpenSettingsTapped;
        }

        private void InvokeMethod()
        {
            while (true)
            {
                if(FormsApp.ble_interface.IsOpen()!=changedStatus)
                {
                    changedStatus = FormsApp.ble_interface.IsOpen();
                    if(FormsApp.ble_interface.IsOpen())
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            IsConnectedUIChange(true);
                          
                        });
                    }

                    if (!FormsApp.ble_interface.IsOpen())
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            IsConnectedUIChange(false);
                        });
                    }
                }
                         
                ChangeListViewData();

                Thread.Sleep(500); // 0.5 Second
            }
        }

        private void IsConnectedUIChange(bool v)
        {
            if(v){
                
                try
                {
                    deviceID.Text = peripheral.Advertisement.DeviceName;
                    macAddress.Text = BitConverter.ToString(btdata);
                    imageBattery.Source = "battery_toolbar_high";
                    imageRssi.Source = "rssi_toolbar_high";
                    batteryLevel.Text = "100%";
                    rssiLevel.Text = peripheral.Rssi.ToString() + " dBm";
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

        private void ChangeListViewData()
        {
            try
            {
                blePeripherals = FormsApp.ble_interface.GetBlePeripheralList();
              
                // YOU CAN RETURN THE PASS BY GETTING THE STRING AND CONVERTING IT TO BYTE ARRAY TO AUTO-PAIR
                byte[] bytes = System.Convert.FromBase64String(CrossSettings.Current.GetValueOrDefault("session_peripheral_DeviceId", string.Empty));

                byte[] byte_now = new byte[] { };

                for (int i = 0; i < blePeripherals.Count; i++)
                {
                    byte_now = blePeripherals[i].Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray();

                    //VERIFY IF PREVIOUSLY BOUNDED DEVICES WITH THE RIGHT USERNAME
                    if(CrossSettings.Current.GetValueOrDefault("session_dynamicpass",string.Empty)!=string.Empty)
                    {
                        if (FormsApp.CredentialsService.UserName.Equals(CrossSettings.Current.GetValueOrDefault("session_username", string.Empty)))
                        {
                            if (bytes.Take(4).ToArray().SequenceEqual(byte_now))
                            {
                                if(blePeripherals[i].Advertisement.DeviceName.Equals(CrossSettings.Current.GetValueOrDefault("session_peripheral", string.Empty)))
                                {
                                    if(!disconnectedButton)
                                    {
                                        if(!FormsApp.ble_interface.IsOpen())
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                try{
                                                  
                                                    FormsApp.ble_interface.Open(blePeripherals[i], true);
                                                    peripheral = blePeripherals[i];
                                                    btdata = peripheral.Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray();
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine(e.StackTrace);
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    DeviceItem device = new DeviceItem
                    {
                        deviceMacAddress = BitConverter.ToString(byte_now),
                        deviceName = blePeripherals[i].Advertisement.DeviceName,
                        deviceBattery = "100%",
                        deviceRssi = blePeripherals[i].Rssi.ToString() + " dBm",
                        deviceBatteryIcon = "battery_toolbar_high",
                        deviceRssiIcon = "rssi_toolbar_high",
                        Peripheral = blePeripherals[i]
                    };

                    if (temporal_ble_peripherals.Count<0){
                        employees.Add(device);
                    }else{
                        
                        bool enc = false;

                        for (int j = 0; j < temporal_ble_peripherals.Count; j++)
                        {
                            if (temporal_ble_peripherals[j].Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray()
                                .SequenceEqual(blePeripherals[i].Advertisement.ManufacturerSpecificData.ElementAt(0).Data.Take(4).ToArray())) //  if (temporal_ble_peripherals[j].DeviceId.Equals(blePeripherals[i].DeviceId)) enc = true;
                                  enc = true;
                        }
                         
                        if(!enc){
                            employees.Add(device);
                        }
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    DeviceList.ItemsSource = employees;
                });

                temporal_ble_peripherals = FormsApp.ble_interface.GetBlePeripheralList();

            }catch(Exception e){
                Console.WriteLine(e.StackTrace);
            } 
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
            Application.Current.MainPage.Navigation.PushAsync(new ReplaceMTUPage(dialogsSaved), false);                   
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

            Task.Delay(2000).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                dialog_turnoff_two.IsVisible = false;
                dialog_turnoff_three.IsVisible = true;
            }));

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
            Application.Current.MainPage.Navigation.PushAsync(new ReplaceMeterPage(dialogsSaved), false);
        }

        private void BluetoothPeripheralDisconnect(object sender, EventArgs e)
        {
            FormsApp.ble_interface.Close();
            disconnectedButton = true;
            DeviceList.RefreshCommand.Execute(true);
        }

        private void LogoutTapped(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;
            FormsApp.CredentialsService.DeleteCredentials();
            background_scan_page.IsEnabled = true;
            background_scan_page_detail.IsEnabled = true;
            Navigation.PopAsync();
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelectedListDevices(object sender, ItemTappedEventArgs e)
        {
            var item = (DeviceItem)e.Item;
            FormsApp.ble_interface.Open(item.Peripheral);

            deviceID.Text = item.deviceName;
            macAddress.Text = item.deviceMacAddress;
            imageBattery.Source = item.deviceBatteryIcon;
            imageRssi.Source = item.deviceRssiIcon;
            batteryLevel.Text = item.deviceBattery;
            rssiLevel.Text = item.deviceRssi;
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
                    }
                }
                catch (Exception w1)
                {
                    Console.WriteLine(w1.StackTrace);
                }
            }
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
                navigationDrawerList.SelectedItem = null;
                Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved), false);
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