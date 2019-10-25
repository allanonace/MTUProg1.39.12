﻿using System;
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
        
        public AddMtuLog(Logger logger, dynamic form, string user )
        {
            this.logger = logger;
            this.form = form;
            this.user = user;
            this.mtuBasicInfo = Data.Get.MtuBasicInfo;
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
            logger.AddAtrribute(this.turnOffAction, "display", Action.LogDisplays[ActionType.TurnOffMtu]);
            logger.AddAtrribute(this.turnOffAction, "type", Action.LogTypes[ActionType.TurnOffMtu]);

            logger.AddParameter(this.turnOffAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss") ));

            if (!string.IsNullOrEmpty(this.user))
                logger.AddParameter(this.turnOffAction, new Parameter("User", "User", this.user));

            logger.AddParameter(this.turnOffAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogTurnOn ()
        {
            logger.AddAtrribute(this.turnOnAction, "display", Action.LogDisplays[ActionType.TurnOnMtu]);
            logger.AddAtrribute(this.turnOnAction, "type", Action.LogTypes[ActionType.TurnOnMtu]);

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

            ActionType actionType = this.action.Type;

            bool isReplaceMeter = actionType == ActionType.ReplaceMeter           ||
                                  actionType == ActionType.ReplaceMtuReplaceMeter ||
                                  actionType == ActionType.AddMtuReplaceMeter;
            bool isReplaceMtu   = actionType == ActionType.ReplaceMTU ||
                                  actionType == ActionType.ReplaceMtuReplaceMeter;
            
            bool rddIn1;
            bool hasRDD = ( ( rddIn1 = mtu.Port1.IsSetFlow ) ||
                            mtu.TwoPorts && mtu.Port2.IsSetFlow );
            bool noRddOrNotIn1 = ! hasRDD || ! rddIn1;
            bool noRddOrNotIn2 = ! hasRDD ||   rddIn1;

            #region General

            logger.AddAtrribute(this.addMtuAction, "display", Action.LogDisplays[this.action.Type]);
            logger.AddAtrribute(this.addMtuAction, "type", Action.LogTypes[this.action.Type]);
            logger.AddAtrribute(this.addMtuAction, "reason", Action.LogReasons[this.action.Type]);

            logger.AddParameter ( this.addMtuAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if ( ! string.IsNullOrEmpty ( this.user ) )
                logger.AddParameter(this.addMtuAction, new Parameter("User", "User", this.user ) );

            // Not used with single port MTUs with RDD
            if ( isReplaceMtu &&
                 form.ContainsParameter ( FIELD.MTU_ID_OLD ) &&
                 noRddOrNotIn1 )
                logger.AddParameter ( this.addMtuAction, form.OldMtuId );

            logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuId",   "MTU ID",   this.mtuBasicInfo.Id   ) );
            logger.AddParameter ( this.addMtuAction, new Parameter ( "MtuType", "MTU Type", this.mtuBasicInfo.Type ) );

            // Not used with single port MTUs with RDD
            if ( noRddOrNotIn1 )
                logger.AddParameter ( this.addMtuAction, form.ReadInterval );

            // Not used with single port MTUs with RDD
            if ( form.ContainsParameter ( FIELD.SNAP_READS ) &&
                 noRddOrNotIn1 )
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

            #region Encryption

            // Avoid try to log encryption info when not it has not been performed
            if ( await map.Encrypted.GetValueFromMtu () )
            {
                //logger.Parameter ( this.addMtuAction, new Parameter ( "Encryption", "Encrypted", map.Encryption.GetValue () ) );
                logger.AddParameter ( this.addMtuAction, new Parameter ( "EncryptionIndex", "Encryption Index", await map.EncryptionIndex.GetValueFromMtu () ) );
            
                if ( ! mtu.IsFamily35xx36xx )
                {
                    Mobile.ConfigData data = Mobile.configData;

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

            // Not used with single port MTUs with RDD
            if ( isReplaceMeter &&
                 noRddOrNotIn1 )
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

            if ( hasRDD &&
                 rddIn1 )
            {
                logger.AddParameter ( port, new Parameter ( "RDDSerialNumber", "RDD SerialNumber", await map.RDDSerialNumber.GetValue () ) );
                logger.AddParameter ( port, new Parameter ( "RDDBatteryStatus", "RDD Battery Status", Data.Get.RDDBattery ) );
                logger.AddParameter ( port, new Parameter ( "RDDCurrentValvePosition", "RDD Current Valve Position", Data.Get.PrevRDDValvePosition ) );
                logger.AddParameter ( port, new Parameter ( "RDDDesiredValvePosition", "RDD Desired Valve Position", Data.Get.RDDPosition ) );
            }

            string meterType = string.Format("({0}) {1}", meter.Id, meter.Display);
            logger.AddParameter ( port, new Parameter("MeterType", "Meter Type", meterType));
            logger.AddParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter.Id.ToString()));
            logger.AddParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter.Vendor));
            logger.AddParameter ( port, new Parameter("MeterModel", "Meter Model", meter.Model));
            
            // Not used with single port MTUs with RDD
            if ( global.UseMeterSerialNumber &&
                 noRddOrNotIn1 )
                logger.AddParameter ( port, form.MeterNumber );
            
            // Not used with single port MTUs with RDD
            if ( ! mtu.Port1.IsForEncoderOrEcoder &&
                 noRddOrNotIn1 )
                logger.AddParameter ( port, form.MeterReading );
            
            logger.AddParameter ( port, new Parameter ( "PulseHi","Pulse Hi Time",  meter.PulseHiTime .ToString ().PadLeft ( 2, '0' ) ) );
            logger.AddParameter ( port, new Parameter ( "PulseLo","Pulse Low Time", meter.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

            this.addMtuAction.Add ( port );

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

                // Not used with RDD
                if ( isReplaceMeter &&
                     noRddOrNotIn2 )
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
                
                if ( hasRDD &&
                     ! rddIn1 )
                {
                    logger.AddParameter ( port, new Parameter ( "RDDSerialNumber", "RDD SerialNumber", await map.RDDSerialNumber.GetValue () ) );
                    logger.AddParameter ( port, new Parameter ( "RDDBatteryStatus", "RDD Battery Status", Data.Get.RDDBattery ) );
                    logger.AddParameter ( port, new Parameter ( "RDDCurrentValvePosition", "RDD Current Valve Position", Data.Get.PrevRDDValvePosition ) );
                    logger.AddParameter ( port, new Parameter ( "RDDDesiredValvePosition", "RDD Desired Valve Position", Data.Get.RDDPosition ) );
                }

                string meterType2 = string.Format("({0}) {1}", meter2.Id, meter2.Display);
                logger.AddParameter ( port, new Parameter("MeterType", "Meter Type", meterType2));
                logger.AddParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter2.Id.ToString()));
                logger.AddParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter2.Vendor));
                logger.AddParameter ( port, new Parameter("MeterModel", "Meter Model", meter2.Model));
                
                // Not used with RDD
                if ( global.UseMeterSerialNumber &&
                     noRddOrNotIn2 )
                    logger.AddParameter ( port, form.MeterNumber_2 );
                
                // Not used with RDD
                if ( ! mtu.Port2.IsForEncoderOrEcoder &&
                     noRddOrNotIn2 )
                    logger.AddParameter ( port, form.MeterReading_2 );

                logger.AddParameter ( port, new Parameter ( "PulseHi","Pulse Hi Time",  meter2.PulseHiTime .ToString ().PadLeft ( 2, '0' ) ) );
                logger.AddParameter ( port, new Parameter ( "PulseLo","Pulse Low Time", meter2.PulseLowTime.ToString ().PadLeft ( 2, '0' ) ) );

                this.addMtuAction.Add ( port );
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


                    // Log the value if the condition is validated
                    dynamic AddParamCond = new Action<string,string,string,bool> (
                        ( tag, display, value, condition ) => {

                            if ( condition )
                                logger.AddParameter ( alarmSelection,
                                    new Parameter ( tag, display, value ) );
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
                    await AddParamCondMap ( "UrgentAlarm",               "DCU Urgent Alarm Transmit",    alarms.DcuUrgentAlarm.ToString (),      map.ContainsMember ( "UrgentAlarm" ) );
                    await AddParamCondMap ( "MemoryMapError",            "Memory Map Error",             "MemoryMapTamperStatus",                mtu.MemoryMapError );
                    await AddParamCondMap ( "MemoryMapErrorImm",         "Memory Map Error Imm",         "MemoryMapImmTamperStatus",             mtu.MemoryMapErrorImm );
                    await AddParamCondMap ( "ProgramMemoryError",        "Program Memory Error",         "ProgramMemoryTamperStatus",            mtu.ProgramMemoryError );
                    await AddParamCondMap ( "ProgramMemoryErrorImm",     "Program Memory Error Imm",     "ProgramMemoryImmTamperStatus",         mtu.ProgramMemoryErrorImm );
                    await AddParamCondMap ( "MoistureDetect",            "Moisture Detect",              "MoistureTamperStatus",                 mtu.MoistureDetect );
                    await AddParamCondMap ( "MoistureDetectImm",         "Moisture Detect Imm",          "MoistureImmTamperStatus",              mtu.MoistureDetectImm );
                    await AddParamCondMap ( "EnergizerLastGasp",         "Energizer Last Gasp",          "EnergizerLastGaspTamperStatus",        mtu.EnergizerLastGasp );
                    await AddParamCondMap ( "EnergizerLastGaspImm",      "Energizer Last Gasp Imm",      "EnergizerLastGaspImmTamperStatus",     mtu.EnergizerLastGaspImm );
                    await AddParamCondMap ( "InsufficentMemory",         "Insufficent Memory",           "InsufficientMemoryTamperStatus",       mtu.InsufficientMemory );
                    await AddParamCondMap ( "InsufficentMemoryImm",      "Insufficent Memory Imm",       "InsufficientMemoryImmTamperStatus",    mtu.InsufficientMemoryImm );
                    await AddParamCondMap ( "CutAlarmCable",             "Cut Alarm Cable",              "GasCutWireTamperStatus",               mtu.GasCutWireAlarm );
                    await AddParamCondMap ( "Cut2AlarmCable",            "Cut Port2 Alarm Cable",        "P2GasCutWireTamperStatus",             form.usePort2 && mtu.GasCutWireAlarm );
                    await AddParamCondMap ( "SerialComProblem",          "Serial Com Problem",           "SerialComProblemTamperStatus",         mtu.SerialComProblem );
                    await AddParamCondMap ( "SerialComProblemImm",       "Serial Com Problem Imm",       "SerialComProblemImmTamperStatus",      mtu.SerialComProblemImm );
                    await AddParamCondMap ( "LastGasp",                  "Last Gasp",                    "LastGaspTamperStatus",                 mtu.LastGasp );
                    await AddParamCondMap ( "LastGaspImm",               "Last Gasp Imm",                "LastGaspImmTamperStatus",              mtu.LastGaspImm );
                    await AddParamCondMap ( "TiltTamper",                "Tilt Tamper",                  "TiltTamperStatus",                     mtu.TiltTamper );
                    await AddParamCondMap ( "TiltTamperImm",             "Tilt Tamper Imm",              "TiltImmTamperStatus",                  mtu.TiltTamperImm );
                    await AddParamCondMap ( "MagneticTamper",            "Magnetic Tamper",              "MagneticTamperStatus",                 mtu.MagneticTamper );
                    await AddParamCondMap ( "MagneticTamperImm",         "Magnetic Tamper Imm",          "MagneticImmTamperStatus",              mtu.MagneticTamperImm );
                    await AddParamCondMap ( "InterfaceTamper",           "Interface Tamper",             "InterfaceTamperStatus",                mtu.InterfaceTamper );
                    await AddParamCondMap ( "InterfaceTamperImm",        "Interface Tamper Imm",         "InterfaceImmTamperStatus",             mtu.InterfaceTamperImm );
                    await AddParamCondMap ( "RegisterCoverTamper",       "Register Cover Tamper",        "RegisterCoverTamperStatus",            mtu.RegisterCoverTamper );
                    await AddParamCondMap ( "RegisterCoverTamperImm",    "Register Cover Tamper Imm",    "RegisterCoverImmTamperStatus",         mtu.RegisterCoverTamperImm );
                    await AddParamCondMap ( "ReverseFlow",               "Reverse Flow Tamper",          "ReverseFlowTamperStatus",              mtu.ReverseFlowTamper );
                    AddParamCond          ( "FlowDirection",             "Flow Direction",               meter.Flow.ToString (),                 mtu.ReverseFlowTamper );
                    await AddParamCondMap ( "ReverseFlowTamperImm",      "Reverse Flow Tamper Imm",      "ReverseFlowImmTamperStatus",           mtu.ReverseFlowTamperImm );
                    await AddParamCondMap ( "SerialCutWire",             "Serial Cut Wire",              "SerialCutWireTamperStatus",            mtu.SerialCutWire );
                    await AddParamCondMap ( "SerialCutWire",             "Serial Cut Wire",              "SerialCutWireImmATamperStatus",        mtu.SerialCutWireImm );
                    await AddParamCondMap ( "Cut1WireTamper",            "Cut Port1 Wire Tamper",        "P1CutWireTamperStatus",                mtu.TamperPort1 );
                    await AddParamCondMap ( "Cut1WireTamperImm",         "Cut Port1 Wire Tamper Imm",    "P1CutWireImmTamperStatus",             mtu.TamperPort1Imm );
                    await AddParamCondMap ( "Cut2WireTamper",            "Cut Port2 Wire Tamper",        "P2CutWireTamperStatus",                form.usePort2 && mtu.TamperPort2 );
                    await AddParamCondMap ( "Cut2WireTamperImm",         "Cut Port2 Wire Tamper Imm",    "P2CutWireImmTamperStatus",             form.usePort2 && mtu.TamperPort2Imm );

                    this.addMtuAction.Add ( alarmSelection );
                }
            }

            #endregion

            #region Demands

            if ( mtu.MtuDemand &&
                 mtu.FastMessageConfig )
            {
                XElement demandConf = new XElement ( "DemandConfiguration" );
                logger.AddAtrribute ( demandConf, "display", "Demand Configuration" );
                logger.AddParameter ( demandConf, new Parameter ( "ConfigurationName",         "Configuration Name",           Data.Get.DemandConf.Name ) );
                logger.AddParameter ( demandConf, new Parameter ( "MtuNumLowPriorityMsg",      "Mtu Num Low Priority Msg",     await map.MtuNumLowPriorityMsg     .GetValue () ) );
                logger.AddParameter ( demandConf, new Parameter ( "MtuPrimaryWindowInterval",  "Mtu Primary WindowInterval",   await map.MtuPrimaryWindowInterval .GetValue () ) );
                logger.AddParameter ( demandConf, new Parameter ( "MtuWindowAStart",           "Mtu Window A Start",           await map.MtuWindowAStart          .GetValue () ) );
                logger.AddParameter ( demandConf, new Parameter ( "MtuWindowBStart",           "Mtu Window B Start",           await map.MtuWindowBStart          .GetValue () ) );
                logger.AddParameter ( demandConf, new Parameter ( "MtuPrimaryWindowIntervalB", "Mtu Primary WindowInterval B", await map.MtuPrimaryWindowIntervalB.GetValue () ) );
                logger.AddParameter ( demandConf, new Parameter ( "MtuPrimaryWindowOffset",    "Mtu Primary Window Offset",    await map.MtuPrimaryWindowOffset   .GetValue () ) );
                this.addMtuAction.Add ( demandConf );
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
            logger.AddAtrribute(this.readMtuAction, "display", Action.LogDisplays[ActionType.ReadMtu]);
            logger.AddAtrribute(this.readMtuAction, "type", Action.LogTypes[ActionType.ReadMtu]);

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
                catch ( Exception )
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
