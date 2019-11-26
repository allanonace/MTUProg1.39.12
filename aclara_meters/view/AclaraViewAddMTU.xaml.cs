﻿using System;
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
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xml;
using ZXing.Net.Mobile.Forms;
using ActionType = MTUComm.Action.ActionType;
using FIELD = MTUComm.actions.AddMtuForm.FIELD;
using MTUStatus = MTUComm.Action.MTUStatus;
using ValidationResult = MTUComm.MTUComm.ValidationResult;

namespace aclara_meters.view
{
    public partial class AclaraViewAddMTU
    {
        #region Mandatory

        public const bool MANDATORY_ACCOUNTNUMBER = true;
        public const bool MANDATORY_WORKORDER = true;
        public const bool MANDATORY_OLDMTUID = true;
        public const bool MANDATORY_OLDMETERSERIAL = true;
        //public const bool MANDATORY_OLDMETERWORKING >> global.MeterWorkRecording
        public const bool MANDATORY_OLDMETERREADING = true;
        //public const bool MANDATORY_REPLACEMETER >> global.RegisterRecordingReq
        public const bool MANDATORY_METERSERIAL = true;
        public const bool MANDATORY_METERTYPE = true;
        public const bool MANDATORY_METERREADING = true;
        public const bool MANDATORY_READINTERVAL = true; // Init with select value
        public const bool MANDATORY_SNAPREADS = true; // Init with select value
        public const bool MANDATORY_TWOWAY = true;
        public const bool MANDATORY_ALARMS = true;
        public const bool MANDATORY_DEMANDS = true;
        public const bool MANDATORY_GPS = false;
        public const bool MANDATORY_RDDFIRMWARE = true;
        public const bool MANDATORY_RDDPOSITION = true;

        #endregion

        #region Constants


        public const string TWOWAY_FAST = "Fast";
        public const string TWOWAY_SLOW = "Slow";
        public const string CANCEL_COMPLETE = "Complete";
        public const string CANCEL_CANCEL = "Cancel";
        public const string CANCEL_SKIP = "Skip";
        public const string CANCEL_NOTHOME = "Not Home";
        public const string CANCEL_OTHER = "Other";
        public const string SWITCH_P2_ON = "Enable Port 2";
        public const string SWITCH_P2_OFF = "Disable Port 2";
        public const string COPY_1_TO_2 = "Copy Port 1";
        private const string AUTO_DETECTING = "Encoder auto-detect...";
        private Color COL_MANDATORY = Color.FromHex("#FF0000");
        private const int MAX_ACCOUNTNUMBER = 12;
        private const int MAX_METERREADING = 12;
        private const int MAX_RDDFIRMWARE = 12;

        private const string DUAL_PREFIX = "Repeat ";
        private const string OLD_PREFIX = "Old ";
        private const string TEXT_READING = "Meter Reading";

        private const string DUAL_ERROR = "* Both values ​​must be the same";
        private const string FIRST_METERTYPE = "* You should select Meter Type before enter readings";
        private const float OPACITY_ENABLE = 1;
        private const float OPACITY_DISABLE = 0.8f;

        private const string LB_PORT1 = "Port 1";
        private const string LB_PORT2 = "Port 2";
        private const string LB_MISC = "Miscellaneous";
        private const string LB_VALVE = "Valve";

        #endregion

        #region GUI Elements

        private IUserDialogs dialogsSaved;
        private bool _userTapped;

        // Meter Type for Port 1 and 2
        private StackLayout divDyna_MeterType_Vendors;
        private StackLayout divDyna_MeterType_Models;
        private StackLayout divDyna_MeterType_Names;
        private StackLayout divDyna_MeterType_Vendors_2;
        private StackLayout divDyna_MeterType_Models_2;
        private StackLayout divDyna_MeterType_Names_2;
        private StackLayout divDyna_MeterType_Vendors_V;
        private StackLayout divDyna_MeterType_Models_V;
        private StackLayout divDyna_MeterType_Names_V;
        private BorderlessPicker pck_MeterType_Vendors;
        private BorderlessPicker pck_MeterType_Models;
        private BorderlessPicker pck_MeterType_Names;
        private BorderlessPicker pck_MeterType_Vendors_2;
        private BorderlessPicker pck_MeterType_Models_2;
        private BorderlessPicker pck_MeterType_Names_2;
        private BorderlessPicker pck_MeterType_Vendors_V;
        private BorderlessPicker pck_MeterType_Models_V;
        private BorderlessPicker pck_MeterType_Names_V;
        private List<string> list_MeterType_Vendors;
        private List<string> list_MeterType_Models;
        private List<string> list_MeterType_Names;
        private List<string> list_MeterType_Vendors_2;
        private List<string> list_MeterType_Models_2;
        private List<string> list_MeterType_Names_2;
        private List<string> list_MeterType_Vendors_V;
        private List<string> list_MeterType_Models_V;
        private List<string> list_MeterType_Names_V;
        private List<Xml.Meter> list_MeterTypesForMtu;
        private List<Xml.Meter> list_MeterTypesForMtu_2;
        private List<Xml.Meter> list_MeterTypesForMtu_V;
        private string selected_MeterType_Vendor;
        private string selected_MeterType_Vendor_2;
        private string selected_MeterType_Vendor_V;
        private string selected_MeterType_Model;
        private string selected_MeterType_Model_2;
        private string selected_MeterType_Model_V;
        private string selected_MeterType_Name;
        private string selected_MeterType_Name_2;
 

        // Miscelanea
        private List<BorderlessPicker> optionalPickers;
        private List<BorderlessEntry> optionalEntries;
        private List<BorderlessDatePicker> optionalDates;
        private List<BorderlessTimePicker> optionalTimes;
        private List<Tuple<BorderlessPicker, Label>> optionalMandatoryPickers;
        private List<Tuple<BorderlessEntry, Label>> optionalMandatoryEntries;
        private List<Tuple<BorderlessDatePicker, Label>> optionalMandatoryDates;
        private List<Tuple<BorderlessTimePicker, Label>> optionalMandatoryTimes;

        // Snap Reads / Daily Reads
        private double snapReadsStep;

        // Alarms
        private List<Alarm> alarmsList = new List<Alarm>();
      
        // Demands
        private List<Demand> demandsList = new List<Demand>();
  
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
        private bool hasValve;
        private bool hasMeterPortTwo;
        private bool hasValvePortOne;
        private bool port2IsActivated;
        private bool p1NoNewMeterReadings;
        private bool p2NoNewMeterReadings;
        private bool waitOnClickLogic;
        private bool isCancellable;
        private bool isLogout;
        private bool isReturn;
        private bool isSettings;
        private bool snapReadsStatus = false;
        private string mtuGeolocationAlt;

        #endregion

        #region Initialization

        public AclaraViewAddMTU()
        {
            InitializeComponent();
        }

