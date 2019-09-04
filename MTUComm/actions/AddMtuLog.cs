using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MTUComm.actions;
using Library;
using Xml;
using System.Threading.Tasks;

using System.IO;

using FIELD = MTUComm.actions.AddMtuForm.FIELD;
using ActionType = MTUComm.Action.ActionType;
using System.Text;

namespace MTUComm
{
    public class AddMtuLog
    {
        private Configuration config;
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

        public AddMtuLog(Logger logger, dynamic form, string user )
        {
            this.logger = logger;
            this.form = form;
            this.user = user;
            this.mtuBasicInfo = MtuForm.mtuBasicInfo;
            this.logUri = this.logger.CreateFileIfNotExist ();
            
            this.config = Singleton.Get.Configuration;
            this.action = Singleton.Get.Action;

            this.addMtuAction  = new XElement("Action");
            this.turnOffAction = new XElement("Action");
            this.turnOnAction  = new XElement("Action");
            this.readMtuAction = new XElement("Action");
        }

        public void LogTurnOff ()
        {
            logger.AddAtrribute(this.turnOffAction, "display", Action.logDisplays[ActionType.TurnOffMtu]);
            logger.AddAtrribute(this.turnOffAction, "type", Action.logTypes[ActionType.TurnOffMtu]);

            logger.AddParameter(this.turnOffAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") ));

            if (!string.IsNullOrEmpty(this.user))
                logger.AddParameter(this.turnOffAction, new Parameter("User", "User", this.user));

            logger.AddParameter(this.turnOffAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogTurnOn ()
        {
            logger.AddAtrribute(this.turnOnAction, "display", Action.logDisplays[ActionType.TurnOnMtu]);
            logger.AddAtrribute(this.turnOnAction, "type", Action.logTypes[ActionType.TurnOnMtu]);

            logger.AddParameter(this.turnOnAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
                logger.AddParameter(this.turnOnAction, new Parameter("User", "User", this.user));

            logger.AddParameter(this.turnOnAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public async Task LogAddMtu ()
        {
            Mtu     mtu    = form.mtu;
            Global  global = this.config.Global;
            dynamic map    = form.map;
            string  temp   = string.Empty;
            string  DISABLED = MemoryMap.MemoryMap.DISABLED;
            string  ENABLED  = MemoryMap.MemoryMap.ENABLED;

            ActionType actionType = this.action.Type;

            bool isReplaceMeter = actionType == ActionType.ReplaceMeter           ||
                                  actionType == ActionType.ReplaceMtuReplaceMeter ||
                                  actionType == ActionType.AddMtuReplaceMeter;
            bool isReplaceMtu   = actionType == ActionType.ReplaceMTU ||
                                  actionType == ActionType.ReplaceMtuReplaceMeter;

            #region General

            //logger.addAtrribute ( this.addMtuAction, "display", addMtuDisplay );
            // logger.addAtrribute ( this.addMtuAction, "type",    addMtuType    );
            // logger.addAtrribute ( this.addMtuAction, "reason",  addMtuReason  );
            logger.AddAtrribute(this.addMtuAction, "display", Action.logDisplays[this.action.Type]);
            logger.AddAtrribute(this.addMtuAction, "type", Action.logTypes[this.action.Type]);
            logger.AddAtrribute(this.addMtuAction, "reason", Action.logReasons[this.action.Type]);

            logger.AddParameter ( this.addMtuAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if ( ! string.IsNullOrEmpty ( this.user ) )
                logger.AddParameter(this.addMtuAction, new Parameter("User", "User", this.user ) );

            if ( isReplaceMtu &&
                 form.ContainsParameter ( FIELD.MTU_ID_OLD ) )
                logger.AddParameter ( this.addMtuAction, form.OldMtuId );

            logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuId",   "MTU ID",   this.mtuBasicInfo.Id   ) );
            logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuType", "MTU Type", this.mtuBasicInfo.Type ) );
            logger.AddParameter ( this.addMtuAction, form.ReadInterval );

            if ( form.ContainsParameter ( FIELD.SNAP_READS ) )
            {
                bool   useDailyReads    = ( global.AllowDailyReads && 
                                            mtu.DailyReads &&
                                            form.ContainsParameter ( FIELD.SNAP_READS ) );

                string dailyReads       = ( useDailyReads ) ? form.SnapReads.Value : DISABLED;
                string dailyGmtHourRead = ( useDailyReads ) ? form.SnapReads.Value : DISABLED;
                logger.AddParameter(this.addMtuAction, new Parameter("DailyGMTHourRead", "GMT Daily Reads", dailyGmtHourRead));
                
                if ( ! dailyGmtHourRead.Equals ( DISABLED ) )
                    logger.AddParameter(this.addMtuAction, new Parameter("DailyReads", "Daily Reads", dailyReads));
            }

            if ( global.TimeToSync &&
                 mtu.TimeToSync    &&
                 mtu.FastMessageConfig )
                logger.AddParameter ( this.addMtuAction, form.TwoWay );

            // Related to F12WAYRegister1XX registers
            string afc = ( mtu.TimeToSync &&
                           global.AFC &&
                           await map.MtuSoftVersion.GetValue () >= 19 ) ? "Set" : "OFF";
            logger.AddParameter ( this.addMtuAction, new Parameter ( "AFC", "AFC", afc ) );

            #endregion

            #region Additional tags

            // Add additional parameters for all actions except for the Add
            foreach ( Parameter param in this.action.AdditionalScriptParameters )
                logger.AddParameter ( this.addMtuAction, param );

            #endregion

            #region Certificate
            
            Mobile.ConfigData data = Mobile.configData;

            // Avoid try to log encryption info when not it has not been performed
            if ( data.isMtuEncrypted )
            {
                //logger.Parameter ( this.addMtuAction, new Parameter ( "Encryption", "Encrypted", map.Encryption.GetValue () ) );
                logger.AddParameter ( this.addMtuAction, new Parameter ( "EncryptionIndex", "Encryption Index", await map.EncryptionIndex.GetValue () ) );
            
                if ( ! mtu.IsFamilly35xx36xx )
                {
                    // Using certificate with public key
                    if ( data.IsCertLoaded )
                    {
                        Utils.Print ( "Using certificate creating activity log" );
                        
                        logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuSymKey",            "MtuSymKey",             data.RandomKeyAndShaEncryptedInBase64 ) );
                        logger.AddParameter ( this.addMtuAction, new Parameter ( "HeadendCertThumb",     "HeadendCertThumb",      data.certificate.Thumbprint ) );
                        logger.AddParameter ( this.addMtuAction, new Parameter ( "HeadendCertValidTill", "HeadendCertExpiration", data.certificate.NotAfter.ToString ( "MM/dd/yy hh:mm:ss tt" ) ) );
                        logger.AddParameter ( this.addMtuAction, new Parameter ( "DeviceCertSubject",    "DeviceCertSubject",     data.certificate.Subject    ) );
                    }
                    // No certificate present
                    else
                    {
                        Utils.Print ( "Not using certificate creating activity log" );
                    
                        logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuSymKey", "MtuSymKey", data.RandomKeyAndShaInBase64 ) );
                    }
                }
                // OnDemand 1.2 MTUs
                else
                {
                    logger.AddParameter ( this.addMtuAction, new Parameter ( "serverRND",          "serverRND",          Data.Get.ServerRND     ) );
                    logger.AddParameter ( this.addMtuAction, new Parameter ( "clientRND",          "clientRND",          Data.Get.ClientRND     ) );
                    logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuPublicKey",       "MtuPublicKey",       Data.Get.MtuPublicKey  ) );
                    logger.AddParameter ( this.addMtuAction, new Parameter ( "STAREncryptionType", "STAREncryptionType", mtu.STAREncryptionType ) );
                }
            }

            #endregion

            #region Port 1

            Meter meter = ( ! Data.Get.IsFromScripting ) ?
                ( Meter )form.Meter.Value :
                this.config.getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter.Value ) );

            XElement port = new XElement("Port");
            logger.AddAtrribute(port, "display", "Port 1");
            logger.AddAtrribute(port, "number", "1");

            logger.AddParameter ( port, form.AccountNumber );

            if ( global.WorkOrderRecording )
                logger.AddParameter ( port, form.WorkOrder );

            if ( isReplaceMeter )
            {
                if ( global.UseMeterSerialNumber )
                    logger.AddParameter ( port, form.MeterNumberOld );
                
                if ( global.MeterWorkRecording )
                    logger.AddParameter ( port, form.OldMeterWorking );
                
                if ( global.OldReadingRecording )
                    logger.AddParameter ( port, form.MeterReadingOld );
                
                if ( global.RegisterRecording )
                    logger.AddParameter ( port, form.ReplaceMeterRegister );
                
                if ( global.AutoRegisterRecording )
                {
                    temp = ( string.Equals ( form.MeterNumber, form.MeterNumberOld ) ) ?
                             "Register head change" : "Meter change";
                    logger.AddParameter ( port, new Parameter ( "MeterRegisterAutoStatus", temp, "Meter Register Auto Status" ) );
                }
            }

            string meterType = string.Format("({0}) {1}", meter.Id, meter.Display);
            logger.AddParameter ( port, new Parameter("MeterType", "Meter Type", meterType));
            logger.AddParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter.Id.ToString()));
            logger.AddParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter.Vendor));
            logger.AddParameter ( port, new Parameter("MeterModel", "Meter Model", meter.Model));
            
            if ( global.UseMeterSerialNumber )
                logger.AddParameter ( port, form.MeterNumber );
            
            if ( ! mtu.Port1.IsForEncoderOrEcoder )
                logger.AddParameter ( port, form.MeterReading );
            
            logger.AddParameter ( port, new Parameter("PulseHi","Pulse Hi Time", meter.PulseHiTime.ToString ().PadLeft ( 2, '0' ) ) );
            logger.AddParameter ( port, new Parameter("PulseLo","Pulse Low Time", meter.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

            this.addMtuAction.Add(port);

            #endregion

            #region Port 2

            if ( form.usePort2 )
            {
                Meter meter2 = ( ! Data.Get.IsFromScripting ) ?
                    ( Meter )form.Meter_2.Value :
                    this.config.getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter_2.Value ) );

                port = new XElement ( "Port");
                logger.AddAtrribute ( port, "display", "Port 2" );
                logger.AddAtrribute ( port, "number", "2" );

                logger.AddParameter ( port, form.AccountNumber_2 );

                if ( global.WorkOrderRecording )
                    logger.AddParameter ( port, form.WorkOrder_2 );

                if ( isReplaceMeter )
                {
                    if ( global.UseMeterSerialNumber )
                        logger.AddParameter ( port, form.MeterNumberOld_2 );

                    if ( global.MeterWorkRecording )
                        logger.AddParameter ( port, form.OldMeterWorking_2 );
                    
                    if ( global.OldReadingRecording )
                        logger.AddParameter ( port, form.MeterReadingOld_2 );
                    
                    if ( global.RegisterRecording )
                        logger.AddParameter ( port, form.ReplaceMeterRegister_2 );
                    
                    if ( global.AutoRegisterRecording )
                    {
                        temp = ( string.Equals ( form.MeterNumber_2, form.MeterNumberOld_2 ) ) ?
                                 "Register head change" : "Meter change";
                        logger.AddParameter ( port, new Parameter ( "MeterRegisterAutoStatus", temp, "Meter Register Auto Status" ) );
                    }
                }
                
                string meterType2 = string.Format("({0}) {1}", meter2.Id, meter2.Display);
                logger.AddParameter ( port, new Parameter("MeterType", "Meter Type", meterType2));
                logger.AddParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter2.Id.ToString()));
                logger.AddParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter2.Vendor));
                logger.AddParameter ( port, new Parameter("MeterModel", "Meter Model", meter2.Model));
                
