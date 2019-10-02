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
    public partial class AclaraViewReadMTU
    {
        private ActionType actionType;
        private ActionType actionTypeNew;
        private MenuView menuOptions;
        private DialogsView dialogView;
        private BottomBar bottomBar;
        private Global global;
        private List<ReadMTUItem> MTUDataListView { get; set; }
        private List<ReadMTUItem> FinalReadListView { get; set; }
       
        private bool _userTapped;
        private IUserDialogs dialogsSaved;

        public AclaraViewReadMTU()
        {
            InitializeComponent();
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

        private void ReadMTU(object sender, EventArgs e)
        {
            if (!_userTapped)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;
                    ContentNav.IsEnabled = false;
                    background_scan_page.IsEnabled = false;
                    ChangeLowerButtonImage(true);
                    _userTapped = true;

                    if (actionType == ActionType.ReadFabric)
                        Task.Factory.StartNew(ThreadProcedureMTUReadFabric);
                    else
                       Task.Factory.StartNew(ThreadProcedureMTUCOMMAction);
                });

            }
        }

    
        public AclaraViewReadMTU(IUserDialogs dialogs, ActionType page)
        {
            InitializeComponent();

            this.actionType = page;
            menuOptions = this.MenuOptions;
            dialogView = this.DialogView;
            bottomBar = this.BottomBar;


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
            this.global = Singleton.Get.Configuration.Global;
            dialogsSaved = dialogs;
          
            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar


            bottomBar.GetLabelElement("label_read").Text = "Push Button to START";

            _userTapped = false;
            
            TappedListeners();
 
            Task.Run(async () =>
            {
                await Task.Delay(250); Device.BeginInvokeOnMainThread(() =>
                {
                    ReadMTU(null, null);
                });
            });
        }

        private void TappedListeners()
        {
            bottomBar.GetImageButtonElement("btnTakePicture").Clicked += TakePicture;
            bottomBar.GetImageButtonElement("btnTakePicture").IsVisible = global.ShowCameraButton;
            TopBar.GetTGRElement("back_button")        .Tapped += ReturnToMainView;
            bottomBar.GetTGRElement("bg_action_button").Tapped += ReadMTU;

            dialogView.GetTGRElement("turnoffmtu_ok").Tapped += TurnOffMTUOkTapped;         
            dialogView.GetTGRElement("turnoffmtu_no").Tapped += TurnOffMTUNoTapped;
            dialogView.GetTGRElement("turnoffmtu_ok_close").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("replacemeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("replacemeter_cancel").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("meter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("meter_cancel").Tapped += dialog_cancelTapped;

            menuOptions.GetTGRElement("logout_button").Tapped += LogoutAsync;
            menuOptions.GetTGRElement("settings_button").Tapped += OpenSettingsView;

            dialogView.GetTGRElement("dialog_NoAction_ok").Tapped += dialog_cancelTapped;
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

        }

        private async void LogOffOkTapped(object sender, EventArgs e)
        {
            // Upload log files
            if (FormsApp.config.Global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();

            dialogView.OpenCloseDialog("dialog_logoff", false);
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            FormsApp.DoLogOff();

            background_scan_page.IsEnabled = true;

            Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogsSaved));

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

        private void LoadPhoneUI()
        {
            ContentNav.IsVisible = false;
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

        private void TurnOffMTUNoTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
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

        private async void LogoutAsync(object sender, EventArgs e)
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

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        public void OnMenuItemSelected(object sender, ItemTappedEventArgs e)
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
                ((ListView)sender).SelectedItem = null;
                try
                {
                    var item = (PageItem)e.Item;
                    ActionType page = item.TargetType;
                    ((ListView)sender).SelectedItem = null;

                    if (this.actionType != page)
                    {
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
            backdark_bg.IsVisible = true;
            indicator.IsVisible = true;
            //background_scan_page.IsEnabled = false;
            background_scan_page.Opacity = 1;

            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

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

            this.actionTypeNew = actionTarget;
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

        private async Task ThreadProcedureMTUCOMMAction()
        {
            //Create Ation when opening Form
            MTUComm.Action add_mtu = new MTUComm.Action (
                FormsApp.ble_interface,
                MTUComm.Action.ActionType.ReadMtu,
                FormsApp.credentialsService.UserName );
  
            add_mtu.OnProgress += ((s, e) =>
            {
                string mensaje = e.Message;
    
                Device.BeginInvokeOnMainThread ( () =>
                {
                    if ( ! string.IsNullOrEmpty ( mensaje ) )
                        bottomBar.GetLabelElement("label_read").Text = mensaje;
                });
            });

            add_mtu.OnFinish -= OnFinish;
            add_mtu.OnFinish += OnFinish;

            add_mtu.OnError  -= OnError;
            add_mtu.OnError  += OnError;

            await add_mtu.Run ();
        }

        public async Task OnFinish ( object sender, Delegates.ActionFinishArgs args )
        {
            FinalReadListView = new List<ReadMTUItem>();
 
            Mtu mtu = Singleton.Get.Configuration.GetMtuTypeById ( args.Mtu.Id );
            InterfaceParameters[] interfacesParams = FormsApp.config.getUserParamsFromInterface( mtu, ActionType.ReadMtu );
            
            Mtu currentMtu = Singleton.Get.Action.CurrentMtu;

            foreach (InterfaceParameters iParameter in interfacesParams)
            {
                // Port 1 or 2 log section
                if (iParameter.Name.Equals("Port"))
                {
                    ActionResult[] ports = args.Result.getPorts ();

                    for ( int i = 0; i < ports.Length; i++ )
                    {
                        foreach ( InterfaceParameters pParameter in iParameter.Parameters )
                        {
                            Parameter param = ports[i].getParameterByTag ( pParameter.Name, pParameter.Source, i );

                            // Port header
                            if (pParameter.Name.Equals("Description"))
                            {
                                string description;
                                
                                // For Read action when no Meter is installed on readed MTU
                                if ( param != null )
                                        description = param.Value;
                                else description = currentMtu.Ports[i].GetProperty ( pParameter.Name );
                                
                                FinalReadListView.Add(new ReadMTUItem()
                                {
                                    Title = "Here lies the Port title...",
                                    isDisplayed = "true",
                                    Height = "40",
                                    isMTU = "false",
                                    isMeter = "true",
                                    Description = "Port " + ( i + 1 ) + ": " + description
                                });
                            }
                            // Port fields
                            else
                            {
                                if ( param != null )
                                    FinalReadListView.Add(new ReadMTUItem()
                                    {
                                        Title = param.getLogDisplay() + ":",
                                        isDisplayed = "true",
                                        Height = "70",
                                        isMTU = "false",
                                        isDetailMeter = "true",
                                        isMeter = "false",
                                        Description = param.Value
                                    });
                            }
                        }
                    }
                }

                // Root log fields
                else
                {
                    Parameter param = args.Result.getParameterByTag ( iParameter.Name, iParameter.Source, 0 );

                    if (param != null)
                    {
                        FinalReadListView.Add(new ReadMTUItem()
                        {
                            Title = param.getLogDisplay() + ":",
                            isDisplayed = "true",
                            Height = "64",
                            isMTU = "true",
                            isMeter = "false",
                            Description = param.Value
                        });
                    }
                }
            }

            await Task.Delay(100).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                bottomBar.GetLabelElement("label_read").Text = "Successful MTU read";
                this.EnableInteraction ();
            }));
        }

        public void OnError ()
        {
            Error error = Errors.LastError;
            
            Task.Delay(100).ContinueWith(t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    MTUDataListView          = new List<ReadMTUItem> { };
                    FinalReadListView        = new List<ReadMTUItem> { };
                    bottomBar.GetLabelElement("label_read").Text          = error.MessageFooter;
                    listaMTUread.ItemsSource = FinalReadListView;
                    this.EnableInteraction ();
                }));
        }
        
        private void EnableInteraction ()
        {
            ChangeLowerButtonImage ( false );
            background_scan_page.IsEnabled = true;
            bottomBar.GetTGRElement("bg_action_button").NumberOfTapsRequired = 1;
            listaMTUread.ItemsSource = FinalReadListView;
            _userTapped            = false;
            backdark_bg.IsVisible  = false;
            ContentNav.IsEnabled   = true;
            indicator.IsVisible    = false;
            listaMTUread.IsVisible = true;
        }


        private async Task ThreadProcedureMTUReadFabric ()
        {
            //Create Ation when opening Form
            MTUComm.Action add_mtu = new MTUComm.Action (
                FormsApp.ble_interface,
                actionType,
                FormsApp.credentialsService.UserName );

            add_mtu.OnProgress += ((s, e) =>
            {
                string mensaje = e.Message;

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!string.IsNullOrEmpty(mensaje))
                        bottomBar.GetLabelElement("label_read").Text = mensaje;
                });
            });

            add_mtu.OnFinish -= Fabric_OnFinish;
            add_mtu.OnFinish += Fabric_OnFinish;

            add_mtu.OnError  -= Fabric_OnError;
            add_mtu.OnError  += Fabric_OnError;

            await add_mtu.Run ();
        }

        public async Task Fabric_OnFinish ( object sender, Delegates.ActionFinishArgs args )
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                EnableInteraction();
            });
        }

        public void Fabric_OnError ()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                EnableInteraction();
            });
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
    }
}
