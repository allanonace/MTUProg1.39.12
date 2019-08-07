using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aclara_meters.Behaviors;
using aclara_meters.Helpers;
using aclara_meters.Models;
using Acr.UserDialogs;
using Library;
using MTUComm;
using MTUComm.actions;
using Plugin.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xml;

using ActionType = MTUComm.Action.ActionType;
using FIELD = MTUComm.actions.AddMtuForm.FIELD;

namespace aclara_meters.view
{
    public partial class AclaraViewDataRead
    {

        #region Constants
        private const bool   DEBUG_AUTO_MODE_ON  = false;
        public enum MTUStatus
        {
            ReadingMtuShortTime,
            ReadingMtuData,
            Autodetect,
            CheckingEnconderLongTime,
            ProgramingMtuShortTime,
            PreparingToProgram,
            TurningOffMtu,
            ReadingMtuAgain,
            ProgramingMtu,
            VerifyingMtuData,
            CheckingEnconderShortTime,
            TurningOnMtu,
            ReadingMtu,
        };

        private Dictionary<ActionType,string[]> actionsTexts =
        new Dictionary<ActionType,string[]> ()
        {
            { ActionType.DataRead,
                new string[]
                {
                   "Data Read",
                   "Data Read",
                   "Data Read",
                   "add_mtu_btn.png"
                }
            },
            { ActionType.AddMtu,
                new string[]
                {
                   "Add MTU",
                   "Add MTU",
                   "Add MTU",
                   "add_mtu_btn.png"
                }
            },
            { ActionType.ReplaceMTU,
                new string[]
                {
                   "Replace MTU",
                   "Replace MTU",
                   "Replace MTU",
                   "rep_mtu_btn.png"
                }
            },
            { ActionType.ReplaceMeter,
                new string[]
                {
                   "Replace Meter",
                   "Replace Meter",
                   "Replace Meter",
                   "rep_meter_btn.png"
                }
            },
            { ActionType.AddMtuAddMeter,
                new string[]
                {
                   "Add MTU / Add Meter",
                   "Add MTU / Add Meter",
                   "Add MTU / Add Meter",
                   "add_mtu_meter_btn.png"
                }
            },
            { ActionType.AddMtuReplaceMeter,
                new string[]
                {
                   "Add MTU / Replace Meter",
                   "Add MTU / Replace Meter",
                   "Add MTU / Replace Meter",
                   "add_mtu_rep_meter_btn.png"
                }
            },
            { ActionType.ReplaceMtuReplaceMeter,
                new string[]
                {
                   "Replace MTU / Replace Meter",
                   "Replace MTU / Replace Meter",
                   "Replace MTU / Replace Meter",
                   "rep_mtu_rep_meter_btn.png"
                }
            },
        };

        private Color COL_MANDATORY = Color.FromHex("#FF0000");
        private const float  OPACITY_ENABLE  = 1;
        private const float  OPACITY_DISABLE = 0.8f;
        
        private const string LB_PORT1 = "MTU";
        private const string LB_MISC  = "Miscellaneous";

        #endregion

        #region GUI Elements

        private List<PageItem> MenuList;

        private IUserDialogs dialogsSaved;
        private bool _userTapped;

        
        // Miscelanea
        private List<BorderlessPicker> optionalPickers;
        private List<BorderlessEntry>  optionalEntries;
        private List<BorderlessDatePicker> optionalDates;
        private List<BorderlessTimePicker> optionalTimes;
        private List<Tuple<BorderlessPicker,Label>> optionalMandatoryPickers;
        private List<Tuple<BorderlessEntry,Label>> optionalMandatoryEntries;
        private List<Tuple<BorderlessDatePicker, Label>> optionalMandatoryDates;
        private List<Tuple<BorderlessTimePicker, Label>> optionalMandatoryTimes;

     

        #endregion

        #region Attributes

        private Configuration config;
        private MTUComm.Action Data_read;
      
        private int detectedMtuType;
        private Mtu currentMtu;
        private Global global;
        private MTUBasicInfo mtuBasicInfo;

        private List<ReadMTUItem> FinalReadListView { get; set; }

        private ActionType actionType;
        private ActionType actionTypeNew;
       
        private bool isCancellable;
        private bool isLogout;
        private bool isReturn;
        private bool isSettings;
        private string mtuGeolocationAlt;

        #endregion

        #region Initialization

        public AclaraViewDataRead ()
        {
            InitializeComponent ();
        }

        public AclaraViewDataRead ( IUserDialogs dialogs, ActionType page )
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

            dialogsSaved = dialogs;

            this.config = Singleton.Get.Configuration;
            
           // this.detectedMtuType = ( int )this.mtuBasicInfo.Type;
           // currentMtu = this.config.GetMtuTypeById ( this.detectedMtuType );
            
           // this.addMtuForm = new AddMtuForm ( currentMtu );
            
            this.Data_read = new MTUComm.Action (
                FormsApp.ble_interface,
                this.actionType,
                FormsApp.credentialsService.UserName );
            
            isCancellable = true;

