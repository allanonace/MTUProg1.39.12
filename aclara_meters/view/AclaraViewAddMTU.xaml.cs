using aclara_meters.Behaviors;
using aclara_meters.Helpers;
using aclara_meters.Models;
using Acr.UserDialogs;
using MTUComm;
using MTUComm.actions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xml;

using FIELD = MTUComm.actions.AddMtuForm.FIELD;

/*
Optional 1  WorkOrderRecording      Hides/shows Field Order
Optional 2  MeterWorkRecording      Shows option to specify if meter is broken when replacing it
Optional 3  ApptScreen              —— —— —— —— —— —— —— ——
Optional 4  AccountDualEntry        Enable popup to repeat Service Pt. ID
Optional 5  WorkOrderDualEntry      Enables popup to repeat Field Order
Optional 6  OldSerialNumDualEntry   Enables popup to repeat Old meter #
Optional 7  NewSerialNumDualEntry   Enable popup to repeat New meter #
Optional 8  ReadingDualEntry        —— —— —— —— —— —— —— ——
Optional 9  OldReadingDualEntry     Enable popup to repeat Old Reading
Optional 10 ReverseReading          All Reading Inputs reversed
Optional 11 IndividualReadInterval  If false, disables Read Interval field (it is visible but readonly)
Optional 12 RegisterRecording       Shows Replace Meter/Register field, with Meter/Register/Both list
Optional 13 UseMeterSerialNumber    Shows/hides different meter # fields (Old, New)
Optional 14 OldReadingRecording     Shows/hides Old Reading field
Optional 15 ShowAddMtu              Disables Add Mtu button (visible, but not clickable)
Optional 16 ShowAddMtuReplaceMeter  Disables Add MTU/Replace Meter button (visible, but not clickable)
Optional 17 ShowAddMtuMeter         Disables Add MTU/Add Meter button (visible, but not clickable)
Optional 18 ShowReplaceMtu          Disables Replace MTU button (visible, but not clickable)
Optional 19 ShowReplaceMtuMeter     Disables Replace MTU/Replace Meter button (visible, but not clickable)
Optional 20 ShowReplaceMeter        Disables Replace Meter button (visible, but not clickable)
*/

namespace aclara_meters.view
{
    public partial class AclaraViewAddMTU
    {
        #region Constants

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

        private enum Names
        {
            Name1 = 0
        };

        public const string TWOWAY_FAST     = "Fast";
        public const string TWOWAY_SLOW     = "Slow";

        public const string CANCEL_COMPLETE = "Complete";
        public const string CANCEL_CANCEL   = "Cancel";
        public const string CANCEL_SKIP     = "Skip";
        public const string CANCEL_NOTHOME  = "Not Home";
        public const string CANCEL_OTHER    = "Other";

        #endregion

        #region GUI Elements

        private List<PageItem> MenuList;

        private IUserDialogs dialogsSaved;
        private bool _userTapped;

        private double snapReadsStep;
        private double snapReads2Step;

        private StackLayout meterVendorsContainerA;
        private StackLayout meterModelsContainerA;
        private StackLayout meterNamesContainerA;
        private StackLayout meterVendors2ContainerA;
        private StackLayout meterModels2ContainerA;
        private StackLayout meterNames2ContainerA;

        private BorderlessPicker meterVendorsPicker;
        private BorderlessPicker meterModelsPicker;
        private BorderlessPicker meterNamesPicker;
        private BorderlessPicker meterVendors2Picker;
        private BorderlessPicker meterModels2Picker;
        private BorderlessPicker meterNames2Picker;

        private List<string> meterVendorsList;
        private List<string> meterModelsList;
        private List<string> meterNamesList;
        private List<string> meterVendors2List;
        private List<string> meterModels2List;
        private List<string> meterNames2List;

        private List<BorderlessPicker> optionalPickers;
        private List<BorderlessEntry> optionalEntries;

        private MeterTypes meterTypes;
        private List<Xml.Meter> meters;
        private List<Xml.Meter> meters2;
        private List<string> vendors;
        private List<string> vendors2;
        private List<Meter> names;
        private List<Meter> names2;
        private string vendor;
        private string vendor2;
        private string model;
        private string model2;
        private string name;
        private string name2;

        private Slider MeterSnapReadsPort1Slider;
        private BorderlessPicker MeterTwoWayPort1Picker;
        private BorderlessPicker MeterAlarmSettingsPort1Picker;

        private List<Alarm> alarmsList = new List<Alarm>();
        private List<Alarm> alarms2List = new List<Alarm>();

        private List<Demand> demandsList = new List<Demand>();
        private List<Demand> demands2List = new List<Demand>();

        #endregion

        #region Attributes

        private MTUComm.Action add_mtu;
        private Global globals;
        private MtuTypes mtuData;
        private Mtu currentMtu;
        private AddMtuForm addMtuForm;
        private int detectedMtuType;
        private Configuration config;

        private List<ReadMTUItem> FinalReadListView { get; set; }

        private bool port2enabled;
        private bool isCancellable;
        private bool snapRead1Status = false;
        private bool snapRead2Status = false;
        private bool port2status;

        // Conditions - Globals
        private bool WorkOrderRecording;
        private bool MeterWorkRecording;
        private bool ApptScreen;
        private bool AccountDualEntry;
        private bool WorkOrderDualEntry;
        private bool OldSerialNumDualEntry;
        private bool NewSerialNumDualEntry;
        private bool ReadingDualEntry;
        private bool OldReadingDualEntry;
        private bool ReverseReading;
        private bool IndividualReadInterval;
        private bool RegisterRecording;
        private bool UseMeterSerialNumber;
        private bool OldReadingRecording;
        private bool ShowAddMtu;
        private bool ShowAddMtuReplaceMeter;
        private bool ShowAddMtuMeter;
        private bool ShowReplaceMtu;
        private bool ShowReplaceMtuMeter;
        private bool ShowReplaceMeter;

        // Conditions - Register Snap reads
        private bool mtuDailyReads;
        private bool globalsAllowDailyReads;
        private bool setGlobalSnap;
        private bool globalsChangeDailyReads;
        private double globalsDailyReadsDefault;
        private double dailyReadsMemoryMapValue;

        // Conditions - Register 2-Way [ Encoders ]
        private bool mtuFastMessageConfig;
        private bool globalsFastMessageConfig;

        // Conditions - Alarms
        private bool mtuRequiresAlarmProfile;

        #endregion

        #region Initialization

        public AclaraViewAddMTU ()
        {
            InitializeComponent ();
        }