                if ( global.UseMeterSerialNumber )
                    logger.AddParameter ( port, form.MeterNumber_2 );
                
                if ( ! mtu.Port2.IsForEncoderOrEcoder )
                    logger.AddParameter ( port, form.MeterReading_2 );

                logger.AddParameter ( port, new Parameter("PulseHi","Pulse Hi Time", meter2.PulseHiTime.ToString ().PadLeft ( 2, '0' ) ) );
                logger.AddParameter ( port, new Parameter("PulseLo","Pulse Low Time", meter2.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

                this.addMtuAction.Add(port);
            }

            #endregion

            #region Alarms

            if ( mtu.RequiresAlarmProfile )
            {
                Alarm alarms = ( Alarm )form.Alarm.Value;
                if ( alarms != null )
                {
                    XElement alarmSelection = new XElement ( "AlarmSelection" );
                    logger.AddAtrribute ( alarmSelection, "display", "Alarm Selection" );

                    dynamic AddParamCond = new Action<string,string,string,bool> (
                        ( tag, display, value, condition ) => {
                            if ( condition )
                                logger.AddParameter ( alarmSelection,
                                    new Parameter ( tag, display, value ) );
                        });
                    
                    dynamic AddParameter = new Action<string,string,string> (
                        ( tag, display, value ) =>
                            AddParamCond ( tag, display, value, true ) );

                    string alarmConfiguration = alarms.Name;
                    AddParameter ( "AlarmConfiguration",        "Alarm Configuration Name",     alarmConfiguration );
                    AddParameter ( "Overlap",                   "Message Overlap",              alarms.Overlap.ToString () );
                    AddParameter ( "ImmediateAlarm",            "Immediate Alarm Transmit",     alarms.ImmediateAlarmTransmit.ToString () );
                    AddParamCond ( "UrgentAlarm",               "DCU Urgent Alarm Transmit",    alarms.DcuUrgentAlarm.ToString (),                          map.ContainsMember ( "UrgentAlarm" ) );
                    AddParamCond ( "MemoryMapError",            "Memory Map Error",             await map.MemoryMapTamperStatus.GetValue (),                mtu.MemoryMapError );
                    AddParamCond ( "MemoryMapErrorImm",         "Memory Map Error Imm",         await map.MemoryMapImmTamperStatus.GetValue (),             mtu.MemoryMapErrorImm );
                    AddParamCond ( "ProgramMemoryError",        "Program Memory Error",         await map.ProgramMemoryTamperStatus.GetValue (),            mtu.ProgramMemoryError );
                    AddParamCond ( "ProgramMemoryErrorImm",     "Program Memory Error Imm",     await map.ProgramMemoryImmTamperStatus.GetValue (),         mtu.ProgramMemoryErrorImm );
                    AddParamCond ( "MoistureDetect",            "Moisture Detect",              await map.MoistureTamperStatus.GetValue (),                 mtu.MoistureDetect );
                    AddParamCond ( "MoistureDetectImm",         "Moisture Detect Imm",          await map.MoistureImmTamperStatus.GetValue (),              mtu.MoistureDetectImm );
                    AddParamCond ( "EnergizerLastGasp",         "Energizer Last Gasp",          await map.EnergizerLastGaspTamperStatus.GetValue (),        mtu.EnergizerLastGasp );
                    AddParamCond ( "EnergizerLastGaspImm",      "Energizer Last Gasp Imm",      await map.EnergizerLastGaspImmTamperStatus.GetValue (),     mtu.EnergizerLastGaspImm );
                    AddParamCond ( "InsufficentMemory",         "Insufficent Memory",           await map.InsufficientMemoryTamperStatus.GetValue (),       mtu.InsufficientMemory );
                    AddParamCond ( "InsufficentMemoryImm",      "Insufficent Memory Imm",       await map.InsufficientMemoryImmTamperStatus.GetValue (),    mtu.InsufficientMemoryImm );
                    AddParamCond ( "CutAlarmCable",             "Cut Alarm Cable",              await map.GasCutWireTamperStatus.GetValue (),               mtu.GasCutWireAlarm );
                    AddParamCond ( "Cut2AlarmCable",            "Cut Port2 Alarm Cable",        await map.P2GasCutWireTamperStatus.GetValue (),             form.usePort2 && mtu.GasCutWireAlarm );
                    AddParamCond ( "SerialComProblem",          "Serial Com Problem",           await map.SerialComProblemTamperStatus.GetValue (),         mtu.SerialComProblem );
                    AddParamCond ( "SerialComProblemImm",       "Serial Com Problem Imm",       await map.SerialComProblemImmTamperStatus.GetValue (),      mtu.SerialComProblemImm );
                    AddParamCond ( "LastGasp",                  "Last Gasp",                    await map.LastGaspTamperStatus.GetValue (),                 mtu.LastGasp );
                    AddParamCond ( "LastGaspImm",               "Last Gasp Imm",                await map.LastGaspImmTamperStatus.GetValue (),              mtu.LastGaspImm );
                    AddParamCond ( "TiltTamper",                "Tilt Tamper",                  await map.TiltTamperStatus.GetValue (),                     mtu.TiltTamper );
                    AddParamCond ( "TiltTamperImm",             "Tilt Tamper Imm",              await map.TiltImmTamperStatus.GetValue (),                  mtu.TiltTamperImm );
                    AddParamCond ( "MagneticTamper",            "Magnetic Tamper",              await map.MagneticTamperStatus.GetValue (),                 mtu.MagneticTamper );
                    AddParamCond ( "MagneticTamperImm",         "Magnetic Tamper Imm",          await map.MagneticImmTamperStatus.GetValue (),              mtu.MagneticTamperImm );
                    AddParamCond ( "InterfaceTamper",           "Interface Tamper",             await map.InterfaceTamperStatus.GetValue (),                mtu.InterfaceTamper );
                    AddParamCond ( "InterfaceTamperImm",        "Interface Tamper Imm",         await map.InterfaceImmTamperStatus.GetValue (),             mtu.InterfaceTamperImm );
                    AddParamCond ( "RegisterCoverTamper",       "Register Cover Tamper",        await map.RegisterCoverTamperStatus.GetValue (),            mtu.RegisterCoverTamper );
                    AddParamCond ( "RegisterCoverTamperImm",    "Register Cover Tamper Imm",    await map.RegisterCoverImmTamperStatus.GetValue (),         mtu.RegisterCoverTamperImm );
                    AddParamCond ( "ReverseFlow",               "Reverse Flow Tamper",          await map.ReverseFlowTamperStatus.GetValue (),              mtu.ReverseFlowTamper );
                    AddParamCond ( "FlowDirection",             "Flow Direction",               meter.Flow.ToString (),                                     mtu.ReverseFlowTamper );
                    AddParamCond ( "ReverseFlowTamperImm",      "Reverse Flow Tamper Imm",      await map.ReverseFlowImmTamperStatus.GetValue (),           mtu.ReverseFlowTamperImm );
                    AddParamCond ( "SerialCutWire",             "Serial Cut Wire",              await map.SerialCutWireTamperStatus.GetValue (),            mtu.SerialCutWire );
                    AddParamCond ( "SerialCutWire",             "Serial Cut Wire",              await map.SerialCutWireImmATamperStatus.GetValue (),        mtu.SerialCutWireImm );
                    AddParamCond ( "Cut1WireTamper",            "Cut Port1 Wire Tamper",        await map.P1CutWireTamperStatus.GetValue (),                mtu.TamperPort1 );
                    AddParamCond ( "Cut1WireTamperImm",         "Cut Port1 Wire Tamper Imm",    await map.P1CutWireImmTamperStatus.GetValue (),             mtu.TamperPort1Imm );
                    AddParamCond ( "Cut2WireTamper",            "Cut Port2 Wire Tamper",        await map.P2CutWireTamperStatus.GetValue (),                form.usePort2 && mtu.TamperPort2 );
                    AddParamCond ( "Cut2WireTamperImm",         "Cut Port2 Wire Tamper Imm",    await map.P2CutWireImmTamperStatus.GetValue (),             form.usePort2 && mtu.TamperPort2Imm );

                    this.addMtuAction.Add ( alarmSelection );
                }
            }

            #endregion

            // TODO (encoders)
            #region Demands

            if ( mtu.MtuDemand )
            {
                XElement demandConf = new XElement("DemandConfiguration");
                logger.AddAtrribute(demandConf, "display", "Demand Configuration");
                logger.AddParameter(demandConf, new Parameter("ConfigurationName", "Configuration Name", "Default")); // TODO: replace real value
                logger.AddParameter(demandConf, new Parameter("MtuNumLowPriorityMsg", "Mtu Num Low Priority Msg", "2")); // TODO: replace real value
                logger.AddParameter(demandConf, new Parameter("MtuPrimaryWindowInterval", "Mtu Primary WindowInterval", "180")); // TODO: replace real value
                logger.AddParameter(demandConf, new Parameter("MtuWindowAStart", "Mtu Window A Start", "0")); // TODO: replace real value
                logger.AddParameter(demandConf, new Parameter("MtuWindowBStart", "Mtu Window B Start", "0")); // TODO: replace real value
                logger.AddParameter(demandConf, new Parameter("MtuPrimaryWindowIntervalB", "Mtu Primary WindowInterval B", "3600")); // TODO: replace real value
                logger.AddParameter(demandConf, new Parameter("MtuPrimaryWindowOffset", "Mtu Primary Window Offset", "51")); // TODO: replace real value
                this.addMtuAction.Add(demandConf);
            }

            #endregion

            #region Misc/Optional

            if ( form.ContainsParameter ( FIELD.GPS_LATITUDE  ) &&
                 form.ContainsParameter ( FIELD.GPS_LONGITUDE ) &&
                 form.ContainsParameter ( FIELD.GPS_ALTITUDE  ) )
            {
                logger.AddParameter(this.addMtuAction, new Parameter("GPS_Y", "Lat", form.GPSLat.Value ));
                logger.AddParameter(this.addMtuAction, new Parameter("GPS_X", "Long", form.GPSLon.Value ));
                logger.AddParameter(this.addMtuAction, new Parameter("Altitude", "Elevation", form.GPSAlt.Value ));
            }

            if ( ! ( form.OptionalParams.Value is string ) )
            {
                List<Parameter> optionalParams = (List<Parameter>)form.OptionalParams.Value;

                if (optionalParams != null)
                    foreach (Parameter p in optionalParams)
                        logger.AddParameter(this.addMtuAction, p);
            }

            #endregion
        }

