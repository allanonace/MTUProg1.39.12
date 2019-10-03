// Copyright M. Griffie <nexus@nexussays.com>
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using aclara_meters.Models;
using aclara_meters.util;
using Acr.UserDialogs;
using Library;
using Library.Exceptions;
using MTUComm;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xml;

using ActionType = MTUComm.Action.ActionType;
using ValidationResult = MTUComm.MTUComm.ValidationResult;

namespace aclara_meters.view
{
    public partial class AclaraViewInstallConfirmation
    {
        private ActionType actionType;
        private ActionType actionTypeNew;
        private MenuView menuOptions;
        private DialogsView dialogView;
        private BottomBar bottomBar;
        private Global global;

        private List<ReadMTUItem> MTUDataListView { get; set; }

        private List<ReadMTUItem> FinalReadListView { get; set; }


        private List<PageItem> MenuList { get; set; }
        private bool _userTapped;
        private IUserDialogs dialogsSaved;

        public AclaraViewInstallConfirmation()
        {
            InitializeComponent();
        }

        public AclaraViewInstallConfirmation(IUserDialogs dialogs)
        {
            InitializeComponent();

            this.actionType = ActionType.MtuInstallationConfirmation;

            menuOptions = this.MenuOptions;
            dialogView = this.DialogView;
            bottomBar = this.BottomBar;

            this.global = Singleton.Get.Configuration.Global;

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                    ChangeLowerButtonImage(false);
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
   

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar


            bottomBar.GetLabelElement("label_read").Text = "Push Button to START";

            _userTapped = false;
            
            TappedListeners();

        }


