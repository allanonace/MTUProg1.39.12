// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using aclara_meters.Models;
using Xamarin.Forms;
using aclara.ViewModels;
using Plugin.Settings;
using MTUComm;
using Library.Exceptions;

using Renci.SshNet;

using Xml;

using ActionType = MTUComm.Action.ActionType;
using System.IO;

using System.Xml.Linq;
using Library;
using Xamarin.Essentials;

namespace aclara_meters.view
{
    public partial class AclaraViewSettings
    {
        private const string TEXT_COPYR   = "Copyright © 2018 Aclara Technologies LLC.";
        private const string TEXT_SUPPORT = "System tech Support: 1-866-205-5058";
        private const string TEXT_VERSION = "Application Version: ";
        private const string TEXT_INTUNE  = " [ using Intune ]";
        private const string TEXT_LICENSE = "Licensed to: ";
        private const string TEXT_CONFVER = "Configuration version: ";

        private ActionType actionType;
        private IUserDialogs dialogsSaved;
        private TabLogViewModel viewModelTabLog;
        private List<PageItem> MenuList { get; set; }

        private string NewConfigVersion;

        Global global;

        public AclaraViewSettings()
        {
            InitializeComponent();
        }

        private void LoadMTUView()
        {
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection

            MenuList = new List<PageItem>();

            // Adding menu items to MenuList

            MenuList.Add(new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png",Color="White", TargetType = ActionType.ReadMtu });

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

            if (FormsApp.config.Global.ShowDataRead)
                MenuList.Add(new PageItem() { Title = "Data Read", Icon = "readmtu_icon.png", Color = "White", TargetType = ActionType.DataRead });

            // ListView needs to be at least  elements for UI Purposes, even empty ones
            while (MenuList.Count < 9)
                MenuList.Add(new PageItem() { Title = "", Color = "#6aa2b8", Icon = "" });

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = MenuList;
        }

        readonly bool notConnected;

        public AclaraViewSettings(bool notConnected)
        {
            InitializeComponent();

            viewModelTabLog  = new TabLogViewModel();
            BindingContext = viewModelTabLog;

            this.notConnected = notConnected;

            global = FormsApp.config.Global;
            //Settings.IsNotConnectedInSettings = true;
            if (Device.Idiom == TargetIdiom.Tablet)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(LoadTabletUIConnected);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(LoadPhoneUIConnected);
                });
            }
            FocusEntryFields();

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar
            LoadMTUView();
            navigationDrawerList.IsEnabled = false;
            navigationDrawerList.Opacity = 0.65;

            //Change username textview to Prefs. String
            if (FormsApp.credentialsService.UserName != null)
            {
                userName.Text = FormsApp.credentialsService.UserName;
            }
            //List<FileInfo> ListFiles = GenericUtilsClass.LogFilesToUpload(Mobile.LogPath, true);
            // portrait
            Task.Run(async () =>
            {
                await Task.Delay(100); 
                Device.BeginInvokeOnMainThread(() =>
                {
                     ChangeLogFile(viewModelTabLog.TotalFiles, false);
                });
           });

