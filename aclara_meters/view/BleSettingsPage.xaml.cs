// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using aclara_meters.Helpers;
using aclara_meters.Models;
using aclara_meters.viewmodel;
using nexus.core.text;
using nexus.protocols.ble;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using aclara.ViewModels;
using System.Threading;

namespace aclara_meters.view
{
    public partial class BleSettingsPage
    {

        public BleSettingsPage()
        {
            InitializeComponent();
        }


        public List<PageItem> menuList2 { get; set; }

        private void cargarMTU()
        {
            
            menuList2 = new List<PageItem>();


            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            var page1menu = new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png", TargetType = "ReadMTU" };
            var page2menu = new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", TargetType = "turnOff" };
            var page3menu = new PageItem() { Title = "Add MTU", Icon = "addMTU.png", TargetType = "AddMTU" };
            var page4menu = new PageItem() { Title = "Replace MTU", Icon = "replaceMTU2.png", TargetType = "replaceMTU" };
            var page5menu = new PageItem() { Title = "Replace Meter", Icon = "replaceMeter.png", TargetType = "replaceMeter" };
            var page6menu = new PageItem() { Title = "Add MTU / Add meter", Icon = "addMTUaddmeter.png", TargetType = "" };
            var page7menu = new PageItem() { Title = "Add MTU / Rep. Meter", Icon = "addMTUrepmeter.png", TargetType = "" };
            var page8menu = new PageItem() { Title = "Rep.MTU / Rep. Meter", Icon = "repMTUrepmeter.png", TargetType = "" };
            var page9menu = new PageItem() { Title = "Install Confirmation", Icon = "installConfirm.png", TargetType = "" };



            // Adding menu items to menuList
            menuList2.Add(page1menu);
            menuList2.Add(page2menu);
            menuList2.Add(page3menu);
            menuList2.Add(page4menu);
            menuList2.Add(page5menu);
            menuList2.Add(page6menu);
            menuList2.Add(page7menu);
            menuList2.Add(page8menu);
            menuList2.Add(page9menu);

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList2;


            logout_button.Tapped += logoutAsync;

        }

        private bool _userTapped;

        IUserDialogs dialogsSaved;

        private CustomSampleViewModel _viewModelread;