            Device.BeginInvokeOnMainThread(() =>
            {
                string[] texts = this.actionsTexts[ this.actionType ];
            
                name_of_window_port1  .Text   = texts[ 0 ] + " - " + LB_PORT1;
                name_of_window_misc   .Text   = texts[ 2 ] + " - " + LB_MISC;
                bg_read_mtu_button_img.Source = texts[ 3 ];
                
                label_read.Opacity    = 1;
                                
                if ( Device.Idiom == TargetIdiom.Tablet )
                     LoadTabletUI ();
                else LoadPhoneUI ();
                
                NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar
                
                battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
                rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");
                
                //Change username textview to Prefs. String
                if (!string.IsNullOrEmpty(FormsApp.credentialsService.UserName))
                    userName.Text = FormsApp.credentialsService.UserName; 

                this.cancelReasonOtherInput.Focused += (s, e) =>
                {
                    if (Device.Idiom == TargetIdiom.Tablet)
                        SetLayoutPosition(true, (int)-120);
                    else
                        SetLayoutPosition(true, (int)-20);
                };

                this.cancelReasonOtherInput.Unfocused += (s, e) =>
                {
                    if (Device.Idiom == TargetIdiom.Tablet)
                        SetLayoutPosition(false, (int)-120);
                    else
                        SetLayoutPosition(false, (int)-20);
                };
            });

            LoadSideMenuElements ();

            _userTapped = false;
            
            TappedListeners ();
            InitializeLowerbarLabel();

            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;

