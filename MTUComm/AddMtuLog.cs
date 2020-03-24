using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Library;
using Xml;
using System.Threading.Tasks;

using APP_FIELD  = MTUComm.ScriptAux.APP_FIELD;
using ActionType = MTUComm.Action.ActionType;

namespace MTUComm
{
    public class AddMtuLog
    {
        #region Attributes

        private Configuration config;
        private readonly string user;
        private readonly Mtu mtu;
        private readonly Action action;
        private readonly MTUBasicInfo mtuBasicInfo;
        private readonly string logUri;

        private XDocument doc;
        private readonly XElement addMtuAction;
        private readonly XElement turnOffAction;
        private readonly XElement turnOnAction;
        private readonly XElement  readMtuAction;

        private Dictionary<APP_FIELD,string[]> texts;

        #endregion

        #region Properties

        public Logger Logger { get; set; }

        public XElement XmlAddMtu
        {
            get { return this.addMtuAction; }
        }

        public XElement XmlTurnOff
        {
            get { return this.turnOffAction; }
        }

        public XElement XmlTurnOn
        {
            get { return this.turnOnAction; }
        }

        public XElement XmlReadMtu
        {
            get { return this.readMtuAction; }
        }

        #endregion

        #region Initialization

        public AddMtuLog (
            Logger logger,
            Mtu    mtu,
            string user )
        {
            this.Logger = logger;
            this.mtu    = mtu;
            this.user   = user;
            this.mtuBasicInfo = Data.Get.MtuBasicInfo;

            if ( ! Data.Get.UNIT_TEST )
                this.logUri = this.Logger.CreateFileIfNotExist ();
            
            this.config = Singleton.Get.Configuration;
            this.action = Singleton.Get.Action;

            this.addMtuAction  = new XElement ( "Action" );
            this.turnOffAction = new XElement ( "Action" );
            this.turnOnAction  = new XElement ( "Action" );
            this.readMtuAction = new XElement ( "Action" );

            Global global = Singleton.Get.Configuration.Global;

            // APP_FIELD = { Tag XML, Display attribute }
            this.texts = new Dictionary<APP_FIELD,string[]> ()
            {
                #region Service Port ID = Account Number = Functl Loctn
                {
                    APP_FIELD.AccountNumber,
                    new string[]
                    {
                        "AccountNumber",
                        global.AccountLabel
                    }
                },
                {
                    APP_FIELD.AccountNumber_2,
                    new string[]
                    {
                        "AccountNumber",
                        global.AccountLabel
                    }
                },
                #endregion
                #region Field Order = Work Order
                {
                    APP_FIELD.WorkOrder,
                    new string[]
                    {
                        "WorkOrder",
                        "Field Order"
                    }
                },
                {
                    APP_FIELD.WorkOrder_2,
                    new string[]
                    {
                        "WorkOrder",
                        "Field Order"
                    }
                },
                #endregion
                #region Old MTU ID
                {
                    APP_FIELD.OldMtuId,
                    new string[]
                    {
                        "OldMtuId",
                        "Old MTU ID"
                    }
                },
                #endregion
                #region Activity Log ID
                {
                    APP_FIELD.ActivityLogId,
                    new string[]
                    {
                        "ActivityLogID",
                        "Activity Log ID"
                    }
                },
                #endregion
                #region Meter Serial Number
                {
                    APP_FIELD.MeterNumber,
                    new string[]
                    {
                        "NewMeterSerialNumber",
                        "New Meter Serial Number"
                    }
                },
                {
                    APP_FIELD.MeterNumber_2,
                    new string[]
                    {
                        "NewMeterSerialNumber",
                        "New Meter Serial Number"
                    }
                },
                {
                    APP_FIELD.MeterNumberOld,
                    new string[]
                    {
                        "OldMeterSerialNumber",
                        "Old Meter Serial Number"
                    }
                },
                {
                    APP_FIELD.MeterNumberOld_2,
                    new string[]
                    {
                        "OldMeterSerialNumber",
                        "Old Meter Serial Number"
                    }
                },
                #endregion
                #region Initial Reading = Meter Reading
                {
                    APP_FIELD.MeterReading,
                    new string[]
                    {
                        "NewMeterReading",
                        "New Meter Reading"
                    }
                },
                {
                    APP_FIELD.MeterReading_2,
                    new string[]
                    {
                        "NewMeterReading",
                        "New Meter Reading"
                    }
                },
                {
                    APP_FIELD.MeterReadingOld,
                    new string[]
                    {
                        "OldMeterReading",
                        "Old Meter Reading"
                    }
                },
                {
                    APP_FIELD.MeterReadingOld_2,
                    new string[]
                    {
                        "OldMeterReading",
                        "Old Meter Reading"
                    }
                },
                #endregion
                #region Meter Type ( Meter ID )
                {
                    APP_FIELD.Meter,
                    new string[]
                    {
                        "SelectedMeterId",
                        "Selected Meter ID"
                    }
                },
                {
                    APP_FIELD.Meter_2,
                    new string[]
                    {
                        "SelectedMeterId",
                        "Selected Meter ID"
                    }
                },
                #endregion

                #region Old Meter Working
                {
                    APP_FIELD.OldMeterWorking,
                    new string[]
                    {
                        "OldMeterWorking",
                        "Old Meter Working"
                    }
                },
                {
                    APP_FIELD.OldMeterWorking_2,
                    new string[]
                    {
                        "OldMeterWorking",
                        "Old Meter Working"
                    }
                },
                #endregion
                #region Replace Meter|Register
                {
                    APP_FIELD.ReplaceMeterRegister,
                    new string[]
                    {
                        "MeterRegisterStatus",
                        "Meter Register Status"
                    }
                },
                {
                    APP_FIELD.ReplaceMeterRegister_2,
                    new string[]
                    {
                        "MeterRegisterStatus",
                        "Meter Register Status"
                    }
                },
                #endregion

                #region Number of Dials
                {
                    APP_FIELD.NumberOfDials,
                    new string[]
                    {
                        "NumberOfDials",
                        "Number of Dials"
                    }
                },
                {
                    APP_FIELD.NumberOfDials_2,
                    new string[]
                    {
                        "NumberOfDials",
                        "Number of Dials"
                    }
                },
                #endregion
                #region Drive Dial Size
                {
                    APP_FIELD.DriveDialSize,
                    new string[]
                    {
                        "DriveDialSize",
                        "Drive Dial Size"
                    }
                },
                {
                    APP_FIELD.DriveDialSize_2,
                    new string[]
                    {
                        "DriveDialSize",
                        "Drive Dial Size"
                    }
                },
                #endregion
                #region Unit of Measure
                {
                    APP_FIELD.UnitOfMeasure,
                    new string[]
                    {
                        "UnitOfMeasure",
                        "Unit of Measure"
                    }
                },
                {
                    APP_FIELD.UnitOfMeasure_2,
                    new string[]
                    {
                        "UnitOfMeasure",
                        "Unit of Measure"
                    }
                },
                #endregion

                #region Read Interval
                {
                    APP_FIELD.ReadInterval,
                    new string[]
                    {
                        "ReadInterval",
                        "Read Interval"
                    }
                },
                #endregion
                #region Snap Reads
                {
                    APP_FIELD.SnapRead,
                    new string[]
                    {
                        "SnapReads",
                        "Snap Reads"
                    }
                },
                #endregion
                #region 2Way
                {
                    APP_FIELD.TwoWay,
                    new string[]
                    {
                        "Fast-2-Way",
                        "Fast Message Config"
                    }
                },
                #endregion
                #region RDDPosition
                {
                    APP_FIELD.RDDPosition,
                    new string[]
                    {
                        "RDDPostion",
                        "RDD Position"
                    }
                },
                #endregion
                #region RDDFirmware
                {
                    APP_FIELD.RDDFirmware,
                    new string[]
                    {
                        "RDDFirmware",
                        "RDD Firmware Version"
                    }
                },
                #endregion
                #region Alarm
                {
                    APP_FIELD.Alarm,
                    new string[]
                    {
                        "Alarms",
                        "Alarms"
                    }
                },
                #endregion
                #region Demand
                {
                    APP_FIELD.Demand,
                    new string[]
                    {
                        "Demands",
                        "Demands"
                    }
                },
                #endregion
                #region GPS
                {
                    APP_FIELD.GpsLatitude,
                    new string[]
                    {
                        "GPS_Y",
                        "Lat"
                    }
                },
                {
                    APP_FIELD.GpsLongitude,
                    new string[]
                    {
                        "GPS_X",
                        "Long"
                    }
                },
                {
                    APP_FIELD.GpsAltitude,
                    new string[]
                    {
                        "Altitude",
                        "Elevation"
                    }
                },
                #endregion
                #region Optional Parameters
                {
                    APP_FIELD.Optional,
                    new string[]
                    {
                        "OptionalParams",
                        "OptionalParams"
                    }
                },
                #endregion
            };
        }

