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
        private ActionType actionType;
        private MTUBasicInfo mtuBasicInfo;
        private string logUri;

        private XDocument doc;
        private XElement  addMtuAction;
        private XElement  turnOffAction;
        private XElement  turnOnAction;
        private XElement  readMtuAction;

        public AddMtuLog(Logger logger, dynamic form, string user, bool isFromScripting )
        {
            this.logger = logger;
            this.form = form;
            this.actionType = (ActionType)form.actionType;
            this.user = user;
            this.mtuBasicInfo = MtuForm.mtuBasicInfo;
            this.logUri = this.logger.CreateFileIfNotExist ( ! isFromScripting );

            this.addMtuAction  = new XElement("Action");
            this.turnOffAction = new XElement("Action");
            this.turnOnAction  = new XElement("Action");
            this.readMtuAction = new XElement("Action");
        }

        public void LogTurnOff ()
        {
            logger.addAtrribute(this.turnOffAction, "display", Action.displays[ActionType.TurnOffMtu]);
            logger.addAtrribute(this.turnOffAction, "type", Action.tag_types[ActionType.TurnOffMtu]);

            logger.logParameter(this.turnOffAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
                logger.logParameter(this.turnOffAction, new Parameter("User", "User", this.user));

            logger.logParameter(this.turnOffAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogTurnOn ()
        {
            logger.addAtrribute(this.turnOnAction, "display", Action.displays[ActionType.TurnOnMtu]);
            logger.addAtrribute(this.turnOnAction, "type", Action.tag_types[ActionType.TurnOnMtu]);

            logger.logParameter(this.turnOnAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
                logger.logParameter(this.turnOnAction, new Parameter("User", "User", this.user));

            logger.logParameter(this.turnOnAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogAddMtu ( bool isFromScripting = false )
        {
            Mtu     mtu    = form.mtu;
            Global  global = form.global;
            dynamic map    = form.map;
            string  temp   = string.Empty;
            string  DISABLED = MemoryMap.MemoryMap.DISABLED;
            string  ENABLED  = MemoryMap.MemoryMap.ENABLED;

            bool isReplaceMeter = form.actionType == ActionType.ReplaceMeter           ||
                                  form.actionType == ActionType.ReplaceMtuReplaceMeter ||
                                  form.actionType == ActionType.AddMtuReplaceMeter;
            bool isReplaceMtu   = form.actionType == ActionType.ReplaceMTU ||
                                  form.actionType == ActionType.ReplaceMtuReplaceMeter;

            #region General

            //logger.addAtrribute ( this.addMtuAction, "display", addMtuDisplay );
            // logger.addAtrribute ( this.addMtuAction, "type",    addMtuType    );
            // logger.addAtrribute ( this.addMtuAction, "reason",  addMtuReason  );
            logger.addAtrribute(this.addMtuAction, "display", Action.displays[this.actionType]);
            logger.addAtrribute(this.addMtuAction, "type", Action.tag_types[this.actionType]);
            logger.addAtrribute(this.addMtuAction, "reason", Action.tag_reasons[this.actionType]);

            logger.logParameter ( this.addMtuAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if ( ! string.IsNullOrEmpty ( this.user ) )
                logger.logParameter(this.addMtuAction, new Parameter("User", "User", this.user ) );

            if ( isReplaceMtu &&
                 form.ContainsParameter ( FIELD.MTU_ID_OLD ) )
                logger.logParameter ( this.addMtuAction, form.OldMtuId );

            logger.logParameter ( this.addMtuAction, new Parameter ( "MtuId",   "MTU ID",   this.mtuBasicInfo.Id   ) );
            logger.logParameter ( this.addMtuAction, new Parameter ( "MtuType", "MTU Type", this.mtuBasicInfo.Type ) );
            logger.logParameter ( this.addMtuAction, form.ReadInterval );

            bool   useDailyReads    = ( global.AllowDailyReads && mtu.DailyReads );
            string dailyReads       = ( useDailyReads ) ? form.SnapReads.Value : DISABLED;
            string dailyGmtHourRead = ( useDailyReads ) ? form.SnapReads.Value : DISABLED;
            logger.logParameter(this.addMtuAction, new Parameter("DailyGMTHourRead", "GMT Daily Reads", dailyGmtHourRead));
            
            if ( ! dailyGmtHourRead.Equals ( DISABLED ) )
                logger.logParameter(this.addMtuAction, new Parameter("DailyReads", "Daily Reads", dailyReads));

            if ( mtu.FastMessageConfig )
                logger.logParameter ( this.addMtuAction, form.TwoWay );

            // Related to F12WAYRegister1XX registers
            string afc = ( mtu.TimeToSync &&
                           global.AFC &&
                           map.MtuSoftVersion >= 19 ) ? "Set" : "Off";
            logger.logParameter ( this.addMtuAction, new Parameter ( "AFC", "AFC", afc ) );

            #endregion

            #region Certificate
            
            Mobile.ConfigData data = Mobile.configData;

            // Avoid try to log encryption info when not it has not been performed
            if ( data.isMtuEncrypted )
            {
                // Using certificate with public key
                if ( data.IsCertLoaded )
                {
                    Console.WriteLine ( "Using certificate creating activity log" );
                
                    logger.logParameter ( this.addMtuAction, new Parameter ( "MtuSymKey", "MtuSymKey", data.RandomKeyAndShaEncryptedInBase64 ) );
                    logger.logParameter ( this.addMtuAction, new Parameter ( "HeadendCertThumb",     "HeadendCertThumb",      data.certificate.Thumbprint ) );
                    logger.logParameter ( this.addMtuAction, new Parameter ( "HeadendCertValidTill", "HeadendCertExpiration", data.certificate.NotAfter.ToString ( "mm/dd/yy hh:mm:ss tt" ) ) );
                    logger.logParameter ( this.addMtuAction, new Parameter ( "DeviceCertSubject",    "DeviceCertSubject",     data.certificate.Subject    ) );
                }
                // No certificate present
                else
                {
                    Console.WriteLine ( "Not using certificate creating activity log" );
                
                    logger.logParameter ( this.addMtuAction, new Parameter ( "MtuSymKey", "MtuSymKey", data.RandomKeyAndShaInBase64 ) );
                }
            }

            #endregion

            #region Port 1

            Meter meter = ( ! isFromScripting ) ?
                ( Meter )form.Meter.Value :
                Configuration.GetInstance().getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter.Value ) );

            XElement port = new XElement("Port");
            logger.addAtrribute(port, "display", "Port 1");
            logger.addAtrribute(port, "number", "1");

            logger.logParameter ( port, form.AccountNumber );

            if ( global.WorkOrderRecording )
                logger.logParameter ( port, form.WorkOrder );

            if ( isReplaceMeter )
            {
                if ( global.UseMeterSerialNumber )
                    logger.logParameter ( port, form.MeterNumberOld );
                
                if ( global.MeterWorkRecording )
                    logger.logParameter ( port, form.OldMeterWorking );
                
                if ( global.OldReadingRecording )
                    logger.logParameter ( port, form.MeterReadingOld );
                
                if ( global.RegisterRecording )
                    logger.logParameter ( port, form.ReplaceMeterRegister );
                
                if ( global.AutoRegisterRecording )
                {
                    temp = ( string.Equals ( form.MeterNumber, form.MeterNumberOld ) ) ?
                             "Register head change" : "Meter change";
                    logger.logParameter ( port, new Parameter ( "MeterRegisterAutoStatus", temp, "Meter Register Auto Status" ) );
                }
            }

            string meterType = string.Format("({0}) {1}", meter.Id, meter.Display);
            logger.logParameter ( port, new Parameter("MeterType", "Meter Type", meterType));
            logger.logParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter.Id.ToString()));
            logger.logParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter.Vendor));
            logger.logParameter ( port, new Parameter("MeterModel", "Meter Model", meter.Model));
            
            if ( global.UseMeterSerialNumber )
                logger.logParameter ( port, form.MeterNumber );
            
            logger.logParameter ( port, form.MeterReading );
            
            logger.logParameter ( port, new Parameter("PulseHi","Pulse Hi Time", meter.PulseHiTime.ToString ().PadLeft ( 2, '0' ) ) );
            logger.logParameter ( port, new Parameter("PulseLo","Pulse Low Time", meter.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

            this.addMtuAction.Add(port);

            #endregion

            #region Port 2

            if ( form.usePort2 )
            {
                Meter meter2 = ( ! isFromScripting ) ?
                    ( Meter )form.Meter_2.Value :
                    Configuration.GetInstance().getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter_2.Value ) );

                port = new XElement ( "Port");
                logger.addAtrribute ( port, "display", "Port 2" );
                logger.addAtrribute ( port, "number", "2" );

                logger.logParameter ( port, form.AccountNumber_2 );

                if ( global.WorkOrderRecording )
                    logger.logParameter ( port, form.WorkOrder_2 );

                if ( isReplaceMeter )
                {
                    if ( global.UseMeterSerialNumber )
                        logger.logParameter ( port, form.MeterNumberOld_2 );

                    if ( global.MeterWorkRecording )
                        logger.logParameter ( port, form.OldMeterWorking_2 );
                    
                    if ( global.OldReadingRecording )
                        logger.logParameter ( port, form.MeterReadingOld_2 );
                    
                    if ( global.RegisterRecording )
                        logger.logParameter ( port, form.ReplaceMeterRegister_2 );
                    
                    if ( global.AutoRegisterRecording )
                    {
                        temp = ( string.Equals ( form.MeterNumber_2, form.MeterNumberOld_2 ) ) ?
                                 "Register head change" : "Meter change";
                        logger.logParameter ( port, new Parameter ( "MeterRegisterAutoStatus", temp, "Meter Register Auto Status" ) );
                    }
                }
                
                string meterType2 = string.Format("({0}) {1}", meter2.Id, meter2.Display);
                logger.logParameter ( port, new Parameter("MeterType", "Meter Type", meterType2));
                logger.logParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter2.Id.ToString()));
                logger.logParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter2.Vendor));
                logger.logParameter ( port, new Parameter("MeterModel", "Meter Model", meter2.Model));
                
                if ( global.UseMeterSerialNumber )
                    logger.logParameter ( port, form.MeterNumber_2 );
                    
                logger.logParameter ( port, form.MeterReading_2 );

                logger.logParameter ( port, new Parameter("PulseHi","Pulse Hi Time", meter2.PulseHiTime.ToString ().PadLeft ( 2, '0' ) ) );
                logger.logParameter ( port, new Parameter("PulseLo","Pulse Low Time", meter2.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

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
                    logger.addAtrribute(alarmSelection, "display", "Alarm Selection");

                    string alarmConfiguration = alarms.Name;
                    logger.logParameter(alarmSelection, new Parameter("AlarmConfiguration", "Alarm Configuration Name", alarmConfiguration));

                    string immediateAlarmTransmit = ( alarms.ImmediateAlarmTransmit ) ? "True" : "False";
                    logger.logParameter(alarmSelection, new Parameter("ImmediateAlarm", "Immediate Alarm Transmit", immediateAlarmTransmit));

                    string urgentAlarm = ( alarms.DcuUrgentAlarm ) ? "True" : "False";
                    logger.logParameter(alarmSelection, new Parameter("UrgentAlarm", "DCU Urgent Alarm Transmit", urgentAlarm));

                    string overlap = alarms.Overlap.ToString();
                    logger.logParameter(alarmSelection, new Parameter("Overlap", "Message Overlap", overlap));

                    if ( mtu.MagneticTamper )
                        logger.logParameter ( alarmSelection, new Parameter("MagneticTamper", "Magnetic Tamper", map.MagneticTamperStatus ));

                    if ( mtu.RegisterCoverTamper )
                        logger.logParameter ( alarmSelection, new Parameter("RegisterCoverTamper", "Reg. Cover Tamper", map.RegisterCoverTamperStatus ));

                    if ( mtu.TiltTamper )
                        logger.logParameter( alarmSelection, new Parameter("TiltTamper", "Tilt Tamper", map.TiltTamperStatus ));

                    if ( mtu.ReverseFlowTamper )
                    {
                        logger.logParameter ( alarmSelection, new Parameter("ReverseFlow", "Rev. Flow Tamper", map.ReverseFlowTamperStatus ));
                        logger.logParameter(alarmSelection, new Parameter("FlowDirection", "Flow Direction", meter.Flow.ToString() ));
                    }

                    if ( mtu.InterfaceTamper)
                        logger.logParameter ( alarmSelection, new Parameter("InterfaceTamper", "Interface Tamper", map.InterfaceTamperStatus ));

                    this.addMtuAction.Add(alarmSelection);
                }
            }

            #endregion

            // TODO (encoders)
            #region Demands

            if ( mtu.MtuDemand )
            {
                XElement demandConf = new XElement("DemandConfiguration");
                logger.addAtrribute(demandConf, "display", "Demand Configuration");
                logger.logParameter(demandConf, new Parameter("ConfigurationName", "Configuration Name", "Default")); // TODO: replace real value
                logger.logParameter(demandConf, new Parameter("MtuNumLowPriorityMsg", "Mtu Num Low Priority Msg", "2")); // TODO: replace real value
                logger.logParameter(demandConf, new Parameter("MtuPrimaryWindowInterval", "Mtu Primary WindowInterval", "180")); // TODO: replace real value
                logger.logParameter(demandConf, new Parameter("MtuWindowAStart", "Mtu Window A Start", "0")); // TODO: replace real value
                logger.logParameter(demandConf, new Parameter("MtuWindowBStart", "Mtu Window B Start", "0")); // TODO: replace real value
                logger.logParameter(demandConf, new Parameter("MtuPrimaryWindowIntervalB", "Mtu Primary WindowInterval B", "3600")); // TODO: replace real value
                logger.logParameter(demandConf, new Parameter("MtuPrimaryWindowOffset", "Mtu Primary Window Offset", "51")); // TODO: replace real value
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

                logger.logParameter(this.addMtuAction, new Parameter("GPS_Y", "Lat", form.GPSLat.Value ));
                logger.logParameter(this.addMtuAction, new Parameter("GPS_X", "Long", form.GPSLon.Value ));
                logger.logParameter(this.addMtuAction, new Parameter("Altitude", "Elevation", form.GPSAlt.Value ));
            }

            if ( ! ( form.OptionalParams.Value is string ) )
            {
                List<Parameter> optionalParams = (List<Parameter>)form.OptionalParams.Value;

                if (optionalParams != null)
                    foreach (Parameter p in optionalParams)
                        logger.logParameter(this.addMtuAction, p);
            }

            #endregion
        }

        public void LogReadMtu(ActionResult result)
        {
            logger.addAtrribute(this.readMtuAction, "display", Action.displays[ActionType.ReadMtu]);
            logger.addAtrribute(this.readMtuAction, "type", Action.tag_types[ActionType.ReadMtu]);

            InterfaceParameters[] parameters = Configuration.GetInstance().getLogInterfaceFields( form.mtu.Id, ActionType.ReadMtu );
            foreach (InterfaceParameters parameter in parameters)
            {
                try
                {
                    if (parameter.Name == "Port")
                    {
                        ActionResult[] ports = result.getPorts();
                        for (int i = 0; i < ports.Length; i++)
                            logger.logPort(i, this.readMtuAction, ports[i], parameter.Parameters.ToArray());
                    }
                    else
                        logger.logComplexParameter(this.readMtuAction, result, parameter);
                }
                catch ( Exception e )
                {
                    
                }
            }
        }

        public void Save ()
        {
            this.addMtuAction.Add ( this.turnOffAction );
            this.addMtuAction.Add ( this.turnOnAction  );
            this.addMtuAction.Add ( this.readMtuAction );

            this.doc = XDocument.Load ( logUri );
            XElement mtus = doc.Root.Element ( "Mtus" );
            mtus.Add ( this.addMtuAction );

            string resultStr = doc.ToString ();

            doc.Save ( logUri );
            
            #if DEBUG
            doc.Save ( Path.Combine ( Mobile.GetPathLogsUni (), this.actionType + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" ) );
            #endif
        }

        public override string ToString ()
        {
            if ( this.doc == null )
                this.Save ();

            return doc.ToString ();         
        }
    }
}