        private void OpenSettingsView(object sender, EventArgs e)
        {
           
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
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        #region New Circular Progress bar Animations    


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

        private void ChangeLowerButtonImage(bool v)
        {
            if (v)
            {
                bottomBar.GetImageElement("bg_action_button_img").Source = "read_mtu_btn_black.png";
            }
            else
            {
                bottomBar.GetImageElement("bg_action_button_img").Source = "read_mtu_btn.png";
            }
        }

        private void InstallConfirmation(object sender, EventArgs e)
        {
            if (!_userTapped)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;
                    background_scan_page.IsEnabled = false;
                    ContentNav.IsEnabled = false;
                    ChangeLowerButtonImage(true);
                    _userTapped = true;
                    bottomBar.GetLabelElement("label_read").Text = "Reading from MTU ... ";


                    Task.Factory.StartNew(ThreadProcedureMTUCOMMAction);


                });




            }
        }

        private async Task ThreadProcedureMTUCOMMAction()
        {
            //Create Ation when opening Form
            MTUComm.Action add_mtu = new MTUComm.Action (
                FormsApp.ble_interface,
                MTUComm.Action.ActionType.MtuInstallationConfirmation,
                FormsApp.credentialsService.UserName );

            add_mtu.OnProgress += ((s, e) =>
            {
                string mensaje = e.Message;

                Device.BeginInvokeOnMainThread(() =>
                {
                    bottomBar.GetLabelElement("label_read").Text = mensaje;
                });
            });

            add_mtu.OnFinish -= OnFinish;
            add_mtu.OnFinish += OnFinish;

            add_mtu.OnError -= OnError;
            add_mtu.OnError += OnError;

            await add_mtu.Run();
        }

        public async Task OnFinish ( object sender, Delegates.ActionFinishArgs args )
        {
            Utils.Print("Action Succefull");
            Utils.Print("Press Key to Exit");
            
            FinalReadListView = new List<ReadMTUItem>(); // Saves the data to view

            Parameter[] paramResult = args.Result.getParameters();

            int mtu_type = 0;

            foreach (Parameter p in paramResult)
            {
                try
                {
                    if (p.CustomParameter.Equals("MtuType"))
                        mtu_type = Int32.Parse(p.Value.ToString());
                }
                catch (Exception e5)
                {
                    Utils.Print(e5.StackTrace);
                }
            }

            Mtu mtu = Singleton.Get.Configuration.GetMtuTypeById ( mtu_type );

            string ndresult = string.Empty;
            InterfaceParameters[] interfacesParams = FormsApp.config.getUserParamsFromInterface ( mtu, ActionType.ReadMtu );

            try
            {

            foreach (InterfaceParameters parameter in interfacesParams)
            {
                if (parameter.Name.Equals("Port"))
                {
                    ActionResult[] ports = args.Result.getPorts(); 

                    for (int i = 0; i < ports.Length; i++)
                    {
                        foreach (InterfaceParameters port_parameter in parameter.Parameters)
                        {
                            Parameter param = ports[i].getParameterByTag ( port_parameter.Name, port_parameter.Source, i );

                            if (port_parameter.Name.Equals("Description"))
                            {
                                string description;

                                // For Read action when no Meter is installed on readed MTU
                                if ( param != null )
                                     description = param.Value;
                                else description = mtu.Ports[i].GetProperty(port_parameter.Name);

                                FinalReadListView.Add(new ReadMTUItem()
                                {
                                    Title       = "Here lies the Port title...",
                                    isDisplayed = "true",
                                    Height      = "40",
                                    isMTU       = "false",
                                    isMeter     = "true",
                                    Description = "Port " + (i+1) + ": " + description
                                });
                            }
                            else
                            {
                                if (param != null)
                                {
                                    FinalReadListView.Add(new ReadMTUItem()
                                    {
                                        Title         = param.getLogDisplay() + ":",
                                        isDisplayed   = "true",
                                        Height        = "70",
                                        isMTU         = "false",
                                        isDetailMeter = "true",
                                        isMeter       = "false",
                                        Description   = param.Value
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Parameter param = args.Result.getParameterByTag ( parameter.Name, parameter.Source, 0 );

                    if ( param != null )
                    {
                        string bgcolorEntry = string.Empty;
                        string fcolorEntry  = string.Empty;

                        // Custom colors for Node Discovery result
                        if ( param.CustomParameter.Equals ( "NodeDiscoveryResult" ) )
                        {
                            ndresult = param.Value.ToString ().Split ( ' ' )[ 0 ].ToLower ();
                            switch ( ndresult )
                            {
                                case "fail"    : bgcolorEntry = COLOR_BG_ND_FAIL;     fcolorEntry = COLOR_FONT_ND_FAIL;     break;
                                case "good"    : bgcolorEntry = COLOR_BG_ND_GOOD;     fcolorEntry = COLOR_FONT_ND_GOOD;     break;
                                case "excelent": bgcolorEntry = COLOR_BG_ND_EXCELENT; fcolorEntry = COLOR_FONT_ND_EXCELENT; break;
                            }

                            FinalReadListView.Add(new ReadMTUItem()
                            {
                                Title           = param.getLogDisplay() + ":",
                                isDisplayed     = "true",
                                Height          = "64",
                                isMTU           = "true",
                                isMeter         = "false",
                                Description     = param.Value,
                                BackgroundColor = bgcolorEntry,
                                FontColor       = fcolorEntry,
                            });
                        }
                        else
                        {
                            // Uses default colors for text and background
                            FinalReadListView.Add(new ReadMTUItem()
                            {
                                Title           = param.getLogDisplay() + ":",
                                isDisplayed     = "true",
                                Height          = "64",
                                isMTU           = "true",
                                isMeter         = "false",
                                Description     = param.Value,
                            });
                        }
                    }
                }
            }

            }
            catch ( Exception e )
            {

            }

            // Get result of the Install Confirmation process
            bool ok = false;
            foreach ( Parameter parameter in paramResult )
            {
                if ( parameter.getLogTag ().Equals ( "InstallationConfirmationStatus" ) &&
                     ! string.IsNullOrEmpty ( parameter.Value ) &&
                     ! string.Equals ( parameter.Value.ToUpper (), "NOT CONFIRMED" ) )
                {
                    ok = true;
                    break;
                }
            }
            
            string resultMsg = ( ! ok ) ? "Unsuccessful Installation" : "Successful Installation";
            
            await Task.Delay(100).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                if ( ! string.IsNullOrEmpty ( ndresult ) )
                {
                    Image imgNdResult     = bottomBar.GetImageElement ( "img_ndresult" );
                    imgNdResult.Source    = "nd_" + ndresult;
                    imgNdResult.IsVisible = true;
                }

                listaMTUread.ItemsSource = FinalReadListView;
                bottomBar.GetLabelElement("label_read").Text = resultMsg;
                _userTapped = false;
                bottomBar.GetTGRElement("bg_action_button").NumberOfTapsRequired = 1;
                ChangeLowerButtonImage(false);
                ContentNav.IsEnabled = true;
                backdark_bg.IsVisible = false;
                indicator.IsVisible = false;
                background_scan_page.IsEnabled = true;
            }));
        }

        public void OnError ()
        {
            Error error = Errors.LastError;

            Task.Delay(100).ContinueWith(t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    MTUDataListView = new List<ReadMTUItem> { };
                    FinalReadListView = new List<ReadMTUItem> { };
                    listaMTUread.ItemsSource = FinalReadListView;
                    bottomBar.GetLabelElement("label_read").Text = error.MessageFooter;
                    _userTapped = false;
                    bottomBar.GetTGRElement("bg_action_button").NumberOfTapsRequired = 1;
                    ChangeLowerButtonImage(false);
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                    background_scan_page.IsEnabled = true;
                    ContentNav.IsEnabled = true;
                })
            );
        }

        private void TappedListeners()
        {
            bottomBar.GetImageButtonElement("btnTakePicture").Clicked += TakePicture;
            bottomBar.GetImageButtonElement("btnTakePicture").IsVisible = global.ShowCameraButton;
            TopBar.GetTGRElement("back_button").Tapped += ReturnToMainView;
            bottomBar.GetTGRElement("bg_action_button").Tapped += InstallConfirmation;


            dialogView.GetTGRElement("turnoffmtu_ok").Tapped += TurnOffMTUOkTapped;
            dialogView.GetTGRElement("turnoffmtu_no").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("turnoffmtu_ok_close").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("replacemeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("replacemeter_cancel").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("meter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("meter_cancel").Tapped += dialog_cancelTapped;

            menuOptions.GetTGRElement("logout_button").Tapped += LogoutTapped;
            menuOptions.GetTGRElement("settings_button").Tapped += OpenSettingsView;

            dialogView.GetTGRElement("dialog_NoAction_ok").Tapped += dialog_cancelTapped;
            menuOptions.GetListElement("navigationDrawerList").ItemTapped += OnMenuItemSelected;


            dialogView.GetTGRElement("logoff_no").Tapped += LogOffNoTapped;
            dialogView.GetTGRElement("logoff_ok").Tapped += LogOffOkTapped;

            dialogView.GetTGRElement("dialog_AddMTU_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTU_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_AddMTUAddMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTUAddMeter_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_AddMTUReplaceMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTUReplaceMeter_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_ReplaceMTUReplaceMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_ReplaceMTUReplaceMeter_cancel").Tapped += dialog_cancelTapped;

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
            this.actionType = this.actionTypeNew;
            
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

            Application.Current.MainPage = new NavigationPage(new AclaraViewLogin ( dialogsSaved ));
           
        }

        private void LogOffNoTapped(object sender, EventArgs e)
        {
            dialogView.GetStackLayoutElement("dialog_logoff").IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void LoadPhoneUI()
        {
            background_scan_page.Margin = new Thickness(0, 0, 0, 0);
  
        }

        private void LoadTabletUI()
        {
            ContentNav.IsVisible = true;
            background_scan_page.Opacity = 1;

                        background_scan_page.Margin = new Thickness(310, 0, 0, 0);
            
            shadoweffect.IsVisible = true;
            shadoweffect.Source = "shadow_effect_tablet";
        }

        private void TurnOffMTUOkTapped(object sender, EventArgs e)
        {
            dialogView.OpenCloseDialog("dialog_turnoff_one", false);
            dialogView.OpenCloseDialog("dialog_turnoff_two", true);

            Task.Factory.StartNew(TurnOffMethod);
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
            Application.Current.MainPage.Navigation.PopToRootAsync(false);
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

                    if (this.actionType != page)
                    {
                        this.actionTypeNew = page;
                        NavigationController(page);
                    }

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

                            this.actionType = this.actionTypeNew;
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
                                this.actionType = this.actionTypeNew;
                                CallLoadViewTurnOff();
                            }
                            #endregion

                        })
                    );

                    #endregion

                    break;

                case ActionType.MtuInstallationConfirmation:

                    #region Install Confirm Controller

                    this.actionType = this.actionTypeNew;

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

        private async void TakePicture(object sender, EventArgs e)
        {
            try
            {
                
                string port; 

                int mtuIdLength = Singleton.Get.Configuration.Global.MtuIdLength;
                var MtuId = await Data.Get.MemoryMap.MtuSerialNumber.GetValue();
                var accName1 = await Data.Get.MemoryMap.P1MeterId.GetValue();
                var accName2 = await Data.Get.MemoryMap.P2MeterId.GetValue();

                string sTick = DateTime.Now.Ticks.ToString();

                if (accName2 != 0)
                {
                    bool bResp = await DisplayAlert("Select port", "Select the port for the picture", "Port 1", "Port 2");
                    port = bResp == true ? "1" : "2";
                }
                else
                    port = "1";

                accName1 = port == "1" ? accName1 : accName2;
                string nameFile = MtuId.ToString().PadLeft(mtuIdLength, '0') + "_" + accName1 + sTick + "_Port" + port;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    MediaFile file = await PictureService.TakePictureService(nameFile);

                    if (file == null)
                        return;

                    string[] fileName = file.Path.Split('/');
                    nameFile = fileName[fileName.Length - 1];
                    DirectoryInfo dir = new DirectoryInfo(file.Path.Substring(0, file.Path.Length - (nameFile.Length + 1)));

                    FileInfo[] imagefiles = dir.GetFiles(nameFile);

                    imagefiles[0].CopyTo(Path.Combine(Mobile.ImagesPath, nameFile));
                    imagefiles[0].Delete();

                    file.Dispose();
                });

            }
            catch (Exception ex)
            {
                await Errors.ShowAlert(new CameraException(ex.Message));
            }

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