        #endregion

        #region Logic

        #region Parts of the log

        public void LogTurnOff ()
        {
            Logger.AddAtrribute(this.turnOffAction, "display", Action.LogDisplays[ActionType.TurnOffMtu]);
            Logger.AddAtrribute(this.turnOffAction, "type", Action.LogTypes[ActionType.TurnOffMtu]);
            Logger.AddParameter(this.turnOffAction, new Parameter("Date", "Date/Time", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") ));
            if (!string.IsNullOrEmpty(this.user))
            Logger.AddParameter(this.turnOffAction, new Parameter("User", "User", this.user));
            Logger.AddParameter(this.turnOffAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogTurnOn ()
        {
            Logger.AddAtrribute(this.turnOnAction, "display", Action.LogDisplays[ActionType.TurnOnMtu]);
            Logger.AddAtrribute(this.turnOnAction, "type", Action.LogTypes[ActionType.TurnOnMtu]);
            Logger.AddParameter(this.turnOnAction, new Parameter("Date", "Date/Time", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));
            if (!string.IsNullOrEmpty(this.user))
            Logger.AddParameter(this.turnOnAction, new Parameter("User", "User", this.user));
            Logger.AddParameter(this.turnOnAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public async Task LogAddMtu (
            dynamic map )
        {
            Global  global = this.config.Global;
            string  temp   = string.Empty;
            string  DISABLED = MemoryMap.MemoryMap.DISABLED;

            ActionType actionType = this.action.Type;

            bool isReplaceMeter = this.action.IsReplaceMeter;
            bool isReplaceMtu   = this.action.IsReplaceMtu;
            
            bool rddIn1;
            bool usePort2 = Data.Get.UsePort2;
            bool hasRDD   = ( ( rddIn1 = mtu.Port1.IsSetFlow ) ||
                              mtu.TwoPorts && mtu.Port2.IsSetFlow );
            bool noRddOrNotIn1 = ! hasRDD || ! rddIn1;
            bool noRddOrNotIn2 = ! hasRDD ||   rddIn1;

            #region General

            Logger.AddAtrribute(this.addMtuAction, "display", Action.LogDisplays[this.action.Type]);
            Logger.AddAtrribute(this.addMtuAction, "type", Action.LogTypes[this.action.Type]);
            Logger.AddAtrribute(this.addMtuAction, "reason", Action.LogReasons[this.action.Type]);

            Logger.AddParameter ( this.addMtuAction, new Parameter("Date", "Date/Time", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")));

            if ( ! string.IsNullOrEmpty ( this.user ) )
                Logger.AddParameter(this.addMtuAction, new Parameter("User", "User", this.user ) );

            // Not used with single port MTUs with RDD
            if ( noRddOrNotIn1 && 
                 isReplaceMtu )
                Logger.AddParameter ( this.addMtuAction, this.PrepareParameter ( APP_FIELD.OldMtuId ) );

            Logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuId",   "MTU ID",   this.mtuBasicInfo.Id   ) );
            Logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuType", "MTU Type", this.mtuBasicInfo.Type ) );

            // Not used with single port MTUs with RDD
            if ( noRddOrNotIn1 )
                Logger.AddParameter ( this.addMtuAction, this.PrepareParameter ( APP_FIELD.ReadInterval ) );

            // Not used with single port MTUs with RDD or family 33xx
            if ( noRddOrNotIn1 &&
                 ! this.mtu.IsFamily33xx &&
                 DataContains ( APP_FIELD.SnapRead ) )
            {
                string dailyReadsLocal = this.GetDataValue ( APP_FIELD.SnapRead ).ToString (); // Local
                int    dailyReadsUTC   = await map.DailyGMTHourRead.GetValue ();    // UTC

                // Value written in the MTU, in UTC time
                Logger.AddParameter ( this.addMtuAction,
                    new Parameter ( "DailyGMTHourRead", "GMT Daily Reads", dailyReadsUTC ) );
                
                // Value selected by the user, in local time
                if ( dailyReadsUTC != Global.MAX_DAILY_OFF )
                    Logger.AddParameter ( this.addMtuAction,
                        new Parameter ( "DailyReads", "Daily Reads", dailyReadsLocal ) );
            }

            if ( global.TimeToSync &&
                 mtu.TimeToSync    &&
                 mtu.FastMessageConfig )
                Logger.AddParameter ( this.addMtuAction, this.PrepareParameter ( APP_FIELD.TwoWay ) );

            // Related to F12WAYRegister1XX registers
            string afc = ( mtu.TimeToSync &&
                           global.AFC &&
                           await map.MtuSoftVersion.GetValue () >= 19 ) ? "Set" : "OFF";
            Logger.AddParameter ( this.addMtuAction, new Parameter ( "AFC", "AFC", afc ) );

            #endregion

            #region Additional tags

            // Add additional parameters for all actions except for the Add
            foreach ( Parameter param in this.action.AdditionalScriptParameters )
                Logger.AddParameter ( this.addMtuAction, param );

            #endregion

            #region Encryption

            // Avoid try to log encryption info when not it has not been performed
            if ( map.ContainsMember( "Encrypted" ) &&
                 await map.Encrypted.GetValueFromMtu () &&
                 this.mtu.SpecialSet )
            {
                //logger.Parameter ( this.addMtuAction, new Parameter ( "Encryption", "Encrypted", map.Encryption.GetValue () ) );
                Logger.AddParameter ( this.addMtuAction,
                    new Parameter ( "EncryptionIndex", "Encryption Index",
                        await map.EncryptionIndex.GetValueFromMtu () ) );
            
                if ( ! mtu.IsFamily35xx36xx )
                {
                    Mobile.ConfigData data = Mobile.ConfData;

                    // Using certificate with public key
                    if ( data.IsCertLoaded )
                    {
                        Utils.PrintDeep ( "Using certificate creating activity log" );
                        
                        Logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuSymKey",            "MtuSymKey",             data.RandomKeyAndShaEncryptedInBase64 ) );
                        Logger.AddParameter ( this.addMtuAction, new Parameter ( "HeadendCertThumb",     "HeadendCertThumb",      data.certificate.Thumbprint ) );
                        Logger.AddParameter ( this.addMtuAction, new Parameter ( "HeadendCertValidTill", "HeadendCertExpiration", data.certificate.NotAfter.ToString ( "MM/dd/yy hh:mm:ss tt" ) ) );
                        Logger.AddParameter ( this.addMtuAction, new Parameter ( "DeviceCertSubject",    "DeviceCertSubject",     data.certificate.Subject    ) );
                    }
                    // No certificate present
                    else
                    {
                        Utils.PrintDeep ( "Not using certificate creating activity log" );
                    
                        Logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuSymKey", "MtuSymKey", data.RandomKeyAndShaInBase64 ) );
                    }
                }
                // OnDemand 1.2 MTUs
                else
                {
                    Logger.AddParameter ( this.addMtuAction, new Parameter ( "serverRND",          "serverRND",          Data.Get.ServerRND     ) );
                    Logger.AddParameter ( this.addMtuAction, new Parameter ( "clientRND",          "clientRND",          Data.Get.ClientRND     ) );
                    Logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuPublicKey",       "MtuPublicKey",       Data.Get.MtuPublicKey  ) );
                    Logger.AddParameter ( this.addMtuAction, new Parameter ( "STAREncryptionType", "STAREncryptionType", mtu.STAREncryptionType ) );
                }
            }

            #endregion

            #region Port 1

            Meter meter = ( Meter )this.GetDataValue ( APP_FIELD.Meter );

            XElement port = new XElement("Port");
            Logger.AddAtrribute(port, "display", "Port 1");
            Logger.AddAtrribute(port, "number", "1");

            Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.AccountNumber ) );

            if ( global.WorkOrderRecording )
                Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.WorkOrder ) );

            // Not used with single port MTUs with RDD
            if ( isReplaceMeter &&
                 noRddOrNotIn1 )
            {
                if ( global.UseMeterSerialNumber )
                    Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.MeterNumberOld ) );
                
                if ( global.MeterWorkRecording )
                    Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.OldMeterWorking ) );
                
                if ( global.OldReadingRecording )
                    Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.MeterReadingOld ) );
                
