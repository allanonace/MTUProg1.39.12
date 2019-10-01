using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using aclara_meters.Helpers;
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
using MTUStatus = MTUComm.Action.MTUStatus;

namespace aclara_meters.view
{
    public partial class AclaraViewRemoteDisconnect
    {

        private const string LB_PORT1 = "Valve";

        private IUserDialogs dialogsSaved;
        private bool _userTapped;


        #region Attributes

        private Configuration config;
        private MTUComm.Action remote_disconnect;
        private Mtu currentMtu;
        private Global global;
        private MenuView menuOptions;
        private DialogsView dialogView;
        private BottomBar bottomBar;

        private List<ReadMTUItem> FinalReadListView { get; set; }

        private ActionType actionType;
        private ActionType actionTypeNew;
       
        private bool isCancellable;
        private bool isLogout;
        private bool isReturn;
        private bool isSettings;

        #endregion

        #region Initialization

        public AclaraViewRemoteDisconnect()
        {
            InitializeComponent ();
        }

        public AclaraViewRemoteDisconnect ( IUserDialogs dialogs, ActionType page )
        {
            InitializeComponent ();
            
            Device.BeginInvokeOnMainThread ( () =>
            {
                  backdark_bg.IsVisible = true;
                  indicator  .IsVisible = true;
                  background_scan_page.IsEnabled = false;
            });
            
            this.global     = Singleton.Get.Configuration.Global;
            this.actionType = page;

            menuOptions = this.MenuOptions;
            dialogView = this.DialogView;
            bottomBar = this.BottomBar;

            dialogsSaved = dialogs;

            this.config = Singleton.Get.Configuration;
         
            this.remote_disconnect = new MTUComm.Action (
                FormsApp.ble_interface,
                this.actionType,
                FormsApp.credentialsService.UserName );
            
            isCancellable = true;

            Device.BeginInvokeOnMainThread(() =>
            {
                string[] texts = MTUComm.Action.actionsTexts[ this.actionType ];
            
                name_of_window_port1  .Text   = texts[ 0 ] + " - " + LB_PORT1;
                
                bottomBar.GetImageElement("bg_action_button_img").Source = texts[ 3 ];

                bottomBar.GetLabelElement("label_read").Opacity    = 1;
                                
                if ( Device.Idiom == TargetIdiom.Tablet )
                     LoadTabletUI ();
                else LoadPhoneUI ();
                
                NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar
                
  
            });

            _userTapped = false;
            
            TappedListeners ();
            InitializeLowerbarLabel();

            Task.Run(async () => await InitializeValveOperationForm())
                .ContinueWith ( t =>
                Device.BeginInvokeOnMainThread ( () =>
                {
                    Task.Delay ( 100 )
                    .ContinueWith ( t0 =>
                        Device.BeginInvokeOnMainThread ( () =>
                        {
                            bottomBar.GetLabelElement("label_read").Opacity = 1;
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            background_scan_page.IsEnabled = true;
                            bottomBar.GetLabelElement("label_read").Text = "Press Button to Start";
                   
                        })
                    );
                })
            );
               

        }
        

        private async Task InitializeValveOperationForm ()
        {
            #region Initialize data

            this.div_MtuId.IsVisible=true;
            this.div_Mtu_Status.IsVisible=true;


            List<string> list = new List<string> ()
            {
                "Close", "Open", "Partial Open"
            };
           
            //Now I am given ItemsSorce to the Pickers
            pck_ValvePosition.ItemsSource   = list;
             
            #endregion
            

            #region Labels

            // Account Number
            this.lb_AccountNumber.Text = global.AccountLabel;      
            
            this.port1label.Text = LB_PORT1;
                 
            #endregion
            int mtuIdLength = Singleton.Get.Configuration.Global.MtuIdLength;
            var MtuId       = await Data.Get.MemoryMap.MtuSerialNumber.GetValue();
            var MtuStatus   = await Data.Get.MemoryMap.MtuStatus.GetValue();
            var accName     = await  Data.Get.MemoryMap.P1MeterId.GetValue();

            Device.BeginInvokeOnMainThread(()=>{
            this.tbx_MtuId.Text         = MtuId.ToString().PadLeft ( mtuIdLength, '0' );
            this.tbx_Mtu_Status.Text    = MtuStatus;
            this.tbx_AccountNumber.Text = accName.ToString();
            });
        }

