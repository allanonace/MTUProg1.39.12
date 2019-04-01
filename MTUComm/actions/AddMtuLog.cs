using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MTUComm.actions;
using Xml;

using System.IO;

using FIELD = MTUComm.actions.AddMtuForm.FIELD;
using ActionType = MTUComm.Action.ActionType;

namespace MTUComm
{
    public class AddMtuLog
    {
        public  Logger logger;
        private string user;
        private dynamic form;
        private Action action;
        private MTUBasicInfo mtuBasicInfo;
        private string logUri;

        private XDocument doc;
        private XElement  addMtuAction;
        private XElement  turnOffAction;
        private XElement  turnOnAction;
        private XElement  readMtuAction;
        
        private string uniLog;

        public AddMtuLog(Logger logger, dynamic form, string user, bool isFromScripting )
        {
            this.logger = logger;
            this.form = form;
            this.action = (Action)form.action;
            this.user = user;
            this.mtuBasicInfo = MtuForm.mtuBasicInfo;
            this.logUri = this.logger.CreateFileIfNotExist ();

            this.addMtuAction  = new XElement("Action");
            this.turnOffAction = new XElement("Action");
            this.turnOnAction  = new XElement("Action");
            this.readMtuAction = new XElement("Action");
        }

        public void LogTurnOff ()
        {
            logger.AddAtrribute(this.turnOffAction, "display", Action.displays[ActionType.TurnOffMtu]);
            logger.AddAtrribute(this.turnOffAction, "type", Action.tag_types[ActionType.TurnOffMtu]);

            logger.Parameter(this.turnOffAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") ));

            if (!string.IsNullOrEmpty(this.user))
                logger.Parameter(this.turnOffAction, new Parameter("User", "User", this.user));

            logger.Parameter(this.turnOffAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogTurnOn ()
        {
            logger.AddAtrribute(this.turnOnAction, "display", Action.displays[ActionType.TurnOnMtu]);
            logger.AddAtrribute(this.turnOnAction, "type", Action.tag_types[ActionType.TurnOnMtu]);

            logger.Parameter(this.turnOnAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
                logger.Parameter(this.turnOnAction, new Parameter("User", "User", this.user));

            logger.Parameter(this.turnOnAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogAddMtu ( bool isFromScripting = false )
        {
            Mtu     mtu    = form.mtu;
            Global  global = form.global;
            dynamic map    = form.map;
            string  temp   = string.Empty;
            string  DISABLED = MemoryMap.MemoryMap.DISABLED;
            string  ENABLED  = MemoryMap.MemoryMap.ENABLED;

            ActionType actionType = form.action.type;

            bool isReplaceMeter = actionType == ActionType.ReplaceMeter           ||
                                  actionType == ActionType.ReplaceMtuReplaceMeter ||
                                  actionType == ActionType.AddMtuReplaceMeter;
            bool isReplaceMtu   = actionType == ActionType.ReplaceMTU ||
                                  actionType == ActionType.ReplaceMtuReplaceMeter;

            #region General

            //logger.addAtrribute ( this.addMtuAction, "display", addMtuDisplay );
            // logger.addAtrribute ( this.addMtuAction, "type",    addMtuType    );
            // logger.addAtrribute ( this.addMtuAction, "reason",  addMtuReason  );
            logger.AddAtrribute(this.addMtuAction, "display", Action.displays[this.action.type]);
            logger.AddAtrribute(this.addMtuAction, "type", Action.tag_types[this.action.type]);
            logger.AddAtrribute(this.addMtuAction, "reason", Action.tag_reasons[this.action.type]);

            logger.Parameter ( this.addMtuAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if ( ! string.IsNullOrEmpty ( this.user ) )
                logger.Parameter(this.addMtuAction, new Parameter("User", "User", this.user ) );

            if ( isReplaceMtu &&
                 form.ContainsParameter ( FIELD.MTU_ID_OLD ) )
                logger.Parameter ( this.addMtuAction, form.OldMtuId );

            logger.Parameter ( this.addMtuAction, new Parameter ( "MtuId",   "MTU ID",   this.mtuBasicInfo.Id   ) );
            logger.Parameter ( this.addMtuAction, new Parameter ( "MtuType", "MTU Type", this.mtuBasicInfo.Type ) );
            logger.Parameter ( this.addMtuAction, form.ReadInterval );

            bool   useDailyReads    = ( global.AllowDailyReads && 
                                        mtu.DailyReads &&
                                        form.ContainsParameter ( FIELD.SNAP_READS ) );

            string dailyReads       = ( useDailyReads ) ? form.SnapReads.Value : DISABLED;
            string dailyGmtHourRead = ( useDailyReads ) ? form.SnapReads.Value : DISABLED;
            logger.Parameter(this.addMtuAction, new Parameter("DailyGMTHourRead", "GMT Daily Reads", dailyGmtHourRead));
            
            if ( ! dailyGmtHourRead.Equals ( DISABLED ) )
                logger.Parameter(this.addMtuAction, new Parameter("DailyReads", "Daily Reads", dailyReads));

            if ( mtu.FastMessageConfig )
                logger.Parameter ( this.addMtuAction, form.TwoWay );

            // Related to F12WAYRegister1XX registers
            string afc = ( mtu.TimeToSync &&
                           global.AFC &&
                           map.MtuSoftVersion >= 19 ) ? "Set" : "Off";
            logger.Parameter ( this.addMtuAction, new Parameter ( "AFC", "AFC", afc ) );

            #endregion

            #region Certificate
            
            Mobile.ConfigData data = Mobile.configData;

            // Avoid try to log encryption info when not it has not been performed
            if ( data.isMtuEncrypted )
            {
                logger.Parameter ( this.addMtuAction, new Parameter ( "EncryptionIndex", "Encryption Index", map.EncryptionIndex ) );
            
                // Using certificate with public key
                if ( data.IsCertLoaded )
                {
                    Console.WriteLine ( "Using certificate creating activity log" );
                    
                    logger.Parameter ( this.addMtuAction, new Parameter ( "MtuSymKey", "MtuSymKey", data.RandomKeyAndShaEncryptedInBase64 ) );
                    logger.Parameter ( this.addMtuAction, new Parameter ( "HeadendCertThumb",     "HeadendCertThumb",      data.certificate.Thumbprint ) );
                    logger.Parameter ( this.addMtuAction, new Parameter ( "HeadendCertValidTill", "HeadendCertExpiration", data.certificate.NotAfter.ToString ( "mm/dd/yy hh:mm:ss tt" ) ) );
                    logger.Parameter ( this.addMtuAction, new Parameter ( "DeviceCertSubject",    "DeviceCertSubject",     data.certificate.Subject    ) );
                }
                // No certificate present
                else
                {
                    Console.WriteLine ( "Not using certificate creating activity log" );
                
                    logger.Parameter ( this.addMtuAction, new Parameter ( "MtuSymKey", "MtuSymKey", data.RandomKeyAndShaInBase64 ) );
                }
            }

            #endregion

            #region Port 1

            Meter meter = ( ! isFromScripting ) ?
                ( Meter )form.Meter.Value :
                Configuration.GetInstance().getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter.Value ) );

            XElement port = new XElement("Port");
            logger.AddAtrribute(port, "display", "Port 1");
            logger.AddAtrribute(port, "number", "1");

            logger.Parameter ( port, form.AccountNumber );

            if ( global.WorkOrderRecording )
                logger.Parameter ( port, form.WorkOrder );

            if ( isReplaceMeter )
            {
                if ( global.UseMeterSerialNumber )
                    logger.Parameter ( port, form.MeterNumberOld );
                
                if ( global.MeterWorkRecording )
                    logger.Parameter ( port, form.OldMeterWorking );
                
                if ( global.OldReadingRecording )
                    logger.Parameter ( port, form.MeterReadingOld );
                
                if ( global.RegisterRecording )
                    logger.Parameter ( port, form.ReplaceMeterRegister );
                
                if ( global.AutoRegisterRecording )
                {
                    temp = ( string.Equals ( form.MeterNumber, form.MeterNumberOld ) ) ?
                             "Register head change" : "Meter change";
                    logger.Parameter ( port, new Parameter ( "MeterRegisterAutoStatus", temp, "Meter Register Auto Status" ) );
                }
            }

            string meterType = string.Format("({0}) {1}", meter.Id, meter.Display);
            logger.Parameter ( port, new Parameter("MeterType", "Meter Type", meterType));
            logger.Parameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter.Id.ToString()));
            logger.Parameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter.Vendor));
            logger.Parameter ( port, new Parameter("MeterModel", "Meter Model", meter.Model));
            
            if ( global.UseMeterSerialNumber )
                logger.Parameter ( port, form.MeterNumber );
            
            logger.Parameter ( port, form.MeterReading );
            
            logger.Parameter ( port, new Parameter("PulseHi","Pulse Hi Time", meter.PulseHiTime.ToString ().PadLeft ( 2, '0' ) ) );
            logger.Parameter ( port, new Parameter("PulseLo","Pulse Low Time", meter.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

            this.addMtuAction.Add(port);

            #endregion

            #region Port 2

            if ( form.usePort2 )
            {
                Meter meter2 = ( ! isFromScripting ) ?
                    ( Meter )form.Meter_2.Value :
                    Configuration.GetInstance().getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter_2.Value ) );

                port = new XElement ( "Port");
                logger.AddAtrribute ( port, "display", "Port 2" );
                logger.AddAtrribute ( port, "number", "2" );

                logger.Parameter ( port, form.AccountNumber_2 );

                if ( global.WorkOrderRecording )
                    logger.Parameter ( port, form.WorkOrder_2 );

                if ( isReplaceMeter )
                {
                    if ( global.UseMeterSerialNumber )
                        logger.Parameter ( port, form.MeterNumberOld_2 );

                    if ( global.MeterWorkRecording )
                        logger.Parameter ( port, form.OldMeterWorking_2 );
                    
                    if ( global.OldReadingRecording )
                        logger.Parameter ( port, form.MeterReadingOld_2 );
                    
                    if ( global.RegisterRecording )
                        logger.Parameter ( port, form.ReplaceMeterRegister_2 );
                    
                    if ( global.AutoRegisterRecording )
                    {
                        temp = ( string.Equals ( form.MeterNumber_2, form.MeterNumberOld_2 ) ) ?
                                 "Register head change" : "Meter change";
                        logger.Parameter ( port, new Parameter ( "MeterRegisterAutoStatus", temp, "Meter Register Auto Status" ) );
                    }
                }
                
                string meterType2 = string.Format("({0}) {1}", meter2.Id, meter2.Display);
                logger.Parameter ( port, new Parameter("MeterType", "Meter Type", meterType2));
                logger.Parameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter2.Id.ToString()));
                logger.Parameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter2.Vendor));
                logger.Parameter ( port, new Parameter("MeterModel", "Meter Model", meter2.Model));
                