        public AclaraViewAddMTU ( IUserDialogs dialogs )
        {
            InitializeComponent();

            this.add_mtu = new MTUComm.Action(
                config: FormsApp.config,
                serial: FormsApp.ble_interface,
                actiontype: MTUComm.Action.ActionType.AddMtu,
                user: FormsApp.CredentialsService.UserName);

            #region Prepare mtuForm

            this.config = Configuration.GetInstance();

            /* Get detected mtu */
            MTUBasicInfo mtuBasicInfo = MtuForm.mtuBasicInfo;
            this.detectedMtuType = (int)mtuBasicInfo.Type;
            currentMtu = this.config.mtuTypes.FindByMtuId(this.detectedMtuType);
            /* Instantiate form */
            addMtuForm = new AddMtuForm(currentMtu);

            #endregion

            #region Conditions

            // Two Ports
            addMtuForm.conditions.mtu.AddCondition("TwoPorts");

            // Field order (work order)
            addMtuForm.conditions.globals.AddCondition("WorkOrderRecording");
            
            // Meter Number (serial number)
            addMtuForm.conditions.globals.AddCondition("UseMeterSerialNumber");

            // Vendor / Model / Name
            addMtuForm.conditions.globals.AddCondition("ShowMeterVendor");

            // Read Interval 
            addMtuForm.conditions.globals.AddCondition("IndividualReadInterval");

            // Snap Reads
            addMtuForm.conditions.globals.AddCondition("AllowDailyReads");
            addMtuForm.conditions.globals.AddCondition("IndividualDailyReads");
            addMtuForm.conditions.mtu.AddCondition("DailyReads");

            // 2-Way
            addMtuForm.conditions.globals.AddCondition("FastMessageConfig");
            addMtuForm.conditions.globals.AddCondition("Fast2Way");
            addMtuForm.conditions.mtu.AddCondition("FastMessageConfig");

            // Alarms
            addMtuForm.conditions.mtu.AddCondition("RequiresAlarmProfile");

            // Demands
            addMtuForm.conditions.mtu.AddCondition("MtuDemand");

            isCancellable = false;

            #endregion

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    label_read.Opacity = 1;
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

            NavigationPage.SetHasNavigationBar(this, false); //Turn off the Navigation bar

            Device.BeginInvokeOnMainThread(() =>
            {
                label_read.Opacity = 1;
            });

            _userTapped = false;

            //Initialize Tap/Clickable element listeners
            TappedListeners();

            //Change username textview to Prefs. String
            if (FormsApp.CredentialsService.UserName != null)
            {
                userName.Text = FormsApp.CredentialsService.UserName; //"Kartik";
            }
            
            battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
            rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

            InitializeLowerbarLabel();

            InitializeAddMtuForm();

            RegisterEventHandlers();

            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;
            submit_dialog.Clicked += submit_send;

            listaMTUread.IsVisible = false;

            Task.Delay(10).ContinueWith(t =>
              Device.BeginInvokeOnMainThread(() =>
              {
                  backdark_bg.IsVisible = true;
                  indicator.IsVisible = true;
                  background_scan_page.IsEnabled = false;


                  Task.Delay(100).ContinueWith(t0 =>
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        label_read.Opacity = 1;
                        backdark_bg.IsVisible = false;
                        indicator.IsVisible = false;
                        background_scan_page.IsEnabled = true;
                        label_read.Text = "Press Button to Start";

                        #region Port 2 Buttons Listener

                        //Task.Factory.StartNew(SetPort2Buttons);
                        

                    #endregion

                    #region Snap Read CheckBox Controller

                        CheckBoxController();

                    #endregion

                    }));
              }
            ));
        }

        #region Checkbox Controller

        private void CheckBoxController()
        {

            #region Port 1 
            snapReadsContainer.IsVisible = true;
            snapReadsContainer.IsEnabled = true;
            snapReadsSubContainer.IsEnabled = true;
            snapReadsSlider.IsEnabled = false;
            snapReadsSubContainer.Opacity = 0.8;
            Checkbox_SnapReads_Port1.Source = "checkbox_off";
            #endregion

            #region Port 2
            snapReads2Container.IsVisible = true;
            snapReads2Container.IsEnabled = true;
            snapReads2SubContainer.IsEnabled = true;
            snapReads2Slider.IsEnabled = false;
            snapReads2SubContainer.Opacity = 0.8;
            Checkbox_SnapReads_Port2.Source = "checkbox_off";
            #endregion

            #region Start Slider Logic ? Should come from globals...

            int snapReadsDefault = 10;
            int snapReadsFromMem = 6;

            snapReadsStep = 1.0;
            snapReadsSlider.ValueChanged += OnSnapReadsSliderValueChanged;

            if (snapReadsDefault > -1)
            {
                snapReadsSlider.Value = snapReadsDefault;
            }
            else
            {
                snapReadsSlider.Value = snapReadsFromMem;
            }

      
            snapReads2Step = 1.0;
            snapReads2Slider.ValueChanged += OnSnapReads2SliderValueChanged;

            if (snapReadsDefault > -1)
            {
                snapReads2Slider.Value = snapReadsDefault;
            }
            else
            {
                snapReads2Slider.Value = snapReadsFromMem;
            }


            #endregion


            #region  Here lies the logic...

            Checkbox_SnapReads_Port1.GestureRecognizers.Add(new TapGestureRecognizer
             {
                 Command = new Command(() =>
                 {
                    if (!snapRead1Status)
                     {
                         Checkbox_SnapReads_Port1.Source = "checkbox_on";
                         snapReadsSlider.IsEnabled = true;
                         snapReadsSubContainer.Opacity = 1;
                         snapRead1Status = true;
                     }
                     else
                     {
                         Checkbox_SnapReads_Port1.Source = "checkbox_off";
                         snapReadsSlider.IsEnabled = false;
                         snapReadsSubContainer.Opacity = 0.8;
                         snapRead1Status = false;
                     }
                 }),
             });


            Checkbox_SnapReads_Port2.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    this.ChangeCheckboxSnapReads ( ! snapRead2Status );
                }),
            });

            #endregion

        }

        

        #endregion

        private void SetPort2Buttons ()
        {
            // Port2 form starts visible or hidden depends on bit 1 of byte 28
            this.port2status = this.add_mtu.comm.ReadMtuBit ( 28, 1 );


            Device.BeginInvokeOnMainThread(() =>
            {
                // Switch On/Off port2 form tab
                enablePort2.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        this.port2status = !this.port2status;

                        bool ok = this.add_mtu.comm.WriteMtuBitAndVerify(28, 1, port2status);
                        Console.WriteLine("-> UPDATE PORT 2 STATUS: " + ok + " " + this.port2status);

                        // Bit correctly modified
                        if (ok)
                            this.UpdateStatusPort2();

                        // Bit have not changed -> return to previous state
                        else
                            this.port2status = !this.port2status;
                    }),
                });

                // Copy current values of port1 form controls to port2 form controls
                copyPort1.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(() =>
                    {
                        servicePortId2Input.Text = servicePortIdInput.Text;
                        fieldOrder2Input.Text = fieldOrderInput.Text;
                        meterSerialNumber2Input.Text = meterSerialNumberInput.Text;
                        initialReading2Input.Text = initialReadingInput.Text;
                        readInterval2Picker.SelectedIndex = readIntervalPicker.SelectedIndex;
                        meterNames2Picker.SelectedIndex = meterNamesPicker.SelectedIndex;
                        snapReads2Slider.Value = snapReadsSlider.Value;
                        twoWay2Picker.SelectedIndex = twoWayPicker.SelectedIndex;
                        alarms2Picker.SelectedIndex = alarmsPicker.SelectedIndex;
                        demands2Picker.SelectedIndex = demandsPicker.SelectedIndex;

                        this.ChangeCheckboxSnapReads(this.snapRead1Status);
                    }),
                });

                this.UpdateStatusPort2();

            });
           


        }

        private void InitializeAddMtuForm ()
        {
            #region Conditions

            dynamic MtuConditions     = addMtuForm.conditions.mtu;
            dynamic GlobalsConditions = addMtuForm.conditions.globals;

            #endregion

            #region Two ports

            bool hasTwoPorts = MtuConditions.TwoPorts;
            port2label.IsVisible = hasTwoPorts;

            #endregion

            #region Field Order ( Work Order )

            bool WorkOrderRecording = GlobalsConditions.WorkOrderRecording;

            fieldOrderContainer .IsVisible = WorkOrderRecording;
            fieldOrderContainer .IsEnabled = WorkOrderRecording;
            fieldOrder2Container.IsVisible = hasTwoPorts && WorkOrderRecording;
            fieldOrder2Container.IsEnabled = hasTwoPorts && WorkOrderRecording;

            #endregion

            #region Meter Serial Number

            bool UseMeterSerialNumber = GlobalsConditions.UseMeterSerialNumber;

            meterSerialNumberContainer .IsVisible = UseMeterSerialNumber;
            meterSerialNumberContainer .IsEnabled = UseMeterSerialNumber;
            meterSerialNumber2Container.IsVisible = hasTwoPorts && UseMeterSerialNumber;
            meterSerialNumber2Container.IsEnabled = hasTwoPorts && UseMeterSerialNumber;

            #endregion

            // TODO
            #region Meter Vendor / Model / Name

            this.meters = this.config.meterTypes.FindByPortTypeAndFlow(currentMtu.Ports[0].Type, currentMtu.Flow);
            InitializeMeterPickers();

            if ( hasTwoPorts )
            {
                this.meters2 = this.config.meterTypes.FindByPortTypeAndFlow(currentMtu.Ports[1].Type, currentMtu.Flow);
                InitializeMeter2Pickers();
            }

            bool ShowMeterVendor = GlobalsConditions.ShowMeterVendor;
            if (ShowMeterVendor)
            {
                // TODO: group meters by vendor / model / name
            }
            else
            {
                // TODO: display meter list directly, by  name
            }

            #endregion

            #region Read Interval

            List<string> readIntervalList = new List<string>()
            {
                "24 Hours",
                "12 Hours",
                "6 Hours",
                "4 Hours",
                "3 Hours",
                "2 Hours",
                "1 Hour",
                "30 Min",
                "20 Min",
                "15 Min",
            };

            bool IndividualReadInterval = GlobalsConditions.IndividualReadInterval;

            readIntervalContainer .IsVisible   = IndividualReadInterval;
            readIntervalContainer .IsEnabled   = IndividualReadInterval;
            readInterval2Container.IsVisible   = hasTwoPorts && IndividualReadInterval;
            readInterval2Container.IsEnabled   = hasTwoPorts && IndividualReadInterval;
            readIntervalPicker    .ItemsSource = readIntervalList;
            readInterval2Picker   .ItemsSource = readIntervalList;

            #endregion

            // TODO: get snap reads value from memory map
            #region Snap Reads

            bool allowSnapReads      = GlobalsConditions.AllowDailyReads;
            bool snapReads           = MtuConditions.DailyReads;
            bool snapReadActive      = allowSnapReads && snapReads;
            bool changeableSnapReads = GlobalsConditions.IndividualDailyReads;
            int  snapReadsDefault    = this.config.global.DailyReadsDefault;

            this.snapReadsContainer .IsEnabled = snapReadActive;
            this.snapReadsContainer .IsVisible = snapReadActive;
            this.snapReads2Container.IsEnabled = hasTwoPorts && snapReadActive;
            this.snapReads2Container.IsVisible = hasTwoPorts && snapReadActive;

            this.snapReadsSubContainer .IsEnabled = changeableSnapReads && snapReadActive;
            this.snapReads2SubContainer.IsEnabled = hasTwoPorts && changeableSnapReads && snapReadActive;
            this.snapReadsSubContainer .Opacity   = ( changeableSnapReads && snapReadActive ) ? 1 : 0.8d;
            this.snapReads2SubContainer.Opacity   = ( hasTwoPorts && changeableSnapReads && snapReadActive ) ? 1 : 0.8d;

            this.snapReadsStep  = 1.0;
            this.snapReads2Step = 1.0;

            if ( snapReadActive )
                this.snapReadsSlider.ValueChanged += OnSnapReadsSliderValueChanged;

            if ( hasTwoPorts && snapReadActive )
                this.snapReads2Slider.ValueChanged += OnSnapReads2SliderValueChanged;

            this.snapReadsSlider .Value = ( snapReadsDefault > -1 ) ? snapReadsDefault : 6;
            this.snapReads2Slider.Value = ( snapReadsDefault > -1 ) ? snapReadsDefault : 6;

            #endregion

            #region 2-Way

            bool GlobalsFastMessageConfig = GlobalsConditions.FastMessageConfig; 
            bool GlobalsFast2Way          = GlobalsConditions.Fast2Way;
            bool MtuFastMessageConfig     = MtuConditions.FastMessageConfig;

            List<string> twoWayList = new List<string> ()
            {
                TWOWAY_FAST,
                TWOWAY_SLOW,
            };
            
            twoWayContainer .IsVisible     = MtuFastMessageConfig;
            twoWayContainer .IsEnabled     = MtuFastMessageConfig;
            twoWay2Container.IsVisible     = hasTwoPorts && MtuFastMessageConfig;
            twoWay2Container.IsEnabled     = hasTwoPorts && MtuFastMessageConfig;
            twoWayPicker    .ItemsSource   = twoWayList;
            twoWay2Picker   .ItemsSource   = twoWayList;
            twoWayPicker    .SelectedIndex = ( GlobalsFastMessageConfig || GlobalsFast2Way ) ? 0 : 1;
            twoWay2Picker   .SelectedIndex = ( GlobalsFastMessageConfig || GlobalsFast2Way ) ? 0 : 1;

            #endregion

            #region Alarms

            alarmsList  = config.alarms.FindByMtuType ( this.detectedMtuType );
            alarms2List = ( hasTwoPorts ) ? config.alarms.FindByMtuType ( this.detectedMtuType ) : new List<Alarm> ();

            bool RequiresAlarmProfile = MtuConditions.RequiresAlarmProfile;
            bool portHasSomeAlarm     = ( RequiresAlarmProfile && alarmsList.Count > 0 );
            bool port2HasSomeAlarm    = ( hasTwoPorts && RequiresAlarmProfile && alarms2List.Count > 0 );

            alarmsPicker .ItemDisplayBinding = new Binding ( "Name" );
            alarms2Picker.ItemDisplayBinding = new Binding ( "Name" );

            alarmsContainer .IsEnabled   = portHasSomeAlarm;
            alarmsContainer .IsVisible   = portHasSomeAlarm;
            alarms2Container.IsEnabled   = port2HasSomeAlarm;
            alarms2Container.IsVisible   = port2HasSomeAlarm;
            alarmsPicker    .ItemsSource = alarmsList;
            alarms2Picker   .ItemsSource = alarms2List;

            #endregion

            #region Demands

            demandsList  = config.demands.FindByMtuType ( this.detectedMtuType );
            demands2List = ( hasTwoPorts ) ? config.demands.FindByMtuType ( this.detectedMtuType ) : new List<Demand> ();

            bool MtuDemand          = MtuConditions.MtuDemand;
            bool portHasSomeDemand  = ( MtuDemand && demandsList.Count > 0 );
            bool port2HasSomeDemand = ( hasTwoPorts && MtuDemand && demands2List.Count > 0 );

            demandsPicker .ItemDisplayBinding = new Binding ( "Name" );
            demands2Picker.ItemDisplayBinding = new Binding ( "Name" );

            demandsContainer .IsEnabled   = portHasSomeDemand;
            demandsContainer .IsVisible   = portHasSomeDemand;
            demands2Container.IsEnabled   = port2HasSomeDemand;
            demands2Container.IsVisible   = port2HasSomeDemand;
            demandsPicker    .ItemsSource = demandsList;
            demands2Picker   .ItemsSource = demands2List;

            #endregion

            #region Misc

            InitializeOptionalFields ();

            #endregion

            #region Cancel reason

            // Load cancel reasons from Global.xml
            List<string> cancelReasons = this.config.global.Cancel;

            // Add "Other" option
            cancelReasons.Add("Other");

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
                    TargetType = "AddMTUAddMeter"
                },

                new PageItem()
                {
                    Title = "Add MTU / Rep. Meter",
                    Icon = "addMTUrepmeter.png",
                    TargetType = "AddMTUReplaceMeter"
                },

                new PageItem()
                {
                    Title = "Rep.MTU / Rep. Meter",
                    Icon = "repMTUrepmeter.png",
                    TargetType = "ReplaceMTUReplaceMeter"
                },

                new PageItem()
                {
                    Title = "Install Confirmation",
                    Icon = "installConfirm.png",
                    TargetType = "InstallConfirm"
                }
            };

            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = MenuList;
        }

        private void InitializeOptionalFields()
        {
            List<Option> optionalFields = this.config.global.Options;

            optionalPickers = new List<BorderlessPicker>();
            optionalEntries = new List<BorderlessEntry>();

            foreach (Option optionalField in optionalFields)
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

                if (optionalField.Type.Equals("list"))
                {
                    List<string> optionalFieldOptions = optionalField.OptionList;
                    BorderlessPicker optionalPicker = new BorderlessPicker()
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HeightRequest = 40,
                        FontSize = 17,
                        ItemsSource = optionalFieldOptions
                    };
                    optionalPicker.Name = optionalField.Name.Replace(" ", "_");
                    optionalPicker.Display = optionalField.Display;

                    optionalPicker.SelectedIndexChanged += GenericPicker_SelectedIndexChanged;


                    optionalPickers.Add(optionalPicker);

                    optionalContainerD.Children.Add(optionalPicker);
                    optionalContainerC.Content = optionalContainerD;
                    optionalContainerB.Content = optionalContainerC;
                    optionalContainerA.Children.Add(optionalLabel);
                    optionalContainerA.Children.Add(optionalContainerB);

                    this.optionalFields.Children.Add(optionalContainerA);
                }
                else if (optionalField.Type.Equals("text"))
                {
                    string format = optionalField.Format;
                    int maxLen = optionalField.Len;
                    int minLen = optionalField.MinLen;
                    bool required = optionalField.Required;

                    Keyboard keyboard = Keyboard.Default;

                    try
                    {
                        if (format.Equals("alpha"))
                        {
                            keyboard = Keyboard.Default;
                        }
                        else if (format.Equals("alphanumeric"))
                        {
                            keyboard = Keyboard.Numeric;
                        }
                        else if (format.Equals("date"))
                        {
                            keyboard = Keyboard.Default;
                        }
                        else if (format.Equals("time"))
                        {
                            keyboard = Keyboard.Numeric;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }

                    BorderlessEntry optionalEntry = new BorderlessEntry()
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
                else
                {
                    // do nothing
                }
            }
        }

        private void RegisterEventHandlers()
        {
            if (AccountDualEntry)
            {
                servicePortIdInput.Unfocused += (s, e) => { ServicePortId_validate(1); };
                servicePortId2Input.Unfocused += (s, e) => { ServicePortId_validate(2); };
                servicePortId_ok.Tapped += ServicePortId_Ok_Tapped;
                servicePortId_cancel.Tapped += ServicePortId_Cancel_Tapped;
            }

            if (WorkOrderDualEntry)
            {
                fieldOrderInput.Unfocused += (s, e) => { FieldOrder_validate(1); };
                fieldOrder2Input.Unfocused += (s, e) => { FieldOrder_validate(2); };
                fieldOrder_ok.Tapped += FieldOrder_Ok_Tapped;
                fieldOrder_cancel.Tapped += FieldOrder_Cancel_Tapped;
            }
        }

        private void TappedListeners()
        {
            logout_button.Tapped += LogoutCallAsync;
            settings_button.Tapped += OpenSettingsCallAsync;
            back_button.Tapped += ReturnToMainView;
            bg_read_mtu_button.Tapped += AddMtu;
            turnoffmtu_ok.Tapped += TurnOffMTUOkTapped;
            turnoffmtu_no.Tapped += TurnOffMTUNoTapped;
            turnoffmtu_ok_close.Tapped += TurnOffMTUCloseTapped;
            replacemeter_ok.Tapped += ReplaceMeterOkTapped;
            replacemeter_cancel.Tapped += ReplaceMeterCancelTapped;
            meter_ok.Tapped += MeterOkTapped;
            meter_cancel.Tapped += MeterCancelTapped;

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

            gps_icon_button.Tapped += GPSUpdateButton;

        }

        private void InitializeLowerbarLabel()
        {
            label_read.Text = "Push Button to START";
        }

        private void InitializeMeterPickers()
        {
            meterVendorsList = this.config.meterTypes.GetVendorsFromMeters(meters);

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

            meterVendorsPicker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = meterVendorsList
            };

            meterVendorsPicker.SelectedIndexChanged += MeterVendorsPicker_SelectedIndexChanged;

            meterVendorsContainerA = new StackLayout()
            {
                StyleId = "bloque" + 1
            };

            Label meterVendorsLabel = new Label()
            {
                Text = "Vendor",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            meterVendorsContainerD.Children.Add(meterVendorsPicker);
            meterVendorsContainerC.Content = meterVendorsContainerD;
            meterVendorsContainerB.Content = meterVendorsContainerC;
            meterVendorsContainerA.Children.Add(meterVendorsLabel);
            meterVendorsContainerA.Children.Add(meterVendorsContainerB);

            meterModelsList = new List<string>();

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

            meterModelsPicker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = meterModelsList,
                StyleId = "pickerModelos"
            };

            meterModelsPicker.SelectedIndexChanged += MeterModelsPicker_SelectedIndexChanged;

            meterModelsContainerA = new StackLayout()
            {
                StyleId = "bloque" + 2
            };

            Label meterModelsLabel = new Label()
            {
                Text = "Model",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            meterModelsContainerD.Children.Add(meterModelsPicker);
            meterModelsContainerC.Content = meterModelsContainerD;
            meterModelsContainerB.Content = meterModelsContainerC;
            meterModelsContainerA.Children.Add(meterModelsLabel);
            meterModelsContainerA.Children.Add(meterModelsContainerB);

            meterNamesList = new List<string>();

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

            meterNamesPicker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = meterNamesList,
                StyleId = "pickerName"
            };

            meterNamesPicker.SelectedIndexChanged += MeterNamesPicker_SelectedIndexChanged;

            meterNamesContainerA = new StackLayout()
            {
                StyleId = "bloque" + 3
            };

            Label meterNamesLabel = new Label()
            {
                Text = "Meter Type",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            meterNamesContainerD.Children.Add(meterNamesPicker);
            meterNamesContainerC.Content = meterNamesContainerD;
            meterNamesContainerB.Content = meterNamesContainerC;
            meterNamesContainerA.Children.Add(meterNamesLabel);
            meterNamesContainerA.Children.Add(meterNamesContainerB);

            meterVendorsModelsNamesContainer.Children.Add(meterVendorsContainerA);
            meterVendorsModelsNamesContainer.Children.Add(meterModelsContainerA);
            meterVendorsModelsNamesContainer.Children.Add(meterNamesContainerA);

            meterNamesContainerA.IsVisible = false;
            meterModelsContainerA.IsVisible = false;
        }

        private void InitializeMeter2Pickers()
        {
            meterVendors2List = this.config.meterTypes.GetVendorsFromMeters(meters2);

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

            meterVendors2Picker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = meterVendors2List
            };

            meterVendors2Picker.SelectedIndexChanged += MeterVendors2Picker_SelectedIndexChanged2;

            meterVendors2ContainerA = new StackLayout()
            {
                StyleId = "bloque" + 1
            };

            Label meterVendors2Label = new Label()
            {
                Text = "Vendor",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            meterVendors2ContainerD.Children.Add(meterVendors2Picker);
            meterVendors2ContainerC.Content = meterVendors2ContainerD;
            meterVendors2ContainerB.Content = meterVendors2ContainerC;
            meterVendors2ContainerA.Children.Add(meterVendors2Label);
            meterVendors2ContainerA.Children.Add(meterVendors2ContainerB);

            meterModels2List = new List<string>();

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
            
            meterModels2Picker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = meterModels2List,
                StyleId = "pickerModelos2"
            };

            meterModels2Picker.SelectedIndexChanged += MeterModels2Picker_SelectedIndexChanged2;

            meterModels2ContainerA = new StackLayout()
            {
                StyleId = "bloque" + 2
            };

            Label meterModels2Label = new Label()
            {
                Text = "Model",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            meterModels2ContainerD.Children.Add(meterModels2Picker);
            meterModels2ContainerC.Content = meterModels2ContainerD;
            meterModels2ContainerB.Content = meterModels2ContainerC;
            meterModels2ContainerA.Children.Add(meterModels2Label);
            meterModels2ContainerA.Children.Add(meterModels2ContainerB);
            
            meterNames2List = new List<string>();

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

            meterNames2Picker = new BorderlessPicker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 40,
                FontSize = 17,
                ItemsSource = meterNames2List,
                StyleId = "pickerName2"
            };

            meterNames2Picker.SelectedIndexChanged += MeterNames2Picker_SelectedIndexChanged2;

            meterNames2ContainerA = new StackLayout()
            {
                StyleId = "bloque" + 3
            };

            Label meterNames2Label = new Label()
            {
                Text = "Meter Type",
                Font = Font.SystemFontOfSize(17).WithAttributes(FontAttributes.Bold),
                Margin = new Thickness(0, 4, 0, 0)
            };

            meterNames2ContainerD.Children.Add(meterNames2Picker);
            meterNames2ContainerC.Content = meterNames2ContainerD;
            meterNames2ContainerB.Content = meterNames2ContainerC;
            meterNames2ContainerA.Children.Add(meterNames2Label);
            meterNames2ContainerA.Children.Add(meterNames2ContainerB);

            meterVendorsModelsNames2Container.Children.Add(meterVendors2ContainerA);
            meterVendorsModelsNames2Container.Children.Add(meterModels2ContainerA);
            meterVendorsModelsNames2Container.Children.Add(meterNames2ContainerA);

            meterNames2ContainerA.IsVisible = false;
            meterModels2ContainerA.IsVisible = false;
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

        #endregion

        #endregion

        #region GUI Handlers

        #region Pickers

        private void GenericPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
        }

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
                    cancelReasonOtherInputContainer.Opacity = 0.8;
                }
            }
        }

        private void MeterVendorsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;

            vendor = meterVendorsList[j];
            meterModelsList = this.config.meterTypes.GetModelsByVendorFromMeters(meters, vendor);
            name = "";

            try
            {
                meterModelsPicker.ItemsSource = meterModelsList;
                meterModelsContainerA.IsVisible = true;
                meterNamesContainerA.IsVisible = false;
            }
            catch (Exception e3)
            {
                meterModelsPicker.ItemsSource = null;
                meterModelsContainerA.IsVisible = false;
                meterNamesContainerA.IsVisible = false;
            }

        }

        private void MeterVendors2Picker_SelectedIndexChanged2(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;

            vendor2 = meterVendors2List[j];

            meterModels2List = this.config.meterTypes.GetModelsByVendorFromMeters(meters2, vendor2);
            name2 = "";

            try
            {
                meterModels2Picker.ItemsSource = meterModels2List;
                meterModels2ContainerA.IsVisible = true;
                meterNames2ContainerA.IsVisible = false;
            }
            catch (Exception e3)
            {
                meterModels2Picker.ItemsSource = null;
                meterModels2ContainerA.IsVisible = false;
                meterNames2ContainerA.IsVisible = false;
            }
        }

        private void MeterModelsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

            int i = ((BorderlessPicker)sender).SelectedIndex;

            meterNamesPicker.ItemDisplayBinding = new Binding("Display");

            model = meterModelsList[i];

            List<Meter> meterlist = this.config.meterTypes.GetMetersByModelAndVendorFromMeters(meters, vendor, model);

            try
            {
                meterNamesPicker.ItemsSource = meterlist;
                meterModelsContainerA.IsVisible = true;
                meterNamesContainerA.IsVisible = true;
            }
            catch (Exception e3)
            {
                meterNamesPicker.ItemsSource = null;
                meterModelsContainerA.IsVisible = false;
                meterNamesContainerA.IsVisible = false;
            }
        }

        private void MeterModels2Picker_SelectedIndexChanged2(object sender, EventArgs e)
        {
            int i = ((BorderlessPicker)sender).SelectedIndex;

            meterNames2Picker.ItemDisplayBinding = new Binding("Display");

            model2 = meterModels2List[i];

            List<Meter> meterlist2 = this.config.meterTypes.GetMetersByModelAndVendorFromMeters(meters2, vendor2, model2);

            try
            {
                meterNames2Picker.ItemsSource = meterlist2;
                meterModels2ContainerA.IsVisible = true;
                meterNames2ContainerA.IsVisible = true;
            }
            catch (Exception e3)
            {
                meterNames2Picker.ItemsSource = null;
                meterModels2ContainerA.IsVisible = false;
                meterNames2ContainerA.IsVisible = false;
            }
        }

        private void MeterNamesPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;

            Meter selectedMeter = (Meter)((BorderlessPicker)sender).SelectedItem;

            name = selectedMeter.Display;

            try
            {
                Console.WriteLine(name + " Selected");
            }
            catch (Exception n2)
            {
                Console.WriteLine(n2.StackTrace);
            }

        }

        private void MeterNames2Picker_SelectedIndexChanged2(object sender, EventArgs e)
        {
            int j = ((BorderlessPicker)sender).SelectedIndex;

            Meter selectedMeter = (Meter)((BorderlessPicker)sender).SelectedItem;

            name2 = selectedMeter.Display;

            try
            {
                Console.WriteLine(name2 + " Selected");
            }
            catch (Exception n2)
            {
                Console.WriteLine(n2.StackTrace);
            }
        }

        #endregion

        #region Sliders

        void OnSnapReadsSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / snapReadsStep);

            snapReadsSlider.Value = newStep * snapReadsStep;
            snapReadsLabel .Text  = snapReadsSlider.Value.ToString ();
        }

        void OnSnapReads2SliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / snapReads2Step);

            snapReads2Slider.Value = newStep * snapReads2Step;
            snapReads2Label .Text  = snapReads2Slider.Value.ToString();
        }

        #endregion

        #region Menu options

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

        private void OnMenuCaseReplaceMeter()
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
             }));
        }

        private void OnMenuCaseReplaceMTU()
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
             }));

        }

        private void OnMenuCaseTurnOFF()
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
             }));

        }

        private void OnMenuCaseAddMTU()
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

                 shadoweffect.IsVisible &= Device.Idiom != TargetIdiom.Phone; // if (Device.Idiom == TargetIdiom.Phone) shadoweffect.IsVisible = false;
             }));
        }

        private void OnMenuCaseReadMTU()
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
             }));
        }

        private void OpenSettingsCallAsync(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    if (FormsApp.ble_interface.IsOpen())
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new AclaraViewSettings(dialogsSaved), false);
                        return;
                    }
                    else
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new AclaraViewSettings(true), false);
                    }
                }
                catch (Exception i2)
                {
                    Console.WriteLine(i2.StackTrace);
                }
            });
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
                }
                catch (Exception v)
                {
                    Console.WriteLine(v.StackTrace);
                }
                contador--;
            }

            try
            {
                await Navigation.PopToRootAsync(false);
            }
            catch (Exception v1)
            {
                Console.WriteLine(v1.StackTrace);
            }

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

            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMTU(dialogsSaved), false);
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

            Task.Delay(2000).ContinueWith(t =>
             Device.BeginInvokeOnMainThread(() =>
             {
                 dialog_turnoff_two.IsVisible = false;
                 dialog_turnoff_three.IsVisible = true;
             }));


        }

        private void MeterCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_meter_replace_one.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void MeterOkTapped(object sender, EventArgs e)
        {
            dialog_meter_replace_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            Application.Current.MainPage.Navigation.PushAsync(new AclaraViewReplaceMeter(dialogsSaved), false);
        }

        #endregion

        #region Confirmation dialogs

        private void ServicePortId_Ok_Tapped(object sender, EventArgs e)
        {

            if (servicePortIdInput.Text.Equals(serviceCheckEntry.Text))
            {
                errorServicePort.IsVisible = false;
                dialog_open_bg.IsVisible = false;
                turnoff_mtu_background.IsVisible = false;
                dialog_servicePortId.IsVisible = false;
                serviceCheckEntry.Text = "";
            }
            else
            {
                errorServicePort.IsVisible = true;
            }
        }

        private void ServicePortId_Cancel_Tapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            dialog_servicePortId.IsVisible = false;
            errorServicePort.IsVisible = false;
            servicePortIdInput.Text = "";
        }

        private void FieldOrder_Ok_Tapped(object sender, EventArgs e)
        {

            if (fieldOrderInput.Text.Equals(fieldOrderCheckEntry.Text))
            {
                errorFieldOrder.IsVisible = false;
                dialog_open_bg.IsVisible = false;
                turnoff_mtu_background.IsVisible = false;
                dialog_fieldOrder.IsVisible = false;
                fieldOrderCheckEntry.Text = "";
            }
            else
            {
                errorFieldOrder.IsVisible = true;
            }
        }

        private void FieldOrder_Cancel_Tapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
            dialog_fieldOrder.IsVisible = false;
            errorFieldOrder.IsVisible = false;
            fieldOrderInput.Text = "";
        }

        #endregion

        #region Form

        protected override void OnAppearing()
        {
            base.OnAppearing();

            background_scan_page.Opacity = 0.5;
            background_scan_page.FadeTo(1, 500);
        }

        private void submit_send(object sender, EventArgs e)
        {
            int selectedCancelReasonIndex = cancelReasonPicker.SelectedIndex;
            string selectedCancelReason = "Other";

            if (selectedCancelReasonIndex > -1)
            {
                selectedCancelReason = cancelReasonPicker.Items[cancelReasonPicker.SelectedIndex];
            }

            this.add_mtu.Cancel(selectedCancelReason);

            dialog_open_bg.IsVisible = false;
            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;
            Task.Run(async () =>
            {
                await Task.Delay(500); Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PopAsync(false);
                });
            });
        }

        private void misc_command()
        {
            port1label.Opacity = 0.5;
            misclabel.Opacity = 1;
            port2label.Opacity = 0.5;

            port1label.FontSize = 19;
            misclabel.FontSize = 22;
            port2label.FontSize = 19;

            port1view.IsVisible = false;
            port2view.IsVisible = false;
            miscview.IsVisible = true;
        }

        private void port2_command()
        {
            port1label.Opacity = 0.5;
            misclabel.Opacity = 0.5;
            port2label.Opacity = 1;

            port1label.FontSize = 19;
            misclabel.FontSize = 19;
            port2label.FontSize = 22;

            port1view.IsVisible = false;
            port2view.IsVisible = true;
            miscview.IsVisible = false;


        }

        private void port1_command()
        {
            port1label.Opacity = 1;
            misclabel.Opacity = 0.5;
            port2label.Opacity = 0.5;

            port1label.FontSize = 22;
            misclabel.FontSize = 19;
            port2label.FontSize = 19;

            port1view.IsVisible = true;
            port2view.IsVisible = false;
            miscview.IsVisible = false;
        }

        private void ReturnToMainView(object sender, EventArgs e)
        {
            if (isCancellable)
            {
                Application.Current.MainPage.Navigation.PopAsync(false);
            }
            else
            {
                //REASON
                dialog_open_bg.IsVisible = true;

                Popup_start.IsVisible = true;
                Popup_start.IsEnabled = true;
            }
        }

        #endregion

        #endregion

        #region Validation

        private void ServicePortId_validate(int v)
        {
            if (!servicePortIdInput.Text.Equals(""))
            {
                if (v == 1)
                {
                    dialog_open_bg.IsVisible = true;
                    turnoff_mtu_background.IsVisible = true;
                    dialog_servicePortId.IsVisible = true;

                }
                else if (v == 2)
                {
                    dialog_open_bg.IsVisible = true;
                    turnoff_mtu_background.IsVisible = true;
                    dialog_servicePortId.IsVisible = true;
                }
            }
        }

        private void FieldOrder_validate(int v)
        {
            if (!fieldOrderInput.Text.Equals(""))
            {

                if (v == 1)
                {
                    dialog_open_bg.IsVisible = true;
                    turnoff_mtu_background.IsVisible = true;
                    dialog_fieldOrder.IsVisible = true;

                }
                else if (v == 2)
                {
                    dialog_open_bg.IsVisible = true;
                    turnoff_mtu_background.IsVisible = true;
                    dialog_fieldOrder.IsVisible = true;
                }
            }

        }

        private bool ValidateEmptyFields()
        {

            /** Validation **/
            /** Check if visible fields are filled with the correct values in order to Enable Add MTU Cmd **/

            /////// Port 1 ///////
            if (servicePortIdInput.Text.Length < servicePortIdInput.MaxLength)
                return false;

            if (fieldOrderContainer.IsVisible)
                if (fieldOrderInput.Text.Length < fieldOrderInput.MaxLength)
                    return false;

            if (meterSerialNumberContainer.IsVisible)
                if (meterSerialNumberInput.Text.Length < meterSerialNumberInput.MaxLength)
                    return false;

            if (meterVendorsModelsNamesContainer.Children[2].IsVisible)
                if (model != null)
                    if (meterVendorsModelsNamesContainer.Children[1].IsVisible)
                        if (name.Length < 0)
                            return false;

            if (initialReadingInput.Text.Length < initialReadingInput.MaxLength)
                return false;

            if (readIntervalContainer.Opacity > 0.8)
                if (readIntervalPicker.SelectedIndex == -1)
                    return false;

            /*
            if (snapReadsContainer.IsVisible)   if(snapReadsSubContainer.Opacity>0.8)
                if(snapReadsLabel.Text -1)
                    isValid = false;
            */

            if (twoWayContainer.IsVisible)
                if (twoWayPicker.SelectedIndex == -1)
                    return false;


            if (alarmsContainer.IsVisible)
                if (alarmsPicker.SelectedIndex == -1)
                    return false;


            if (demandsContainer.IsVisible)
                if (demandsPicker.SelectedIndex == -1)
                    return false;


            /////// Port 2 ///////

            if (servicePortId2Input.Text.Length < servicePortId2Input.MaxLength)
                return false;

            if (fieldOrder2Container.IsVisible)
                if (fieldOrder2Input.Text.Length < fieldOrder2Input.MaxLength)
                    return false;


            if (meterSerialNumber2Container.IsVisible)
                if (meterSerialNumber2Input.Text.Length < meterSerialNumber2Input.MaxLength)
                    return false;


            if (meterVendorsModelsNames2Container.Children[2].IsVisible)
                if (model2 != null)
                    if (meterVendorsModelsNames2Container.Children[1].IsVisible)
                        if (name2.Length < 0)
                            return false;

            if (initialReading2Input.Text.Length < initialReading2Input.MaxLength)
                return false;

            if (readInterval2Container.Opacity > 0.8)
                if (readInterval2Picker.SelectedIndex == -1)
                    return false;

            if (twoWay2Container.IsVisible)
                if (twoWay2Picker.SelectedIndex == -1)
                    return false;


            if (alarms2Container.IsVisible)
                if (alarms2Picker.SelectedIndex == -1)
                    return false;


            if (demands2Container.IsVisible)
                if (demands2Picker.SelectedIndex == -1)
                    return false;

            /////// Misc ///////

            if (mtuGeolocationLat.Text.Length < 0)
                return false;

            if (mtuGeolocationLong.Text.Length < 0)
                return false;

            foreach (BorderlessPicker picker in optionalPickers)
            {
                if(picker.SelectedIndex != -1)
                {
                    return false;
                }
            }

            foreach (BorderlessEntry entry in optionalEntries)
            {
                if(entry.Text.Equals(""))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region GUI Logic

        private void UpdateStatusPort2 ()
        {
            block_view_port2.IsVisible = this.port2status;
            enablePort2.Text      = ( this.port2status ) ? "Disable Port 2" : "Enable Port 2";
            enablePort2.TextColor = ( this.port2status ) ? Color.Gold : Color.White;

            this.ContainerCopyPort1.IsVisible = this.port2status;
            this.copyPort1.IsEnabled = this.port2status;
            //this.copyPort1.IsVisible = this.port2status;
        }

        private void ChangeCheckboxSnapReads ( bool active )
        {
            snapReads2SubContainer.Opacity = ( active ) ? 1 : 0.8;
            Checkbox_SnapReads_Port2.Source = "checkbox_" + ( ( active ) ? "on" : "off" );
            snapReads2Slider.IsEnabled = active;
            snapRead2Status = active;
        }

        #endregion

        #region Action

        private void AddMtu(object sender, EventArgs e)
        {
            /*if (!ValidateEmptyFields())
            {

                DisplayAlert("Error", "Mandatory fields are not filled in", "Ok");
                return;
            }*/

            isCancellable = true;

            if (!_userTapped)
            {
                //Task.Delay(100).ContinueWith(t =>

                Device.BeginInvokeOnMainThread(() =>
                {
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;
                    _userTapped = true;
                    background_scan_page.IsEnabled = false;
                    ChangeLowerButtonImage(true);
                //}));
                    /*Task.Delay(100).ContinueWith(t =>

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        backdark_bg.IsVisible = true;
                        indicator.IsVisible = true;
                        _userTapped = true;
                        background_scan_page.IsEnabled = false;
                        ChangeLowerButtonImage(true);


                        Task.Delay(1000).ContinueWith(t0 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 5);
                        }));

                        Task.Delay(2000).ContinueWith(t1 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 4);
                        }));

                        Task.Delay(3000).ContinueWith(t2 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 3);
                        }));

                        Task.Delay(4000).ContinueWith(t3 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 2);
                        }));

                        Task.Delay(5000).ContinueWith(t4 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtuShortTime, 1);
                        }));

                        Task.Delay(6000).ContinueWith(t5 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.PreparingToProgram, 0);
                        }));

                        Task.Delay(7000).ContinueWith(t6 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.TurningOffMtu, 0);
                        }));

                        Task.Delay(8000).ContinueWith(t7 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtuAgain, 0);
                        }));

                        Task.Delay(9000).ContinueWith(t8 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 20);
                        }));

                        Task.Delay(10000).ContinueWith(t9 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 19);
                        }));

                        Task.Delay(11000).ContinueWith(t10 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 18);
                        }));

                        Task.Delay(12000).ContinueWith(t11 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 17);
                        }));

                        Task.Delay(13000).ContinueWith(t12 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.ProgramingMtu, 0);
                        }));

                        Task.Delay(14000).ContinueWith(t13 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.VerifyingMtuData, 0);
                        }));


                        Task.Delay(15000).ContinueWith(t14 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 20);
                        }));

                        Task.Delay(16000).ContinueWith(t15 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 19);
                        }));

                        Task.Delay(17000).ContinueWith(t16 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 18);
                        }));

                        Task.Delay(18000).ContinueWith(t17 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            label_read.Text = GetStringFromMTUStatus(MTUStatus.CheckingEnconderShortTime, 17);
                        }));

                        Task.Delay(19000).ContinueWith(t18 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.TurningOnMtu, 0);
                        }));

                        Task.Delay(20000).ContinueWith(t17 =>
                        Device.BeginInvokeOnMainThread(() =>
                        {
                             label_read.Text = GetStringFromMTUStatus(MTUStatus.ReadingMtu, 0);

                             //ThreadProcedureMTUCOMMAction();
                             FinalReadListView = new List<ReadMTUItem>();


                             FinalReadListView.Add(new ReadMTUItem()
                             {
                                 Title = "Param Field" + ":",
                                 isDisplayed = "true",
                                 Height = "64",
                                 isMTU = "true",
                                 isMeter = "false",
                                 Description = "99" //parameter.Value
                             });

                             FinalReadListView.Add(new ReadMTUItem()
                             {
                                 Title = "Here lies the Port title...",
                                 isDisplayed = "true",
                                 Height = "40",
                                 isMTU = "false",
                                 isMeter = "true",
                                 Description = "Port " + "99" + ": " + " PlaceHolder"
                             });

                             FinalReadListView.Add(new ReadMTUItem()
                             {
                                 Title = "\t\t" + "Param Field" + ":",
                                 isDisplayed = "true",
                                 Height = "64",
                                 isMTU = "true",
                                 isMeter = "false",
                                 Description = "\t\t" + "99"//parameter.Value
                             });


                             FinalReadListView.Add(new ReadMTUItem()
                             {
                                 Title = "Another Param Field" + ":",
                                 isDisplayed = "true",
                                 Height = "64",
                                 isMTU = "true",
                                 isMeter = "false",
                                 Description = "99" //parameter.Value
                             });


                             ReadMTUChangeView.IsVisible = false;
                             listaMTUread.IsVisible = true;
                             listaMTUread.ItemsSource = FinalReadListView;

                             Task.Run(() =>
                             {
                                 Device.BeginInvokeOnMainThread(() =>
                                 {
                                     label_read.Opacity = 1;
                                     backdark_bg.IsVisible = false;
                                     indicator.IsVisible = false;
                                     _userTapped = false;
                                     ChangeLowerButtonImage(false);
                                     background_scan_page.IsEnabled = true;
                                 });
                             });
                        }));
                    }));*/
                //    Device.BeginInvokeOnMainThread(() =>
                //{
                    Task.Factory.StartNew(AddMtu_Action);
                    //Task.Run ( async () => await AddMtu_Action () );
                    //AddMtu_Action();
                });
            }
        }

        private void AddMtu_Action()
        {
            #region Form Parameters

            dynamic MtuConditions     = addMtuForm.conditions.mtu;
            dynamic GlobalsConditions = addMtuForm.conditions.globals;

            #region Port 1

            // Service Port ID
            addMtuForm.AddParameter (
                FIELD.SERVICE_PORT_ID,
                servicePortIdInput.Text );

            // Field Order
            if ( GlobalsConditions.WorkOrderRecording )
                addMtuForm.AddParameter (
                    FIELD.FIELD_ORDER,
                    fieldOrderInput.Text );

            // Meter Number
            if ( GlobalsConditions.UseMeterSerialNumber )
                addMtuForm.AddParameter (
                    FIELD.METER_NUMBER,
                    meterSerialNumberInput.Text );

            // Initial Reading
            addMtuForm.AddParameter (
                FIELD.INITIAL_READING,
                initialReadingInput.Text);

            // Selected Meter ID
            addMtuForm.AddParameter (
                FIELD.SELECTED_METER,
                ( Meter )meterNamesPicker.SelectedItem );

            // Read Interval
            if ( GlobalsConditions.IndividualReadInterval )
                addMtuForm.AddParameter (
                    FIELD.READ_INTERVAL,
                    readIntervalPicker.SelectedItem.ToString() );

            // Snap Reads
            if ( GlobalsConditions.AllowDailyReads && MtuConditions.DailyReads )
                addMtuForm.AddParameter (
                    FIELD.SNAP_READS,
                    snapReadsSlider.Value.ToString() );

            // 2-Way
            if ( MtuConditions.FastMessageConfig )
                addMtuForm.AddParameter (
                    FIELD.TWO_WAY,
                    twoWayPicker.SelectedItem.ToString() );

            // Alarms
            if ( MtuConditions.RequiresAlarmProfile )
                addMtuForm.AddParameter (
                    FIELD.ALARM,
                    ( Alarm )alarmsPicker.SelectedItem );

            // Demands
            if ( MtuConditions.MtuDemand )
                addMtuForm.AddParameter (
                    FIELD.DEMAND,
                    ( Demand )demandsPicker.SelectedItem );

            #endregion

            #region Port 2

            if ( MtuConditions.TwoPorts )
            {
                // Service Port ID 2
                addMtuForm.AddParameter (
                    FIELD.SERVICE_PORT_ID2,
                    servicePortId2Input.Text );

                // Field Order 2
                if ( GlobalsConditions.WorkOrderRecording )
                    addMtuForm.AddParameter (
                        FIELD.FIELD_ORDER2,
                        fieldOrder2Input.Text );

                // Meter Number 2
                if ( GlobalsConditions.UseMeterSerialNumber )
                    addMtuForm.AddParameter (
                        FIELD.METER_NUMBER2,
                        meterSerialNumber2Input.Text );

                // Initial Reading 2
                addMtuForm.AddParameter (
                    FIELD.INITIAL_READING2,
                    initialReading2Input.Text );

                // Read Interval 2
                if ( GlobalsConditions.IndividualReadInterval )
                    addMtuForm.AddParameter (
                        FIELD.READ_INTERVAL2,
                        readInterval2Picker.SelectedItem.ToString() );

                // Selected Meter ID 2
                addMtuForm.AddParameter (
                    FIELD.SELECTED_METER2,
                    ( Meter )meterNames2Picker.SelectedItem );

                // Snap Reads 2
                if ( GlobalsConditions.AllowDailyReads && MtuConditions.DailyReads )
                    addMtuForm.AddParameter (
                        FIELD.SNAP_READS2,
                        snapReads2Slider.Value.ToString() );

                // 2-Way 2
                if ( MtuConditions.FastMessageConfig )
                    addMtuForm.AddParameter (
                        FIELD.TWO_WAY2,
                        twoWay2Picker.SelectedItem.ToString() );

                // Alarms 2
                if ( MtuConditions.RequiresAlarmProfile )
                    addMtuForm.AddParameter (
                        FIELD.ALARM2,
                        ( Alarm )alarms2Picker.SelectedItem );

                // Demands 2
                if ( MtuConditions.MtuDemand )
                    addMtuForm.AddParameter (
                        FIELD.DEMAND2,
                        ( Demand )demands2Picker.SelectedItem );
            }

            #endregion

            #region Optional parameters

            List<Parameter> optionalParams = new List<Parameter>();

            foreach ( BorderlessPicker p in optionalPickers )
                optionalParams.Add ( new Parameter ( p.Name, p.Display, p.SelectedItem, true ) );

            foreach ( BorderlessEntry e in optionalEntries )
                optionalParams.Add ( new Parameter ( e.Name, e.Display, e.Text, true ) );

            addMtuForm.AddParameter (
                FIELD.OPTIONAL_PARAMS,
                optionalParams );

            #endregion

            #endregion

            #region On Add MTU Action finish

            this.add_mtu.OnFinish += ((s, e) =>
            {
                FinalReadListView = new List<ReadMTUItem>();

                Parameter[] paramResult = e.Result.getParameters();

                int mtu_type = 0;

                foreach (Parameter p in paramResult)
                {
                    try
                    {
                        if ( p.CustomParameter.Equals("MtuType") )
                        {
                            mtu_type = Int32.Parse(p.Value.ToString());
                        }
                    }
                    catch (Exception e5)
                    {
                        Console.WriteLine(e5.StackTrace);
                    }
                }

                InterfaceParameters[] interfacesParams = FormsApp.config.getUserInterfaceFields(mtu_type, "ReadMTU");

                foreach (InterfaceParameters parameter in interfacesParams)
                {
                    if (parameter.Name.Equals("Port"))
                    {
                        ActionResult[] ports = e.Result.getPorts();

                        for (int i = 0; i < ports.Length; i++)
                        {
                            foreach (InterfaceParameters port_parameter in parameter.Parameters)
                            {
                                Parameter param = null;

                                if (port_parameter.Name.Equals("Description"))
                                {
                                    param = ports[i].getParameterByTag(port_parameter.Name);

                                    FinalReadListView.Add(new ReadMTUItem()
                                    {
                                        Title = "Here lies the Port title...",
                                        isDisplayed = "true",
                                        Height = "40",
                                        isMTU = "false",
                                        isMeter = "true",
                                        Description = "Port " + i + ": " + param.getValue() //parameter.Value
                                    });
                                }
                                else
                                {
                                    if (port_parameter.Source != null)
                                    {
                                        try
                                        {
                                            param = ports[i].getParameterByTag(port_parameter.Source.Split(new char[] { '.' })[1]);
                                        }
                                        catch (Exception e2)
                                        {
                                            Console.WriteLine(e2.StackTrace);
                                        }
                                    }

                                    if (param == null)
                                    {
                                        param = ports[i].getParameterByTag(port_parameter.Name);
                                    }

                                    if (param != null)
                                    {
                                        FinalReadListView.Add(new ReadMTUItem()
                                        {
                                            Title = "\t\t\t\t\t" + param.getLogDisplay() + ":",
                                            isDisplayed = "true",
                                            Height = "64",
                                            isMTU = "true",
                                            isMeter = "false",
                                            Description = "\t\t\t\t\t" + param.getValue() //parameter.Value
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Parameter param = null;

                        if (parameter.Source != null)
                        {
                            try
                            {
                                param = e.Result.getParameterByTag(parameter.Source.Split(new char[] { '.' })[1]);
                            }
                            catch (Exception e3)
                            {
                                Console.WriteLine(e3.StackTrace);
                            }
                        }

                        if (param == null)
                        {
                            param = e.Result.getParameterByTag(parameter.Name);
                        }

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
                    label_read.Text = "Successful MTU write";
                    background_scan_page.IsEnabled = true;
                    
                    ReadMTUChangeView.IsVisible = false;
                    listaMTUread.IsVisible = true;
                    listaMTUread.ItemsSource = FinalReadListView;
                }));
            });

            #endregion

            #region On Add MTU Action error

            this.add_mtu.OnError += ((s, e) =>
            {
                Console.WriteLine("Action Errror");
                Console.WriteLine("Press Key to Exit");
                // Console.WriteLine(s.ToString());

                String result = e.Message;



                Task.Delay(100).ContinueWith(t =>
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      _userTapped = false;
                      bg_read_mtu_button.NumberOfTapsRequired = 1;
                      ChangeLowerButtonImage(false);
                      backdark_bg.IsVisible = false;
                      indicator.IsVisible = false;
                      label_read.Text = result;
                      background_scan_page.IsEnabled = true;
                  }));


                DisplayAlert("Error", result, "Ok");
                Console.WriteLine(result.ToString());

            });

            #endregion

            // Launch action!
            add_mtu.Run ( addMtuForm );
        }

        #endregion

        #region Location
        private void GPSUpdateButton(object sender, EventArgs e)
        {

            if (IsLocationAvailable())
            {

                Task.Run(async () => { await StartListening(); });
            }

        }


        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            CrossGeolocator.Current.DesiredAccuracy = 5;


            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

        async Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return;
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, true);
            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
            await Task.Delay(5000).ContinueWith(t => StopListening());
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            //If updating the UI, ensure you invoke on main thread
            var position = e.Position;
            var output = "Full: Lat: " + position.Latitude + " Long: " + position.Longitude;
            output += "\n" + $"Time: {position.Timestamp}";
            output += "\n" + $"Heading: {position.Heading}";
            output += "\n" + $"Speed: {position.Speed}";
            output += "\n" + $"Accuracy: {position.Accuracy}";
            output += "\n" + $"Altitude: {position.Altitude}";
            output += "\n" + $"Altitude Accuracy: {position.AltitudeAccuracy}";
            Console.WriteLine(output);
            //accuracy.Text = output.ToString();

            mtuGeolocationLat.Text = position.Latitude.ToString();
            mtuGeolocationLong.Text = position.Longitude.ToString();

        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Console.WriteLine(e.Error);
        }

        private async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;
            await CrossGeolocator.Current.StopListeningAsync();
            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }
        #endregion

        #region Other methods

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

        #endregion
    }
}