        #endregion

        #region Status message

        public void SetUserInterfaceMTUStatus(string statusMsg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                bottomBar.GetLabelElement("label_read").Text = statusMsg;
            });
        }

        public string GetStringFromMTUStatus(MTUStatus mtuStatus, int time)
        {
            switch (mtuStatus)
            {
                case MTUStatus.ReadingMtuShortTime:
                    return "Reading MTU in " + time + " seconds...";

                case MTUStatus.ReadingMtuData:
                    return "Reading MTU data...";

                case MTUStatus.Autodetect:
                    return "Autodetect...";

                case MTUStatus.CheckingEnconderLongTime:
                    return "Checking Encoder... " + time;

                case MTUStatus.ProgramingMtuShortTime:
                    return "Programming MTU in " + time + " seconds...";

                case MTUStatus.PreparingToProgram:
                    return "Preparing to program...";

                case MTUStatus.TurningOffMtu:
                    return "Turning off MTU...";

                case MTUStatus.ReadingMtuAgain:
                    return "Reading MTU again...";

                case MTUStatus.ProgramingMtu:
                    return "Programming MTU...";

                case MTUStatus.VerifyingMtuData:
                    return "Verifying Mtu Data...";

                case MTUStatus.CheckingEnconderShortTime:
                    return "Checking Encoder... " + time;

                case MTUStatus.TurningOnMtu:
                    return "Autodetect...";

                case MTUStatus.ReadingMtu:
                    return "Reading MTU...";

            }

            return "Error Detected";
        }

        #endregion

        #region GUI Initialization

        private void TappedListeners ()
        {
            bottomBar.GetImageButtonElement("btnTakePicture").Clicked += TakePicture;
            bottomBar.GetImageButtonElement("btnTakePicture").IsVisible = global.ShowCameraButton;
            TopBar.GetTGRElement("back_button").Tapped += ReturnToMainView;
            bottomBar.GetTGRElement("bg_action_button").Tapped += ValveOperationCommand;

            dialogView.GetTGRElement("turnoffmtu_ok").Tapped += TurnOffMTUOkTapped;
            dialogView.GetTGRElement("turnoffmtu_no").Tapped += TurnOffMTUNoTapped;
            dialogView.GetTGRElement("turnoffmtu_ok_close").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("replacemeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("replacemeter_cancel").Tapped += dialog_cancelTapped;
            dialogView.GetTGRElement("meter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("meter_cancel").Tapped += dialog_cancelTapped;

            menuOptions.GetTGRElement("logout_button").Tapped += LogoutTapped;
            menuOptions.GetTGRElement("settings_button").Tapped += OpenSettingsCallAsync;


            menuOptions.GetListElement("navigationDrawerList").ItemTapped += OnMenuItemSelected;
            dialogView.GetTGRElement("dialog_NoAction_ok").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("logoff_no").Tapped += Confirm_No_LogOut;
            dialogView.GetTGRElement("logoff_ok").Tapped += Confirm_Yes_LogOut;

            dialogView.GetTGRElement("dialog_AddMTUAddMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTUAddMeter_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_AddMTUReplaceMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTUReplaceMeter_cancel").Tapped += dialog_cancelTapped;

            dialogView.GetTGRElement("dialog_ReplaceMTUReplaceMeter_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_ReplaceMTUReplaceMeter_cancel").Tapped += dialog_cancelTapped;


            dialogView.GetTGRElement("dialog_AddMTU_ok").Tapped += dialog_OKBasicTapped;
            dialogView.GetTGRElement("dialog_AddMTU_cancel").Tapped += dialog_cancelTapped;
 
        }

        #region Dialogs

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

        private async void Confirm_Yes_LogOut ( object sender, EventArgs e )
        {
            // Upload log files
            if (global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();

            dialogView.GetStackLayoutElement("dialog_logoff").IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                //REASON
                if (!isCancellable)
                {
                    isLogout = true;
                    dialog_open_bg.IsVisible = true;
               
                }
                else DoLogoff();
            });
           
        }

        private void Confirm_No_LogOut ( object sender, EventArgs e )
        {
            dialogView.GetStackLayoutElement("dialog_logoff").IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            
        }

        #endregion

        private void InitializeLowerbarLabel ()
        {
            bottomBar.GetLabelElement("label_read").Text = "Push Button to START";
        }

        #region Phone/Tablet

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

        #endregion

        #endregion

        #region GUI Handlers

        #region Menu options

        private void OnItemSelected(Object sender, SelectedItemChangedEventArgs e )
        {
            ((ListView)sender).SelectedItem = null;
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private void OnMenuItemSelected(object sender, ItemTappedEventArgs e)
        {
            if (Device.Idiom == TargetIdiom.Tablet)
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
                            if (!isCancellable)
                            { 
                                //REASON
                                dialog_open_bg.IsVisible = true;

                               // Popup_start.IsVisible = true;
                               // Popup_start.IsEnabled = true;
                            }
                            else
                              NavigationController(page);

                        }
                    }
                    catch (Exception w1)
                    {
                        Utils.Print(w1.StackTrace);
                    }
                }
            }
        }

        private async Task NavigationController (
            ActionType actionTarget )
        {
            backdark_bg.IsVisible = true;
            indicator.IsVisible = true;

            background_scan_page.Opacity = 1;

            background_scan_page.IsEnabled = true;

            if (Device.Idiom == TargetIdiom.Phone)
            {
                ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

            if ( ! await base.ValidateNavigation ( actionTarget ) )
            {
                dialog_open_bg.IsVisible = true;
                turnoff_mtu_background.IsVisible = true;
                dialogView.CloseDialogs();
                dialogView.OpenCloseDialog("dialog_NoAction", true);
                return;

            }

            if (!isCancellable)
            {
                //REASON
                dialog_open_bg.IsVisible = true;

               // Popup_start.IsVisible = true;
               // Popup_start.IsEnabled = true;
            }
            else
                SwitchToControler(actionTarget);
        }

        private void SwitchToControler(ActionType page)
        {
            
            this.actionTypeNew = page; 

            switch ( page )
            {
                case ActionType.DataRead:

                     #region Read Data Controller
                    this.actionType = this.actionTypeNew; 

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                             this.GoToPage ();
                        })
                    );

                    #endregion

                    break;
                case ActionType.ReadMtu:

 
                    #region Read Mtu Controller
  

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved, page), false);

  
                        })
                    );

                    #endregion

                    break;

                case ActionType.AddMtu:

                    #region Add Mtu Controller
                    ControllerAction(page, "dialog_AddMTU");
  
                    #endregion

                    break;

                case ActionType.TurnOffMtu:

                    #region Turn Off Controller

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                           
                            dialogView.CloseDialogs();

                            #region Check ActionVerify

                            if (this.global.ActionVerify)
                            {
                                dialog_open_bg.IsVisible = true;
                                turnoff_mtu_background.IsVisible = true;
                                dialogView.GetStackLayoutElement("dialog_turnoff_one").IsVisible = true;
                            }
                            else
                            {
                                this.actionType = page;
                                CallLoadViewTurnOff();
                            }
                            #endregion


                        })
                    );

                    #endregion

                    break;

                case ActionType.MtuInstallationConfirmation:

                    #region Install Confirm Controller

                    this.actionType = page;

                    Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                           
                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewInstallConfirmation(dialogsSaved), false);

                        })
                    );

                    #endregion

                    break;

                case ActionType.ReplaceMTU:

                    #region Replace Mtu Controller
                    ControllerAction(page, "dialog_replacemeter_one");
                
                    #endregion

                    break;

                case ActionType.ReplaceMeter:

                    #region Replace Meter Controller
                    ControllerAction(page, "dialog_meter_replace_one");
                  
                    #endregion

                    break;

                case ActionType.AddMtuAddMeter:

                    #region Add Mtu | Add Meter Controller
                    ControllerAction(page, "dialog_AddMTUAddMeter");
                  
                    #endregion

                    break;

                case ActionType.AddMtuReplaceMeter:

                    #region Add Mtu | Replace Meter Controller
                    ControllerAction(page, "dialog_AddMTUReplaceMeter");
                   

                    #endregion

                    break;

                case ActionType.ReplaceMtuReplaceMeter:

                    #region Replace Mtu | Replace Meter Controller
                    ControllerAction(page, "dialog_ReplaceMTUReplaceMeter");
 
                    #endregion

                    break;
            }
        }

        private void ControllerAction(ActionType page, string nameDialog)
        {

            Task.Delay(200).ContinueWith(t =>

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
                        CallLoadPage();
                    }
                    #endregion


                })
            );
        }
        private void CallLoadPage()
        {

            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            this.GoToPage ();
        }
 
        private void CallLoadViewTurnOff()
        {
            dialogView.OpenCloseDialog("dialog_turnoff_one", false);
            dialogView.OpenCloseDialog("dialog_turnoff_two", true);

            Task.Factory.StartNew(TurnOffMethod);
        }

        private void OpenSettingsCallAsync(object sender, EventArgs e)
        {

            if (!isCancellable)
            {
                //REASON
                isSettings = true;
                dialog_open_bg.IsVisible = true;

              //  Popup_start.IsVisible = true;
              //  Popup_start.IsEnabled = true;
                return;
            }
            
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

        private void LogoutTapped(object sender, EventArgs e)
        {
            #region Check if no action done

     
            Device.BeginInvokeOnMainThread(() =>
            {
                dialogView.CloseDialogs();
                dialogView.GetStackLayoutElement("dialog_logoff").IsVisible = true;
              
                dialog_open_bg.IsVisible = true;
                turnoff_mtu_background.IsVisible = true;
            });

            #endregion
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

        private async Task TurnOffMethod ()
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

        #endregion

        #region Form

        protected override void OnAppearing()
        {
            base.OnAppearing();

            background_scan_page.Opacity = 0.5;
            background_scan_page.FadeTo(1, 500);
        }

 
        private void DoLogoff()
        {
            Settings.IsLoggedIn = false;

            try
            {
                FormsApp.DoLogOff();
              
            }
            catch (Exception e25)
            {
                Utils.Print(e25.StackTrace);
            }

            background_scan_page.IsEnabled = true;
            Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogsSaved));
          
        }
         

        private void ReturnToMainView(object sender, EventArgs e)
        {
            if (isCancellable)
            {
                Navigation.PopToRootAsync(false);
            }
            else
            {
                isReturn = true;

                //REASON
                dialog_open_bg.IsVisible = true;

              //  Popup_start.IsVisible = true;
              //  Popup_start.IsEnabled = true;
            }
        }

        #endregion

        #endregion
     
        #region Action

        private void ValveOperationCommand ( object sender, EventArgs e )
        {
            
            isCancellable =true;

            if (!_userTapped)
            {
                //Task.Delay(100).ContinueWith(t =>

                Device.BeginInvokeOnMainThread(() =>
                {
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;
                    _userTapped = true;
                    ContentNav.IsEnabled = false;
                    background_scan_page.IsEnabled = false;
                    ChangeLowerButtonImage(true);

                    Task.Factory.StartNew(ValveOperation_Action);
                });
            }
        }

        private async Task ValveOperation_Action ()
        { 
            #region Get values from form

            Data.SetTemp ( "AccountNumber", tbx_AccountNumber.Text );
            Data.SetTemp ( "MtuId", tbx_MtuId.Text );
            Data.SetTemp ( "MtuStatus", tbx_Mtu_Status.Text );
            Data.SetTemp ( "ValvePosition", pck_ValvePosition.SelectedItem.ToString() );

            #endregion

            #region Events

            this.remote_disconnect.OnProgress -= OnProgress;
            this.remote_disconnect.OnProgress += OnProgress;

            this.remote_disconnect.OnFinish -= OnFinish;
            this.remote_disconnect.OnFinish += OnFinish;

            this.remote_disconnect.OnError -= OnError;
            this.remote_disconnect.OnError += OnError;

            #endregion

            // Launch action!
            await remote_disconnect.Run ();
        }

        private void OnProgress ( object sender, MTUComm.Delegates.ProgressArgs e )
        {
            string mensaje = e.Message;

            Device.BeginInvokeOnMainThread ( () =>
            {
                if ( ! string.IsNullOrEmpty ( mensaje ) )
                    bottomBar.GetLabelElement("label_read").Text = mensaje;
            });
        }
        
        private async Task OnFinish ( object sender, MTUComm.Delegates.ActionFinishArgs args )
        {
            FinalReadListView = new List<ReadMTUItem>();

            Parameter[] paramResult = args.Result.getParameters();

            int mtu_type = 0;

            // Get MtuType = MtuID
            foreach ( Parameter p in paramResult)
            {
                if ( ! string.IsNullOrEmpty ( p.CustomParameter ) &&
                     p.CustomParameter.Equals ( "MtuType" ) )
                    mtu_type = Int32.Parse(p.Value.ToString());
            }

            Mtu mtu = Singleton.Get.Configuration.GetMtuTypeById ( mtu_type );
            InterfaceParameters[] interfacesParams = FormsApp.config.getUserParamsFromInterface( mtu, ActionType.ReadMtu );
            
            currentMtu = Singleton.Get.Action.CurrentMtu;

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

            await  Task.Delay(100).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                _userTapped = false;
                bottomBar.GetTGRElement("bg_action_button").NumberOfTapsRequired = 1;
                ChangeLowerButtonImage(false);
                backdark_bg.IsVisible = false;
                indicator.IsVisible = false;
                bottomBar.GetLabelElement("label_read").Text = "Successful Data Read";
                ContentNav.IsEnabled = true;
                background_scan_page.IsEnabled = true;
                ReadMTUChangeView.IsVisible = false;
                listaMTUread.IsVisible = true;
                listaMTUread.ItemsSource = FinalReadListView;

               #region Hide button

                bottomBar.GetImageElement("bg_action_button_img").IsEnabled = false;
                bottomBar.GetImageElement("bg_action_button_img").Opacity = 0;

                #endregion
            }));
        }

        private void OnError ()
        {
            Error error = Errors.LastError;

            Task.Delay(100).ContinueWith(t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    _userTapped = false;
                    bottomBar.GetTGRElement("bg_action_button").NumberOfTapsRequired = 1;
                    ChangeLowerButtonImage(false);
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                    bottomBar.GetLabelElement("label_read").Text = error.MessageFooter;
                    background_scan_page.IsEnabled = true;
                    ContentNav.IsEnabled = true;
                })
            );
        }

        #endregion
 
        #region Other methods

  

        private void ChangeLowerButtonImage(bool v)
        {
            Image buttonImg = bottomBar.GetImageElement("bg_action_button_img");
            if(v)
                buttonImg.Source = "read_mtu_btn_black.png";
            else
                buttonImg.Source = "read_mtu_btn.png";
              
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
        #endregion
    }
}