            ButtonListeners();
            InitLayout(1); 
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {

                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                    battery_level.Opacity = 0;
                    rssi_level.Opacity = 0;
                });
            });
        }

        public AclaraViewSettings(IUserDialogs dialogs)
        {
            InitializeComponent();

            viewModelTabLog = new TabLogViewModel();
            BindingContext = viewModelTabLog;

            global = FormsApp.config.Global;

            if (Device.Idiom == TargetIdiom.Tablet)
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(LoadTabletUIConnected);
                });
            }
            else
            {
                Task.Run(() =>
                {
                    Device.BeginInvokeOnMainThread(LoadPhoneUIConnected);
                });
            }
            FocusEntryFields();

            dialogsSaved = dialogs;
            LoadMTUView();
            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            //Change username textview to Prefs. String
            if (FormsApp.credentialsService.UserName != null)
            {
                userName.Text = FormsApp.credentialsService.UserName;
            }

            ButtonListeners();
            InitLayout(1);

            // portrait
            Task.Run(async () =>
            {
                await Task.Delay(100); 
                Device.BeginInvokeOnMainThread(() =>
                {
                    ChangeLogFile(viewModelTabLog.TotalFiles, false);
                });
            });

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                });
            });


            battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
            rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

        }

        private void LoadPhoneUIConnected()
        {
            background_scan_page.Margin = new Thickness(0, 0, 0, 0);
            close_menu_icon.Opacity = 1;
            hamburger_icon.IsVisible = true;
            sync_block.Scale = 0.9;
            logs_block.Scale = 1;
            about_block.Scale = 1;
            tablet_user_view.TranslationY = 0;
            tablet_user_view.Scale = 1;
            logo_tablet_aclara.Opacity = 1;
            file_name.FontSize = 20;
        }

        private void LoadTabletUIConnected()
        {
            ContentNav.IsVisible = true;
            background_scan_page.Opacity = 1;
            close_menu_icon.Opacity = 0;
            hamburger_icon.IsVisible = true;
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

            shadoweffect.Source = "shadow_effect_tablet";
        }

        private void DoBasicRead()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Task.Factory.StartNew(BasicReadThread);
            });
        }

        private void ReplaceMtuCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
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
            Navigation.PopToRootAsync(false);
        }

        private void TurnOffMTUNoTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
        }

        private void TurnOffMTUOKTapped(object sender, EventArgs e)
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


        private async void LogoutTapped(object sender, EventArgs e)
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
    

        private void ReturnToMainView(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync(false);
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
                    ActionType page = item.TargetType;
                    ((ListView)sender).SelectedItem = null;
                 // MRA   la pagina de settings tiene como actiontype ReadMtu
                 //   if (this.actionType != page)
                 //   {
                        this.actionType = page;
                        NavigationController(page);
                 //   }
                   
                   

                }
                catch (Exception w2)
                {
                    Utils.Print(w2.StackTrace);
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
                case ActionType.DataRead:

                    #region Read data Controller

                    background_scan_page.Opacity = 1;

                    background_scan_page.IsEnabled = true;

                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                        shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
                    }

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            navigationDrawerList.SelectedItem = null;

                            //Application.Current.MainPage.Navigation.PushAsync(new AclaraViewDataRead(dialogsSaved, page), false);
                            DoBasicRead();

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
                        })
                    );

                    #endregion

                    break;
                case ActionType.ReadMtu:

                    #region Read Mtu Controller

                    background_scan_page.Opacity = 1;

                    background_scan_page.IsEnabled = true;

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

                case ActionType.AddMtu:

                    #region Add Mtu Controller

                    background_scan_page.Opacity = 1;

                    background_scan_page.IsEnabled = true;


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

                    background_scan_page.IsEnabled = true;

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

                    background_scan_page.IsEnabled = true;

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

                    background_scan_page.IsEnabled = true;

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

                    background_scan_page.IsEnabled = true;

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

                    background_scan_page.IsEnabled = true;

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

                    background_scan_page.IsEnabled = true;

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

                    background_scan_page.IsEnabled = true;

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


        void BasicReadThread()
        {
            MTUComm.Action basicRead = new MTUComm.Action(
               config: FormsApp.config,
               serial: FormsApp.ble_interface,
               type: MTUComm.Action.ActionType.BasicRead,
               user: FormsApp.credentialsService.UserName);

            basicRead.OnFinish += ((s, e) =>
            {
                Task.Delay(100).ContinueWith(t =>
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (actionType == ActionType.DataRead)
                             Application.Current.MainPage.Navigation.PushAsync(new AclaraViewDataRead(dialogsSaved,  this.actionType), false);
                        else
                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved,  this.actionType), false);

                        #region New Circular Progress bar Animations    

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

                backdark_bg.IsVisible = true;
                indicator.IsVisible = true;
                background_scan_page.IsEnabled = false;

                #endregion

            });

            basicRead.Run();

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

        private async void ForceSyncButtonTapped(object sender, EventArgs e)
        {
            force_sync.IsEnabled = false;
            Wait(true);

            // Upload log files
            await GenericUtilsClass.UploadFiles (false);
            
            viewModelTabLog.RefreshList();
            ChangeLogFile(viewModelTabLog.TotalFiles,false);

          
                String myDate = DateTime.Now.ToString();
                date_sync.Text = myDate;
                updated_files.Text = GenericUtilsClass.NumFilesUploaded.ToString();
                pending_files.Text = GenericUtilsClass.NumLogFilesToUpload(Mobile.LogPath).ToString();
                backup_files.Text = GenericUtilsClass.NumBackupFiles().ToString();
                Color colorText = pending_files.TextColor;
                if (int.Parse(backup_files.Text) >= 100)
                    colorText = Color.Red;

                lbl_backup.TextColor = colorText;
                force_sync.IsEnabled = true;
                Wait(false);
           
        }

        private void ButtonListeners()
        {
            about_button_pressed.Tapped += AboutButtonTapped;
            logs_button_pressed.Tapped += LogsButtonTapped;
            sync_button_pressed.Tapped += SyncButtonTapped;
            ftp_button_pressed.Tapped += FtpButtonTapped;
            force_sync.Clicked += ForceSyncButtonTapped;
            back_button.Tapped += ReturnToMainView;
            logout_button.Tapped += LogoutTapped;
            turnoffmtu_ok.Tapped += TurnOffMTUOKTapped;
            turnoffmtu_no.Tapped += TurnOffMTUNoTapped;
            turnoffmtu_ok_close.Tapped += TurnOffMTUCloseTapped;
            replacemeter_ok.Tapped += ReplaceMtuOkTapped;
            replacemeter_cancel.Tapped += ReplaceMtuCancelTapped;
            meter_ok.Tapped += MeterOkTapped;
            meter_cancel.Tapped += MeterCancelTapped;

            btn_Test.Clicked += Btn_Test_Clicked;
            btn_Save.Clicked += Btn_Save_Clicked;
            btn_Cancel.Clicked += Btn_Cancel_Clicked;
            
            btn_DownloadConf.Clicked += Btn_DownloadConf_Clicked;

            TabPrevious.Tapped += Previous_Clicked;
            TabNext.Tapped += Next_Clicked;
            logoff_no.Tapped += LogOffNoTapped;
            logoff_ok.Tapped += LogOffOkTapped;


            dialog_AddMTUAddMeter_ok.Tapped += dialog_AddMTUAddMeter_okTapped;
            dialog_AddMTUAddMeter_cancel.Tapped += dialog_AddMTUAddMeter_cancelTapped;

            dialog_AddMTUReplaceMeter_ok.Tapped += dialog_AddMTUReplaceMeter_okTapped;
            dialog_AddMTUReplaceMeter_cancel.Tapped += dialog_AddMTUReplaceMeter_cancelTapped;

            dialog_ReplaceMTUReplaceMeter_ok.Tapped += dialog_ReplaceMTUReplaceMeter_okTapped;
            dialog_ReplaceMTUReplaceMeter_cancel.Tapped += dialog_ReplaceMTUReplaceMeter_cancelTapped;


            dialog_AddMTU_ok.Tapped += dialog_AddMTU_okTapped;
            dialog_AddMTU_cancel.Tapped += dialog_AddMTU_cancelTapped;

        }

        public async void Btn_DownloadConf_Clicked(object sender, EventArgs e)
        {

            if (await ConfirmDownloadFilesAsync())
            {
                Wait(true);

                GenericUtilsClass.BackUpConfigFiles();
                if (await DownloadConfigProcess())
                {
                    if (Configuration.CheckLoadXML())
                    {
                        await SecureStorage.SetAsync("ConfigVersion", NewConfigVersion);
                        await Application.Current.MainPage.DisplayAlert("Attention", "The application will end, restart it to make changes in the configuration", "OK");
                        //Errors.LogErrorNowAndKill(new ConfigFilesChangedException());
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    else
                    {
                        GenericUtilsClass.RestoreConfigFiles();
                        await Application.Current.MainPage.DisplayAlert("Attention", 
                            "The new version configuration files are corrupted, the app will continues with the actual files. Contact your IT administratorn", "OK");
                    }
                   
                }
                Wait(false);

            }
        }
    
        public async Task<bool> ConfirmDownloadFilesAsync()
        {
            var config = new Acr.UserDialogs.ConfirmConfig()
            {
                Message = "Are you sure you want to download the configuration files?",
                OkText = "Yes",
                CancelText = "No"
            };
            return await UserDialogs.Instance.ConfirmAsync(config);
        }

        void Btn_Cancel_Clicked(object sender, EventArgs e)
        {
            tbx_user_name.Text = string.Empty;
            tbx_user_pass.Text = string.Empty;
            tbx_remote_host.Text = string.Empty;
            tbx_remote_path.Text = string.Empty;
        }


        void Btn_Save_Clicked(object sender, EventArgs e)
        {

            Wait(true);
            if (!GenericUtilsClass.TestFtpCredentials(tbx_remote_host.Text, tbx_user_name.Text, tbx_user_pass.Text, tbx_remote_path.Text))
            {
                DisplayAlert("Infomation", "Can't connect with the FTP, please check the entered data", "OK");
            }
            else
                SaveFTPCredentials();
            Wait(false);
        }

        private void SaveFTPCredentials()
        {
            Wait(true);
            //Save FTP in global.xml and in global data
            try
            {
                global.ftpPassword = tbx_user_pass.Text;
                global.ftpUserName = tbx_user_name.Text;
                global.ftpRemoteHost = tbx_remote_host.Text;
                global.ftpRemotePath = tbx_remote_path.Text;


                String uri = Path.Combine(Mobile.ConfigPath, Configuration.XML_GLOBAL);

                XDocument doc = XDocument.Load(uri);

                doc.Root.SetElementValue("ftpRemotePath", tbx_remote_path.Text);
                doc.Root.SetElementValue("ftpRemoteHost", tbx_remote_host.Text);
                doc.Root.SetElementValue("ftpUserName", tbx_user_name.Text);
                doc.Root.SetElementValue("ftpPassword", tbx_user_pass.Text);


                doc.Save(uri);
                DisplayAlert("Infomation", "SFTP/FTP settings saved in global.xml", "OK");
            }
            catch (Exception e)
            {
                Errors.ShowAlert(new FtpConnectionException());
                
            }
            Wait(false);
        }

        private void Btn_Test_Clicked(object sender, EventArgs e)
        {
            Wait(true);
            if (GenericUtilsClass.TestFtpCredentials(tbx_remote_host.Text, tbx_user_name.Text, tbx_user_pass.Text, tbx_remote_path.Text))
            {
               DisplayAlert("Information", "Connect to FTP succesfully", "OK");
            }
            else
            {
                Errors.ShowAlert(new FtpConnectionException());
            }
            Wait(false);
        }


        private void TapToHome_Tabletmode(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync(false);

        }


        void MeterCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_meter_replace_one.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
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
            Navigation.PopToRootAsync(false);
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
            Navigation.PopToRootAsync(false);
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
            Navigation.PopToRootAsync(false);
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
            Navigation.PopToRootAsync(false);
        }

        void dialog_AddMTU_okTapped(object sender, EventArgs e)
        {
            dialog_AddMTU.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            DoBasicRead();
        }

        private async void LogOffOkTapped(object sender, EventArgs e)
        {
            // Upload log files
            if (FormsApp.config.Global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();

            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            FormsApp.DoLogOff();

            background_scan_page.IsEnabled = true;

            Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogsSaved));
            //Navigation.PopToRootAsync(false);

        }

        private void LogOffNoTapped(object sender, EventArgs e)
        {
            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }



        private void AboutButtonTapped(object sender, EventArgs e)
        {
            InitLayout(1);
        }

        private void LogsButtonTapped(object sender, EventArgs e)
        {
            InitLayout(2);
        }

        private void SyncButtonTapped(object sender, EventArgs e)
        {
            InitLayout(3);
        }

        private void FtpButtonTapped(object sender, EventArgs e)
        {
            InitLayout(4);
        }

        private void InitLayout(int valor)
        {
            #region Customer name

            customers_copyr  .Text = TEXT_COPYR;
            customers_support.Text = TEXT_SUPPORT;
            customers_version.Text = TEXT_VERSION + Singleton.Get.Configuration.GetApplicationVersion () + ( ( Mobile.configData.HasIntune ) ? TEXT_INTUNE : string.Empty );
            config_version.Text = TEXT_CONFVER + SecureStorage.GetAsync("ConfigVersion").Result;

            if ( Mobile.configData.IsCertLoaded )
            {
                certificate_name.Text = $"Certificate: {Mobile.configData.certificate.Subject}";
                certificate_exp .Text = $"Expiration date: {Mobile.configData.certificate.NotAfter.ToString("MM/dd/yyyy hh:mm:ss")}";
            }
            else
            {
                certificate_name.Text = string.Empty;
                certificate_exp .Text = string.Empty;
            }

            /*
            #if __IOS__
            customers_version.Text = TEXT_VERSION + NSBundle.MainBundle
                                     .ObjectForInfoDictionary ( "CFBundleShortVersionString" ).ToString ();
            #elif __ANDROID__
            customers_version.Text = TEXT_VERSION + NSBundle.MainBundle
                                     .ObjectForInfoDictionary ( "CFBundleShortVersionString" ).ToString ();
            #endif
            */
            customers_name   .Text = TEXT_LICENSE + FormsApp.config.Global.CustomerName;

            #endregion
            about_block.Opacity = 0;
            logs_block.Opacity = 0;
            sync_block.Opacity = 0;
            ftp_block.Opacity = 0;

            switch (valor)
            {
                case 1:
                    about_block.IsVisible = true; logs_block.IsVisible = false; sync_block.IsVisible = false; ftp_block.IsVisible = false;
                    about_block.IsEnabled = true; logs_block.IsEnabled = false; sync_block.IsEnabled = false; ftp_block.IsEnabled = false;
                    about_button_text.Opacity = 1; about_button.Opacity = 1;
                    logs_button_text.Opacity = 0.5; logs_button.Opacity = 0.5;
                    sync_button_text.Opacity = 0.5; sync_button.Opacity = 0.5;
                    ftp_button_text.Opacity = 0.5; ftp_button.Opacity = 0.5;
                    title_text.Text = "About";
                    title_text.IsVisible = true;
                    img_barra.IsVisible = true;

                    about_block.FadeTo(1, 200);

                    break;

                case 2:

                    about_block.IsVisible = false; logs_block.IsVisible = true; sync_block.IsVisible = false; ftp_block.IsVisible = false;
                    about_block.IsEnabled = false; logs_block.IsEnabled = true; sync_block.IsEnabled = false; ftp_block.IsEnabled = false;
                    about_button_text.Opacity = 0.5; about_button.Opacity = 0.5;
                    logs_button_text.Opacity = 1; logs_button.Opacity = 1;
                    sync_button_text.Opacity = 0.5; sync_button.Opacity = 0.5;
                    ftp_button_text.Opacity = 0.5; ftp_button.Opacity = 0.5;
                    title_text.Text = "Activity Logs";
                    title_text.IsVisible = false;
                    img_barra.IsVisible = false;

                    logs_block.FadeTo(1, 200);

                    break;

                case 3:

                    about_block.IsVisible = false; logs_block.IsVisible = false; sync_block.IsVisible = true; ftp_block.IsVisible = false;
                    about_block.IsEnabled = false; logs_block.IsEnabled = false; sync_block.IsEnabled = true; ftp_block.IsEnabled = false;
                    about_button_text.Opacity = 0.5; about_button.Opacity = 0.5;
                    logs_button_text.Opacity = 0.5; logs_button.Opacity = 0.5;
                    sync_button_text.Opacity = 1; sync_button.Opacity = 1;
                    ftp_button_text.Opacity = 0.5; ftp_button.Opacity = 0.5;
                    title_text.Text = "File Syncronization";
                    title_text.IsVisible = true;
                    img_barra.IsVisible = true;

                    date_sync.Text = DateTime.Now.ToString();
                    updated_files.Text = GenericUtilsClass.NumFilesUploaded.ToString();
                   
                    pending_files.Text = GenericUtilsClass.NumLogFilesToUpload(Mobile.LogPath).ToString();
                    backup_files.Text = GenericUtilsClass.NumBackupFiles().ToString();
                    Color colorText = pending_files.TextColor;
                    if (int.Parse(backup_files.Text) >= 100)
                        colorText = Color.Red;
                  
                    lbl_backup.TextColor = colorText;
                    sync_block.FadeTo(1, 200);

                    break;

                case 4:
                

                    tbx_user_pass.Text = global.ftpPassword;
                    tbx_user_name.Text = global.ftpUserName;
                    tbx_remote_host.Text = global.ftpRemoteHost;
                    tbx_remote_path.Text = global.ftpRemotePath;

                    about_block.IsVisible = false; logs_block.IsVisible = false; sync_block.IsVisible = false; ftp_block.IsVisible = true;
                    about_block.IsEnabled = false; logs_block.IsEnabled = false; sync_block.IsEnabled = false; ftp_block.IsEnabled = true;
                    about_button_text.Opacity = 0.5; about_button.Opacity = 0.5;
                    logs_button_text.Opacity = 0.5; logs_button.Opacity = 0.5;
                    sync_button_text.Opacity = 0.5; sync_button.Opacity = 0.5;
                    ftp_button_text.Opacity = 1; ftp_button.Opacity = 1;
                    title_text.Text = "Upload FTP Settings";
                    title_text.IsVisible = true;
                    img_barra.IsVisible = true;
                    bool bHasInt = Mobile.IsNetAvailable();
                    btn_Save.IsEnabled = bHasInt;
                    btn_Test.IsEnabled = bHasInt;
                    ftp_block.FadeTo(1, 200);

                    break;
            }
        }

        private void ChangeLogFile(int index, bool bUI)
        {
            if (index < 0)
            {
                btnPrevious.IsVisible = false;
                btnNext.IsVisible = false;
                return;
            }
              

            if (bUI) Wait(true);
            Task.WaitAll(viewModelTabLog.LoadData(index));

            if (viewModelTabLog.IndexFile == 0) btnPrevious.IsVisible = false;
            else btnPrevious.IsVisible = true;

            if (viewModelTabLog.IndexFile == viewModelTabLog.TotalFiles) btnNext.IsVisible = false;
            else btnNext.IsVisible = true;

            if (Device.Idiom == TargetIdiom.Tablet)
                file_name.Text = $"Activity Log: {viewModelTabLog.FileDateTime}";
            else
                file_name.Text = viewModelTabLog.FileDateTime;

            if (bUI)  Wait(false);
        }

        void Previous_Clicked(object sender, System.EventArgs e)
        {
            ChangeLogFile(viewModelTabLog.IndexFile - 1,true);
        }

        void Next_Clicked(object sender, System.EventArgs e)
        {
            ChangeLogFile(viewModelTabLog.IndexFile + 1,true);
        }

        private void Wait(bool state)
        {
            backdark_bg.IsVisible = state;
            indicator.IsVisible = state;
            ContentNav.IsEnabled = !state;
        }

        private void FocusEntryFields()
        {
            this.tbx_remote_path.Focused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(true, (int)-120);
                else
                    SetLayoutPosition(true, (int)-100);
            };

            this.tbx_remote_path.Unfocused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(false, (int)-120);
                else
                    SetLayoutPosition(false, (int)-100);
            };
            this.tbx_user_name.Focused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(true, (int)-120);
                else
                    SetLayoutPosition(true, (int)-100);
            };

            this.tbx_user_name.Unfocused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(false, (int)-120);
                else
                    SetLayoutPosition(false, (int)-100);
            };

            this.tbx_user_pass.Focused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(true, (int)-120);
                else
                    SetLayoutPosition(true, (int)-100);
            };

            this.tbx_user_pass.Unfocused += (s, e) =>
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                    SetLayoutPosition(false, (int)-120);
                else
                    SetLayoutPosition(false, (int)-100);
            };

        }

        void SetLayoutPosition(bool onFocus, int value)
        {
            if (onFocus)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.ftp_block.TranslateTo(0, value, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.ftp_block.TranslateTo(0, value, 50);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.ftp_block.TranslateTo(0, 0, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.ftp_block.TranslateTo(0, 0, 50);
                }
            }
        }


        public async Task<bool> DownloadConfigProcess()
        {
            if (Mobile.configData.HasIntune)
            {
                if (Mobile.IsNetAvailable())
                {
                    GenericUtilsClass.DownloadConfigFiles(out string sFileCert);
                    if (!string.IsNullOrEmpty(sFileCert))
                        Mobile.configData.StoreCertificate(Mobile.configData.CreateCertificate(null, sFileCert));
                    NewConfigVersion = GenericUtilsClass.CheckFTPConfigVersion();
                    return true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Attention", "There is not connection at this moment, try again later", "OK");
                    return false;
                }
            }
            else if (Mobile.configData.HasFTP)
            {
                if (Mobile.IsNetAvailable())
                {
                    bool result = false;
                    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                    // Console.WriteLine($"------------------------------------FTP  Thread: {Thread.CurrentThread.ManagedThreadId}");
                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        //MainPage = new NavigationPage(new FtpDownloadSettings());
                        await Application.Current.MainPage.Navigation.PushAsync(new FtpDownloadSettings(tcs));
                        //PopupNavigation.Instance.PushAsync(new FtpDownloadSettings());

                        result = await tcs.Task;

                        if (result)
                        {
                            if (Configuration.CheckLoadXML())
                            {
                                await SecureStorage.SetAsync("ConfigVersion", GenericUtilsClass.CheckFTPConfigVersion());
                                await Application.Current.MainPage.DisplayAlert("Attention", "The application will end, restart it to make changes in the configuration", "ok");
                                //Errors.LogErrorNowAndKill(new ConfigFilesChangedException());
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
                            }
                            else
                            {
                                GenericUtilsClass.RestoreConfigFiles();
                                await Application.Current.MainPage.DisplayAlert("Attention", 
                                     "The new version configuration files are corrupted, the app will continues with the actual files. Contact your IT administratorn", "OK"); 
                            }
                        }
                        //return result;
                    });

                    return false; 
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Attention", "There is not connection at this moment, try again later", "OK");
                    return false;
                }
            }
            else
            {
                // Check if all configuration files are available in public folder
                bool HasPublicFiles = GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPublicPath);
                //this.abortMission = !this.HasDeviceAllXmls(Mobile.ConfigPublicPath);
                if (HasPublicFiles)
                {

                    bool CPD = false;
                    if (GenericUtilsClass.TagGlobal(true,"ConfigPublicDir", out dynamic value))
                    {
                        if (value != null)
                            bool.TryParse((string)value, out CPD);
                    }
                    GenericUtilsClass.CopyConfigFiles(!CPD, Mobile.ConfigPublicPath, Mobile.ConfigPath, out string sFileCert);
                    if (!string.IsNullOrEmpty(sFileCert))
                        Mobile.configData.StoreCertificate(Mobile.configData.CreateCertificate(null, sFileCert));

                    NewConfigVersion = GenericUtilsClass.CheckPubConfigVersion();
                    if (!GenericUtilsClass.HasDeviceAllXmls(Mobile.ConfigPath))
                        return false;
                    return true;

                }
                else
                    await Application.Current.MainPage.DisplayAlert("Attention", "There is not configuration files in public folder,copy files and try again", "OK");
                return false;

            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