                if ( global.RegisterRecording )
                    Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.ReplaceMeterRegister ) );
                
                if ( global.AutoRegisterRecording )
                {
                    temp = ( string.Equals (
                                this.GetDataValue ( APP_FIELD.MeterNumber    ),
                                this.GetDataValue ( APP_FIELD.MeterNumberOld ) ) ) ?
                             "Register head change" : "Meter change";
                    Logger.AddParameter ( port, new Parameter ( "MeterRegisterAutoStatus", temp, "Meter Register Auto Status" ) );
                }
            }

            if ( hasRDD &&
                 rddIn1 )
            {
                Logger.AddParameter ( port, new Parameter ( "RDDSerialNumber", "RDD SerialNumber", Data.Get.RDDSerialNumber ) );
                Logger.AddParameter ( port, new Parameter ( "RDDBatteryStatus", "RDD Battery Status", Data.Get.RDDBattery ) );
                Logger.AddParameter ( port, new Parameter ( "RDDCurrentValvePosition", "RDD Current Valve Position", Data.Get.PrevRDDValvePosition ) );
                Logger.AddParameter ( port, new Parameter ( "RDDDesiredValvePosition", "RDD Desired Valve Position", Data.Get.RDDPosition ) );
            }

            string meterType = string.Format("({0}) {1}", meter.Id, meter.Display);
            Logger.AddParameter ( port, new Parameter("MeterType", "Meter Type", meterType));
            Logger.AddParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter.Id.ToString()));
            Logger.AddParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter.Vendor));
            Logger.AddParameter ( port, new Parameter("MeterModel", "Meter Model", meter.Model));
            
            // Not used with single port MTUs with RDD
            if ( global.UseMeterSerialNumber &&
                 noRddOrNotIn1 )
                Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.MeterNumber ) );
            
            // Not used with single port MTUs with RDD
            if ( ! mtu.Port1.IsForEncoderOrEcoder &&
                 noRddOrNotIn1 )
                Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.MeterReading ) );
            
            Logger.AddParameter ( port, new Parameter ( "PulseHi","Pulse Hi Time",  meter.PulseHiTime .ToString ().PadLeft ( 2, '0' ) ) );
            Logger.AddParameter ( port, new Parameter ( "PulseLo","Pulse Low Time", meter.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

            this.addMtuAction.Add ( port );

            #endregion

            #region Port 2

            if ( usePort2 )
            {
                Meter meter2 = (Meter)this.GetDataValue ( APP_FIELD.Meter_2 );

                port = new XElement ( "Port");
                Logger.AddAtrribute ( port, "display", "Port 2" );
                Logger.AddAtrribute ( port, "number", "2" );

                Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.AccountNumber_2, 1 ) );

                if ( global.WorkOrderRecording )
                    Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.WorkOrder_2, 1 ) );

                // Not used with RDD
                if ( isReplaceMeter &&
                     noRddOrNotIn2 )
                {
                    if ( global.UseMeterSerialNumber )
                        Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.MeterNumberOld_2, 1 ) );

                    if ( global.MeterWorkRecording )
                        Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.OldMeterWorking_2, 1 ) );
                    
                    if ( global.OldReadingRecording )
                        Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.MeterReadingOld_2, 1 ) );
                    
                    if ( global.RegisterRecording )
                        Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.ReplaceMeterRegister_2, 1 ) );
                    
                    if ( global.AutoRegisterRecording )
                    {
                        temp = string.Equals (
                                    this.GetDataValue ( APP_FIELD.MeterNumber_2 ),
                                    this.GetDataValue ( APP_FIELD.MeterNumberOld_2 ) ) ?
                                 "Register head change" : "Meter change";
                        Logger.AddParameter ( port, new Parameter ( "MeterRegisterAutoStatus", temp, "Meter Register Auto Status" ) );
                    }
                }
                
                if ( hasRDD &&
                     ! rddIn1 )
                {
                    Logger.AddParameter ( port, new Parameter ( "RDDSerialNumber", "RDD SerialNumber", Data.Get.RDDSerialNumber ) );
                    Logger.AddParameter ( port, new Parameter ( "RDDBatteryStatus", "RDD Battery Status", Data.Get.RDDBattery ) );
                    Logger.AddParameter ( port, new Parameter ( "RDDCurrentValvePosition", "RDD Current Valve Position", Data.Get.PrevRDDValvePosition ) );
                    Logger.AddParameter ( port, new Parameter ( "RDDDesiredValvePosition", "RDD Desired Valve Position", Data.Get.RDDPosition ) );
                }

                string meterType2 = string.Format("({0}) {1}", meter2.Id, meter2.Display);
                Logger.AddParameter ( port, new Parameter("MeterType", "Meter Type", meterType2));
                Logger.AddParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter2.Id.ToString()));
                Logger.AddParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter2.Vendor));
                Logger.AddParameter ( port, new Parameter("MeterModel", "Meter Model", meter2.Model));
                
                // Not used with RDD
                if ( global.UseMeterSerialNumber &&
                     noRddOrNotIn2 )
                    Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.MeterNumber_2, 1 ) );
                
                // Not used with RDD
                if ( ! mtu.Port2.IsForEncoderOrEcoder &&
                     noRddOrNotIn2 )
                    Logger.AddParameter ( port, this.PrepareParameter ( APP_FIELD.MeterReading_2, 1 ) );

                Logger.AddParameter ( port, new Parameter ( "PulseHi","Pulse Hi Time",  meter2.PulseHiTime .ToString ().PadLeft ( 2, '0' ) ) );
                Logger.AddParameter ( port, new Parameter ( "PulseLo","Pulse Low Time", meter2.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

                this.addMtuAction.Add ( port );
            }

            #endregion

            #region Alarms

            if ( mtu.RequiresAlarmProfile )
            {
                if ( this.DataContains ( APP_FIELD.Alarm ) )
                {
                    Alarm alarms = ( Alarm )this.GetDataValue ( APP_FIELD.Alarm );
                    if ( alarms != null )
                    {
                        XElement alarmSelection = new XElement ( "AlarmSelection" );
                        Logger.AddAtrribute ( alarmSelection, "display", "Alarm Selection" );


                        // Log the value if the condition is validated
                        dynamic AddParamCond = new Action<string,string,dynamic,bool> (
                            ( tag, display, value, condition ) => {

                                if ( condition )
                                    Logger.AddParameter ( alarmSelection,
                                        new Parameter ( tag, display, value.ToString () ) );
                            });
                        
                        // Log the memory map register value if the condition is validated
                        dynamic AddParamCondMap = new Func<string,string,string,bool,Task> (
                            async ( tag, display, id, condition ) => {
                                if ( map.ContainsMember ( id ) )
                                    AddParamCond ( tag, display, await map[ id ].GetValue (), condition );
                            });

                        // Log always because these entries have no condition
                        dynamic AddParameter = new Action<string,string,string> (
                            ( tag, display, value ) =>
                                AddParamCond ( tag, display, value, true ) );

                        string alarmConfiguration = alarms.Name;

                        AddParameter          ( "AlarmConfiguration",        "Alarm Configuration Name",     alarmConfiguration );
                        AddParameter          ( "Overlap",                   "Message Overlap",              alarms.Overlap.ToString () );
                        AddParameter          ( "ImmediateAlarm",            "Immediate Alarm Transmit",     alarms.ImmediateAlarmTransmit.ToString () );
                        AddParamCond          ( "UrgentAlarm",               "DCU Urgent Alarm Transmit",    alarms.DcuUrgentAlarm.ToString (),      map.ContainsMember ( "UrgentAlarm" ) );

                        // NOTE: In the ADD block in the log, alarms must be written without checking the status of the tampers
                        // NOTE: Immediate alarms use a custom method that only takes into account the alarm status,
                        // NOTE: but the custom method for normal/non-immediate alarms works with alarm and tamper status -> Read alarm, no the overload

                        await AddParamCondMap ( "MemoryMapError",            "Memory Map Error",             "MemoryMapAlarm",                       mtu.MemoryMapError );
                        await AddParamCondMap ( "MemoryMapErrorImm",         "Memory Map Error Imm",         "MemoryMapImmTamperStatus",             mtu.MemoryMapErrorImm );
                        await AddParamCondMap ( "ProgramMemoryError",        "Program Memory Error",         "ProgramMemoryAlarm",                   mtu.ProgramMemoryError );
                        await AddParamCondMap ( "ProgramMemoryErrorImm",     "Program Memory Error Imm",     "ProgramMemoryImmTamperStatus",         mtu.ProgramMemoryErrorImm );
                        await AddParamCondMap ( "MoistureDetect",            "Moisture Detect",              "MoistureAlarm",                        mtu.MoistureDetect );
                        await AddParamCondMap ( "MoistureDetectImm",         "Moisture Detect Imm",          "MoistureImmTamperStatus",              mtu.MoistureDetectImm );
                        await AddParamCondMap ( "EnergizerLastGasp",         "Energizer Last Gasp",          "EnergizerLastGaspAlarm",               mtu.EnergizerLastGasp );
                        await AddParamCondMap ( "EnergizerLastGaspImm",      "Energizer Last Gasp Imm",      "EnergizerLastGaspImmTamperStatus",     mtu.EnergizerLastGaspImm );
                        await AddParamCondMap ( "InsufficentMemory",         "Insufficent Memory",           "InsufficientMemoryAlarm",              mtu.InsufficientMemory );
                        await AddParamCondMap ( "InsufficentMemoryImm",      "Insufficent Memory Imm",       "InsufficientMemoryImmTamperStatus",    mtu.InsufficientMemoryImm );
                        await AddParamCondMap ( "CutAlarmCable",             "Cut Alarm Cable",              "GasCutWireAlarm",                      mtu.GasCutWireAlarm );
                        await AddParamCondMap ( "Cut2AlarmCable",            "Cut Port2 Alarm Cable",        "P2GasCutWireAlarm",                    usePort2 && mtu.GasCutWireAlarm );
                        await AddParamCondMap ( "SerialComProblem",          "Serial Com Problem",           "SerialComProblemAlarm",                mtu.SerialComProblem );
                        await AddParamCondMap ( "SerialComProblemImm",       "Serial Com Problem Imm",       "SerialComProblemImmTamperStatus",      mtu.SerialComProblemImm );
                        await AddParamCondMap ( "LastGasp",                  "Last Gasp",                    "LastGaspAlarm",                        mtu.LastGasp );
                        await AddParamCondMap ( "LastGaspImm",               "Last Gasp Imm",                "LastGaspImmTamperStatus",              mtu.LastGaspImm );
                        await AddParamCondMap ( "TiltTamper",                "Tilt Tamper",                  "TiltAlarm",                            mtu.TiltTamper );
                        await AddParamCondMap ( "TiltTamperImm",             "Tilt Tamper Imm",              "TiltImmTamperStatus",                  mtu.TiltTamperImm );
                        await AddParamCondMap ( "MagneticTamper",            "Magnetic Tamper",              "MagneticAlarm",                        mtu.MagneticTamper );
                        await AddParamCondMap ( "MagneticTamperImm",         "Magnetic Tamper Imm",          "MagneticImmTamperStatus",              mtu.MagneticTamperImm );
                        await AddParamCondMap ( "InterfaceTamper",           "Interface Tamper",             "InterfaceAlarm",                       mtu.InterfaceTamper );
                        await AddParamCondMap ( "InterfaceTamperImm",        "Interface Tamper Imm",         "InterfaceImmTamperStatus",             mtu.InterfaceTamperImm );
                        await AddParamCondMap ( "RegisterCoverTamper",       "Register Cover Tamper",        "RegisterCoverAlarm",                   mtu.RegisterCoverTamper );
                        await AddParamCondMap ( "RegisterCoverTamperImm",    "Register Cover Tamper Imm",    "RegisterCoverImmTamperStatus",         mtu.RegisterCoverTamperImm );
                        await AddParamCondMap ( "ReverseFlow",               "Reverse Flow Tamper",          "ReverseFlowAlarm",                     mtu.ReverseFlowTamper );
                        AddParamCond          ( "FlowDirection",             "Flow Direction",               meter.Flow.ToString (),                 mtu.ReverseFlowTamper );
                        await AddParamCondMap ( "ReverseFlowTamperImm",      "Reverse Flow Tamper Imm",      "ReverseFlowImmTamperStatus",           mtu.ReverseFlowTamperImm );
                        await AddParamCondMap ( "SerialCutWire",             "Serial Cut Wire",              "SerialCutWireAlarm",                   mtu.SerialCutWire );
                        await AddParamCondMap ( "SerialCutWire",             "Serial Cut Wire",              "SerialCutWireImmTamperStatus",         mtu.SerialCutWireImm );
                        await AddParamCondMap ( "Cut1WireTamper",            "Cut Port1 Wire Tamper",        "P1CutWireAlarm",                       mtu.TamperPort1 );
                        await AddParamCondMap ( "Cut1WireTamperImm",         "Cut Port1 Wire Tamper Imm",    "P1CutWireImmTamperStatus",             mtu.TamperPort1Imm );
                        await AddParamCondMap ( "Cut2WireTamper",            "Cut Port2 Wire Tamper",        "P2CutWireAlarm",                       usePort2 && mtu.TamperPort2 );
                        await AddParamCondMap ( "Cut2WireTamperImm",         "Cut Port2 Wire Tamper Imm",    "P2CutWireImmTamperStatus",             usePort2 && mtu.TamperPort2Imm );
                        await AddParamCondMap ( "CutWireDelaySetting",       "Cut Wire Delay Setting",       "CutWireDelaySetting",                  mtu.CutWireDelaySetting );

                        this.addMtuAction.Add ( alarmSelection );
                    }
                }
            }

            #endregion

            #region Demands

            if ( this.DataContains ( APP_FIELD.Demand ) )
            {
                Demand demand = ( Demand )this.GetDataValue ( APP_FIELD.Demand );
                if ( demand != null &&
                     mtu.MtuDemand  &&
                     mtu.FastMessageConfig )
                {
                    XElement demandConf = new XElement ( "DemandConfiguration" );
                    Logger.AddAtrribute ( demandConf, "display", "Demand Configuration" );
                    Logger.AddParameter ( demandConf, new Parameter ( "ConfigurationName",         "Configuration Name",           demand.Name ) );
                    Logger.AddParameter ( demandConf, new Parameter ( "MtuNumLowPriorityMsg",      "Mtu Num Low Priority Msg",     await map.MtuNumLowPriorityMsg     .GetValue () ) );
                    Logger.AddParameter ( demandConf, new Parameter ( "MtuPrimaryWindowInterval",  "Mtu Primary WindowInterval",   await map.MtuPrimaryWindowInterval .GetValue () ) );
                    Logger.AddParameter ( demandConf, new Parameter ( "MtuWindowAStart",           "Mtu Window A Start",           await map.MtuWindowAStart          .GetValue () ) );
                    Logger.AddParameter ( demandConf, new Parameter ( "MtuWindowBStart",           "Mtu Window B Start",           await map.MtuWindowBStart          .GetValue () ) );
                    Logger.AddParameter ( demandConf, new Parameter ( "MtuPrimaryWindowIntervalB", "Mtu Primary WindowInterval B", await map.MtuPrimaryWindowIntervalB.GetValue () ) );
                    Logger.AddParameter ( demandConf, new Parameter ( "MtuPrimaryWindowOffset",    "Mtu Primary Window Offset",    await map.MtuPrimaryWindowOffset   .GetValue () ) );

                    this.addMtuAction.Add ( demandConf );
                }
            }

            #endregion

            #region Misc/Optional

            if ( this.DataContains ( APP_FIELD.GpsLatitude  ) &&
                 this.DataContains ( APP_FIELD.GpsLongitude ) &&
                 this.DataContains ( APP_FIELD.GpsAltitude  ) )
            {
                string lat = Utils.FormatNumber ( this.GetDataValue ( APP_FIELD.GpsLatitude  ), "F6" );
                string lon = Utils.FormatNumber ( this.GetDataValue ( APP_FIELD.GpsLongitude ), "F6" );
                string alt = Utils.FormatNumber ( this.GetDataValue ( APP_FIELD.GpsAltitude  ), "F2" );

                Logger.AddParameter ( this.addMtuAction, new Parameter ( "GPS_Y",    "Lat",       lat ) );
                Logger.AddParameter ( this.addMtuAction, new Parameter ( "GPS_X",    "Long",      lon ) );
                Logger.AddParameter ( this.addMtuAction, new Parameter ( "Altitude", "Elevation", alt ) );
            }

            if ( this.DataContains ( APP_FIELD.Optional ) &&
                 ! ( this.GetDataValue ( APP_FIELD.Optional ) is string ) )
            {
                List<Parameter> optionalParams = (List<Parameter>)this.GetDataValue ( APP_FIELD.Optional );

                if ( optionalParams != null )
                    foreach ( Parameter p in optionalParams )
                        Logger.AddParameter( this.addMtuAction, p );
            }

            #endregion
        }

        public void LogReadMtu (
            ActionResult result )
        {
            Logger.AddAtrribute(this.readMtuAction, "display", Action.LogDisplays[ActionType.ReadMtu]);
            Logger.AddAtrribute(this.readMtuAction, "type", Action.LogTypes[ActionType.ReadMtu]);

            InterfaceParameters[] parameters = this.config.getLogParamsFromInterface ( mtu, ActionType.ReadMtu );
            foreach (InterfaceParameters parameter in parameters)
            {
                try
                {
                    if (parameter.Name == "Port")
                    {
                        ActionResult[] ports = result.getPorts();
                        for (int i = 0; i < ports.Length; i++)
                            Logger.Port(i, this.readMtuAction, ports[i], parameter.Parameters.ToArray());
                    }
                    else
                        Logger.ComplexParameter(this.readMtuAction, result, parameter);
                }
                catch ( Exception )
                {
                    // catch outside
                }
            }
        }

        public string Save ()
        {
            this.addMtuAction.Add ( this.turnOffAction );
            this.addMtuAction.Add ( this.turnOnAction  );
            this.addMtuAction.Add ( this.readMtuAction );

            this.doc = XDocument.Load ( logUri );
            XElement mtus = doc.Root.Element ( "Mtus" );
            mtus.Add ( this.addMtuAction );
            doc.Save ( logUri );

            // Launching multiple times scripts with the same output path, concatenates the actions logs,
            // but the log send to the explorer should be only the last action performed
            byte[] byteArray = Encoding.UTF8.GetBytes(Logger.CreateBasicStructure());
            Stream BasicStruct = new MemoryStream(byteArray);
            XDocument uniDoc = XDocument.Load(BasicStruct);
            XElement uniMtus = uniDoc.Root.Element("Mtus");
            uniMtus.Add(this.addMtuAction);

            #if DEBUG
            string uniUri = Path.Combine ( Mobile.LogUniPath,
                this.mtuBasicInfo.Type + "-" + this.action.Type + ( ( mtu.SpecialSet ) ? "-Encrypted" : "" ) + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.Logger.CreateFileIfNotExist ( Logger.BasicFileType.READ, false, uniUri );

            uniDoc.Save ( uniUri );
            #endif
            
            // Write in ActivityLog
            if ( Data.Get.IsFromScripting &&
                 ! this.config.Global.ScriptOnly )
            {
                // Reset fixed_name to add to the ActivityLog in CreateFileIfNotExist
                this.Logger.ResetFixedName ();
                
                String uri = this.Logger.CreateFileIfNotExist ();
                doc  = XDocument.Load ( uri );
                mtus = doc.Root.Element ( "Mtus" );
                mtus.Add ( this.addMtuAction );
                doc.Save(uri);
            }
            
            return uniDoc.ToString ();
        }

        #endregion

        #region Auxiliary

        private Parameter PrepareParameter (
            APP_FIELD field,
            int port = 0 )
        {
            string[] texts = this.texts[ field ];
            dynamic  value = Data.Get[ field.ToString () ];

            return new Parameter ( texts[ 0 ], texts[ 1 ], value, null, port );
        }

        private bool DataContains (
            APP_FIELD field )
        {
            return Data.Contains ( field.ToString () );
        }

        private dynamic GetDataValue (
            APP_FIELD field )
        {
            return Data.Get[ field.ToString () ];
        }

        #endregion

        #endregion
    }
}
