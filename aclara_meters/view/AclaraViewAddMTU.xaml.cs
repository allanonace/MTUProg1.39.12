using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using aclara_meters.Behaviors;
using aclara_meters.Helpers;
using aclara_meters.Models;
using aclara_meters.util;
using Acr.UserDialogs;
using Library;
using Library.Exceptions;
using MTUComm;
using MTUComm.actions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xml;
using ZXing.Net.Mobile.Forms;
using ActionType = MTUComm.Action.ActionType;
using FIELD = MTUComm.actions.AddMtuForm.FIELD;
using MTUStatus = MTUComm.Action.MTUStatus;

namespace aclara_meters.view
{
    public partial class AclaraViewAddMTU
    {
        #region Debug

        //#if ADHOC || RELEASE
        private const bool   DEBUG_AUTO_MODE_ON  = false;
        //#elif DEBUG
        //private const bool   DEBUG_AUTO_MODE_ON  = true;
        //#endif
        private const string DEBUG_ACCOUNTNUMBER = "123456789"; // 9
        private const string DEBUG_WORKORDER     = "12345678901234567890"; // 20
        private const string DEBUG_OLDMETERNUM   = "09876543210987654321"; // 20
        private const int    DEBUG_OLDMETERWORK  = 0;
        private const string DEBUG_OLDMETERREAD  = "444444444444"; //"210987654321"; // 12
        private const int    DEBUG_REPLACEREGIS  = 0;
        private const string DEBUG_METERNUM      = "12345678901234567890"; // 20
        private const int    DEBUG_VENDOR_INDEX  = 0; // GENERIC
        private const int    DEBUG_MODEL_INDEX   = 0; // 4D PF2
        private const int    DEBUG_MTRNAME_INDEX = 0; // Pos 4D PF2 CCF
        private const string DEBUG_INITREADING   = "222222222222"; //"123456789012"; // 12
        private const int    DEBUG_ALARM_INDEX   = 0; // All
        private const int    DEBUG_DEMAND_INDEX  = 0;
        private const string DEBUG_READSINTERVAL = "1 Hour";
        private const string DEBUG_SNAPSREADS    = "10";
        private const bool   DEBUG_SNAPSREADS_OK = false;
        private const string DEBUG_GPS_LAT       = "43,316";
        private const string DEBUG_GPS_LON       = "-2.981";
        private const string DEBUG_GPS_ALT       = "1";
        private const int    DEBUG_MTULOCATION   = 0; // Outside
        private const int    DEBUG_METERLOC      = 0; // Outside
        private const int    DEBUG_CONSTRUC      = 0; // Vinyl

        #endregion

        #region Mandatory

        public const bool MANDATORY_ACCOUNTNUMBER   = true;
        public const bool MANDATORY_WORKORDER       = true;
        public const bool MANDATORY_OLDMTUID        = true;
        public const bool MANDATORY_OLDMETERSERIAL  = true;
        //public const bool MANDATORY_OLDMETERWORKING >> global.MeterWorkRecording
        public const bool MANDATORY_OLDMETERREADING = true;
        //public const bool MANDATORY_REPLACEMETER >> global.RegisterRecordingReq
        public const bool MANDATORY_METERSERIAL     = true;
        public const bool MANDATORY_METERTYPE       = true;
        public const bool MANDATORY_METERREADING    = true;
        public const bool MANDATORY_READINTERVAL    = true; // Init with select value
        public const bool MANDATORY_SNAPREADS       = true; // Init with select value
        public const bool MANDATORY_TWOWAY          = true;
        public const bool MANDATORY_ALARMS          = true;
        public const bool MANDATORY_DEMANDS         = true;
        public const bool MANDATORY_GPS             = false;

        #endregion

        #region Constants


        public  const string TWOWAY_FAST     = "Fast";
        public  const string TWOWAY_SLOW     = "Slow";
        public  const string CANCEL_COMPLETE = "Complete";
        public  const string CANCEL_CANCEL   = "Cancel";
        public  const string CANCEL_SKIP     = "Skip";
        public  const string CANCEL_NOTHOME  = "Not Home";
        public  const string CANCEL_OTHER    = "Other";
        public  const string SWITCH_P2_ON    = "Enable Port 2";
        public  const string SWITCH_P2_OFF   = "Disable Port 2";
        public  const string COPY_1_TO_2     = "Copy Port 1";
        private const string AUTO_DETECTING  = "Encoder auto-detect...";
        private       Color  COL_MANDATORY   = Color.FromHex ( "#FF0000" );
        private const int    MAX_ACCOUNTNUMBER = 12;
        private const int    MAX_METERREADING  = 12;

        private const string DUAL_PREFIX    = "Repeat ";
        private const string OLD_PREFIX     = "Old ";
        private const string TEXT_READING   = "Meter Reading";
        
        private const string DUAL_ERROR      = "* Both values ​​must be the same";
        private const string FIRST_METERTYPE = "* You should select Meter Type before enter readings";
        private const float  OPACITY_ENABLE  = 1;
        private const float  OPACITY_DISABLE = 0.8f;
        
        private const string LB_PORT1 = "Port 1";
        private const string LB_PORT2 = "Port 2";
        private const string LB_MISC  = "Miscellaneous";

        #endregion

        #region GUI Elements

        private List<PageItem> MenuList;

        private IUserDialogs dialogsSaved;
        private bool _userTapped;

        // Meter Type for Port 1 and 2
        private StackLayout      divDyna_MeterType_Vendors;
        private StackLayout      divDyna_MeterType_Models;
        private StackLayout      divDyna_MeterType_Names;
        private StackLayout      divDyna_MeterType_Vendors_2;
        private StackLayout      divDyna_MeterType_Models_2;
        private StackLayout      divDyna_MeterType_Names_2;
        private BorderlessPicker pck_MeterType_Vendors;
        private BorderlessPicker pck_MeterType_Models;
        private BorderlessPicker pck_MeterType_Names;
        private BorderlessPicker pck_MeterType_Vendors_2;
        private BorderlessPicker pck_MeterType_Models_2;
        private BorderlessPicker pck_MeterType_Names_2;
        private List<string>     list_MeterType_Vendors;
        private List<string>     list_MeterType_Models;
        private List<string>     list_MeterType_Names;
        private List<string>     list_MeterType_Vendors_2;
        private List<string>     list_MeterType_Models_2;
        private List<string>     list_MeterType_Names_2;
        private List<Xml.Meter>  list_MeterTypesForMtu;
        private List<Xml.Meter>  list_MeterTypesForMtu_2;
        private string           selected_MeterType_Vendor;
        private string           selected_MeterType_Vendor_2;
        private string           selected_MeterType_Model;
        private string           selected_MeterType_Model_2;
        private string           selected_MeterType_Name;
        private string           selected_MeterType_Name_2;
        
        // Miscelanea
        private List<BorderlessPicker> optionalPickers;
        private List<BorderlessEntry>  optionalEntries;
        private List<BorderlessDatePicker> optionalDates;
        private List<BorderlessTimePicker> optionalTimes;
        private List<Tuple<BorderlessPicker,Label>> optionalMandatoryPickers;
        private List<Tuple<BorderlessEntry,Label>> optionalMandatoryEntries;
        private List<Tuple<BorderlessDatePicker, Label>> optionalMandatoryDates;
        private List<Tuple<BorderlessTimePicker, Label>> optionalMandatoryTimes;

        // Snap Reads / Daily Reads
        private double snapReadsStep;

        // Alarms
        private List<Alarm> alarmsList  = new List<Alarm>();
        private List<Alarm> alarms2List = new List<Alarm>();

        // Demands
        private List<Demand> demandsList  = new List<Demand>();
        private List<Demand> demands2List = new List<Demand>();

        #endregion

        #region Attributes

        ZXingScannerPage scanPage;
        private Configuration config;
        private MTUComm.Action add_mtu;
        private AddMtuForm addMtuForm;
        private int detectedMtuType;
        private Mtu currentMtu;
        private Global global;
        private MTUBasicInfo mtuBasicInfo;
        private MenuView menuOptions;
        private DialogsView dialogView;
        private BottomBar bottomBar;

        private List<ReadMTUItem> FinalReadListView { get; set; }
        private List<FileInfo> PicturesMTU;

        private bool barCodeEnabled;
        public bool BarCodeEnabled
        {
            get => barCodeEnabled;
            set
            {
                barCodeEnabled = value;
                OnPropertyChanged();
            }
        }
        private bool imagesEnabled;
        public bool ImagesEnabled
        {
            get => imagesEnabled;
            set
            {
                imagesEnabled = value;
                OnPropertyChanged();
            }
        }
        private ActionType actionType;
        private ActionType actionTypeNew;
        private bool hasTwoPorts;
        private bool port2IsActivated;
        private bool p1NoNewMeterReadings;
        private bool p2NoNewMeterReadings;
        private bool waitOnClickLogic;
        private bool isCancellable;
        private bool isLogout;
        private bool isReturn;
        private bool isSettings;
        private bool   snapReadsStatus = false;
        private string mtuGeolocationAlt;

        #endregion

        #region Initialization

        public AclaraViewAddMTU ()
        {
            InitializeComponent ();
        }

