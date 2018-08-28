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
using System.Threading;
using System.Collections.ObjectModel;

namespace aclara_meters.view
{
    public partial class ReplaceMTUPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();

            background_scan_page.Opacity = 0.5;
            background_scan_page.FadeTo(1, 500);
        }


        public ReplaceMTUPage()
        {
            InitializeComponent();
        }


        //List of ADD MTU components
        string strFunctlLoctn = "";

        public List<ReadMTUItem> menuList { get; set; }

        public List<PageItem> menuList2 { get; set; }

        private void PickerSelection(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
        }

        private void InitPickerList()
        {
            InitPickerReadInterval();
            InitPickerSnapReads();
            InitPickerTwoWay();
            InitPickerMeterType();

        }


        private void InitPickerReadInterval()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>();

            objClassList.Add(new PickerItems { Name = "Read Interval 1" });
            objClassList.Add(new PickerItems { Name = "Read Interval 2" });
            objClassList.Add(new PickerItems { Name = "Read Interval 3" });

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerReadInterval.ItemsSource = objStringList;
        }


        private void InitPickerMeterType()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>();

            objClassList.Add(new PickerItems { Name = "Type 1" });
            objClassList.Add(new PickerItems { Name = "Type 2" });
            objClassList.Add(new PickerItems { Name = "Type 3" });

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerMeterType.ItemsSource = objStringList;
        }


        private void InitPickerSnapReads()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>();

            objClassList.Add(new PickerItems { Name = "Snap Reads 1" });
            objClassList.Add(new PickerItems { Name = "Snap Reads 2" });
            objClassList.Add(new PickerItems { Name = "Snap Reads 3" });

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerSnapReads.ItemsSource = objStringList;
        }



        private void InitPickerTwoWay()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>();

            objClassList.Add(new PickerItems { Name = "2-Way 1" });
            objClassList.Add(new PickerItems { Name = "2-Way 2" });
            objClassList.Add(new PickerItems { Name = "2-Way 3" });

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */

            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerTwoWay.ItemsSource = objStringList;
        }


        private void cargarMTU()
        {

            InitPickerList();

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
            settings_button.Tapped += openSettings;

        }


        private void openSettings(object sender, EventArgs e)
        {

            Task.Run(async () =>
            {

                await Task.Delay(100); Device.BeginInvokeOnMainThread(() =>
                {

                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewSettings(dialogsSaved), false);

                });

            });


        }

        private void changeImageColor(bool v)
        {
            if (v)
            {
                bg_read_mtu_button_img.Source = "rep_mtu_btn_black.png";

            }
            else
            {
                bg_read_mtu_button_img.Source = "rep_mtu_btn.png";

            }
        }

        private bool _userTapped;

        private void ReadMTU(object sender, EventArgs e)
        {
            if (!_userTapped)
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    label_read.Opacity = 1;
                  
                });

                Task.Run(async () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        backdark_bg.IsVisible = true;
                        indicator.IsVisible = true;

                    });
                });



                background_scan_page.IsEnabled = false;
                _userTapped = true;
                changeImageColor(true);
                load_read_mtu();
                label_read.Text = "Replacing MTU ... ";
                Task.Run(async () =>
                {
                    await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                    {

                        label_read.Text = "Replacing MTU ... 3 sec";
                       


                        Task.Run(async () =>
                        {
                            await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                            {
                                label_read.Text = "Replacing MTU ... 2 sec";
                              

                                Task.Run(async () =>
                                {
                                    await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                                    {
                                        label_read.Text = "Replacing MTU ... 1 sec";
                                      

                                        Task.Run(async () =>
                                        {
                                            await Task.Delay(1000); Device.BeginInvokeOnMainThread(() =>
                                            {

                                                _userTapped = false;
                                                label_read.Text = "Successful MTU replace";
                                                //pb_ProgressBar.ProgressTo(1, 450, Easing.Linear);
                                                bg_read_mtu_button.NumberOfTapsRequired = 1;
                                                changeImageColor(false);

                                                Task.Run(async () =>
                                                {
                                                    await Task.Delay(100); Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        
                                                        Task.Run(async () =>
                                                        {
                                                            Device.BeginInvokeOnMainThread(() =>
                                                            {

                                                                backdark_bg.IsVisible = false;
                                                                indicator.IsVisible = false;

                                                            });
                                                        });

                                                        background_scan_page.IsEnabled = true;
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });


            }

        }

        private void load_read_mtu()
        {
            //throw new NotImplementedException();
        }

        IUserDialogs dialogsSaved;

        public ReplaceMTUPage(IUserDialogs dialogs)
        {
            InitializeComponent();

            label_read.Opacity = 0;
         

            Task.Run(async () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {

                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;

                });
            });


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
                        background_scan_page.Margin = new Thickness(0, 0, 0, 0);


                        close_menu_icon.Opacity = 1;
                        hamburger_icon.IsVisible = true;
        
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

            //Button READ
            bg_read_mtu_button.Tapped += ReadMTU;

            label_read.Text = "Push Button to START";


            _userTapped = false;



            //Change username textview to Prefs. String
            if (Settings.SavedUserName != null)
            {
                userName.Text = Settings.SavedUserName;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                label_read.Opacity = 1;

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


                                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved), false);


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






    }
}
