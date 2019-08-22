using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    /// <summary>
    /// Class used to map the Global.xml configuration file.
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="Global">
    /// <item>
    ///   <term>AccountDualEntry</term>
    ///   <description>Enables or disables dual entry for AccountNumber</description>
    /// </item>
    /// <item>
    ///   <term>AccountEnabledAppt</term>
    ///   <description>[LOW PRIORITY = NOT CONFIRMED] Enable or disable values after appointment in interactive mode ( Account Number )</description>
    /// </item>
    /// <item>
    ///   <term>AccountFilled</term>
    ///   <description>Allows AccountNumber to be filled with MTU ID automatically</description>
    /// </item>
    /// <item>
    ///   <term>AccountLabel</term>
    ///   <description>[1-15] Label for AccountNumber field</description>
    /// </item>
    /// <item>
    ///   <term>AccountLength</term>
    ///   <description>[1-12] Number of digits to use in AccountNumber field</description>
    /// </item>
    /// <item>
    ///   <term>ActionVerify</term>
    ///   <description>Display popup to confirm that user wants toperform the action</description>
    /// </item>
    /// <item>
    ///   <term>Address1Len</term>
    ///   <description>Length of appointment address line 1 field</description>
    /// </item>
    /// <item>
    ///   <term>Address2Len</term>
    ///   <description>... 2 field</description>
    /// </item>
    /// <item>
    ///   <term>Address3Len</term>
    ///   <description>... 3 field</description>
    /// </item>
    /// <item>
    ///   <term>Address4Len</term>
    ///   <description>... 4 field</description>
    /// </item>
    /// <item>
    ///   <term>AddressLine1</term>
    ///   <description>[Custumer specific] Definition of line 1 ofverified addres in appointment file ( e.g. HouseNo )</description>
    /// </item>
    /// <item>
    ///   <term>AddressLine2</term>
    ///   <description>... line 2 ... ( e.g. Street )</description>
    /// </item>
    /// <item>
    ///   <term>AddressLine3</term>
    ///   <description>... line 3 ... ( e.g. N/A )</description>
    /// </item>
    /// <item>
    ///   <term>AddressLine4</term>
    ///   <description>... line 4 ... ( e.g. N/A )</description>
    /// </item>
    /// <item>
    ///   <term>AddressVerify</term>
    ///   <description>Set to true is appointment address should be verified</description>
    /// </item>
    /// <item>
    ///   <term>AFC</term>
    ///   <description>Advance Frequency Change on 3000 MTUs afterfirmware 19</description>
    /// </item>
    /// <item>
    ///   <term>Allow2Port1Appt</term>
    ///   <description>Allow use of dual port MTUs for one port appointments</description>
    /// </item>
    /// <item>
    ///   <term>AllowAbort</term>
    ///   <description>Adds an abort button on cancellation screenduring installation</description>
    /// </item>
    /// <item>
    ///   <term>AllowDailyReads</term>
    ///   <description>Enable SnapReads/DailyReads</description>
    /// </item>
    /// <item>
    ///   <term>AppointmentsLocation</term>
    ///   <description>Specifies an alternate location for the Appointments file ( PC only )</description>
    /// </item>
    /// <item>
    ///   <term>AppointmentsName</term>
    ///   <description>[LOW PRIORITY = NOT CONFIRMED][1-255] Name of the appointments file</description>
    /// </item>
    /// <item>
    ///   <term>AppointmentsReq</term>
    ///   <description>Forces you to accept the appointment beforebeing allowed to program the MTU</description>
    /// </item>
    /// <item>
    ///   <term>ApptGridConditionStatus</term>
    ///   <description>[LOW PRIORITY = NOT CONFIRMED][All,New,Plan] Controls contents of conditional grid</description>
    /// </item>
    /// <item>
    ///   <term>ApptInstallerId</term>
    ///   <description>Works only if AppointmentsReq is true and will add ( as prefix ) installer ID to the activity file with installer id name from appointments file</description>
    /// </item>
    /// <item>
    ///   <term>ApptInstallerName</term>
    ///   <description>[LOW PRIORITY = NOT CONFIRMED][1-100] Installer name indicates the tag to use in appointments file</description>
    /// </item>
    /// <item>
    ///   <term>ApptPort1Field</term>
    ///   <description>Saving fields in addition to the status</description>
    /// </item>
    /// <item>
    ///   <term>ApptPort2Field</term>
    ///   <description>Saving fields in addition to the status</description>
    /// </item>
    /// <item>
    ///   <term>ApptSave</term>
    ///   <description>Allows editing and saving changes to the appointments file</description>
    /// </item>
    /// <item>
    ///   <term>ApptScreen</term>
    ///   <description>[LOW PRIORITY = NOT CONFIRMED] Enables or disables display of the appointments screen</description>
    /// </item>
    /// <item>
    ///   <term>ApptStatusDefinitions</term>
    ///   <description>Used to display Appointment Statuses</description>
    /// </item>
    /// <item>
    ///   <term>AreYouSure</term>
    ///   <description>Displays Are you sure? confirmation messages if true</description>
    /// </item>
    /// <item>
    ///   <term>AutoNewMeterPort2isTheSame</term>
    ///   <description>Automatically copies values from Port 1 to Port 2</description>
    /// </item>
    /// <item>
    ///   <term>AutoPurge</term>
    ///   <description>Enables / disables automatic purging of logfiles</description>
    /// </item>
    /// <item>
    ///   <term>AutoPurgeSize</term>
    ///   <description>[x-9999] Maximum size in kilobytes for log file before auto purge if AutoPurge is true</description>
    /// </item>
    /// <item>
    ///   <term>AutoRegisterRecording</term>
    ///   <description>Records Register selection in Activity Log based on the old meter serial number and new serial number</description>
    /// </item>
    /// <item>
    ///   <term>AutoRFCheck</term>
    ///   <description>NOT PRESENT IN THE PDF NORE KG.CODE // It will be used for 34XX series MTUs</description>
    /// </item>
    /// <item>
    ///   <term>ByPassAutoDetect</term>
    ///   <description>Bypass F1 Ecoder/Encoder Autodetect</description>
    /// </item>
    /// <item>
    ///   <term>CertPair</term>
    ///   <description>Whether a certificate pair (i.e. Public andPrivate) is installed ( PC only )</description>
    /// </item>
    /// <item>
    ///   <term>CertPath</term>
    ///   <description>Key Store Path for certificate ( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>CertPswd</term>
    ///   <description>Digital Certificate Password if Full certificate stored</description>
    /// </item>
    /// <item>
    ///   <term>CertPubKey</term>
    ///   <description>[Base64] Digital Certificate Public Key</description>
    /// </item>
    /// <item>
    ///   <term>CertRecord</term>
    ///   <description>Copy Certificate attributes into Global.xml( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>CertSubject</term>
    ///   <description>Digital Certificate Subject Search ( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>CertThumbprint</term>
    ///   <description>NOT PRESENT IN KG.CODE // Unique Value of Digital Certificate</description>
    /// </item>
    /// <item>
    ///   <term>CertUpdate</term>
    ///   <description>[DateTime] Date on which certificate must be updated</description>
    /// </item>
    /// <item>
    ///   <term>CertUpdateValid</term>
    ///   <description>If true, application makes CertUpdate one day prior to Certvalid</description>
    /// </item>
    /// <item>
    ///   <term>CertValid</term>
    ///   <description>[DateTime] CertValidActual certificate expiration date. Application will not run if current date is past this date</description>
    /// </item>
    /// <item>
    ///   <term>CheckConfigFiles</term>
    ///   <description>Check for updated XML config files and certificate</description>
    /// </item>
    /// <item>
    ///   <term>CheckMTUfield</term>
    ///   <description>What field to use with CheckMtuType tag</description>
    /// </item>
    /// <item>
    ///   <term>CheckMTUtype</term>
    ///   <description>Check if this MTU is suitable for this appointment</description>
    /// </item>
    /// <item>
    ///   <term>CheckMTUvalue</term>
    ///   <description>What value to compare</description>
    /// </item>
    /// <item>
    ///   <term>CheckSavedField</term>
    ///   <description>Checks and saves fields in appointment file</description>
    /// </item>
    /// <item>
    ///   <term>CoilDetect</term>
    ///   <description>Check coil presence</description>
    /// </item>
    /// <item>
    ///   <term>ColorEntry</term>
    ///   <description>Uses a color screen for newer model handhelds</description>
    /// </item>
    /// <item>
    ///   <term>ComPort</term>
    ///   <description>[COM1-COM9] Default COM port to use if the CF ( compact flash ) interface card is not found</description>
    /// </item>
    /// <item>
    ///   <term>ConfigLocation</term>
    ///   <description>Specifies an alternate location for the permanent configuration log file ( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>ConfigPublicDir</term>
    ///   <description>Specifies if the config files remains in the public folder or not, after copy the files in private folder</description>
    /// </item>
    /// <item>
    ///   <term>CustomerMeterError</term>
    ///   <description>[1-20] If meter error occurs, this message will appear</description>
    /// </item>
    /// <item>
    ///   <term>CustomerName</term>
    ///   <description>[1-20] Display the tag value in the About sceen</description>
    /// </item>
    /// <item>
    ///   <term>DailyReadingOffset</term>
    ///   <description>[Byte] Daily Reading Offset for Electric MTU Type 92</description>
    /// </item>
    /// <item>
    ///   <term>DailyReadsDefault</term>
    ///   <description>[0-23] Hour (in military time) for daily reads/snap</description>
    /// </item>
    /// <item>
    ///   <term>dangerosZone</term>
    ///   <description>PPC W 5.0 and Up volatile area flash file store</description>
    /// </item>
    /// <item>
    ///   <term>DefaultF1ReadInterval</term>
    ///   <description>[NOT NEED TO SUPPORT = NOT CONFIRMED] Use [1Hr] Interval for F1 radio. Allows using 2 read intervals for old and new flat packs</description>
    /// </item>
    /// <item>
    ///   <term>DeviceCertSubject</term>
    ///   <description>Actual certificate subject</description>
    /// </item>
    /// <item>
    ///   <term>ElectricPort1_2Interval</term>
    ///   <description>[NOT NEED TO SUPPORT = NOT CONFIRMED][5,10,15,20,30,60,120,180,240] Default transmit/read intervals in minutes for Port 1 and Port 2 of electric MTUs</description>
    /// </item>
    /// <item>
    ///   <term>ElectricPort3_4Interval</term>
    ///   <description>[NOT NEED TO SUPPORT = NOT CONFIRMED][5,10,15,20,30,60,120,180,240] Default transmit/read intervals in minutes for Port 3 and Port 4 of electric MTUs</description>
    /// </item>
    /// <item>
    ///   <term>EMeterConnected</term>
    ///   <description>NOT PRESENT IN THE PDF // KG: To display the meter info in full read MTU</description>
    /// </item>
    /// <item>
    ///   <term>EnableFEC</term>
    ///   <description>Enable Forward Error Correction (FEC) for Electric MTU</description>
    /// </item>
    /// <item>
    ///   <term>ErrorId</term>
    ///   <description>Add Error Id numbers to Activity Log error messages</description>
    /// </item>
    /// <item>
    ///   <term>F12WAYRegister1</term>
    ///   <description>PDF: 0x01C75011 // [4bytes Hex or Decimals]Frequency value for channel 1</description>
    /// </item>
    /// <item>
    ///   <term>FastMessageConfig</term>
    ///   <description>To make migratable switch for migratable MTUs => Bit 0: Fast Messaging Mode - 0=Off, 1=On Location 163 Migratable</description>
    /// </item>
    /// <item>
    ///   <term>F12WAYRegister10</term>
    ///   <description>... channel 10</description>
    /// </item>
    /// <item>
    ///   <term>F12WAYRegister14</term>
    ///   <description>... channel 14</description>
    /// </item>
    /// <item>
    ///   <term>F1TamperCheck</term>
    ///   <description>Perform continuity test between MTU and Meter for Pulse</description>
    /// </item>
    /// <item>
    ///   <term>forceElectricMtuOn</term>
    ///   <description>Forcibly turn on electric MTUs except types74 and 75 ( Landis & Gyr Focus AX )</description>
    /// </item>
    /// <item>
    ///   <term>ForceTimeSync</term>
    ///   <description>Force an Installation Confirmation during MTU installation for 3000 MTU</description>
    /// </item>
    /// <item>
    ///   <term>ftpPassword</term>
    ///   <description>FTP login password ( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>ftpRemoteHost</term>
    ///   <description>FTP remote host name ( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>ftpRemotePath</term>
    ///   <description>Path to FTP remote host ( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>ftpTransferredPath</term>
    ///   <description>FTP Parameters ( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>ftpUserName</term>
    ///   <description>FTP login user name ( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>FullResult</term>
    ///   <description>Makes Result file schema identical to that of the Activity Log</description>
    /// </item>
    /// <item>
    ///   <term>FutureDate</term>
    ///   <description>[1-255] The number of days from the appointment's scheduled start date within which it is acceptable for work to startInteger</description>
    /// </item>
    /// <item>
    ///   <term>GetMtuDelay</term>
    ///   <description>[1-5000] If required, contact Engineering for details. MTU read delay</description>
    /// </item>
    /// <item>
    ///   <term>GoodF1Rely</term>
    ///   <description>Good F1 frequency threshold in meters for the evaluation of nodes during the Node Discovery process</description>
    /// </item>
    /// <item>
    ///   <term>GoodF2Rely</term>
    ///   <description>Good F2 frequency threshold in meters for the evaluation of nodes during the Node Discovery process</description>
    /// </item>
    /// <item>
    ///   <term>GoodNumDCU</term>
    ///   <description>The desired number of DCUs for the evaluation of the Node Discovery process</description>
    /// </item>
    /// <item>
    ///   <term>GpsBaudRate</term>
    ///   <description>[9600,4800] GPS Communication Baud Rate</description>
    /// </item>
    /// <item>
    ///   <term>GpsComPort</term>
    ///   <description>[NOT NEED TO SUPPORT = NOT CONFIRMED][COMX]Specifies the COM port for GPS tracking</description>
    /// </item>
    /// <item>
    ///   <term>GpsMetric</term>
    ///   <description>Record the result in Metric or Imperial</description>
    /// </item>
    /// <item>
    ///   <term>gridColumn</term>
    ///   <description>[Custom] Description for Grid</description>
    /// </item>
    /// <item>
    ///   <term>HideProgressScreen</term>
    ///   <description>Hides the Progress Bar during MTU installation or reading</description>
    /// </item>
    /// <item>
    ///   <term>HideRevision</term>
    ///   <description>Hides Revision number in About screen</description>
    /// </item>
    /// <item>
    ///   <term>HourToAdjust</term>
    ///   <description>Use Hours to randomize for 3000 MTU</description>
    /// </item>
    /// <item>
    ///   <term>ICfield1</term>
    ///   <description>[LOW PRIORITY = NOT CONFIRMED][0-100] Installation Confirmation field free-form description</description>
    /// </item>
    /// <item>
    ///   <term>ICfield2</term>
    ///   <description>[LOW PRIORITY = NOT CONFIRMED][0-100] Installation Confirmation field free-form description</description>
    /// </item>
    /// <item>
    ///   <term>IndividualDailyReads</term>
    ///   <description>Allow selection of daily Reads interval value</description>
    /// </item>
    /// <item>
    ///   <term>IndividualReadInterval</term>
    ///   <description>Controls whether the read interval can be specified on a per MTU basis or default only</description>
    /// </item>
    /// <item>
    ///   <term>LatestVersion</term>
    ///   <description>[Byte] F1 MTU Good firmware version. Specifies the minimum version that can have a 20-minute read interval</description>
    /// </item>
    /// <item>
    ///   <term>LiveDigitsOnly</term>
    ///   <description>If true it will show X for Dummy Digits. False it will show 0</description>
    /// </item>
    /// <item>
    ///   <term>LoadOptions</term>
    ///   <description>Load options screen through appointments</description>
    /// </item>
    /// <item>
    ///   <term>LogLocation</term>
    ///   <description>Specifies an alternate location for the Activity Log file ( PC Only )</description>
    /// </item>
    /// <item>
    ///   <term>LogsPublicDir</term>
    ///   <description>specifies if the logs files are in the public folder or in the privte folder</description>
    /// </item>
    /// <item>
    ///   <term>MaxTimeRFCheck</term>
    ///   <description>Maximum time in seconds to perform the Node Discovery process</description>
    /// </item>
    /// <item>
    ///   <term>MeterNumberLength</term>
    ///   <description>[1-20] Maximum length of the meter serial number</description>
    /// </item>
    /// <item>
    ///   <term>MeterSerialEnabledAppt</term>
    ///   <description>Enable or disable values after appointment in interactive mode ( Meter Old/Serial Number )</description>
    /// </item>
    /// <item>
    ///   <term>MeterWorkRecording</term>
    ///   <description>Enables or disables the Old Meter Working dialog box during MTU programming</description>
    /// </item>
    /// <item>
    ///   <term>MinDate</term>
    ///   <description>If the handheld reports a date earlier than“MinDate”, STAR Programmer stops working in order to prevent the wrong date from being entered</description>
    /// </item>
    /// <item>
    ///   <term>MinF1Rely</term>
    ///   <description>The minimum F1 frequency threshold in meters for the evaluation of nodes during the Node Discovery process</description>
    /// </item>
    /// <item>
    ///   <term>MinF2Rely</term>
    ///   <description>The minimum F2 frequency threshold in meters for the evaluation of nodes during the Node Discovery process</description>
    /// </item>
    /// <item>
    ///   <term>MinNumDCU</term>
    ///   <description>The minimum number of DCUs for the evaluation of the Node Discovery process</description>
    /// </item>
    /// <item>
    ///   <term>MtuIdLength</term>
    ///   <description>[5-11] Number of digits in the MTU ID, including leading zeros</description>
    /// </item>
    /// <item>
    ///   <term>NewMeterCalc</term>
    ///   <description>Prefix of new meter ID</description>
    /// </item>
    /// <item>
    ///   <term>NewMeterFormat</term>
    ///   <description>Sets guidelines for entering a new meter</description>
    /// </item>
    /// <item>
    ///   <term>NewMeterLabel</term>
    ///   <description>[1-15] Optional parameter for new meter</description>
    /// </item>
    /// <item>
    ///   <term>NewMeterPort2isTheSame</term>
    ///   <description>Optionally copies Port 1 data to Port 2</description>
    /// </item>
    /// <item>
    ///   <term>NewMeterPrefix</term>
    ///   <description>Enables adding prefix to meter number</description>
    /// </item>
    /// <item>
    ///   <term>NewMeterValidation</term>
    ///   <description>Enable New Meter navigation/validation based on prefix and actual meter</description>
    /// </item>
    /// <item>
    ///   <term>NewSerialNumDualEntry</term>
    ///   <description>Enables or disables dual entry of the new meter serial number during programming</description>
    /// </item>
    /// <item>
    ///   <term>NormXmitInterval</term>
    ///   <description>Default transmit interval</description>
    /// </item>
    /// <item>
    ///   <term>OldReadingDualEntry</term>
    ///   <description>F1 Flatpack 2000/3000 series up to version 2.1.1 and firmware up to 15</description>
    /// </item>
    /// <item>
    ///   <term>OldReadingRecording</term>
    ///   <description>Enables or disables dual entry of the old meter reading during programming</description>
    /// </item>
    /// <item>
    ///   <term>OldSerialNumDualEntry</term>
    ///   <description>Enables or disables entry of old meter reading</description>
    /// </item>
    /// <item>
    ///   <term>OtherCancelCode</term>
    ///   <description>[1-15] Use a special code for other cancel option</description>
    /// </item>
    /// <item>
    ///   <term>OverWriteAutoDetect</term>
    ///   <description>To show overwrite button on the sceen</description>
    /// </item>
    /// <item>
    ///   <term>PasswordMaxLength</term>
    ///   <description>[1-10] Sets the maximum logon user passwordlength in characters</description>
    /// </item>
    /// <item>
    ///   <term>PasswordMinLength</term>
    ///   <description>[1-10] Sets the minimum logon user passwordlength in characters</description>
    /// </item>
    /// <item>
    ///   <term>PlaySound</term>
    ///   <description>Play sound on HH after Installation Confirmation is complete</description>
    /// </item>
    /// <item>
    ///   <term>Port2DisableNo</term>
    ///   <description>Disable ability to click on Display checkbox to disable Port2</description>
    /// </item>
    /// <item>
    ///   <term>Port2MeterTypeTheSameWarning</term>
    ///   <description>Display a warning if Meter types are the same for dual port MTU</description>
    /// </item>
    /// <item>
    ///   <term>PowerPolicy</term>
    ///   <description>Disable/Enable USB Power programatically</description>
    /// </item>
    /// <item>
    ///   <term>ReadDelay</term>
    ///   <description>[1-10] Number of seconds of wait time before reading or writing the MTU</description>
    /// </item>
    /// <item>
    ///   <term>ReadingDualEntry</term>
    ///   <description>Enables or disables dual entry of the meterreading during programming</description>
    /// </item>
    /// <item>
    ///   <term>RegisterRecording</term>
    ///   <description>Enables or disables display of the Register/Meter change question during programming</description>
    /// </item>
    /// <item>
    ///   <term>RegisterRecordingDefault</term>
    ///   <description>[Meter,Register] What is the default value for recording</description>
    /// </item>
    /// <item>
    ///   <term>RegisterRecordingItems</term>
    ///   <description>[0,1] Controls contents of the Register Recording Selection dropdown list. Which one first and so on</description>
    /// </item>
    /// <item>
    ///   <term>RegisterRecordingReq</term>
    ///   <description>Required to choose if RegisterRecording is “true” on the screen</description>
    /// </item>
    /// <item>
    ///   <term>ReportLogLocationStatus</term>
    ///   <description>Specifies whether to display warning messages if log location is in volatile memory</description>
    /// </item>
    /// <item>
    ///   <term>ReverseReading</term>
    ///   <description>Enables or disables entry of old and new meter readings in reverse format</description>
    /// </item>
    /// <item>
    ///   <term>ScanDetail</term>
    ///   <description>Scan meter Manufacture Bar Code</description>
    /// </item>
    /// <item>
    ///   <term>ScanDetailLength</term>
    ///   <description>[1-99] Scan Meter Manufacture Bar Code length</description>
    /// </item>
    /// <item>
    ///   <term>scanfield</term>
    ///   <description>[Custom] Description of Manufacturing Barcode</description>
    /// </item>
    /// <item>
    ///   <term>ScanMtu</term>
    ///   <description>Enables STAR Programmer to read an electricMTU if the MTU does not have power</description>
    /// </item>
    /// <item>
    ///   <term>ScanSumCheck</term>
    ///   <description>[Not used yet] Scan Manufacture Bar Code</description>
    /// </item>
    /// <item>
    ///   <term>ScriptOnly</term>
    ///   <description>Specifies whether the application should log to the permanent log file in addition to the results log ( Only applies in scripted mode )</description>
    /// </item>
    /// <item>
    ///   <term>SecondNormXmitCondition</term>
    ///   <description>[Custom] Second transmit interval condition. For F1 MTUs only. Works with old and new MTUs</description>
    /// </item>
    /// <item>
    ///   <term>SecondNormXmitField</term>
    ///   <description>[Custom] Corresponding Field in AppointmentFile</description>
    /// </item>
    /// <item>
    ///   <term>SecondNormXmitInterval</term>
    ///   <description>[Custom] Alternate transmit interval in hours or minutes. Works for F1 only. The same values as regular read interval</description>
    /// </item>
    /// <item>
    ///   <term>SerialNumLabel</term>
    ///   <description>[1-20] Labels the Serial Number from Appointment</description>
    /// </item>
    /// <item>
    ///   <term>ShowAddMTU</term>
    ///   <description>Display or hide the “Add MTU” programming option on the main menu</description>
    /// </item>
    /// <item>
    ///   <term>ShowAddMTUMeter</term>
    ///   <description>Display or hide the “Add MTU and Meter” programming option on the main menu</description>
    /// </item>
    /// <item>
    ///   <term>ShowAddMTUReplaceMeter</term>
    ///   <description>Display or hide the “Add MTU and Replace Meter” programming option on the main menu</description>
    /// </item>
    /// <item>
    ///   <term>ShowFreq</term>
    ///   <description>Show the MTU operating frequency in the Read MTU display</description>
    /// </item>
    /// <item>
    ///   <term>ShowInstallConfirmation</term>
    ///   <description>Show the Installation Confirmation Button on the main screen</description>
    /// </item>
    /// <item>
    ///   <term>ShowMeterVendor</term>
    ///   <description>Specifies whether to display vendor and model information for meters during MTU programming</description>
    /// </item>
    /// <item>
    ///   <term>ShowReplaceMeter</term>
    ///   <description>Display or hide the “Replace Meter” programming option on the main menu</description>
    /// </item>
    /// <item>
    ///   <term>ShowReplaceMTU</term>
    ///   <description>Display or hide the Replace MTU programmingoption on the main menu</description>
    /// </item>
    /// <item>
    ///   <term>ShowReplaceMTUMeter</term>
    ///   <description>Display or hide the “Replace MTU And Meter”programming option on the main menu</description>
    /// </item>
    /// <item>
    ///   <term>ShowDataRead</term>
    ///   <description>Display or hide the “DataRead” programming option on the main menu</description>
    /// </item>
    /// <item>
    ///   <term>ShowScriptErrorMessage</term>
    ///   <description>Specifies whether error messages are shown in scripted mode</description>
    /// </item>
    /// <item>
    ///   <term>ShowTime</term>
    ///   <description>Show System time on the screen during the MTU read or install</description>
    /// </item>
    /// <item>
    ///   <term>ShowTurnOff</term>
    ///   <description>Show Turn Off MTU button on main screen</description>
    /// </item>
    /// <item>
    ///   <term>Siesta</term>
    ///   <description>Turn off wake up during installation for F1Flatpack</description>
    /// </item>
    /// <item>
    ///   <term>SpecialSet</term>
    ///   <description>[NOT NEED TO SUPPORT = NOT CONFIRMED][16bytes] Encryption Initial Vector</description>
    /// </item>
    /// <item>
    ///   <term>StartPoint</term>
    ///   <description>Tag name in Appointments file that containsthe Start Date</description>
    /// </item>
    /// <item>
    ///   <term>SystemDateVerify</term>
    ///   <description>Verify System Date</description>
    /// </item>
    /// <item>
    ///   <term>TempXmitCount</term>
    ///   <description>Default temporary reading transmit durationin days. Available for Legacy Flatpack Only</description>
    /// </item>
    /// <item>
    ///   <term>TempXmitInterval</term>
    ///   <description>Default temporary transmit/read intervals in hours or minutes</description>
    /// </item>
    /// <item>
    ///   <term>TimeSyncCountDefault</term>
    ///   <description>[0-255] How long to check (in seconds) after requesting Time Sync</description>
    /// </item>
    /// <item>
    ///   <term>TimeSyncCountRepeat</term>
    ///   <description>NOT PRESENT IN THE PDF // Number of attempts to for the installation confirmation process</description>
    /// </item>
    /// <item>
    ///   <term>TimeToSync</term>
    ///   <description>Enable MTU time sync</description>
    /// </item>
    /// <item>
    ///   <term>TimeToSyncHR</term>
    ///   <description>[0-23] Hour at which MTU listens for time sync</description>
    /// </item>
    /// <item>
    ///   <term>TimeToSyncMin</term>
    ///   <description>[0-59] Minute at which MTU listens for timesync</description>
    /// </item>
    /// <item>
    ///   <term>UploadPrompt</term>
    ///   <description>NOT PRESENT IN THE PDF // Show a popup whenuser log out remembering that has pending files to upload</description>
    /// </item>
    /// <item>
    ///   <term>Use83</term>
    ///   <description>[NOT NEED TO SUPPORT = NOT CONFIRMED] Specifies whether to use 8.3 formatted file names for log files</description>
    /// </item>
    /// <item>
    ///   <term>UseMeterSerialNumber</term>
    ///   <description>Enables or disables the recording of meter serial numbers</description>
    /// </item>
    /// <item>
    ///   <term>UserIdMaxLength</term>
    ///   <description>[1-10] Sets the maximum logon user ID length in characters</description>
    /// </item>
    /// <item>
    ///   <term>UserIdMinLength</term>
    ///   <description>[1-10] Sets the minimum logon user ID length in characters</description>
    /// </item>
    /// <item>
    ///   <term>UTCOffset</term>
    ///   <description>[Byte] Universal Time Offset for Electric MTU Type 92</description>
    /// </item>
    /// <item>
    ///   <term>WakeUpCount</term>
    ///   <description>Number of wake up messages to initially send. Old FlatPack only</description>
    /// </item>
    /// <item>
    ///   <term>WorkOrderDualEntry</term>
    ///   <description>Enables or disables dual entry of the work order number during programming</description>
    /// </item>
    /// <item>
    ///   <term>WorkOrderEnabledAppt</term>
    ///   <description>Enable or disable values after appointment in interactive mode ( Work Order )</description>
    /// </item>
    /// <item>
    ///   <term>WorkOrderLabel</term>
    ///   <description>[1-15] The label for the Work Order Number field</description>
    /// </item>
    /// <item>
    ///   <term>WorkOrderLength</term>
    ///   <description>[1-20] Maximum length in characters of the Work Order Number field</description>
    /// </item>
    /// <item>
    ///   <term>WorkOrderRecording</term>
    ///   <description>Enables or disables work order number recording during MTU programming</description>
    /// </item>
    /// <item>
    ///   <term>WriteDelay</term>
    ///   <description>[0-5000] Additional delay in milliseconds after write to MTU</description>
    /// </item>
    /// <item>
    ///   <term>WriteF1SystemTime</term>
    ///   <description>F1 write MTU system time from handheld or PC</description>
    /// </item>
    /// <item>
    ///   <term>XmitTimer</term>
    ///   <description>[UInt] Transmit Timer</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>&#160;</para>
    /// </summary>
    /// <remarks>
    /// NOTE: The values set in the constructor of the class are the default
    /// values that are used when a tag is not present in the configuration file.
    /// </remarks>
    [XmlRoot("Globals")]
    public class Global
    {
        public Global ()
        {
            // Default values and comments are extracted from Y20318-TUM_Rev_E.PDF and STAR Programmer source code

            //this.Cancel                     = [Custom] Cancel options
            //this.Options                    = [Custom] Misc Fields. Needs new section to describe

            this.AccountDualEntry             = true;                 // Enables or disables dual entry for AccountNumber
            this.AccountEnabledAppt           = false;                // [LOW PRIORITY = NOT CONFIRMED] Enable or disable values after appointment in interactive mode ( Account Number )
            this.AccountFilled                = false;                // Allows AccountNumber to be filled with MTU ID automatically
            this.AccountLabel                 = "Service Pt. ID";     // [1-15] Label for AccountNumber field
            this.AccountLength                = 7;                    // [1-12] Number of digits to use in AccountNumber field
            this.ActionVerify                 = true;                 // Display popup to confirm that user wants to perform the action
            this.Address1Len                  = 100;                  // Length of appointment address line 1 field
            this.Address2Len                  = 100;                  // ... 2 field
            this.Address3Len                  = 100;                  // ... 3 field
            this.Address4Len                  = 100;                  // ... 4 field
            this.AddressLine1                 = string.Empty;         // [Custumer specific] Definition of line 1 of verified addres in appointment file ( e.g. HouseNo )
            this.AddressLine2                 = string.Empty;         // ... line 2 ... ( e.g. Street )
            this.AddressLine3                 = string.Empty;         // ... line 3 ... ( e.g. N/A )
            this.AddressLine4                 = string.Empty;         // ... line 4 ... ( e.g. N/A )
            this.AddressVerify                = false;                // Set to true is appointment address should be verified
            this.AFC                          = true;                 // Advance Frequency Change on 3000 MTUs after firmware 19
            this.Allow2Port1Appt              = false;                // Allow use of dual port MTUs for one port appointments
            this.AllowAbort                   = false;                // Adds an abort button on cancellation screen during installation
            this.AllowDailyReads              = true;                 // Enable SnapReads/DailyReads
            this.AppointmentsLocation         = string.Empty;         // Specifies an alternate location for the Appointments file ( PC only )
            this.AppointmentsName             = "Appointments.xml";   // [LOW PRIORITY = NOT CONFIRMED][1-255] Name of the appointments file
            this.AppointmentsReq              = false;                // Forces you to accept the appointment before being allowed to program the MTU
            this.ApptGridConditionStatus      = "All";                // [LOW PRIORITY = NOT CONFIRMED][All,New,Plan] Controls contents of conditional grid
            this.ApptInstallerId              = false;                // Works only if AppointmentsReq is true and will add ( as prefix ) installer ID to the activity file with installer id name from appointments file
            this.ApptInstallerName            = "ApptInstallerName";  // [LOW PRIORITY = NOT CONFIRMED][1-100] Installer name indicates the tag to use in appointments file
            this.ApptPort1Field               = string.Empty;         // Saving fields in addition to the status
            this.ApptPort2Field               = string.Empty;         // Saving fields in addition to the status
            this.ApptSave                     = false;                // Allows editing and saving changes to the appointments file
            this.ApptScreen                   = false;                // [LOW PRIORITY = NOT CONFIRMED] Enables or disables display of the appointments screen
            this.ApptStatusDefinitions        = string.Empty;         // Used to display Appointment Statuses
            this.AreYouSure                   = false;                // Displays "Are you sure?" confirmation messages if true
            this.AutoNewMeterPort2isTheSame   = false;                // Automatically copies values from Port 1 to Port 2
            this.AutoPurge                    = false;                // Enables / disables automatic purging of log files
            this.AutoPurgeSize                = 0;                    // [x-9999] Maximum size in kilobytes for log file before auto purge if AutoPurge is true
            this.AutoRegisterRecording        = false;                // Records Register selection in Activity Log based on the old meter serial number and new serial number
            this.AutoRFCheck                  = false;                // If true, it forces to execute the NodeDiscovery process even though the IC has failed
            this.ByPassAutoDetect             = false;                // Bypass F1 Ecoder/Encoder Autodetect
            this.CertPair                     = false;                // Whether a certificate pair (i.e. Public and Private) is installed ( PC only )
            this.CertPath                     = string.Empty;         // Key Store Path for certificate ( PC Only )
            this.CertPswd                     = string.Empty;         // Digital Certificate Password if Full certificate stored
            this.CertPubKey                   = string.Empty;         // [Base64] Digital Certificate Public Key
            this.CertRecord                   = true;                 // Copy Certificate attributes into Global.xml ( PC Only )
            this.CertSubject                  = string.Empty;         // Digital Certificate Subject Search ( PC Only )
            this.CertThumbprint               = string.Empty;         // NOT PRESENT IN KG.CODE // Unique Value of Digital Certificate
            this.CertUpdate                   = string.Empty;         // [DateTime] Date on which certificate must be updated
            this.CertUpdateValid              = false;                // If true, application makes CertUpdate one day prior to Certvalid
            this.CertValid                    = string.Empty;         // [DateTime] CertValidActual certificate expiration date. Application will not run if current date is past this date
            this.CheckConfigFiles             = false;                //Check for updated XML config files and certificate            
            this.CheckMTUfield                = string.Empty;         // What field to use with CheckMtuType tag
            this.CheckMTUtype                 = false;                // Check if this MTU is suitable for this appointment
            this.CheckMTUvalue                = string.Empty;         // What value to compare
            this.CheckSavedField              = false;                // Checks and saves fields in appointment file
            this.CoilDetect                   = true;                 // Check coil presence
            this.ColorEntry                   = false;                // Uses a color screen for newer model handhelds
            this.ComPort                      = string.Empty;         // [COM1-COM9] Default COM port to use if the CF ( compact flash ) interface card is not found
            this.ConfigLocation               = string.Empty;         // Specifies an alternate location for the permanent configuration log file ( PC Only )
            this.ConfigPublicDir              = false;                // Specifies if the config files remains in the public folder or not, after copy the files in private folder
            this.CustomerMeterError           = string.Empty;         // [1-20] If meter error occurs, this message will appear
            this.CustomerName                 = "Aclara";             // [1-20] Display the tag value in the About sceen
            this.DailyReadingOffset           = 0;                    // [Byte] Daily Reading Offset for Electric MTU Type 92
            this.DailyReadsDefault            = 13;                   // [0-23] Hour (in military time) for daily reads/snap
            this.dangerosZone                 = "cache disk";         // PPC W 5.0 and Up volatile area "flash file store"
            this.DefaultF1ReadInterval        = false;                // [NOT NEED TO SUPPORT = NOT CONFIRMED] Use [1Hr] Interval for F1 radio. Allows using 2 read intervals for old and new flat packs
            this.DeviceCertSubject            = string.Empty;         // Actual certificate subject
            this.ElectricPort1_2Interval      = 60;                   // [NOT NEED TO SUPPORT = NOT CONFIRMED][5,10,15,20,30,60,120,180,240] Default transmit/read intervals in minutes for Port 1 and Port 2 of electric MTUs
            this.ElectricPort3_4Interval      = 120;                  // [NOT NEED TO SUPPORT = NOT CONFIRMED][5,10,15,20,30,60,120,180,240] Default transmit/read intervals in minutes for Port 3 and Port 4 of electric MTUs
            this.EMeterConnected              = false;                // NOT PRESENT IN THE PDF // KG: To display the meter info in full read MTU
            this.EnableFEC                    = false;                // Enable Forward Error Correction (FEC) for Electric MTU
            this.ErrorId                      = false;                // Add Error Id numbers to Activity Log error messages
            this.F12WAYRegister1              = 0x01C7D011;           // PDF: 0x01C75011 // [4bytes Hex or Decimals] Frequency value for channel 1
            this.FastMessageConfig            = false;                // To make migratable switch for migratable MTUs => Bit 0: Fast Messaging Mode - 0=Off, 1=On Location 163 Migratable
            this.F12WAYRegister10             = 0x0130360A;           // ... channel 10
            this.F12WAYRegister14             = 0x021B5021;           // ... channel 14
            this.F1TamperCheck                = false;                // Perform continuity test between MTU and Meter for Pulse
            this.forceElectricMtuOn           = true;                 // Forcibly turn on electric MTUs except types 74 and 75 ( Landis & Gyr Focus AX )
            this.ForceTimeSync                = false;                // Force an Installation Confirmation during MTU installation for 3000 MTU
            this.ftpPassword                  = string.Empty;         // FTP login password ( PC Only )
            this.ftpRemoteHost                = string.Empty;         // FTP remote host name ( PC Only )
            this.ftpRemotePath                = string.Empty;         // Path to FTP remote host ( PC Only )
            this.ftpTransferredPath           = string.Empty;         // FTP Parameters ( PC Only )
            this.ftpUserName                  = string.Empty;         // FTP login user name ( PC Only )
            this.FullResult                   = false;                // Makes Result file schema identical to that of the Activity Log
            this.FutureDate                   = 7;                    // [1-255] The number of days from the appointment's scheduled start date within which it is acceptable for work to startInteger
            this.GetMtuDelay                  = 0;                    // [1-5000] If required, contact Engineering for details. MTU read delay
            this.GpsBaudRate                  = 9600;                 // [9600,4800] GPS Communication Baud Rate
            this.GpsComPort                   = "COM2";               // [NOT NEED TO SUPPORT = NOT CONFIRMED][COMX] Specifies the COM port for GPS tracking
            this.GpsMetric                    = false;                // Record the result in Metric or Imperial
            this.gridColumn                   = string.Empty;         // [Custom] Description for Grid
            this.HideProgressScreen           = false;                // Hides the Progress Bar during MTU installation or reading
            this.HideRevision                 = false;                // Hides Revision number in About screen
            this.HourToAdjust                 = 0;                    // Use Hours to randomize for 3000 MTU
            this.ICfield1                     = 0;                    // [LOW PRIORITY = NOT CONFIRMED][0-100] Installation Confirmation field free-form description
            this.ICfield2                     = 0;                    // [LOW PRIORITY = NOT CONFIRMED][0-100] Installation Confirmation field free-form description
            this.IndividualDailyReads         = true;                 // Allow selection of daily Reads interval value
            this.IndividualReadInterval       = false;                // Controls whether the read interval can be specified on a per MTU basis or default only
            this.LatestVersion                = 16;                   // [Byte] F1 MTU Good firmware version. Specifies the minimum version that can have a 20-minute read interval
            this.LiveDigitsOnly               = true;                 // If true it will show X for Dummy Digits. False it will show 0
            this.LoadOptions                  = false;                // Load options screen through appointments
            this.LogLocation                  = string.Empty;         // Specifies an alternate location for the Activity Log file ( PC Only )
            this.LogsPublicDir                = false;                //specifies if the logs files are in the public folder or in the privte folder
            this.MeterNumberLength            = 12;                   // [1-20] Maximum length of the meter serial number
            this.MeterSerialEnabledAppt       = false;                // Enable or disable values after appointment in interactive mode ( Meter Old/Serial Number )
            this.MeterWorkRecording           = true;                 // Enables or disables the "Old Meter Working" dialog box during MTU programming
            this.MinDate                      = string.Empty;         // If the handheld reports a date earlier than “MinDate”, STAR Programmer stops working in order to prevent the wrong date from being entered
            this.MtuIdLength                  = 8;                    // [5-11] Number of digits in the MTU ID, including leading zeros
            this.NewMeterCalc                 = string.Empty;         // Prefix of new meter ID
            this.NewMeterFormat               = string.Empty;         // Sets guidelines for entering a new meter
            this.NewMeterLabel                = "New Meter #";        // [1-15] Optional parameter for new meter
            this.NewMeterPort2isTheSame       = false;                // Optionally copies Port 1 data to Port 2
            this.NewMeterPrefix               = false;                // Enables adding prefix to meter number
            this.NewMeterValidation           = false;                // Enable New Meter navigation/validation based on prefix and actual meter
            this.NewSerialNumDualEntry        = true;                 // Enables or disables dual entry of the new meter serial number during programming
            this.NormXmitInterval             = "1 Hr";               // Default transmit interval
            this.OldReadingDualEntry          = true;                 // F1 Flatpack 2000/3000 series up to version 2.1.1 and firmware up to 15
            this.OldReadingRecording          = true;                 // Enables or disables dual entry of the old meter reading during programming
            this.OldSerialNumDualEntry        = true;                 // Enables or disables entry of old meter reading
            this.OtherCancelCode              = string.Empty;         // [1-15] Use a special code for other cancel option
            this.OverWriteAutoDetect          = false;                // To show overwrite button on the sceen
            this.PasswordMaxLength            = 10;                   // [1-10] Sets the maximum logon user password length in characters
            this.PasswordMinLength            = 1;                    // [1-10] Sets the minimum logon user password length in characters
            this.PlaySound                    = false;                // Play sound on HH after Installation Confirmation is complete
            this.Port2DisableNo               = false;                // Disable ability to click on Display checkbox to disable Port2
            this.Port2MeterTypeTheSameWarning = false;                // Display a warning if Meter types are the same for dual port MTU
            this.PowerPolicy                  = true;                 // Disable/Enable USB Power programatically
            this.ReadDelay                    = 5;                    // [1-10] Number of seconds of wait time before reading or writing the MTU
            this.ReadingDualEntry             = true;                 // Enables or disables dual entry of the meter reading during programming
            this.RegisterRecording            = true;                 // Enables or disables display of the Register/Meter change question during programming
            this.RegisterRecordingDefault     = "Meter";              // [Meter,Register] What is the default value for recording
            this.RegisterRecordingItems       = "101";                // [0,1] Controls contents of the Register Recording Selection dropdown list. Which one first and so on
            this.RegisterRecordingReq         = false;                // Required to choose if RegisterRecording is “true” on the screen
            this.ReportLogLocationStatus      = true;                 // Specifies whether to display warning messages if log location is in volatile memory
            this.ReverseReading               = false;                // Enables or disables entry of old and new meter readings in reverse format
            this.ScanDetail                   = false;                // Scan meter Manufacture Bar Code
            this.ScanDetailLength             = 9;                    // [1-99] Scan Meter Manufacture Bar Code length
            this.scanfield                    = string.Empty;         // [Custom] Description of Manufacturing Barcode
            this.ScanMtu                      = false;                // Enables STAR Programmer to read an electric MTU if the MTU does not have power
            this.ScanSumCheck                 = false;                // [Not used yet] Scan Manufacture Bar Code
            this.ScriptOnly                   = true;                 // Specifies whether the application should log to the permanent log file in addition to the results log ( Only applies in scripted mode )
            this.SecondNormXmitCondition      = string.Empty;         // [Custom] Second transmit interval condition. For F1 MTUs only. Works with old and new MTUs
            this.SecondNormXmitField          = string.Empty;         // [Custom] Corresponding Field in Appointment File
            this.SecondNormXmitInterval       = string.Empty;         // [Custom] Alternate transmit interval in hours or minutes. Works for F1 only. The same values as regular read interval
            this.SerialNumLabel               = "Meter #";            // [1-20] Labels the Serial Number from Appointment
            this.ShowAddMTU                   = true;                 // Display or hide the “Add MTU” programming option on the main menu
            this.ShowAddMTUMeter              = true;                 // Display or hide the “Add MTU and Meter” programming option on the main menu
            this.ShowAddMTUReplaceMeter       = true;                 // Display or hide the “Add MTU and Replace Meter” programming option on the main menu
            this.ShowFreq                     = false;                // Show the MTU operating frequency in the Read MTU display
            this.ShowInstallConfirmation      = false;                // Show the Installation Confirmation Button on the main screen
            this.ShowMeterVendor              = false;                // Specifies whether to display vendor and model information for meters during MTU programming
            this.ShowReplaceMeter             = true;                 // Display or hide the “Replace Meter” programming option on the main menu
            this.ShowReplaceMTU               = true;                 // Display or hide the "Replace MTU" programming option on the main menu
            this.ShowReplaceMTUMeter          = true;                 // Display or hide the “Replace MTU And Meter” programming option on the main menu
            this.ShowDataRead                 = true;                // Display or hide the “DataRead” programming option on the main menu
            this.ShowScriptErrorMessage       = true;                 // Specifies whether error messages are shown in scripted mode
            this.ShowTime                     = false;                // Show System time on the screen during the MTU read or install
            this.ShowTurnOff                  = true;                 // Show Turn Off MTU button on main screen
            this.Siesta                       = false;                // Turn off wake up during installation for F1 Flatpack
            this.SpecialSet                   = "SOLONROCKSACLARA";   // [NOT NEED TO SUPPORT = NOT CONFIRMED][16bytes] Encryption Initial Vector
            this.StartPoint                   = string.Empty;         // Tag name in Appointments file that contains the Start Date
            this.SystemDateVerify             = false;                // Verify System Date
            this.TempXmitCount                = 0;                    // Default temporary reading transmit duration in days. Available for Legacy Flatpack Only
            this.TempXmitInterval             = string.Empty;         // Default temporary transmit/read intervals in hours or minutes
            this.TimeSyncCountDefault         = 63;                   // [0-255] How long to check (in seconds) after requesting Time Sync
            this.TimeSyncCountRepeat          = 1;                    // NOT PRESENT IN THE PDF // [1-3] Number of attempts to for the installation confirmation process
            this.TimeToSync                   = false;                // Enable MTU time sync
            this.TimeToSyncHR                 = 0;                    // [0-23] Hour at which MTU listens for time sync
            this.TimeToSyncMin                = 0;                    // [0-59] Minute at which MTU listens for time sync
            this.UploadPrompt                 = false;                // NOT PRESENT IN THE PDF // Show a popup when user log out remembering that has pending files to upload
            this.Use83                        = false;                // [NOT NEED TO SUPPORT = NOT CONFIRMED] Specifies whether to use 8.3 formatted file names for log files
            this.UseMeterSerialNumber         = true;                 // Enables or disables the recording of meter serial numbers
            this.UserIdMaxLength              = 10;                   // [1-10] Sets the maximum logon user ID length in characters
            this.UserIdMinLength              = 1;                    // [1-10] Sets the minimum logon user ID length in characters
            this.UTCOffset                    = 235;                  // [Byte] Universal Time Offset for Electric MTU Type 92
            this.WakeUpCount                  = 32;                   // Number of wake up messages to initially send. Old FlatPack only
            this.WorkOrderDualEntry           = true;                 // Enables or disables dual entry of the work order number during programming
            this.WorkOrderEnabledAppt         = false;                // Enable or disable values after appointment in interactive mode ( Work Order )
            this.WorkOrderLabel               = "Field Order";        // [1-15] The label for the Work Order Number field
            this.WorkOrderLength              = 15;                   // [1-20] Maximum length in characters of the Work Order Number field
            this.WorkOrderRecording           = true;                 // Enables or disables work order number recording during MTU programming
            this.WriteDelay                   = 0;                    // [0-5000] Additional delay in milliseconds after write to MTU
            this.WriteF1SystemTime            = false;                // F1 write MTU system time from handheld or PC
            this.XmitTimer                    = 0;                    // [UInt] Transmit Timer
            
            // DataRead
            this.NumOfDays                    = 1;                    // Number of days used for recover DataRead events

            // Node Discovery
            this.MinNumDCU                    = 1;                    // Minimum number of DCUs
            this.GoodNumDCU                   = 3;                    // Good number of DCUs
            this.MinF1Rely                    = 75m;                  // Minimum F1 frequency threshold
            this.MinF2Rely                    = 50m;                  // Minimum F2 frequency threshold
            this.GoodF1Rely                   = 98.5m;                // Good F1 frequency threshold
            this.GoodF2Rely                   = 75m;                  // Good F2 frequency threshold
            this.MaxTimeRFCheck               = 60;                   // Max time in seconds to perform the NodeDiscovery process ( Value set to NodeDiscoveryTime in STAR Programmer )

            // New parameters/tags ( Own )
            
            // gzip is simply deflate plus a checksum and header/footer. Deflate is faster and smaller
            // So naturally, no checksum is faster but then you also can't detect corrupt streams
            this.Compression                  = string.Empty;         // "", "gzip" and "deflate"
        
            #region Collections
        
            this.Cancel_Deserialized = new List<string> ();
            this.Cancel_Default      = new List<string> ()
            {
                "Not Home",
                "Meter Missing",
                "Bored",
                "On Strike",
                "Quit"
            };
            
            this.Options_Deserialized = new List<Option> ();
            this.Options_Default      = new List<Option> ()
            {
                new Option ()
                {
                    Name     = "LocationInfo",
                    Display  = "MTU Location",
                    Type     = "list",
                    Required = true,
                    OptionList = new List<string> ()
                    {
                        "Outside",
                        "Inside",
                        "Basement"
                    }
                },
                new Option ()
                {
                    Name     = "LocationInfo",
                    Display  = "Meter Location",
                    Type     = "list",
                    Required = true,
                    OptionList = new List<string> ()
                    {
                        "Outside",
                        "Inside",
                        "Basement"
                    }
                },
                new Option ()
                {
                    Name     = "Construction",
                    Display  = "Construction",
                    Type     = "list",
                    Required = false,
                    OptionList = new List<string> ()
                    {
                        "Vinyl",
                        "Wood",
                        "Brick",
                        "Aluminum",
                        "Other"
                    }
                }
            };

            #endregion
        }

        #region Tags

        public bool IsFtpUploadSet
        {
            get
            {
                return ! string.IsNullOrEmpty ( this.ftpRemoteHost ) &&
                       ! string.IsNullOrEmpty ( this.ftpRemotePath ) &&
                       ! string.IsNullOrEmpty ( this.ftpUserName   ) &&
                       ! string.IsNullOrEmpty ( this.ftpPassword   ) &&
                       ! string.IsNullOrEmpty ( this.ftpRemoteHost );
            }
        }

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

        [XmlElement("CheckConfigFiles")]
        public bool CheckConfigFiles { get; set; }

        [XmlElement("CoilDetect")]
        public bool CoilDetect { get; set; }

        [XmlElement("ColorEntry")]
        public bool ColorEntry { get; set; }

        [XmlElement("ComPort")]
        public string ComPort { get; set; }

        [XmlElement("ConfigLocation")]
        public string ConfigLocation { get; set; }

        [XmlElement("ConfigPublicDir")]
        public bool ConfigPublicDir { get; set; }

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
        public bool DefaultF1ReadInterval { get; set; }

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
        public uint F12WAYRegister1 { get; set; }

        [XmlElement("F12WAYRegister10")]
        public uint F12WAYRegister10 { get; set; }

        [XmlElement("F12WAYRegister14")]
        public uint F12WAYRegister14 { get; set; }

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

        [XmlElement("GoodF1Rely")]
        public decimal GoodF1Rely { get; set; }

        [XmlElement("GoodF2Rely")]
        public decimal GoodF2Rely { get; set; }

        [XmlElement("GoodNumDCU")]
        public int GoodNumDCU { get; set; }

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
        public int HourToAdjust { get; set; }

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

        [XmlElement("LogsPublicDir")]
        public bool LogsPublicDir { get; set; }

        [XmlElement("MaxTimeRFCheck")]
        public int MaxTimeRFCheck { get; set; }

        [XmlElement("MeterNumberLength")]
        public int MeterNumberLength { get; set; }

        [XmlElement("MeterSerialEnabledAppt")]
        public bool MeterSerialEnabledAppt { get; set; }

        [XmlElement("MeterWorkRecording")]
        public bool MeterWorkRecording { get; set; }

        [XmlElement("MinDate")]
        public string MinDate { get; set; }

        [XmlElement("MinF1Rely")]
        public decimal MinF1Rely { get; set; }

        [XmlElement("MinF2Rely")]
        public decimal MinF2Rely { get; set; }

        [XmlElement("MinNumDCU")]
        public int MinNumDCU { get; set; }

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

        [XmlElement("NumOfDays")]
        public int NumOfDays { get; set; }

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

        [XmlElement("ShowDataRead")]
        public bool ShowDataRead { get; set; }

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

        [XmlIgnore]
        public bool FastMessageConfig;

        [XmlElement("FastMessageConfig")]
        public bool FastMessageConfigXml
        {
            get { return this.FastMessageConfig; }
            set { this.FastMessageConfig = value; }
        }

        [XmlElement("Fast-2-Way")]
        public bool Fast2Way
        {
            get { return this.FastMessageConfig; }
            set { this.FastMessageConfig = value; }
        }
        
        [XmlIgnore]
        public List<Option> Options_Default;

        [XmlArray("Options")]
        [XmlArrayItem("option")]
        public List<Option> Options_Deserialized;
        
        [XmlIgnore]
        public List<Option> Options
        {
            get
            {
                if ( this.Options_Deserialized.Count <= 0 )
                    return this.Options_Default;
                return this.Options_Deserialized;
            }
        }
        
        [XmlIgnore]
        public List<string> Cancel_Default;
        
        [XmlArray("Cancel")]
        [XmlArrayItem("option")]
        public List<string> Cancel_Deserialized;

        [XmlIgnore]
        public List<string> Cancel
        {
            get
            {
                if ( this.Cancel_Deserialized.Count <= 0 )
                    return this.Cancel_Default;
                return this.Cancel_Deserialized;
            }
        }

        #endregion
        
        #region New Tags ( Own )
        
        [XmlElement("Compression")]
        public string Compression { get; set; }

        #endregion
    }
}
