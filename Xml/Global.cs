using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    /// <summary>
    /// Class used to map the Global.xml configuration file.
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Display or Hide actions
    /// <list type="Display">
    /// <item>
    ///   <term>ShowAddMTU</term>
    ///   <description>Show the Add MTU button on the main menu or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowAddMTUMeter</term>
    ///   <description>Show the Add MTU and Meter button on the main menu or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowAddMTUReplaceMeter</term>
    ///   <description>Show the Add MTU and Replace Meter button on the main menu or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowBarCodeButton</term>
    ///   <description>Show the Barcode Scanner button on the main menu or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowCameraButton</term>
    ///   <description>Show the Camera button on the action screen or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowDataRead</term>
    ///   <description>Show the Data Read button or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowFreq</term>
    ///   <description>Show the MTU operating frequency in the Read MTU display</description>
    /// </item>
    /// <item>
    ///   <term>ShowInstallConfirmation</term>
    ///   <description>Show the Installation Confirmation button on the main menu or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowMeterVendor</term>
    ///   <description>Specifies whether to display vendor and model information for meters during MTU programming</description>
    /// </item>
    /// <item>
    ///   <term>ShowReplaceMeter</term>
    ///   <description>Show the Replace Meter button on the main menu or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowReplaceMTU</term>
    ///   <description>Show the Replace MTU button on the main menu or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowReplaceMTUMeter</term>
    ///   <description>Show the Replace MTU And Meter button on the main menu or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowTurnOff</term>
    ///   <description>Show the Turn Off MTU button on the main menu or not</description>
    /// </item>
    /// <item>
    ///   <term>ShowValvePosition</term>
    ///   <description>Show the Remote Disconnect button on the main menu or not</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// General Configuration
    /// <list type="Config">
    /// <item>
    ///   <term>ActionVerify</term>
    ///   <description>Display popup to confirm that user wants to perform the action</description>
    /// </item>
    /// <item>
    ///   <term>AutoRegisterRecording</term>
    ///   <description>Records Register selection in Activity Log based on the old meter serial number and new serial number</description>
    /// </item>
    /// <item>
    ///   <term>ErrorId</term>
    ///   <description>Add Error Id numbers to Activity Log error messages</description>
    /// </item>
    /// <item>
    ///   <term>LogsPublicDir</term>
    ///   <description>Specifies if the logs files are in the public folder or in the privte folder</description>
    /// </item>
    /// <item>
    ///   <term>MinDate</term>
    ///   <description>If the handheld reports a date earlier than “MinDate”, STAR Programmer stops working in order to prevent the wrong date from being entered</description>
    /// </item>
    /// <item>
    ///   <term>ScriptOnly</term>
    ///   <description>Specifies whether the application should log to the permanent log file in addition to the results log ( Only applies in scripted mode )</description>
    /// </item>
    /// <item>
    ///   <term>UploadPrompt</term>
    ///   <description>Show a popup when user log out remembering that has pending files to upload</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Labels
    /// <list type="Labels">
    /// <item>
    ///   <term>CustomerName</term>
    ///   <description>It is used to display the customer name on the Settings/About screen ( def. "Aclara" )</description>
    /// </item>
    /// <item>
    ///   <term>AccountLabel</term>
    ///   <description>It is used to identify the account number field in the form ( def. "Service Pt. ID" )</description>
    /// </item>
    /// <item>
    ///   <term>NewMeterLabel</term>
    ///   <description>It is used to identify the new meter reading field in the form ( def. "New Meter #" )</description>
    /// </item>
    /// <item>
    ///   <term>SerialNumLabel</term>
    ///   <description>It is used to identify the meter serial number field in the form ( default: "Meter #" )</description>
    /// </item>
    /// <item>
    ///   <term>WorkOrderLabel</term>
    ///   <description>It is used to identify the work order field in the form ( def. "Field Order" )</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Entries Length
    /// <list type="Lengths">
    /// <item>
    ///   <term>AccountLength</term>
    ///   <description>Number of digits to use in AccountNumber field ( [1-12] )</description>
    /// </item>
    /// <item>
    ///   <term>MeterNumberLength</term>
    ///   <description>Maximum length of the meter serial number ( [1-20] )</description>
    /// </item>
    /// <item>
    ///   <term>MtuIdLength</term>
    ///   <description>Number of digits in the MTU ID, including leading zeros ( [5-11] )</description>
    /// </item>
    /// <item>
    ///   <term>PasswordMaxLength</term>
    ///   <description>Sets the maximum logon user password length in characters ( [1-10] )</description>
    /// </item>
    /// <item>
    ///   <term>PasswordMinLength</term>
    ///   <description>Sets the minimum logon user password length in characters ( [1-10] )</description>
    /// </item>
    /// <item>
    ///   <term>UserIdMaxLength</term>
    ///   <description>Sets the maximum logon user ID length in characters ( [1-10] )</description>
    /// </item>
    /// <item>
    ///   <term>UserIdMinLength</term>
    ///   <description>Sets the minimum logon user ID length in characters ( [1-10] )</description>
    /// </item>
    /// <item>
    ///   <term>WorkOrderLength</term>
    ///   <description>Maximum length in characters of the Work Order Number field ( [1-20] )</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Form
    /// <list type="Form">
    /// <item>
    ///   <term>ColorEntry</term>
    ///   <description>Uses a color screen for newer model handhelds</description>
    /// </item>
    /// <item>
    ///   <term>Compression</term>
    ///   <description>
    /// Indicates whether or not ( by default ) to compress the result data sent in scripted mode ( nothing, deflate or gzip )
    /// <para>
    /// See <see cref="MTUComm.Compression"/>
    /// </para>
    ///   </description>
    /// </item>
    /// <item>
    ///   <term>NewMeterPort2isTheSame</term>
    ///   <description>Optionally copies Port 1 data to Port 2</description>
    /// </item>
    /// <item>
    ///   <term>LiveDigitsOnly</term>
    ///   <description>If true it will show X for Dummy Digits. False it will show 0</description>
    /// </item>
    /// <item>
    ///   <term>Port2DisableNo</term>
    ///   <description>Disable ability to click on Display checkbox to disable Port2</description>
    /// </item>
    /// <item>
    ///   <term>ReverseReading</term>
    ///   <description>Enables or disables entry of old and new meter readings in reverse format</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Enable or Disable fields
    /// <list type="Fields">
    /// <item>
    ///   <term>AllowDailyReads</term>
    ///   <description>Enable SnapReads/DailyReads</description>
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
    ///   <term>MeterWorkRecording</term>
    ///   <description>Enables or disables the "Old Meter Working" dialog box during MTU programming</description>
    /// </item>
    /// <item>
    ///   <term>OldReadingRecording</term>
    ///   <description>Enables or disables entry of the old meter reading during programming</description>
    /// </item>
    /// <item>
    ///   <term>RegisterRecording</term>
    ///   <description>Enables or disables display of the Register/Meter change question during programming</description>
    /// </item>
    /// <item>
    ///   <term>UseMeterSerialNumber</term>
    ///   <description>Enables or disables the recording of meter serial numbers</description>
    /// </item>
    /// <item>
    ///   <term>WorkOrderRecording</term>
    ///   <description>Enables or disables work order number recording during MTU programming</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Dual Entries
    /// <list type="Dual">
    /// <item>
    ///   <term>AccountDualEntry</term>
    ///   <description>Enables or disables dual entry for AccountNumber</description>
    /// </item>
    /// <item>
    ///   <term>NewSerialNumDualEntry</term>
    ///   <description>Enables or disables dual entry of the new meter serial number during programming</description>
    /// </item>
    /// <item>
    ///   <term>OldReadingDualEntry</term>
    ///   <description>F1 Flatpack 2000/3000 series up to version 2.1.1 and firmware up to 15</description>
    /// </item>
    /// <item>
    ///   <term>OldSerialNumDualEntry</term>
    ///   <description>Enables or disables entry of old meter reading</description>
    /// </item>
    /// <item>
    ///   <term>ReadingDualEntry</term>
    ///   <description>Enables or disables dual entry of the meter reading during programming</description>
    /// </item>
    /// <item>
    ///   <term>WorkOrderDualEntry</term>
    ///   <description>Enables or disables dual entry of the work order number during programming</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Register Recording
    /// <list type="Recording">
    /// <item>
    ///   <term>RegisterRecordingDefault</term>
    ///   <description>[Meter,Register] What is the default value for recording</description>
    /// </item>
    /// <item>
    ///   <term>RegisterRecordingItems</term>
    ///   <description>Controls contents of the Register Recording dropdown list, using one/1 for active status ( def. "101" )</description>
    /// </item>
    /// <item>
    ///   <term>RegisterRecordingReq</term>
    ///   <description>It is required to choose if RegisterRecording is “true” on the screen</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// S/FTP
    /// <list type="FTP">
    /// <item>
    ///   <term>ftpRemoteHost</term>
    ///   <description>Remote host name</description>
    /// </item>
    /// <item>
    ///   <term>ftpRemotePath</term>
    ///   <description>Remote path</description>
    /// </item>
    /// <item>
    ///   <term>ftpUserName</term>
    ///   <description>User name</description>
    /// </item>
    /// <item>
    ///   <term>ftpPassword</term>
    ///   <description>Password</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Read Internal
    /// <list type="ReadInterval">
    /// <item>
    ///   <term>LatestVersion</term>
    ///   <description>F1 MTU Good firmware version. Specifies the minimum version that can have a 20-minute read interval</description>
    /// </item>
    /// <item>
    ///   <term>NormXmitInterval</term>
    ///   <description>Default transmit interval</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Snap Reads
    /// <list type="SnapReads">
    /// <item>
    ///   <term>DailyReadsDefault</term>
    ///   <description>Hour (in military time) for daily reads/snap</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Frequencies
    /// <list type="Frequencies">
    /// <item>
    ///   <term>AFC</term>
    ///   <description>Advance Frequency Change on 3000 MTUs after firmware 19</description>
    /// </item>
    /// <item>
    ///   <term>F12WAYRegister1</term>
    ///   <description>Frequency value for channel 1</description>
    /// </item>
    /// <item>
    ///   <term>F12WAYRegister10</term>
    ///   <description>Frequency value for channel 10</description>
    /// </item>
    /// <item>
    ///   <term>F12WAYRegister14</term>
    ///   <description>Frequency value for channel 14</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// DataRead
    /// <list type="DataRead">
    /// <item>
    ///   <term>NumOfDays</term>
    ///   <description>Number of days used for recover DataRead events</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Install Confirmation
    /// <list type="IC">
    /// <item>
    ///   <term>ForceTimeSync</term>
    ///   <description>Forces an Installation Confirmation during the MTU installation for 3000 MTU</description>
    /// </item>
    /// <item>
    ///   <term>TimeSyncCountDefault</term>
    ///   <description>How long to check (in seconds) after requesting Time Sync</description>
    /// </item>
    /// <item>
    ///   <term>TimeSyncCountRepeat</term>
    ///   <description>Number of attempts to for the installation confirmation process</description>
    /// </item>
    /// <item>
    ///   <term>TimeToSync</term>
    ///   <description>Enables MTU time sync</description>
    /// </item>
    /// <item>
    ///   <term>TimeToSyncHR</term>
    ///   <description>Hour at which MTU listens for time sync</description>
    /// </item>
    /// <item>
    ///   <term>TimeToSyncMin</term>
    ///   <description>Minute at which MTU listens for time sync</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Node Discovery
    /// <list type="ND">
    /// <item>
    ///   <term>AutoRFCheck</term>
    ///   <description>It is used within the condition that allows to perform the Node Discovery process with OnDemand 1.2 MTUs</description>
    /// </item>
    /// <item>
    ///   <term>MinNumDCU</term>
    ///   <description>Minimum number of DCUs</description>
    /// </item>
    /// <item>
    ///   <term>GoodNumDCU</term>
    ///   <description>Good number of DCUs</description>
    /// </item>
    /// <item>
    ///   <term>MinF1Rely</term>
    ///   <description>Minimum F1 frequency threshold</description>
    /// </item>
    /// <item>
    ///   <term>MinF2Rely</term>
    ///   <description>Minimum F2 frequency threshold</description>
    /// </item>
    /// <item>
    ///   <term>GoodF1Rely</term>
    ///   <description>Good F1 frequency threshold</description>
    /// </item>
    /// <item>
    ///   <term>GoodF2Rely</term>
    ///   <description>Good F2 frequency threshold</description>
    /// </item>
    /// <item>
    ///   <term>MaxTimeRFCheck</term>
    ///   <description>Max time in seconds to perform the NodeDiscovery process ( Value set to NodeDiscoveryTime in STAR Programmer )</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// <para>
    /// Encryption for OnDemand 1.2
    /// <list type="NewEncryption">
    /// <item>
    ///   <term>BroadcastSet</term>
    ///   <description>Broadcast key used during the OnDemand 1.2 MTUs new encryption process</description>
    /// </item>
    /// <item>
    ///   <term>PublicKey</term>
    ///   <description>Public key used during the OnDemand 1.2 MTUs new encryption process</description>
    /// </item>
    /// </list>
    /// </para>
    /// 
    /// <para>&#160;</para>
    /// </summary>
    /// <remarks>
    /// NOTE: The values set in the constructor of the class are the default
    /// values that are used when a tag is not present in the configuration file.
    /// </remarks>
    [XmlRoot("Globals")]
    public class Global
    {
        #region Constants

        private const int DEF_LEXI_ATTEMPTS = 4;
        private const int MIN_LEXI_ATTEMPTS = 1;  // Original/initial attempt + 1 = 2
        public  const int MAX_LEXI_ATTEMPTS = 10; // Original/initial attempt + 10 = 11
        private const int DEF_LEXI_TIMEOUT  = 10;
        private const int MIN_LEXI_TIMEOUT  = 5;  // Seconds
        public  const int MAX_LEXI_TIMEOUT  = 30; // Seconds

        #endregion

        public Global ()
        {
            // Default values and comments are extracted from Y20318-TUM_Rev_E.PDF and STAR Programmer source code
            
            // NOTE: Is this tag used?
            this.FastMessageConfig            = false;                // To make migratable switch for migratable MTUs => Bit 0: Fast Messaging Mode - 0=Off, 1=On Location 163 Migratable
            
            // Display or hide
            this.ShowAddMTU                   = false;                // Show the Add MTU button on the main menu or not
            this.ShowAddMTUMeter              = false;                // Show the Add MTU and Meter button on the main menu or not
            this.ShowAddMTUReplaceMeter       = false;                // Show the Add MTU and Replace Meter button on the main menu or not
            this.ShowBarCodeButton            = true;                 // Show the Barcode Scanner button on the main menu or not
            this.ShowCameraButton             = true;                 // Show the Camera button on the action screen or not
            this.ShowDataRead                 = false;                // Show the Data Read button or not
            this.ShowFreq                     = false;                // Show the MTU operating frequency in the Read MTU display
            this.ShowInstallConfirmation      = false;                // Show the Installation Confirmation button on the main menu or not
            this.ShowMeterVendor              = false;                // Specifies whether to display vendor and model information for meters during MTU programming
            this.ShowReplaceMeter             = false;                // Show the Replace Meter button on the main menu or not
            this.ShowReplaceMTU               = false;                // Show the Replace MTU button on the main menu or not
            this.ShowReplaceMTUMeter          = false;                // Show the Replace MTU And Meter button on the main menu or not
            this.ShowTurnOff                  = false;                // Show the Turn Off MTU button on the main menu or not
            this.ShowValvePosition            = false;                // Show the Remote Disconnect button on the main menu or not

            // General Configuration
            this.ActionVerify                 = true;                 // Display popup to confirm that user wants to perform the action
            this.AutoRegisterRecording        = false;                // Records Register selection in Activity Log based on the old meter serial number and new serial number
            this.ErrorId                      = false;                // Add Error Id numbers to Activity Log error messages
            this.LogsPublicDir                = false;                // Specifies if the logs files are in the public folder or in the privte folder
            this.MinDate                      = string.Empty;         // If the handheld reports a date earlier than “MinDate”, STAR Programmer stops working in order to prevent the wrong date from being entered
            this.ScriptOnly                   = true;                 // Specifies whether the application should log to the permanent log file in addition to the results log ( Only applies in scripted mode )
            this.UploadPrompt                 = false;                // Show a popup when user log out remembering that has pending files to upload

            // Labels
            this.CustomerName                 = "Aclara";             // [1-20] Display the tag value in the About sceen
            this.AccountLabel                 = "Service Pt. ID";     // [1-15] Label for AccountNumber field
            this.NewMeterLabel                = "New Meter #";        // [1-15] Optional parameter for new meter
            this.SerialNumLabel               = "Meter #";            // [1-20] Labels the Serial Number from Appointment
            this.WorkOrderLabel               = "Field Order";        // [1-15] The label for the Work Order Number field

            // Lengths
            this.AccountLength                = 7;                    // [1-12] Number of digits to use in AccountNumber field
            this.MeterNumberLength            = 12;                   // [1-20] Maximum length of the meter serial number
            this.MtuIdLength                  = 8;                    // [5-11] Number of digits in the MTU ID, including leading zeros
            this.PasswordMaxLength            = 10;                   // [1-10] Sets the maximum logon user password length in characters
            this.PasswordMinLength            = 1;                    // [1-10] Sets the minimum logon user password length in characters
            this.UserIdMaxLength              = 10;                   // [1-10] Sets the maximum logon user ID length in characters
            this.UserIdMinLength              = 1;                    // [1-10] Sets the minimum logon user ID length in characters
            this.WorkOrderLength              = 15;                   // [1-20] Maximum length in characters of the Work Order Number field

            // Form
            this.ColorEntry                   = false;                // Uses a color screen for newer model handhelds
            this.NewMeterPort2isTheSame       = false;                // Optionally copies Port 1 data to Port 2
            this.LiveDigitsOnly               = false;                 // If true it will show 0 for Dummy Digits. False it will show X
            this.Port2DisableNo               = false;                // Disable ability to click on Display checkbox to disable Port2
            this.ReverseReading               = false;                // Enables or disables entry of old and new meter readings in reverse format

            // Enable or Disable form fields
            this.AllowDailyReads              = true;                 // Enable SnapReads/DailyReads
            this.IndividualDailyReads         = true;                 // Allow selection of daily Reads interval value
            this.IndividualReadInterval       = false;                // Controls whether the read interval can be specified on a per MTU basis or default only
            this.MeterWorkRecording           = true;                 // Enables or disables the "Old Meter Working" dialog box during MTU programming
            this.OldReadingRecording          = true;                 // Enables or disables entry of the old meter reading during programming
            this.RegisterRecording            = true;                 // Enables or disables display of the Register/Meter change question during programming
            this.UseMeterSerialNumber         = true;                 // Enables or disables the recording of meter serial numbers
            this.WorkOrderRecording           = true;                 // Enables or disables work order number recording during MTU programming

            // Dual Entries
            this.AccountDualEntry             = true;                 // Enables or disables dual entry for AccountNumber
            this.NewSerialNumDualEntry        = true;                 // Enables or disables dual entry of the new meter serial number during programming
            this.OldReadingDualEntry          = true;                 // F1 Flatpack 2000/3000 series up to version 2.1.1 and firmware up to 15
            this.OldSerialNumDualEntry        = true;                 // Enables or disables entry of old meter reading
            this.ReadingDualEntry             = true;                 // Enables or disables dual entry of the meter reading during programming
            this.WorkOrderDualEntry           = true;                 // Enables or disables dual entry of the work order number during programming

            // Register recording
            this.RegisterRecordingDefault     = "Meter";              // [Meter,Register] What is the default value for recording
            this.RegisterRecordingItems       = "111";                // [0,1] Controls contents of the Register Recording Selection dropdown list. Which one first and so on
            this.RegisterRecordingReq         = false;                // Required to choose if RegisterRecording is “true” on the screen

            // FTP
            this.ftpPassword                  = string.Empty;         // FTP login password
            this.ftpRemoteHost                = string.Empty;         // FTP remote host name
            this.ftpRemotePath                = string.Empty;         // Path to FTP remote host
            this.ftpUserName                  = string.Empty;         // FTP login user name

            // Read Internal
            this.LatestVersion                = 16;                   // [Byte] F1 MTU Good firmware version. Specifies the minimum version that can have a 20-minute read interval
            this.NormXmitInterval             = "1 Hr";               // Default transmit interval

            // Snap Reads
            this.DailyReadsDefault            = 13;                   // [0-23] Hour (in military time) for daily reads/snap

            // Frequencies
            this.AFC                          = true;                 // Advance Frequency Change on 3000 MTUs after firmware 19
            this.F12WAYRegister1              = 0x01C7D011;           // PDF: 0x01C75011 // [4bytes Hex or Decimals] Frequency value for channel 1
            this.F12WAYRegister10             = 0x0130360A;           // ... channel 10
            this.F12WAYRegister14             = 0x021B5021;           // ... channel 14

            // DataRead
            this.NumOfDays                    = 1;                    // Number of days used for recover DataRead events

            // Install Confirmation
            this.ForceTimeSync                = false;                // Force an Installation Confirmation during MTU installation for 3000 MTU
            this.TimeSyncCountDefault         = 63;                   // [0-255] How long to check (in seconds) after requesting Time Sync
            this.TimeSyncCountRepeat          = 3;                    // [1-3] Number of attempts to for the installation confirmation process
            this.TimeToSync                   = false;                // Enable MTU time sync
            this.TimeToSyncHR                 = 0;                    // [0-23] Hour at which MTU listens for time sync
            this.TimeToSyncMin                = 0;                    // [0-59] Minute at which MTU listens for time sync
            this.ArtificialTimeSync           = false;                // Force to indicate that the IC was successful and simulate it

            // Node Discovery
            this.AutoRFCheck                  = false;                // It is used within the condition that allows to perform the Node Discovery process with OnDemand 1.2 MTUs
            this.MinNumDCU                    = 1;                    // Minimum number of DCUs
            this.GoodNumDCU                   = 3;                    // Good number of DCUs
            this.MinF1Rely                    = 75m;                  // Minimum F1 frequency threshold
            this.MinF2Rely                    = 50m;                  // Minimum F2 frequency threshold
            this.GoodF1Rely                   = 98.5m;                // Good F1 frequency threshold
            this.GoodF2Rely                   = 75m;                  // Good F2 frequency threshold
            this.MaxTimeRFCheck               = 60;                   // Max time in seconds to perform the NodeDiscovery process ( Value set to NodeDiscoveryTime in STAR Programmer )

            // Remote Disconnect
            this.RDDFirmwareVersion           = string.Empty;         // RDD device firmware version

            // New Encryption ( OD 1.2 )
            this.BroadcastSet                 = string.Empty;         // Broadcast key used during the OnDemand 1.2 MTUs new encryption process
            this.PublicKey                    = string.Empty;         // Public key used during the OnDemand 1.2 MTUs new encryption process
            #if DEBUG
            this.BroadcastSet                 = "MTIzNDU2Nzg5MDEyMzQ1Njc4OTAxMjM0NTY3ODkwMTI="; // 12345678901234567890123456789012 = 32 bytes
            //this.PublicKey                    = "RUNLMSAAAACyuwIi50PTmsr1fy5RpVqEpM1jXUyXVN61xPKbj9ivQItW+Hei0TPa1fur/X7k7fy36seQ7jNdz5/iLnxrqlZT"; // = 72 - 8 = 64 bytes
            this.PublicKey                    = "RUNLMSAAAAANgbc9dncOLCyU/hgU5XqdDlb/SKbKxkMWXUKt/sT+2ETqwsKT6Y1FT738mCkIW5UASU4T75SF7Z/VSzKRZqkc";
            /*
            this.PublicKey                    = "25-FE-1B-41-81-00-01-02-03-04-05-06-07-08-09-0A-0B-0C-0D-0E-0F-10-11-12-13-" + // Konstantin's new key = 72 bytes in hex format
			                                    "14-15-16-17-18-19-1A-1B-1C-1D-1E-1F-20-01-02-03-04-05-06-07-08-09-0A-0B-0C-" +
			                                    "0D-0E-0F-10-11-12-13-14-15-16-17-18-19-1A-1B-1C-1D-1E-1F-20-BC-C2";
            */
            #endif

            // New parameters/tags ( Own )
            
            // gzip is simply deflate plus a checksum and header/footer. Deflate is faster and smaller
            // So naturally, no checksum is faster but then you also can't detect corrupt streams
            this.Compression                  = string.Empty;         // "", "gzip" and "deflate"

            // LExI communication attempts
            this.LexiAttempts                 = DEF_LEXI_ATTEMPTS;
            this.LexiTimeout                  = DEF_LEXI_TIMEOUT * 1000;  // to milliseconds
        
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

        [XmlElement("AccountDualEntry")]
        public bool AccountDualEntry { get; set; }

        [XmlElement("AccountLabel")]
        public string AccountLabel { get; set; }

        [XmlElement("AccountLength")]
        public int AccountLength { get; set; }

        [XmlElement("ActionVerify")]
        public bool ActionVerify { get; set; }

        [XmlElement("AFC")]
        public bool AFC { get; set; }

        [XmlElement("AllowDailyReads")]
        public bool AllowDailyReads { get; set; }

        [XmlElement("ArtificialTimeSync")]
        public bool ArtificialTimeSync { get; set; }

        [XmlElement("AutoRegisterRecording")]
        public bool AutoRegisterRecording { get; set; }

        [XmlElement("AutoRFCheck")]
        public bool AutoRFCheck { get; set; }

        [XmlElement("BroadcastSet")]
        public string BroadcastSet { get; set; }

        [XmlElement("ColorEntry")]
        public bool ColorEntry { get; set; }

        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }

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

        [XmlElement("EnableFEC")]
        public string EnableFEC { get; set; }

        [XmlElement("ErrorId")]
        public bool ErrorId { get; set; }

        [XmlElement("F12WAYRegister1")]
        public uint F12WAYRegister1 { get; set; }

        [XmlElement("F12WAYRegister10")]
        public uint F12WAYRegister10 { get; set; }

        [XmlElement("F12WAYRegister14")]
        public uint F12WAYRegister14 { get; set; }

        [XmlElement("ForceTimeSync")]
        public bool ForceTimeSync { get; set; }

        [XmlElement("ftpPassword")]
        public string ftpPassword { get; set; }

        [XmlElement("ftpRemoteHost")]
        public string ftpRemoteHost { get; set; }

        [XmlElement("ftpRemotePath")]
        public string ftpRemotePath { get; set; }

        [XmlElement("ftpUserName")]
        public string ftpUserName { get; set; }

        [XmlElement("GoodF1Rely")]
        public decimal GoodF1Rely { get; set; }

        [XmlElement("GoodF2Rely")]
        public decimal GoodF2Rely { get; set; }

        [XmlElement("GoodNumDCU")]
        public int GoodNumDCU { get; set; }

        [XmlElement("IndividualDailyReads")]
        public bool IndividualDailyReads { get; set; }

        [XmlElement("IndividualReadInterval")]
        public bool IndividualReadInterval { get; set; }

        [XmlElement("LatestVersion")]
        public int LatestVersion { get; set; }

        [XmlElement("LiveDigitsOnly")]
        public bool LiveDigitsOnly { get; set; }

        [XmlElement("LogsPublicDir")]
        public bool LogsPublicDir { get; set; }

        [XmlElement("MaxTimeRFCheck")]
        public int MaxTimeRFCheck { get; set; }

        [XmlElement("MeterNumberLength")]
        public int MeterNumberLength { get; set; }

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

        [XmlElement("NewMeterLabel")]
        public string NewMeterLabel { get; set; }

        [XmlElement("NewMeterPort2isTheSame")]
        public bool NewMeterPort2isTheSame { get; set; }

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

        [XmlElement("PasswordMaxLength")]
        public int PasswordMaxLength { get; set; }

        [XmlElement("PasswordMinLength")]
        public int PasswordMinLength { get; set; }

        [XmlElement("Port2DisableNo")]
        public bool Port2DisableNo { get; set; }

        [XmlElement("PublicKey")]
        public string PublicKey { get; set; }

        [XmlElement("RDDFirmwareVersion")]
        public string RDDFirmwareVersion { get; set; }

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

        [XmlElement("ReverseReading")]
        public bool ReverseReading { get; set; }

        [XmlElement("ScriptOnly")]
        public bool ScriptOnly { get; set; }

        [XmlElement("SerialNumLabel")]
        public string SerialNumLabel { get; set; }

        [XmlElement("ShowAddMtu")]
        public bool ShowAddMTU { get; set; }

        [XmlElement("ShowAddMtuMeter")]
        public bool ShowAddMTUMeter { get; set; }

        [XmlElement("ShowAddMtuReplaceMeter")]
        public bool ShowAddMTUReplaceMeter { get; set; }

        [XmlElement("ShowBarCodeButton")]
        public bool ShowBarCodeButton { get; set; }

        [XmlElement("ShowCameraButton")]
        public bool ShowCameraButton { get; set; }

        [XmlElement("ShowDataRead")]
        public bool ShowDataRead { get; set; }

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

        [XmlElement("ShowTurnOff")]
        public bool ShowTurnOff { get; set; }

        [XmlElement("ShowValvePosition")]
        public bool ShowValvePosition { get; set; }

        [XmlElement("SpecialSet")]
        public string SpecialSet { get; set; }

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

        [XmlElement("UseMeterSerialNumber")]
        public bool UseMeterSerialNumber { get; set; }

        [XmlElement("UserIdMaxLength")]
        public int UserIdMaxLength { get; set; }

        [XmlElement("UserIdMinLength")]
        public int UserIdMinLength { get; set; }

        [XmlElement("WorkOrderDualEntry")]
        public bool WorkOrderDualEntry { get; set; }

        [XmlElement("WorkOrderLabel")]
        public string WorkOrderLabel { get; set; }

        [XmlElement("WorkOrderLength")]
        public int WorkOrderLength { get; set; }

        [XmlElement("WorkOrderRecording")]
        public bool WorkOrderRecording { get; set; }

        [XmlIgnore]
        public bool FastMessageConfig { get; set; }

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
        
        #region Collections

        [XmlIgnore]
        private List<Option> Options_Default;

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

        #endregion
        
        #region New Tags ( Own )
        
        [XmlElement("Compression")]
        public string Compression { get; set; }

        [XmlIgnore]
        public int LexiTimeout { get; set; }

        [XmlElement("LexiTimeout")]
        public string Deserialize_LexiTimeout
        {
            set
            {
                int v;
                if ( ! string.IsNullOrEmpty ( value ) &&
                     int.TryParse ( value, out v ) )
                {
                    if      ( v < MIN_LEXI_TIMEOUT ) this.LexiTimeout = MIN_LEXI_TIMEOUT;
                    else if ( v > MAX_LEXI_TIMEOUT ) this.LexiTimeout = MAX_LEXI_TIMEOUT;
                    else                             this.LexiTimeout = v;
                }
                else this.LexiTimeout = DEF_LEXI_TIMEOUT;

                this.LexiTimeout *= 1000; // From sec to msec
            }
            get { return this.LexiTimeout.ToString (); }
        }

        [XmlIgnore]
        public int LexiAttempts;

        [XmlElement("LexiAttempts")]
        public string Deserialize_LexiAttempts
        {
            set
            {
                int v;
                if ( ! string.IsNullOrEmpty ( value ) &&
                     int.TryParse ( value, out v ) )
                {
                    if      ( v < MIN_LEXI_ATTEMPTS ) this.LexiAttempts = MIN_LEXI_ATTEMPTS;
                    else if ( v > MAX_LEXI_ATTEMPTS ) this.LexiAttempts = MAX_LEXI_ATTEMPTS;
                    else                              this.LexiAttempts = v;
                }
                else this.LexiAttempts = DEF_LEXI_ATTEMPTS;
            }
            get { return this.LexiAttempts.ToString (); }
        }

        /*
        private int value_lexiTimeout;
        [XmlElement("LexiTimeout")]
        public dynamic LexiTimeout
        {
            set { this.value_lexiTimeout = XmlAux.Set ( value,
                    DEF_LEXI_TIMEOUT, MIN_LEXI_TIMEOUT, MAX_LEXI_TIMEOUT ); }
            get { return this.value_lexiTimeout; }
        }

        private int value_lexiAttempts;
        [XmlElement("LexiAttempts")]
        public dynamic LexiAttempts
        {
            set { this.value_lexiAttempts = XmlAux.Set ( value,
                    DEF_LEXI_ATTEMPTS, MIN_LEXI_ATTEMPTS, MAX_LEXI_ATTEMPTS ); }
            get { return this.value_lexiAttempts; }
        }
        */

        #endregion

        #region Logic

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

        #endregion
    }
}