        public void LogReadMtu(ActionResult result)
        {
            logger.AddAtrribute(this.readMtuAction, "display", Action.logDisplays[ActionType.ReadMtu]);
            logger.AddAtrribute(this.readMtuAction, "type", Action.logTypes[ActionType.ReadMtu]);

            InterfaceParameters[] parameters = this.config.getLogParamsFromInterface( form.mtu, ActionType.ReadMtu );
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
            byte[] byteArray = Encoding.UTF8.GetBytes(logger.CreateBasicStructure());
            Stream BasicStruct = new MemoryStream(byteArray);
            XDocument uniDoc = XDocument.Load(BasicStruct);
            XElement uniMtus = uniDoc.Root.Element("Mtus");
            uniMtus.Add(this.addMtuAction);
#if DEBUG
            string uniUri = Path.Combine ( Mobile.LogUniPath,
                this.mtuBasicInfo.Type + "-" + this.action.Type + ( ( form.mtu.SpecialSet ) ? "-Encrypted" : "" ) + "-" + DateTime.Today.ToString ( "MM_dd_yyyy" ) + ".xml" );
            this.logger.CreateFileIfNotExist ( Logger.BasicFileType.READ, false, uniUri );

            uniDoc.Save ( uniUri );           
#endif
            
            // Write in ActivityLog
            if ( Data.Get.IsFromScripting &&
                 ! this.config.Global.ScriptOnly )
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