                if ( global.UseMeterSerialNumber )
                    logger.Parameter ( port, form.MeterNumber_2 );
                    
                logger.Parameter ( port, form.MeterReading_2 );

                logger.Parameter ( port, new Parameter("PulseHi","Pulse Hi Time", meter2.PulseHiTime.ToString ().PadLeft ( 2, '0' ) ) );
                logger.Parameter ( port, new Parameter("PulseLo","Pulse Low Time", meter2.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

                this.addMtuAction.Add(port);
            }

            #endregion

            #region Alarms

            if ( mtu.RequiresAlarmProfile )
            {
                Alarm alarms = (Alarm)form.Alarm.Value;
                if ( alarms != null )
                {
                    XElement alarmSelection = new XElement("AlarmSelection");
                    logger.AddAtrribute(alarmSelection, "display", "Alarm Selection");

                    string alarmConfiguration = alarms.Name;
                    logger.Parameter(alarmSelection, new Parameter("AlarmConfiguration", "Alarm Configuration Name", alarmConfiguration));

                    string immediateAlarmTransmit = ( alarms.ImmediateAlarmTransmit ) ? "True" : "False";
                    logger.Parameter(alarmSelection, new Parameter("ImmediateAlarm", "Immediate Alarm Transmit", immediateAlarmTransmit));

                    string urgentAlarm = ( alarms.DcuUrgentAlarm ) ? "True" : "False";
                    logger.Parameter(alarmSelection, new Parameter("UrgentAlarm", "DCU Urgent Alarm Transmit", urgentAlarm));

                    string overlap = alarms.Overlap.ToString();
                    logger.Parameter(alarmSelection, new Parameter("Overlap", "Message Overlap", overlap));

                    if ( mtu.MagneticTamper )
                        logger.Parameter ( alarmSelection, new Parameter("MagneticTamper", "Magnetic Tamper", map.MagneticTamperStatus ));

                    if ( mtu.RegisterCoverTamper )
                        logger.Parameter ( alarmSelection, new Parameter("RegisterCoverTamper", "Register Cover Tamper", map.RegisterCoverTamperStatus ));

                    if ( mtu.TiltTamper )
                        logger.Parameter( alarmSelection, new Parameter("TiltTamper", "Tilt Tamper", map.TiltTamperStatus ));

                    if ( mtu.ReverseFlowTamper )
                    {
                        logger.Parameter ( alarmSelection, new Parameter("ReverseFlow", "Reverse Flow Tamper", map.ReverseFlowTamperStatus ));
                        logger.Parameter(alarmSelection, new Parameter("FlowDirection", "Flow Direction", meter.Flow.ToString() ));
                    }

                    if ( mtu.InterfaceTamper)
                        logger.Parameter ( alarmSelection, new Parameter("InterfaceTamper", "Interface Tamper", map.InterfaceTamperStatus ));

                    this.addMtuAction.Add(alarmSelection);
                }
            }

            #endregion

            // TODO (encoders)
            #region Demands

            if ( mtu.MtuDemand )
            {
                XElement demandConf = new XElement("DemandConfiguration");
                logger.AddAtrribute(demandConf, "display", "Demand Configuration");
                logger.Parameter(demandConf, new Parameter("ConfigurationName", "Configuration Name", "Default")); // TODO: replace real value
                logger.Parameter(demandConf, new Parameter("MtuNumLowPriorityMsg", "Mtu Num Low Priority Msg", "2")); // TODO: replace real value
                logger.Parameter(demandConf, new Parameter("MtuPrimaryWindowInterval", "Mtu Primary WindowInterval", "180")); // TODO: replace real value
                logger.Parameter(demandConf, new Parameter("MtuWindowAStart", "Mtu Window A Start", "0")); // TODO: replace real value
                logger.Parameter(demandConf, new Parameter("MtuWindowBStart", "Mtu Window B Start", "0")); // TODO: replace real value
                logger.Parameter(demandConf, new Parameter("MtuPrimaryWindowIntervalB", "Mtu Primary WindowInterval B", "3600")); // TODO: replace real value
                logger.Parameter(demandConf, new Parameter("MtuPrimaryWindowOffset", "Mtu Primary Window Offset", "51")); // TODO: replace real value
                this.addMtuAction.Add(demandConf);
            }

            #endregion

            #region Misc/Optional

            if ( form.ContainsParameter ( FIELD.GPS_LATITUDE  ) &&
                 form.ContainsParameter ( FIELD.GPS_LONGITUDE ) &&
                 form.ContainsParameter ( FIELD.GPS_ALTITUDE  ) )
            {
                //logger.logParameter ( this.addMtuAction, form.GPS_LATITUDE  );
                //logger.logParameter ( this.addMtuAction, form.GPS_LONGITUDE );
                //logger.logParameter ( this.addMtuAction, form.GPS_ALTITUDE  );

                logger.Parameter(this.addMtuAction, new Parameter("GPS_Y", "Lat", form.GPSLat.Value ));
                logger.Parameter(this.addMtuAction, new Parameter("GPS_X", "Long", form.GPSLon.Value ));
                logger.Parameter(this.addMtuAction, new Parameter("Altitude", "Elevation", form.GPSAlt.Value ));
            }

            if ( ! ( form.OptionalParams.Value is string ) )
            {
                List<Parameter> optionalParams = (List<Parameter>)form.OptionalParams.Value;

                if (optionalParams != null)
                    foreach (Parameter p in optionalParams)
                        logger.Parameter(this.addMtuAction, p);
            }

            #endregion
        }

