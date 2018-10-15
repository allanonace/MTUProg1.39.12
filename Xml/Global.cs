using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("Globals")]
    public class Global
    {
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

        [XmlElement("Cancel")]
        public string Cancel { get; set; }

        [XmlElement("CertPair")]
        public bool CertPair { get; set; }

        [XmlElement("CertPath")]
        public string CertPath { get; set; }

        [XmlElement("CertPswd")]
        public string CertPswd { get; set; }

        [XmlElement("CertPubKey")]
        public string CertPubKey { get; set; }

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

        [XmlElement("DailyReadsDefault")]
        public int DailyReadsDefault { get; set; }

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

        [XmlElement("ShowAddMTU")]
        public bool ShowAddMTU { get; set; }

        [XmlElement("ShowAddMTUMeter")]
        public bool ShowAddMTUMeter { get; set; }

        [XmlElement("ShowAddMTUReplaceMeter")]
        public bool ShowAddMTUReplaceMeter { get; set; }

        [XmlElement("ShowFreq")]
        public bool ShowFreq { get; set; }

        [XmlElement("ShowInstallConfirmation")]
        public bool ShowInstallConfirmation { get; set; }

        [XmlElement("ShowMeterVendor")]
        public bool ShowMeterVendor { get; set; }

        [XmlElement("ShowReplaceMeter")]
        public bool ShowReplaceMeter { get; set; }

        [XmlElement("ShowReplaceMTU")]
        public bool ShowReplaceMTU { get; set; }

        [XmlElement("ShowReplaceMTUMeter")]
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

        [XmlElement("Options")]
        public List<Option> Options { get; set; }
    }
}