        public AclaraViewAddMTU(IUserDialogs dialogs, ActionType page)
        {
            InitializeComponent();
            BindingContext = this;

            Device.BeginInvokeOnMainThread(() =>
          {
              backdark_bg.IsVisible = true;
              indicator.IsVisible = true;
              background_scan_page.IsEnabled = false;
          });

            this.global = Singleton.Get.Configuration.Global;
            this.actionType = page;

            menuOptions = this.MenuOptions;
            dialogView = this.DialogView;
            bottomBar = this.BottomBar;

            dialogsSaved = dialogs;

            this.config = Singleton.Get.Configuration;
            this.mtuBasicInfo = Data.Get.MtuBasicInfo;

            this.detectedMtuType = (int)this.mtuBasicInfo.Type;
            currentMtu = this.config.GetMtuTypeById(this.detectedMtuType);

            this.addMtuForm = new AddMtuForm(currentMtu);

            this.add_mtu = new MTUComm.Action(
                FormsApp.ble_interface,
                this.actionType,
                FormsApp.credentialsService.UserName);

            this.add_mtu.OnProgress -= OnProgress;
            this.add_mtu.OnProgress += OnProgress;
            this.add_mtu.LinkOnProgressEvents ();

            isCancellable = false;

            PicturesMTU = new List<FileInfo>();

            Device.BeginInvokeOnMainThread(() =>
            {
                string[] texts = MTUComm.Action.ActionsTexts[this.actionType];

                name_of_window_port1.Text = texts[0] + " - " + LB_PORT1;
                name_of_window_port2.Text = texts[1] + " - " + LB_PORT2;
                name_of_window_misc.Text = texts[2] + " - " + LB_MISC;
                bottomBar.GetImageElement("bg_action_button_img").Source = texts[3];
               

                bottomBar.GetLabelElement("label_read").Opacity = 1;
               
                BarCodeEnabled = global.ShowBarCodeButton;
                ImagesEnabled = global.ShowCameraButton;

                if (Device.Idiom == TargetIdiom.Tablet)
                    LoadTabletUI();
                else LoadPhoneUI();

                NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

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

            TappedListeners();
            InitializeLowerbarLabel();

            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;

            InitializeAddMtuForm();

            Task.Run ( async () =>
            {
                if ( hasValve )
                    await InitializeRDDForm();

                await InitilizeValuesAsync();
                await LoadMetersList();
            })
            .ContinueWith(t =>
              Device.BeginInvokeOnMainThread(() =>
            {
                  Task.Delay(100)
                  .ContinueWith(t0 =>
                      Device.BeginInvokeOnMainThread(() =>
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

        private void CheckBoxController()
        {
            if (!div_SnapReads.IsEnabled)
                return;

            sld_SnapReads.Value = global.DailyReadsDefault;
            sld_SnapReads.IsEnabled = false;
            divSub_SnapReads.Opacity = 0.8;
            cbx_SnapReads.Source = "checkbox_off";

            snapReadsStep = 1.0;
            sld_SnapReads.ValueChanged += OnSnapReadsSlider_ValueChanged;

            cbx_SnapReads.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
              {
                  bool ok = !snapReadsStatus;

                  cbx_SnapReads.Source = "checkbox_" + ((ok) ? "on" : "off");
                  sld_SnapReads.IsEnabled = ok;
                  divSub_SnapReads.Opacity = (ok) ? 1 : OPACITY_DISABLE;
                  snapReadsStatus = ok;

#pragma warning disable S2589 // Boolean expressions should not be gratuitous
                  if (MANDATORY_SNAPREADS &&
#pragma warning restore S2589 // Boolean expressions should not be gratuitous
                       global.IndividualDailyReads &&
                       snapReadsStatus)
                      this.lb_SnapReads.TextColor = COL_MANDATORY;
                  else this.lb_SnapReads.TextColor = Color.Black;
              }),
            });
        }

        private async Task SetPort2Buttons()
        {
            // Port2 form starts visible or hidden depends on bit 1 of byte 28
            this.port2IsActivated = await Data.Get.MemoryMap.P2StatusFlag.GetValue();

            Device.BeginInvokeOnMainThread(() =>
            {
                // Switch On|Off port2 form
                if (!global.Port2DisableNo)
                {
                    BtnSwitchPort2.Tapped += OnClick_BtnSwitchPort2;
                    div_EnablePort2.IsEnabled = true;
                }

                // Copy current values of port1 form controls to port2 form controls
                btn_CopyPort1To2.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                  {
                      if (this.port2IsActivated)
                      {
                          this.tbx_AccountNumber_2.Text = this.tbx_AccountNumber.Text;
                          this.tbx_AccountNumber_Dual_2.Text = this.tbx_AccountNumber_Dual.Text;
                          this.tbx_WorkOrder_2.Text = this.tbx_WorkOrder.Text;
                          this.tbx_WorkOrder_Dual_2.Text = this.tbx_WorkOrder_Dual.Text;

                          if (global.NewMeterPort2isTheSame)
                          {
                              this.tbx_MeterSerialNumber_2.Text = this.tbx_MeterSerialNumber.Text;
                              this.tbx_MeterSerialNumber_Dual_2.Text = this.tbx_MeterSerialNumber_Dual.Text;
                          }
                      }
                  }),
                });
            });

            this.UpdatePortButtons();
        }

        private async Task LoadMetersList()
        {
            if (currentMtu.Port1.IsForEncoderOrEcoder ||
                 hasMeterPortTwo &&
                 currentMtu.Port2.IsForEncoderOrEcoder)
                Device.BeginInvokeOnMainThread(() =>
              {
                  bottomBar.GetLabelElement("label_read").Text = AUTO_DETECTING;
              });

            // No RDD
            if (!this.currentMtu.Port1.IsSetFlow)
            {
                // Ecoder/Encoder
                if (currentMtu.Port1.IsForEncoderOrEcoder)
                {
                    bool autoDetect = await this.add_mtu.MTUComm.AutodetectMeterEncoders(currentMtu);
                    if (autoDetect)
                        this.list_MeterTypesForMtu = this.config.MeterTypes.FindByEncoderTypeAndLiveDigits(
                            currentMtu.Port1.MeterProtocol,
                            currentMtu.Port1.MeterLiveDigits);

                    // If auto-detect fails, show all Encoder/Ecoder Meters    
                    if (!autoDetect ||
                         this.list_MeterTypesForMtu.Count <= 0)
                        this.list_MeterTypesForMtu = this.config.MeterTypes.FindAllForEncodersAndEcoders();
                }
                // Pulse
                else this.list_MeterTypesForMtu = this.config.MeterTypes.FindByPortTypeAndFlow(currentMtu);
            }
            // RDD
            else this.list_MeterTypesForMtu_V = this.config.MeterTypes.FindByPortTypeAndFlow(currentMtu);

            if (this.currentMtu.TwoPorts)
            {
                // No RDD
                if (!this.currentMtu.Port2.IsSetFlow)
                {
                    // Ecoder/Encoder
                    if (currentMtu.Port2.IsForEncoderOrEcoder)
                    {
                        bool autoDetect = await this.add_mtu.MTUComm.AutodetectMeterEncoders(currentMtu, 2);
                        if (autoDetect)
                            this.list_MeterTypesForMtu_2 = this.config.MeterTypes.FindByEncoderTypeAndLiveDigits(
                                currentMtu.Port2.MeterProtocol,
                                currentMtu.Port2.MeterLiveDigits);

                        // If auto-detect fails, show all Encoder/Ecoder Meters    
                        if (!autoDetect ||
                             this.list_MeterTypesForMtu_2.Count <= 0)
                            this.list_MeterTypesForMtu_2 = this.config.MeterTypes.FindAllForEncodersAndEcoders();
                    }
                    // Pulse
                    else this.list_MeterTypesForMtu_2 = this.config.MeterTypes.FindByPortTypeAndFlow(currentMtu, 1);
                }
                // RDD
                else this.list_MeterTypesForMtu_V = this.config.MeterTypes.FindByPortTypeAndFlow(currentMtu, 1);
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                // RDD devices is always in port two
                if (! hasValvePortOne )
                    this.InitializePicker_MeterType ();

                if ( hasMeterPortTwo )
                    this.InitializePicker_MeterType_2 ();
                
                if ( hasValve )
                    this.InitializePicker_MeterType_V ();
            });

        }

        private async Task InitializeRDDForm()
        {
            #region RDD

            List<string> list = new List<string>()
            {
                "Close", "Open", "Partial Open"
            };

            //Now I am given ItemsSorce to the Pickers
            pck_ValvePosition.ItemsSource = list;

            dynamic map = Data.Get.MemoryMap;

            string rddPosition = await map.RDDValvePosition.GetValue();
            ulong rddSerial = await map.RDDSerialNumber.GetValue();
            string rddBattery = await map.RDDBatteryStatus.GetValue();

            Device.BeginInvokeOnMainThread(() =>
            {
                this.tbx_RDDPosition       .Text = rddPosition;
                this.tbx_RDDSerialNumber   .Text = rddSerial.ToString();
                this.tbx_Battery           .Text = rddBattery;
                this.tbx_RDDFirmwareVersion.Text = this.global.RDDFirmwareVersion;
            });

            #endregion
        }

        private async Task InitilizeValuesAsync()
        {
            dynamic map = Data.Get.MemoryMap;
            if (map.ContainsMember("FastMessagingConfigFreq"))
            {
                bool two = await map.FastMessagingConfigFreq.GetValue();
                int twoway = two ? 1 : 0;

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!div_RDDGeneral.IsVisible)
                        pck_TwoWay.SelectedIndex = twoway;
                    else pck_TwoWay_V.SelectedIndex = twoway;
                });
            }
        }

        private void InitializeAddMtuForm()
        {
            try
            {
                #region Conditions

                this.currentMtu = this.addMtuForm.mtu;
                this.mtuBasicInfo = Data.Get.MtuBasicInfo;

                #endregion

                #region Two ports or RDD

                if (this.currentMtu.Port1.IsSetFlow)
                {                    
                    valve_command();
                    port1label.IsVisible = false;
                    port2label.IsVisible = false;
                    div_RDDGeneral.IsVisible = true;
                    hasValve = true;
                    hasMeterPortTwo = false;
                    hasValvePortOne = true;
                }
                else if (this.currentMtu.TwoPorts && this.currentMtu.Port2.IsSetFlow)
                {
                    port1label.IsVisible = true;
                    port2label.IsVisible = false;
                    valvelabel.IsVisible = true;
                    hasValve = true;
                    hasMeterPortTwo = false;
                    hasValvePortOne = false;
                }
                else
                {
                    this.hasMeterPortTwo = this.currentMtu.TwoPorts;
                    port2label.IsVisible = this.hasMeterPortTwo;
                    valvelabel.IsVisible = false;
                    hasValve = false;
                }

                #endregion

                #region Labels

                this.port1label.Text = LB_PORT1;
                this.port2label.Text = LB_PORT2;
                this.misclabel.Text = LB_MISC;
                this.valvelabel.Text = LB_VALVE;

                #endregion

                #region Account Number / Service Port ID

                // Dual entry
                bool useDualAccountNumber = global.AccountDualEntry;

                // Port 1
                this.div_AccountNumber_Dual.IsVisible = useDualAccountNumber;
                this.div_AccountNumber_Dual.IsEnabled = useDualAccountNumber;

                // Port 2
                this.div_AccountNumber_Dual_2.IsVisible = hasMeterPortTwo && useDualAccountNumber;
                this.div_AccountNumber_Dual_2.IsEnabled = hasMeterPortTwo && useDualAccountNumber;

                // Valve
                this.div_AccountNumber_Dual_V.IsVisible = hasValve && useDualAccountNumber;
                this.div_AccountNumber_Dual_V.IsEnabled = hasValve && useDualAccountNumber;

                #endregion

                #region Work Order / Field Order

                bool useWorkOrder = global.WorkOrderRecording;

                // Port 1            
                this.div_WorkOrder.IsVisible = useWorkOrder;
                this.div_WorkOrder.IsEnabled = useWorkOrder;

                // Port 2
                this.div_WorkOrder_2.IsVisible = hasMeterPortTwo && useWorkOrder;
                this.div_WorkOrder_2.IsEnabled = hasMeterPortTwo && useWorkOrder;

                // RDD
                this.div_WorkOrder_V.IsVisible = hasValve && useWorkOrder;
                this.div_WorkOrder_V.IsEnabled = hasValve && useWorkOrder;

                // Dual entry
                bool useDualWorkOrder = global.WorkOrderDualEntry && useWorkOrder;

                // Port 1
                this.div_WorkOrder_Dual.IsVisible = useDualWorkOrder;
                this.div_WorkOrder_Dual.IsEnabled = useDualWorkOrder;

                // Port 2
                this.div_WorkOrder_Dual_2.IsVisible = hasMeterPortTwo && useDualWorkOrder;
                this.div_WorkOrder_Dual_2.IsEnabled = hasMeterPortTwo && useDualWorkOrder;

                // Valve
                this.div_WorkOrder_Dual_V.IsVisible = hasValve && useDualWorkOrder;
                this.div_WorkOrder_Dual_V.IsEnabled = hasValve && useDualWorkOrder;

                #endregion

                #region Old MTU ID

                bool isReplaceMtu = (
                    this.actionType == ActionType.ReplaceMTU ||
                    this.actionType == ActionType.ReplaceMtuReplaceMeter);

                this.div_OldMtuId.IsVisible = isReplaceMtu;
                this.div_OldMtuId.IsEnabled = isReplaceMtu;

                #endregion

                #region Meter Serial Number and Reading ( and OLD fields )

                this.Initialize_OldMeterPickers();

                // Action is about Replace Meter
                bool isReplaceMeter = (
                    this.actionType == ActionType.ReplaceMeter ||
                    this.actionType == ActionType.ReplaceMtuReplaceMeter ||
                    this.actionType == ActionType.AddMtuReplaceMeter);

                // ( New ) Meter Serial Number
                bool useMeterSerialNumber = global.UseMeterSerialNumber;

                // Port 1
                this.div_MeterSerialNumber.IsVisible = useMeterSerialNumber;
                this.div_MeterSerialNumber.IsEnabled = useMeterSerialNumber;

                // Port 2
                this.div_MeterSerialNumber_2.IsVisible = hasMeterPortTwo && useMeterSerialNumber;
                this.div_MeterSerialNumber_2.IsEnabled = hasMeterPortTwo && useMeterSerialNumber;

                ///////////////////////////////////
                // Dual entry - ( New ) Meter Serial Number
                bool useDualSeriaNumber = global.NewSerialNumDualEntry && useMeterSerialNumber;

                // Port 1
                this.div_MeterSerialNumber_Dual.IsVisible = useDualSeriaNumber;
                this.div_MeterSerialNumber_Dual.IsEnabled = useDualSeriaNumber;

                // Port 2
                this.div_MeterSerialNumber_Dual_2.IsVisible = hasMeterPortTwo && useDualSeriaNumber;
                this.div_MeterSerialNumber_Dual_2.IsEnabled = hasMeterPortTwo && useDualSeriaNumber;

                ///////////////////////////////////
                // ( New ) Meter Reading
                this.p1NoNewMeterReadings = !this.currentMtu.Port1.IsForEncoderOrEcoder;
                this.p2NoNewMeterReadings = this.hasMeterPortTwo && !this.currentMtu.Port2.IsForEncoderOrEcoder;

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
                this.div_OldMeterSerialNumber_2.IsVisible = hasMeterPortTwo && isReplaceMeter && useMeterSerialNumber;
                this.div_OldMeterSerialNumber_2.IsEnabled = hasMeterPortTwo && isReplaceMeter && useMeterSerialNumber;

                ///////////////////////////////////
                // Dual entry - Old Meter Serial Number
                bool useDualOldSeriaNumber = global.OldSerialNumDualEntry && this.div_OldMeterSerialNumber.IsVisible;

                // Port 1
                this.div_OldMeterSerialNumber_Dual.IsVisible = useDualOldSeriaNumber;
                this.div_OldMeterSerialNumber_Dual.IsEnabled = useDualOldSeriaNumber;

                // Port 2
                this.div_OldMeterSerialNumber_Dual_2.IsVisible = hasMeterPortTwo && useDualOldSeriaNumber;
                this.div_OldMeterSerialNumber_Dual_2.IsEnabled = hasMeterPortTwo && useDualOldSeriaNumber;

                ///////////////////////////////////
                // Old Meter Working ( Change reason )
                bool useMeterWorking = isReplaceMeter && global.MeterWorkRecording;

                // Port 1
                this.div_OldMeterWorking.IsVisible = useMeterWorking;
                this.div_OldMeterWorking.IsEnabled = useMeterWorking;

                // Port 2
                this.div_OldMeterWorking_2.IsVisible = hasMeterPortTwo && useMeterWorking;
                this.div_OldMeterWorking_2.IsEnabled = hasMeterPortTwo && useMeterWorking;

                ///////////////////////////////////
                // Old Meter Reading
                bool useOldReading = isReplaceMeter && global.OldReadingRecording;

                // Port 1
                this.div_OldMeterReading.IsVisible = useOldReading;
                this.div_OldMeterReading.IsEnabled = useOldReading;

                // Port 2
                this.div_OldMeterReading_2.IsVisible = hasMeterPortTwo && useOldReading;
                this.div_OldMeterReading_2.IsEnabled = hasMeterPortTwo && useOldReading;

                ///////////////////////////////////
                // Dual entry - Old Meter Reading
                bool useDualOldReading = global.OldReadingDualEntry && useOldReading;

                // Port 1
                this.div_OldMeterReading_Dual.IsVisible = useDualOldReading;
                this.div_OldMeterReading_Dual.IsEnabled = useDualOldReading;

                // Port 2
                this.div_OldMeterReading_Dual_2.IsVisible = hasMeterPortTwo && useDualOldReading;
                this.div_OldMeterReading_Dual_2.IsEnabled = hasMeterPortTwo && useDualOldReading;

                ///////////////////////////////////
                // Replace Meter/Register
                bool useReplaceMeterRegister = isReplaceMeter && global.RegisterRecording;

                // Port 1
                this.div_ReplaceMeterRegister.IsVisible = useReplaceMeterRegister;
                this.div_ReplaceMeterRegister.IsEnabled = useReplaceMeterRegister;

                // Port 2
                this.div_ReplaceMeterRegister_2.IsVisible = hasMeterPortTwo && useReplaceMeterRegister;
                this.div_ReplaceMeterRegister_2.IsEnabled = hasMeterPortTwo && useReplaceMeterRegister;

                ///////////////////////////////////
                // Introduce values from right to left, but finish with the same number
                // e.g. 1 _ _ -> 1 2 _ -> 1 2 3
                //      _ _ 3 -> _ 2 3 -> 1 2 3
                FlowDirection flow = (global.ReverseReading) ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
                this.tbx_MeterReading.FlowDirection = flow;
                this.tbx_MeterReading_2.FlowDirection = flow;
                this.tbx_MeterReading_Dual.FlowDirection = flow;
                this.tbx_MeterReading_Dual_2.FlowDirection = flow;
                this.tbx_OldMeterReading.FlowDirection = flow;
                this.tbx_OldMeterReading_2.FlowDirection = flow;
                this.tbx_OldMeterReading_Dual.FlowDirection = flow;
                this.tbx_OldMeterReading_Dual_2.FlowDirection = flow;

                ///////////////////////////////////
                // New Meter readings will be disabled before meter type was selected
                this.tbx_MeterReading.IsEnabled = false;
                this.tbx_MeterReading_2.IsEnabled = false;
                this.tbx_MeterReading_Dual.IsEnabled = false;
                this.tbx_MeterReading_Dual_2.IsEnabled = false;

                this.div_MeterReading.Opacity = OPACITY_DISABLE;
                this.divSub_MeterReading_Dual.Opacity = OPACITY_DISABLE;
                this.divSub_MeterReading_2.Opacity = OPACITY_DISABLE;
                this.divSub_MeterReading_Dual_2.Opacity = OPACITY_DISABLE;

                this.lb_MeterReading_MeterType.Text = FIRST_METERTYPE;
                this.lb_MeterReading_DualMeterType.Text = FIRST_METERTYPE;
                this.lb_MeterReading_MeterType_2.Text = FIRST_METERTYPE;
                this.lb_MeterReading_DualMeterType_2.Text = FIRST_METERTYPE;

                this.lb_MeterReading_MeterType.IsVisible = true;
                this.lb_MeterReading_DualMeterType.IsVisible = true;
                this.lb_MeterReading_MeterType_2.IsVisible = true;
                this.lb_MeterReading_DualMeterType_2.IsVisible = true;

                #endregion

                #region Read Interval

                this.InitializePicker_ReadInterval(this.mtuBasicInfo, this.currentMtu);

                // Use IndividualReadInterval tag to enable o disable read interval picker
                this.pck_ReadInterval.IsEnabled = global.IndividualReadInterval;
                if ( !this.pck_ReadInterval.IsEnabled )
                {
                    this.div_ReadInterval.BackgroundColor = Color.LightGray;
                    this.pck_ReadInterval.BackgroundColor = Color.LightGray;
                    this.pck_ReadInterval.TextColor = Color.Gray;
                }

                #endregion

                #region Snap Reads / Daily Reads

                bool useDailyReads = global.AllowDailyReads && this.currentMtu.DailyReads && !this.currentMtu.IsFamily33xx;
                bool changeableDailyReads = global.IndividualDailyReads;

                int dailyReadsDefault = global.DailyReadsDefault;

                this.div_SnapReads.IsEnabled = useDailyReads;
                this.div_SnapReads.IsVisible = useDailyReads;
                this.divSub_SnapReads.IsEnabled = changeableDailyReads && useDailyReads;
                this.divSub_SnapReads.Opacity = (changeableDailyReads && useDailyReads) ? 1 : 0.8d;

                this.snapReadsStep = 1.0;

                if (useDailyReads)
                    this.sld_SnapReads.ValueChanged += OnSnapReadsSlider_ValueChanged;

                this.sld_SnapReads.Value = (dailyReadsDefault > -1) ? dailyReadsDefault : 13;


                #endregion

                #region 2-Way

                // Only for 34xx MTUs and above
                bool useTwoWay = global.TimeToSync &&
                                this.currentMtu.TimeToSync &&
                                this.currentMtu.FastMessageConfig;

                if (!div_RDDGeneral.IsVisible)
                {
                    this.Initialize_TwoWay(pck_TwoWay);
                    div_TwoWay.IsVisible = useTwoWay;
                    div_TwoWay.IsEnabled = useTwoWay;
                }
                else
                {
                    this.Initialize_TwoWay(pck_TwoWay_V);
                    div_TwoWay_V.IsVisible = useTwoWay;
                    div_TwoWay_V.IsEnabled = useTwoWay;
                }
                #endregion

                #region Alarms

                div_Alarms.IsEnabled   = false;
                div_Alarms.IsVisible   = false;
                div_Alarms_V.IsEnabled = false;
                div_Alarms_V.IsVisible = false;

                if ( this.currentMtu.RequiresAlarmProfile )
                {
                    // Remove "Scripting" option in interactive mode
                    alarmsList = new List<Alarm> ( config.Alarms.FindByMtuType_Interactive ( this.detectedMtuType ) );
                    
                    if (!div_RDDGeneral.IsVisible)
                    {
                        pck_Alarms.ItemDisplayBinding = new Binding("Name");

                        div_Alarms.IsEnabled   = true;
                        div_Alarms.IsVisible   = true;
                        pck_Alarms.ItemsSource = alarmsList;

                        // Hide alarms dropdownlist if contains only one option
                        div_Alarms.IsVisible = (alarmsList.Count > 1);
                    }
                    else
                    {
                        pck_Alarms_V.ItemDisplayBinding = new Binding("Name");

                        div_Alarms_V.IsEnabled   = true;
                        div_Alarms_V.IsVisible   = true;
                        pck_Alarms_V.ItemsSource = alarmsList;

                        // Hide alarms dropdownlist if contains only one option
                        div_Alarms_V.IsVisible = (alarmsList.Count > 1);
                    }
                }

                #endregion

                #region Demands

                div_Demands.IsEnabled   = false;
                div_Demands.IsVisible   = false;
                div_Demands_V.IsEnabled = false;
                div_Demands_V.IsVisible = false;

                if ( this.currentMtu.MtuDemand &&
                     this.currentMtu.FastMessageConfig )
                {
                    // Remove "Scripting" option in interactive mode
                    demandsList = new List<Demand> ( config.Demands.FindByMtuType_Interactive ( this.detectedMtuType ) );

                    if (!div_RDDGeneral.IsVisible)
                    {
                        pck_Demands.ItemDisplayBinding = new Binding("Name");

                        div_Demands.IsEnabled   = true;
                        div_Demands.IsVisible   = true;
                        pck_Demands.ItemsSource = demandsList;

                        // Hide alarms dropdownlist if contains only one option
                        div_Demands.IsVisible = ( demandsList.Count > 1 );
                    }
                    else
                    {
                        pck_Demands_V.ItemDisplayBinding = new Binding("Name");

                        div_Demands_V.IsEnabled   = true;
                        div_Demands_V.IsVisible   = true;
                        pck_Demands_V.ItemsSource = demandsList;

                        // Hide alarms dropdownlist if contains only one option
                        div_Demands_V.IsVisible = ( demandsList.Count > 1 );
                    }
                }
                
                #endregion

                #region Misc

                if (this.global.Options.Count > 0)
                    InitializeOptionalFields();

                #endregion

                #region Cancel reason

                // Load cancel reasons from Global.xml
                List<string> cancelReasons = this.global.Cancel;

                if (!cancelReasons.Contains("Other"))
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
                int acnLength = (global.AccountLength <= MAX_ACCOUNTNUMBER) ? global.AccountLength : MAX_ACCOUNTNUMBER;
                tbx_AccountNumber.MaxLength = acnLength;
                tbx_AccountNumber_2.MaxLength = acnLength;
                tbx_AccountNumber_V.MaxLength = acnLength;
                tbx_AccountNumber_Dual.MaxLength = acnLength;
                tbx_AccountNumber_Dual_2.MaxLength = acnLength;
                tbx_AccountNumber_Dual_V.MaxLength = acnLength;

                tbx_WorkOrder.MaxLength = global.WorkOrderLength;
                tbx_WorkOrder_2.MaxLength = global.WorkOrderLength;
                tbx_WorkOrder_V.MaxLength = global.WorkOrderLength;
                tbx_WorkOrder_Dual.MaxLength = global.WorkOrderLength;
                tbx_WorkOrder_Dual_2.MaxLength = global.WorkOrderLength;
                tbx_WorkOrder_Dual_V.MaxLength = global.WorkOrderLength;

                tbx_OldMtuId.MaxLength = global.MtuIdLength;

                tbx_OldMeterSerialNumber.MaxLength = global.MeterNumberLength;
                tbx_OldMeterSerialNumber_2.MaxLength = global.MeterNumberLength;
                tbx_OldMeterSerialNumber_Dual.MaxLength = global.MeterNumberLength;
                tbx_OldMeterSerialNumber_Dual_2.MaxLength = global.MeterNumberLength;

                tbx_MeterSerialNumber.MaxLength = global.MeterNumberLength;
                tbx_MeterSerialNumber_2.MaxLength = global.MeterNumberLength;
                tbx_MeterSerialNumber_Dual.MaxLength = global.MeterNumberLength;
                tbx_MeterSerialNumber_Dual_2.MaxLength = global.MeterNumberLength;

                tbx_OldMeterReading.MaxLength = MAX_METERREADING;
                tbx_OldMeterReading_2.MaxLength = MAX_METERREADING;
                tbx_OldMeterReading_Dual.MaxLength = MAX_METERREADING;
                tbx_OldMeterReading_Dual_2.MaxLength = MAX_METERREADING;

                tbx_MeterReading.MaxLength = MAX_METERREADING;
                tbx_MeterReading_2.MaxLength = MAX_METERREADING;
                tbx_MeterReading_Dual.MaxLength = MAX_METERREADING;
                tbx_MeterReading_Dual_2.MaxLength = MAX_METERREADING;

                tbx_MtuGeolocationLat.MaxLength  = 10;
                tbx_MtuGeolocationLong.MaxLength = 10;

                tbx_RDDFirmwareVersion.MaxLength = MAX_RDDFIRMWARE;

                #endregion

                #region Labels

                // Account Number
                this.lb_AccountNumber.Text = global.AccountLabel;
                this.lb_AccountNumber_Dual.Text = DUAL_PREFIX + global.AccountLabel;
                this.lb_AccountNumber_2.Text = global.AccountLabel;
                this.lb_AccountNumber_Dual_2.Text = DUAL_PREFIX + global.AccountLabel;

                // Work Order
                this.lb_WorkOrder.Text = global.WorkOrderLabel;
                this.lb_WorkOrder_Dual.Text = DUAL_PREFIX + global.WorkOrderLabel;
                this.lb_WorkOrder_2.Text = global.WorkOrderLabel;
                this.lb_WorkOrder_Dual_2.Text = DUAL_PREFIX + global.WorkOrderLabel;

                string NEW_LABEL = (isReplaceMeter) ? "New " : string.Empty;

                // Meter Reading
                this.lb_MeterReading.Text = NEW_LABEL + TEXT_READING;
                this.lb_MeterReading_Dual.Text = DUAL_PREFIX + NEW_LABEL + TEXT_READING;
                this.lb_MeterReading_2.Text = NEW_LABEL + TEXT_READING;
                this.lb_MeterReading_Dual_2.Text = DUAL_PREFIX + NEW_LABEL + TEXT_READING;

                this.lb_OldMeterReading.Text = OLD_PREFIX + TEXT_READING;
                this.lb_OldMeterReading_Dual.Text = DUAL_PREFIX + OLD_PREFIX + TEXT_READING;
                this.lb_OldMeterReading_2.Text = OLD_PREFIX + TEXT_READING;
                this.lb_OldMeterReading_Dual_2.Text = DUAL_PREFIX + OLD_PREFIX + TEXT_READING;

                // Meter Serial Number
                if (useMeterSerialNumber)
                {
                    if (isReplaceMeter)
                    {
                        this.lb_MeterSerialNumber.Text = global.NewMeterLabel;
                        this.lb_MeterSerialNumber_Dual.Text = DUAL_PREFIX + global.NewMeterLabel;
                        this.lb_MeterSerialNumber_2.Text = global.NewMeterLabel;
                        this.lb_MeterSerialNumber_Dual_2.Text = DUAL_PREFIX + global.NewMeterLabel;

                        this.lb_OldMeterSerialNumber.Text = OLD_PREFIX + global.SerialNumLabel;
                        this.lb_OldMeterSerialNumber_Dual.Text = DUAL_PREFIX + OLD_PREFIX + global.SerialNumLabel;
                        this.lb_OldMeterSerialNumber_2.Text = OLD_PREFIX + global.SerialNumLabel;
                        this.lb_OldMeterSerialNumber_Dual_2.Text = DUAL_PREFIX + OLD_PREFIX + global.SerialNumLabel;
                    }
                    else
                    {
                        this.lb_MeterSerialNumber.Text = global.SerialNumLabel;
                        this.lb_MeterSerialNumber_Dual.Text = DUAL_PREFIX + global.SerialNumLabel;
                        this.lb_MeterSerialNumber_2.Text = global.SerialNumLabel;
                        this.lb_MeterSerialNumber_Dual_2.Text = DUAL_PREFIX + global.SerialNumLabel;

                        this.lb_OldMeterSerialNumber.Text = OLD_PREFIX + global.SerialNumLabel;
                        this.lb_OldMeterSerialNumber_Dual.Text = DUAL_PREFIX + OLD_PREFIX + global.SerialNumLabel;
                        this.lb_OldMeterSerialNumber_2.Text = OLD_PREFIX + global.SerialNumLabel;
                        this.lb_OldMeterSerialNumber_Dual_2.Text = DUAL_PREFIX + OLD_PREFIX + global.SerialNumLabel;
                    }
                }

                #endregion

                #region Mandatory Fields

                if (global.ColorEntry)
                {
                    // Account Number
                    if (MANDATORY_ACCOUNTNUMBER)
                    {
                        // Port 1
                        this.lb_AccountNumber.TextColor = COL_MANDATORY;
                        this.lb_AccountNumber_Dual.TextColor = COL_MANDATORY;

                        // Port 2
                        this.lb_AccountNumber_2.TextColor = COL_MANDATORY;
                        this.lb_AccountNumber_Dual_2.TextColor = COL_MANDATORY;

                        // Valve
                        this.lb_AccountNumber_V.TextColor = COL_MANDATORY;
                        this.lb_AccountNumber_Dual_V.TextColor = COL_MANDATORY;
                    }

                    // Work Order
                    if (MANDATORY_WORKORDER)
                    {
                        // Port 1
                        this.lb_WorkOrder.TextColor = COL_MANDATORY;
                        this.lb_WorkOrder_Dual.TextColor = COL_MANDATORY;

                        // Port 2
                        this.lb_WorkOrder_2.TextColor = COL_MANDATORY;
                        this.lb_WorkOrder_Dual_2.TextColor = COL_MANDATORY;

                        // Valve
                        this.lb_WorkOrder_V.TextColor = COL_MANDATORY;
                        this.lb_WorkOrder_Dual_V.TextColor = COL_MANDATORY;
                    }

                    // Old MTU ID
                    if (MANDATORY_OLDMTUID)
                        this.lb_OldMtuId.TextColor = COL_MANDATORY;

                    // Old Meter Serial Number
                    if (MANDATORY_OLDMETERSERIAL)
                    {
                        // Port 1
                        this.lb_OldMeterSerialNumber.TextColor = COL_MANDATORY;
                        this.lb_OldMeterSerialNumber_Dual.TextColor = COL_MANDATORY;

                        // Port 2
                        this.lb_OldMeterSerialNumber_2.TextColor = COL_MANDATORY;
                        this.lb_OldMeterSerialNumber_Dual_2.TextColor = COL_MANDATORY;
                    }

                    // Old Meter Working
                    if (global.MeterWorkRecording)
                    {
                        // Port 1
                        this.lb_OldMeterWorking.TextColor = COL_MANDATORY;

                        // Port 2
                        this.lb_OldMeterWorking_2.TextColor = COL_MANDATORY;
                    }

                    // Old Meter Reading
                    if (MANDATORY_OLDMETERREADING)
                    {
                        // Port 1
                        this.lb_OldMeterReading.TextColor = COL_MANDATORY;
                        this.lb_OldMeterReading_Dual.TextColor = COL_MANDATORY;

                        // Port 2
                        this.lb_OldMeterReading_2.TextColor = COL_MANDATORY;
                        this.lb_OldMeterReading_Dual_2.TextColor = COL_MANDATORY;
                    }

                    // Replace Meter|Register
                    if (global.RegisterRecordingReq)
                    {
                        // Port 1
                        this.lb_ReplaceMeterRegister.TextColor = COL_MANDATORY;

                        // Port 2
                        this.lb_ReplaceMeterRegister_2.TextColor = COL_MANDATORY;
                    }

                    // ( New ) Meter Serial Number
                    if (MANDATORY_METERSERIAL)
                    {
                        // Port 1
                        this.lb_MeterSerialNumber.TextColor = COL_MANDATORY;
                        this.lb_MeterSerialNumber_Dual.TextColor = COL_MANDATORY;

                        // Port 2
                        this.lb_MeterSerialNumber_2.TextColor = COL_MANDATORY;
                        this.lb_MeterSerialNumber_Dual_2.TextColor = COL_MANDATORY;
                    }

                    // ( New ) Meter Reading
                    if (MANDATORY_METERREADING)
                    {
                        // Port 1
                        this.lb_MeterReading.TextColor = COL_MANDATORY;
                        this.lb_MeterReading_Dual.TextColor = COL_MANDATORY;

                        // Port 2
                        this.lb_MeterReading_2.TextColor = COL_MANDATORY;
                        this.lb_MeterReading_Dual_2.TextColor = COL_MANDATORY;
                    }

                    // Read Interval
                    if (MANDATORY_READINTERVAL &&
                        global.IndividualReadInterval)
                        this.lb_ReadInterval.TextColor = COL_MANDATORY;

                    // Snap Reads
                    if (MANDATORY_SNAPREADS &&
                        global.IndividualDailyReads &&
                        snapReadsStatus)
                        this.lb_SnapReads.TextColor = COL_MANDATORY;

                    // Two-Way
                    if (MANDATORY_TWOWAY)
                        this.lb_TwoWay.TextColor = COL_MANDATORY;

                    // Alarms
                    if (MANDATORY_ALARMS)
                        this.lb_Alarms.TextColor = COL_MANDATORY;

                    // Demands
                    if (MANDATORY_DEMANDS)
                        this.lb_Demands.TextColor = COL_MANDATORY;

                    // GPS
                    if (MANDATORY_GPS)
                        this.lb_GPS.TextColor = COL_MANDATORY;
                }

                #endregion

                #region Port 2 Buttons

                // Button for enable|disable the second port
                this.div_EnablePort2.IsEnabled = global.Port2DisableNo;
                if (!this.div_EnablePort2.IsEnabled)
                {
                    this.block_view_port2.IsVisible = this.port2IsActivated;
                    this.btn_EnablePort2.Text = (this.port2IsActivated) ? SWITCH_P2_OFF : SWITCH_P2_ON;               
                }
                // Auto-enable second port because Port2DisableNo is true
                else
                {
                    this.port2IsActivated = true;
                    this.block_view_port2.IsVisible = true;
                    this.div_EnablePort2.IsVisible = false;
                    this.div_EnablePort2.IsEnabled = false;
                }

                // Button for copy port 1 common fields values to port 2
                this.div_CopyPort1To2.IsVisible = this.port2IsActivated;
                this.div_CopyPort1To2.IsEnabled = this.port2IsActivated;

                btn_CopyPort1To2.Text = COPY_1_TO_2;

                #endregion

                #region UnFocus events

                // Account Number
                System.Action valEqAccountNumber = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_AccountNumber,
                        this.tbx_AccountNumber_Dual,
                        this.lb_AccountNumber_DualError);
                System.Action valEqAccountNumber_2 = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_AccountNumber_2,
                        this.tbx_AccountNumber_Dual_2,
                        this.lb_AccountNumber_DualError_2);
                System.Action valEqAccountNumber_V = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_AccountNumber_V,
                        this.tbx_AccountNumber_Dual_V,
                        this.lb_AccountNumber_DualError_V);

                this.tbx_AccountNumber_Dual.Unfocused += (s, e) => { valEqAccountNumber(); };
                this.tbx_AccountNumber.Unfocused += (s, e) => { valEqAccountNumber(); };
                this.tbx_AccountNumber_2.Unfocused += (s, e) => { valEqAccountNumber_2(); };
                this.tbx_AccountNumber_Dual_2.Unfocused += (s, e) => { valEqAccountNumber_2(); };
                this.tbx_AccountNumber_V.Unfocused += (s, e) => { valEqAccountNumber_V(); };
                this.tbx_AccountNumber_Dual_V.Unfocused += (s, e) => { valEqAccountNumber_V(); };

                // Work Order
                System.Action valEqWorkOrder = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_WorkOrder,
                        this.tbx_WorkOrder_Dual,
                        this.lb_WorkOrder_DualError);
                System.Action valEqWorkOrder_2 = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_WorkOrder_2,
                        this.tbx_WorkOrder_Dual_2,
                        this.lb_WorkOrder_DualError_2);
                System.Action valEqWorkOrder_V = () =>
                ValidateEqualityOnFocus(
                    this.tbx_WorkOrder_V,
                    this.tbx_WorkOrder_Dual_V,
                    this.lb_WorkOrder_DualError_V);

                this.tbx_WorkOrder.Unfocused += (s, e) => { valEqWorkOrder(); };
                this.tbx_WorkOrder_Dual.Unfocused += (s, e) => { valEqWorkOrder(); };
                this.tbx_WorkOrder_2.Unfocused += (s, e) => { valEqWorkOrder_2(); };
                this.tbx_WorkOrder_Dual_2.Unfocused += (s, e) => { valEqWorkOrder_2(); };
                this.tbx_WorkOrder_V.Unfocused += (s, e) => { valEqWorkOrder_V(); };
                this.tbx_WorkOrder_Dual_V.Unfocused += (s, e) => { valEqWorkOrder_V(); };

                // ( New ) Meter Serial Number
                System.Action valEqMeterSerialNumber = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_MeterSerialNumber,
                        this.tbx_MeterSerialNumber_Dual,
                        this.lb_MeterSerialNumber_DualError);
                System.Action valEqMeterSerialNumber_2 = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_MeterSerialNumber_2,
                        this.tbx_MeterSerialNumber_Dual_2,
                        this.lb_MeterSerialNumber_DualError_2);

                this.tbx_MeterSerialNumber.Unfocused += (s, e) => { valEqMeterSerialNumber(); };
                this.tbx_MeterSerialNumber_Dual.Unfocused += (s, e) => { valEqMeterSerialNumber(); };
                this.tbx_MeterSerialNumber_2.Unfocused += (s, e) => { valEqMeterSerialNumber_2(); };
                this.tbx_MeterSerialNumber_Dual_2.Unfocused += (s, e) => { valEqMeterSerialNumber_2(); };

                // Old Meter Serial Number
                System.Action valEqOldMeterSerialNumber = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_OldMeterSerialNumber,
                        this.tbx_OldMeterSerialNumber_Dual,
                        this.lb_OldMeterSerialNumber_DualError);
                System.Action valEqOldMeterSerialNumber_2 = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_OldMeterSerialNumber_2,
                        this.tbx_OldMeterSerialNumber_Dual_2,
                        this.lb_OldMeterSerialNumber_DualError_2);

                this.tbx_OldMeterSerialNumber.Unfocused += (s, e) => { valEqOldMeterSerialNumber(); };
                this.tbx_OldMeterSerialNumber_Dual.Unfocused += (s, e) => { valEqOldMeterSerialNumber(); };
                this.tbx_OldMeterSerialNumber_2.Unfocused += (s, e) => { valEqOldMeterSerialNumber_2(); };
                this.tbx_OldMeterSerialNumber_Dual_2.Unfocused += (s, e) => { valEqOldMeterSerialNumber_2(); };

                // ( New ) Meter Reading
                System.Action valEqMeterReading = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_MeterReading,
                        this.tbx_MeterReading_Dual,
                        this.lb_MeterReading_DualError);
                System.Action valEqMeterReading_2 = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_MeterReading_2,
                        this.tbx_MeterReading_Dual_2,
                        this.lb_MeterReading_DualError_2);

                this.tbx_MeterReading.Unfocused += (s, e) => { valEqMeterReading(); };
                this.tbx_MeterReading_Dual.Unfocused += (s, e) => { valEqMeterReading(); };
                this.tbx_MeterReading.Unfocused += (s, e) => { valEqMeterReading_2(); };
                this.tbx_MeterReading_Dual_2.Unfocused += (s, e) => { valEqMeterReading_2(); };

                // Old Meter Reading
                System.Action valOldEqMeterReading = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_OldMeterReading,
                        this.tbx_OldMeterReading_Dual,
                        this.lb_OldMeterReading_DualError);
                System.Action valOldEqMeterReading_2 = () =>
                    ValidateEqualityOnFocus(
                        this.tbx_OldMeterReading_2,
                        this.tbx_OldMeterReading_Dual_2,
                        this.lb_OldMeterReading_DualError_2);

                this.tbx_OldMeterReading.Unfocused += (s, e) => { valOldEqMeterReading(); };
                this.tbx_OldMeterReading_Dual.Unfocused += (s, e) => { valOldEqMeterReading(); };
                this.tbx_OldMeterReading.Unfocused += (s, e) => { valOldEqMeterReading_2(); };
                this.tbx_OldMeterReading_Dual_2.Unfocused += (s, e) => { valOldEqMeterReading_2(); };

                this.lb_AccountNumber_DualError.Text = DUAL_ERROR;
                this.lb_AccountNumber_DualError_2.Text = DUAL_ERROR;
                this.lb_WorkOrder_DualError.Text = DUAL_ERROR;
                this.lb_WorkOrder_DualError_2.Text = DUAL_ERROR;
                this.lb_MeterSerialNumber_DualError.Text = DUAL_ERROR;
                this.lb_MeterSerialNumber_DualError_2.Text = DUAL_ERROR;
                this.lb_OldMeterSerialNumber_DualError.Text = DUAL_ERROR;
                this.lb_OldMeterSerialNumber_DualError_2.Text = DUAL_ERROR;
                this.lb_MeterReading_DualError.Text = DUAL_ERROR;
                this.lb_MeterReading_DualError_2.Text = DUAL_ERROR;
                this.lb_OldMeterReading_DualError.Text = DUAL_ERROR;
                this.lb_OldMeterReading_DualError_2.Text = DUAL_ERROR;

                #endregion
            }
            catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
            {
                if (!Errors.IsOwnException(e))
                    Errors.LogErrorNowAndContinue( new PuckCantCommWithMtuException () );
                else Errors.LogErrorNowAndContinue ( e );

                // Return to main menu
                Navigation.PopToRootAsync(false);
                
            }
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
            optionalMandatoryPickers = new List<Tuple<BorderlessPicker, Label>>();
            optionalMandatoryEntries = new List<Tuple<BorderlessEntry, Label>>();
            optionalMandatoryDates = new List<Tuple<BorderlessDatePicker, Label>>();
            optionalMandatoryTimes = new List<Tuple<BorderlessTimePicker, Label>>();
            foreach (Option optionalField in this.global.Options)
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
                    FontSize=17,
                    FontAttributes=FontAttributes.Bold,
                    Margin = new Thickness(0, 4, 0, 0)
                };

                BorderlessPicker optionalPicker = null;
                BorderlessEntry optionalEntry = null;
                BorderlessDatePicker optionalDate = null;
                BorderlessTimePicker optionalTime = null;

                bool isList = optionalField.Type.Equals("list");
                if (isList)
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
                else if (optionalField.Format == "date")
                {
                   
                    optionalDate = new BorderlessDatePicker()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        // HeightRequest = 70,
                        FontSize = 17
                    };
                    optionalDate.Name = optionalField.Name.Replace(" ", "_");
                    optionalDate.Display = optionalField.Display;

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
                    
                    optionalTime = new BorderlessTimePicker()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        // HeightRequest = 70,
                        FontSize = 17
                    };
                    optionalTime.Name = optionalField.Name.Replace(" ", "_");
                    optionalTime.Display = optionalField.Display;

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
 
                    Keyboard keyboard = Keyboard.Default;
                    if (format.Equals("alphanumeric")) keyboard = Keyboard.Numeric;
                    else if (format.Equals("time")) keyboard = Keyboard.Numeric;

                    optionalEntry = new BorderlessEntry()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 40,
                        Keyboard = keyboard,
                        FontSize = 17
                    };
                    optionalEntry.Name = optionalField.Name.Replace(" ", "_");
                    optionalEntry.Display = optionalField.Display;

                    CommentsLengthValidatorBehavior behavior = new CommentsLengthValidatorBehavior
                    {
                        MaxLength = optionalField.Len
                    };

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
                if (optionalField.Required)
                {
                    if (isList)
                        this.optionalMandatoryPickers.Add(new Tuple<BorderlessPicker, Label>(optionalPicker, optionalLabel));
                    else if (optionalField.Format == "date")
                        this.optionalMandatoryDates.Add(new Tuple<BorderlessDatePicker, Label>(optionalDate, optionalLabel));
                    else if (optionalField.Format == "time")
                        this.optionalMandatoryTimes.Add(new Tuple<BorderlessTimePicker, Label>(optionalTime, optionalLabel));
                    else this.optionalMandatoryEntries.Add(new Tuple<BorderlessEntry, Label>(optionalEntry, optionalLabel));

                    if (global.ColorEntry)
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            optionalLabel.TextColor = COL_MANDATORY;
                        });
                }
            }
        }

        private void TappedListeners()
        {
            bottomBar.GetTGRElement("btnTakePicture").Tapped += TakePicture;
            bottomBar.GetImageElement("imgTakePicture").IsVisible = global.ShowCameraButton;
            TopBar.GetTGRElement("back_button").Tapped += ReturnToMainView;
            bottomBar.GetTGRElement("bg_action_button").Tapped += AddMtu;
           
            dialogView.GetTGRElement("turnoffmtu_ok").Tapped += TurnOnOffMTUOkTapped;
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
            valvelabel.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => valve_command()),
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

            this.GoToPage();
        }

        private void GoToPage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (actionType == ActionType.DataRead)
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewDataRead(dialogsSaved, this.actionType), false);
                else if (actionType == ActionType.ValveOperation)
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewRemoteDisconnect(dialogsSaved, this.actionType), false);
                else
                    Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved, this.actionType), false);
            });

            backdark_bg.IsVisible = false;
            indicator.IsVisible = false;
            background_scan_page.IsEnabled = true;
        }

        private void Cancel_No(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;
            this.cancelReasonOtherInput.Text = String.Empty;
            this.cancelReasonPicker.SelectedIndex = 0;            
        }

        private async void Confirm_Yes_LogOut(object sender, EventArgs e)
        {
            // Upload log files
            if (global.UploadPrompt)
                await GenericUtilsClass.UploadFiles();

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

        private void Confirm_No_LogOut(object sender, EventArgs e)
        {
            dialogView.OpenCloseDialog("dialog_logoff", false);
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;           
        }

        #endregion

        private void InitializeLowerbarLabel()
        {
            bottomBar.GetLabelElement("label_read").Text = "Push Button to START";
        }

        private void InitializePicker_MeterType()
        {
            list_MeterType_Vendors = this.config.MeterTypes.GetVendorsFromMeters(list_MeterTypesForMtu);

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
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 4, 0, 0)
            };

            if (this.global.ColorEntry)
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
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 4, 0, 0)
            };

            if (this.global.ColorEntry)
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
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 4, 0, 0)
            };

            if (this.global.ColorEntry)
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

        private void InitializePicker_MeterType_2()
        {
            list_MeterType_Vendors_2 = this.config.MeterTypes.GetVendorsFromMeters(list_MeterTypesForMtu_2);

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
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
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
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
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
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
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

            this.div_MeterType_2.Children.Add(divDyna_MeterType_Vendors_2);
            this.div_MeterType_2.Children.Add(divDyna_MeterType_Models_2);
            this.div_MeterType_2.Children.Add(divDyna_MeterType_Names_2);

            divDyna_MeterType_Names_2.IsVisible = false;
            divDyna_MeterType_Models_2.IsVisible = false;
        }

        private void InitializePicker_MeterType_V()
        {
            list_MeterType_Vendors_V = this.config.MeterTypes.GetVendorsFromMeters(list_MeterTypesForMtu_V);

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

            pck_MeterType_Vendors_V = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = list_MeterType_Vendors_V
            };

            pck_MeterType_Vendors_V.SelectedIndexChanged += MeterVendorsVPicker_SelectedIndexChanged;

            divDyna_MeterType_Vendors_V = new StackLayout()
            {
                StyleId = "bloque" + 1
            };

            Label meterVendors2Label = new Label()
            {
                Text = "Vendor",
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 4, 0, 0)
            };


            #region Color Entry

            if (this.global.ColorEntry)
            {
                meterVendors2Label.TextColor = COL_MANDATORY;

            }

            #endregion

            meterVendors2ContainerD.Children.Add(pck_MeterType_Vendors_V);
            meterVendors2ContainerC.Content = meterVendors2ContainerD;
            meterVendors2ContainerB.Content = meterVendors2ContainerC;
            divDyna_MeterType_Vendors_V.Children.Add(meterVendors2Label);
            divDyna_MeterType_Vendors_V.Children.Add(meterVendors2ContainerB);

            list_MeterType_Models_V = new List<string>();

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

            pck_MeterType_Models_V = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = list_MeterType_Models_V,
                StyleId = "pickerModelosV"
            };

            pck_MeterType_Models_V.SelectedIndexChanged += MeterModelsVPicker_SelectedIndexChanged;

            divDyna_MeterType_Models_V = new StackLayout()
            {
                StyleId = "bloque" + 2
            };

            Label meterModels2Label = new Label()
            {
                Text = "Model",
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 4, 0, 0)
            };


            #region Color Entry

            if (this.global.ColorEntry)
            {
                meterModels2Label.TextColor = COL_MANDATORY;

            }

            #endregion


            meterModels2ContainerD.Children.Add(pck_MeterType_Models_V);
            meterModels2ContainerC.Content = meterModels2ContainerD;
            meterModels2ContainerB.Content = meterModels2ContainerC;
            divDyna_MeterType_Models_V.Children.Add(meterModels2Label);
            divDyna_MeterType_Models_V.Children.Add(meterModels2ContainerB);

            list_MeterType_Names_V = new List<string>();

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

            pck_MeterType_Names_V = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = list_MeterType_Names_2,
                StyleId = "pickerNameV"
            };

            divDyna_MeterType_Names_V = new StackLayout()
            {
                StyleId = "bloque" + 3
            };

            Label meterNames2Label = new Label()
            {
                Text = "Meter Type",
                FontSize = 17,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0, 4, 0, 0)
            };


            #region Color Entry

            if (this.global.ColorEntry)
                meterNames2Label.TextColor = COL_MANDATORY;

            #endregion

            meterNames2ContainerD.Children.Add(pck_MeterType_Names_V);
            meterNames2ContainerC.Content = meterNames2ContainerD;
            meterNames2ContainerB.Content = meterNames2ContainerC;
            divDyna_MeterType_Names_V.Children.Add(meterNames2Label);
            divDyna_MeterType_Names_V.Children.Add(meterNames2ContainerB);

            this.div_MeterType_V.Children.Add(divDyna_MeterType_Vendors_V);
            this.div_MeterType_V.Children.Add(divDyna_MeterType_Models_V);
            this.div_MeterType_V.Children.Add(divDyna_MeterType_Names_V);

            divDyna_MeterType_Names_V.IsVisible = false;
            divDyna_MeterType_Models_V.IsVisible = false;
        }

        private void InitializePicker_ReadInterval(
            MTUBasicInfo mtuBasicInfo,
            Mtu mtu)
        {
            List<string> readIntervalList;

            if (mtuBasicInfo.version >= this.global.LatestVersion)
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
            if (!mtu.TimeToSync)
            {
                readIntervalList.AddRange(new string[]{
                    "10 Min",
                    "5 Min"
                });
            }

            pck_ReadInterval.ItemsSource = readIntervalList;

            // If tag NormXmitInterval is present inside Global,
            // its value is used as default selection
            string normXmitInterval = global.NormXmitInterval;
            if (!string.IsNullOrEmpty(normXmitInterval))
            {
                // Convert "Hr/s" to "Hour/s"
                normXmitInterval = normXmitInterval.ToLower()
                                   .Replace("hr", "hour")
                                   .Replace("h", "H");

                int index = readIntervalList.IndexOf(normXmitInterval);
                pck_ReadInterval.SelectedIndex = ((index > -1) ? index : readIntervalList.IndexOf("1 Hour"));
            }
            // If tag NormXmitInterval is NOT present, use "1 Hour" as default value
            else
                pck_ReadInterval.SelectedIndex = readIntervalList.IndexOf("1 Hour");
        }

        private void Initialize_TwoWay(BorderlessPicker picker)
        {
            List<string> twoWayList = new List<string>()
            {
                TWOWAY_SLOW,
                TWOWAY_FAST
            };

            picker.ItemsSource = twoWayList;
        }

        private void Initialize_OldMeterPickers()
        {
            List<string> list = new List<string>()
            {
                "Yes",
                "No",
                "Broken"
            };

            pck_OldMeterWorking.ItemsSource = list;
            pck_OldMeterWorking_2.ItemsSource = list;

            list = new List<string>();
            string regRecordingItems = global.RegisterRecordingItems;
            string[] values = new string[] { "Meter", "Register", "Both" };

            if (!string.IsNullOrEmpty(regRecordingItems) &&
                 regRecordingItems.Contains("1"))
            {
                char[] chars = regRecordingItems.ToCharArray();

                int i = 0;
                foreach (char c in chars)
                {
                    if (c.Equals('1'))
                        list.Add(values[i]);
                    i++;
                }
            }
            else list.AddRange(values);

            //Now I am given ItemsSorce to the Pickers
            pck_ReplaceMeterRegister.ItemsSource = list;
            pck_ReplaceMeterRegister_2.ItemsSource = list;

            int index;
            string defvalue = global.RegisterRecordingDefault;
            if (!string.IsNullOrEmpty(defvalue) &&
                 (index = list.IndexOf(
                    defvalue.ToUpper()[0] +
                    defvalue.ToLower().Substring(1))) > -1)
            {
                pck_ReplaceMeterRegister.SelectedIndex = index;
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

        private void CancelReasonPicker_SelectedIndexChanged(object sender, EventArgs e)
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

        private void SetMeterVendor(int selectedIndex)
        {
            if (selectedIndex > -1)
            {
                selected_MeterType_Vendor = list_MeterType_Vendors[selectedIndex];

                list_MeterType_Models = this.config.MeterTypes.GetModelsByVendorFromMeters(list_MeterTypesForMtu, selected_MeterType_Vendor);
                selected_MeterType_Name = "";

                pck_MeterType_Models.ItemsSource = list_MeterType_Models;
                divDyna_MeterType_Models.IsVisible = true;
                divDyna_MeterType_Names.IsVisible = false;
            }
        }

        private void MeterVendorsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetMeterVendor(((BorderlessPicker)sender).SelectedIndex);
        }

        private void MeterVendors2Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = ((BorderlessPicker)sender).SelectedIndex;

            if (selectedIndex > -1)
            {
                selected_MeterType_Vendor_2 = list_MeterType_Vendors_2[selectedIndex];

                list_MeterType_Models_2 = this.config.MeterTypes.GetModelsByVendorFromMeters(list_MeterTypesForMtu_2, selected_MeterType_Vendor_2);
                selected_MeterType_Name_2 = "";

                pck_MeterType_Models_2.ItemsSource = list_MeterType_Models_2;
                divDyna_MeterType_Models_2.IsVisible = true;
                divDyna_MeterType_Names_2.IsVisible = false;
            }
        }

        private void MeterVendorsVPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = ((BorderlessPicker)sender).SelectedIndex;

            if (selectedIndex > -1)
            {
                selected_MeterType_Vendor_V = list_MeterType_Vendors_V[selectedIndex];

                list_MeterType_Models_V = this.config.MeterTypes.GetModelsByVendorFromMeters(list_MeterTypesForMtu_V, selected_MeterType_Vendor_V);
              
                pck_MeterType_Models_V.ItemsSource = list_MeterType_Models_V;
                divDyna_MeterType_Models_V.IsVisible = true;
                divDyna_MeterType_Names_V.IsVisible = false;
            }
        }

        private void SetMeterModel(int selectedIndex)
        {
            if (selectedIndex > -1)
            {
                pck_MeterType_Names.ItemDisplayBinding = new Binding("Display");

                selected_MeterType_Model = list_MeterType_Models[selectedIndex];

                List<Meter> meterlist = this.config.MeterTypes.GetMetersByModelAndVendorFromMeters(list_MeterTypesForMtu, selected_MeterType_Vendor, selected_MeterType_Model);

                pck_MeterType_Names.ItemsSource = meterlist;
                divDyna_MeterType_Models.IsVisible = true;
                divDyna_MeterType_Names.IsVisible = true;
            }
        }

        private void MeterModelsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetMeterModel(((BorderlessPicker)sender).SelectedIndex);
        }

        private void MeterModels2Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = ((BorderlessPicker)sender).SelectedIndex;

            if (selectedIndex > -1)
            {
                pck_MeterType_Names_2.ItemDisplayBinding = new Binding("Display");

                selected_MeterType_Model_2 = list_MeterType_Models_2[selectedIndex];

                List<Meter> meterlist2 = this.config.MeterTypes.GetMetersByModelAndVendorFromMeters(list_MeterTypesForMtu_2, selected_MeterType_Vendor_2, selected_MeterType_Model_2);

                pck_MeterType_Names_2.ItemsSource = meterlist2;
                divDyna_MeterType_Models_2.IsVisible = true;
                divDyna_MeterType_Names_2.IsVisible = true;
            }
        }

        private void MeterModelsVPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = ((BorderlessPicker)sender).SelectedIndex;

            if (selectedIndex > -1)
            {
                pck_MeterType_Names_V.ItemDisplayBinding = new Binding("Display");

                selected_MeterType_Model_V = list_MeterType_Models_V[selectedIndex];

                List<Meter> meterlist2 = this.config.MeterTypes.GetMetersByModelAndVendorFromMeters(list_MeterTypesForMtu_V, selected_MeterType_Vendor_V, selected_MeterType_Model_V);

                pck_MeterType_Names_V.ItemsSource = meterlist2;
                divDyna_MeterType_Models_V.IsVisible = true;
                divDyna_MeterType_Names_V.IsVisible = true;
            }
        }

        private void MeterNamesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((BorderlessPicker)sender).SelectedIndex > -1)
            {
                Meter selectedMeter = (Meter)((BorderlessPicker)sender).SelectedItem;

                selected_MeterType_Name = selectedMeter.Display;

                Utils.Print(selected_MeterType_Name + " Selected");

                Device.BeginInvokeOnMainThread(() =>
                {
                    // Update MeterReading field length to use and validate
                    this.tbx_MeterReading.MaxLength = selectedMeter.LiveDigits;
                    this.tbx_MeterReading_Dual.MaxLength = selectedMeter.LiveDigits;
                 
                    this.div_MeterReading.Opacity = OPACITY_ENABLE;
                    this.divSub_MeterReading_Dual.Opacity = OPACITY_ENABLE;
                   
                    this.tbx_MeterReading.IsEnabled = true;
                    this.tbx_MeterReading_Dual.IsEnabled = true;
                    this.btnScanMeterReading.IsEnabled = true;
                    this.btnScanMeterReadingDual.IsEnabled = true;
                  
                    this.lb_MeterReading_MeterType.IsVisible = false;
                    this.lb_MeterReading_DualMeterType.IsVisible = false;
                });
            }
        }

        private void MeterNames2Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((BorderlessPicker)sender).SelectedIndex > -1)
            {
                Meter selectedMeter = (Meter)((BorderlessPicker)sender).SelectedItem;

                selected_MeterType_Name_2 = selectedMeter.Display;

                Utils.Print(selected_MeterType_Name_2 + " Selected");

                Device.BeginInvokeOnMainThread(() =>
                {
                    // Update MeterReading field length to use and validate
                    this.tbx_MeterReading_2.MaxLength = selectedMeter.LiveDigits;
                    this.tbx_MeterReading_Dual_2.MaxLength = selectedMeter.LiveDigits;
                   
                    this.divSub_MeterReading_2.Opacity = OPACITY_ENABLE;
                    this.divSub_MeterReading_Dual_2.Opacity = OPACITY_ENABLE;
                  
                    this.tbx_MeterReading_2.IsEnabled = true;
                    this.tbx_MeterReading_Dual_2.IsEnabled = true;
                    this.btnScannerMeterReading_2.IsEnabled = true;
                    this.btnScannerMeterReadingDual_2.IsEnabled = true;
                   
                    this.lb_MeterReading_MeterType_2.IsVisible = false;
                    this.lb_MeterReading_DualMeterType_2.IsVisible = false;
                
                });
            }
        }

 
        #endregion

        #region Sliders

        void OnSnapReadsSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / snapReadsStep);

            sld_SnapReads.Value = newStep * snapReadsStep;
            lb_SnapReads_Num.Text = sld_SnapReads.Value.ToString();
        }

        #endregion

        #region Menu options

        object menu_sender;
        ItemTappedEventArgs menu_tappedevents;

        private void OnItemSelected(Object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        // Event for Menu Item selection, here we are going to handle navigation based
        // on user selection in menu ListView
        private async void OnMenuItemSelected(object sender, ItemTappedEventArgs e)
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
                                
                                await NavigationController(page);
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

        private async Task NavigationController(
            ActionType actionTarget)
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

        private async Task SwitchToControler ( ActionType page )
        {
            backdark_bg.IsVisible = true;
            indicator.IsVisible = true;
            background_scan_page.Opacity = 1;

            background_scan_page.IsEnabled = true;

            if ( Device.Idiom == TargetIdiom.Phone )
            {
                await ContentNav.TranslateTo(-310, 0, 175, Easing.SinOut);
                await shadoweffect.TranslateTo(-310, 0, 175, Easing.SinOut);
            }

            switch ( await base.ValidateNavigation ( page ) )
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
                    dialogView.UpdateNoActionText ();
                    dialogView.OpenCloseDialog("dialog_NoAction", true);
                    return;
            }

            this.actionTypeNew = page;

            switch ( page )
            {
                case ActionType.DataRead:
                case ActionType.ValveOperation:
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
                            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReadMTU(dialogsSaved, page), false);

                        })
                    );

                    #endregion
                    break;
                case ActionType.TurnOffMtu:
                case ActionType.TurnOnMtu:
                    #region Turn On|Off Controller

                    await Task.Delay(200).ContinueWith(t =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            dialogView.CloseDialogs ();

                            #region Check ActionVerify

                            turnOnOffIsOn = ( page == ActionType.TurnOnMtu );

                            if ( this.global.ActionVerify )
                            {
                                Label lb = ( Label )dialogView.FindByName ( "lb_TurnOnOff_Question" );
                                lb.Text = $"Are you sure you want to turn {( ( turnOnOffIsOn ) ? "On" : "Off" )} MTU?";

                                dialog_open_bg.IsVisible = true;
                                turnoff_mtu_background.IsVisible = true;
                                dialogView.OpenCloseDialog("dialog_turnoff_one", true);
                            }
                            else
                            {
                                this.actionType = this.actionTypeNew;
                                CallLoadViewTurnOnOff ();
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

        private void CallLoadViewTurnOnOff ()
        {
            dialogView.OpenCloseDialog("dialog_turnoff_one", false);
            dialogView.OpenCloseDialog("dialog_turnoff_two", true);

            Task.Factory.StartNew ( TurnOnOffMethod );
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
            indicator.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Navigation.PopToRootAsync(false);
        }

        private bool turnOnOffIsOn;

        private void TurnOnOffMTUOkTapped(object sender, EventArgs e)
        {
            CallLoadViewTurnOnOff ();
        }

        private async Task TurnOnOffMethod ()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Label lb = ( Label )dialogView.FindByName ( "lb_TurnOnOff_Wait" );
                lb.Text = $"Turning {(( turnOnOffIsOn ) ? "On" : "Off")} MTU";
            });

            MTUComm.Action turnOffAction = new MTUComm.Action (
                FormsApp.ble_interface,
                ( turnOnOffIsOn ) ? MTUComm.Action.ActionType.TurnOnMtu : MTUComm.Action.ActionType.TurnOffMtu,
                FormsApp.credentialsService.UserName );

            turnOffAction.OnFinish -= TurnOff_OnOffFinish;
            turnOffAction.OnFinish += TurnOff_OnOffFinish;

            turnOffAction.OnError  -= TurnOff_OnOffError;
            turnOffAction.OnError  += TurnOff_OnOffError;

            await turnOffAction.Run ();
        }

        public async Task TurnOff_OnOffFinish ( object sender, Delegates.ActionFinishArgs args )
        {
            await Task.Delay(2000).ContinueWith(t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label textResult = (Label)dialogView.FindByName("dialog_turnoff_text");
                    textResult.Text = $"MTU turned {(( turnOnOffIsOn ) ? "On" : "Off")} Successfully";

                    dialogView.OpenCloseDialog("dialog_turnoff_two", false);
                    dialogView.OpenCloseDialog("dialog_turnoff_three", true);
                }));
        }

        public void TurnOff_OnOffError ()
        {
            Task.Delay(2000).ContinueWith(t =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    Label textResult = (Label)dialogView.FindByName("dialog_turnoff_text");
                    textResult.Text = $"MTU turned {(( turnOnOffIsOn ) ? "On" : "Off")} Unsuccessfully";

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

        private async void submit_send(object sender, EventArgs e3)
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
            /* Data and Mtu Id for cancel log */
            this.add_mtu.AddAdditionalParameter(new Parameter("MtuId","MTU ID",this.mtuBasicInfo.Id));
            /* Port 1 */
            if (!string.IsNullOrEmpty(tbx_AccountNumber.Text))
                this.add_mtu.AddAdditionalParameter(new Parameter("AccountNumber","Account Number",tbx_AccountNumber.Text,null, 1));

            if (!string.IsNullOrEmpty(tbx_MeterSerialNumber.Text))
                this.add_mtu.AddAdditionalParameter(new Parameter("OldMeterSerialNumber", "Old Meter Serial Number", tbx_MeterSerialNumber.Text,null, 1));
            /* Port 2 */
            if (this.add_mtu.CurrentMtu.TwoPorts)
            {
                if (!string.IsNullOrEmpty(tbx_AccountNumber_2.Text))
                    this.add_mtu.AddAdditionalParameter(new Parameter("AccountNumber", "Account Number", tbx_AccountNumber_2.Text,null, 2));

                if (!string.IsNullOrEmpty(tbx_MeterSerialNumber_2.Text))
                    this.add_mtu.AddAdditionalParameter(new Parameter("OldMeterSerialNumber", "Old Meter Serial Number", tbx_MeterSerialNumber_2.Text,null, 2));
            }

            this.add_mtu.Cancel ( selectedCancelReason );

            dialog_open_bg.IsVisible = false;
            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;

            if (Device.Idiom == TargetIdiom.Tablet && !isLogout && !isReturn && !isSettings)
            {
                this.actionType = this.actionTypeNew;

                await SwitchToControler ( this.actionType );
            }
            else
            {
                #region I guess it's logout time ...

                await Task.Run(async () =>
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
            }
            catch (Exception e25)
            {
                Utils.Print(e25.StackTrace);
            }

            background_scan_page.IsEnabled = true;
            Application.Current.MainPage = new NavigationPage(new AclaraViewLogin(dialogsSaved));
        }

        private void misc_command()
        {
            miscview.Opacity = 0;

            port1label.Opacity = 0.5;
            misclabel.Opacity = 1;
            port2label.Opacity = 0.5;
            valvelabel.Opacity = 0.5;

            port1label.FontSize = 19;
            misclabel.FontSize = 22;
            port2label.FontSize = 19;
            valvelabel.FontSize = 19;

            port1view.IsVisible = false;
            port2view.IsVisible = false;
            miscview.IsVisible = true;
            valveview.IsVisible = false;

            miscview.FadeTo(1, 200);

        }

        private void valve_command()
        {
            valveview.Opacity = 0;

            port1label.Opacity = 0.5;
            misclabel.Opacity = 0.5;
            port2label.Opacity = 0.5;
            valvelabel.Opacity = 1;

            port1label.FontSize = 19;
            misclabel.FontSize = 19;
            port2label.FontSize = 19;
            valvelabel.FontSize = 22;

            port1view.IsVisible = false;
            port2view.IsVisible = false;
            miscview.IsVisible = false;
            valveview.IsVisible = true;

            valveview.FadeTo(1, 200);
        }

        private void port2_command()
        {
            port2view.Opacity = 0;

            port1label.Opacity = 0.5;
            misclabel.Opacity = 0.5;
            valveview.Opacity = 0.5;
            port2label.Opacity = 1;

            port1label.FontSize = 19;
            misclabel.FontSize = 19;
            valvelabel.FontSize = 19;
            port2label.FontSize = 22;

            port1view.IsVisible = false;
            port2view.IsVisible = true;
            valveview.IsVisible = false;
            miscview.IsVisible = false;

            port2view.FadeTo(1, 200);
        }

        private void port1_command()
        {
            port1view.Opacity = 0;

            port1label.Opacity = 1;
            misclabel.Opacity = 0.5;
            port2label.Opacity = 0.5;
            valvelabel.Opacity = 0.5;

            port1label.FontSize = 22;
            misclabel.FontSize = 19;
            port2label.FontSize = 19;
            valvelabel.FontSize = 19;

            port1view.IsVisible = true;
            port2view.IsVisible = false;
            miscview.IsVisible = false;
            valveview.IsVisible = false;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1121:Assignments should not be made from within sub-expressions", Justification = "<pendiente>")]
        private bool ValidateFields ( ref string msgError )
        {

            Mtu mtu = this.add_mtu.CurrentMtu;

            string FILL_ERROR = string.Empty;
            string DUAL_ERROR_V = string.Empty;

            #region Methods

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
            
            dynamic NoValNOrEmpty = new Func<string,bool> ( ( value ) =>
                                      ! string.IsNullOrEmpty ( value ) &&
                                      ! Validations.IsNumeric ( value ) );

            #endregion

            #region Port 1

            // Check if it is not for a RDD device
            if ( ! mtu.Port1.IsSetFlow )
            {
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
                
                FILL_ERROR = "Field '_' is incorrectly filled";
                DUAL_ERROR_V = " ( Second entry )";
                
                if      ( ( badAcn &= ! noAcn ) ) msgError = this.lb_AccountNumber.Text;
                else if ( ( badDAc &= ! noDAc ) ) msgError = this.lb_AccountNumber.Text + DUAL_ERROR_V;
                else if ( ( badWor &= ! noWor ) ) msgError = this.lb_WorkOrder.Text;
                else if ( ( badDWr &= ! noDWr ) ) msgError = this.lb_WorkOrder.Text + DUAL_ERROR_V;
                else if ( ( badOMt &= ! noOMt ) ) msgError = this.lb_OldMtuId.Text;
                else if ( ( badOMs &= ! noOMs ) ) msgError = this.lb_OldMeterSerialNumber.Text;
                else if ( ( badDOs &= ! noDOs ) ) msgError = this.lb_OldMeterSerialNumber.Text + DUAL_ERROR_V;
                else if ( ( badOMw &= ! noOMw ) ) msgError = this.lb_OldMeterWorking.Text;
                else if ( ( badOMr &= ! noOMr ) ) msgError = this.lb_OldMeterReading.Text;
                else if ( ( badDOr &= ! noDOr ) ) msgError = this.lb_OldMeterReading.Text + DUAL_ERROR_V;
                else if ( ( badRpc &= ! noRpc ) ) msgError = this.lb_ReplaceMeterRegister.Text;
                else if ( ( badMsn &= ! noMsn ) ) msgError = this.lb_MeterSerialNumber.Text;
                else if ( ( badDMs &= ! noDMs ) ) msgError = this.lb_MeterSerialNumber.Text + DUAL_ERROR_V;
                else if ( ( badMty &= ! noMty ) ) msgError = "Meter Type";
                else if ( ( badMre &= ! noMre ) ) msgError = this.lb_MeterReading.Text;
                else if ( ( badDMr &= ! noDMr ) ) msgError = this.lb_MeterReading.Text + DUAL_ERROR_V;
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
                DUAL_ERROR_V = "Field '_' dual entries are not the same";

                if ( global.AccountDualEntry &&
                    ! string.Equals ( tbx_AccountNumber.Text, tbx_AccountNumber_Dual.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_AccountNumber.Text );
                    return false;
                }
                
                if ( global.WorkOrderDualEntry &&
                    ! string.Equals ( tbx_WorkOrder.Text, tbx_WorkOrder_Dual.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_WorkOrder.Text );
                    return false;
                }
                
                if ( global.OldSerialNumDualEntry &&
                    ! string.Equals ( tbx_OldMeterSerialNumber.Text, tbx_OldMeterSerialNumber_Dual.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_OldMeterSerialNumber.Text );
                    return false;
                }
                
                if ( global.OldReadingDualEntry &&
                    ! string.Equals ( tbx_OldMeterReading.Text, tbx_OldMeterReading_Dual.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_OldMeterReading.Text );
                    return false;
                }
                
                if ( global.NewSerialNumDualEntry &&
                    ! string.Equals ( tbx_MeterSerialNumber.Text, tbx_MeterSerialNumber_Dual.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_MeterSerialNumber.Text );
                    return false;
                }

                if ( global.ReadingDualEntry &&
                    ! string.Equals ( tbx_MeterReading.Text, tbx_MeterReading_Dual.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_MeterReading.Text );
                    return false;
                }
            }

            #endregion

            #region Port 2

            // Check if it is not for a RDD device
            if ( this.hasMeterPortTwo &&
                 this.port2IsActivated &&
                 ! mtu.Port2.IsSetFlow )
            {
                // No mandatory fields can be empty
                // TRUE when the field does not need to be validated ( not mandatory and empty/not selected )
                bool noAcn = EmptyNoReq ( this.tbx_AccountNumber_2       .Text, MANDATORY_ACCOUNTNUMBER   );
                bool noWor = EmptyNoReq ( this.tbx_WorkOrder_2           .Text, MANDATORY_WORKORDER       );
                bool noOMs = EmptyNoReq ( this.tbx_OldMeterSerialNumber_2.Text, MANDATORY_OLDMETERSERIAL  );
                bool noMsn = EmptyNoReq ( this.tbx_MeterSerialNumber_2   .Text, MANDATORY_METERSERIAL     );
                bool noOMr = EmptyNoReq ( this.tbx_OldMeterReading_2     .Text, MANDATORY_OLDMETERREADING );
                bool noMre = EmptyNoReq ( this.tbx_MeterReading_2        .Text, MANDATORY_METERREADING    );
                
                bool noOMw = NoSelNoReq ( this.pck_OldMeterWorking_2     .SelectedIndex, global.MeterWorkRecording   );
                bool noRpc = NoSelNoReq ( this.pck_ReplaceMeterRegister_2.SelectedIndex, global.RegisterRecordingReq );
                bool noMty = NoSelNoReq ( this.pck_MeterType_Names_2     .SelectedIndex, MANDATORY_METERTYPE       );
    
                bool noDAc = EmptyNoReq ( this.tbx_AccountNumber_Dual_2       .Text, MANDATORY_ACCOUNTNUMBER   );
                bool noDWr = EmptyNoReq ( this.tbx_WorkOrder_Dual_2           .Text, MANDATORY_WORKORDER       );
                bool noDOs = EmptyNoReq ( this.tbx_OldMeterSerialNumber_Dual_2.Text, MANDATORY_OLDMETERSERIAL  );
                bool noDMs = EmptyNoReq ( this.tbx_MeterSerialNumber_Dual_2   .Text, MANDATORY_METERSERIAL     );
                bool noDOr = EmptyNoReq ( this.tbx_OldMeterReading_Dual_2     .Text, MANDATORY_OLDMETERREADING );
                bool noDMr = EmptyNoReq ( this.tbx_MeterReading_Dual_2        .Text, MANDATORY_METERREADING    );
    
                // Correct length
                // TRUE when the field has not correct length or is not selected yet
                bool badAcn =                                               NoEqNum ( this.tbx_AccountNumber_2       .Text, global.AccountLength                 );
                bool badWor = this.div_WorkOrder_2            .IsVisible && NoELTxt ( this.tbx_WorkOrder_2           .Text, global.WorkOrderLength               );
                bool badOMs = this.div_OldMeterSerialNumber_2 .IsVisible && NoELTxt ( this.tbx_OldMeterSerialNumber_2.Text, global.MeterNumberLength             );
                bool badMsn = this.div_MeterSerialNumber_2    .IsVisible && NoELTxt ( this.tbx_MeterSerialNumber_2   .Text, global.MeterNumberLength             );
                bool badOMr = this.div_OldMeterReading        .IsVisible && NoELNum ( this.tbx_OldMeterReading       .Text, this.tbx_OldMeterReading_2.MaxLength );
                bool badMre = this.div_MeterReadings_2        .IsVisible && NoEqNum ( this.tbx_MeterReading_2        .Text, this.tbx_MeterReading_2   .MaxLength );
                
                bool badOMw = this.div_OldMeterWorking_2      .IsVisible && this.pck_OldMeterWorking_2     .SelectedIndex <= -1;
                bool badRpc = this.div_ReplaceMeterRegister_2 .IsVisible && this.pck_ReplaceMeterRegister_2.SelectedIndex <= -1;
                bool badMty = this.divDyna_MeterType_Vendors_2.IsVisible && this.pck_MeterType_Names_2     .SelectedIndex <= -1;
                
                bool badDAc = global.AccountDualEntry      && this.div_AccountNumber_Dual_2       .IsVisible && NoEqNum ( this.tbx_AccountNumber_Dual_2       .Text, global.AccountLength     );
                bool badDWr = global.WorkOrderDualEntry    && this.div_WorkOrder_Dual_2           .IsVisible && NoELTxt ( this.tbx_WorkOrder_Dual_2           .Text, global.WorkOrderLength   );
                bool badDOs = global.OldSerialNumDualEntry && this.div_OldMeterSerialNumber_Dual_2.IsVisible && NoELTxt ( this.tbx_OldMeterSerialNumber_Dual_2.Text, global.MeterNumberLength );
                bool badDMs = global.NewSerialNumDualEntry && this.div_MeterSerialNumber_Dual_2   .IsVisible && NoELTxt ( this.tbx_MeterSerialNumber_Dual_2   .Text, global.MeterNumberLength );
                bool badDOr = global.OldReadingDualEntry   && this.div_OldMeterReading_Dual_2     .IsVisible && NoELNum ( this.tbx_OldMeterReading_Dual_2     .Text, this.tbx_OldMeterReading_Dual_2.MaxLength );
                bool badDMr = global.ReadingDualEntry      && this.div_MeterReading_Dual_2        .IsVisible && NoEqNum ( this.tbx_MeterReading_Dual_2        .Text, this.tbx_MeterReading_Dual_2   .MaxLength );
                
                FILL_ERROR = "Field 'Port2 _' is incorrectly filled";
                DUAL_ERROR_V = " ( Second entry )";
                
                if      ( ( badAcn &= ! noAcn ) ) msgError = this.lb_AccountNumber_2.Text;
                else if ( ( badDAc &= ! noDAc ) ) msgError = this.lb_AccountNumber_2.Text + DUAL_ERROR_V;
                else if ( ( badWor &= ! noWor ) ) msgError = this.lb_WorkOrder_2.Text;
                else if ( ( badDWr &= ! noDWr ) ) msgError = this.lb_WorkOrder_2.Text + DUAL_ERROR_V;
                else if ( ( badOMs &= ! noOMs ) ) msgError = this.lb_OldMeterSerialNumber_2.Text;
                else if ( ( badDOs &= ! noDOs ) ) msgError = this.lb_OldMeterSerialNumber_2.Text + DUAL_ERROR_V;
                else if ( ( badOMw &= ! noOMw ) ) msgError = this.lb_OldMeterWorking_2.Text;
                else if ( ( badOMr &= ! noOMr ) ) msgError = this.lb_OldMeterReading_2.Text;
                else if ( ( badDOr &= ! noDOr ) ) msgError = this.lb_OldMeterReading_2.Text + DUAL_ERROR_V;
                else if ( ( badRpc &= ! noRpc ) ) msgError = this.lb_ReplaceMeterRegister_2.Text;
                else if ( ( badMsn &= ! noMsn ) ) msgError = this.lb_MeterSerialNumber_2.Text;
                else if ( ( badDMs &= ! noDMs ) ) msgError = this.lb_MeterSerialNumber_2.Text + DUAL_ERROR_V;
                else if ( ( badMty &= ! noMty ) ) msgError = "Meter Type";
                else if ( ( badMre &= ! noMre ) ) msgError = this.lb_MeterReading_2.Text;
                else if ( ( badDMr &= ! noDMr ) ) msgError = this.lb_MeterReading_2.Text + DUAL_ERROR_V;
                
                if ( badAcn || badWor || badOMs || badOMw || badOMr ||
                        badRpc || badMsn || badMre || badMty || badDAc ||
                        badDWr || badDOs || badDOr || badDMs || badDMr )
                {
                    msgError = FILL_ERROR.Replace ( "_", msgError );
                    return false;
                }
                
                // Dual entries
                DUAL_ERROR_V = "Field 'Port2 _' dual entries are not the same";

                if (global.AccountDualEntry &&
                    ! string.Equals ( tbx_AccountNumber_2.Text, tbx_AccountNumber_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_AccountNumber_2.Text );
                    return false;
                }
                
                if (global.WorkOrderDualEntry &&
                    ! string.Equals ( tbx_WorkOrder_2.Text, tbx_WorkOrder_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_WorkOrder_2.Text );
                    return false;
                }
                
                if (global.OldSerialNumDualEntry &&
                    ! string.Equals ( tbx_OldMeterSerialNumber_2.Text, tbx_OldMeterSerialNumber_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_OldMeterSerialNumber_2.Text );
                    return false;
                }
                
                if (global.OldReadingDualEntry &&
                    ! string.Equals ( tbx_OldMeterReading_2.Text, tbx_OldMeterReading_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_OldMeterReading_2.Text );
                    return false;
                }
                
                if (global.NewSerialNumDualEntry &&
                    ! string.Equals ( tbx_MeterSerialNumber_2.Text, tbx_MeterSerialNumber_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_MeterSerialNumber_2.Text );
                    return false;
                }
    
                if (global.ReadingDualEntry &&
                    ! string.Equals ( tbx_MeterReading_2.Text, tbx_MeterReading_Dual_2.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_MeterReading_2.Text );
                    return false;
                }
            }
            
            #endregion

            #region RDD

            if ( mtu.Port1.IsSetFlow ||
                 mtu.TwoPorts && mtu.Port2.IsSetFlow )
            {
                // No mandatory fields can be empty
                // TRUE when the field does not need to be validated ( not mandatory and empty/not selected )
                bool noAcn = EmptyNoReq ( this.tbx_AccountNumber_V   .Text, MANDATORY_ACCOUNTNUMBER );
                bool noWor = EmptyNoReq ( this.tbx_WorkOrder_V       .Text, MANDATORY_WORKORDER );
                bool noFir = EmptyNoReq ( this.tbx_RDDFirmwareVersion.Text, MANDATORY_RDDFIRMWARE );
                // Dual entry
                bool noDAc = EmptyNoReq ( this.tbx_AccountNumber_Dual_V.Text, MANDATORY_ACCOUNTNUMBER );
                bool noDWr = EmptyNoReq ( this.tbx_WorkOrder_Dual_V    .Text, MANDATORY_WORKORDER );
                // Drop-down-lists
                bool noRDD = NoSelNoReq ( this.pck_MeterType_Names_V.SelectedIndex, MANDATORY_METERTYPE );
                bool noPos = NoSelNoReq ( this.pck_ValvePosition    .SelectedIndex, MANDATORY_RDDPOSITION );
                bool noTwo = true;
                bool noAlr = true;
                bool noDmd = true;
                // Validate only when the RDD is in port 1
                if ( mtu.Port1.IsSetFlow )
                {
                    noTwo = NoSelNoReq ( this.pck_TwoWay_V .SelectedIndex, MANDATORY_TWOWAY );
                    noAlr = NoSelNoReq ( this.pck_Alarms_V .SelectedIndex, MANDATORY_ALARMS );
                    noDmd = NoSelNoReq ( this.pck_Demands_V.SelectedIndex, MANDATORY_DEMANDS );
                }

                // Correct length
                // TRUE when the field has not correct length or is not selected yet
                bool badAcn =                                   NoEqNum ( this.tbx_AccountNumber_V   .Text, global.AccountLength );
                bool badWor = this.div_WorkOrder_V.IsVisible && NoELTxt ( this.tbx_WorkOrder_V       .Text, global.WorkOrderLength );
                bool badFir =                                   NoELTxt ( this.tbx_RDDFirmwareVersion.Text, RDDStatusResult.MAX_LENGTH_FIRMWARE );
                // Dual entry
                bool badDAc = global.AccountDualEntry   && this.div_AccountNumber_Dual_V.IsVisible && NoEqNum ( this.tbx_AccountNumber_Dual_V.Text, global.AccountLength );
                bool badDWr = global.WorkOrderDualEntry && this.div_WorkOrder_Dual_V    .IsVisible && NoELTxt ( this.tbx_WorkOrder_Dual_V    .Text, global.WorkOrderLength );
                // Drop-down-lists
                bool badRDD =                                 this.pck_MeterType_Names_V.SelectedIndex <= -1;
                bool badPos =                                 this.pck_ValvePosition    .SelectedIndex <= -1;
                bool badTwo = false;
                bool badAlr = false;
                bool badDmd = false;
                // Validate only when the RDD is in port 1
                if ( mtu.Port1.IsSetFlow )
                {
                    badTwo = this.div_TwoWay_V .IsVisible && this.pck_TwoWay_V .SelectedIndex <= -1;
                    badAlr = this.div_Alarms_V .IsVisible && this.pck_Alarms_V .SelectedIndex <= -1;
                    badDmd = this.div_Demands_V.IsVisible && this.pck_Demands_V.SelectedIndex <= -1;
                }

                FILL_ERROR = "Field 'RDD _' is incorrectly filled";
                DUAL_ERROR_V = " ( Second entry )";
                
                if      ( ( badAcn &= ! noAcn ) ) msgError = this.lb_AccountNumber_V.Text;
                else if ( ( badDAc &= ! noDAc ) ) msgError = this.lb_AccountNumber_V.Text + DUAL_ERROR_V;
                else if ( ( badWor &= ! noWor ) ) msgError = this.lb_WorkOrder_V.Text;
                else if ( ( badDWr &= ! noDWr ) ) msgError = this.lb_WorkOrder_V.Text + DUAL_ERROR_V;
                else if ( ( badRDD &= ! noRDD ) ) msgError = "RDD Type";
                else if ( ( badPos &= ! noPos ) ) msgError = this.lb_ValvePosition.Text;
                else if ( ( badFir &= ! noFir ) ) msgError = this.lb_RDDFirmwareVersion.Text;
                else if ( ( badTwo &= ! noTwo ) ) msgError = this.lb_TwoWay_V.Text;
                else if ( ( badAlr &= ! noAlr ) ) msgError = this.lb_Alarms_V.Text;
                else if ( ( badDmd &= ! noDmd ) ) msgError = this.lb_Demands_V.Text;

                if ( badAcn || badWor || badDAc ||
                     badDWr || badRDD || badPos || badFir ||
                     badTwo || badAlr || badDmd )
                {
                    msgError = FILL_ERROR.Replace ( "_", msgError );
                    return false;
                }

                // Dual entries
                DUAL_ERROR_V = "Field '_' dual entries are not the same";

                if ( global.AccountDualEntry &&
                    ! string.Equals ( tbx_AccountNumber_V.Text, tbx_AccountNumber_Dual_V.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_AccountNumber_V.Text );
                    return false;
                }
                
                if ( global.WorkOrderDualEntry &&
                    ! string.Equals ( tbx_WorkOrder_V.Text, tbx_WorkOrder_Dual_V.Text ) )
                {
                    msgError = DUAL_ERROR_V.Replace ( "_", this.lb_WorkOrder_V.Text );
                    return false;
                }
            }

            #endregion

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
                Utils.PrintDeep ( "CLICK!" );
            
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
            if ( !this.ValidateFields ( ref msgError ) )
            {
                DisplayAlert ( "Error", msgError, "OK" );
                return;
            }

           

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

            Mtu mtu = this.add_mtu.CurrentMtu;

            bool isReplaceMeter = ( this.actionType == ActionType.ReplaceMeter           ||
                                    this.actionType == ActionType.ReplaceMtuReplaceMeter ||
                                    this.actionType == ActionType.AddMtuReplaceMeter );

            bool rddIn1 = mtu.Port1.IsSetFlow;
            bool hasRDD = (  rddIn1  || mtu.TwoPorts && mtu.Port2.IsSetFlow );

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
            string value_omt = string.Empty; // Old MTU
            string value_rin = string.Empty; // Read Interval
            string value_sre = string.Empty; // Snap Reads / Daily Reads
            string value_two = string.Empty; // Two-Way
            Alarm  value_alr = null;         // Alarms
            Demand value_dmd = null;         // Demands

            // RDD
            string value_fir = string.Empty; // RDD Firmware
            string value_pos = string.Empty; // Valve Position
            Meter  value_rdd = null;         // RDD Type
            
            // GPS
            string value_lat;              // Latitude
            string value_lon;              // Longitude
            string value_alt;              // Altitude

            // Port 1
            if ( ! mtu.Port1.IsSetFlow )
            {
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
            }
                
            // Port 2
            if ( ( addMtuForm.UsePort2 = mtu.TwoPorts && this.port2IsActivated ) &&
                    ! mtu.Port2.IsSetFlow )
            {
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
                        value_omw_2 = this.pck_OldMeterWorking_2.SelectedItem.ToString ();
                        
                    if ( global.RegisterRecording )
                        value_rpl_2 = this.pck_ReplaceMeterRegister_2.SelectedItem.ToString ();
                }
            }
                
            // General fields, for the MTU itself
            // No RDD or RDD in port two
            if ( ! hasRDD ||
                 ! rddIn1 )
            {
                value_omt = this.tbx_OldMtuId.Text;
                value_rin = this.pck_ReadInterval.SelectedItem.ToString ();
                value_sre = this.sld_SnapReads   .Value.ToString ();

                // Is a two-way MTU
                if ( global.TimeToSync &&
                        mtu.TimeToSync    &&
                        mtu.FastMessageConfig )
                    value_two = this.pck_TwoWay.SelectedItem.ToString ();

                // Alarms dropdownlist is hidden when only has one option
                if (this.pck_Alarms.ItemsSource != null)
                {
                    if (this.pck_Alarms.ItemsSource.Count == 1)
                        value_alr = (Alarm)this.pck_Alarms.ItemsSource[0];
                    else if (this.pck_Alarms.ItemsSource.Count > 1)
                        value_alr = (Alarm)this.pck_Alarms.SelectedItem;
                }

                // Demands dropdownlist is hidden when only has one option
                if (this.pck_Demands.ItemsSource != null)
                {
                    if (this.pck_Demands.ItemsSource.Count == 1)
                        value_dmd = (Demand)this.pck_Demands.ItemsSource[0];
                    else if (this.pck_Demands.ItemsSource.Count > 1)
                        value_dmd = (Demand)this.pck_Demands.SelectedItem;
                }
            }
            // RDD in port 1
            else
            {
                // Is a two-way MTU
                if ( global.TimeToSync &&
                        mtu.TimeToSync &&
                        mtu.FastMessageConfig )
                    value_two = this.pck_TwoWay_V.SelectedItem.ToString ();
                    
                // Alarms dropdownlist is hidden when only has one option
                if ( this.pck_Alarms_V.ItemsSource.Count == 1 )
                    value_alr = ( Alarm )this.pck_Alarms_V.ItemsSource[ 0 ];
                else if ( this.pck_Alarms_V.ItemsSource.Count > 1 )
                    value_alr = ( Alarm )this.pck_Alarms_V.SelectedItem;
                    
                // Demands dropdownlist is hidden when only has one option
                if ( this.pck_Demands_V.ItemsSource.Count == 1 )
                    value_dmd = ( Demand )this.pck_Demands_V.ItemsSource[ 0 ];
                else if ( this.pck_Demands_V.ItemsSource.Count > 1 )
                    value_dmd = ( Demand )this.pck_Demands_V.SelectedItem;
            }

            // RDD
            if ( hasRDD )
            {
                if ( rddIn1 )
                {
                    value_acn = this.tbx_AccountNumber_V.Text;
                    value_wor = this.tbx_WorkOrder_V    .Text;
                }
                else
                {
                    value_acn_2 = this.tbx_AccountNumber_V.Text;
                    value_wor_2 = this.tbx_WorkOrder_V    .Text;
                }
                value_rdd = ( Meter )this.pck_MeterType_Names_V.SelectedItem;
                value_fir = this.tbx_RDDFirmwareVersion.Text;
                value_pos = ( string )this.pck_ValvePosition.SelectedItem;
            }
                
            // GPS
            value_lat = this.tbx_MtuGeolocationLat .Text;
            value_lon = this.tbx_MtuGeolocationLong.Text;
            value_alt = this.mtuGeolocationAlt;


            // Reset needed when same actions is launched more than one time ( Exception/error )
            this.addMtuForm.RemoveParameters ();

            #endregion

            #region Set parameters Port 1

            // No RDD
            if ( ! mtu.Port1.IsSetFlow )
            {
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

                // Snap Reads
                if ( global.AllowDailyReads &&
                     mtu.DailyReads &&
                     ! mtu.IsFamily33xx )
                    this.addMtuForm.AddParameter ( FIELD.SNAP_READS, value_sre );

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
            }
            // RDD
            else
            {
                // Account Number / Service Port ID
                this.addMtuForm.AddParameter ( FIELD.ACCOUNT_NUMBER, value_acn );

                // Work Order / Field Order
                if ( global.WorkOrderRecording )
                    this.addMtuForm.AddParameter ( FIELD.WORK_ORDER, value_wor );
                
                // Meter Type
                this.addMtuForm.AddParameter ( FIELD.METER_TYPE, value_rdd );
            }

            // General fields, also for RDD
            // Is a two-way MTU
            if ( global.TimeToSync &&
                 mtu.TimeToSync    &&
                 mtu.FastMessageConfig )
                this.addMtuForm.AddParameter ( FIELD.TWO_WAY, value_two );

            // Alarms
            if ( value_alr != null &&
                 mtu.RequiresAlarmProfile )
                this.addMtuForm.AddParameter ( FIELD.ALARM, value_alr );

            // Demands
            if ( value_dmd != null &&
                 mtu.MtuDemand &&
                 mtu.FastMessageConfig )
                this.addMtuForm.AddParameter ( FIELD.DEMAND, value_dmd );

            #endregion

            #region Set parameters Port 2

            if ( addMtuForm.UsePort2 )
            {
                // No RDD
                if ( ! mtu.Port2.IsSetFlow )
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
                // RDD
                else
                {
                    // Account Number / Service Port ID
                    this.addMtuForm.AddParameter ( FIELD.ACCOUNT_NUMBER_2, value_acn_2, 1 );

                    // Work Order / Field Order
                    if ( global.WorkOrderRecording )
                        this.addMtuForm.AddParameter ( FIELD.WORK_ORDER_2, value_wor_2, 1 );
                    
                    // Meter Type
                    this.addMtuForm.AddParameter ( FIELD.METER_TYPE_2, value_rdd, 1 );
                }
            }

            #endregion

            #region Set parameters RDD

            if ( hasRDD )
            {
                Data.SetTemp ( "RDDFirmware", value_fir );
                Data.SetTemp ( "RDDPosition", value_pos );
            }

            #endregion

            #region Set Optional parameters

            // Gps
            if ( ! string.IsNullOrEmpty ( value_lat ) &&
                 ! string.IsNullOrEmpty ( value_lon ) )
            {
                double lat = Convert.ToDouble ( value_lat );
                double lon = Convert.ToDouble ( value_lon );
                double alt = Convert.ToDouble ( ( ! string.IsNullOrEmpty ( value_alt ) ) ? value_alt : new string ( '1', 10 ) );

                this.addMtuForm.AddParameter ( FIELD.GPS_LATITUDE,  lat );
                this.addMtuForm.AddParameter ( FIELD.GPS_LONGITUDE, lon );
                this.addMtuForm.AddParameter ( FIELD.GPS_ALTITUDE,  alt );
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

            this.add_mtu.OnFinish -= OnFinish;
            this.add_mtu.OnFinish += OnFinish;

            this.add_mtu.OnError -= OnError;
            this.add_mtu.OnError += OnError;

            #endregion

            // TODO: Use Library.Data as first step to remove AddMtuForm from the system
            foreach ( KeyValuePair<string,Parameter> entry in addMtuForm.Dictionary )
                Data.SetTemp ( entry.Key, entry.Value.Value );

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
            isCancellable = true;

            // Get MtuType = MtuID
            foreach ( Parameter p in paramResult)
            {
                if ( ! string.IsNullOrEmpty ( p.CustomParameter ) &&
                     p.CustomParameter.Equals ( "MtuType" ) )
                    mtu_type = Int32.Parse(p.Value.ToString());
            }

            Mtu mtu = Singleton.Get.Configuration.GetMtuTypeById ( mtu_type );

            string ndresult = string.Empty;
            InterfaceParameters[] interfacesParams = Singleton.Get.Configuration.getUserParamsFromInterface ( mtu, ActionType.ReadMtu );
            
            Mtu currentMtu = Singleton.Get.Action.CurrentMtu;

            // NOTE: Special case when is one port MTU for valve/RDD
            // NOTE: Create temporal MTU instace because if the final log fails,
            // NOTE: the MTU info should continue with the correct/real number of ports
            Mtu copyCurrentMtu = currentMtu.SimulateRddInPortTwoIfNeeded () as Mtu;

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
                                else description = copyCurrentMtu.Ports[i].GetProperty ( pParameter.Name );
                                
                                FinalReadListView.Add(new ReadMTUItem()
                                {
                                    Title       = "Here lies the Port title...",
                                    isDisplayed = "true",
                                    Height      = "40",
                                    isMTU       = "false",
                                    isMeter     = "true",
                                    Description = "Port " + ( i + 1 ) + ": " + description
                                });
                            }
                            // Port fields
                            else
                            {
                                if ( param != null )
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

                // Root log fields
                else
                {
                    Parameter param = args.Result.getParameterByTag ( iParameter.Name, iParameter.Source, 0 );
                    
                    if (param != null)
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
            }

            await Task.Delay(100).ContinueWith(t =>
            Device.BeginInvokeOnMainThread(() =>
            {
                if (PicturesMTU.Count != 0)
                {
                    bottomBar.GetLabelElement("label_read").Text = "Saving pictures...";
                    CopyPicturesToUserImagesFolder();
                }

                if ( ! string.IsNullOrEmpty ( ndresult ) )
                {
                    Image imgNdResult     = bottomBar.GetImageElement ( "img_ndresult" );
                    imgNdResult.Source    = "nd_" + ndresult;
                    imgNdResult.IsVisible = true;
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
            isCancellable = false;
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
            catch (FeatureNotSupportedException )
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException )
            {
               return null; // Handle not enabled on device exception
            }
            catch (PermissionException )
            {
                // Handle permission exception
            }
            catch (Exception )
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
            catch ( Exception ex ) when ( Data.SaveIfDotNetAndContinue ( ex ) )
            {
                await Errors.ShowAlert ( new CameraException () );
            }
        }

        private async void TakePicture(object sender, EventArgs e)
        { 
            try
            {

                string port; 

                int mtuIdLength = Singleton.Get.Configuration.Global.MtuIdLength;
                var MtuId = await Data.Get.MemoryMap.MtuSerialNumber.GetValue();
                
                string sTick = DateTime.Now.Ticks.ToString();

                if (hasMeterPortTwo)
                {
                    bool bResp = await DisplayAlert("Select port", "Select the port for the picture", "Port 1", "Port 2");
                    port = bResp == true ? "1" : "2";
                }
                else
                    port = "1";

                string AccFieldName = port == "1" ? "tbx_AccountNumber" : $"tbx_AccountNumber_{port}";
                BorderlessEntry field = (BorderlessEntry)this.FindByName(AccFieldName);

                string nameFile = MtuId.ToString().PadLeft(mtuIdLength, '0')+"_"+ field.Text + "_" + sTick + "_Port"+ port;
                
                Device.BeginInvokeOnMainThread(async () =>
                {
                    MediaFile file = await PictureService.TakePictureService(nameFile);
                   
                    if (file == null)
                        return;
                   

                    string[] fileName = file.Path.Split('/');
                    nameFile = fileName[fileName.Length-1];
                    DirectoryInfo dir = new DirectoryInfo(file.Path.Substring(0,file.Path.Length-(nameFile.Length+1)));
                    
                    FileInfo[]  imagefiles = dir.GetFiles(nameFile);
                 
                    imagefiles[0].CopyTo(Path.Combine(Mobile.ImagesPath, nameFile));
                    imagefiles[0].Delete();
 
                    file.Dispose();
                });
            }
            catch ( Exception ex ) when ( Data.SaveIfDotNetAndContinue ( ex ) )
            {
                await Errors.ShowAlert ( new CameraException () );
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
