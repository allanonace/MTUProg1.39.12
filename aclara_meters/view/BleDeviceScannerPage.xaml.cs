// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Acr.UserDialogs;
using aclara_meters.Helpers;
using aclara_meters.Models;
using aclara_meters.viewmodel;
using nexus.core.logging;
using nexus.protocols.ble;
using Xamarin.Forms;
using System.Threading;

namespace aclara_meters.view
{
    
   public partial class BleDeviceScannerPage
   {
       
      public BleDeviceScannerPage()
      {
         InitializeComponent();
      }

     
      public List<PageItem> menuList { get; set; }


        IUserDialogs dialogsSaved;
     

        public List<ReadMTUItem> menuListReadMTU { get; set; }


        public BleDeviceScannerPage(IUserDialogs dialogs )
        {
            InitializeComponent();

            Settings.IsConnectedBLE = false;

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            turnoffmtu_ok.Tapped += TurnOffMTU_OK;
            turnoffmtu_no.Tapped += Turnoffmtu_No_Tapped;
            turnoffmtu_ok_close.Tapped += TurnOffMtu_Close;
            replacemeter_ok.Tapped += Replacemeter_Ok_Tapped;
            replacemeter_cancel.Tapped += Replacemeter_Cancel_Tapped;
            meter_ok.Tapped += Meter_Ok_Tapped;
            meter_cancel.Tapped += Meter_Cancel_Tapped;
            connectElementMockUp.Tapped += ConnectElementMockUp_Tapped;


            shadoweffect.IsVisible = false;

            background_scan_page_detail.IsVisible = true;
            background_scan_page_detail.IsVisible = false;

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
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
                    });
                });
            }
            else
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
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
                    });
                });
            }



          
            dialogsSaved = dialogs;
            disconnectDevice.Tapped += bleDisconnect;
            background_scan_page_detail.IsVisible = true;
            background_scan_page_detail.IsVisible = false;
            background_scan_page.IsVisible = true;
            back_button.Tapped += hamburgerOpen;
            back_button_menu.Tapped += hamburgerClose;
            logout_button.Tapped += logout;
            back_button_detail.Tapped += hamburgerOpen;
            settings_button.Tapped += openSettings;

            navigationDrawerList.IsEnabled = true;

            navigationDrawerList.Opacity = 0.65;

           

            ContentNav.IsVisible = false;

            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;




            //Change username textview to Prefs. String
            if (FormsApp.CredentialsService.UserName != null)
            {
                userName.Text = FormsApp.CredentialsService.UserName;
            }

            menuList = new List<PageItem>();

            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var page1 = new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png", TargetType = "ReadMTU" };
            var page2 = new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", TargetType = "turnOff" };
            var page3 = new PageItem() { Title = "Add MTU", Icon = "addMTU.png", TargetType = "AddMTU" };
            var page4 = new PageItem() { Title = "Replace MTU", Icon = "replaceMTU2.png", TargetType = "replaceMTU" };
            var page5 = new PageItem() { Title = "Replace Meter", Icon = "replaceMeter.png", TargetType = "replaceMeter" };
            var page6 = new PageItem() { Title = "Add MTU / Add meter", Icon = "addMTUaddmeter.png", TargetType = "" };
            var page7 = new PageItem() { Title = "Add MTU / Rep. Meter", Icon = "addMTUrepmeter.png", TargetType = "" };
            var page8 = new PageItem() { Title = "Rep.MTU / Rep. Meter", Icon = "repMTUrepmeter.png", TargetType = "" };
            var page9 = new PageItem() { Title = "Install Confirmation", Icon = "installConfirm.png", TargetType = "" };


            // Adding menu items to menuList
            menuList.Add(page1);
            menuList.Add(page2);
            menuList.Add(page3);
            menuList.Add(page4);
            menuList.Add(page5);
            menuList.Add(page6);
            menuList.Add(page7);
            menuList.Add(page8);
            menuList.Add(page9);

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;
          

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


            //SCAN BLE DEVICES
            FormsApp.ble_interface.Scan();

        }

        public Boolean changedStatus;

        private void InvokeMethod()
        {
            while (true)
            {
                if(FormsApp.ble_interface.IsOpen()!=changedStatus){
                    changedStatus = FormsApp.ble_interface.IsOpen();
                    if(FormsApp.ble_interface.IsOpen()){
                        Device.BeginInvokeOnMainThread(() =>
                        {

                            background_scan_page_detail.IsVisible = true;
                            block_ble_disconnect.Opacity = 0;
                            block_ble_disconnect.FadeTo(1, 250);
                            block_ble.Opacity = 0;
                            block_ble.FadeTo(1, 250);
                            background_scan_page.IsVisible = false;
                            navigationDrawerList.IsEnabled = true;
                            navigationDrawerList.Opacity = 1;
                        });

                    }

                    if (!FormsApp.ble_interface.IsOpen())
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            background_scan_page_detail.IsVisible = false;
                            navigationDrawerList.Opacity = 0.65;
                            navigationDrawerList.IsEnabled = true;
                            background_scan_page.IsVisible = true;
                        });
                    }

                }
                Thread.Sleep(500); // 1.5 Second
            }
        }

    

        private void Replacemeter_Cancel_Tapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void Replacemeter_Ok_Tapped(object sender, EventArgs e)
        {

            //APP OPEN VIEW
            dialog_replacemeter_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

           
            Application.Current.MainPage.Navigation.PushAsync(new ReplaceMTUPage(dialogsSaved), false);
                                   
        }

        private void TurnOffMtu_Close(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void Turnoffmtu_No_Tapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void TurnOffMTU_OK(object sender, EventArgs e)
        {

            dialog_turnoff_one.IsVisible = false;
            dialog_turnoff_two.IsVisible = true;

            Task.Run(async () =>
            {
                await Task.Delay(2000); Device.BeginInvokeOnMainThread(() =>
                {
                    dialog_turnoff_two.IsVisible = false;
                    dialog_turnoff_three.IsVisible = true;
                });
            });


        }



        void Meter_Cancel_Tapped(object sender, EventArgs e)
        {

            dialog_open_bg.IsVisible = false;
            dialog_meter_replace_one.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

        }


        void Meter_Ok_Tapped(object sender, EventArgs e)
        {
            dialog_meter_replace_one.IsVisible = false;

            //APP OPEN VIEW
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Application.Current.MainPage.Navigation.PushAsync(new ReplaceMeterPage(dialogsSaved), false);
                                   

        }






        private void bleDisconnect(object sender, EventArgs e)
        {


            FormsApp.ble_interface.Close();

        }

        private void logout(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;

            FormsApp.CredentialsService.DeleteCredentials();

            background_scan_page.IsEnabled = true;
            background_scan_page_detail.IsEnabled = true;
            Navigation.PopAsync();



        }


        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private async void OnMenuItemSelectedAsync(object sender, ItemTappedEventArgs e)
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
                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;

                            background_scan_page.IsEnabled = true;
                            background_scan_page_detail.IsEnabled = true;

                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }
                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                                {


                                  
                                    navigationDrawerList.SelectedItem = null;


                                    Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(dialogsSaved), false);
                                    //    ReadMtuMethod(account, savedServer, dialogsSaved);


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

                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }




                                });

                            });

                            //  Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, bleGattServerViewModel.returnConnect(), dialogsSaved));

                            break;

                        case "AddMTU":
                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;
                            background_scan_page.IsEnabled = true;
                            background_scan_page_detail.IsEnabled = true;
                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }
                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                                {


                                 
                                    navigationDrawerList.SelectedItem = null;
            

                                    Application.Current.MainPage.Navigation.PushAsync(new AddMTUPage(dialogsSaved), false);
                                    //    ReadMtuMethod(account, savedServer, dialogsSaved);

                                 
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
                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }


                                });

                            });

                            //  Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, bleGattServerViewModel.returnConnect(), dialogsSaved));

                            break;






                        case "turnOff":
                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;
                            background_scan_page.IsEnabled = true;
                            background_scan_page_detail.IsEnabled = true;
                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }
                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
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
                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }


                                });

                            });

                            //  Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, bleGattServerViewModel.returnConnect(), dialogsSaved));

                            break;

                        case "replaceMTU":
                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;
                            background_scan_page.IsEnabled = true;
                            background_scan_page_detail.IsEnabled = true;
                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }
                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
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

                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }



                                });

                            });

                            //  Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, bleGattServerViewModel.returnConnect(), dialogsSaved));

                            break;


                        case "replaceMeter":
                            background_scan_page.Opacity = 1;
                            background_scan_page_detail.Opacity = 1;
                            background_scan_page.IsEnabled = true;
                            background_scan_page_detail.IsEnabled = true;
                            if (Device.Idiom == TargetIdiom.Phone)
                            {
                                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                            }
                            await Task.Run(async () =>
                            {

                                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
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

                                    if (Device.Idiom == TargetIdiom.Phone)
                                    {
                                        shadoweffect.IsVisible = false;
                                    }



                                });

                            });

                            //  Application.Current.MainPage.Navigation.PushAsync(new BleGattServicePage(account, bleGattServerViewModel.returnConnect(), dialogsSaved));

                            break;




                    }


                }
                catch (Exception w)
                {

                }
            }

        }

        BleGattServiceViewModel model_saved;




        private void openSettings(object sender, EventArgs e)
        {
            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;
            background_scan_page.IsEnabled = true;
            background_scan_page_detail.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Tablet)
            {
            }
            else
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);

            }

            Task.Run(async () =>
            {
                
                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                {

                    try{
                     
                        Application.Current.MainPage.Navigation.PushAsync(new BleSettingsPage(dialogsSaved), false);

                    
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

                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            shadoweffect.IsVisible = false;
                        }
                        return;
                    }catch(Exception f){
                        
                    }

                    Application.Current.MainPage.Navigation.PushAsync(new BleSettingsPage(true),false);


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

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        shadoweffect.IsVisible = false;
                    }

                });

            });


        }


                


        private void hamburgerOpen(object sender, EventArgs e)
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
     



        private void hamburgerClose(object sender, EventArgs e)
        {
            fondo.Opacity = 1;
            ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
            shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);

            background_scan_page.Opacity = 1;
            background_scan_page_detail.Opacity = 1;


            Task.Run(async () =>
            {

                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                {
                    ContentNav.Opacity = 0;
                    shadoweffect.IsVisible = false;
                    ContentNav.IsVisible = false;

                    background_scan_page.IsEnabled = true;
                    background_scan_page_detail.IsEnabled = true;

                });

            });

        }





        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // todo: this is a hack - hopefully Xamarin adds the ability to name a Pushed Page.
            //MainMenu.IsSegmentShowing = false;




            if(Navigation.NavigationStack.Count < 3){
              
                Settings.IsLoggedIn = false;
            }

        }





        void ConnectElementMockUp_Tapped(object sender, EventArgs e)
        {

            FormsApp.ble_interface.Open();

            //ble_library.BlePort.ConnectoToDevice();



        }












      private void Switch_OnToggled( Object sender, ToggledEventArgs e )
      {
         var vm = BindingContext as BleDeviceScannerViewModel;
         if(vm == null)
         {
            return;
         }
         if(e.Value)
         {
            if(vm.EnableAdapterCommand.CanExecute( null ))
            {
               vm.EnableAdapterCommand.Execute( null );
            }
         }
         else if(vm.DisableAdapterCommand.CanExecute( null ))
         {
            vm.DisableAdapterCommand.Execute( null );
         }
      }
   }
}