        public void LogReadMtu(ActionResult result)
        {
            logger.AddAtrribute(this.readMtuAction, "display", Action.displays[ActionType.ReadMtu]);
            logger.AddAtrribute(this.readMtuAction, "type", Action.tag_types[ActionType.ReadMtu]);

            InterfaceParameters[] parameters = Configuration.GetInstance().getLogInterfaceFields( form.mtu, ActionType.ReadMtu );
            foreach (InterfaceParameters parameter in parameters)
            {
                try
                {
                    if (parameter.Name == "Port")
                    {
                        ActionResult[] ports = result.getPorts();
                        for (int i = 0; i < ports.Length; i++)
                            logger.Port(i, this.readMtuAction, ports[i], parameter.Parameters.ToArray());
                    }
                    else
                        logger.ComplexParameter(this.readMtuAction, result, parameter);
                }
                catch ( Exception e )
                {
                    
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
            #if DEBUG

            string uniUri = Path.Combine ( Mobile.LogUniPath,
                this.mtuBasicInfo.Type + "-" + this.action.type + ( ( form.mtu.SpecialSet ) ? "-Encrypted" : "" ) + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.logger.CreateFileIfNotExist ( false, uniUri );
            
            XDocument uniDoc = XDocument.Load ( uniUri );
            XElement uniMtus = uniDoc.Root.Element ( "Mtus" );
            uniMtus.Add ( this.addMtuAction );
            
            uniDoc.Save ( uniUri );
            
            #endif
            
            // Write in ActivityLog
            if ( Action.IsFromScripting &&
                 ! Configuration.GetInstance ().global.ScriptOnly )
            {
                // Reset fixed_name to add to the ActivityLog in CreateFileIfNotExist
                this.logger.ResetFixedName ();
                
                String uri = this.logger.CreateFileIfNotExist ();
                doc  = XDocument.Load ( uri );
                mtus = doc.Root.Element ( "Mtus" );
                mtus.Add ( this.addMtuAction );
                doc.Save(uri);
            }
            
            return uniDoc.ToString ();
        }
    }
}