        public BleSettingsPage(bool error)
        {
            InitializeComponent();

            Settings.IsNotConnectedInSettings = true;
                
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ContentNav.IsVisible = true;
                        background_scan_page.Opacity = 1;

                        close_menu_icon.Opacity = 0;
                        hamburger_icon.IsVisible = false;


                        background_scan_page.Margin = new Thickness(310, 0, 0, 0);

                        tablet_user_view.TranslationY = -22;
                        tablet_user_view.Scale = 1.2;

                        logo_tablet_aclara.Opacity = 0;


                        aclara_logo.Scale = 1.2;
                        aclara_logo.TranslationX = 42;
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
                        //ContentNav.IsVisible = false;
                        background_scan_page.Margin = new Thickness(0, 0, 0, 0);


                        close_menu_icon.Opacity = 1;
                        hamburger_icon.IsVisible = true;

                 
                
                        tablet_user_view.TranslationY = 0;

                        tablet_user_view.Scale = 1;
                        logo_tablet_aclara.Opacity = 1;

                    });
                });
            }








            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            back_button.Tapped += returntomain;


            cargarMTU();

            _userTapped = false;


            navigationDrawerList.IsEnabled = false;
            navigationDrawerList.Opacity = 0.65;


            //Change username textview to Prefs. String
            if (Settings.SavedUserName != null)
            {
                userName.Text = Settings.SavedUserName;
            }




            // portrait
            Task.Run(async () =>
            {

                await Task.Delay(100); Device.BeginInvokeOnMainThread(() =>
                {
                    _viewModelread = new CustomSampleViewModel();

                    BindingContext = _viewModelread;

                    _viewModelread.LoadData();

                });
            });

            ButtonListeners();

            Task.Run(async () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {

                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;

                });
            });

            turnoffmtu_ok.Tapped += TurnOffMTU_OK;
            turnoffmtu_no.Tapped += Turnoffmtu_No_Tapped;
            turnoffmtu_ok_close.Tapped += TurnOffMtu_Close;
            replacemeter_ok.Tapped += Replacemeter_Ok_Tapped;
            replacemeter_cancel.Tapped += Replacemeter_Cancel_Tapped;
            meter_ok.Tapped += Meter_Ok_Tapped;
            meter_cancel.Tapped += Meter_Cancel_Tapped;
        }

        public BleSettingsPage(IUserDialogs dialogs)
        {
            InitializeComponent();


            if (Device.Idiom == TargetIdiom.Tablet)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ContentNav.IsVisible = true;
                        background_scan_page.Opacity = 1;

                        close_menu_icon.Opacity = 0;
                        hamburger_icon.IsVisible = false;


                        background_scan_page.Margin = new Thickness(310, 0, 0, 0);

                      
                        sync_block.Scale = 1;
                        logs_block.Scale = 1;
                        about_block.Scale = 1;

           

                        tablet_user_view.TranslationY = -22;
                        tablet_user_view.Scale = 1.2;

                        logo_tablet_aclara.Opacity = 0;

                        lowerbar.TranslationX = 310;
                        lowerbar.Margin = new Thickness(0, 0, 310, 0);

                        shadoweffect.IsVisible = true;

                        aclara_logo.Scale = 1.2;
                        aclara_logo.TranslationX = 42;
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
                        //ContentNav.IsVisible = false;
                        background_scan_page.Margin = new Thickness(0, 0, 0, 0);


                        close_menu_icon.Opacity = 1;
                        hamburger_icon.IsVisible = true;

                      
                        sync_block.Scale = 0.9;
                        logs_block.Scale = 1;
                        about_block.Scale = 1;

                        tablet_user_view.TranslationY = 0;

                        tablet_user_view.Scale = 1;
                        logo_tablet_aclara.Opacity = 1;
                      
                    });
                });
            }


            dialogsSaved = dialogs;

            cargarMTU();

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            back_button.Tapped += returntomain;

            _userTapped = false;

            //Change username textview to Prefs. String
            if (Settings.SavedUserName != null)
            {
                userName.Text = Settings.SavedUserName;
            }


            ButtonListeners();

            // portrait
            Task.Run(async () =>
            {

                await Task.Delay(100); Device.BeginInvokeOnMainThread(() =>
                {
                    _viewModelread = new CustomSampleViewModel();

                    BindingContext = _viewModelread;

                    _viewModelread.LoadData();

                });
            });


            Task.Run(async () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {

                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;

                });
            });

       

            turnoffmtu_ok.Tapped += TurnOffMTU_OK;
            turnoffmtu_no.Tapped += Turnoffmtu_No_Tapped;
            turnoffmtu_ok_close.Tapped += TurnOffMtu_Close;
            replacemeter_ok.Tapped += Replacemeter_Ok_Tapped;
            replacemeter_cancel.Tapped += Replacemeter_Cancel_Tapped;
            meter_ok.Tapped += Meter_Ok_Tapped;
            meter_cancel.Tapped += Meter_Cancel_Tapped;


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



        private async void logoutAsync(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;
            FormsApp.CredentialsService.DeleteCredentials();

            try
            {
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception Message: " + ex.Message);
            }

            // Application.Current.MainPage = new LoginMenuPage(bleAdapterSaved, dialogsSaved);
            int contador = Navigation.NavigationStack.Count;

            //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
            while (contador > 0)
            {
                //Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);

                try
                {
                    Navigation.PopAsync(false);

                }
                catch (Exception v)
                {

                }

                contador--;
            }

            try
            {
                await Navigation.PopToRootAsync(false);

            }
            catch (Exception v)
            {

            }


        }


        private void returntomain(object sender, EventArgs e)
        {

            Application.Current.MainPage.Navigation.PopAsync(false);
            Settings.IsNotConnectedInSettings = false;


        }

       



        private void OnItemSelected(Object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;


        }





        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private async void OnMenuItemSelectedAsync(object sender, ItemTappedEventArgs e)
        {


            if (!Settings.IsConnectedBLE)
            {
                // don't do anything if we just de-selected the row.
                if (e.Item == null) return;
                // Deselect the item.
                if (sender is ListView lv) lv.SelectedItem = null;
            }
            if (Settings.IsConnectedBLE)
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


                            background_scan_page.IsEnabled = true;

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


                                    background_scan_page.Opacity = 1;


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


                            break;

                        case "AddMTU":
                            background_scan_page.Opacity = 1;

                            background_scan_page.IsEnabled = true;

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


                                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved), false);


                                    background_scan_page.Opacity = 1;


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


                            break;




                        case "turnOff":
                            background_scan_page.Opacity = 1;

                            background_scan_page.IsEnabled = true;

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


                            break;

                        case "replaceMTU":
                            background_scan_page.Opacity = 1;

                            background_scan_page.IsEnabled = true;

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


                            break;


                        case "replaceMeter":
                            background_scan_page.Opacity = 1;

                            background_scan_page.IsEnabled = true;

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



                            break;




                    }


                }
                catch (Exception w)
                {

                }
            }

        }



        private void forceClick(object sender, EventArgs e)
        {
           

              
                force_sync.IsEnabled = false;

                //date_sync.Text = "                -";
                //updated_files.Text = "         -";
               // pending_files.Text = "        -";

          
               backdark_bg.IsVisible = true;
               indicator.IsVisible = true;

          
                Task.Run(async () =>
                {
                    await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                    {
                        Task.Run(async () =>
                        {
                            await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                            {
                              
                                Task.Run(async () =>
                                {
                                    await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                                    {
                                     
                                        String myDate = DateTime.Now.ToString();

                                        date_sync.Text = myDate;

                                        updated_files.Text = "1456";
                                        pending_files.Text = "23";

                                        force_sync.IsEnabled = true;

                                        backdark_bg.IsVisible = false;
                                        indicator.IsVisible = false;
                                    });
                                });
                            });
                        });
                    });
                });



           
                                           
        }



        private void ButtonListeners()
        {
            about_button_pressed.Tapped += aboutClick;
            logs_button_pressed.Tapped += logsClick;
            sync_button_pressed.Tapped += syncClick;
            force_sync.Clicked += forceClick;

            InitLayout(1); 
   
        }




        private void aboutClick(object sender, EventArgs e)
        {
            InitLayout(1);

           
        }

        private void logsClick(object sender, EventArgs e)
        {
            InitLayout(2);
           
        }

        private void syncClick(object sender, EventArgs e)
        {
            InitLayout(3);
           
        }


        private void InitLayout(int valor)
        {
            switch (valor)
            {
                case 1:
                    about_block.IsVisible = true; logs_block.IsVisible = false; sync_block.IsVisible = false;
                    about_block.IsEnabled = true; logs_block.IsEnabled = false; sync_block.IsEnabled = false;
                    about_button_text.Opacity = 1; about_button.Opacity = 1;
                    logs_button_text.Opacity = 0.5; logs_button.Opacity = 0.5;
                    sync_button_text.Opacity = 0.5; sync_button.Opacity = 0.5;
                    title_text.Text = "About";

                    //popup_show.IsVisible = false; popup_show.IsEnabled = false;


                    break;

                case 2:
                    about_block.IsVisible = false; logs_block.IsVisible = true; sync_block.IsVisible = false;
                    about_block.IsEnabled = false; logs_block.IsEnabled = true; sync_block.IsEnabled = false;
                    about_button_text.Opacity = 0.5; about_button.Opacity = 0.5;
                    logs_button_text.Opacity = 1; logs_button.Opacity = 1;
                    sync_button_text.Opacity = 0.5; sync_button.Opacity = 0.5;
                    title_text.Text = "Activity Logs";
                    //popup_show.IsVisible = false; popup_show.IsEnabled = false;
                    break;

                case 3:
                    about_block.IsVisible = false; logs_block.IsVisible = false; sync_block.IsVisible = true;
                    about_block.IsEnabled = false; logs_block.IsEnabled = false; sync_block.IsEnabled = true;
                    about_button_text.Opacity = 0.5; about_button.Opacity = 0.5;
                    logs_button_text.Opacity = 0.5; logs_button.Opacity = 0.5;
                    sync_button_text.Opacity = 1; sync_button.Opacity = 1;
                    title_text.Text = "File Syncronization";
                    //popup_show.IsVisible = false; popup_show.IsEnabled = false;
                    break;
            }
        }



    }
}
