// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using aclara_meters.Models;
using Xamarin.Forms;
using aclara.ViewModels;
using MTUComm;
using Library.Exceptions;
using Xml;

using ActionType = MTUComm.Action.ActionType;
using ValidationResult = MTUComm.MTUComm.ValidationResult;
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

        private MenuView menuOptions;
        private DialogsView dialogView;


      
        private string NewConfigVersion;

        Global global;

        public AclaraViewSettings()
        {
            InitializeComponent();
        }

 
        readonly bool notConnected;

        public AclaraViewSettings(bool notConnected)
        {
            InitializeComponent();

            viewModelTabLog  = new TabLogViewModel();
            BindingContext = viewModelTabLog;

            this.notConnected = notConnected;

            menuOptions = this.MenuOptions;
            dialogView = this.DialogView;

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

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar
   
            menuOptions.GetListElement("navigationDrawerList").IsEnabled = false;
            menuOptions.GetListElement("navigationDrawerList").Opacity = 0.65;
                               
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
                    TopBar.GetImageElement("battery_level").Opacity = 0;
                    TopBar.GetImageElement("rssi_level").Opacity = 0;
                });
            });
        }

        public AclaraViewSettings(IUserDialogs dialogs)
        {
            InitializeComponent();

            viewModelTabLog = new TabLogViewModel();
            BindingContext = viewModelTabLog;


            menuOptions = this.MenuOptions;
            dialogView = this.DialogView;

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
  
            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            
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

        }

        private void LoadPhoneUIConnected()
        {
            background_scan_page.Margin = new Thickness(0, 0, 0, 0);
   
            sync_block.Scale = 0.9;
            logs_block.Scale = 1;
            about_block.Scale = 1;
         
            file_name.FontSize = 20;
        }

        private void LoadTabletUIConnected()
        {
            ContentNav.IsVisible = true;
            background_scan_page.Opacity = 1;
           
            background_scan_page.Margin = new Thickness(310, 0, 0, 0);
            sync_block.Scale = 1;
            logs_block.Scale = 1;
            about_block.Scale = 1;
           
            lowerbar.TranslationX = 310;
            lowerbar.Margin = new Thickness(0, 0, 310, 0);
            shadoweffect.IsVisible = true;

            shadoweffect.Source = "shadow_effect_tablet";
        }

        private void TurnOffMTUOKTapped(object sender, EventArgs e)
        {
            CallLoadViewTurnOff();
        }

        private async void LogoutTapped(object sender, EventArgs e)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                dialogView.CloseDialogs();
                dialogView.OpenCloseDialog("dialog_logoff", true);
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
                if (sender is ListView lv) lv.SelectedItem = null;
                try
                {
                    var item = (PageItem)e.Item;
                    ActionType page = item.TargetType;
                    ((ListView)sender).SelectedItem = null;
                    this.actionType = page;
                    NavigationController(page);
 
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

        private async Task NavigationController (
            ActionType actionTarget )
        {
            background_scan_page.Opacity = 1;

            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

            backdark_bg.IsVisible = true;
            indicator.IsVisible = true;
            background_scan_page.IsEnabled = false;

            switch ( await base.ValidateNavigation ( actionTarget ) )
            {
                case ValidationResult.EXCEPTION:
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                    background_scan_page.IsEnabled = true;
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

                            this.actionType = actionTarget;
                            this.GoToPage();
                        })
                    );

                    #endregion

                    break;

                case ActionType.ReadMtu:
                    #region ReadMTU  
                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved, actionTarget), false);

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

                            if (this.global.ActionVerify)
                            {
                                dialog_open_bg.IsVisible = true;
                                turnoff_mtu_background.IsVisible = true;
                                dialogView.OpenCloseDialog("dialog_turnoff_one", true);
                            }
                            else
                            {
                                this.actionType = actionTarget;
                                CallLoadViewTurnOff();
                            }
                            #endregion

                        })
                    );

                    #endregion

                    break;

                case ActionType.MtuInstallationConfirmation:

                    #region Install Confirm Controller

                    this.actionType = actionTarget;

                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {

                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewInstallConfirmation(dialogsSaved), false);

                        })
                    );

                    #endregion

                    break;

                case ActionType.AddMtu:
                    #region AddMTU
                    await ControllerAction(actionTarget, "dialog_AddMTU");


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
                    if (this.global.ActionVerify)
                    {
                        dialog_open_bg.IsVisible = true;
                        turnoff_mtu_background.IsVisible = true;
                        dialogView.GetStackLayoutElement(nameDialog).IsVisible = true;
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

        async Task BasicReadThread()
        {
            MTUComm.Action basicRead = new MTUComm.Action (
               FormsApp.ble_interface,
               MTUComm.Action.ActionType.BasicRead,
               FormsApp.credentialsService.UserName );

            basicRead.OnFinish -= BasicRead_OnFinish;
            basicRead.OnFinish += BasicRead_OnFinish;

            basicRead.OnError  -= BasicRead_OnError;
            basicRead.OnError  += BasicRead_OnError;

            Device.BeginInvokeOnMainThread(() =>
            {
                #region New Circular Progress bar Animations    

                backdark_bg.IsVisible = true;
                indicator.IsVisible = true;
                background_scan_page.IsEnabled = false;

                #endregion

            });

            await basicRead.Run();
        }

        public async Task BasicRead_OnFinish ( object sender, Delegates.ActionFinishArgs args )
        {
            await Task.Delay(100).ContinueWith(t =>
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
        }

        public void BasicRead_OnError ()
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
            TopBar.GetTGRElement("back_button").Tapped += ReturnToMainView;
         
            btn_Test.Clicked += Btn_Test_Clicked;
            btn_Save.Clicked += Btn_Save_Clicked;
            btn_Cancel.Clicked += Btn_Cancel_Clicked;
            
            btn_DownloadConf.Clicked += Btn_DownloadConf_Clicked;

            TabPrevious.Tapped += Previous_Clicked;
            TabNext.Tapped += Next_Clicked;

            dialogView.GetTGRElement("turnoffmtu_ok").Tapped += TurnOffMTUOKTapped;
            dialogView.GetTGRElement("turnoffmtu_no").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("turnoffmtu_ok_close").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("replacemeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("replacemeter_cancel").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("meter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("meter_cancel").Tapped += dialog_cancelTapped;

            menuOptions.GetTGRElement("logout_button").Tapped += LogoutTapped;
            dialogView.GetTGRElement("dialog_NoAction_ok").Tapped += dialog_cancelTapped;
            menuOptions.GetListElement("navigationDrawerList").ItemTapped += OnMenuItemSelected;
            
            dialogView.GetTGRElement("logoff_no").Tapped += LogOffNoTapped;
            dialogView.GetTGRElement("logoff_ok").Tapped += LogOffOkTapped;

            dialogView.GetTGRElement("dialog_AddMTUAddMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTUAddMeter_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_AddMTUReplaceMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTUReplaceMeter_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_ReplaceMTUReplaceMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_ReplaceMTUReplaceMeter_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_AddMTU_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTU_cancel").Tapped += dialog_cancelTapped;

        }

        void dialog_cancelTapped(object sender, EventArgs e)
        {
            Label obj = (Label)sender;
            StackLayout parent = (StackLayout)obj.Parent;
            StackLayout dialog = (StackLayout)parent.Parent;
            dialog.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
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
            backdark_bg.IsVisible = false;
            indicator.IsVisible = false;
            background_scan_page.IsEnabled = true;

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

        private async void LogOffOkTapped(object sender, EventArgs e)
        {
            // Upload log files
            if (FormsApp.config.Global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();

            dialogView.GetStackLayoutElement("dialog_logoff").IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            FormsApp.DoLogOff();

            background_scan_page.IsEnabled = true;

            Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogsSaved));
 
        }

        private void LogOffNoTapped(object sender, EventArgs e)
        {
            dialogView.GetStackLayoutElement("dialog_logoff").IsVisible = false;
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
            config_version   .Text = TEXT_CONFVER + SecureStorage.GetAsync("ConfigVersion").Result;

            if ( Mobile.configData.IsCertLoaded )
            {
                certificate_name.Text = "Certificate: " + Mobile.configData.certificate.Subject;
                certificate_exp .Text = $"Expiration date: " + Mobile.configData.certificate.NotAfter.ToString ( "MM/dd/yyyy hh:mm:ss" );
            }
            else
            {
                certificate_name.Text = string.Empty;
                certificate_exp .Text = string.Empty;
            }

            customers_name.Text = TEXT_LICENSE + FormsApp.config.Global.CustomerName;

            #endregion
            about_block.Opacity = 0;
            logs_block.Opacity = 0;
            sync_block.Opacity = 0;
            ftp_block.Opacity = 0;
            TitleGen.IsVisible = true;
            TitleLog.IsVisible = false;

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
                    TitleGen.IsVisible = false;
                    TitleLog.IsVisible = true;

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
                    TaskCompletionSource<bool> taskSemaphoreDownload = new TaskCompletionSource<bool>();
               
                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        await Application.Current.MainPage.Navigation.PushAsync(new FtpDownloadSettings(taskSemaphoreDownload));

                        result = await taskSemaphoreDownload.Task;

                        if (result)
                        {
                            if (Configuration.CheckLoadXML())
                            {
                                await SecureStorage.SetAsync("ConfigVersion", GenericUtilsClass.CheckFTPConfigVersion());
                                await Application.Current.MainPage.DisplayAlert("Attention", "The application will end, restart it to make changes in the configuration", "ok");
                                
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
                            }
                            else
                            {
                                GenericUtilsClass.RestoreConfigFiles();
                                await Application.Current.MainPage.DisplayAlert("Attention", 
                                     "The new version configuration files are corrupted, the app will continues with the actual files. Contact your IT administratorn", "OK"); 
                            }
                        }

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