        public AclaraViewAddMTU ( IUserDialogs dialogs, ActionType page )
        {
            InitializeComponent ();
            BindingContext = this;
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
            this.mtuBasicInfo = Data.Get.MtuBasicInfo;

            this.detectedMtuType = ( int )this.mtuBasicInfo.Type;
            currentMtu = this.config.GetMtuTypeById ( this.detectedMtuType );
            
            this.addMtuForm = new AddMtuForm ( currentMtu );
            
            this.add_mtu = new MTUComm.Action (
                FormsApp.ble_interface,
                this.actionType,
                FormsApp.credentialsService.UserName );
            
            isCancellable = false;

            PicturesMTU = new List<FileInfo>();

            Device.BeginInvokeOnMainThread(() =>
            {
                string[] texts = MTUComm.Action.actionsTexts[ this.actionType ];
            
                name_of_window_port1  .Text   = texts[ 0 ] + " - " + LB_PORT1;
                name_of_window_port2  .Text   = texts[ 1 ] + " - " + LB_PORT2;
                name_of_window_misc   .Text   = texts[ 2 ] + " - " + LB_MISC;
                bottomBar.GetImageElement("bg_action_button_img").Source = texts[ 3 ];
                //bottomBar.GetImageButtonElement("bg_action_button").Source = texts[3];

                bottomBar.GetLabelElement("label_read").Opacity    = 1;
                //backdark_bg.IsVisible = false;
                //indicator.IsVisible   = false;
                BarCodeEnabled = global.ShowBarCodeButton;
                ImagesEnabled = global.ShowCameraButton;

                if ( Device.Idiom == TargetIdiom.Tablet )
                     LoadTabletUI ();
                else LoadPhoneUI ();
                
                NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar
                
                //TopBar.GetImageElement("battery_level").Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
                //TopBar.GetImageElement("rssi_level").Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");
                
           
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

  
            _userTapped = false;
            
            TappedListeners ();
            InitializeLowerbarLabel();
            InitializeAddMtuForm ();
            
            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;

            Task.Run ( async () =>
            {
                await LoadMetersList ();
            })
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
                            
                            #region Port 2 Buttons Listener
                            
                            Task.Factory.StartNew(SetPort2Buttons);
                            
                            #endregion
                            
                            #region Snap Read CheckBox Controller
                            
                            CheckBoxController();
                            
                            #endregion
                        })
                    );
                })
            );
        }
        
        private void CheckBoxController ()
        {
            if ( ! div_SnapReads.IsEnabled )
                return;
        
            sld_SnapReads.Value = global.DailyReadsDefault;
            sld_SnapReads.IsEnabled = false;
            divSub_SnapReads.Opacity = 0.8;
            cbx_SnapReads.Source = "checkbox_off";

            snapReadsStep = 1.0;
            sld_SnapReads.ValueChanged += OnSnapReadsSlider_ValueChanged;

            cbx_SnapReads.GestureRecognizers.Add ( new TapGestureRecognizer
            {
                Command = new Command ( () =>
                {
                    bool ok = ! snapReadsStatus;
                
                    cbx_SnapReads.Source = "checkbox_" + ( ( ok ) ? "on" : "off" );
                    sld_SnapReads.IsEnabled = ok;
                    divSub_SnapReads.Opacity = ( ok ) ? 1 : OPACITY_DISABLE;
                    snapReadsStatus = ok;
                    
                    if ( MANDATORY_SNAPREADS &&
                         global.IndividualDailyReads &&
                         snapReadsStatus )
                         this.lb_SnapReads.TextColor = COL_MANDATORY;
                    else this.lb_SnapReads.TextColor = Color.Black;
                }),
            });
        }

        private async void SetPort2Buttons ()
        {
            // Port2 form starts visible or hidden depends on bit 1 of byte 28
            this.port2IsActivated = await Data.Get.MemoryMap.P2StatusFlag.GetValue();

            Device.BeginInvokeOnMainThread(() =>
            {
                // Switch On|Off port2 form
                if ( ! global.Port2DisableNo)
                {
                    BtnSwitchPort2.Tapped += OnClick_BtnSwitchPort2;
                    div_EnablePort2.IsEnabled = true;
                }

                // Copy current values of port1 form controls to port2 form controls
                btn_CopyPort1To2.GestureRecognizers.Add ( new TapGestureRecognizer
                {
                    Command = new Command ( () =>
                    {
                        if ( this.port2IsActivated )
                        {
                            this.tbx_AccountNumber_2     .Text = this.tbx_AccountNumber.Text;
                            this.tbx_AccountNumber_Dual_2.Text = this.tbx_AccountNumber_Dual.Text;
                            this.tbx_WorkOrder_2         .Text = this.tbx_WorkOrder.Text;
                            this.tbx_WorkOrder_Dual_2    .Text = this.tbx_WorkOrder_Dual.Text;

                            if ( global.NewMeterPort2isTheSame )
                            {
                                this.tbx_MeterSerialNumber_2     .Text = this.tbx_MeterSerialNumber.Text;
                                this.tbx_MeterSerialNumber_Dual_2.Text = this.tbx_MeterSerialNumber_Dual.Text;
                            }
                        }
                    }),
                });
            });
            
            this.UpdatePortButtons ();
        }

        private async Task LoadMetersList ()
        {
            if ( currentMtu.Port1.IsForEncoderOrEcoder ||
                 hasTwoPorts &&
                 currentMtu.Port2.IsForEncoderOrEcoder )
                Device.BeginInvokeOnMainThread ( () =>
                {
                    bottomBar.GetLabelElement("label_read").Text = AUTO_DETECTING;
                });

            // Ecoder/Encoder
            if ( currentMtu.Port1.IsForEncoderOrEcoder )
            {
                bool autoDetect = await this.add_mtu.MTUComm.AutodetectMeterEncoders ( currentMtu );
                if ( autoDetect )
                    this.list_MeterTypesForMtu = this.config.meterTypes.FindByEncoderTypeAndLiveDigits (
                        currentMtu.Port1.MeterProtocol,
                        currentMtu.Port1.MeterLiveDigits );

                // If auto-detect fails, show all Encoder/Ecoder Meters    
                if ( ! autoDetect ||
                     this.list_MeterTypesForMtu.Count <= 0 )
                    this.list_MeterTypesForMtu = this.config.meterTypes.FindAllForEncodersAndEcoders ();
            }
            // Pulse
            else
                this.list_MeterTypesForMtu = this.config.meterTypes.FindByPortTypeAndFlow ( currentMtu );

            if ( hasTwoPorts )
            {
                // Ecoder/Encoder
                if ( currentMtu.Port2.IsForEncoderOrEcoder )
                {
                    bool autoDetect = await this.add_mtu.MTUComm.AutodetectMeterEncoders ( currentMtu, 2 );
                    if ( autoDetect )
                        this.list_MeterTypesForMtu_2 = this.config.meterTypes.FindByEncoderTypeAndLiveDigits (
                            currentMtu.Port2.MeterProtocol,
                            currentMtu.Port2.MeterLiveDigits );

                    // If auto-detect fails, show all Encoder/Ecoder Meters    
                    if ( ! autoDetect ||
                         this.list_MeterTypesForMtu_2.Count <= 0 )
                        this.list_MeterTypesForMtu_2 = this.config.meterTypes.FindAllForEncodersAndEcoders ();
                }
                // Pulse
                else
                    this.list_MeterTypesForMtu_2 = this.config.meterTypes.FindByPortTypeAndFlow ( currentMtu, 1 );
            }
            
            Device.BeginInvokeOnMainThread ( () =>
            {
                this.InitializePicker_MeterType ();
                if ( hasTwoPorts )
                    this.InitializePicker_MeterType_2 ();
            });

            bool ShowMeterVendor = global.ShowMeterVendor;
            if (ShowMeterVendor)
            {
                // TODO: group meters by vendor / model / name
            }
            else
            {
                // TODO: display meter list directly, by  name
            }
        }

        private void InitializeAddMtuForm ()
        {
            #region Conditions

            this.currentMtu   = this.addMtuForm.mtu;
            this.mtuBasicInfo = Data.Get.MtuBasicInfo;

            #endregion

            #region Two ports

            this.hasTwoPorts = this.currentMtu.TwoPorts;
            port2label.IsVisible = this.hasTwoPorts;

            #endregion

            #region Account Number / Service Port ID

            // Dual entry
            bool useDualAccountNumber = global.AccountDualEntry;

            // Port 1
            this.div_AccountNumber_Dual.IsVisible = useDualAccountNumber;
            this.div_AccountNumber_Dual.IsEnabled = useDualAccountNumber;
            
            // Port 2
            this.div_AccountNumber_Dual_2.IsVisible = hasTwoPorts && useDualAccountNumber;
            this.div_AccountNumber_Dual_2.IsEnabled = hasTwoPorts && useDualAccountNumber;

            #endregion

            #region Work Order / Field Order

            bool useWorkOrder = global.WorkOrderRecording;

            // Port 1            
            this.div_WorkOrder.IsVisible = useWorkOrder;
            this.div_WorkOrder.IsEnabled = useWorkOrder;
            
            // Port 2
            this.div_WorkOrder_2.IsVisible = hasTwoPorts && useWorkOrder;
            this.div_WorkOrder_2.IsEnabled = hasTwoPorts && useWorkOrder;
            
            // Dual entry
            bool useDualWorkOrder = global.WorkOrderDualEntry && useWorkOrder;
            
            // Port 1
            this.div_WorkOrder_Dual.IsVisible = useDualWorkOrder;
            this.div_WorkOrder_Dual.IsEnabled = useDualWorkOrder;
            
            // Port 2
            this.div_WorkOrder_Dual_2.IsVisible = hasTwoPorts && useDualWorkOrder;
            this.div_WorkOrder_Dual_2.IsEnabled = hasTwoPorts && useDualWorkOrder;

            #endregion

            #region Old MTU ID

            bool isReplaceMtu = (
                this.actionType == ActionType.ReplaceMTU ||
                this.actionType == ActionType.ReplaceMtuReplaceMeter );

            this.div_OldMtuId.IsVisible = isReplaceMtu;
            this.div_OldMtuId.IsEnabled = isReplaceMtu;

            #endregion

            #region Meter Serial Number and Reading ( and OLD fields )

            this.Initialize_OldMeterPickers ();

            // Action is about Replace Meter
            bool isReplaceMeter = (
                this.actionType == ActionType.ReplaceMeter           ||
                this.actionType == ActionType.ReplaceMtuReplaceMeter ||
                this.actionType == ActionType.AddMtuReplaceMeter );

            // ( New ) Meter Serial Number
            bool useMeterSerialNumber = global.UseMeterSerialNumber;

            // Port 1
            this.div_MeterSerialNumber.IsVisible = useMeterSerialNumber;
            this.div_MeterSerialNumber.IsEnabled = useMeterSerialNumber;
            
            // Port 2
            this.div_MeterSerialNumber_2.IsVisible = hasTwoPorts && useMeterSerialNumber;
            this.div_MeterSerialNumber_2.IsEnabled = hasTwoPorts && useMeterSerialNumber;
            
            ///////////////////////////////////
            // Dual entry - ( New ) Meter Serial Number
            bool useDualSeriaNumber = global.NewSerialNumDualEntry && useMeterSerialNumber;
            
            // Port 1
            this.div_MeterSerialNumber_Dual.IsVisible = useDualSeriaNumber;
            this.div_MeterSerialNumber_Dual.IsEnabled = useDualSeriaNumber;
            
            // Port 2
            this.div_MeterSerialNumber_Dual_2.IsVisible = hasTwoPorts && useDualSeriaNumber;
            this.div_MeterSerialNumber_Dual_2.IsEnabled = hasTwoPorts && useDualSeriaNumber;
            
            ///////////////////////////////////
            // ( New ) Meter Reading
            this.p1NoNewMeterReadings = ! this.currentMtu.Port1.IsForEncoderOrEcoder;
            this.p2NoNewMeterReadings = this.hasTwoPorts && ! this.currentMtu.Port2.IsForEncoderOrEcoder;

            // Port 1
            this.div_MeterReadings.IsVisible = this.p1NoNewMeterReadings;
            this.div_MeterReadings.IsEnabled = this.p1NoNewMeterReadings;

            // Port 2
            this.div_MeterReadings_2.IsVisible = this.p2NoNewMeterReadings;
            this.div_MeterReadings_2.IsEnabled = this.p2NoNewMeterReadings;

            ///////////////////////////////////
            // Dual Entry - ( New ) Meter Reading
            bool useDualMeterReading = global.ReadingDualEntry;

            // Port 1
            this.div_MeterReading_Dual.IsVisible = this.p1NoNewMeterReadings && useDualMeterReading;
            this.div_MeterReading_Dual.IsEnabled = this.p1NoNewMeterReadings && useDualMeterReading;
            
            // Port 2
            this.div_MeterReading_Dual_2.IsVisible = this.p2NoNewMeterReadings && useDualMeterReading;
            this.div_MeterReading_Dual_2.IsEnabled = this.p2NoNewMeterReadings && useDualMeterReading;

            ///////////////////////////////////
            // Old Meter Serial Number
            // Port 1
            this.div_OldMeterSerialNumber.IsVisible = isReplaceMeter && useMeterSerialNumber;
            this.div_OldMeterSerialNumber.IsEnabled = isReplaceMeter && useMeterSerialNumber;
            
            // Port 2
            this.div_OldMeterSerialNumber_2.IsVisible = hasTwoPorts && isReplaceMeter && useMeterSerialNumber;
            this.div_OldMeterSerialNumber_2.IsEnabled = hasTwoPorts && isReplaceMeter && useMeterSerialNumber;
            
            ///////////////////////////////////
            // Dual entry - Old Meter Serial Number
            bool useDualOldSeriaNumber = global.OldSerialNumDualEntry && this.div_OldMeterSerialNumber.IsVisible;
            
            // Port 1
            this.div_OldMeterSerialNumber_Dual.IsVisible = useDualOldSeriaNumber;
            this.div_OldMeterSerialNumber_Dual.IsEnabled = useDualOldSeriaNumber;
            
            // Port 2
            this.div_OldMeterSerialNumber_Dual_2.IsVisible = hasTwoPorts && useDualOldSeriaNumber;
            this.div_OldMeterSerialNumber_Dual_2.IsEnabled = hasTwoPorts && useDualOldSeriaNumber;
            
            ///////////////////////////////////
            // Old Meter Working ( Change reason )
            bool useMeterWorking = isReplaceMeter && global.MeterWorkRecording;
            
            // Port 1
            this.div_OldMeterWorking.IsVisible = useMeterWorking;
            this.div_OldMeterWorking.IsEnabled = useMeterWorking;
            
            // Port 2
            this.div_OldMeterWorking_2.IsVisible = hasTwoPorts && useMeterWorking;
            this.div_OldMeterWorking_2.IsEnabled = hasTwoPorts && useMeterWorking;

            ///////////////////////////////////
            // Old Meter Reading
            bool useOldReading = isReplaceMeter && global.OldReadingRecording;
            
            // Port 1
            this.div_OldMeterReading.IsVisible = useOldReading;
            this.div_OldMeterReading.IsEnabled = useOldReading;
            
            // Port 2
            this.div_OldMeterReading_2.IsVisible = hasTwoPorts && useOldReading;
            this.div_OldMeterReading_2.IsEnabled = hasTwoPorts && useOldReading;
            
            ///////////////////////////////////
            // Dual entry - Old Meter Reading
            bool useDualOldReading = global.OldReadingDualEntry && useOldReading;
            
            // Port 1
            this.div_OldMeterReading_Dual.IsVisible = useDualOldReading;
            this.div_OldMeterReading_Dual.IsEnabled = useDualOldReading;
            
            // Port 2
            this.div_OldMeterReading_Dual_2.IsVisible = hasTwoPorts && useDualOldReading;
            this.div_OldMeterReading_Dual_2.IsEnabled = hasTwoPorts && useDualOldReading;
            
            ///////////////////////////////////
            // Replace Meter/Register
            bool useReplaceMeterRegister = isReplaceMeter && global.RegisterRecording;
            
            // Port 1
            this.div_ReplaceMeterRegister.IsVisible = useReplaceMeterRegister;
            this.div_ReplaceMeterRegister.IsEnabled = useReplaceMeterRegister;
            
            // Port 2
            this.div_ReplaceMeterRegister_2.IsVisible = hasTwoPorts && useReplaceMeterRegister;
            this.div_ReplaceMeterRegister_2.IsEnabled = hasTwoPorts && useReplaceMeterRegister;

            ///////////////////////////////////
            // Introduce values from right to left, but finish with the same number
            // e.g. 1 _ _ -> 1 2 _ -> 1 2 3
            //      _ _ 3 -> _ 2 3 -> 1 2 3
            FlowDirection flow = ( global.ReverseReading ) ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            this.tbx_MeterReading          .FlowDirection = flow;
            this.tbx_MeterReading_2        .FlowDirection = flow;
            this.tbx_MeterReading_Dual     .FlowDirection = flow;
            this.tbx_MeterReading_Dual_2   .FlowDirection = flow;
            this.tbx_OldMeterReading       .FlowDirection = flow;
            this.tbx_OldMeterReading_2     .FlowDirection = flow;
            this.tbx_OldMeterReading_Dual  .FlowDirection = flow;
            this.tbx_OldMeterReading_Dual_2.FlowDirection = flow;
            
            ///////////////////////////////////
            // New Meter readings will be disabled before meter type was selected
            this.tbx_MeterReading       .IsEnabled = false;
            this.tbx_MeterReading_2     .IsEnabled = false;
            this.tbx_MeterReading_Dual  .IsEnabled = false;
            this.tbx_MeterReading_Dual_2.IsEnabled = false;
            
            this.div_MeterReading          .Opacity = OPACITY_DISABLE;
            this.divSub_MeterReading_Dual  .Opacity = OPACITY_DISABLE;
            this.divSub_MeterReading_2     .Opacity = OPACITY_DISABLE;
            this.divSub_MeterReading_Dual_2.Opacity = OPACITY_DISABLE;
            
            this.lb_MeterReading_MeterType      .Text = FIRST_METERTYPE;
            this.lb_MeterReading_DualMeterType  .Text = FIRST_METERTYPE;
            this.lb_MeterReading_MeterType_2    .Text = FIRST_METERTYPE;
            this.lb_MeterReading_DualMeterType_2.Text = FIRST_METERTYPE;
            
            this.lb_MeterReading_MeterType      .IsVisible = true;
            this.lb_MeterReading_DualMeterType  .IsVisible = true;
            this.lb_MeterReading_MeterType_2    .IsVisible = true;
            this.lb_MeterReading_DualMeterType_2.IsVisible = true;

            #endregion

            #region Read Interval

            this.InitializePicker_ReadInterval ( this.mtuBasicInfo, this.currentMtu );
            
            // Use IndividualReadInterval tag to enable o disable read interval picker
            if ( ! ( this.pck_ReadInterval.IsEnabled = global.IndividualReadInterval ) )
            {
                this.div_ReadInterval.BackgroundColor = Color.LightGray;
                this.pck_ReadInterval.BackgroundColor = Color.LightGray;
                this.pck_ReadInterval.TextColor       = Color.Gray;
            }

            #endregion

            #region Snap Reads / Daily Reads

            bool useDailyReads        = global.AllowDailyReads && this.currentMtu.DailyReads && ! this.currentMtu.IsFamily33xx;
            bool changeableDailyReads = global.IndividualDailyReads;
            int  dailyReadsDefault    = global.DailyReadsDefault;
            
            this.div_SnapReads   .IsEnabled = useDailyReads;
            this.div_SnapReads   .IsVisible = useDailyReads;
            this.divSub_SnapReads.IsEnabled = changeableDailyReads && useDailyReads;
            this.divSub_SnapReads.Opacity   = ( changeableDailyReads && useDailyReads ) ? 1 : 0.8d;

            this.snapReadsStep  = 1.0;

            if ( useDailyReads )
                this.sld_SnapReads.ValueChanged += OnSnapReadsSlider_ValueChanged;

            this.sld_SnapReads.Value = ( dailyReadsDefault > -1 ) ? dailyReadsDefault : 13;

            #endregion

            #region 2-Way

            // Only for 34xx MTUs and above
            bool useTwoWay = global.TimeToSync          &&
                             this.currentMtu.TimeToSync &&
                             this.currentMtu.FastMessageConfig;
            
            this.Initialize_TwoWay ();

            div_TwoWay.IsVisible = useTwoWay;
            div_TwoWay.IsEnabled = useTwoWay;

            #endregion

            #region Alarms

            alarmsList  = config.alarms.FindByMtuType ( this.detectedMtuType );
            alarms2List = ( hasTwoPorts ) ? config.alarms.FindByMtuType ( this.detectedMtuType ) : new List<Alarm> ();

            // Remove "Scripting" option in interactive mode
            alarmsList  = alarmsList .FindAll ( alarm => ! string.Equals ( alarm.Name.ToLower (), "scripting" ) );
            alarms2List = alarms2List.FindAll ( alarm => ! string.Equals ( alarm.Name.ToLower (), "scripting" ) );

            bool RequiresAlarmProfile = this.currentMtu.RequiresAlarmProfile;
            bool portHasSomeAlarm     = ( RequiresAlarmProfile && alarmsList.Count > 0 );

            pck_Alarms.ItemDisplayBinding = new Binding ( "Name" );

            div_Alarms.IsEnabled   = portHasSomeAlarm;
            div_Alarms.IsVisible   = portHasSomeAlarm;
            pck_Alarms   .ItemsSource = alarmsList;

            // Hide alarms dropdownlist if contains only one option
            div_Alarms.IsVisible = ( alarmsList .Count > 1 );

            #endregion

            #region Demands

            demandsList  = config.demands.FindByMtuType ( this.detectedMtuType );
            demands2List = ( hasTwoPorts ) ? config.demands.FindByMtuType ( this.detectedMtuType ) : new List<Demand> ();

            bool MtuDemand          = this.currentMtu.MtuDemand;
            bool portHasSomeDemand  = ( MtuDemand && demandsList.Count > 0 );
            bool port2HasSomeDemand = ( hasTwoPorts && MtuDemand && demands2List.Count > 0 );

            pck_Demands.ItemDisplayBinding = new Binding ( "Name" );

            div_Demands.IsEnabled   = portHasSomeDemand;
            div_Demands.IsVisible   = portHasSomeDemand;
            pck_Demands   .ItemsSource = demandsList;

            #endregion

            #region Misc

            if ( this.global.Options.Count>0)
                InitializeOptionalFields ();

            #endregion

            #region Cancel reason

            // Load cancel reasons from Global.xml
            List<string> cancelReasons = this.global.Cancel;

            if(!cancelReasons.Contains("Other"))
                cancelReasons.Add("Other"); // Add "Other" option

            cancelReasonPicker.ItemsSource = cancelReasons;

            // Select default reason
            cancelReasonPicker.SelectedIndex = 0;

            // Enable "Other" reason box if default is "Other"
            string defaultCancelReason = cancelReasonPicker.Items[cancelReasonPicker.SelectedIndex];
            if ("Other".Equals(defaultCancelReason))
            {
                cancelReasonOtherInput.IsEnabled = true;
                cancelReasonOtherInputContainer.Opacity = 1;
            }
            else
            {
                cancelReasonOtherInput.IsEnabled = false;
                cancelReasonOtherInputContainer.Opacity = 0.8;
            }

            // Add handler
            cancelReasonPicker.SelectedIndexChanged += CancelReasonPicker_SelectedIndexChanged;

            #endregion

            #region Set Max

            // Set maximum values from global.xml
            int acnLength = ( global.AccountLength <= MAX_ACCOUNTNUMBER ) ? global.AccountLength : MAX_ACCOUNTNUMBER;
            tbx_AccountNumber              .MaxLength = acnLength;
            tbx_AccountNumber_2            .MaxLength = acnLength;
            tbx_AccountNumber_Dual         .MaxLength = acnLength;
            tbx_AccountNumber_Dual_2       .MaxLength = acnLength;
            
            tbx_WorkOrder                  .MaxLength = global.WorkOrderLength;
            tbx_WorkOrder_2                .MaxLength = global.WorkOrderLength;
            tbx_WorkOrder_Dual             .MaxLength = global.WorkOrderLength;
            tbx_WorkOrder_Dual_2           .MaxLength = global.WorkOrderLength;
            
            tbx_OldMtuId                   .MaxLength = global.MtuIdLength;
            
            tbx_OldMeterSerialNumber       .MaxLength = global.MeterNumberLength;
            tbx_OldMeterSerialNumber_2     .MaxLength = global.MeterNumberLength;
            tbx_OldMeterSerialNumber_Dual  .MaxLength = global.MeterNumberLength;
            tbx_OldMeterSerialNumber_Dual_2.MaxLength = global.MeterNumberLength;
            
            tbx_MeterSerialNumber          .MaxLength = global.MeterNumberLength;
            tbx_MeterSerialNumber_2        .MaxLength = global.MeterNumberLength;
            tbx_MeterSerialNumber_Dual     .MaxLength = global.MeterNumberLength;
            tbx_MeterSerialNumber_Dual_2   .MaxLength = global.MeterNumberLength;
            
            tbx_OldMeterReading            .MaxLength = MAX_METERREADING;
            tbx_OldMeterReading_2          .MaxLength = MAX_METERREADING;
            tbx_OldMeterReading_Dual       .MaxLength = MAX_METERREADING;
            tbx_OldMeterReading_Dual_2     .MaxLength = MAX_METERREADING;
            
            tbx_MeterReading               .MaxLength = MAX_METERREADING;
            tbx_MeterReading_2             .MaxLength = MAX_METERREADING;
            tbx_MeterReading_Dual          .MaxLength = MAX_METERREADING;
            tbx_MeterReading_Dual_2        .MaxLength = MAX_METERREADING;

            tbx_MtuGeolocationLat          .MaxLength = 20;
            tbx_MtuGeolocationLong         .MaxLength = 20;

            #endregion

            #region Labels

            // Account Number
            this.lb_AccountNumber       .Text = global.AccountLabel;
            this.lb_AccountNumber_Dual  .Text = DUAL_PREFIX + global.AccountLabel;
            this.lb_AccountNumber_2     .Text = global.AccountLabel;
            this.lb_AccountNumber_Dual_2.Text = DUAL_PREFIX + global.AccountLabel;
            
            // Work Order
            this.lb_WorkOrder       .Text = global.WorkOrderLabel;
            this.lb_WorkOrder_Dual  .Text = DUAL_PREFIX + global.WorkOrderLabel;
            this.lb_WorkOrder_2     .Text = global.WorkOrderLabel;
            this.lb_WorkOrder_Dual_2.Text = DUAL_PREFIX + global.WorkOrderLabel;
            
            string NEW_LABEL = ( isReplaceMeter ) ? "New " : string.Empty;
            
            // Meter Reading
            this.lb_MeterReading       .Text = NEW_LABEL + TEXT_READING;
            this.lb_MeterReading_Dual  .Text = DUAL_PREFIX + NEW_LABEL + TEXT_READING;
            this.lb_MeterReading_2     .Text = NEW_LABEL + TEXT_READING;
            this.lb_MeterReading_Dual_2.Text = DUAL_PREFIX + NEW_LABEL + TEXT_READING;
            
            this.lb_OldMeterReading       .Text = OLD_PREFIX + TEXT_READING;
            this.lb_OldMeterReading_Dual  .Text = DUAL_PREFIX + OLD_PREFIX + TEXT_READING;
            this.lb_OldMeterReading_2     .Text = OLD_PREFIX + TEXT_READING;
            this.lb_OldMeterReading_Dual_2.Text = DUAL_PREFIX + OLD_PREFIX + TEXT_READING;
            
            // Meter Serial Number
            if ( useMeterSerialNumber )
            {
                if ( isReplaceMeter )
                {
                    this.lb_MeterSerialNumber       .Text = global.NewMeterLabel;
                    this.lb_MeterSerialNumber_Dual  .Text = DUAL_PREFIX + global.NewMeterLabel;
                    this.lb_MeterSerialNumber_2     .Text = global.NewMeterLabel;
                    this.lb_MeterSerialNumber_Dual_2.Text = DUAL_PREFIX + global.NewMeterLabel;
                    
                    this.lb_OldMeterSerialNumber       .Text = OLD_PREFIX + global.SerialNumLabel;
                    this.lb_OldMeterSerialNumber_Dual  .Text = DUAL_PREFIX + OLD_PREFIX + global.SerialNumLabel;
                    this.lb_OldMeterSerialNumber_2     .Text = OLD_PREFIX + global.SerialNumLabel;
                    this.lb_OldMeterSerialNumber_Dual_2.Text = DUAL_PREFIX + OLD_PREFIX + global.SerialNumLabel;
                }
                else
                {
                    this.lb_MeterSerialNumber       .Text = global.SerialNumLabel;
                    this.lb_MeterSerialNumber_Dual  .Text = DUAL_PREFIX + global.SerialNumLabel;
                    this.lb_MeterSerialNumber_2     .Text = global.SerialNumLabel;
                    this.lb_MeterSerialNumber_Dual_2.Text = DUAL_PREFIX + global.SerialNumLabel;
                    
                    this.lb_OldMeterSerialNumber       .Text = OLD_PREFIX + global.SerialNumLabel;
                    this.lb_OldMeterSerialNumber_Dual  .Text = DUAL_PREFIX + OLD_PREFIX + global.SerialNumLabel;
                    this.lb_OldMeterSerialNumber_2     .Text = OLD_PREFIX + global.SerialNumLabel;
                    this.lb_OldMeterSerialNumber_Dual_2.Text = DUAL_PREFIX + OLD_PREFIX + global.SerialNumLabel;
                }
            }

            #endregion

            #region Mandatory Fields

            if ( global.ColorEntry )
            {
                // Account Number
                if ( MANDATORY_ACCOUNTNUMBER )
                {
                    // Port 1
                    this.lb_AccountNumber     .TextColor = COL_MANDATORY;
                    this.lb_AccountNumber_Dual.TextColor = COL_MANDATORY;
                    
                    // Port 2
                    this.lb_AccountNumber_2     .TextColor = COL_MANDATORY;
                    this.lb_AccountNumber_Dual_2.TextColor = COL_MANDATORY;
                }
                
                // Work Order
                if ( MANDATORY_WORKORDER )
                {
                    // Port 1
                    this.lb_WorkOrder     .TextColor = COL_MANDATORY;
                    this.lb_WorkOrder_Dual.TextColor = COL_MANDATORY;
                    
                    // Port 2
                    this.lb_WorkOrder_2     .TextColor = COL_MANDATORY;
                    this.lb_WorkOrder_Dual_2.TextColor = COL_MANDATORY;
                }
                
                // Old MTU ID
                if ( MANDATORY_OLDMTUID )
                    this.lb_OldMtuId.TextColor = COL_MANDATORY;
                
                // Old Meter Serial Number
                if ( MANDATORY_OLDMETERSERIAL )
                {
                    // Port 1
                    this.lb_OldMeterSerialNumber     .TextColor = COL_MANDATORY;
                    this.lb_OldMeterSerialNumber_Dual.TextColor = COL_MANDATORY;
                    
                    // Port 2
                    this.lb_OldMeterSerialNumber_2     .TextColor = COL_MANDATORY;
                    this.lb_OldMeterSerialNumber_Dual_2.TextColor = COL_MANDATORY;
                }
                
                // Old Meter Working
                if ( global.MeterWorkRecording )
                {
                    // Port 1
                    this.lb_OldMeterWorking.TextColor = COL_MANDATORY;
                    
                    // Port 2
                    this.lb_OldMeterWorking_2.TextColor = COL_MANDATORY;
                }
                
                // Old Meter Reading
                if ( MANDATORY_OLDMETERREADING )
                {
                    // Port 1
                    this.lb_OldMeterReading     .TextColor = COL_MANDATORY;
                    this.lb_OldMeterReading_Dual.TextColor = COL_MANDATORY;
                    
                    // Port 2
                    this.lb_OldMeterReading_2     .TextColor = COL_MANDATORY;
                    this.lb_OldMeterReading_Dual_2.TextColor = COL_MANDATORY;
                }
                
                // Replace Meter|Register
                if ( global.RegisterRecordingReq )
                {
                    // Port 1
                    this.lb_ReplaceMeterRegister.TextColor = COL_MANDATORY;
                    
                    // Port 2
                    this.lb_ReplaceMeterRegister_2.TextColor = COL_MANDATORY;
                }
                
                // ( New ) Meter Serial Number
                if ( MANDATORY_METERSERIAL )
                {
                    // Port 1
                    this.lb_MeterSerialNumber     .TextColor = COL_MANDATORY;
                    this.lb_MeterSerialNumber_Dual.TextColor = COL_MANDATORY;
                    
                    // Port 2
                    this.lb_MeterSerialNumber_2     .TextColor = COL_MANDATORY;
                    this.lb_MeterSerialNumber_Dual_2.TextColor = COL_MANDATORY;
                }
                
                // ( New ) Meter Reading
                if ( MANDATORY_METERREADING )
                {
                    // Port 1
                    this.lb_MeterReading     .TextColor = COL_MANDATORY;
                    this.lb_MeterReading_Dual.TextColor = COL_MANDATORY;
                    
                    // Port 2
                    this.lb_MeterReading_2     .TextColor = COL_MANDATORY;
                    this.lb_MeterReading_Dual_2.TextColor = COL_MANDATORY;
                }
                
                // Read Interval
                if ( MANDATORY_READINTERVAL &&
                     global.IndividualReadInterval )
                    this.lb_ReadInterval.TextColor = COL_MANDATORY;
               
                // Snap Reads
                if ( MANDATORY_SNAPREADS &&
                     global.IndividualDailyReads &&
                     snapReadsStatus )
                    this.lb_SnapReads.TextColor = COL_MANDATORY;
                
                // Two-Way
                if ( MANDATORY_TWOWAY )
                    this.lb_TwoWay.TextColor = COL_MANDATORY;
                
                // Alarms
                if ( MANDATORY_ALARMS )
                    this.lb_Alarms.TextColor = COL_MANDATORY;
                    
                // Demands
                if ( MANDATORY_DEMANDS )
                    this.lb_Demands.TextColor = COL_MANDATORY;
                
                // GPS
                if ( MANDATORY_GPS )
                    this.lb_GPS.TextColor = COL_MANDATORY;
            }

            #endregion

            #region Port 2 Buttons

            // Button for enable|disable the second port
            if ( ! ( this.div_EnablePort2.IsEnabled = global.Port2DisableNo ) )
            {
                this.block_view_port2.IsVisible = this.port2IsActivated;
                this.btn_EnablePort2.Text       = ( this.port2IsActivated ) ? SWITCH_P2_OFF : SWITCH_P2_ON;
                //this.btn_EnablePort2.TextColor  = ( this.port2IsActivated ) ? Color.Gold : Color.White;
            }
            // Auto-enable second port because Port2DisableNo is true
            else
            {
                this.port2IsActivated           = true;
                this.block_view_port2.IsVisible = true;
                this.div_EnablePort2.IsVisible  = false;
                this.div_EnablePort2.IsEnabled  = false;
            }
            
            // Button for copy port 1 common fields values to port 2
            this.div_CopyPort1To2.IsVisible = this.port2IsActivated;
            this.div_CopyPort1To2.IsEnabled = this.port2IsActivated;
            
            btn_CopyPort1To2.Text = COPY_1_TO_2;

            #endregion

            #region UnFocus events

            // Account Number
            System.Action valEqAccountNumber = () =>
                ValidateEqualityOnFocus (
                    this.tbx_AccountNumber,
                    this.tbx_AccountNumber_Dual,
                    this.lb_AccountNumber_DualError );
            System.Action valEqAccountNumber_2 = () =>
                ValidateEqualityOnFocus (
                    this.tbx_AccountNumber_2,
                    this.tbx_AccountNumber_Dual_2,
                    this.lb_AccountNumber_DualError_2 );
            
            this.tbx_AccountNumber_Dual  .Unfocused += ( s, e ) => { valEqAccountNumber   (); };
            this.tbx_AccountNumber       .Unfocused += ( s, e ) => { valEqAccountNumber   (); };
            this.tbx_AccountNumber_2     .Unfocused += ( s, e ) => { valEqAccountNumber_2 (); };
            this.tbx_AccountNumber_Dual_2.Unfocused += ( s, e ) => { valEqAccountNumber_2 (); };
            
            // Work Order
            System.Action valEqWorkOrder = () =>
                ValidateEqualityOnFocus (
                    this.tbx_WorkOrder,
                    this.tbx_WorkOrder_Dual,
                    this.lb_WorkOrder_DualError );
            System.Action valEqWorkOrder_2 = () =>
                ValidateEqualityOnFocus (
                    this.tbx_WorkOrder_2,
                    this.tbx_WorkOrder_Dual_2,
                    this.lb_WorkOrder_DualError_2 );
            
            this.tbx_WorkOrder       .Unfocused += ( s, e ) => { valEqWorkOrder   (); };
            this.tbx_WorkOrder_Dual  .Unfocused += ( s, e ) => { valEqWorkOrder   (); };
            this.tbx_WorkOrder_2     .Unfocused += ( s, e ) => { valEqWorkOrder_2 (); };
            this.tbx_WorkOrder_Dual_2.Unfocused += ( s, e ) => { valEqWorkOrder_2 (); };
            
            // ( New ) Meter Serial Number
            System.Action valEqMeterSerialNumber = () =>
                ValidateEqualityOnFocus (
                    this.tbx_MeterSerialNumber,
                    this.tbx_MeterSerialNumber_Dual,
                    this.lb_MeterSerialNumber_DualError );
            System.Action valEqMeterSerialNumber_2 = () =>
                ValidateEqualityOnFocus (
                    this.tbx_MeterSerialNumber_2,
                    this.tbx_MeterSerialNumber_Dual_2,
                    this.lb_MeterSerialNumber_DualError_2 );
            
            this.tbx_MeterSerialNumber       .Unfocused += ( s, e ) => { valEqMeterSerialNumber   (); };
            this.tbx_MeterSerialNumber_Dual  .Unfocused += ( s, e ) => { valEqMeterSerialNumber   (); };
            this.tbx_MeterSerialNumber_2     .Unfocused += ( s, e ) => { valEqMeterSerialNumber_2 (); };
            this.tbx_MeterSerialNumber_Dual_2.Unfocused += ( s, e ) => { valEqMeterSerialNumber_2 (); };
            
            // Old Meter Serial Number
            System.Action valEqOldMeterSerialNumber = () =>
                ValidateEqualityOnFocus (
                    this.tbx_OldMeterSerialNumber,
                    this.tbx_OldMeterSerialNumber_Dual,
                    this.lb_OldMeterSerialNumber_DualError );
            System.Action valEqOldMeterSerialNumber_2 = () =>
                ValidateEqualityOnFocus (
                    this.tbx_OldMeterSerialNumber_2,
                    this.tbx_OldMeterSerialNumber_Dual_2,
                    this.lb_OldMeterSerialNumber_DualError_2 );
            
            this.tbx_OldMeterSerialNumber       .Unfocused += ( s, e ) => { valEqOldMeterSerialNumber   (); };
            this.tbx_OldMeterSerialNumber_Dual  .Unfocused += ( s, e ) => { valEqOldMeterSerialNumber   (); };
            this.tbx_OldMeterSerialNumber_2     .Unfocused += ( s, e ) => { valEqOldMeterSerialNumber_2 (); };
            this.tbx_OldMeterSerialNumber_Dual_2.Unfocused += ( s, e ) => { valEqOldMeterSerialNumber_2 (); };
            
            // ( New ) Meter Reading
            System.Action valEqMeterReading = () =>
                ValidateEqualityOnFocus (
                    this.tbx_MeterReading,
                    this.tbx_MeterReading_Dual,
                    this.lb_MeterReading_DualError );
            System.Action valEqMeterReading_2 = () =>
                ValidateEqualityOnFocus (
                    this.tbx_MeterReading_2,
                    this.tbx_MeterReading_Dual_2,
                    this.lb_MeterReading_DualError_2 );
            
            this.tbx_MeterReading       .Unfocused += ( s, e ) => { valEqMeterReading   (); };
            this.tbx_MeterReading_Dual  .Unfocused += ( s, e ) => { valEqMeterReading   (); };
            this.tbx_MeterReading       .Unfocused += ( s, e ) => { valEqMeterReading_2 (); };
            this.tbx_MeterReading_Dual_2.Unfocused += ( s, e ) => { valEqMeterReading_2 (); };
            
            // Old Meter Reading
            System.Action valOldEqMeterReading = () =>
                ValidateEqualityOnFocus (
                    this.tbx_OldMeterReading,
                    this.tbx_OldMeterReading_Dual,
                    this.lb_OldMeterReading_DualError );
            System.Action valOldEqMeterReading_2 = () =>
                ValidateEqualityOnFocus (
                    this.tbx_OldMeterReading_2,
                    this.tbx_OldMeterReading_Dual_2,
                    this.lb_OldMeterReading_DualError_2 );
            
            this.tbx_OldMeterReading       .Unfocused += ( s, e ) => { valOldEqMeterReading   (); };
            this.tbx_OldMeterReading_Dual  .Unfocused += ( s, e ) => { valOldEqMeterReading   (); };
            this.tbx_OldMeterReading       .Unfocused += ( s, e ) => { valOldEqMeterReading_2 (); };
            this.tbx_OldMeterReading_Dual_2.Unfocused += ( s, e ) => { valOldEqMeterReading_2 (); };

            this.lb_AccountNumber_DualError         .Text = DUAL_ERROR;
            this.lb_AccountNumber_DualError_2       .Text = DUAL_ERROR;
            this.lb_WorkOrder_DualError             .Text = DUAL_ERROR;
            this.lb_WorkOrder_DualError_2           .Text = DUAL_ERROR;
            this.lb_MeterSerialNumber_DualError     .Text = DUAL_ERROR;
            this.lb_MeterSerialNumber_DualError_2   .Text = DUAL_ERROR;
            this.lb_OldMeterSerialNumber_DualError  .Text = DUAL_ERROR;
            this.lb_OldMeterSerialNumber_DualError_2.Text = DUAL_ERROR;
            this.lb_MeterReading_DualError          .Text = DUAL_ERROR;
            this.lb_MeterReading_DualError_2        .Text = DUAL_ERROR;
            this.lb_OldMeterReading_DualError       .Text = DUAL_ERROR;
            this.lb_OldMeterReading_DualError_2     .Text = DUAL_ERROR;

            #endregion
            
            #region Labels
            
            this.port1label.Text = LB_PORT1;
            this.port2label.Text = LB_PORT2;
            this.misclabel .Text = LB_MISC;
                        
            #endregion
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
            bottomBar.GetImageButtonElement("btnTakePicture").Clicked += TakePicture;
            bottomBar.GetImageButtonElement("btnTakePicture").IsVisible = global.ShowCameraButton;
            TopBar.GetTGRElement("back_button").Tapped += ReturnToMainView;
            bottomBar.GetTGRElement("bg_action_button").Tapped += AddMtu;
            //bottomBar.GetImageButtonElement("bg_action_button").Clicked += AddMtu;


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

            dialogView.GetTGRElement("dialog_NoAction_ok").Tapped += dialog_cancelTapped;


            port1label.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => port1_command()),
            });
            misclabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => misc_command()),
            });
            port2label.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => port2_command()),
            });

            gps_icon_button.Tapped += GpsUpdateButton;

            submit_dialog.Clicked += submit_send;
            cancel_dialog.Clicked += Cancel_No;
                 
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
            Device.BeginInvokeOnMainThread(() =>
            {
                if (actionType == ActionType.DataRead)
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewDataRead(dialogsSaved,  this.actionType), false);
                else if(actionType == ActionType.RemoteDisconnect)
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewRemoteDisconnect(dialogsSaved,  this.actionType), false);
                else
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved,  this.actionType), false);
            });

            backdark_bg.IsVisible = false;
            indicator.IsVisible = false;
            background_scan_page.IsEnabled = true;
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

        private async void Confirm_Yes_LogOut ( object sender, EventArgs e )
        {
            // Upload log files
            if (global.UploadPrompt)
                await GenericUtilsClass.UploadFiles ();

            dialogView.OpenCloseDialog("dialog_logoff", false);
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
            dialogView.OpenCloseDialog("dialog_logoff", false);
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            //Navigation.PopToRootAsync(false);
        }

        #endregion

        private void InitializeLowerbarLabel ()
        {
            bottomBar.GetLabelElement("label_read").Text = "Push Button to START";
        }

        private void InitializePicker_MeterType ()
        {
            list_MeterType_Vendors = this.config.meterTypes.GetVendorsFromMeters(list_MeterTypesForMtu);

            Frame meterVendorsContainerB = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };
            
            Frame meterVendorsContainerC = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout meterVendorsContainerD = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };

            pck_MeterType_Vendors = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = list_MeterType_Vendors
            };

            pck_MeterType_Vendors.SelectedIndexChanged += MeterVendorsPicker_SelectedIndexChanged;

            divDyna_MeterType_Vendors = new StackLayout()
            {
                StyleId = "bloque" + 1
            };

            Label meterVendorsLabel = new Label()
            {
                Text = "Vendor",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            if ( this.global.ColorEntry )
                meterVendorsLabel.TextColor = COL_MANDATORY;

            meterVendorsContainerD.Children.Add(pck_MeterType_Vendors);
            meterVendorsContainerC.Content = meterVendorsContainerD;
            meterVendorsContainerB.Content = meterVendorsContainerC;
            divDyna_MeterType_Vendors.Children.Add(meterVendorsLabel);
            divDyna_MeterType_Vendors.Children.Add(meterVendorsContainerB);

            list_MeterType_Models = new List<string>();

            Frame meterModelsContainerB = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };

            Frame meterModelsContainerC = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout meterModelsContainerD = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };

            pck_MeterType_Models = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = list_MeterType_Models,
                StyleId = "pickerModelos"
            };

            pck_MeterType_Models.SelectedIndexChanged += MeterModelsPicker_SelectedIndexChanged;

            divDyna_MeterType_Models = new StackLayout()
            {
                StyleId = "bloque" + 2
            };

            Label meterModelsLabel = new Label()
            {
                Text = "Model",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            if ( this.global.ColorEntry )
                meterModelsLabel.TextColor = COL_MANDATORY;

            meterModelsContainerD.Children.Add(pck_MeterType_Models);
            meterModelsContainerC.Content = meterModelsContainerD;
            meterModelsContainerB.Content = meterModelsContainerC;
            divDyna_MeterType_Models.Children.Add(meterModelsLabel);
            divDyna_MeterType_Models.Children.Add(meterModelsContainerB);

            list_MeterType_Names = new List<string>();

            Frame meterNamesContainerB = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };

            Frame meterNamesContainerC = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout meterNamesContainerD = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };

            pck_MeterType_Names = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = list_MeterType_Names,
                StyleId = "pickerName"
            };

            pck_MeterType_Names.SelectedIndexChanged += MeterNamesPicker_SelectedIndexChanged;

            divDyna_MeterType_Names = new StackLayout()
            {
                StyleId = "bloque" + 3
            };

            Label meterNamesLabel = new Label()
            {
                Text = "Meter Type",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            if ( this.global.ColorEntry )
                meterNamesLabel.TextColor = COL_MANDATORY;

            meterNamesContainerD.Children.Add(pck_MeterType_Names);
            meterNamesContainerC.Content = meterNamesContainerD;
            meterNamesContainerB.Content = meterNamesContainerC;
            divDyna_MeterType_Names.Children.Add(meterNamesLabel);
            divDyna_MeterType_Names.Children.Add(meterNamesContainerB);

            div_MeterType.Children.Add(divDyna_MeterType_Vendors);
            div_MeterType.Children.Add(divDyna_MeterType_Models);
            div_MeterType.Children.Add(divDyna_MeterType_Names);

            divDyna_MeterType_Names.IsVisible = false;
            divDyna_MeterType_Models.IsVisible = false;
        }

        private void InitializePicker_MeterType_2 ()
        {
            list_MeterType_Vendors_2 = this.config.meterTypes.GetVendorsFromMeters(list_MeterTypesForMtu_2);

            Frame meterVendors2ContainerB = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };

            Frame meterVendors2ContainerC = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout meterVendors2ContainerD = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };

            pck_MeterType_Vendors_2 = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = list_MeterType_Vendors_2
            };

            pck_MeterType_Vendors_2.SelectedIndexChanged += MeterVendors2Picker_SelectedIndexChanged;

            divDyna_MeterType_Vendors_2 = new StackLayout()
            {
                StyleId = "bloque" + 1
            };

            Label meterVendors2Label = new Label()
            {
                Text = "Vendor",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };


            #region Color Entry

            if (this.global.ColorEntry)
            {
                meterVendors2Label.TextColor = COL_MANDATORY;

            }

            #endregion

            meterVendors2ContainerD.Children.Add(pck_MeterType_Vendors_2);
            meterVendors2ContainerC.Content = meterVendors2ContainerD;
            meterVendors2ContainerB.Content = meterVendors2ContainerC;
            divDyna_MeterType_Vendors_2.Children.Add(meterVendors2Label);
            divDyna_MeterType_Vendors_2.Children.Add(meterVendors2ContainerB);

            list_MeterType_Models_2 = new List<string>();

            Frame meterModels2ContainerB = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };

            Frame meterModels2ContainerC = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout meterModels2ContainerD = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };
            
            pck_MeterType_Models_2 = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = list_MeterType_Models_2,
                StyleId = "pickerModelos2"
            };

            pck_MeterType_Models_2.SelectedIndexChanged += MeterModels2Picker_SelectedIndexChanged;

            divDyna_MeterType_Models_2 = new StackLayout()
            {
                StyleId = "bloque" + 2
            };

            Label meterModels2Label = new Label()
            {
                Text = "Model",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };


            #region Color Entry

            if (this.global.ColorEntry)
            {
                meterModels2Label.TextColor = COL_MANDATORY;

            }

            #endregion


            meterModels2ContainerD.Children.Add(pck_MeterType_Models_2);
            meterModels2ContainerC.Content = meterModels2ContainerD;
            meterModels2ContainerB.Content = meterModels2ContainerC;
            divDyna_MeterType_Models_2.Children.Add(meterModels2Label);
            divDyna_MeterType_Models_2.Children.Add(meterModels2ContainerB);
            
            list_MeterType_Names_2 = new List<string>();

            Frame meterNames2ContainerB = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(0, 4, 0, 0),
                BackgroundColor = Color.FromHex("#7a868c")
            };

            Frame meterNames2ContainerC = new Frame()
            {
                CornerRadius = 6,
                HeightRequest = 30,
                Margin = new Thickness(-7, -7, -7, -7),
                BackgroundColor = Color.White
            };

            StackLayout meterNames2ContainerD = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(1, 1, 1, 1),
                BackgroundColor = Color.White
            };

            pck_MeterType_Names_2 = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = list_MeterType_Names_2,
                StyleId = "pickerName2"
            };

            pck_MeterType_Names_2.SelectedIndexChanged += MeterNames2Picker_SelectedIndexChanged;

            divDyna_MeterType_Names_2 = new StackLayout()
            {
                StyleId = "bloque" + 3
            };

            Label meterNames2Label = new Label()
            {
                Text = "Meter Type",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };


            #region Color Entry

            if (this.global.ColorEntry)
                meterNames2Label.TextColor = COL_MANDATORY;

            #endregion

            meterNames2ContainerD.Children.Add(pck_MeterType_Names_2);
            meterNames2ContainerC.Content = meterNames2ContainerD;
            meterNames2ContainerB.Content = meterNames2ContainerC;
            divDyna_MeterType_Names_2.Children.Add(meterNames2Label);
            divDyna_MeterType_Names_2.Children.Add(meterNames2ContainerB);

            this.div_MeterType_2.Children.Add ( divDyna_MeterType_Vendors_2 );
            this.div_MeterType_2.Children.Add ( divDyna_MeterType_Models_2  );
            this.div_MeterType_2.Children.Add ( divDyna_MeterType_Names_2   );

            divDyna_MeterType_Names_2.IsVisible = false;
            divDyna_MeterType_Models_2.IsVisible = false;
        }

        private void InitializePicker_ReadInterval (
            MTUBasicInfo mtuBasicInfo,
            Mtu mtu )
        {
            List<string> readIntervalList;

            if ( mtuBasicInfo.version >= this.global.LatestVersion )
            {
                readIntervalList = new List<string>()
                {
                    "24 Hours",
                    "12 Hours",
                    "8 Hours",
                    "6 Hours",
                    "4 Hours",
                    "3 Hours",
                    "2 Hours",
                    "1 Hour",
                    "30 Min",
                    "20 Min",
                    "15 Min"
                };
            }
            else
            {
                readIntervalList = new List<string>()
                {
                    "1 Hour",
                    "30 Min",
                    "20 Min",
                    "15 Min"
                };
            }
            
            // TwoWay MTU reading interval cannot be less than 15 minutes
            if ( ! mtu.TimeToSync )
            {
                readIntervalList.AddRange ( new string[]{
                    "10 Min",
                    "5 Min"
                });
            }

            pck_ReadInterval.ItemsSource = readIntervalList;
            
            // If tag NormXmitInterval is present inside Global,
            // its value is used as default selection
            string normXmitInterval = global.NormXmitInterval;
            if ( ! string.IsNullOrEmpty ( normXmitInterval ) )
            {
                // Convert "Hr/s" to "Hour/s"
                normXmitInterval = normXmitInterval.ToLower ()
                                   .Replace ( "hr", "hour" )
                                   .Replace ( "h", "H" );

                int index = readIntervalList.IndexOf ( normXmitInterval );
                pck_ReadInterval.SelectedIndex = ( ( index > -1 ) ? index : readIntervalList.IndexOf ( "1 Hour" ) );
            }
            // If tag NormXmitInterval is NOT present, use "1 Hour" as default value
            else
                pck_ReadInterval.SelectedIndex = readIntervalList.IndexOf ( "1 Hour" );
        }

        private void Initialize_TwoWay ()
        {
            List<string> twoWayList = new List<string> ()
            {
                TWOWAY_FAST,
                TWOWAY_SLOW,
            };
            
            pck_TwoWay.ItemsSource = twoWayList;
        }

        private void Initialize_OldMeterPickers ()
        {
            List<string> list = new List<string> ()
            {
                "Yes",
                "No",
                "Broken"
            };
            
            pck_OldMeterWorking.ItemsSource   = list;
            pck_OldMeterWorking_2.ItemsSource = list;

            list = new List<string> ();
            string regRecordingItems = global.RegisterRecordingItems;
            string[] values = new string[] { "Meter", "Register", "Both" };
            
            if ( ! string.IsNullOrEmpty ( regRecordingItems ) &&
                 regRecordingItems.Contains ( "1" ) )
            {
                char[] chars = regRecordingItems.ToCharArray ();
                
                int i = 0;
                foreach ( char c in chars )
                    if ( c.Equals ( '1' ) )
                        list.Add ( values[ i++ ] );
            }
            else list.AddRange ( values );

            //Now I am given ItemsSorce to the Pickers
            pck_ReplaceMeterRegister.ItemsSource   = list;
            pck_ReplaceMeterRegister_2.ItemsSource = list;
            
            int index;
            string defvalue = global.RegisterRecordingDefault;
            if ( ! string.IsNullOrEmpty ( defvalue ) &&
                 ( index = list.IndexOf (
                    defvalue.ToUpper ()[ 0 ] +
                    defvalue.ToLower ().Substring ( 1 ) ) ) > -1 )
            {
                pck_ReplaceMeterRegister  .SelectedIndex = index;
                pck_ReplaceMeterRegister_2.SelectedIndex = index;
            }
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

        #region Pickers

        private void CancelReasonPicker_SelectedIndexChanged ( object sender, EventArgs e )
        {
            var picker = (Picker)sender;
            int selectedCancelReasonIndex = picker.SelectedIndex;
            if (selectedCancelReasonIndex > -1)
            {
                string selectedCancelReason = picker.Items[selectedCancelReasonIndex];

                if ("Other".Equals(selectedCancelReason))
                {
                    cancelReasonOtherInput.IsEnabled = true;
                    cancelReasonOtherInputContainer.Opacity = 1;
                }
                else
                {
                    cancelReasonOtherInput.IsEnabled = false;
                    cancelReasonOtherInputContainer.Opacity = OPACITY_DISABLE;

                }
            }
        }

        private void SetMeterVendor ( int selectedIndex )
        {
            if ( selectedIndex > -1 )
            {
                selected_MeterType_Vendor = list_MeterType_Vendors [ selectedIndex ];
                
                list_MeterType_Models = this.config.meterTypes.GetModelsByVendorFromMeters(list_MeterTypesForMtu, selected_MeterType_Vendor);
                selected_MeterType_Name = "";
    
                pck_MeterType_Models.ItemsSource = list_MeterType_Models;
                divDyna_MeterType_Models.IsVisible = true;
                divDyna_MeterType_Names.IsVisible = false;
            }
        }

        private void MeterVendorsPicker_SelectedIndexChanged ( object sender, EventArgs e )
        {
            this.SetMeterVendor ( ((BorderlessPicker)sender).SelectedIndex );
        }

        private void MeterVendors2Picker_SelectedIndexChanged ( object sender, EventArgs e )
        {
            int selectedIndex = ((BorderlessPicker)sender).SelectedIndex;

            if ( selectedIndex > -1 )
            {
                selected_MeterType_Vendor_2 = list_MeterType_Vendors_2[ selectedIndex ];
    
                list_MeterType_Models_2 = this.config.meterTypes.GetModelsByVendorFromMeters(list_MeterTypesForMtu_2, selected_MeterType_Vendor_2);
                selected_MeterType_Name_2 = "";
    
                pck_MeterType_Models_2.ItemsSource = list_MeterType_Models_2;
                divDyna_MeterType_Models_2.IsVisible = true;
                divDyna_MeterType_Names_2.IsVisible = false;
            }
        }

        private void SetMeterModel ( int selectedIndex )
        {
            if ( selectedIndex > -1 )
            {
                pck_MeterType_Names.ItemDisplayBinding = new Binding("Display");
    
                selected_MeterType_Model = list_MeterType_Models[ selectedIndex ];
    
                List<Meter> meterlist = this.config.meterTypes.GetMetersByModelAndVendorFromMeters(list_MeterTypesForMtu, selected_MeterType_Vendor, selected_MeterType_Model);
    
                pck_MeterType_Names.ItemsSource = meterlist;
                divDyna_MeterType_Models.IsVisible = true;
                divDyna_MeterType_Names.IsVisible = true;
            }
        }

        private void MeterModelsPicker_SelectedIndexChanged ( object sender, EventArgs e )
        {
            this.SetMeterModel ( ((BorderlessPicker)sender).SelectedIndex );
        }

        private void MeterModels2Picker_SelectedIndexChanged ( object sender, EventArgs e )
        {
            int selectedIndex = ((BorderlessPicker)sender).SelectedIndex;

            if ( selectedIndex > -1 )
            {
                pck_MeterType_Names_2.ItemDisplayBinding = new Binding("Display");
    
                selected_MeterType_Model_2 = list_MeterType_Models_2[ selectedIndex ];
    
                List<Meter> meterlist2 = this.config.meterTypes.GetMetersByModelAndVendorFromMeters(list_MeterTypesForMtu_2, selected_MeterType_Vendor_2, selected_MeterType_Model_2);
    
                pck_MeterType_Names_2.ItemsSource = meterlist2;
                divDyna_MeterType_Models_2.IsVisible = true;
                divDyna_MeterType_Names_2.IsVisible = true;
            }
        }

        private void MeterNamesPicker_SelectedIndexChanged ( object sender, EventArgs e )
        {
            if ( ( ( BorderlessPicker )sender ).SelectedIndex > -1 )
            {
                Meter selectedMeter = (Meter)((BorderlessPicker)sender).SelectedItem;
    
                selected_MeterType_Name = selectedMeter.Display;
    
                Utils.Print(selected_MeterType_Name + " Selected");
    
                Device.BeginInvokeOnMainThread(() =>
                {
                    // Update MeterReading field length to use and validate
                    this.tbx_MeterReading        .MaxLength = selectedMeter.LiveDigits;
                    this.tbx_MeterReading_Dual   .MaxLength = selectedMeter.LiveDigits;
                    //this.tbx_OldMeterReading     .MaxLength = selectedMeter.LiveDigits;
                    //this.tbx_OldMeterReading_Dual.MaxLength = selectedMeter.LiveDigits;
                    
                    this.div_MeterReading           .Opacity = OPACITY_ENABLE;
                    this.divSub_MeterReading_Dual   .Opacity = OPACITY_ENABLE;
                    //this.divSub_OldMeterReading     .Opacity = OPACITY_ENABLE;
                    //this.divSub_OldMeterReading_Dual.Opacity = OPACITY_ENABLE;
                    
                    this.tbx_MeterReading        .IsEnabled = true;
                    this.tbx_MeterReading_Dual   .IsEnabled = true;
                    this.btnScanMeterReading     .IsEnabled = true;
                    this.btnScanMeterReadingDual .IsEnabled = true;
                    //this.tbx_OldMeterReading     .IsEnabled = true;
                    //this.tbx_OldMeterReading_Dual.IsEnabled = true;
                    
                    this.lb_MeterReading_MeterType       .IsVisible = false;
                    this.lb_MeterReading_DualMeterType   .IsVisible = false;
                    //this.lb_OldMeterReading_MeterType    .IsVisible = false;
                    //this.lb_OldMeterReading_DualMeterType.IsVisible = false;
                });
            }
        }

        private void MeterNames2Picker_SelectedIndexChanged ( object sender, EventArgs e )
        {
            if ( ( ( BorderlessPicker )sender ).SelectedIndex > -1 )
            {
                Meter selectedMeter = (Meter)((BorderlessPicker)sender).SelectedItem;
    
                selected_MeterType_Name_2 = selectedMeter.Display;
                
                Utils.Print(selected_MeterType_Name_2 + " Selected");
    
                Device.BeginInvokeOnMainThread(() =>
                {
                    // Update MeterReading field length to use and validate
                    this.tbx_MeterReading_2        .MaxLength = selectedMeter.LiveDigits;
                    this.tbx_MeterReading_Dual_2   .MaxLength = selectedMeter.LiveDigits;
                    //this.tbx_OldMeterReading_2     .MaxLength = selectedMeter.LiveDigits;
                    //this.tbx_OldMeterReading_Dual_2.MaxLength = selectedMeter.LiveDigits;
                
                    this.divSub_MeterReading_2        .Opacity = OPACITY_ENABLE;
                    this.divSub_MeterReading_Dual_2   .Opacity = OPACITY_ENABLE;
                    //this.divSub_OldMeterReading_2     .Opacity = OPACITY_ENABLE;
                    //this.divSub_OldMeterReading_Dual_2.Opacity = OPACITY_ENABLE;
                
                    this.tbx_MeterReading_2          .IsEnabled = true;
                    this.tbx_MeterReading_Dual_2     .IsEnabled = true;
                    this.btnScannerMeterReading_2    .IsEnabled = true;
                    this.btnScannerMeterReadingDual_2.IsEnabled = true;
                    //this.tbx_OldMeterReading_2     .IsEnabled = true;
                    //this.tbx_OldMeterReading_Dual_2.IsEnabled = true;
                    
                    this.lb_MeterReading_MeterType_2       .IsVisible = false;
                    this.lb_MeterReading_DualMeterType_2   .IsVisible = false;
                    //this.lb_OldMeterReading_MeterType_2    .IsVisible = false;
                    //this.lb_OldMeterReading_DualMeterType_2.IsVisible = false;
                });
            }
        }

        #endregion

        #region Sliders

        void OnSnapReadsSlider_ValueChanged ( object sender, ValueChangedEventArgs e )
        {
            var newStep = Math.Round(e.NewValue / snapReadsStep);

            sld_SnapReads.Value   = newStep * snapReadsStep;
            lb_SnapReads_Num.Text = sld_SnapReads.Value.ToString ();
        }

        #endregion

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
                    menuOptions.GetListElement("navigationDrawerList").SelectedItem = null;
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

        private async Task NavigationController (
            ActionType actionTarget )
        {
            if (!isCancellable)
            {
                //REASON
                dialog_open_bg.IsVisible = true;

                Popup_start.IsVisible = true;
                Popup_start.IsEnabled = true;
            }
            else
            {
                
                await SwitchToControler(actionTarget);

            }
        }

        private async Task SwitchToControler(ActionType page)
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

            backdark_bg.IsVisible = true;
            indicator.IsVisible = true;
            background_scan_page.IsEnabled = false;

            if (!await base.ValidateNavigation(page))
            {
                Console.WriteLine("NOOOOO PUEDESSSSS PASARRRRRR!!!");
                dialog_open_bg.IsVisible = true;
                turnoff_mtu_background.IsVisible = true;
                dialogView.CloseDialogs();
                dialogView.OpenCloseDialog("dialog_NoAction", true);
                return;
            }

            this.actionTypeNew = page; 

            switch ( page )
            {
                 case ActionType.DataRead:
                 case ActionType.RemoteDisconnect:
                    #region DataRead  
                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
   
                            this.actionType = this.actionTypeNew;
                            this.GoToPage ();
                        })
                    );

                    #endregion

                    break;

                case ActionType.ReadMtu:
                    #region ReadMTU  
                    await Task.Delay(200).ContinueWith(t =>

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved, page), false);

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
                    await ControllerAction(page, "dialog_AddMTU");

                   
                    #endregion
                    break;

                case ActionType.ReplaceMTU:

                    #region Replace Mtu Controller
                    await ControllerAction(page, "dialog_replacemeter_one");
                    

                    #endregion

                    break;

                case ActionType.ReplaceMeter:

                    #region Replace Meter Controller
                    await ControllerAction(page, "dialog_meter_replace_one");
 
                    #endregion

                    break;

                case ActionType.AddMtuAddMeter:

                    #region Add Mtu | Add Meter Controller
                    await ControllerAction(page, "dialog_AddMTUAddMeter");
                    
                    #endregion

                    break;

                case ActionType.AddMtuReplaceMeter:

                    #region Add Mtu | Replace Meter Controller
                    await ControllerAction(page, "dialog_AddMTUReplaceMeter");

                    #endregion

                    break;

                case ActionType.ReplaceMtuReplaceMeter:

                    #region Replace Mtu | Replace Meter Controller

                    await ControllerAction(page, "dialog_ReplaceMTUReplaceMeter");


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

        private async void LogoutTapped(object sender, EventArgs e)
        {
            #region Check if no action done

     
            Device.BeginInvokeOnMainThread(() =>
            {
                dialogView.CloseDialogs();
                dialogView.OpenCloseDialog("dialog_logoff", true);

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

            await turnOffAction.Run ();
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
            this.add_mtu.Cancel ( selectedCancelReason );

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
            port2label.Opacity = 0.5;

            port1label.FontSize = 19;
            misclabel.FontSize = 22;
            port2label.FontSize = 19;

            port1view.IsVisible = false;
            port2view.IsVisible = false;
            miscview.IsVisible = true;

            miscview.FadeTo(1, 200);

        }

        private void port2_command()
        {
            port2view.Opacity = 0;

            port1label.Opacity = 0.5;
            misclabel.Opacity = 0.5;
            port2label.Opacity = 1;

            port1label.FontSize = 19;
            misclabel.FontSize = 19;
            port2label.FontSize = 22;

            port1view.IsVisible = false;
            port2view.IsVisible = true;
            miscview.IsVisible = false;

            port2view.FadeTo(1, 200);
        }

        private void port1_command()
        {
            port1view.Opacity = 0;

            port1label.Opacity = 1;
            misclabel.Opacity = 0.5;
            port2label.Opacity = 0.5;

            port1label.FontSize = 22;
            misclabel.FontSize = 19;
            port2label.FontSize = 19;

           

            port1view.IsVisible = true;
            port2view.IsVisible = false;
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
            if ( DEBUG_AUTO_MODE_ON )
                return true;

            dynamic EmptyNoReq = new Func<string,bool,bool> ( ( value, isMandatory ) =>
                                    string.IsNullOrEmpty ( value ) && ! isMandatory );
                                
            dynamic NoSelNoReq = new Func<int,bool,bool> ( ( index, isMandatory ) =>
                                    index <= -1 && ! isMandatory );

            // Value equals to maximum length
            dynamic NoEqNum = new Func<string,int,bool> ( ( value, maxLength ) =>
                                ! Validations.NumericText ( value, maxLength ) );
                                
            dynamic NoEqTxt = new Func<string,int,bool> ( ( value, maxLength ) =>
                                ! Validations.Text ( value, maxLength ) );

            // Value equals or lower to maximum length
            dynamic NoELNum = new Func<string,int,bool> ( ( value, maxLength ) =>
                                ! Validations.NumericText ( value, maxLength, 1, true, true, false ) );
                                
            dynamic NoELTxt = new Func<string,int,bool> ( ( value, maxLength ) =>
                                ! Validations.Text ( value, maxLength, 1, true, true, false ) );

            #region Port 1

            // No mandatory fields can be empty
            // TRUE when the field does not need to be validated ( not mandatory and empty/not selected )
            bool noAcn = EmptyNoReq ( this.tbx_AccountNumber       .Text, MANDATORY_ACCOUNTNUMBER   );
            bool noWor = EmptyNoReq ( this.tbx_WorkOrder           .Text, MANDATORY_WORKORDER       );
            bool noOMt = EmptyNoReq ( this.tbx_OldMtuId            .Text, MANDATORY_OLDMTUID        );
            bool noOMs = EmptyNoReq ( this.tbx_OldMeterSerialNumber.Text, MANDATORY_OLDMETERSERIAL  );
            bool noMsn = EmptyNoReq ( this.tbx_MeterSerialNumber   .Text, MANDATORY_METERSERIAL     );
            bool noSnr = EmptyNoReq ( this.lb_SnapReads_Num        .Text, MANDATORY_SNAPREADS       );
            bool noOMr = EmptyNoReq ( this.tbx_OldMeterReading     .Text, MANDATORY_OLDMETERREADING );
            bool noMre = EmptyNoReq ( this.tbx_MeterReading        .Text, MANDATORY_METERREADING    );
            
            bool noOMw = NoSelNoReq ( this.pck_OldMeterWorking     .SelectedIndex, global.MeterWorkRecording   );
            bool noRpc = NoSelNoReq ( this.pck_ReplaceMeterRegister.SelectedIndex, global.RegisterRecordingReq );
            bool noMty = NoSelNoReq ( this.pck_MeterType_Names     .SelectedIndex, MANDATORY_METERTYPE       );
            bool noRin = NoSelNoReq ( this.pck_ReadInterval        .SelectedIndex, MANDATORY_READINTERVAL    );
            bool noTwo = NoSelNoReq ( this.pck_TwoWay              .SelectedIndex, MANDATORY_TWOWAY          );
            bool noAlr = NoSelNoReq ( this.pck_Alarms              .SelectedIndex, MANDATORY_ALARMS          );
            bool noDmd = NoSelNoReq ( this.pck_Demands             .SelectedIndex, MANDATORY_DEMANDS         );

            bool noDAc = EmptyNoReq ( this.tbx_AccountNumber_Dual       .Text, MANDATORY_ACCOUNTNUMBER   );
            bool noDWr = EmptyNoReq ( this.tbx_WorkOrder_Dual           .Text, MANDATORY_WORKORDER       );
            bool noDOs = EmptyNoReq ( this.tbx_OldMeterSerialNumber_Dual.Text, MANDATORY_OLDMETERSERIAL  );
            bool noDMs = EmptyNoReq ( this.tbx_MeterSerialNumber_Dual   .Text, MANDATORY_METERSERIAL     );
            bool noDOr = EmptyNoReq ( this.tbx_OldMeterReading_Dual     .Text, MANDATORY_OLDMETERREADING );
            bool noDMr = EmptyNoReq ( this.tbx_MeterReading_Dual        .Text, MANDATORY_METERREADING    );

            // Numeric fields
            // - AccountNumber
            // - SnapReads/DailyReads
            // - OldMeterReading
            // - MeterReading
            
            // Alphanumeric fields
            // - WorkOrder
            // - OldMeterSerialNumber
            // - MeterSerialNumber

            // Correct length
            // TRUE when the field has not correct length or is not selected yet
            bool badAcn =                                             NoEqNum ( this.tbx_AccountNumber       .Text, global.AccountLength               );
            bool badWor = this.div_WorkOrder            .IsVisible && NoELTxt ( this.tbx_WorkOrder           .Text, global.WorkOrderLength             );
            bool badOMt = this.div_OldMtuId             .IsVisible && NoEqTxt ( this.tbx_OldMtuId            .Text, global.MtuIdLength                 );
            bool badOMs = this.div_OldMeterSerialNumber .IsVisible && NoELTxt ( this.tbx_OldMeterSerialNumber.Text, global.MeterNumberLength           );
            bool badMsn = this.div_MeterSerialNumber    .IsVisible && NoELTxt ( this.tbx_MeterSerialNumber   .Text, global.MeterNumberLength           );
            bool badSnr = this.div_SnapReads            .IsVisible && NoELNum ( this.lb_SnapReads_Num        .Text, (int)this.sld_SnapReads .Maximum   ) && snapReadsStatus;
            bool badOMr = this.div_OldMeterReading      .IsVisible && NoELNum ( this.tbx_OldMeterReading     .Text, this.tbx_OldMeterReading.MaxLength );
            bool badMre = this.div_MeterReadings        .IsVisible && NoEqNum ( this.tbx_MeterReading        .Text, this.tbx_MeterReading   .MaxLength );
            
            bool badOMw = this.div_OldMeterWorking      .IsVisible && this.pck_OldMeterWorking     .SelectedIndex <= -1;
            bool badRpc = this.div_ReplaceMeterRegister .IsVisible && this.pck_ReplaceMeterRegister.SelectedIndex <= -1;
            bool badMty = this.divDyna_MeterType_Vendors.IsVisible && this.pck_MeterType_Names     .SelectedIndex <= -1;
            bool badRin = this.div_ReadInterval         .IsVisible && this.pck_ReadInterval        .SelectedIndex <= -1;
            bool badTwo = this.div_TwoWay               .IsVisible && this.pck_TwoWay              .SelectedIndex <= -1;
            bool badAlr = this.div_Alarms               .IsVisible && this.pck_Alarms              .SelectedIndex <= -1;
            bool badDmd = this.div_Demands              .IsVisible && this.pck_Demands             .SelectedIndex <= -1;

            bool badDAc = global.AccountDualEntry      && this.div_AccountNumber_Dual       .IsVisible && NoEqNum ( this.tbx_AccountNumber_Dual       .Text, global.AccountLength     );
            bool badDWr = global.WorkOrderDualEntry    && this.div_WorkOrder_Dual           .IsVisible && NoELTxt ( this.tbx_WorkOrder_Dual           .Text, global.WorkOrderLength   );
            bool badDOs = global.OldSerialNumDualEntry && this.div_OldMeterSerialNumber_Dual.IsVisible && NoELTxt ( this.tbx_OldMeterSerialNumber_Dual.Text, global.MeterNumberLength );
            bool badDMs = global.NewSerialNumDualEntry && this.div_MeterSerialNumber_Dual   .IsVisible && NoELTxt ( this.tbx_MeterSerialNumber_Dual   .Text, global.MeterNumberLength );
            bool badDOr = global.OldReadingDualEntry   && this.div_OldMeterReading_Dual     .IsVisible && NoELNum ( this.tbx_OldMeterReading_Dual     .Text, this.tbx_OldMeterReading_Dual.MaxLength );
            bool badDMr = global.ReadingDualEntry      && this.div_MeterReading_Dual        .IsVisible && NoEqNum ( this.tbx_MeterReading_Dual        .Text, this.tbx_MeterReading_Dual   .MaxLength );
            
            string FILL_ERROR = "Field '_' is incorrectly filled";
            string DUAL_ERROR = " ( Second entry )";
            
            if      ( ( badAcn &= ! noAcn ) ) msgError = this.lb_AccountNumber.Text;
            else if ( ( badDAc &= ! noDAc ) ) msgError = this.lb_AccountNumber.Text + DUAL_ERROR;
            else if ( ( badWor &= ! noWor ) ) msgError = this.lb_WorkOrder.Text;
            else if ( ( badDWr &= ! noDWr ) ) msgError = this.lb_WorkOrder.Text + DUAL_ERROR;
            else if ( ( badOMt &= ! noOMt ) ) msgError = this.lb_OldMtuId.Text;
            else if ( ( badOMs &= ! noOMs ) ) msgError = this.lb_OldMeterSerialNumber.Text;
            else if ( ( badDOs &= ! noDOs ) ) msgError = this.lb_OldMeterSerialNumber.Text + DUAL_ERROR;
            else if ( ( badOMw &= ! noOMw ) ) msgError = this.lb_OldMeterWorking.Text;
            else if ( ( badOMr &= ! noOMr ) ) msgError = this.lb_OldMeterReading.Text;
            else if ( ( badDOr &= ! noDOr ) ) msgError = this.lb_OldMeterReading.Text + DUAL_ERROR;
            else if ( ( badRpc &= ! noRpc ) ) msgError = this.lb_ReplaceMeterRegister.Text;
            else if ( ( badMsn &= ! noMsn ) ) msgError = this.lb_MeterSerialNumber.Text;
            else if ( ( badDMs &= ! noDMs ) ) msgError = this.lb_MeterSerialNumber.Text + DUAL_ERROR;
            else if ( ( badMty &= ! noMty ) ) msgError = "Meter Type";
            else if ( ( badMre &= ! noMre ) ) msgError = this.lb_MeterReading.Text;
            else if ( ( badDMr &= ! noDMr ) ) msgError = this.lb_MeterReading.Text + DUAL_ERROR;
            else if ( ( badRin &= ! noRin ) ) msgError = this.lb_ReadInterval.Text;
            else if ( ( badSnr &= ! noSnr ) ) msgError = this.lb_SnapReads.Text;
            else if ( ( badTwo &= ! noTwo ) ) msgError = this.lb_TwoWay.Text;
            else if ( ( badAlr &= ! noAlr ) ) msgError = this.lb_Alarms.Text;
            else if ( ( badDmd &= ! noDmd ) ) msgError = this.lb_Demands.Text;

            if ( badAcn || badWor || badOMs || badOMw || badOMr ||
                 badRpc || badMsn || badMre || badSnr || badRin ||
                 badMty || badTwo || badAlr || badDmd || badDAc ||
                 badDWr || badDOs || badDOr || badDMs || badDMr )
            {
                msgError = FILL_ERROR.Replace ( "_", msgError );
                return false;
            }

            // Dual entries
            DUAL_ERROR = "Field '_' dual entries are not the same";

            if (global.AccountDualEntry &&
                 ! string.Equals ( tbx_AccountNumber.Text, tbx_AccountNumber_Dual.Text ) )
            {
                msgError = DUAL_ERROR.Replace ( "_", this.lb_AccountNumber.Text );
                return false;
            }
            
            if (global.WorkOrderDualEntry &&
                 ! string.Equals ( tbx_WorkOrder.Text, tbx_WorkOrder_Dual.Text ) )
            {
                msgError = DUAL_ERROR.Replace ( "_", this.lb_WorkOrder.Text );
                return false;
            }
            
            if (global.OldSerialNumDualEntry &&
                 ! string.Equals ( tbx_OldMeterSerialNumber.Text, tbx_OldMeterSerialNumber_Dual.Text ) )
            {
                msgError = DUAL_ERROR.Replace ( "_", this.lb_OldMeterSerialNumber.Text );
                return false;
            }
            
            if (global.OldReadingDualEntry &&
                 ! string.Equals ( tbx_OldMeterReading.Text, tbx_OldMeterReading_Dual.Text ) )
            {
                msgError = DUAL_ERROR.Replace ( "_", this.lb_OldMeterReading.Text );
                return false;
            }
            
            if (global.NewSerialNumDualEntry &&
                 ! string.Equals ( tbx_MeterSerialNumber.Text, tbx_MeterSerialNumber_Dual.Text ) )
            {
                msgError = DUAL_ERROR.Replace ( "_", this.lb_MeterSerialNumber.Text );
                return false;
            }

            if (global.ReadingDualEntry &&
                 ! string.Equals ( tbx_MeterReading.Text, tbx_MeterReading_Dual.Text ) )
            {
                msgError = DUAL_ERROR.Replace ( "_", this.lb_MeterReading.Text );
                return false;
            }

            #endregion

            #region Port 2

            if ( this.hasTwoPorts &&
                 this.port2IsActivated )
            {
                // No mandatory fields can be empty
                // TRUE when the field does not need to be validated ( not mandatory and empty/not selected )
                noAcn = EmptyNoReq ( this.tbx_AccountNumber_2       .Text, MANDATORY_ACCOUNTNUMBER   );
                noWor = EmptyNoReq ( this.tbx_WorkOrder_2           .Text, MANDATORY_WORKORDER       );
                noOMs = EmptyNoReq ( this.tbx_OldMeterSerialNumber_2.Text, MANDATORY_OLDMETERSERIAL  );
                noMsn = EmptyNoReq ( this.tbx_MeterSerialNumber_2   .Text, MANDATORY_METERSERIAL     );
                noOMr = EmptyNoReq ( this.tbx_OldMeterReading_2     .Text, MANDATORY_OLDMETERREADING );
                noMre = EmptyNoReq ( this.tbx_MeterReading_2        .Text, MANDATORY_METERREADING    );
                
                noOMw = NoSelNoReq ( this.pck_OldMeterWorking_2     .SelectedIndex, global.MeterWorkRecording   );
                noRpc = NoSelNoReq ( this.pck_ReplaceMeterRegister_2.SelectedIndex, global.RegisterRecordingReq );
                noMty = NoSelNoReq ( this.pck_MeterType_Names_2     .SelectedIndex, MANDATORY_METERTYPE       );
    
                noDAc = EmptyNoReq ( this.tbx_AccountNumber_Dual_2       .Text, MANDATORY_ACCOUNTNUMBER   );
                noDWr = EmptyNoReq ( this.tbx_WorkOrder_Dual_2           .Text, MANDATORY_WORKORDER       );
                noDOs = EmptyNoReq ( this.tbx_OldMeterSerialNumber_Dual_2.Text, MANDATORY_OLDMETERSERIAL  );
                noDMs = EmptyNoReq ( this.tbx_MeterSerialNumber_Dual_2   .Text, MANDATORY_METERSERIAL     );
                noDOr = EmptyNoReq ( this.tbx_OldMeterReading_Dual_2     .Text, MANDATORY_OLDMETERREADING );
                noDMr = EmptyNoReq ( this.tbx_MeterReading_Dual_2        .Text, MANDATORY_METERREADING    );
    
                // Correct length
                // TRUE when the field has not correct length or is not selected yet
                badAcn =                                               NoEqNum ( this.tbx_AccountNumber_2       .Text, global.AccountLength                 );
                badWor = this.div_WorkOrder_2            .IsVisible && NoELTxt ( this.tbx_WorkOrder_2           .Text, global.WorkOrderLength               );
                badOMs = this.div_OldMeterSerialNumber_2 .IsVisible && NoELTxt ( this.tbx_OldMeterSerialNumber_2.Text, global.MeterNumberLength             );
                badMsn = this.div_MeterSerialNumber_2    .IsVisible && NoELTxt ( this.tbx_MeterSerialNumber_2   .Text, global.MeterNumberLength             );
                badOMr = this.div_OldMeterReading        .IsVisible && NoELNum ( this.tbx_OldMeterReading       .Text, this.tbx_OldMeterReading_2.MaxLength );
                badMre = this.div_MeterReadings_2        .IsVisible && NoEqNum ( this.tbx_MeterReading_2        .Text, this.tbx_MeterReading_2   .MaxLength );
                
                badOMw = this.div_OldMeterWorking_2      .IsVisible && this.pck_OldMeterWorking_2     .SelectedIndex <= -1;
                badRpc = this.div_ReplaceMeterRegister_2 .IsVisible && this.pck_ReplaceMeterRegister_2.SelectedIndex <= -1;
                badMty = this.divDyna_MeterType_Vendors_2.IsVisible && this.pck_MeterType_Names_2     .SelectedIndex <= -1;
                
                badDAc = global.AccountDualEntry      && this.div_AccountNumber_Dual_2       .IsVisible && NoEqNum ( this.tbx_AccountNumber_Dual_2       .Text, global.AccountLength     );
                badDWr = global.WorkOrderDualEntry    && this.div_WorkOrder_Dual_2           .IsVisible && NoELTxt ( this.tbx_WorkOrder_Dual_2           .Text, global.WorkOrderLength   );
                badDOs = global.OldSerialNumDualEntry && this.div_OldMeterSerialNumber_Dual_2.IsVisible && NoELTxt ( this.tbx_OldMeterSerialNumber_Dual_2.Text, global.MeterNumberLength );
                badDMs = global.NewSerialNumDualEntry && this.div_MeterSerialNumber_Dual_2   .IsVisible && NoELTxt ( this.tbx_MeterSerialNumber_Dual_2   .Text, global.MeterNumberLength );
                badDOr = global.OldReadingDualEntry   && this.div_OldMeterReading_Dual_2     .IsVisible && NoELNum ( this.tbx_OldMeterReading_Dual_2     .Text, this.tbx_OldMeterReading_Dual_2.MaxLength );
                badDMr = global.ReadingDualEntry      && this.div_MeterReading_Dual_2        .IsVisible && NoEqNum ( this.tbx_MeterReading_Dual_2        .Text, this.tbx_MeterReading_Dual_2   .MaxLength );
                
                FILL_ERROR = "Field 'Port2 _' is incorrectly filled";
                DUAL_ERROR = " ( Second entry )";
                
                if      ( ( badAcn &= ! noAcn ) ) msgError = this.lb_AccountNumber_2.Text;
                else if ( ( badDAc &= ! noDAc ) ) msgError = this.lb_AccountNumber_2.Text + DUAL_ERROR;
                else if ( ( badWor &= ! noWor ) ) msgError = this.lb_WorkOrder_2.Text;
                else if ( ( badDWr &= ! noDWr ) ) msgError = this.lb_WorkOrder_2.Text + DUAL_ERROR;
                else if ( ( badOMs &= ! noOMs ) ) msgError = this.lb_OldMeterSerialNumber_2.Text;
                else if ( ( badDOs &= ! noDOs ) ) msgError = this.lb_OldMeterSerialNumber_2.Text + DUAL_ERROR;
                else if ( ( badOMw &= ! noOMw ) ) msgError = this.lb_OldMeterWorking_2.Text;
                else if ( ( badOMr &= ! noOMr ) ) msgError = this.lb_OldMeterReading_2.Text;
                else if ( ( badDOr &= ! noDOr ) ) msgError = this.lb_OldMeterReading_2.Text + DUAL_ERROR;
                else if ( ( badRpc &= ! noRpc ) ) msgError = this.lb_ReplaceMeterRegister_2.Text;
                else if ( ( badMsn &= ! noMsn ) ) msgError = this.lb_MeterSerialNumber_2.Text;
                else if ( ( badDMs &= ! noDMs ) ) msgError = this.lb_MeterSerialNumber_2.Text + DUAL_ERROR;
                else if ( ( badMty &= ! noMty ) ) msgError = "Meter Type";
                else if ( ( badMre &= ! noMre ) ) msgError = this.lb_MeterReading_2.Text;
                else if ( ( badDMr &= ! noDMr ) ) msgError = this.lb_MeterReading_2.Text + DUAL_ERROR;
                
                if ( badAcn || badWor || badOMs || badOMw || badOMr ||
                     badRpc || badMsn || badMre || badMty || badDAc ||
                     badDWr || badDOs || badDOr || badDMs || badDMr )
                {
                    msgError = FILL_ERROR.Replace ( "_", msgError );
                    return false;
                }
                
                // Dual entries
                DUAL_ERROR = "Field 'Port2 _' dual entries are not the same";

                if (global.AccountDualEntry &&
                     ! string.Equals ( tbx_AccountNumber_2.Text, tbx_AccountNumber_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR.Replace ( "_", this.lb_AccountNumber_2.Text );
                    return false;
                }
                
                if (global.WorkOrderDualEntry &&
                     ! string.Equals ( tbx_WorkOrder_2.Text, tbx_WorkOrder_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR.Replace ( "_", this.lb_WorkOrder_2.Text );
                    return false;
                }
                
                if (global.OldSerialNumDualEntry &&
                     ! string.Equals ( tbx_OldMeterSerialNumber_2.Text, tbx_OldMeterSerialNumber_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR.Replace ( "_", this.lb_OldMeterSerialNumber_2.Text );
                    return false;
                }
                
                if (global.OldReadingDualEntry &&
                     ! string.Equals ( tbx_OldMeterReading_2.Text, tbx_OldMeterReading_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR.Replace ( "_", this.lb_OldMeterReading_2.Text );
                    return false;
                }
                
                if (global.NewSerialNumDualEntry &&
                     ! string.Equals ( tbx_MeterSerialNumber_2.Text, tbx_MeterSerialNumber_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR.Replace ( "_", this.lb_MeterSerialNumber_2.Text );
                    return false;
                }
    
                if (global.ReadingDualEntry &&
                     ! string.Equals ( tbx_MeterReading_2.Text, tbx_MeterReading_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR.Replace ( "_", this.lb_MeterReading_2.Text );
                    return false;
                }
            }
            
            #endregion

            dynamic NoValNOrEmpty = new Func<string,bool> ( ( value ) =>
                                        ! string.IsNullOrEmpty ( value ) &&
                                        ! Validations.IsNumeric ( value ) );

            #region Miscelanea

            if ( NoValNOrEmpty ( this.tbx_MtuGeolocationLat .Text ) ||
                 NoValNOrEmpty ( this.tbx_MtuGeolocationLong.Text ) )
            {
                msgError = "Field 'GPS Coordinates' are incorrectly filled";
                return false;
            }

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

        #region GUI Logic

        private void OnClick_BtnSwitchPort2 ( object sender, EventArgs e )
        {
            if ( ! this.waitOnClickLogic )
            {
                Utils.Print ( "CLICK!" );
            
                this.waitOnClickLogic = true;
                
                Task.Factory.StartNew ( NewPort2ClickTask );
            }
        }

        private async Task NewPort2ClickTask ()
        {
            // Button for enable|disable the second port
            if ( ! global.Port2DisableNo )
            {
                bool ok = await this.add_mtu.MTUComm.WriteMtuBitAndVerify ( 28, 1, ( this.port2IsActivated = !this.port2IsActivated ) );

                // Bit have not changed -> return to previous state
                if ( ok )
                    this.UpdatePortButtons ();
                else
                    this.waitOnClickLogic = false;
            }
        }
        
        private void UpdatePortButtons ()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if ( ! global.Port2DisableNo )
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        block_view_port2.IsVisible = this.port2IsActivated;
                        btn_EnablePort2.Text = (this.port2IsActivated) ? "Disable Port 2" : "Enable Port 2";
                        //btn_EnablePort2.TextColor = (this.port2IsActivated) ? Color.Gold : Color.White;
                    });
                }
            
                // Button for copy port 1 common fields values to port 2
                this.div_CopyPort1To2.IsVisible = this.port2IsActivated;
                this.div_CopyPort1To2.IsEnabled = this.port2IsActivated;
                
                // Allow click again the button
                this.waitOnClickLogic = false;
            });
        }

        #endregion

        #region Action

        private void AddMtu ( object sender, EventArgs e )
        {
            string msgError = string.Empty;
            if ( ! DEBUG_AUTO_MODE_ON &&
                 ! this.ValidateFields ( ref msgError ) )
            {
                DisplayAlert ( "Error", msgError, "OK" );
                return;
            }

            isCancellable = true;

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

                    Task.Factory.StartNew(AddMtu_Action);
                });
            }
        }

        private async Task AddMtu_Action ()
        { 
            #region Get values from form

            Mtu mtu = this.addMtuForm.mtu;

            bool isReplaceMeter = ( this.actionType == ActionType.ReplaceMeter           ||
                                    this.actionType == ActionType.ReplaceMtuReplaceMeter ||
                                    this.actionType == ActionType.AddMtuReplaceMeter );

            // For port 1 and 2
            string value_acn   = string.Empty; // Account Number / Service Port ID
            string value_acn_2 = string.Empty;
            string value_wor   = string.Empty; // Work Order / Field Order
            string value_wor_2 = string.Empty;
            string value_oms   = string.Empty; // Old Meter Serial Number
            string value_oms_2 = string.Empty;
            string value_omr   = string.Empty; // Old Meter Reading / Initial Reading
            string value_omr_2 = string.Empty;
            string value_msn   = string.Empty; // ( New ) Meter Serial Number
            string value_msn_2 = string.Empty;
            string value_mre   = string.Empty; // ( New ) Meter Reading / Initial Reading
            string value_mre_2 = string.Empty;
            string value_omw   = string.Empty; // Old Meter Working
            string value_omw_2 = string.Empty;
            string value_rpl   = string.Empty; // Replace Meter|Register
            string value_rpl_2 = string.Empty;
            Meter  value_mty   = null;         // Meter Type
            Meter  value_mty_2 = null;
            
            // Only for port 1 ( for MTU itself )
            string value_omt;                // Old MTU
            string value_rin;                // Read Interval
            string value_sre;                // Snap Reads / Daily Reads
            string value_two = string.Empty; // Two-Way
            Alarm  value_alr = null;         // Alarms
            Demand value_dmd = null;         // Demands
            
            // GPS
            string value_lat;              // Latitude
            string value_lon;              // Longitude
            string value_alt;              // Altitude

            // Debug values
            if ( DEBUG_AUTO_MODE_ON )
            {
                // Port 1
                value_acn = DEBUG_ACCOUNTNUMBER;
                value_wor = DEBUG_WORKORDER;
                value_oms = DEBUG_OLDMETERNUM;
                value_omr = DEBUG_OLDMETERREAD;
                value_msn = DEBUG_METERNUM;
                value_mre = DEBUG_INITREADING;
                value_mty = (Meter)this.pck_MeterType_Names.ItemsSource[ DEBUG_MTRNAME_INDEX ];
                
                if ( isReplaceMeter )
                {
                    if ( global.MeterWorkRecording )
                        value_rpl = this.pck_ReplaceMeterRegister.ItemsSource[ DEBUG_REPLACEREGIS ].ToString ();
                    
                    if ( global.RegisterRecording )
                        value_omw = this.pck_OldMeterWorking.ItemsSource[ DEBUG_OLDMETERWORK ].ToString ();
                }
                
                if ( mtu.TwoPorts )
                {
                    // Port 2
                    value_acn_2 = value_acn;
                    value_wor_2 = value_wor;
                    value_oms_2 = value_oms;
                    value_omw_2 = value_omw;
                    value_omr_2 = value_omr;
                    value_rpl_2 = value_rpl;
                    value_msn_2 = value_msn;
                    value_mre_2 = value_mre;
                    value_mty_2 = value_mty;
                }
                
                // Only for port 1 ( for MTU itself )
                value_omt = "";
                value_rin = DEBUG_READSINTERVAL;
                value_sre = DEBUG_SNAPSREADS;
                value_two = (string)pck_TwoWay.ItemsSource[ 0 ];
                value_alr = (Alarm)this.pck_Alarms.ItemsSource[ DEBUG_ALARM_INDEX   ];
                //value_dmd = (Demand)pck_Demands.ItemsSource[ DEBUG_DEMAND_INDEX  ];
                
                // GPS
                value_lat = DEBUG_GPS_LAT;
                value_lon = DEBUG_GPS_LON;
                value_alt = DEBUG_GPS_ALT;
            }
            // Real values
            else
            {
                // Port 1
                value_acn = this.tbx_AccountNumber           .Text;
                value_wor = this.tbx_WorkOrder               .Text;
                value_oms = this.tbx_OldMeterSerialNumber    .Text;
                value_omr = this.tbx_OldMeterReading         .Text;
                value_msn = this.tbx_MeterSerialNumber       .Text;
                value_mre = this.tbx_MeterReading            .Text;
                value_mty = ( Meter )this.pck_MeterType_Names.SelectedItem;
                
                if ( isReplaceMeter )
                {
                    if ( global.MeterWorkRecording )
                        value_omw = this.pck_OldMeterWorking.SelectedItem.ToString ();
                        
                    if ( global.RegisterRecording )
                        value_rpl = this.pck_ReplaceMeterRegister.SelectedItem.ToString ();
                }
                
                if ( ( addMtuForm.usePort2 = mtu.TwoPorts && this.port2IsActivated ) )
                {
                    // Port 2
                    value_acn_2 = this.tbx_AccountNumber_2           .Text;
                    value_wor_2 = this.tbx_WorkOrder_2               .Text;
                    value_oms_2 = this.tbx_OldMeterSerialNumber_2    .Text;
                    value_omr_2 = this.tbx_OldMeterReading_2         .Text;
                    value_msn_2 = this.tbx_MeterSerialNumber_2       .Text;
                    value_mre_2 = this.tbx_MeterReading_2            .Text;
                    value_mty_2 = ( Meter )this.pck_MeterType_Names_2.SelectedItem;
                    
                    if ( isReplaceMeter )
                    {
                        if ( global.MeterWorkRecording )
                            value_omw = this.pck_OldMeterWorking_2.SelectedItem.ToString ();
                        
                        if ( global.RegisterRecording )
                            value_rpl = this.pck_ReplaceMeterRegister_2.SelectedItem.ToString ();
                    }
                }
                
                // Only for port 1 ( for MTU itself )
                value_omt = this.tbx_OldMtuId    .Text;
                value_rin = this.pck_ReadInterval.SelectedItem.ToString();
                value_sre = this.sld_SnapReads   .Value.ToString();
                
                // Is a two-way MTU
                if ( global.TimeToSync &&
                     mtu.TimeToSync    &&
                     mtu.FastMessageConfig )
                    value_two = this.pck_TwoWay.SelectedItem.ToString ();
                
                //value_dmd = ( Demand )pck_Demands.SelectedItem;
                
                // GPS
                value_lat = this.tbx_MtuGeolocationLat .Text;
                value_lon = this.tbx_MtuGeolocationLong.Text;
                value_alt = this.mtuGeolocationAlt;
                
                // Alarms dropdownlist is hidden when only has one option
                if ( this.pck_Alarms.ItemsSource.Count == 1 )
                    value_alr = ( Alarm )this.pck_Alarms.ItemsSource[ 0 ];
                else if ( this.pck_Alarms.ItemsSource.Count > 1 )
                    value_alr = ( Alarm )this.pck_Alarms.SelectedItem;
            }

            // Reset needed when same actions is launched more than one time ( Exception/error )
            this.addMtuForm.RemoveParameters ();

            #endregion

            #region Set parameters Port 1

            // Account Number / Service Port ID
            this.addMtuForm.AddParameter ( FIELD.ACCOUNT_NUMBER, value_acn );

            // Work Order / Field Order
            if ( global.WorkOrderRecording )
                this.addMtuForm.AddParameter ( FIELD.WORK_ORDER, value_wor );

            // Old MTU ID
            if ( this.actionType == ActionType.ReplaceMTU ||
                 this.actionType == ActionType.ReplaceMtuReplaceMeter )
                this.addMtuForm.AddParameter ( FIELD.MTU_ID_OLD, value_omt );

            // ( New ) Meter Serial Number
            if ( global.UseMeterSerialNumber )
                this.addMtuForm.AddParameter ( FIELD.METER_NUMBER, value_msn );

            // ( New ) Meter Reading / Initial Reading
            this.addMtuForm.AddParameter ( FIELD.METER_READING, value_mre );

            // Meter Type
            this.addMtuForm.AddParameter ( FIELD.METER_TYPE, value_mty );

            // Read Interval
            this.addMtuForm.AddParameter ( FIELD.READ_INTERVAL, value_rin );

            // Snap Reads [ SOLO SE LOGEA ¿? ]
            if ( ( DEBUG_AUTO_MODE_ON && DEBUG_SNAPSREADS_OK || ! DEBUG_AUTO_MODE_ON ) &&
                 global.AllowDailyReads &&
                 mtu.DailyReads &&
                 ! mtu.IsFamily33xx )
                this.addMtuForm.AddParameter ( FIELD.SNAP_READS, value_sre );

            // Is a two-way MTU
            if ( global.TimeToSync &&
                 mtu.TimeToSync    &&
                 mtu.FastMessageConfig )
                this.addMtuForm.AddParameter ( FIELD.TWO_WAY, value_two );

            // Alarms
            if ( value_alr != null &&
                 mtu.RequiresAlarmProfile )
                this.addMtuForm.AddParameter ( FIELD.ALARM, value_alr );

            // Demands [ SOLO SE LOGEA ¿? ]
            //if ( MtuConditions.MtuDemand )
            //    this.addMtuForm.AddParameter ( FIELD.DEMAND, value_dmd );

            // Action is about Replace Meter
            if ( isReplaceMeter )
            {
                // Old Meter Serial Number
                if ( global.UseMeterSerialNumber )
                    this.addMtuForm.AddParameter ( FIELD.METER_NUMBER_OLD, value_oms );
            
                // Old Meter Working
                if ( global.MeterWorkRecording )
                    this.addMtuForm.AddParameter ( FIELD.METER_WORKING_OLD, value_omw );
            
                // Old Meter Reading / Initial Reading
                if ( global.OldReadingRecording )
                    this.addMtuForm.AddParameter ( FIELD.METER_READING_OLD, value_omr );
                    
                // Replace Meter|Register
                if ( global.RegisterRecording )
                    this.addMtuForm.AddParameter ( FIELD.REPLACE_METER_REG, value_rpl );
            }

            #endregion

            #region Set parameters Port 2

            if ( addMtuForm.usePort2 )
            {
                // Account Number / Service Port ID
                this.addMtuForm.AddParameter ( FIELD.ACCOUNT_NUMBER_2, value_acn_2, 1 );

                // Work Order / Field Order
                if ( global.WorkOrderRecording )
                    this.addMtuForm.AddParameter ( FIELD.WORK_ORDER_2, value_wor_2, 1 );

                // ( New ) Meter Serial Number
                if ( global.UseMeterSerialNumber )
                    this.addMtuForm.AddParameter ( FIELD.METER_NUMBER_2, value_msn_2, 1 );

                // ( New ) Meter Reading / Initial Reading
                this.addMtuForm.AddParameter ( FIELD.METER_READING_2, value_mre_2, 1 );

                // Meter Type
                this.addMtuForm.AddParameter ( FIELD.METER_TYPE_2, value_mty_2, 1 );
                
                // Action is about Replace Meter
                if ( isReplaceMeter )
                {
                    // Old Meter Serial Number
                    if ( global.UseMeterSerialNumber )
                        this.addMtuForm.AddParameter ( FIELD.METER_NUMBER_OLD_2, value_oms_2, 1 );
                
                    // Old Meter Working
                    if ( global.MeterWorkRecording )
                        this.addMtuForm.AddParameter ( FIELD.METER_WORKING_OLD_2, value_omw_2, 1 );
                
                    // Old Meter Reading / Initial Reading
                    if ( global.OldReadingRecording )
                        this.addMtuForm.AddParameter ( FIELD.METER_READING_OLD_2, value_omr_2, 1 );
                        
                    // Replace Meter|Register
                    if ( global.RegisterRecording )
                        this.addMtuForm.AddParameter ( FIELD.REPLACE_METER_REG_2, value_rpl_2, 1 );
                }
            }

            #endregion

            #region Set Optional parameters

            // Gps
            if ( ! string.IsNullOrEmpty ( value_lat ) &&
                 ! string.IsNullOrEmpty ( value_lon ) )
            {
                double lat = Convert.ToDouble ( value_lat );
                double lon = Convert.ToDouble ( value_lon );
                //string latDir = ( lat < 0d ) ? "S" : "N";
                //string lonDir = ( lon < 0d ) ? "W" : "E";

                this.addMtuForm.AddParameter ( FIELD.GPS_LATITUDE,  lat );
                this.addMtuForm.AddParameter ( FIELD.GPS_LONGITUDE, lon );
                this.addMtuForm.AddParameter ( FIELD.GPS_ALTITUDE,  value_alt );
            }

            List<Parameter> optionalParams = new List<Parameter>();

            foreach ( BorderlessPicker p in optionalPickers )
                if ( p.SelectedItem != null )
                    optionalParams.Add ( new Parameter ( p.Name, p.Display, p.SelectedItem, string.Empty, 0, true ) );

            foreach ( BorderlessEntry e in optionalEntries )
                if ( ! string.IsNullOrEmpty ( e.Text ) )
                    optionalParams.Add ( new Parameter ( e.Name, e.Display, e.Text, string.Empty, 0, true ) );

            foreach (BorderlessDatePicker d in optionalDates)
                if (!string.IsNullOrEmpty(d.Date.ToShortDateString()))
                    optionalParams.Add(new Parameter(d.Name, d.Display,$"{d.Date.ToShortDateString()} 12:00:00", string.Empty, 0, true));

            foreach (BorderlessTimePicker t in optionalTimes)
                if (!string.IsNullOrEmpty(t.Time.ToString()))
                    optionalParams.Add(new Parameter(t.Name, t.Display,$"{System.DateTime.Today.ToShortDateString()} {t.Time.ToString()}", string.Empty, 0, true));

            if ( optionalParams.Count > 0 )
                this.addMtuForm.AddParameter ( FIELD.OPTIONAL_PARAMS, optionalParams );

            #endregion

            #region Events

            this.add_mtu.OnProgress -= OnProgress;
            this.add_mtu.OnProgress += OnProgress;

            this.add_mtu.OnFinish -= OnFinish;
            this.add_mtu.OnFinish += OnFinish;

            this.add_mtu.OnError -= OnError;
            this.add_mtu.OnError += OnError;

            #endregion

            // Launch action!
            await add_mtu.Run ( this.addMtuForm );
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
            InterfaceParameters[] interfacesParams = FormsApp.config.getUserParamsFromInterface ( mtu, ActionType.ReadMtu );
            
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
            // copy the pictures from MTU to user images folder

            
            await Task.Delay(100).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                if (PicturesMTU.Count != 0)
                {
                    bottomBar.GetLabelElement("label_read").Text = "Saving pictures...";
                    CopyPicturesToUserImagesFolder();
                }

                _userTapped = false;
                bottomBar.GetTGRElement("bg_action_button").NumberOfTapsRequired = 1;
                ChangeLowerButtonImage(false);
                backdark_bg.IsVisible = false;
                indicator.IsVisible = false;
                bottomBar.GetLabelElement("label_read").Text = "Successful MTU write";
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

        #region Location

        private async void GpsUpdateButton ( object sender, EventArgs e )
        {
            var position = await GetCurrentPosition();
            if (position==null)
                await dialogsSaved.AlertAsync("You must activate the GPS on the device to return coordinates","Alert");
            else
            {
                this.tbx_MtuGeolocationLat .Text = position.Latitude .ToString ();
                this.tbx_MtuGeolocationLong.Text = position.Longitude.ToString ();
                this.mtuGeolocationAlt           = position.Altitude .ToString ();
            }
        }

        public static async Task<Xamarin.Essentials.Location> GetCurrentPosition()
	    {
            Xamarin.Essentials.Location location = null;
           
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
                    bottomBar.GetImageElement("bg_action_button_img").Source = "add_mtu" + ( ( v ) ? black : btn );
                    break;

                case ActionType.ReplaceMTU:
                    bottomBar.GetImageElement("bg_action_button_img").Source = "rep_mtu" + ( ( v ) ? black : btn );
                    break;

                case ActionType.ReplaceMeter:
                    bottomBar.GetImageElement("bg_action_button_img").Source = "rep_meter" + ( ( v ) ? black : btn );
                    break;

                case ActionType.AddMtuAddMeter:
                    bottomBar.GetImageElement("bg_action_button_img").Source = "add_mtu_meter" + ( ( v ) ? black : btn );
                    break;

                case ActionType.AddMtuReplaceMeter:
                    bottomBar.GetImageElement("bg_action_button_img").Source = "add_mtu_rep_meter" + ( ( v ) ? black : btn );
                    break;

                case ActionType.ReplaceMtuReplaceMeter:
                    bottomBar.GetImageElement("bg_action_button_img").Source = "rep_mtu_rep_meter" + ( ( v ) ? black : btn );
                    break;
            }

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void BarCodeScanner(object sender, EventArgs e)
        {
            try
            {
                ImageButton ctlButton = (ImageButton)sender;
                BorderlessEntry field = (BorderlessEntry)this.FindByName((string)ctlButton.CommandParameter);
                                   
                var overlay = new ZXingDefaultOverlay
                {
                    TopText = "Hold your device up to the barcode",
                    BottomText = "Scanning will happen automatically",
                    ShowFlashButton = true,
                    AutomationId = "zxingDeafultOverlay"
                };
                overlay.FlashButtonClicked += delegate { scanPage.ToggleTorch(); };

                scanPage = new ZXingScannerPage(null,overlay);

                scanPage.OnScanResult += (result) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.PopAsync();
                        field.Text = result.Text;
                    });
                };
                await Navigation.PushAsync(scanPage);
            }
            catch (Exception ex)
            {
                await Errors.ShowAlert(new CameraException(ex.Message));
            }
        }

        private async void TakePicture(object sender, EventArgs e)
        { 
            try
            {
                //ImageButton ctlButton = (ImageButton)sender;
                string port; //= (string)ctlButton.CommandParameter;

                int mtuIdLength = Singleton.Get.Configuration.Global.MtuIdLength;
                var MtuId = await Data.Get.MemoryMap.MtuSerialNumber.GetValue();
                // var accName1 = await Data.Get.MemoryMap.P1MeterId.GetValue();
               
                string sTick = DateTime.Now.Ticks.ToString();

                if (hasTwoPorts)
                {
                    bool bResp = await DisplayAlert("Select port", "Select the port for the picture", "Port 1", "Port 2");
                    port = bResp == true ? "1" : "2";
                }
                else
                    port = "1";

                string AccFieldName = port == "1" ? "tbx_AccountNumber" : $"tbx_AccountNumber_{port}";
                BorderlessEntry field = (BorderlessEntry)this.FindByName(AccFieldName);

                string nameFile = MtuId.ToString().PadLeft(mtuIdLength, '0')+"_"+ field.Text+ sTick + "_Port"+ port;
                
                Device.BeginInvokeOnMainThread(async () =>
                {
                    MediaFile file = await PictureService.TakePictureService(nameFile);
                   
                    if (file == null)
                        return;
                   

                    string[] fileName = file.Path.Split('/');
                    nameFile = fileName[fileName.Length-1];
                    DirectoryInfo dir = new DirectoryInfo(file.Path.Substring(0,file.Path.Length-(nameFile.Length+1)));
                    
                    FileInfo[]  imagefiles = dir.GetFiles(nameFile);
                                                       
                    //PicturesMTU.Add(imagefiles[0]);
                   
                    imagefiles[0].CopyTo(Path.Combine(Mobile.ImagesPath, nameFile));
                    imagefiles[0].Delete();
                    
                    //await DisplayAlert("File Location", file.Path, "OK");

                    file.Dispose();
                });

            }
            catch (Exception)
            {
                await Errors.ShowAlert(new CameraException());
            }   

        }

        private void CopyPicturesToUserImagesFolder()
        {
           
            string nameFile, newFile;

            foreach (FileInfo file in PicturesMTU)
            {
                nameFile = file.Name;
                newFile=Path.Combine(Mobile.ImagesPath, nameFile);
                if (File.Exists(newFile))
                    File.Delete(newFile);
                
                file.CopyTo(Path.Combine(Mobile.ImagesPath, nameFile));
                file.Delete();
            }
        }

        #endregion
    }
}
