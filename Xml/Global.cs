using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("Globals")]
    public class Global
    {
        public Global ()
        {
            // Default values extracted from Konstantin code
            this.AccountDualEntry           = true; // Should the account number require dual entry?
            this.AccountEnabledAppt         = false; // Enable or disable values after appointment in interactive mode ( Account Number )
            this.WorkOrderEnabledAppt       = false; // Enable or disable values after appointment in interactive mode ( Work Order )
            this.MeterSerialEnabledAppt     = false; //  Enable or disable values after appointment in interactive mode ( Meter Old/Serial Number )
            //this.AccountFilled            = NOT PRESENT IN K.CODE
            this.AccountLabel               = "Account #"; // The account number label
            this.AccountLength              = 7; // Account number length
            this.ActionVerify               = true; // Pop up the Message box after Add MTU
            this.AddressVerify              = false; // Appointment verify address
            this.AddressLine1               = string.Empty; // First field of verification
            this.AddressLine2               = string.Empty; // Second...
            this.AddressLine3               = string.Empty; // Third...
            this.AddressLine4               = string.Empty; // Fourth...
            this.Address1Len                = 100; // Works with above
            this.Address2Len                = 100; // Works...
            this.Address3Len                = 100; // Works...
            this.Address4Len                = 100; // Works...
            this.AFC                        = true; // Register Changes indicator change for 3000 MTUs
            this.Allow2Port1Appt            = false; // Allow 2 Port MTU for 1 Port Appointment
            this.AllowAbort                 = false; // Allow to have an abort button on cancellation screen during real installation
            this.AllowDailyReads            = true; // For DailyReads CheckBox
            this.AppointmentsLocation       = string.Empty; // The alternate appointments location, if specified
            this.AppointmentsName           = "Appointments.xml"; // ...
            this.AppointmentsReq            = false; // [ NYC ] Preclude NoApp Botton from Appointments Screen
            this.ApptGridConditionStatus    = "ALL"; // Condition for Appt Grid
            //this.ApptInstallerId          = IS apptInstallerIdPrepend or InstallerIdTag?
            this.ApptInstallerName          = string.Empty; // [ NYC ] The name from above
            this.ApptPort1Field             = string.Empty; // ...
            this.ApptPort2Field             = string.Empty; // ...
            this.ApptSave                   = false; // Allow to save appoitments: works only if loadoptions is true
            this.ApptScreen                 = true; // Should the appointments screen be shown if appointments are available?
            //this.ApptStatusDefinitions    = IN K.CODE is string[] that not string // Appintment Grid Statuses for LoadOptions
            this.AreYouSure                 = false; // To ask the question before pushing Ok
            this.NewMeterPort2isTheSame     = false; // To carry on NewMeter from Port1 to Port2
            this.AutoNewMeterPort2isTheSame = false; // Auto carry above
            this.AutoPurge                  = false; // Should log files be automatically purged?
            this.AutoPurgeSize              = 0; // Size at which the auto purge should take place
            this.AutoRegisterRecording      = false; // Record based on New# and Old#
            //this.AutoRFCheck              = NOT PRESENT IN K.CODE
            //this.ByPassAutoDetect         = IS showByPass? // It will create additional button for autodetect overwrite
            this.Cancel                     = new List<string> (); // Reason why the programming was cancelled
            this.CertPair                   = false; // ...
            this.CertPath                   = string.Empty; // ...
            this.CertPswd                   = string.Empty; // ...
            //this.CertPubKey               = IS certPublicKey? Is string that not byte[] // ...
            this.CertRecord                 = true; // record the cert values to Global.xml
            this.CertSubject                = string.Empty; // ...
            this.CertThumbprint             = string.Empty; // ...
            this.CertUpdate                 = string.Empty; // IN K.CODE IS DateTime that no string
            this.CertUpdateValid            = false; // Make update one day before valid
            this.CertValid                  = string.Empty; // IN K.CODE IS DateTime that no string
            this.CheckMTUfield              = string.Empty; // [ NYC ] what field to use fo above
            this.CheckMTUvalue              = string.Empty; // [ NYC ] what value of the field to use fo above
            this.CheckMTUtype               = false; // [ NYC ] to check if MTU is right type appointments only
            this.CheckSavedField            = false; // Check fields and values
            this.CoilDetect                 = true; // New for MC75A needs to be false
            this.ColorEntry                 = false; // [ NYC ] To show entry fields in Color
            this.ComPort                    = string.Empty; // The fallback comport from the XML file
            this.ConfigLocation             = string.Empty; // Configuration Location (XML) new entry in Global.xml
            this.CustomerMeterError         = string.Empty; // Special Customer Meter Error
            this.CustomerName               = "Aclara"; // To show customer name in About Screen
            this.DailyReadingOffset         = 0; // S4E (92) Default
            this.DailyReadsDefault          = 0; // Default Value for Daily Reads 3.30.2016 Makes it disable when it 255. -> got back 0
            this.dangerosZone               = "cache disk"; // PPC W 5.0 and Up volotile area "flash file store"
            //this.DefaultF1ReadInterval    = IN K.CODE IS bool THAT NO string //false; // [ NYC ] Force F1 MTU read every hour
            this.DeviceCertSubject          = string.Empty; // ...
            //this.ElectricPort1_2Interval  = IN K.CODE IS string THAT NO int //string.Empty; // The default read/transmit interval for ports 1&2 on electrics
            //this.ElectricPort3_4Interval  = IN K.CODE IS string THAT NO int //string.Empty; // The default read/transmit interval for ports 3&4 on electrics
            this.EMeterConnected            = false; // [ NYC ] To display the meter info in full read MTU
            //this.EnableFEC                = IN K.CODE IS string THAT NO bool //string.Empty; // Turn FEC on/off for electric, null - do nothing
            this.ErrorId                    = false; // [ SCG ] errorId reporting
            this.F12WAYRegister1            = "0x01C7D011";
            this.F12WAYRegister10           = "0x0130360A";
            this.F12WAYRegister14           = "0x021B5021";
            this.F1TamperCheck              = false; // To check F1 Pulser Tamper
            this.forceElectricMtuOn         = true; // [ MTU 91 ] Force on
            this.ForceTimeSync              = false; // F1 force to ask for timesync from dcu
            this.ftpUserName                = string.Empty; // FTP user name
            this.ftpPassword                = string.Empty; // FTP Password
            this.ftpRemoteHost              = string.Empty; // FTP Remote Host address
            this.ftpRemotePath              = string.Empty; // FTP Remote Location for Files
            this.ftpTransferredPath         = string.Empty; // Where to copy files inside FTP
            this.FullResult                 = false; // for Product Management SCG fiasco
            this.FutureDate                 = 7; // How many days in a future from appt date
            //this.GetMtuDelay              = IS getMtuInfoDelay?
            this.GpsBaudRate                = 9600; // GPS connection speed
            this.GpsComPort                 = string.Empty; // Name of the port where the GPS can be found
            this.GpsMetric                  = false; // GPS Altitude Values default in feets ( 1 meter = 3.2808399 feets )
            //this.gridColumn               = NOT PRESENT IN K.CODE
            this.HideProgressScreen         = false; // [ SCG ] To hide Progress Screen during MTU interogation. Only in Scripted Mode
            this.HideRevision               = false; // [ NYC ] To show Release Candidate / Revision
            //this.HourToAdjust             = IN K.CODE IS short THAT NO bool //0; // [ MTU 3000 ] Use Hours to randomize
            //this.ICfield1                 = IN K.CODE IS string THAT NO int //string.Empty; // Installation confirmation Data entry field1
            //this.ICfield2                 = IN K.CODE IS string THAT NO int //string.Empty; // Installation confirmation Data entry field2
            //this.IndividualDailyReads     = NOT PRESENT IN K.CODE
            this.IndividualReadInterval     = false; // Should the read interval be allowed to change?
            this.LatestVersion              = 16;
            //this.LiveDigitsOnly           = IS eForceDigits?
            this.LoadOptions                = false; // Allow to load options screen through appointments
            //this.LogLocation              = IN K.CODE IS string[] THAT NO string; // List of possible log file locations
            this.MeterNumberLength          = 12; // Maximum number of characters for meter serial number
            
            this.TimeSyncCountRepeat        = 1;
            this.TimeSyncCountDefault       = 63;
            
            /*
            this.MeterWorkRecording          = ;
            this.MinDate                     = ;
            this.MtuIdLength                 = ;
            this.NewMeterCalc                = ;
            this.NewMeterFormat              = ;
            this.NewMeterLabel               = ;
            this.NewMeterPort2isTheSame      = ;
            this.NewMeterPrefix              = ;
            this.NewMeterValidation          = ;
            this.NewSerialNumDualEntry       = ;
            this.NormXmitInterval            = "1 Hr";
            this.OldReadingDualEntry         = ;
            this.OldReadingRecording         = ;
            this.OldSerialNumDualEntry       = ;
            this.OtherCancelCode             = ;
            this.OverWriteAutoDetect         = ;
            this.PasswordMaxLength           = ;
            this.PasswordMinLength           = ;
            this.PlaySound                   = ;
            this.Port2DisableNo              = ;
            this.Port2MeterTypeTheSameWarning= ;
            this.PowerPolicy                 = ;
            this.ReadDelay                   = ;
            this.ReadingDualEntry            = ;
            this.RegisterRecording           = ;
            this.RegisterRecordingDefault    = ;
            this.RegisterRecordingItems      = ;
            this.RegisterRecordingReq        = ;
            this.ReportLogLocationStatus     = ;
            this.ReverseReading              = ;
            this.ScanDetail                  = ;
            this.ScanDetailLength            = ;
            this.scanfield                   = ;
            this.ScanMtu                     = ;
            this.ScanSumCheck                = ;
            this.ScriptOnly                  = ;
            this.SecondNormXmitCondition     = ;
            this.SecondNormXmitField         = ;
            this.SecondNormXmitInterval      = ;
            this.SerialNumLabel              = ;
            this.ShowAddMTU                  = ;
            this.ShowAddMTUMeter             = ;
            this.ShowAddMTUReplaceMeter      = ;
            this.ShowFreq                    = false;
            this.ShowInstallConfirmation     = ;
            this.ShowMeterVendor             = ;
            this.ShowReplaceMeter            = ;
            this.ShowReplaceMTU              = ;
            this.ShowReplaceMTUMeter         = ;
            this.ShowScriptErrorMessage      = ;
            this.ShowTime                    = ;
            this.ShowTurnOff                 = ;
            this.Siesta                      = ;
            this.SpecialSet                  = ;
            this.StartPoint                  = ;
            this.SystemDateVerify            = ;
            this.TempXmitCount               = ;
            this.TempXmitInterval            = ;
            this.TimeSyncCountDefault        = ;
            this.TimeToSync                  = ;
            this.TimeToSyncHR                = ;
            this.TimeToSyncMin               = ;
            this.UploadPrompt                = ;
            this.Use83                       = ;
            this.UseMeterSerialNumber        = ;
            this.UserIdMaxLength             = ;
            this.UserIdMinLength             = ;
            this.UTCOffset                   = ;
            this.WakeUpCount                 = ;
            this.WorkOrderDualEntry          = ;
            this.WorkOrderLabel              = ;
            this.WorkOrderLength             = ;
            this.WorkOrderRecording          = ;
            this.WriteDelay                  = ;
            this.WriteF1SystemTime           = ;
            this.XmitTimer                   = ;
            this.Options                     = ;
            this.FastMessageConfig           = ;
            this.Fast2Way                    = ;
            */
        }

        #region Tags

        [XmlElement("AccountDualEntry")]
        public bool AccountDualEntry { get; set; }

        [XmlElement("AccountEnabledAppt")]
        public bool AccountEnabledAppt { get; set; }

        [XmlElement("AccountFilled")]
        public bool AccountFilled { get; set; }

        [XmlElement("AccountLabel")]
        public string AccountLabel { get; set; }

        [XmlElement("AccountLength")]
        public int AccountLength { get; set; }

        [XmlElement("ActionVerify")]
        public bool ActionVerify { get; set; }

        [XmlElement("Address1Len")]
        public int Address1Len { get; set; }

        [XmlElement("Address2Len")]
        public int Address2Len { get; set; }

        [XmlElement("Address3Len")]
        public int Address3Len { get; set; }

        [XmlElement("Address4Len")]
        public int Address4Len { get; set; }

        [XmlElement("AddressLine1")]
        public string AddressLine1 { get; set; }

        [XmlElement("AddressLine2")]
        public string AddressLine2 { get; set; }

        [XmlElement("AddressLine3")]
        public string AddressLine3 { get; set; }

        [XmlElement("AddressLine4")]
        public string AddressLine4 { get; set; }

        [XmlElement("AddressVerify")]
        public bool AddressVerify { get; set; }

        [XmlElement("AFC")]
        public bool AFC { get; set; }

        [XmlElement("Allow2Port1Appt")]
        public bool Allow2Port1Appt { get; set; }

        [XmlElement("AllowAbort")]
        public bool AllowAbort { get; set; }

        [XmlElement("AllowDailyReads")]
        public bool AllowDailyReads { get; set; }

        [XmlElement("AppointmentsLocation")]
        public string AppointmentsLocation { get; set; }

        [XmlElement("AppointmentsName")]
        public string AppointmentsName { get; set; }

        [XmlElement("AppointmentsReq")]
        public bool AppointmentsReq { get; set; }

        [XmlElement("ApptGridConditionStatus")]
        public string ApptGridConditionStatus { get; set; }

        [XmlElement("ApptInstallerId")]
        public bool ApptInstallerId { get; set; }

        [XmlElement("ApptInstallerName")]
        public string ApptInstallerName { get; set; }

        [XmlElement("ApptPort1Field")]
        public string ApptPort1Field { get; set; }

        [XmlElement("ApptPort2Field")]
        public string ApptPort2Field { get; set; }

        [XmlElement("ApptSave")]
        public bool ApptSave { get; set; }

        [XmlElement("ApptScreen")]
        public bool ApptScreen { get; set; }

        [XmlElement("ApptStatusDefinitions")]
        public string ApptStatusDefinitions { get; set; }

        [XmlElement("AreYouSure")]
        public bool AreYouSure { get; set; }

        [XmlElement("AutoNewMeterPort2isTheSame")]
        public bool AutoNewMeterPort2isTheSame { get; set; }

        [XmlElement("AutoPurge")]
        public bool AutoPurge { get; set; }

        [XmlElement("AutoPurgeSize")]
        public int AutoPurgeSize { get; set; }

        [XmlElement("AutoRegisterRecording")]
        public bool AutoRegisterRecording { get; set; }

        [XmlElement("AutoRFCheck")]
        public bool AutoRFCheck { get; set; }

        [XmlElement("ByPassAutoDetect")]
        public bool ByPassAutoDetect { get; set; }

        [XmlArray("Cancel")]
        [XmlArrayItem("option")]
        public List<string> Cancel { get; set; }

        [XmlElement("CertPair")]
        public bool CertPair { get; set; }

        [XmlElement("CertPath")]
        public string CertPath { get; set; }

        [XmlElement("CertPswd")]
        public string CertPswd { get; set; }

        [XmlElement("CertPubKey")]
        public string CertPubKey { get; set; }

        #endregion












        [XmlElement("CertRecord")]
        public bool CertRecord { get; set; }

        [XmlElement("CertSubject")]
        public string CertSubject { get; set; }

        [XmlElement("CertThumbprint")]
        public string CertThumbprint { get; set; }

        [XmlElement("CertUpdate")]
        public string CertUpdate { get; set; }

        [XmlElement("CertUpdateValid")]
        public bool CertUpdateValid { get; set; }

        [XmlElement("CertValid")]
        public string CertValid { get; set; }

        [XmlElement("CheckMTUfield")]
        public string CheckMTUfield { get; set; }

        [XmlElement("CheckMTUtype")]
        public bool CheckMTUtype { get; set; }

        [XmlElement("CheckMTUvalue")]
        public string CheckMTUvalue { get; set; }

        [XmlElement("CheckSavedField")]
        public bool CheckSavedField { get; set; }

        [XmlElement("CoilDetect")]
        public bool CoilDetect { get; set; }

        [XmlElement("ColorEntry")]
        public bool ColorEntry { get; set; }

        [XmlElement("ComPort")]
        public string ComPort { get; set; }

        [XmlElement("ConfigLocation")]
        public string ConfigLocation { get; set; }

        [XmlElement("CustomerMeterError")]
        public string CustomerMeterError { get; set; }

        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }

        [XmlElement("DailyReadingOffset")]
        public int DailyReadingOffset { get; set; }

        [XmlIgnore]
        public int DailyReadsDefault { get; set; }

        [XmlElement("DailyReadsDefault")]
        public string DailyReadsDefaultAllowEmptyField
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int v;
                    if (int.TryParse(value, out v))
                        this.DailyReadsDefault = v;
                    else this.DailyReadsDefault = -1;
                }
                else this.DailyReadsDefault = -1;
            }
        }

        [XmlElement("dangerosZone")]
        public string dangerosZone { get; set; }

        [XmlElement("DefaultF1ReadInterval")]
        public string DefaultF1ReadInterval { get; set; }

        [XmlElement("DeviceCertSubject")]
        public string DeviceCertSubject { get; set; }

        [XmlElement("ElectricPort1_2Interval")]
        public int ElectricPort1_2Interval { get; set; }

        [XmlElement("ElectricPort3_4Interval")]
        public int ElectricPort3_4Interval { get; set; }

        [XmlElement("EMeterConnected")]
        public bool EMeterConnected { get; set; }

        [XmlElement("EnableFEC")]
        public bool EnableFEC { get; set; }

        [XmlElement("ErrorId")]
        public bool ErrorId { get; set; }

        [XmlElement("F12WAYRegister1")]
        public string F12WAYRegister1 { get; set; }

        [XmlElement("F12WAYRegister10")]
        public string F12WAYRegister10 { get; set; }

        [XmlElement("F12WAYRegister14")]
        public string F12WAYRegister14 { get; set; }

        [XmlElement("F1TamperCheck")]
        public bool F1TamperCheck { get; set; }

        [XmlElement("forceElectricMtuOn")]
        public bool forceElectricMtuOn { get; set; }

        [XmlElement("ForceTimeSync")]
        public bool ForceTimeSync { get; set; }

        [XmlElement("ftpPassword")]
        public string ftpPassword { get; set; }

        [XmlElement("ftpRemoteHost")]
        public string ftpRemoteHost { get; set; }

        [XmlElement("ftpRemotePath")]
        public string ftpRemotePath { get; set; }

        [XmlElement("ftpTransferredPath")]
        public string ftpTransferredPath { get; set; }

        [XmlElement("ftpUserName")]
        public string ftpUserName { get; set; }

        [XmlElement("FullResult")]
        public bool FullResult { get; set; }

        [XmlElement("FutureDate")]
        public int FutureDate { get; set; }

        [XmlElement("GetMtuDelay")]
        public int GetMtuDelay { get; set; }

        [XmlElement("GpsBaudRate")]
        public int GpsBaudRate { get; set; }

        [XmlElement("GpsComPort")]
        public string GpsComPort { get; set; }

        [XmlElement("GpsMetric")]
        public bool GpsMetric { get; set; }

        [XmlElement("gridColumn")]
        public string gridColumn { get; set; }

        [XmlElement("HideProgressScreen")]
        public bool HideProgressScreen { get; set; }

        [XmlElement("HideRevision")]
        public bool HideRevision { get; set; }

        [XmlElement("HourToAdjust")]
        public bool HourToAdjust { get; set; }

        [XmlElement("ICfield1")]
        public int ICfield1 { get; set; }

        [XmlElement("ICfield2")]
        public int ICfield2 { get; set; }

        [XmlElement("IndividualDailyReads")]
        public bool IndividualDailyReads { get; set; }

        [XmlElement("IndividualReadInterval")]
        public bool IndividualReadInterval { get; set; }

        [XmlElement("LatestVersion")]
        public int LatestVersion { get; set; }

        [XmlElement("LiveDigitsOnly")]
        public bool LiveDigitsOnly { get; set; }

        [XmlElement("LoadOptions")]
        public bool LoadOptions { get; set; }

        [XmlElement("LogLocation")]
        public string LogLocation { get; set; }

        [XmlElement("MeterNumberLength")]
        public int MeterNumberLength { get; set; }

        [XmlElement("MeterSerialEnabledAppt")]
        public bool MeterSerialEnabledAppt { get; set; }

        [XmlElement("MeterWorkRecording")]
        public bool MeterWorkRecording { get; set; }

        [XmlElement("MinDate")]
        public string MinDate { get; set; }

        [XmlElement("MtuIdLength")]
        public int MtuIdLength { get; set; }

        [XmlElement("NewMeterCalc")]
        public string NewMeterCalc { get; set; }

        [XmlElement("NewMeterFormat")]
        public string NewMeterFormat { get; set; }

        [XmlElement("NewMeterLabel")]
        public string NewMeterLabel { get; set; }

        [XmlElement("NewMeterPort2isTheSame")]
        public bool NewMeterPort2isTheSame { get; set; }

        [XmlElement("NewMeterPrefix")]
        public bool NewMeterPrefix { get; set; }

        [XmlElement("NewMeterValidation")]
        public bool NewMeterValidation { get; set; }

        [XmlElement("NewSerialNumDualEntry")]
        public bool NewSerialNumDualEntry { get; set; }

        [XmlElement("NormXmitInterval")]
        public string NormXmitInterval { get; set; }

        [XmlElement("OldReadingDualEntry")]
        public bool OldReadingDualEntry { get; set; }

        [XmlElement("OldReadingRecording")]
        public bool OldReadingRecording { get; set; }

        [XmlElement("OldSerialNumDualEntry")]
        public bool OldSerialNumDualEntry { get; set; }

        [XmlElement("OtherCancelCode")]
        public string OtherCancelCode { get; set; }

        [XmlElement("OverWriteAutoDetect")]
        public bool OverWriteAutoDetect { get; set; }

        [XmlElement("PasswordMaxLength")]
        public int PasswordMaxLength { get; set; }

        [XmlElement("PasswordMinLength")]
        public int PasswordMinLength { get; set; }

        [XmlElement("PlaySound")]
        public bool PlaySound { get; set; }

        [XmlElement("Port2DisableNo")]
        public bool Port2DisableNo { get; set; }

        [XmlElement("Port2MeterTypeTheSameWarning")]
        public bool Port2MeterTypeTheSameWarning { get; set; }

        [XmlElement("PowerPolicy")]
        public bool PowerPolicy { get; set; }

        [XmlElement("ReadDelay")]
        public int ReadDelay { get; set; }

        [XmlElement("ReadingDualEntry")]
        public bool ReadingDualEntry { get; set; }

        [XmlElement("RegisterRecording")]
        public bool RegisterRecording { get; set; }

        [XmlElement("RegisterRecordingDefault")]
        public string RegisterRecordingDefault { get; set; }

        [XmlElement("RegisterRecordingItems")]
        public string RegisterRecordingItems { get; set; }

        [XmlElement("RegisterRecordingReq")]
        public bool RegisterRecordingReq { get; set; }

        [XmlElement("ReportLogLocationStatus")]
        public bool ReportLogLocationStatus { get; set; }

        [XmlElement("ReverseReading")]
        public bool ReverseReading { get; set; }

        [XmlElement("ScanDetail")]
        public bool ScanDetail { get; set; }

        [XmlElement("ScanDetailLength")]
        public int ScanDetailLength { get; set; }

        [XmlElement("scanfield")]
        public string scanfield { get; set; }

        [XmlElement("ScanMtu")]
        public bool ScanMtu { get; set; }

        [XmlElement("ScanSumCheck")]
        public bool ScanSumCheck { get; set; }

        [XmlElement("ScriptOnly")]
        public bool ScriptOnly { get; set; }

        [XmlElement("SecondNormXmitCondition")]
        public string SecondNormXmitCondition { get; set; }

        [XmlElement("SecondNormXmitField")]
        public string SecondNormXmitField { get; set; }

        [XmlElement("SecondNormXmitInterval")]
        public string SecondNormXmitInterval { get; set; }

        [XmlElement("SerialNumLabel")]
        public string SerialNumLabel { get; set; }

        [XmlElement("ShowAddMtu")]
        public bool ShowAddMTU { get; set; }

        [XmlElement("ShowAddMtuMeter")]
        public bool ShowAddMTUMeter { get; set; }

        [XmlElement("ShowAddMtuReplaceMeter")]
        public bool ShowAddMTUReplaceMeter { get; set; }

        [XmlElement("ShowFreq")]
        public bool ShowFreq { get; set; }

        [XmlElement("ShowInstallConfirmation")]
        public bool ShowInstallConfirmation { get; set; }

        [XmlElement("ShowMeterVendor")]
        public bool ShowMeterVendor { get; set; }

        [XmlElement("ShowReplaceMeter")]
        public bool ShowReplaceMeter { get; set; }

        [XmlElement("ShowReplaceMtu")]
        public bool ShowReplaceMTU { get; set; }

        [XmlElement("ShowReplaceMtuMeter")]
        public bool ShowReplaceMTUMeter { get; set; }

        [XmlElement("ShowScriptErrorMessage")]
        public bool ShowScriptErrorMessage { get; set; }

        [XmlElement("ShowTime")]
        public bool ShowTime { get; set; }

        [XmlElement("ShowTurnOff")]
        public bool ShowTurnOff { get; set; }

        [XmlElement("Siesta")]
        public bool Siesta { get; set; }

        [XmlElement("SpecialSet")]
        public string SpecialSet { get; set; }

        [XmlElement("StartPoint")]
        public string StartPoint { get; set; }

        [XmlElement("SystemDateVerify")]
        public bool SystemDateVerify { get; set; }

        [XmlElement("TempXmitCount")]
        public int TempXmitCount { get; set; }

        [XmlElement("TempXmitInterval")]
        public string TempXmitInterval { get; set; }

        [XmlElement("TimeSyncCountDefault")]
        public int TimeSyncCountDefault { get; set; }

        [XmlIgnore]
        public int TimeSyncCountRepeat { get; set; }
        
        [XmlElement("TimeSyncCountRepeat")]
        public int TimeSyncCountRepeat_Range
        {
            set
            {
                // Value must be always inside the range [1,3]
                this.TimeSyncCountRepeat = ( value < 1 ) ? 1 : ( ( value > 3 ) ? 3 : value );
            }
        }
        
        [XmlElement("TimeToSync")]
        public bool TimeToSync { get; set; }

        [XmlElement("TimeToSyncHR")]
        public int TimeToSyncHR { get; set; }

        [XmlElement("TimeToSyncMin")]
        public int TimeToSyncMin { get; set; }

        [XmlElement("UploadPrompt")]
        public bool UploadPrompt { get; set; }

        [XmlElement("Use8.3")]
        public bool Use83 { get; set; }

        [XmlElement("UseMeterSerialNumber")]
        public bool UseMeterSerialNumber { get; set; }

        [XmlElement("UserIdMaxLength")]
        public int UserIdMaxLength { get; set; }

        [XmlElement("UserIdMinLength")]
        public int UserIdMinLength { get; set; }

        [XmlElement("UTCOffset")]
        public int UTCOffset { get; set; }

        [XmlElement("WakeUpCount")]
        public int WakeUpCount { get; set; }

        [XmlElement("WorkOrderDualEntry")]
        public bool WorkOrderDualEntry { get; set; }

        [XmlElement("WorkOrderEnabledAppt")]
        public bool WorkOrderEnabledAppt { get; set; }

        [XmlElement("WorkOrderLabel")]
        public string WorkOrderLabel { get; set; }

        [XmlElement("WorkOrderLength")]
        public int WorkOrderLength { get; set; }

        [XmlElement("WorkOrderRecording")]
        public bool WorkOrderRecording { get; set; }

        [XmlElement("WriteDelay")]
        public int WriteDelay { get; set; }

        [XmlElement("WriteF1SystemTime")]
        public bool WriteF1SystemTime { get; set; }

        [XmlElement("XmitTimer")]
        public int XmitTimer { get; set; }

        [XmlArray("Options")]
        [XmlArrayItem("option")]
        public List<Option> Options { get; set; }

        [XmlElement("FastMessageConfig")]
        public bool FastMessageConfig { get; set; }

        [XmlElement("Fast-2-Way")]
        public bool Fast2Way { get; set; }
    }
}
