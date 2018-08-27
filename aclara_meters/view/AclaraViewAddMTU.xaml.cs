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
using System.Collections.ObjectModel;

namespace aclara_meters.view
{
    public partial class AclaraViewAddMTU
    {
        private IUserDialogs dialogsSaved;
        private bool _userTapped;

        public AclaraViewAddMTU()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            background_scan_page.Opacity = 0.5;
            background_scan_page.FadeTo(1, 500);
        }

        private List<PageItem> MenuList { get; set; }

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
        }

        private void InitPickerReadInterval()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Read Interval 1" },
                new PickerItems { Name = "Read Interval 2" },
                new PickerItems { Name = "Read Interval 3" }
            };

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

        private void InitPickerSnapReads()
        {
            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();

            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "Snap Reads 1" },
                new PickerItems { Name = "Snap Reads 2" },
                new PickerItems { Name = "Snap Reads 3" }
            };

            /* Here we have to assign service Items to one ObservableCollection<string>() for this purpose
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
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
            {
                new PickerItems { Name = "2-Way 1" },
                new PickerItems { Name = "2-Way 2" },
                new PickerItems { Name = "2-Way 3" }
            };

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


        private void OpenSettingsCallAsync(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                await Task.Delay(100); 
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PushAsync(new BleSettingsPage(dialogsSaved), false);
                });

            });
        }

        private void ChangeLowerButtonImage(bool v)
        {
            if (v)
            {
                bg_read_mtu_button_img.Source = "add_mtu_btn_black.png";

            }
            else
            {
                bg_read_mtu_button_img.Source = "add_mtu_btn.png";
            }
        }

        private void ReadMTU(object sender, EventArgs e)
        {
            if (!_userTapped)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        backdark_bg.IsVisible = true;
                        indicator.IsVisible = true;
                    });
                });

                _userTapped = true;
                background_scan_page.IsEnabled = false;
                ChangeLowerButtonImage(true);

                AddDataFromMTU();
                label_read.Text = "Writing to MTU ... ";

                Task.Run(async () =>
                {
                    await Task.Delay(1000); 
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        label_read.Text = "Writing to MTU ... 3 sec";
                      
                        Task.Run(async () =>
                        {
                            await Task.Delay(1000); 
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                label_read.Text = "Writing to MTU ... 2 sec";
                                Task.Run(async () =>
                                {
                                    await Task.Delay(1000); 
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        label_read.Text = "Writing to MTU ... 1 sec";
                                    });
                                });
                            });
                        });
                    });
                });

                Task.Run(async () =>
                {
                    await Task.Delay(4000); 
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _userTapped = false;
                        bg_read_mtu_button.NumberOfTapsRequired = 1;
                        ChangeLowerButtonImage(false);
                        backdark_bg.IsVisible = false;
                        indicator.IsVisible = false;
                        label_read.Text = "Successful MTU write";
                        background_scan_page.IsEnabled = true;
                    });
                });
            }
        }

        private void AddDataFromMTU()
        {
            //TO-DO
        }

        public AclaraViewAddMTU(IUserDialogs dialogs)
        {
            InitializeComponent();

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    label_read.Opacity = 0;
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                });
            });

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

            // Load side menu list
            LoadSideMenuElements();

            //Init picker list elements
            InitPickerList();

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            InitializeLowerbarLabel();

            //Change username textview to Prefs. String
            if (FormsApp.CredentialsService.UserName != null)
            {
                userName.Text = FormsApp.CredentialsService.UserName;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                label_read.Opacity = 1;
            });

            _userTapped = false;

            //Initialize Tap/Clickable element listeners
            TappedListeners();
        }

        private void LoadPhoneUI()
        {
            background_scan_page.Margin = new Thickness(0, 0, 0, 0);
            close_menu_icon.Opacity = 1;
            hamburger_icon.IsVisible = true;
            tablet_user_view.TranslationY = 0;
            tablet_user_view.Scale = 1;
            logo_tablet_aclara.Opacity = 1;
        }

        private void LoadTabletUI()
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
        }

        private void InitializeLowerbarLabel()
        {
            label_read.Text = "Push Button to START";
        }

        private void TappedListeners()
        {
            logout_button.Tapped += LogoutCallAsync;
            settings_button.Tapped += OpenSettingsCallAsync;
            back_button.Tapped += ReturnToMainView;
            bg_read_mtu_button.Tapped += ReadMTU;
            turnoffmtu_ok.Tapped += TurnOffMTUOkTapped;
            turnoffmtu_no.Tapped += TurnOffMTUNoTapped;
            turnoffmtu_ok_close.Tapped += TurnOffMTUCloseTapped;
            replacemeter_ok.Tapped += ReplaceMeterOkTapped;
            replacemeter_cancel.Tapped += ReplaceMeterCancelTapped;
            meter_ok.Tapped += MeterOkTapped;
            meter_cancel.Tapped += MeterCancelTapped;
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

            Task.Run(async () =>
            {
                await Task.Delay(2000); Device.BeginInvokeOnMainThread(() =>
                {
                    dialog_turnoff_two.IsVisible = false;
                    dialog_turnoff_three.IsVisible = true;
                });
            });
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

        private async void LogoutCallAsync(object sender, EventArgs e)
        {
            Settings.IsLoggedIn = false;
            FormsApp.CredentialsService.DeleteCredentials();

            int contador = Navigation.NavigationStack.Count;
            while (contador > 0)
            {
                try
                {
                    await Navigation.PopAsync(false);
                } catch (Exception v){
                    Console.WriteLine(v.StackTrace);
                }
                contador--;
            }

            try
            {
                await Navigation.PopToRootAsync(false);
            }catch (Exception v1){
                Console.WriteLine(v1.StackTrace);
            }

        }

        private void ReturnToMainView(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync(false);
        }

        private void OnItemSelected(Object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
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
                            OnMenuCaseReadMTU();
                            break;

                        case "AddMTU":
                            OnMenuCaseAddMTU();
                            break;

                        case "turnOff":
                            OnMenuCaseTurnOFF();
                            break;

                        case "replaceMTU":
                            OnMenuCaseReplaceMTU();
                            break;

                        case "replaceMeter":
                            OnMenuCaseReplaceMeter();
                            break;
                    }
                }
                catch (Exception w1)
                {
                    Console.WriteLine(w1.StackTrace);
                }
            }
        }

		#pragma warning disable RECS0165 // Asynchronous methods must return a task instead of a null value
        private async void OnMenuCaseReplaceMeter()
        {
            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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

                    shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; //if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                });
               
            });
            
        }

        private async void OnMenuCaseReplaceMTU()
        {
            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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

                    shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if(Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                });
            });
        }

        private async void OnMenuCaseTurnOFF()
        {
            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
                   
                    shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                });
            });
        }

        private async void OnMenuCaseAddMTU()
        {
            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            await Task.Run(async () =>
            {
                await Task.Delay(200); Device.BeginInvokeOnMainThread(() =>
                {
                    navigationDrawerList.SelectedItem = null;
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved),false);
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

                    shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                });

            });
        }

        private async void OnMenuCaseReadMTU()
        {
            background_scan_page.Opacity = 1;
            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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

                    shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
                }); 
            });
        }
		#pragma warning restore RECS0165 // Asynchronous methods must return a task instead of a null value
    }
}