            Task.Run(async () => await InitializeDataReadForm ())
                .ContinueWith ( t =>
                Device.BeginInvokeOnMainThread ( () =>
                {
                    Task.Delay ( 100 )
                    .ContinueWith ( t0 =>
                        Device.BeginInvokeOnMainThread ( () =>
                        {
                            label_read.Opacity = 1;
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            background_scan_page.IsEnabled = true;
                            label_read.Text = "Press Button to Start";
                   
                        })
                    );
                })
            );
               

        }
        
        
        private async Task InitializeDataReadForm ()
        {
            #region Initialize data

            this.div_MtuId.IsVisible=true;
            this.div_Mtu_Status.IsVisible=true;
            this.div_MtuId.IsVisible=true;

            List<string> list = new List<string> ()
            {
                "1", "8", "32", "64", "96"
            };
           
            //Now I am given ItemsSorce to the Pickers
            pck_DaysOfRead.ItemsSource   = list;
             
            #endregion

            #region Misc

            if ( this.global.Options.Count>0)
                InitializeOptionalFields ();
            

            #endregion

            #region Labels

            // Account Number
            this.lb_AccountNumber.Text = global.AccountLabel;      

            #endregion
             
            #region Labels
            
            this.port1label.Text = LB_PORT1;
            this.misclabel .Text = LB_MISC;
                        
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
                label_read.Text = statusMsg;
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

        private void LoadSideMenuElements()
        {
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection

            MenuList = new List<PageItem>();

            // Adding menu items to MenuList

            MenuList.Add(new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png",Color="White",TargetType = ActionType.ReadMtu });

            if (this.global.ShowTurnOff)
                MenuList.Add(new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", Color = "White", TargetType = ActionType.TurnOffMtu });

            if (this.global.ShowAddMTU)
                MenuList.Add(new PageItem() { Title = "Add MTU", Icon = "addMTU.png", Color = "White", TargetType = ActionType.AddMtu });

            if (this.global.ShowReplaceMTU)
                MenuList.Add(new PageItem() { Title = "Replace MTU", Icon = "replaceMTU2.png", Color = "White", TargetType = ActionType.ReplaceMTU });

            if (this.global.ShowReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Replace Meter", Icon = "replaceMeter.png", Color = "White", TargetType = ActionType.ReplaceMeter });

            if (this.global.ShowAddMTUMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Add Meter", Icon = "addMTUaddmeter.png", Color = "White", TargetType = ActionType.AddMtuAddMeter });

            if (this.global.ShowAddMTUReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Rep. Meter", Icon = "addMTUrepmeter.png", Color = "White", TargetType = ActionType.AddMtuReplaceMeter });

            if (this.global.ShowReplaceMTUMeter)
                MenuList.Add(new PageItem() { Title = "Rep.MTU / Rep. Meter", Icon = "repMTUrepmeter.png", Color = "White", TargetType = ActionType.ReplaceMtuReplaceMeter });

            if (this.global.ShowInstallConfirmation)
                MenuList.Add(new PageItem() { Title = "Install Confirmation", Icon = "installConfirm.png", Color = "White", TargetType = ActionType.MtuInstallationConfirmation });

            if (FormsApp.config.Global.ShowDataRead)
                MenuList.Add(new PageItem() { Title = "Data Read", Icon = "readmtu_icon.png", Color = "White", TargetType = ActionType.DataRead });

            // ListView needs to be at least  elements for UI Purposes, even empty ones
            while (MenuList.Count < 9)
                MenuList.Add(new PageItem() { Title = "", Color = "#6aa2b8", Icon = "" });

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = MenuList;

        }

        private void InitializeOptionalFields()
        {
           
            optionalPickers = new List<BorderlessPicker>();
            optionalEntries = new List<BorderlessEntry>();
            optionalDates = new List<BorderlessDatePicker>();
            optionalTimes = new List<BorderlessTimePicker>();
            optionalMandatoryPickers = new List<Tuple<BorderlessPicker,Label>> ();
            optionalMandatoryEntries = new List<Tuple<BorderlessEntry,Label>> ();
            optionalMandatoryDates = new List<Tuple<BorderlessDatePicker, Label>>();
            optionalMandatoryTimes = new List<Tuple<BorderlessTimePicker, Label>>();
            foreach ( Option optionalField in this.global.Options )
            {
                Frame optionalContainerB = new Frame()
                {
                    CornerRadius = 6,
                    HeightRequest = 30,
                    Margin = new Thickness(0, 4, 0, 0),
                    BackgroundColor = Color.FromHex("#7a868c")
                };

                Frame optionalContainerC = new Frame()
                {
                    CornerRadius = 6,
                    HeightRequest = 30,
                    Margin = new Thickness(-7, -7, -7, -7),
                    BackgroundColor = Color.White
                };

                StackLayout optionalContainerD = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(1, 1, 1, 1),
                    BackgroundColor = Color.White
                };

                StackLayout optionalContainerA = new StackLayout()
                {
                    StyleId = "bloque" + 1
                };

                Label optionalLabel = new Label()
                {
                    Text = optionalField.Display,
                    Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                    Margin = new Thickness(0, 4, 0, 0)
                };

                BorderlessPicker optionalPicker = null;
                BorderlessEntry  optionalEntry  = null;
                BorderlessDatePicker optionalDate = null;
                BorderlessTimePicker optionalTime = null;

                bool isList = optionalField.Type.Equals("list");
                if ( isList )
                {
                    List<string> optionalFieldOptions = optionalField.OptionList;
                    optionalPicker = new BorderlessPicker()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 40,
                        FontSize = 17,
                        ItemsSource = optionalFieldOptions
                    };
                    optionalPicker.Name = optionalField.Name.Replace(" ", "_");
                    optionalPicker.Display = optionalField.Display;

                    optionalPickers.Add(optionalPicker);

                    optionalContainerD.Children.Add(optionalPicker);
                    optionalContainerC.Content = optionalContainerD;
                    optionalContainerB.Content = optionalContainerC;
                    optionalContainerA.Children.Add(optionalLabel);
                    optionalContainerA.Children.Add(optionalContainerB);

                    this.optionalFields.Children.Add(optionalContainerA);
                }
                else if(optionalField.Format=="date")
                {
                    bool required = optionalField.Required;
                    optionalDate = new BorderlessDatePicker()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                       // HeightRequest = 70,
                        FontSize = 17
                    };
                    optionalDate.Name = optionalField.Name.Replace(" ", "_");
                    optionalDate.Display = optionalField.Display;

                    //CommentsLengthValidatorBehavior behavior = new CommentsLengthValidatorBehavior();
                    //behavior.MaxLength = optionalField.Len;

                    //optionalEntry.Behaviors.Add(behavior);

                    optionalDates.Add(optionalDate);

                    optionalContainerD.Children.Add(optionalDate);
                    optionalContainerC.Content = optionalContainerD;
                    optionalContainerB.Content = optionalContainerC;
                    optionalContainerA.Children.Add(optionalLabel);
                    optionalContainerA.Children.Add(optionalContainerB);

                    this.optionalFields.Children.Add(optionalContainerA);
                }
                else if (optionalField.Format == "time")
                {
                    bool required = optionalField.Required;
                    optionalTime = new BorderlessTimePicker()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        // HeightRequest = 70,
                        FontSize = 17
                    };
                    optionalTime.Name = optionalField.Name.Replace(" ", "_");
                    optionalTime.Display = optionalField.Display;

                    //CommentsLengthValidatorBehavior behavior = new CommentsLengthValidatorBehavior();
                    //behavior.MaxLength = optionalField.Len;

                    //optionalEntry.Behaviors.Add(behavior);

                    optionalTimes.Add(optionalTime);

                    optionalContainerD.Children.Add(optionalTime);
                    optionalContainerC.Content = optionalContainerD;
                    optionalContainerB.Content = optionalContainerC;
                    optionalContainerA.Children.Add(optionalLabel);
                    optionalContainerA.Children.Add(optionalContainerB);

                    this.optionalFields.Children.Add(optionalContainerA);
                }
                else // Text
                {
                    string format = optionalField.Format;
                    int maxLen = optionalField.Len;
                    int minLen = optionalField.MinLen;
                    bool required = optionalField.Required;

                    Keyboard keyboard = Keyboard.Default;
                    //if      ( format.Equals ( "alpha"        ) ) keyboard = Keyboard.Default;
                    //else if ( format.Equals ( "date"         ) ) keyboard = Keyboard.Default;
                    if      ( format.Equals ( "alphanumeric" ) ) keyboard = Keyboard.Numeric;
                    else if ( format.Equals ( "time"         ) ) keyboard = Keyboard.Numeric;

                    optionalEntry = new BorderlessEntry()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 40,
                        Keyboard = keyboard,
                        FontSize = 17
                    };
                    optionalEntry.Name = optionalField.Name.Replace(" ", "_");
                    optionalEntry.Display = optionalField.Display;

                    CommentsLengthValidatorBehavior behavior = new CommentsLengthValidatorBehavior();
                    behavior.MaxLength = optionalField.Len;

                    optionalEntry.Behaviors.Add(behavior);

                    optionalEntries.Add(optionalEntry);

                    optionalContainerD.Children.Add(optionalEntry);
                    optionalContainerC.Content = optionalContainerD;
                    optionalContainerB.Content = optionalContainerC;
                    optionalContainerA.Children.Add(optionalLabel);
                    optionalContainerA.Children.Add(optionalContainerB);

                    this.optionalFields.Children.Add(optionalContainerA);
                }

                // Mandatory fields
                if ( optionalField.Required )
                {
                    if ( isList )
                         this.optionalMandatoryPickers.Add ( new Tuple<BorderlessPicker,Label> ( optionalPicker, optionalLabel ) );
                    else if (optionalField.Format=="date")
                        this.optionalMandatoryDates.Add(new Tuple<BorderlessDatePicker, Label>(optionalDate, optionalLabel));
                    else if (optionalField.Format == "time")
                        this.optionalMandatoryTimes.Add(new Tuple<BorderlessTimePicker, Label>(optionalTime, optionalLabel));
                    else this.optionalMandatoryEntries.Add ( new Tuple<BorderlessEntry,Label>  ( optionalEntry,  optionalLabel ) );
                    
                    if ( global.ColorEntry )
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            optionalLabel.TextColor = COL_MANDATORY;
                        });
                }
            }
        }

        private void TappedListeners ()
        {
            logout_button.Tapped += LogoutTapped;
            
            settings_button.Tapped += OpenSettingsCallAsync;
            back_button.Tapped += ReturnToMainView;
            bg_read_mtu_button.Tapped += DataReadMtu;

            turnoffmtu_ok.Tapped += TurnOffMTUOkTapped;
            turnoffmtu_no.Tapped += TurnOffMTUNoTapped;
            turnoffmtu_ok_close.Tapped += TurnOffMTUCloseTapped;

            replacemeter_ok.Tapped += ReplaceMtuOkTapped;
            replacemeter_cancel.Tapped += ReplaceMtuCancelTapped;

            meter_ok.Tapped += Confirm_Yes_ReplaceMeter;
            meter_cancel.Tapped += Confirm_No_ReplaceMeter;

            port1label.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => port1_command()),
            });
            misclabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => misc_command()),
            });
          
            gps_icon_button.Tapped += GpsUpdateButton;

            logoff_ok.Tapped += Confirm_Yes_LogOut;
            logoff_no.Tapped += Confirm_No_LogOut;

            submit_dialog.Clicked += submit_send;
            cancel_dialog.Clicked += Cancel_No;

            dialog_AddMTUAddMeter_ok.Tapped += Confirm_Yes_AddMtuAddMeter;
            dialog_AddMTUAddMeter_cancel.Tapped += Confirm_No_AddMtuAddMeter;

            dialog_AddMTUReplaceMeter_ok.Tapped += Confirm_Yes_AddMtuReplaceMeter;
            dialog_AddMTUReplaceMeter_cancel.Tapped += Confirm_No_AddMtuReplaceMeter;

            dialog_ReplaceMTUReplaceMeter_ok.Tapped += Confirm_Yes_ReplaceMtuReplaceMeter;
            dialog_ReplaceMTUReplaceMeter_cancel.Tapped += Confirm_No_ReplaceMtuReplaceMeter;

            dialog_AddMTU_ok.Tapped += Confirm_Yes_AddMtu;
            dialog_AddMTU_cancel.Tapped += Confirm_No_AddMtu;
        }

        #region Dialogs

        private void ChangeAction ()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (actionType == ActionType.DataRead)
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewDataRead(dialogsSaved, this.actionType ), false);
                else 
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved, this.actionType ), false);
                
                #region New Circular Progress bar Animations    

                backdark_bg.IsVisible = false;
                indicator.IsVisible = false;
                background_scan_page.IsEnabled = true;

                #endregion
            });
        }

        private void Cancel_No ( object sender, EventArgs e )
        {
            dialog_open_bg.IsVisible = false;
            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;
            this.cancelReasonOtherInput.Text = String.Empty;
            this.cancelReasonPicker.SelectedIndex = 0;
            //Navigation.PopToRootAsync(false);
        }

        private void Confirm_No_AddMtu (object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_AddMTU.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
        }

        private void Confirm_Yes_AddMtu (object sender, EventArgs e)
        {
            dialog_AddMTU.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            this.actionType = this.actionTypeNew;
            this.ChangeAction ();
        }

        private void Confirm_No_AddMtuAddMeter (object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_AddMTUAddMeter.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
        }

        private void Confirm_Yes_AddMtuAddMeter (object sender, EventArgs e)
        {
            dialog_AddMTUAddMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            this.actionType = this.actionTypeNew;
            this.ChangeAction ();
        }

        private void Confirm_No_AddMtuReplaceMeter (object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_AddMTUReplaceMeter.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
        }

        private void Confirm_Yes_AddMtuReplaceMeter (object sender, EventArgs e)
        {
            dialog_AddMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            this.actionType = this.actionTypeNew;
            this.ChangeAction ();
        }

        private void Confirm_No_ReplaceMeter ( object sender, EventArgs e )
        {
            dialog_open_bg.IsVisible = false;
            dialog_meter_replace_one.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
        }

        private void Confirm_Yes_ReplaceMeter ( object sender, EventArgs e )
        {
            dialog_meter_replace_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            this.actionType = this.actionTypeNew;
            this.ChangeAction ();
        }

        private void Confirm_No_ReplaceMtuReplaceMeter (object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_ReplaceMTUReplaceMeter.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
        }

        private void Confirm_Yes_ReplaceMtuReplaceMeter (object sender, EventArgs e)
        {
            dialog_ReplaceMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            this.actionType = this.actionTypeNew;
            this.ChangeAction ();
        }

        private async void Confirm_Yes_LogOut ( object sender, EventArgs e )
        {
            // Upload log files
            if (global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();

            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                //REASON
                if (!isCancellable)
                {
                    isLogout = true;
                    dialog_open_bg.IsVisible = true;
                    Popup_start.IsVisible = true;
                    Popup_start.IsEnabled = true;
                }
                else DoLogoff();
            });
           
        }

        private void Confirm_No_LogOut ( object sender, EventArgs e )
        {
            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            //Navigation.PopToRootAsync(false);
        }

        #endregion

        private void InitializeLowerbarLabel ()
        {
            label_read.Text = "Push Button to START";
        }

 

    
        #region Phone/Tablet

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
            hamburger_icon.IsVisible = true;
            background_scan_page.Margin = new Thickness(310, 0, 0, 0);
            tablet_user_view.TranslationY = -22;
            tablet_user_view.Scale = 1.2;
            logo_tablet_aclara.Opacity = 0;
            shadoweffect.IsVisible = true;
            aclara_logo.Scale = 1.2;
            aclara_logo.TranslationX = 42;
            aclara_logo.TranslationX = 42;
            shadoweffect.Source = "shadow_effect_tablet";
        }

        #endregion

        #endregion

        #region GUI Handlers

        #region Menu options

        object menu_sender;
        ItemTappedEventArgs menu_tappedevents;

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
                
                menu_sender = sender;
                menu_tappedevents = e;

            
                //Application.Current.MainPage.Navigation.PopAsync(false);0
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

                        if (this.actionType != page)
                        {
                            this.actionTypeNew = page;
                            if (!isCancellable)
                            { 
                                //REASON
                                dialog_open_bg.IsVisible = true;

                                Popup_start.IsVisible = true;
                                Popup_start.IsEnabled = true;
                            }
                            else
                            {
                                //this.actionType = page;
                                NavigationController(page);
                            }
                        }
                    }
                    catch (Exception w1)
                    {
                        Utils.Print(w1.StackTrace);
                    }
                }
            }
        }

        private void NavigationController(ActionType page)
        {
            if (!isCancellable)
            {
                //REASON
                dialog_open_bg.IsVisible = true;

                Popup_start.IsVisible = true;
                Popup_start.IsEnabled = true;
            }
            else
                SwitchToControler(page);
        }

        private void SwitchToControler(ActionType page)
        {
            this.actionTypeNew = page; 

            switch ( page )
            {
                case ActionType.DataRead:

                    #region New Circular Progress bar Animations    

             
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;

                    #endregion


                    #region Read Data Controller
                    this.actionType = this.actionTypeNew; 

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

                            //Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved, page), false);
                            ChangeAction();

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

                            #region New Circular Progress bar Animations    

                  
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;

                            #endregion
                        })
                    );

                    #endregion

                    break;
                case ActionType.ReadMtu:

                    #region New Circular Progress bar Animations    

             
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;

                    #endregion


                    #region Read Mtu Controller
                    this.actionType = this.actionTypeNew; 

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

                            #region New Circular Progress bar Animations    

                  
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;

                            #endregion
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

                            if (this.global.ActionVerify)
                                dialog_AddMTU.IsVisible = true;
                            else
                            {
                                this.actionType = page;
                                CallLoadViewAddMtu();
                            }
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

                            if (this.global.ActionVerify)
                                dialog_turnoff_one.IsVisible = true;
                            else
                            {
                                this.actionType = page;
                                CallLoadViewTurnOff();
                            }
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
                    this.actionType = page;

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

                            if (this.global.ActionVerify)
                                dialog_replacemeter_one.IsVisible = true;
                            else
                            {
                                this.actionType = page;
                                CallLoadViewReplaceMtu();
                            }
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

                            if (this.global.ActionVerify)
                                dialog_meter_replace_one.IsVisible = true;
                            else
                            {
                                this.actionType = page;
                                CallLoadViewReplaceMeter();
                            }
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

                            if (this.global.ActionVerify)
                                dialog_AddMTUAddMeter.IsVisible = true;
                            else
                            {
                                this.actionType = page;
                                CallLoadViewAddMTUAddMeter();
                            }
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

                            if (this.global.ActionVerify)
                                dialog_AddMTUReplaceMeter.IsVisible = true;
                            else
                            {
                                this.actionType = page;
                                CallLoadViewAddMTUReplaceMeter();
                            }
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

                            if (this.global.ActionVerify)
                                dialog_ReplaceMTUReplaceMeter.IsVisible = true;
                            else
                            {
                                this.actionType = page;
                                CallLoadViewReplaceMTUReplaceMeter();
                            }
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

            this.ChangeAction ();
        }

        private void CallLoadViewAddMTUReplaceMeter()
        {
            dialog_AddMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            this.ChangeAction ();
        }

        private void CallLoadViewAddMTUAddMeter()
        {

            dialog_AddMTUAddMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            this.ChangeAction ();
        }

        private void CallLoadViewReplaceMeter()
        {
            dialog_meter_replace_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            this.ChangeAction ();
        }

        private void CallLoadViewReplaceMtu()
        {
            dialog_replacemeter_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            this.ChangeAction ();
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

            this.ChangeAction ();
        }

        private void OpenSettingsCallAsync(object sender, EventArgs e)
        {

            if (!isCancellable)
            {
                //REASON
                isSettings = true;
                dialog_open_bg.IsVisible = true;

                Popup_start.IsVisible = true;
                Popup_start.IsEnabled = true;
                return;
            }
            //printer.Suspend();
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
   

                        shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; //   if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            #region New Circular Progress bar Animations    

                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            background_scan_page.IsEnabled = true;

                            #endregion
                        });

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
            

                        shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false; 

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            #region New Circular Progress bar Animations    

                          
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;
                            background_scan_page.IsEnabled = true;

                            #endregion
                        });
                    }
                }
                catch (Exception i2)
                {
                    Utils.Print(i2.StackTrace);
                }
            }));
        }

        private async void LogoutTapped(object sender, EventArgs e)
        {
            #region Check if no action done

     
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

            #endregion
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

            this.actionType = this.actionTypeNew;
            this.ChangeAction ();
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

        private void TurnOffMTUOkTapped(object sender, EventArgs e)
        {
            dialog_turnoff_one.IsVisible = false;
            dialog_turnoff_two.IsVisible = true;


            Task.Factory.StartNew(TurnOffMethod);
        }

        private void TurnOffMethod ()
        {
            MTUComm.Action turnOffAction = new MTUComm.Action (
                FormsApp.ble_interface,
                MTUComm.Action.ActionType.TurnOffMtu,
                FormsApp.credentialsService.UserName );

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

        #endregion

        #region Form

        protected override void OnAppearing()
        {
            base.OnAppearing();

            background_scan_page.Opacity = 0.5;
            background_scan_page.FadeTo(1, 500);
        }

        private void submit_send(object sender, EventArgs e3)
        {
            int selectedCancelReasonIndex = cancelReasonPicker.SelectedIndex;
            string selectedCancelReason = String.Empty;

            if ( selectedCancelReasonIndex > -1 )
                selectedCancelReason = cancelReasonPicker.Items[cancelReasonPicker.SelectedIndex];
            if (selectedCancelReason == "Other")
            {
                if (String.IsNullOrEmpty(cancelReasonOtherInput.Text))
                {
                    this.cancelReasonOtherInput.Placeholder = "Please, enter the reason for the cancellation";
                    return;
                }
                else
                    selectedCancelReason = cancelReasonOtherInput.Text;
            }
            this.Data_read.Cancel ( selectedCancelReason );

            dialog_open_bg.IsVisible = false;
            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;

            if (Device.Idiom == TargetIdiom.Tablet && !isLogout && !isReturn && !isSettings)
            {
                this.actionType = this.actionTypeNew;

                SwitchToControler ( this.actionType );
            }
            else
            {
                #region I guess it's logout time ...

                Task.Run(async () =>
                {
                    await Task.Delay(500); Device.BeginInvokeOnMainThread(() =>
                    {

                        if (!FormsApp.ble_interface.IsOpen())
                        {
                            // don't do anything if we just de-selected the row.
                            if (menu_tappedevents.Item == null) return;
                            // Deselect the item.
                            if (menu_sender is ListView lv) lv.SelectedItem = null;
                        }

                        if (FormsApp.ble_interface.IsOpen())
                        {
                            if (isLogout)
                            {
                                DoLogoff();
                            }
                            else
                            {
                                Navigation.PopToRootAsync(false);
                                isReturn = false;
                                isSettings = false;

                                //Navigation.PopAsync();
                            }

                        }
                    });
                });
            }

            #endregion
        }

        private void DoLogoff()
        {
            Settings.IsLoggedIn = false;

            try
            {
                FormsApp.DoLogOff();
                //FormsApp.credentialsService.DeleteCredentials();
                //FormsApp.ble_interface.Close();
                //Singleton.Remove<Puck>();
            }
            catch (Exception e25)
            {
                Utils.Print(e25.StackTrace);
            }

            background_scan_page.IsEnabled = true;
            Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogsSaved));
            //Navigation.PopToRootAsync(false);
        }

        private void misc_command()
        {
            miscview.Opacity = 0;

            port1label.Opacity = 0.5;
            misclabel.Opacity = 1;
            
            port1label.FontSize = 19;
            misclabel.FontSize = 22;
            

            port1view.IsVisible = false;
            miscview.IsVisible = true;

            miscview.FadeTo(1, 200);

        }

        private void port1_command()
        {
            port1view.Opacity = 0;

            port1label.Opacity = 1;
            misclabel.Opacity = 0.5;
            
            port1label.FontSize = 22;
            misclabel.FontSize = 19;
                 

            port1view.IsVisible = true;
            miscview.IsVisible = false;

            port1view.FadeTo(1, 200);


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

                Popup_start.IsVisible = true;
                Popup_start.IsEnabled = true;
            }
        }

        #endregion

        #endregion

        #region Validation

        private void ValidateEqualityOnFocus (
            BorderlessEntry tbx1,
            BorderlessEntry tbx2,
            Label lb )
        {
            lb.IsVisible = ! tbx1.Text.Equals ( tbx2.Text );
        }

        private bool ValidateFields ( ref string msgError )
        {


            #region Miscelanea
            dynamic NoValNOrEmpty = new Func<string,bool> ( ( value ) =>
                                        ! string.IsNullOrEmpty ( value ) &&
                                        ! Validations.IsNumeric ( value ) );
                  

            if ( NoValNOrEmpty ( this.tbx_MtuGeolocationLat .Text ) ||
                 NoValNOrEmpty ( this.tbx_MtuGeolocationLong.Text ) )
            {
                msgError = "Field 'GPS Coordinates' are incorrectly filled";
                return false;
            }
            string FILL_ERROR = String.Empty;
            FILL_ERROR = "Miscellaneous field '_' is incorrectly filled";

            foreach ( Tuple<BorderlessPicker,Label> tuple in optionalMandatoryPickers )
                if ( tuple.Item1.SelectedIndex <= -1 )
                {
                    msgError = FILL_ERROR.Replace ( "_", tuple.Item2.Text );
                    return false;
                }

            foreach ( Tuple<BorderlessEntry,Label> tuple in optionalMandatoryEntries )
                if ( string.IsNullOrEmpty ( tuple.Item1.Text ) )
                {
                    msgError = FILL_ERROR.Replace ( "_", tuple.Item2.Text );
                    return false;
                }

            foreach (Tuple<BorderlessDatePicker, Label> tuple in optionalMandatoryDates)
                if (string.IsNullOrEmpty(tuple.Item1.Date.ToShortDateString()))
                {
                    msgError = FILL_ERROR.Replace("_", tuple.Item2.Text);
                    return false;
                }
            #endregion

            return true;
        }

        #endregion

        #region Action

        private void DataReadMtu ( object sender, EventArgs e )
        {
            string msgError = string.Empty;
            if ( ! DEBUG_AUTO_MODE_ON &&
                 ! this.ValidateFields ( ref msgError ) )
            {
                DisplayAlert ( "Error", msgError, "OK" );
                return;
            }

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

                    Task.Factory.StartNew(DataRead_Action);
                });
            }
        }

        private void DataRead_Action ()
        { 
            #region Get values from form

            Data.Set("AccountNumber", tbx_AccountNumber.Text,true);
            Data.Set("MtuId", tbx_MtuId.Text, true);
            Data.Set("MtuStatus", tbx_Mtu_Status.Text,true);
            Data.Set("NumOfDays", pck_DaysOfRead.SelectedItem.ToString(),true);

            // GPS
            string value_lat = this.tbx_MtuGeolocationLat .Text;
            string value_lon = this.tbx_MtuGeolocationLong.Text;
            string value_alt = this.mtuGeolocationAlt;
   

            // Gps
            if ( ! string.IsNullOrEmpty ( value_lat ) &&
                 ! string.IsNullOrEmpty ( value_lon ) )
            {
                double lat = Convert.ToDouble ( value_lat );
                double lon = Convert.ToDouble ( value_lon );
        
                Data.Set ( "GpsLat", lat, true );
                Data.Set ( "GpsLon", lon, true );
                Data.Set ( "GpsAlt", value_alt, true );
                
            }
            else
            {
                Data.Set("GpsLat", string.Empty, true);
                Data.Set("GpsLon", string.Empty, true);
                Data.Set("GpsAlt", string.Empty, true);
            }


            //List<Parameter> optionalParams = new List<Parameter>();
            Data.Set("Options", new List<Parameter>(), true);

            foreach (BorderlessPicker p in optionalPickers)
                if (p.SelectedItem != null)
                    Data.Get.Options.Add(new Parameter(p.Name, p.Display, p.SelectedItem,"",0,true));
 
            foreach ( BorderlessEntry e in optionalEntries )
                if( ! string.IsNullOrEmpty ( e.Text ) )
                    Data.Get.Options.Add(new Parameter(e.Name, e.Display, e.Text, "", 0, true));

            foreach (BorderlessDatePicker d in optionalDates)
                if (!string.IsNullOrEmpty(d.Date.ToShortDateString()))
                    Data.Get.Options.Add(new Parameter(d.Name, d.Display, $"{d.Date.ToShortDateString()} 12:00:00", "", 0, true));

            foreach (BorderlessTimePicker t in optionalTimes)
                if (!string.IsNullOrEmpty(t.Time.ToString()))
                    Data.Get.Options.Add(new Parameter(t.Name, t.Display, $"{System.DateTime.Today.ToShortDateString()} {t.Time.ToString()}", "", 0, true));

            //if ( optionalParams.Count > 0 )
            //    Data.Set("Miscelanea",optionalParams, true);
            //    //this.addMtuForm.AddParameter ( FIELD.OPTIONAL_PARAMS, optionalParams );

            #endregion

            #region Events

            this.Data_read.OnProgress -= OnProgress;
            this.Data_read.OnProgress += OnProgress;

            this.Data_read.OnFinish -= OnFinish;
            this.Data_read.OnFinish += OnFinish;

            this.Data_read.OnError -= OnError;
            this.Data_read.OnError += OnError;

            #endregion

            // Launch action!
            Data_read.Run ();
        }

        private void OnProgress ( object sender, MTUComm.Delegates.ProgressArgs e )
        {
            string mensaje = e.Message;

            Device.BeginInvokeOnMainThread ( () =>
            {
                if ( ! string.IsNullOrEmpty ( mensaje ) )
                    label_read.Text = mensaje;
            });
        }
        
        private void OnFinish ( object sender, MTUComm.Action.ActionFinishArgs e )
        {
            FinalReadListView = new List<ReadMTUItem>();

            Parameter[] paramResult = e.Result.getParameters();

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
            
            Mtu currentMtu = Singleton.Get.Action.CurrentMtu;

            foreach (InterfaceParameters iParameter in interfacesParams)
            {
                // Port 1 or 2 log section
                if (iParameter.Name.Equals("Port"))
                {
                    ActionResult[] ports = e.Result.getPorts ();

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
                    Parameter param = e.Result.getParameterByTag ( iParameter.Name, iParameter.Source, 0 );

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

            Task.Delay(100).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                _userTapped = false;
                bg_read_mtu_button.NumberOfTapsRequired = 1;
                ChangeLowerButtonImage(false);
                backdark_bg.IsVisible = false;
                indicator.IsVisible = false;
                label_read.Text = "Successful Data Read";
                ContentNav.IsEnabled = true;
                background_scan_page.IsEnabled = true;
                ReadMTUChangeView.IsVisible = false;
                listaMTUread.IsVisible = true;
                listaMTUread.ItemsSource = FinalReadListView;

               #region Hide button

                bg_read_mtu_button_img.IsEnabled = false;
                bg_read_mtu_button_img.Opacity = 0;

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
                    bg_read_mtu_button.NumberOfTapsRequired = 1;
                    ChangeLowerButtonImage(false);
                    backdark_bg.IsVisible = false;
                    indicator.IsVisible = false;
                    label_read.Text = error.MessageFooter;
                    background_scan_page.IsEnabled = true;
                    ContentNav.IsEnabled = true;
                })
            );
        }

        #endregion

        #region Location

        private async void GpsUpdateButton ( object sender, EventArgs e )
        {
             Device.BeginInvokeOnMainThread(() =>
                {
                    backdark_bg.IsVisible = true;              
                    indicator.IsVisible = true;   
                    background_scan_page.IsEnabled = false;
                    ContentNav.IsEnabled = false;
                });
            var position = await GetCurrentPosition();
            if (position==null)
                await dialogsSaved.AlertAsync("You must activate the GPS on the device to return coordinates","Alert");
            else
            {
                this.tbx_MtuGeolocationLat .Text = position.Latitude .ToString ();
                this.tbx_MtuGeolocationLong.Text = position.Longitude.ToString ();
                this.mtuGeolocationAlt           = position.Altitude .ToString ();
            }
            Device.BeginInvokeOnMainThread(() =>
                {
                    backdark_bg.IsVisible = false;              
                    indicator.IsVisible = false;   
                    background_scan_page.IsEnabled = true;
                    ContentNav.IsEnabled = true;
                });
        }

        public static async Task<Location> GetCurrentPosition()
	    {
            Location location = null;
           
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                location = await Geolocation.GetLocationAsync(request);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
               return null; // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }

            return location;
            
	    }
        #endregion

        #region Other methods

        void SetLayoutPosition(bool onFocus, int value)
        {
            if (onFocus)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.Popup_start.TranslateTo(0, value, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.Popup_start.TranslateTo(0, value, 50);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    this.Popup_start.TranslateTo(0, 0, 50);
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    this.Popup_start.TranslateTo(0, 0, 50);
                }
            }
        }

        private void ChangeLowerButtonImage(bool v)
        {
            string ext   = ".png";
            string btn   = "_btn";
            string black = btn + "_black" + ext;
            btn += ext;
        
            switch ( this.actionType )
            {
                case ActionType.AddMtu:
                    bg_read_mtu_button_img.Source = "add_mtu" + ( ( v ) ? black : btn );
                    break;

                case ActionType.ReplaceMTU:
                    bg_read_mtu_button_img.Source = "rep_mtu" + ( ( v ) ? black : btn );
                    break;

                case ActionType.ReplaceMeter:
                    bg_read_mtu_button_img.Source = "rep_meter" + ( ( v ) ? black : btn );
                    break;

                case ActionType.AddMtuAddMeter:
                    bg_read_mtu_button_img.Source = "add_mtu_meter" + ( ( v ) ? black : btn );
                    break;

                case ActionType.AddMtuReplaceMeter:
                    bg_read_mtu_button_img.Source = "add_mtu_rep_meter" + ( ( v ) ? black : btn );
                    break;

                case ActionType.ReplaceMtuReplaceMeter:
                    bg_read_mtu_button_img.Source = "rep_mtu_rep_meter" + ( ( v ) ? black : btn );
                    break;
            }

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        #endregion
  
    }

}
