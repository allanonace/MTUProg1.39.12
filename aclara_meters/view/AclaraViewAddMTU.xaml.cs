using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using aclara_meters.Behaviors;
using aclara_meters.Helpers;
using aclara_meters.Models;
using Acr.UserDialogs;
using MTUComm;
using MTUComm.actions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Settings;
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
        #region Debug

        //#if ADHOC || RELEASE
        private const bool   DEBUG_AUTO_MODE_ON  = false;
        //#elif DEBUG
        //private const bool   DEBUG_AUTO_MODE_ON  = true;
        //#endif
        private const int    DEBUG_VENDOR_INDEX  = 0; // GENERIC
        private const int    DEBUG_MODEL_INDEX   = 0; // 4D PF2
        private const int    DEBUG_MTRNAME_INDEX = 0; // Pos 4D PF2 CCF
        private const int    DEBUG_ALARM_INDEX   = 0; // All
        private const int    DEBUG_DEMAND_INDEX  = 0;
        private const string DEBUG_SERVICEPORTID = "111111111";
        private const string DEBUG_FIELDORDER    = "11111111111111111111";
        private const string DEBUG_METERSERIAL   = "111111111111";
        private const string DEBUG_INITREADING   = "000020";
        private const string DEBUG_READSINTERVAL = "1 Hour";
        private const bool   DEBUG_SNAPSREADS_OK = false;
        private const string DEBUG_SNAPSREADS    = "10";
        private const string DEBUG_GPS_LAT       = "43,316";
        private const string DEBUG_GPS_LON       = "-2.981";
        private const string DEBUG_GPS_ALT       = "1";
        private const int    DEBUG_MTULOCATION   = 0; // Outside
        private const int    DEBUG_METERLOC      = 0; // Outside
        private const int    DEBUG_CONSTRUC      = 0; // Vinyl

        #endregion

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

        private string page_to_controller;

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
        //private Global globals;
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

        // Conditions - Alarms
        private bool mtuRequiresAlarmProfile;

        // GPS
        private string altitude;

        //Logout control
        private bool isLogout;

        #endregion

        #region Initialization

        public AclaraViewAddMTU ()
        {
            InitializeComponent ();
        }

        public AclaraViewAddMTU(IUserDialogs dialogs, string page)
        {
            InitializeComponent();

            page_to_controller = page;

            dialogsSaved = dialogs;

            CheckPageByName();

        }

        private void CheckPageByName()
        {

            switch (page_to_controller)
            {
                case "AddMTU":

                    Device.BeginInvokeOnMainThread(() =>
                    {

                        #region Set Add MTU Info

                        name_of_window_port1.Text = "Add MTU";
                        name_of_window_port2.Text = "Add MTU";
                        name_of_window_misc.Text = "Add MTU";

                        bg_read_mtu_button_img.Source = "add_mtu_btn.png";

              
                        #endregion
                    });

                    #region AddMTU Case

                    this.add_mtu = new MTUComm.Action(
                        config: FormsApp.config,
                        serial: FormsApp.ble_interface,
                        type: MTUComm.Action.ActionType.AddMtu,
                        user: FormsApp.credentialsService.UserName);

                    #region Prepare mtuForm

                    this.config = Configuration.GetInstance();

                    // Get detected mtu
                    MTUBasicInfo mtuBasicInfo = MtuForm.mtuBasicInfo;
                    this.detectedMtuType = (int)mtuBasicInfo.Type;
                    currentMtu = this.config.mtuTypes.FindByMtuId(this.detectedMtuType);

                    // Initialize logic-form
                    this.addMtuForm = new AddMtuForm(currentMtu);

                    #endregion

                    isCancellable = false;

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
                    if (!string.IsNullOrEmpty(FormsApp.credentialsService.UserName))
                        userName.Text = FormsApp.credentialsService.UserName; //"Kartik";

                    battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
                    rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

                    InitializeLowerbarLabel();

                    InitializeAddMtuForm();

                    RegisterEventHandlers();

                    Popup_start.IsVisible = false;
                    Popup_start.IsEnabled = false;

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

                            })
                         );
                      })
                    );

                    #endregion


                    break;


                case "replaceMTU":

                    Device.BeginInvokeOnMainThread(() =>
                    {

                        #region Set Replace MTU Info

                        name_of_window_port1.Text = "Replace MTU";
                        name_of_window_port2.Text = "Replace MTU";
                        name_of_window_misc.Text = "Replace MTU";

                        bg_read_mtu_button_img.Source = "rep_mtu_btn.png";

                        oldMtuContainer.IsVisible = true;
                        oldMtuContainer2.IsVisible = true;


                        meterSerialContainer.IsVisible = false;
                        meterSerial2Container.IsVisible = false;

                        newMeterContainer.IsVisible = true;
                        newMeterContainer2.IsVisible = true;


                        #region NewMeterLabel Validation

                        if (FormsApp.config.global.NewMeterLabel != null)
                        {
                            new_meter_number_port1.Text = FormsApp.config.global.NewMeterLabel;
                            new_meter_number_port2.Text = FormsApp.config.global.NewMeterLabel;
                        }

                        #endregion



                        #region Color Entry

                        if (FormsApp.config.global.ColorEntry)
                        {
                            new_meter_number_port1.TextColor = Color.FromHex("#FF0000");
                            new_meter_number_port2.TextColor = Color.FromHex("#FF0000");

                            old_mtu_id_port1.TextColor = Color.FromHex("#FF0000");
                            old_mtu_id_port2.TextColor = Color.FromHex("#FF0000");
                        }

                        #endregion
                    });

                    #endregion

                    #region ReplaceMTU Case

                    this.add_mtu = new MTUComm.Action(
                        config: FormsApp.config,
                        serial: FormsApp.ble_interface,
                        type: MTUComm.Action.ActionType.ReplaceMTU,
                        user: FormsApp.credentialsService.UserName);

                    #region Prepare mtuForm

                    this.config = Configuration.GetInstance();

                    // Get detected mtu
                    MTUBasicInfo mtuBasicInfo_replaceMtu = MtuForm.mtuBasicInfo;
                    this.detectedMtuType = (int)mtuBasicInfo_replaceMtu.Type;
                    currentMtu = this.config.mtuTypes.FindByMtuId(this.detectedMtuType);

                    // Initialize logic-form
                    this.addMtuForm = new AddMtuForm(currentMtu);

                    #endregion

                    isCancellable = false;

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
                    if (!string.IsNullOrEmpty(FormsApp.credentialsService.UserName))
                        userName.Text = FormsApp.credentialsService.UserName; //"Kartik";

                    battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
                    rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

                    InitializeLowerbarLabel();

                    InitializeAddMtuForm();

                    RegisterEventHandlers();

                    Popup_start.IsVisible = false;
                    Popup_start.IsEnabled = false;

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

                            })
                         );
                      })
                    );

                    #endregion


                    break;


                case "replaceMeter":

                    Device.BeginInvokeOnMainThread(() =>
                    {

                        #region Set Replace meter Info

                        name_of_window_port1.Text = "Replace Meter";
                        name_of_window_port2.Text = "Replace Meter";
                        name_of_window_misc.Text = "Replace Meter";

                        bg_read_mtu_button_img.Source = "rep_meter_btn.png";

                        oldMeterContainer.IsVisible = true;
                        oldMeterContainer2.IsVisible = true;

                        oldReadingContainer.IsVisible = true;
                        oldReadingContainer2.IsVisible = true;

                        if(FormsApp.config.global.MeterWorkRecording)
                        {
                            oldMeterWorkingContainer.IsVisible = true;
                            oldMeterWorkingContainer2.IsVisible = true;
                        }
                      
                        replaceMeterContainer.IsVisible = true;
                        replaceMeterContainer2.IsVisible = true;

                        newMeterContainer.IsVisible = true;
                        newMeterContainer2.IsVisible = true;


                        meterSerialContainer.IsVisible = false;
                        meterSerial2Container.IsVisible = false;

             
                        #region NewMeterLabel Validation

                        if(FormsApp.config.global.NewMeterLabel != null)
                        {
                            new_meter_number_port1.Text = FormsApp.config.global.NewMeterLabel;
                            new_meter_number_port2.Text = FormsApp.config.global.NewMeterLabel;
                        }

                        #endregion

                        #region Color Entry

                        if (FormsApp.config.global.ColorEntry)
                        {

                            old_meter_number_port1.TextColor = Color.FromHex("#FF0000");
                            old_meter_number_port2.TextColor = Color.FromHex("#FF0000");

                            old_reading_port1.TextColor = Color.FromHex("#FF0000");
                            old_reading_port2.TextColor = Color.FromHex("#FF0000");

                            old_meter_working_port1.TextColor = Color.FromHex("#FF0000");
                            old_meter_working_port2.TextColor = Color.FromHex("#FF0000");

                            replace_meter_port1.TextColor = Color.FromHex("#FF0000");
                            replace_meter_port2.TextColor = Color.FromHex("#FF0000");

                            new_meter_number_port1.TextColor = Color.FromHex("#FF0000");
                            new_meter_number_port2.TextColor = Color.FromHex("#FF0000");
                        }

                    });

                    CallToInitNewUIPickers();

                    #endregion


                    #endregion

                    #region ReplaceMeter Case

                    this.add_mtu = new MTUComm.Action(
                        config: FormsApp.config,
                        serial: FormsApp.ble_interface,
                        type: MTUComm.Action.ActionType.ReplaceMeter,
                        user: FormsApp.credentialsService.UserName);

                    #region Prepare mtuForm

                    this.config = Configuration.GetInstance();

                    // Get detected mtu
                    MTUBasicInfo mtuBasicInfo_replaceMeter= MtuForm.mtuBasicInfo;
                    this.detectedMtuType = (int)mtuBasicInfo_replaceMeter.Type;
                    currentMtu = this.config.mtuTypes.FindByMtuId(this.detectedMtuType);

                    // Initialize logic-form
                    this.addMtuForm = new AddMtuForm(currentMtu);

                    #endregion

                    isCancellable = false;

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
                    if (!string.IsNullOrEmpty(FormsApp.credentialsService.UserName))
                        userName.Text = FormsApp.credentialsService.UserName; //"Kartik";

                    battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
                    rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

                    InitializeLowerbarLabel();

                    InitializeAddMtuForm();

                    RegisterEventHandlers();

                    Popup_start.IsVisible = false;
                    Popup_start.IsEnabled = false;

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

                            })
                         );
                      })
                    );

                    #endregion


                    break;


                case "AddMTUAddMeter":

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        #region Set Add MTU Add Meter Info

                        name_of_window_port1.Text = "Add MTU / Add Meter";
                        name_of_window_port2.Text = "Add MTU / Add Meter";
                        name_of_window_misc.Text = "Add MTU / Add Meter";

                        bg_read_mtu_button_img.Source = "add_mtu_meter_btn.png";

                        #endregion


                        #region Set UI for Add Mtu Add Meter

                        meterSerialContainer.IsVisible = false;
                        meterSerial2Container.IsVisible = false;

                        if (FormsApp.config.global.MeterWorkRecording)
                        {
                            oldMeterWorkingContainer.IsVisible = true;
                            oldMeterWorkingContainer2.IsVisible = true;
                        }

                        replaceMeterContainer.IsVisible = false;
                        replaceMeterContainer2.IsVisible = false;

                        newMeterContainer.IsVisible = true;
                        newMeterContainer2.IsVisible = true;

                        meterSerialContainer.IsVisible = false;
                        meterSerial2Container.IsVisible = false;


                        #region NewMeterLabel Validation

                        if (FormsApp.config.global.NewMeterLabel != null)
                        {
                            new_meter_number_port1.Text = FormsApp.config.global.NewMeterLabel;
                            new_meter_number_port2.Text = FormsApp.config.global.NewMeterLabel;
                        }

                        #endregion

                        #endregion

                        #region Color Entry

                        if (FormsApp.config.global.ColorEntry)
                        {
                            old_meter_working_port1.TextColor = Color.FromHex("#FF0000");
                            old_meter_working_port2.TextColor = Color.FromHex("#FF0000");

                            replace_meter_port1.TextColor = Color.FromHex("#FF0000");
                            replace_meter_port2.TextColor = Color.FromHex("#FF0000");

                            new_meter_number_port1.TextColor = Color.FromHex("#FF0000");
                            new_meter_number_port2.TextColor = Color.FromHex("#FF0000");
                        }

                    });

                    #endregion

                    CallToInitNewUIPickers();

                    #region AddMTU | AddMeter Case

                    this.add_mtu = new MTUComm.Action(
                        config: FormsApp.config,
                        serial: FormsApp.ble_interface,
                        type: MTUComm.Action.ActionType.AddMtuAddMeter,
                        user: FormsApp.credentialsService.UserName);

                    #region Prepare mtuForm

                    this.config = Configuration.GetInstance();

                    // Get detected mtu
                    MTUBasicInfo mtuBasicInfo_addmtuaddmeter = MtuForm.mtuBasicInfo;
                    this.detectedMtuType = (int)mtuBasicInfo_addmtuaddmeter.Type;
                    currentMtu = this.config.mtuTypes.FindByMtuId(this.detectedMtuType);

                    // Initialize logic-form
                    this.addMtuForm = new AddMtuForm(currentMtu);

                    #endregion

                    isCancellable = false;

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
                    if (!string.IsNullOrEmpty(FormsApp.credentialsService.UserName))
                        userName.Text = FormsApp.credentialsService.UserName; //"Kartik";

                    battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
                    rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

                    InitializeLowerbarLabel();

                    InitializeAddMtuForm();

                    RegisterEventHandlers();

                    Popup_start.IsVisible = false;
                    Popup_start.IsEnabled = false;

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

                            })
                         );
                      })
                    );

                    #endregion


                    break;


                case "AddMTUReplaceMeter":

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        #region Set Add MTU Replace Meter Info

                        name_of_window_port1.Text = "Add MTU / Replace Meter";
                        name_of_window_port2.Text = "Add MTU / Replace Meter";
                        name_of_window_misc.Text = "Add MTU / Replace Meter";

                        bg_read_mtu_button_img.Source = "add_mtu_rep_meter_btn.png";

                        #endregion


                        #region Set UI for Add Mtu Replace Meter

                        meterSerialContainer.IsVisible = false;
                        meterSerial2Container.IsVisible = false;

                        if (FormsApp.config.global.MeterWorkRecording)
                        {
                            oldMeterWorkingContainer.IsVisible = true;
                            oldMeterWorkingContainer2.IsVisible = true;
                        }

                        replaceMeterContainer.IsVisible = true;
                        replaceMeterContainer2.IsVisible = true;

                        newMeterContainer.IsVisible = true;
                        newMeterContainer2.IsVisible = true;

                        oldMeterContainer.IsVisible = true;
                        oldMeterContainer2.IsVisible = true;

                        oldReadingContainer.IsVisible = true;
                        oldReadingContainer2.IsVisible = true;


                        meterSerialContainer.IsVisible = false;
                        meterSerial2Container.IsVisible = false;


                        #region NewMeterLabel Validation

                        if (FormsApp.config.global.NewMeterLabel != null)
                        {
                            new_meter_number_port1.Text = FormsApp.config.global.NewMeterLabel;
                            new_meter_number_port2.Text = FormsApp.config.global.NewMeterLabel;
                        }

                        #endregion


                        #endregion

                        #region Color Entry

                        if (FormsApp.config.global.ColorEntry)
                        {
                            old_meter_working_port1.TextColor = Color.FromHex("#FF0000");
                            old_meter_working_port2.TextColor = Color.FromHex("#FF0000");

                            replace_meter_port1.TextColor = Color.FromHex("#FF0000");
                            replace_meter_port2.TextColor = Color.FromHex("#FF0000");

                            new_meter_number_port1.TextColor = Color.FromHex("#FF0000");
                            new_meter_number_port2.TextColor = Color.FromHex("#FF0000");

                            old_reading_port1.TextColor = Color.FromHex("#FF0000");
                            old_reading_port2.TextColor = Color.FromHex("#FF0000");

                            old_meter_number_port1.TextColor = Color.FromHex("#FF0000");
                            old_meter_number_port2.TextColor = Color.FromHex("#FF0000");
                        }

                    });

                    #endregion

                    CallToInitNewUIPickers();

                    #region AddMTU | Replace Meter Case

                    this.add_mtu = new MTUComm.Action(
                        config: FormsApp.config,
                        serial: FormsApp.ble_interface,
                        type: MTUComm.Action.ActionType.AddMtuReplaceMeter,
                        user: FormsApp.credentialsService.UserName);

                    #region Prepare mtuForm

                    this.config = Configuration.GetInstance();

                    // Get detected mtu
                    MTUBasicInfo mtuBasicInfo_addmtureplacemeter = MtuForm.mtuBasicInfo;
                    this.detectedMtuType = (int)mtuBasicInfo_addmtureplacemeter.Type;
                    currentMtu = this.config.mtuTypes.FindByMtuId(this.detectedMtuType);

                    // Initialize logic-form
                    this.addMtuForm = new AddMtuForm(currentMtu);

                    #endregion

                    isCancellable = false;

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
                    if (!string.IsNullOrEmpty(FormsApp.credentialsService.UserName))
                        userName.Text = FormsApp.credentialsService.UserName; //"Kartik";

                    battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
                    rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

                    InitializeLowerbarLabel();

                    InitializeAddMtuForm();

                    RegisterEventHandlers();

                    Popup_start.IsVisible = false;
                    Popup_start.IsEnabled = false;

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

                            })
                         );
                      })
                    );

                    #endregion


                    break;


                case "ReplaceMTUReplaceMeter":

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        #region Set Replace MTU Replace Meter Info

                        name_of_window_port1.Text = "Replace MTU / Replace Meter";
                        name_of_window_port2.Text = "Replace MTU / Replace Meter";
                        name_of_window_misc.Text = "Replace MTU / Replace Meter";

                        bg_read_mtu_button_img.Source = "rep_mtu_rep_meter_btn.png";

                        #endregion


                        #region Set UI for Replace Mtu Replace Meter

                        meterSerialContainer.IsVisible = false;
                        meterSerial2Container.IsVisible = false;

                        if (FormsApp.config.global.MeterWorkRecording)
                        {
                            oldMeterWorkingContainer.IsVisible = true;
                            oldMeterWorkingContainer2.IsVisible = true;
                        }

                        replaceMeterContainer.IsVisible = true;
                        replaceMeterContainer2.IsVisible = true;

                        newMeterContainer.IsVisible = true;
                        newMeterContainer2.IsVisible = true;

                        oldMeterContainer.IsVisible = true;
                        oldMeterContainer2.IsVisible = true;

                        oldReadingContainer.IsVisible = true;
                        oldReadingContainer2.IsVisible = true;

                        oldMtuContainer.IsVisible = true;
                        oldMtuContainer2.IsVisible = true;

                        meterSerialContainer.IsVisible = false;
                        meterSerial2Container.IsVisible = false;


                        #region NewMeterLabel Validation

                        if (FormsApp.config.global.NewMeterLabel != null)
                        {
                            new_meter_number_port1.Text = FormsApp.config.global.NewMeterLabel;
                            new_meter_number_port2.Text = FormsApp.config.global.NewMeterLabel;
                        }

                        #endregion


                        #endregion

                        #region Color Entry

                        if (FormsApp.config.global.ColorEntry)
                        {
                            old_meter_working_port1.TextColor = Color.FromHex("#FF0000");
                            old_meter_working_port2.TextColor = Color.FromHex("#FF0000");

                            replace_meter_port1.TextColor = Color.FromHex("#FF0000");
                            replace_meter_port2.TextColor = Color.FromHex("#FF0000");

                            new_meter_number_port1.TextColor = Color.FromHex("#FF0000");
                            new_meter_number_port2.TextColor = Color.FromHex("#FF0000");

                            old_reading_port1.TextColor = Color.FromHex("#FF0000");
                            old_reading_port2.TextColor = Color.FromHex("#FF0000");

                            old_meter_number_port1.TextColor = Color.FromHex("#FF0000");
                            old_meter_number_port2.TextColor = Color.FromHex("#FF0000");

                            old_mtu_id_port1.TextColor = Color.FromHex("#FF0000");
                            old_mtu_id_port2.TextColor = Color.FromHex("#FF0000");
                        }

                    });

                    #endregion

                    CallToInitNewUIPickers();

                    #region AddMTU | AddMeter Case

                    this.add_mtu = new MTUComm.Action(
                        config: FormsApp.config,
                        serial: FormsApp.ble_interface,
                        type: MTUComm.Action.ActionType.ReplaceMtuReplaceMeter,
                        user: FormsApp.credentialsService.UserName);

                    #region Prepare mtuForm

                    this.config = Configuration.GetInstance();

                    // Get detected mtu
                    MTUBasicInfo mtuBasicInfo_replacemtureplacemeter = MtuForm.mtuBasicInfo;
                    this.detectedMtuType = (int)mtuBasicInfo_replacemtureplacemeter.Type;
                    currentMtu = this.config.mtuTypes.FindByMtuId(this.detectedMtuType);

                    // Initialize logic-form
                    this.addMtuForm = new AddMtuForm(currentMtu);

                    #endregion

                    isCancellable = false;

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
                    if (!string.IsNullOrEmpty(FormsApp.credentialsService.UserName))
                        userName.Text = FormsApp.credentialsService.UserName; //"Kartik";

                    battery_level.Source = CrossSettings.Current.GetValueOrDefault("battery_icon_topbar", "battery_toolbar_high_white");
                    rssi_level.Source = CrossSettings.Current.GetValueOrDefault("rssi_icon_topbar", "rssi_toolbar_high_white");

                    InitializeLowerbarLabel();

                    InitializeAddMtuForm();

                    RegisterEventHandlers();

                    Popup_start.IsVisible = false;
                    Popup_start.IsEnabled = false;

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

                            })
                         );
                      })
                    );

                    #endregion


                    break;

            }


        }

        private void CallToInitNewUIPickers()
        {

            #region HardCoded Picker for Meter working  // Should be global???

            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList = new ObservableCollection<string>();
            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList = new ObservableCollection<PickerItems>
                    {
                        new PickerItems { Name = "Yes" },
                        new PickerItems { Name = "No" },
                        new PickerItems { Name = "Broken" }
                    };

            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */
            foreach (var item in objClassList)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList.Add(item.Name);
            }
            //Now I am given ItemsSorce to the Pickers
            pickerOldMeterWorking.ItemsSource = objStringList;
            pickerOldMeterWorking2.ItemsSource = objStringList;

            #endregion

            #region HardCoded Picker for Replace Metee Register // Should be global???


            //This ObservableCollection later we will assign ItemsSource for Picker.
            ObservableCollection<string> objStringList2 = new ObservableCollection<string>();
            //Mostly below ObservableCollection Items we will get from server but here Iam mentioned static data.
            ObservableCollection<PickerItems> objClassList2 = new ObservableCollection<PickerItems>
                    {
                        new PickerItems { Name = "Meter" },
                        new PickerItems { Name = "Register" },
                        new PickerItems { Name = "Both" }
                    };
            /*Here we have to assign service Items to one ObservableCollection<string>() for this purpose
            I am using foreach and we can add each item to the ObservableCollection<string>(). */
            foreach (var item in objClassList2)
            {
                // Here I am adding each item Name to the ObservableCollection<string>() and below I will assign to the Picker
                objStringList2.Add(item.Name);
            }

            //Now I am given ItemsSorce to the Pickers
            pickerReplaceMeterRegister.ItemsSource = objStringList2;
            pickerReplaceMeterRegister2.ItemsSource = objStringList2;

            #endregion

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
                        meterSerial2Input.Text = meterSerialInput.Text;
                        initialRead2Input.Text = initialReadInput.Text;
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

            Mtu    mtu    = this.addMtuForm.mtu;
            Global global = this.addMtuForm.global;
            MTUBasicInfo mtuBasicinfo = MtuForm.mtuBasicInfo;

            #endregion

            #region Two ports

            bool hasTwoPorts = mtu.TwoPorts;
            port2label.IsVisible = hasTwoPorts;

            #endregion

            #region Service Port ID

            // Hide container if second account number is no needed
            this.servicePortIdInputDualContainer.IsVisible = global.AccountDualEntry;

            #endregion

            #region Field Order ( Work Order )

            bool WorkOrderRecording = global.WorkOrderRecording;

            fieldOrderContainer .IsVisible = WorkOrderRecording;
            fieldOrderContainer .IsEnabled = WorkOrderRecording;
            fieldOrder2Container.IsVisible = hasTwoPorts && WorkOrderRecording;
            fieldOrder2Container.IsEnabled = hasTwoPorts && WorkOrderRecording;

            #endregion

            #region Meter Serial Number

            bool UseMeterSerialNumber = global.UseMeterSerialNumber;

            meterSerialContainer .IsVisible = UseMeterSerialNumber;
            meterSerialContainer .IsEnabled = UseMeterSerialNumber;
            meterSerial2Container.IsVisible = hasTwoPorts && UseMeterSerialNumber;
            meterSerial2Container.IsEnabled = hasTwoPorts && UseMeterSerialNumber;

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

            bool ShowMeterVendor = global.ShowMeterVendor;
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

            List<string> readIntervalList;

            if ( mtuBasicinfo.version >= global.LatestVersion )
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
            
                // TwoWay MTU reading interval cannot be less than 15 minutes
                if ( ! mtu.TimeToSync )
                {
                    readIntervalList.Add ( "10 Min" );
                    readIntervalList.Add ( "5 Min" );
                }
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
                
                // TwoWay MTU reading interval cannot be less than 15 minutes
                if ( ! mtu.TimeToSync )
                {
                    readIntervalList.Add ( "10 Min" );
                    readIntervalList.Add ( "5 Min" );
                }
            }

            readInterval2Container.IsVisible   = hasTwoPorts;
            readInterval2Container.IsEnabled   = hasTwoPorts;
            readIntervalPicker    .ItemsSource = readIntervalList;
            readInterval2Picker   .ItemsSource = readIntervalList;

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
                readIntervalPicker.SelectedIndex = ( ( index > -1 ) ? index : readIntervalList.IndexOf ( "1 Hour" ) );
            }
            // If tag NormXmitInterval is NOT present, use "1 Hour" as default value
            else
                readIntervalPicker.SelectedIndex = readIntervalList.IndexOf ( "1 Hour" );
            
            // Use IndividualReadInterval tag to enable o disable read interval picker
            if ( ! ( this.readIntervalPicker.IsEnabled = global.IndividualReadInterval ) )
            {
              this.readIntervalContainer.BackgroundColor = Color.LightGray;
              this.readIntervalPicker   .BackgroundColor = Color.LightGray;
              this.readIntervalPicker   .TextColor       = Color.Gray;
            }

            #endregion

            #region Snap Reads / Daily Reads

            bool snapReadActive      = global.AllowDailyReads && mtu.DailyReads;
            bool changeableSnapReads = global.IndividualDailyReads;
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

            bool onTimeSync = mtu.OnTimeSync;

            List<string> twoWayList = new List<string> ()
            {
                TWOWAY_FAST,
                TWOWAY_SLOW,
            };
            
            twoWayContainer .IsVisible   = onTimeSync;
            twoWayContainer .IsEnabled   = onTimeSync;
            twoWay2Container.IsVisible   = hasTwoPorts && onTimeSync;
            twoWay2Container.IsEnabled   = hasTwoPorts && onTimeSync;
            twoWayPicker    .ItemsSource = twoWayList;
            twoWay2Picker   .ItemsSource = twoWayList;

            #endregion

            #region Alarms

            alarmsList  = config.alarms.FindByMtuType ( this.detectedMtuType );
            alarms2List = ( hasTwoPorts ) ? config.alarms.FindByMtuType ( this.detectedMtuType ) : new List<Alarm> ();

            // Remove "Scripting" option in interactive mode
            alarmsList  = alarmsList .FindAll ( alarm => ! string.Equals ( alarm.Name.ToLower (), "scripting" ) );
            alarms2List = alarms2List.FindAll ( alarm => ! string.Equals ( alarm.Name.ToLower (), "scripting" ) );

            bool RequiresAlarmProfile = mtu.RequiresAlarmProfile;
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

            // Hide alarms dropdownlist if contains only one option
            alarmsContainer .IsVisible = ( alarmsList .Count > 1 );
            alarms2Container.IsVisible = ( alarms2List.Count > 1 );

            #endregion

            #region Demands

            demandsList  = config.demands.FindByMtuType ( this.detectedMtuType );
            demands2List = ( hasTwoPorts ) ? config.demands.FindByMtuType ( this.detectedMtuType ) : new List<Demand> ();

            bool MtuDemand          = mtu.MtuDemand;
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
            servicePortIdInput    .MaxLength = global.AccountLength;
            servicePortIdInputDual.MaxLength = global.AccountLength;
            fieldOrderInput       .MaxLength = global.WorkOrderLength;
            meterSerialInput      .MaxLength = global.MeterNumberLength;

            #endregion


            #region AccountLabel

            AccountLabel_Port1.Text = global.AccountLabel;
            AccountLabel_Port2.Text = global.AccountLabel;

            #region Mandatory Fields should be labeled with colored field

            if (global.ColorEntry)
            {
                AccountLabel_Port1.TextColor = Color.FromHex("#FF0000");
                AccountLabel_Port2.TextColor = Color.FromHex("#FF0000");

                repeat_serviceportid_port1.TextColor = Color.FromHex("#FF0000");
                repeat_serviceportid_port2.TextColor = Color.FromHex("#FF0000");

                meter_number_port1.TextColor = Color.FromHex("#FF0000");
                meter_number_port2.TextColor = Color.FromHex("#FF0000");


                read_interval_port1.TextColor = Color.FromHex("#FF0000");
                read_interval_port2.TextColor = Color.FromHex("#FF0000");
           


                //Vendor is modified in realtime
            }



            #endregion


            #endregion


            #region WorkOrderLabel

            field_order_port1.Text = global.WorkOrderLabel;
            field_order_port2.Text = global.WorkOrderLabel;

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
            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection

            MenuList = new List<PageItem>();

            // Adding menu items to MenuList

            MenuList.Add(new PageItem() { Title = "Read MTU", Icon = "readmtu_icon.png", TargetType = "ReadMTU" });

            if (FormsApp.config.global.ShowTurnOff)
                MenuList.Add(new PageItem() { Title = "Turn Off MTU", Icon = "turnoff_icon.png", TargetType = "turnOff" });

            if (FormsApp.config.global.ShowAddMTU)
                MenuList.Add(new PageItem() { Title = "Add MTU", Icon = "addMTU.png", TargetType = "AddMTU" });

            if (FormsApp.config.global.ShowReplaceMTU)
                MenuList.Add(new PageItem() { Title = "Replace MTU", Icon = "replaceMTU2.png", TargetType = "replaceMTU" });

            if (FormsApp.config.global.ShowReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Replace Meter", Icon = "replaceMeter.png", TargetType = "replaceMeter" });

            if (FormsApp.config.global.ShowAddMTUMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Add Meter", Icon = "addMTUaddmeter.png", TargetType = "AddMTUAddMeter" });

            if (FormsApp.config.global.ShowAddMTUReplaceMeter)
                MenuList.Add(new PageItem() { Title = "Add MTU / Rep. Meter", Icon = "addMTUrepmeter.png", TargetType = "AddMTUReplaceMeter" });

            if (FormsApp.config.global.ShowReplaceMTUMeter)
                MenuList.Add(new PageItem() { Title = "Rep.MTU / Rep. Meter", Icon = "repMTUrepmeter.png", TargetType = "ReplaceMTUReplaceMeter" });

            if (FormsApp.config.global.ShowInstallConfirmation)
                MenuList.Add(new PageItem() { Title = "Install Confirmation", Icon = "installConfirm.png", TargetType = "InstallConfirm" });



            // ListView needs to be at least  elements for UI Purposes, even empty ones
            while (MenuList.Count < 9)
                MenuList.Add(new PageItem() { Title = "", Icon = "", TargetType = "" });

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
            logout_button.Tapped += LogoutTapped;
            
            settings_button.Tapped += OpenSettingsCallAsync;
            back_button.Tapped += ReturnToMainView;
            bg_read_mtu_button.Tapped += AddMtu;

            turnoffmtu_ok.Tapped += TurnOffMTUOkTapped;
            turnoffmtu_no.Tapped += TurnOffMTUNoTapped;
            turnoffmtu_ok_close.Tapped += TurnOffMTUCloseTapped;

            replacemeter_ok.Tapped += ReplaceMtuOkTapped;
            replacemeter_cancel.Tapped += ReplaceMtuCancelTapped;

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

            gps_icon_button.Tapped += GpsUpdateButton;


            logoff_no.Tapped += LogOffNoTapped;
            logoff_ok.Tapped += LogOffOkTapped;

            submit_dialog.Clicked += submit_send;
            cancel_dialog.Clicked += CancelTapped;


            dialog_AddMTUAddMeter_ok.Tapped += dialog_AddMTUAddMeter_okTapped;
            dialog_AddMTUAddMeter_cancel.Tapped += dialog_AddMTUAddMeter_cancelTapped;

            dialog_AddMTUReplaceMeter_ok.Tapped += dialog_AddMTUReplaceMeter_okTapped;
            dialog_AddMTUReplaceMeter_cancel.Tapped += dialog_AddMTUReplaceMeter_cancelTapped;

            dialog_ReplaceMTUReplaceMeter_ok.Tapped += dialog_ReplaceMTUReplaceMeter_okTapped;
            dialog_ReplaceMTUReplaceMeter_cancel.Tapped += dialog_ReplaceMTUReplaceMeter_cancelTapped;


            dialog_AddMTU_ok.Tapped += dialog_AddMTU_okTapped;
            dialog_AddMTU_cancel.Tapped += dialog_AddMTU_cancelTapped;


            if (Device.Idiom == TargetIdiom.Tablet)
            {
                hamburger_icon_home.IsVisible = true;
                back_button_home.Tapped += TapToHome_Tabletmode;
            }
    

        }

        private void TapToHome_Tabletmode(object sender, EventArgs e)
        {

            int contador = Navigation.NavigationStack.Count;

            while (contador > 2)
            {
                try
                {
                    Navigation.PopAsync(false);
                }
                catch (Exception v)
                {
                    Console.WriteLine(v.StackTrace);
                }
                contador--;
            }


        }

        void dialog_AddMTUAddMeter_cancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_AddMTUAddMeter.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void dialog_AddMTUAddMeter_okTapped(object sender, EventArgs e)
        {
            dialog_AddMTUAddMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "AddMTUAddMeter";
                Task.Factory.StartNew(BasicReadThread);
            });


        }

        void dialog_AddMTUReplaceMeter_cancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_AddMTUReplaceMeter.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void dialog_AddMTUReplaceMeter_okTapped(object sender, EventArgs e)
        {
            dialog_AddMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "AddMTUReplaceMeter";
                Task.Factory.StartNew(BasicReadThread);
            });

        }

        void dialog_ReplaceMTUReplaceMeter_cancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_ReplaceMTUReplaceMeter.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void dialog_ReplaceMTUReplaceMeter_okTapped(object sender, EventArgs e)
        {
            dialog_ReplaceMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "ReplaceMTUReplaceMeter";
                Task.Factory.StartNew(BasicReadThread);
            });


        }

        void dialog_AddMTU_cancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            dialog_AddMTU.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        void dialog_AddMTU_okTapped(object sender, EventArgs e)
        {
            dialog_AddMTU.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                // TODO: cambiar usuario
                // TODO: BasicRead no loguea
                try
                {
                    page_to_controller = "AddMTU";
                    Task.Factory.StartNew(BasicReadThread);
                }
                catch (Exception addmtu)
                {
                    Console.WriteLine(addmtu.StackTrace);
                }

            });

            //Bug fix Android UI Animation

        }


        private void CancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;
        }

        private void LogOffOkTapped(object sender, EventArgs e)
        {


            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                //REASON
                isLogout = true;
                dialog_open_bg.IsVisible = true;
                Popup_start.IsVisible = true;
                Popup_start.IsEnabled = true;
            });
           
        }

        private void LogOffNoTapped(object sender, EventArgs e)
        {
            dialog_logoff.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
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

            #region Color Entry

            if (FormsApp.config.global.ColorEntry)
            {
                meterVendorsLabel.TextColor = Color.FromHex("#FF0000");
       
            }

            #endregion

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


            #region Color Entry

            if (FormsApp.config.global.ColorEntry)
            {
                meterModelsLabel.TextColor = Color.FromHex("#FF0000");

            }

            #endregion


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


            #region Color Entry

            if (FormsApp.config.global.ColorEntry)
            {
                meterNamesLabel.TextColor = Color.FromHex("#FF0000");

            }

            #endregion


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


            #region Color Entry

            if (FormsApp.config.global.ColorEntry)
            {
                meterVendors2Label.TextColor = Color.FromHex("#FF0000");

            }

            #endregion

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


            #region Color Entry

            if (FormsApp.config.global.ColorEntry)
            {
                meterModels2Label.TextColor = Color.FromHex("#FF0000");

            }

            #endregion


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


            #region Color Entry

            if (FormsApp.config.global.ColorEntry)
            {
                meterNames2Label.TextColor = Color.FromHex("#FF0000");

            }

            #endregion


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
            shadoweffect.Source = "shadow_effect_tablet";
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
            this.SetMeterVendor ( ((BorderlessPicker)sender).SelectedIndex );
        }

        private void SetMeterVendor ( int selectedIndex )
        {
            vendor = meterVendorsList [ selectedIndex ];
            meterModelsList = this.config.meterTypes.GetModelsByVendorFromMeters(meters, vendor);
            name = "";

            try
            {
                meterModelsPicker.ItemsSource = meterModelsList;
                meterModelsContainerA.IsVisible = true;
                meterNamesContainerA.IsVisible = false;
            }
            catch ( Exception e )
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
            this.SetMeterModel ( ((BorderlessPicker)sender).SelectedIndex );
        }

        private void SetMeterModel ( int selectedIndex )
        {
            meterNamesPicker.ItemDisplayBinding = new Binding("Display");

            model = meterModelsList[ selectedIndex ];

            List<Meter> meterlist = this.config.meterTypes.GetMetersByModelAndVendorFromMeters(meters, vendor, model);

            try
            {
                meterNamesPicker.ItemsSource = meterlist;
                meterModelsContainerA.IsVisible = true;
                meterNamesContainerA.IsVisible = true;
            }
            catch ( Exception e )
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

        object menu_sender;
        ItemTappedEventArgs menu_tappedevents;

        private void OnItemSelected(Object sender, SelectedItemChangedEventArgs e)
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
                        String page = item.TargetType;

                        ((ListView)sender).SelectedItem = null;

                        switch (page)
                        {
                            case "ReadMTU":
                                if(!page_to_controller.Equals(page))
                                    NavigationController("ReadMTU");
                                //OnCaseReadMTU();
                                break;

                            case "AddMTU":
                                if (!page_to_controller.Equals(page))
                                    NavigationController("AddMTU");
                                //OnCaseAddMTU();
                                break;

                            case "turnOff":
                                if (!page_to_controller.Equals(page))
                                    NavigationController("turnOff");
                                //OnCaseTurnOff();
                                break;

                            case "InstallConfirm":
                                if (!page_to_controller.Equals(page))
                                    NavigationController("InstallConfirm");
                                //OnCaseInstallConfirm();
                                break;

                            case "replaceMTU":
                                if (!page_to_controller.Equals(page))
                                    NavigationController("replaceMTU");
                                //OnCaseReplaceMTU();
                                break;

                            case "replaceMeter":
                                if (!page_to_controller.Equals(page))
                                    //Application.Current.MainPage.DisplayAlert("Alert", "Feature not available", "Ok");
                                    NavigationController("replaceMeter");
                                //OnCaseReplaceMeter();
                                break;

                            case "AddMTUAddMeter":
                                if (!page_to_controller.Equals(page))
                                    //Application.Current.MainPage.DisplayAlert("Alert", "Feature not available", "Ok");
                                    NavigationController("AddMTUAddMeter");
                                //OnCaseAddMTUAddMeter();
                                break;

                            case "AddMTUReplaceMeter":
                                if (!page_to_controller.Equals(page))
                                    //Application.Current.MainPage.DisplayAlert("Alert", "Feature not available", "Ok");
                                    NavigationController("AddMTUReplaceMeter");
                                //OnCaseAddMTUReplaceMeter();
                                break;

                            case "ReplaceMTUReplaceMeter":
                                if (!page_to_controller.Equals(page))
                                    //Application.Current.MainPage.DisplayAlert("Alert", "Feature not available", "Ok");
                                    NavigationController("ReplaceMTUReplaceMeter");
                                //OnCaseReplaceMTUReplaceMeter();
                                break;

                            default:

                                if (!isCancellable)
                                { 
                                    //REASON
                                    dialog_open_bg.IsVisible = true;

                                    Popup_start.IsVisible = true;
                                    Popup_start.IsEnabled = true;
                                }

                                break;

                        }
                    }
                    catch (Exception w1)
                    {
                        Console.WriteLine(w1.StackTrace);
                    }
                }

               
            }
        }


        private void NavigationController(string page)
        {
            page_to_controller = page;

            if (!isCancellable)
            {
                //REASON
                dialog_open_bg.IsVisible = true;

                Popup_start.IsVisible = true;
                Popup_start.IsEnabled = true;

            }
            else
            {
                SwitchToControler(page);

            }

        }


        private void SwitchToControler(string page)
        {
            switch (page)
            {
                case "ReadMTU":

                    #region New Circular Progress bar Animations    

             
                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;

                    #endregion


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

                            #region New Circular Progress bar Animations    

                  
                            backdark_bg.IsVisible = false;
                            indicator.IsVisible = false;

                            #endregion
                        })
                    );

                    #endregion

                    break;

                case "AddMTU":

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

                                if (FormsApp.config.global.ActionVerify)
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

                case "turnOff":

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

                                if (FormsApp.config.global.ActionVerify)
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

                case "InstallConfirm":

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

                case "replaceMTU":

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

                                if (FormsApp.config.global.ActionVerify)
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

                case "replaceMeter":

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

                                if (FormsApp.config.global.ActionVerify)
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

                case "AddMTUAddMeter":

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

                                if (FormsApp.config.global.ActionVerify)
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

                case "AddMTUReplaceMeter":

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

                                if (FormsApp.config.global.ActionVerify)
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

                case "ReplaceMTUReplaceMeter":

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

                                if (FormsApp.config.global.ActionVerify)
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

            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "ReplaceMTUReplaceMeter";
                Task.Factory.StartNew(BasicReadThread);
            });

        }

        private void CallLoadViewAddMTUReplaceMeter()
        {
            dialog_AddMTUReplaceMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "AddMTUReplaceMeter";
                Task.Factory.StartNew(BasicReadThread);
            });
        }

        private void CallLoadViewAddMTUAddMeter()
        {

            dialog_AddMTUAddMeter.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "AddMTUAddMeter";
                Task.Factory.StartNew(BasicReadThread);
            });

        }

        private void CallLoadViewReplaceMeter()
        {
            dialog_meter_replace_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;

            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "replaceMeter";
                Task.Factory.StartNew(BasicReadThread);
            });
        }

        private void CallLoadViewReplaceMtu()
        {
            dialog_replacemeter_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;


            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "replaceMTU";
                Task.Factory.StartNew(BasicReadThread);
            });

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

            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "AddMTU";
                Task.Factory.StartNew(BasicReadThread);
            });
        }

        void BasicReadThread()
        {
            MTUComm.Action basicRead = new MTUComm.Action(
               config: FormsApp.config,
               serial: FormsApp.ble_interface,
               type: MTUComm.Action.ActionType.BasicRead,
               user: FormsApp.credentialsService.UserName);

            /*
            basicRead.OnFinish += ((s, args) =>
            { });
            */

            basicRead.OnFinish += ((s, e) =>
            {
                Task.Delay(100).ContinueWith(t =>
                    Device.BeginInvokeOnMainThread(() =>
                    {

                        Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved, page_to_controller), false);

                        #region New Circular Progress bar Animations    

                        backdark_bg.IsVisible = false;
                        indicator.IsVisible = false;
                        background_scan_page.IsEnabled = true;

                        #endregion

                    })
                );
            });

            basicRead.OnError += ((s, e) =>
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

                            Application.Current.MainPage.DisplayAlert("Alert", "Cannot read device, try again", "Ok");

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


        private void OnCaseInstallConfirm()
        {
            background_scan_page.Opacity = 1;
            //background_scan_page_detail.Opacity = 1;
            background_scan_page.IsEnabled = true;
            //background_scan_page_detail.IsEnabled = true;

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
                //background_scan_page_detail.Opacity = 1;
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
                 Application.Current.MainPage.Navigation.PushAsync(new AclaraViewAddMTU(dialogsSaved, "AddMTU"), false);
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
                    Console.WriteLine(i2.StackTrace);
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


            /*
            Settings.IsLoggedIn = false;
            FormsApp.credentialsService.DeleteCredentials();

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
            */

        }


        private void ReplaceMtuCancelTapped(object sender, EventArgs e)
        {
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;
        }

        private void ReplaceMtuOkTapped(object sender, EventArgs e)
        {
            dialog_replacemeter_one.IsVisible = false;
            dialog_open_bg.IsVisible = false;
            turnoff_mtu_background.IsVisible = false;


            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "replaceMTU";
                Task.Factory.StartNew(BasicReadThread);
            });

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


            Task.Factory.StartNew(TurnOffMethod);
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

            turnOffAction.OnError += ((s, args) =>
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

            Device.BeginInvokeOnMainThread(() =>
            {
                page_to_controller = "replaceMeter";
                Task.Factory.StartNew(BasicReadThread);
            });


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

        private void submit_send(object sender, EventArgs e3)
        {
            int selectedCancelReasonIndex = cancelReasonPicker.SelectedIndex;
            string selectedCancelReason = "Other";

            if ( selectedCancelReasonIndex > -1 )
                selectedCancelReason = cancelReasonPicker.Items[cancelReasonPicker.SelectedIndex];

            this.add_mtu.Cancel ( selectedCancelReason );

            dialog_open_bg.IsVisible = false;
            Popup_start.IsVisible = false;
            Popup_start.IsEnabled = false;

            if (Device.Idiom == TargetIdiom.Tablet && !isLogout)
            {
                SwitchToControler(page_to_controller);
            }
            else
            {
                #region I guess it's logout time ...

                Task.Run(async () =>
                {
                    await Task.Delay(500); Device.BeginInvokeOnMainThread(() =>
                    {
                        //Application.Current.MainPage.Navigation.PopAsync(false);

                        //Application.Current.MainPage.Navigation.PopAsync(false);0
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
                                Settings.IsLoggedIn = false;

                                try
                                {
                                    FormsApp.credentialsService.DeleteCredentials();
                                    FormsApp.ble_interface.Close();
                                }
                                catch (Exception e25)
                                {
                                    Console.WriteLine(e25.StackTrace);
                                }
                              

                                background_scan_page.IsEnabled = true;


                                Navigation.PopToRootAsync(false);


                            }
                            else
                            {
                                Navigation.PopAsync();
                            }

                        }
                    });
                });
            }

            #endregion



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

        private bool ValidateFields ( ref string msgError )
        {
            Global global = this.config.GetGlobal ();

            // Value equals to maximum length
            dynamic NoValEq = new Func<string,int,bool> ( ( value, maxLength ) =>
                                ! Validations.NumericText ( value, maxLength ) );

            // Value equals or lower to maximum length
            dynamic NoValEL = new Func<string,int,bool> ( ( value, maxLength ) =>
                                ! Validations.NumericText ( value, maxLength, 1, true, true, false ) );

            #region Port 1

            bool okSP =                                     NoValEq ( servicePortIdInput.Text, global.AccountLength           );
            bool okSD = global.AccountDualEntry          && NoValEq ( servicePortIdInputDual.Text, global.AccountLength       );
            bool okFO = fieldOrderContainer   .IsVisible && NoValEL ( fieldOrderInput   .Text, global.WorkOrderLength         );
            bool okMS = meterSerialContainer  .IsVisible && NoValEL ( meterSerialInput  .Text, global.MeterNumberLength       );
            bool okIR =                                     NoValEq ( initialReadInput  .Text, initialReadInput    .MaxLength );
            bool okSR = snapReadsContainer    .IsVisible && NoValEL ( snapReadsLabel    .Text, (int)snapReadsSlider.Maximum   ) && snapRead1Status;
            bool okRI = readIntervalContainer .IsVisible && readIntervalPicker.SelectedIndex <= -1;
            bool okMN = meterVendorsContainerA.IsVisible && meterNamesPicker  .SelectedIndex <= -1;
            bool okTW = twoWayContainer       .IsVisible && twoWayPicker      .SelectedIndex <= -1;
            bool okAL = alarmsContainer       .IsVisible && alarmsPicker      .SelectedIndex <= -1;
            bool okDM = demandsContainer      .IsVisible && demandsPicker     .SelectedIndex <= -1;
           
            if      ( okSP ) msgError = "Service Port ID" + ( ( global.AccountDualEntry ) ? " ( First entry )" : string.Empty );
            else if ( okSD && ! DEBUG_AUTO_MODE_ON ) msgError = "Service Port ID ( Second entry )";
            else if ( okFO ) msgError = "Field Order";
            else if ( okMS ) msgError = "Meter Serial Number";
            else if ( okMN ) msgError = "Meter Type";
            else if ( okIR ) msgError = "Initial Read";
            else if ( okRI ) msgError = "Read Interval";
            else if ( okSR ) msgError = "Snap Reads";
            else if ( okTW ) msgError = "2-Way";
            else if ( okAL ) msgError = "Alarms";
            else if ( okDM ) msgError = "Demands";

            if ( okSP || okFO || okMS || okIR || okSR || okRI || okMN || okTW || okAL || okDM )
                return false;

            // If Global.AccountDualEntry is true, two ServicePortId entries have to be equal
            if ( ! DEBUG_AUTO_MODE_ON    &&
                 global.AccountDualEntry &&
                 ! string.Equals ( servicePortIdInput.Text, servicePortIdInputDual.Text ) )
            {
                msgError = "Service Port ID entries are not the same";
                return false;
            }

            #endregion

            #region Port 2

            /*
            if ( port2enabled &&
                                                      NoVal ( servicePortId2Input.Text, servicePortId2Input  .MaxLength ) ||
                 fieldOrder2Container   .IsVisible && NoVal ( fieldOrder2Input   .Text, fieldOrder2Input     .MaxLength ) ||
                 meterSerial2Container  .IsVisible && NoVal ( meterSerial2Input  .Text, meterSerial2Input    .MaxLength ) ||
                                                      NoVal ( initialRead2Input  .Text, initialRead2Input    .MaxLength ) ||
                 snapReads2Container    .IsVisible && NoVal ( snapReads2Label    .Text, (int)snapReads2Slider.Maximum   ) && snapRead2Status ||
                 readInterval2Container .IsVisible && readInterval2Picker.SelectedIndex <= -1 ||
                 meterVendors2ContainerA.IsVisible && meterNames2Picker  .SelectedIndex <= -1 ||
                 twoWay2Container       .IsVisible && twoWay2Picker      .SelectedIndex <= -1 ||
                 alarms2Container       .IsVisible && alarms2Picker      .SelectedIndex <= -1 ||
                 demands2Container      .IsVisible && demands2Picker     .SelectedIndex <= -1 )
                return false;
            */

            #endregion

            dynamic NoValNOrEmpty = new Func<string,bool> ( ( value ) =>
                                        ! string.IsNullOrEmpty ( value ) &&
                                        ! Validations.IsNumeric ( value ) );

            #region Miscelanea

            if ( NoValNOrEmpty ( mtuGeolocationLat .Text ) ||
                 NoValNOrEmpty ( mtuGeolocationLong.Text ) )
            {
                msgError = "GPS Coordinates";
                return false;
            }

            //foreach ( BorderlessPicker picker in optionalPickers )
            //    if ( picker.SelectedIndex <= -1 )
            //        return false;

            //foreach ( BorderlessEntry entry in optionalEntries )
            //    if ( string.IsNullOrEmpty ( entry.Text ) )
            //        return false;

            #endregion

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

            if(FormsApp.config.global.NewMeterPort2isTheSame)
                this.copyPort1.IsVisible = true;
            else
                this.copyPort1.IsVisible = false;


            if(FormsApp.config.global.Port2DisableNo)
            {
                enablePort2.IsVisible = false;

                this.port2status = !this.port2status;

                bool ok = this.add_mtu.comm.WriteMtuBitAndVerify(28, 1, port2status);
                Console.WriteLine("-> UPDATE PORT 2 STATUS: " + ok + " " + this.port2status);

                // Bit correctly modified
                if (ok)
                    this.UpdateStatusPort2();

                // Bit have not changed -> return to previous state
                else
                    this.port2status = !this.port2status;
            }
        
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

        private void AddMtu ( object sender, EventArgs e )
        {
            string msgError = string.Empty;
            if ( ! DEBUG_AUTO_MODE_ON &&
                 ! this.ValidateFields ( ref msgError ) )
            {
                DisplayAlert ( "Error", "Mandatory '" + msgError + "' field is incorrectly filled", "OK" );
                return;
            }

            isCancellable = true;

            if (!_userTapped)
            {
                //Task.Delay(100).ContinueWith(t =>

                Device.BeginInvokeOnMainThread(() =>
                {
                    // DEBUG
                    if ( DEBUG_AUTO_MODE_ON )
                    {
                        this.SetMeterVendor ( DEBUG_VENDOR_INDEX );
                        this.SetMeterModel  ( DEBUG_MODEL_INDEX  );
                    }

                    backdark_bg.IsVisible = true;
                    indicator.IsVisible = true;
                    _userTapped = true;
                    background_scan_page.IsEnabled = false;
                    ChangeLowerButtonImage(true);

                    #region comment
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
                    #endregion

                    Task.Factory.StartNew(AddMtu_Action);
                    //Task.Run ( async () => await AddMtu_Action () );
                    //AddMtu_Action();
                });
            }
        }

        private void AddMtu_Action ()
        {
            #region Get Values from Form

            Mtu    mtu    = this.addMtuForm.mtu;
            Global global = this.addMtuForm.global;

            string value_spi;
            string value_fo;
            string value_msn;
            string value_ir;
            string value_srs;
            string value_tw;
            string value_ri;
            Meter  value_mtr;
            Alarm  value_alr = null;
            Demand value_dmd;
            string value_lat;
            string value_lon;
            string value_alt;

            // DEBUG
            if ( DEBUG_AUTO_MODE_ON )
            {
                value_spi = DEBUG_SERVICEPORTID;
                value_fo  = DEBUG_FIELDORDER;
                value_msn = DEBUG_METERSERIAL;
                value_ir  = DEBUG_INITREADING;
                value_srs = DEBUG_SNAPSREADS;
                //value_tw  = (string)twoWayPicker.ItemsSource[ 0 ];
                value_ri  = DEBUG_READSINTERVAL;
                value_mtr = (Meter)meterNamesPicker.ItemsSource[ DEBUG_MTRNAME_INDEX ];
                value_alr = (Alarm)alarmsPicker    .ItemsSource[ DEBUG_ALARM_INDEX   ];
                //value_dmd = (Demand)demandsPicker  .ItemsSource[ DEBUG_DEMAND_INDEX  ];
                value_lat = DEBUG_GPS_LAT;
                value_lon = DEBUG_GPS_LON;
                value_alt = DEBUG_GPS_ALT;
            }
            else
            {
                // Real values
                value_spi = servicePortIdInput.Text;
                value_fo  = fieldOrderInput.Text;
                value_msn = meterSerialInput.Text;
                value_ir  = initialReadInput.Text;
                value_srs = snapReadsSlider.Value.ToString();
                //value_tw  = twoWayPicker.SelectedItem.ToString();
                value_ri  = readIntervalPicker.SelectedItem.ToString();
                value_mtr = ( Meter )meterNamesPicker.SelectedItem;
                //value_dmd = ( Demand )demandsPicker.SelectedItem;
                value_lat = mtuGeolocationLat .Text;
                value_lon = mtuGeolocationLong.Text;
                value_alt = altitude;

                // Alarms dropdownlist is hidden when only has one option
                if ( alarmsPicker.ItemsSource.Count == 1 )
                    value_alr = ( Alarm )alarmsPicker.ItemsSource[ 0 ];
                else if ( alarmsPicker.ItemsSource.Count > 1 )
                    value_alr = ( Alarm )alarmsPicker.SelectedItem;
            }

            #endregion

            #region Set parameters Port 1

            // Service Port ID
            this.addMtuForm.AddParameter ( FIELD.SERVICE_PORT_ID, value_spi );

            // Field Order [ SOLO SE LOGEA ¿? ]
            if ( global.WorkOrderRecording )
                this.addMtuForm.AddParameter ( FIELD.FIELD_ORDER, value_fo );

            // Meter Number [ SOLO SE LOGEA ¿? ]
            if ( global.UseMeterSerialNumber )
                this.addMtuForm.AddParameter ( FIELD.METER_NUMBER, value_msn );

            // Initial Reading
            this.addMtuForm.AddParameter ( FIELD.INITIAL_READING, value_ir );

            // Selected Meter ID
            this.addMtuForm.AddParameter ( FIELD.SELECTED_METER, value_mtr );

            // Read Interval
            if ( global.IndividualReadInterval )
                this.addMtuForm.AddParameter ( FIELD.READ_INTERVAL, value_ri );

            // Snap Reads [ SOLO SE LOGEA ¿? ]
            if ( ( DEBUG_AUTO_MODE_ON && DEBUG_SNAPSREADS_OK || ! DEBUG_AUTO_MODE_ON ) &&
                 global.AllowDailyReads &&
                 mtu.DailyReads )
                this.addMtuForm.AddParameter ( FIELD.SNAP_READS, value_srs );

            // 2-Way [ SOLO SE LOGEA ¿? ]
            //if ( mtu.OnTimeSync ) // Is a two-way MTU and forces time sync ( InstallConfirmation )
            //    this.addMtuForm.AddParameter ( FIELD.TWO_WAY,  );

            // Alarms
            if ( value_alr != null &&
                 mtu.RequiresAlarmProfile )
                this.addMtuForm.AddParameter ( FIELD.ALARM, value_alr );

            // Demands [ SOLO SE LOGEA ¿? ]
            //if ( MtuConditions.MtuDemand )
            //    this.addMtuForm.AddParameter ( FIELD.DEMAND, value_dmd );

            #endregion

            #region Set parameters Port 2

            if ( mtu.TwoPorts )
            {
                // Service Port ID 2
                this.addMtuForm.AddParameter ( FIELD.SERVICE_PORT_ID2, servicePortId2Input.Text );

                // Field Order 2
                if ( global.WorkOrderRecording )
                    this.addMtuForm.AddParameter ( FIELD.FIELD_ORDER2, fieldOrder2Input.Text );

                // Meter Number 2
                if ( global.UseMeterSerialNumber )
                    this.addMtuForm.AddParameter ( FIELD.METER_NUMBER2, meterSerial2Input.Text );

                // Initial Reading 2
                this.addMtuForm.AddParameter ( FIELD.INITIAL_READING2, initialRead2Input.Text );

                // Read Interval 2
                if ( global.IndividualReadInterval )
                    this.addMtuForm.AddParameter ( FIELD.READ_INTERVAL2, readInterval2Picker.SelectedItem.ToString() );

                // Selected Meter ID 2
                this.addMtuForm.AddParameter ( FIELD.SELECTED_METER2, ( Meter )meterNames2Picker.SelectedItem );

                // Snap Reads 2
                if ( global.AllowDailyReads && mtu.DailyReads )
                    this.addMtuForm.AddParameter ( FIELD.SNAP_READS2, snapReads2Slider.Value.ToString() );

                // 2-Way 2
                //if ( mtu.OnTimeSync )
                //    this.addMtuForm.AddParameter ( FIELD.TWO_WAY2, twoWay2Picker.SelectedItem.ToString() );

                // Alarms 2
                if ( mtu.RequiresAlarmProfile )
                    this.addMtuForm.AddParameter ( FIELD.ALARM2, ( Alarm )alarms2Picker.SelectedItem );

                // Demands 2
                if ( mtu.MtuDemand )
                    this.addMtuForm.AddParameter ( FIELD.DEMAND2, ( Demand )demands2Picker.SelectedItem );
            }

            #endregion

            #region Set Optional parameters

            // Gps
            if ( ! string.IsNullOrEmpty ( value_lat ) &&
                 ! string.IsNullOrEmpty ( value_lon ) )
            {
                double lat    = Convert.ToDouble ( value_lat );
                double lon    = Convert.ToDouble ( value_lon );
                //string latDir = ( lat < 0d ) ? "S" : "N";
                //string lonDir = ( lon < 0d ) ? "W" : "E";

                this.addMtuForm.AddParameter ( FIELD.GPS_LATITUDE,  lat );
                this.addMtuForm.AddParameter ( FIELD.GPS_LONGITUDE, lon );
                this.addMtuForm.AddParameter ( FIELD.GPS_ALTITUDE,  value_alt );
            }

            List<Parameter> optionalParams = new List<Parameter>();

            foreach ( BorderlessPicker p in optionalPickers )
                if ( p.SelectedItem != null )
                    optionalParams.Add ( new Parameter ( p.Name, p.Display, p.SelectedItem, true ) );

            foreach ( BorderlessEntry e in optionalEntries )
                if ( ! string.IsNullOrEmpty ( e.Text ) )
                    optionalParams.Add ( new Parameter ( e.Name, e.Display, e.Text, true ) );

            if ( optionalParams.Count > 0 )
                this.addMtuForm.AddParameter ( FIELD.OPTIONAL_PARAMS, optionalParams );

            #endregion

            #region OnProgress

            this.add_mtu.OnProgress += ( ( s, e ) =>
            {
                string mensaje = e.Message;

                Device.BeginInvokeOnMainThread ( () =>
                {
                    if ( ! string.IsNullOrEmpty ( mensaje ) )
                        label_read.Text = mensaje;
                });
            });

            #endregion

            #region OnFinish

            this.add_mtu.OnFinish += ( ( s, e ) =>
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

                InterfaceParameters[] interfacesParams = FormsApp.config.getUserInterfaceFields(mtu_type, "ReadMTU");

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
                                Parameter param = null;

                                // Port header
                                if (pParameter.Name.Equals("Description"))
                                {
                                    param = ports[i].getParameterByTag(pParameter.Name);

                                    FinalReadListView.Add(new ReadMTUItem()
                                    {
                                        Title = "Here lies the Port title...",
                                        isDisplayed = "true",
                                        Height = "40",
                                        isMTU = "false",
                                        isMeter = "true",
                                        Description = "Port " + ( i + 1 ) + ": " + param.Value
                                    });
                                }
                                // Port fields
                                else
                                {
                                    if ( ! string.IsNullOrEmpty ( pParameter.Source ) &&
                                         pParameter.Source.Contains ( "." ) )
                                    {
                                        string tag = pParameter.Source.Split(new char[] { '.' })[ 1 ];
                                        param = ports[ i ].getParameterByTag ( tag );
                                    }

                                    if ( param == null )
                                        param = e.Result.getParameterByTag ( pParameter.Name );

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
                        Parameter param = null;
                       
                        if ( ! string.IsNullOrEmpty ( iParameter.Source ) &&
                             iParameter.Source.Contains ( "." ) )
                        {
                            string tag = iParameter.Source.Split(new char[] { '.' })[ 1 ];
                            param = e.Result.getParameterByTag ( tag );
                        }

                        if ( param == null )
                            param = e.Result.getParameterByTag ( iParameter.Name );

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

                   #region Hide button

                    bg_read_mtu_button_img.IsEnabled = false;
                    bg_read_mtu_button_img.Opacity = 0;

                    #endregion


                }));
            });

            #endregion

            #region OnError

            this.add_mtu.OnError += ( ( s, e ) =>
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
            add_mtu.Run ( this.addMtuForm );
        }

        #endregion

        #region Location

        private void GpsUpdateButton ( object sender, EventArgs e )
        {
            if ( IsLocationAvailable () )
                Task.Run(async () => { await GpsStartListening(); });
        }

        public bool IsLocationAvailable ()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            CrossGeolocator.Current.DesiredAccuracy = 5;


            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

        async Task GpsStartListening ()
        {
            if (CrossGeolocator.Current.IsListening)
                return;
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 1, true);
            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
            await Task.Delay(5000).ContinueWith(t => GpsStopListening());
        }

        private void PositionChanged ( object sender, PositionEventArgs e )
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

            mtuGeolocationLat .Text = position.Latitude .ToString ();
            mtuGeolocationLong.Text = position.Longitude.ToString ();
            this.altitude           = position.Altitude .ToString ();
        }

        private void PositionError ( object sender, PositionErrorEventArgs e )
        {
            Console.WriteLine(e.Error);
        }

        private async Task GpsStopListening ()
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
            switch (page_to_controller)
            {
                case "AddMTU":

                    if (v)
                    {
                        bg_read_mtu_button_img.Source = "add_mtu_btn_black.png";

                    }
                    else
                    {
                        bg_read_mtu_button_img.Source = "add_mtu_btn.png";
                    }
                    break;

                case "replaceMTU":

                    if (v)
                    {
                        bg_read_mtu_button_img.Source = "rep_mtu_btn_black.png";

                    }
                    else
                    {
                        bg_read_mtu_button_img.Source = "rep_mtu_btn.png";
                    }
                    break;

                case "replaceMeter":

                    if (v)
                    {
                        bg_read_mtu_button_img.Source = "rep_meter_btn_black.png";
                    }
                    else
                    {
                        bg_read_mtu_button_img.Source = "rep_meter_btn.png";
                    }

                    break;

                case "AddMTUAddMeter":

                    if (v)
                    {
                        bg_read_mtu_button_img.Source = "add_mtu_meter_btn_black.png";
                    }
                    else
                    {
                        bg_read_mtu_button_img.Source = "add_mtu_meter_btn.png";
                    }

                    break;

                case "AddMTUReplaceMeter":

                    if (v)
                    {
                        bg_read_mtu_button_img.Source = "add_mtu_rep_meter_btn_black.png";
                    }
                    else
                    {
                        bg_read_mtu_button_img.Source = "add_mtu_rep_meter_btn.png";
                    }

                    break;

                case "ReplaceMTUReplaceMeter":

                    if (v)
                    {
                        bg_read_mtu_button_img.Source = "rep_mtu_rep_meter_btn_black.png";
                    }
                    else
                    {
                        bg_read_mtu_button_img.Source = "rep_mtu_rep_meter_btn.png";
                    }

                    break;

            }

        }

        private void PickerSelection(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
        }
        private void PickerSelection2(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
        }


        #endregion
    }
}
