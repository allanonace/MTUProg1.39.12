﻿using MTUComm.actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xml;

using FIELD = MTUComm.actions.AddMtuForm.FIELD;

namespace MTUComm
{
    public class AddMtuLog
    {
        public  Logger logger;
        private string user;
        private dynamic form;
        private MTUBasicInfo mtuBasicInfo;
        private string logUri;
        private Mtu mtu;

        private const string addMtuDisplay = "Add MTU";
        private const string addMtuType = "Program MTU";
        private const string addMtuReason = "AddMtu";

        private const string turnOffDisplay = "Turn Off MTU";
        private const string turnOffType = "TurnOffType";

        private const string turnOnDisplay = "Turn On MTU";
        private const string turnOnType = "TurnOnType";

        private const string readMtuDisplay = "Read MTU";
        private const string readMtuType = "ReadMTU";

        private XDocument doc;
        private XElement  addMtuAction;
        private XElement  turnOffAction;
        private XElement  turnOnAction;
        private XElement  readMtuAction;

        public AddMtuLog(Logger logger, dynamic form, string user, bool isFromScripting )
        {
            this.logger = logger;
            this.form = form;
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
            logger.addAtrribute(this.turnOffAction, "display", turnOffDisplay);
            logger.addAtrribute(this.turnOffAction, "type", turnOffType);

            logger.logParameter(this.turnOffAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
                logger.logParameter(this.turnOffAction, new Parameter("User", "User", this.user));

            logger.logParameter(this.turnOffAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogTurnOn ()
        {
            logger.addAtrribute(this.turnOnAction, "display", turnOnDisplay);
            logger.addAtrribute(this.turnOnAction, "type", turnOnType);

            logger.logParameter(this.turnOnAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
                logger.logParameter(this.turnOnAction, new Parameter("User", "User", this.user));

            logger.logParameter(this.turnOnAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogAddMtu ( bool isFromScripting = false )
        {
            this.mtu = form.mtu;
            dynamic MtuConditions     = form.conditions.mtu;
            dynamic GlobalsConditions = form.conditions.globals;

            Meter meter = ( ! isFromScripting ) ?
                ( Meter )form.Meter.getValue() :
                Configuration.GetInstance().getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter.Value ) );

            #region General

            logger.addAtrribute ( this.addMtuAction, "display", addMtuDisplay );
            logger.addAtrribute ( this.addMtuAction, "type",    addMtuType    );
            logger.addAtrribute ( this.addMtuAction, "reason",  addMtuReason  );

            logger.logParameter ( this.addMtuAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if ( ! string.IsNullOrEmpty ( this.user ) )
                logger.logParameter(this.addMtuAction, new Parameter("User", "User", this.user ) );

            logger.logParameter ( this.addMtuAction, new Parameter ( "MtuId",   "MTU ID",   this.mtuBasicInfo.Id   ) );
            logger.logParameter ( this.addMtuAction, new Parameter ( "MtuType", "MTU Type", this.mtuBasicInfo.Type ) );

            if ( GlobalsConditions.IndividualReadInterval )
                logger.logParameter ( this.addMtuAction, form.ReadInterval );

            if ( MtuConditions.FastMessageConfig )
                logger.logParameter ( this.addMtuAction, form.TwoWay );

            if ( MtuConditions.DailyReads )
            {
                string dailyReads       = "Disable";
                string dailyGmtHourRead = "Disable";

                if (GlobalsConditions.IndividualDailyReads) // TODO: check values
                {
                    dailyReads       = form.SnapReads.getValue ();
                    dailyGmtHourRead = form.SnapReads.getValue ();
                }
                logger.logParameter(this.addMtuAction, new Parameter("DailyGMTHourRead", "GMT Daily Reads", dailyGmtHourRead));
                logger.logParameter(this.addMtuAction, new Parameter("DailyReads", "Daily Reads", dailyReads));
            }

            string afc = (Configuration.GetInstance().global.AFC) ? "Set" : "Off";
            logger.logParameter(this.addMtuAction, new Parameter("AFC", "AFC", afc));

            #endregion

            #region Port 1

            XElement port = new XElement("Port");
            logger.addAtrribute(port, "display", "Port 1");
            logger.addAtrribute(port, "number", "1");

            logger.logParameter ( port, form.ServicePortId );

            if ( GlobalsConditions.WorkOrderRecording )
                logger.logParameter ( port, form.FieldOrder );

            string meterType = string.Format("({0}) {1}", meter.Id, meter.Display);
            logger.logParameter ( port, new Parameter("MeterType", "Meter Type", meterType));
            logger.logParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter.Id.ToString()));
            logger.logParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter.Vendor));
            logger.logParameter ( port, new Parameter("MeterModel", "Meter Model", meter.Model));
            logger.logParameter ( port, form.MeterNumber );
            logger.logParameter ( port, form.InitialReading );

            this.addMtuAction.Add(port);

            #endregion

            #region Port 2

            if (MtuConditions.TwoPorts)
            {
                Meter meter2 = ( ! isFromScripting ) ?
                    ( Meter )form.Meter2.getValue() :
                    Configuration.GetInstance().getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter2.Value ) );

                port = new XElement ( "Port");
                logger.addAtrribute ( port, "display", "Port 2" );
                logger.addAtrribute ( port, "number", "2" );

                logger.logParameter ( port, form.ServicePortId2 );

                if ( GlobalsConditions.WorkOrderRecording )
                    logger.logParameter ( port, form.FieldOrder2 );

                string meterType2 = string.Format("({0}) {1}", meter2.Id, meter2.Display);
                logger.logParameter ( port, new Parameter("MeterType", "Meter Type", meterType2));
                logger.logParameter ( port, new Parameter("MeterTypeId", "Meter Type ID", meter2.Id.ToString()));
                logger.logParameter ( port, new Parameter("MeterVendor", "Meter Vendor", meter2.Vendor));
                logger.logParameter ( port, new Parameter("MeterModel", "Meter Model", meter2.Model));
                logger.logParameter ( port, form.MeterNumber2 );
                logger.logParameter ( port, form.InitialReading2 );

                this.addMtuAction.Add(port);
            }

            #endregion

            #region Alarms

            if ( ! isFromScripting &&
                 MtuConditions.RequiresAlarmProfile)
            {
                Alarm alarms = (Alarm)form.Alarm.getValue();
                XElement alarmSelection = new XElement("AlarmSelection");
                logger.addAtrribute(alarmSelection, "display", "Alarm Selection");

                string alarmConfiguration = alarms.Name;
                logger.logParameter(alarmSelection, new Parameter("AlarmConfiguration", "Alarm Configuration Name", alarmConfiguration));

                string immediateAlarmTransmit = "False";
                if (alarms.ImmediateAlarmTransmit)
                {
                    immediateAlarmTransmit = "True";
                }
                logger.logParameter(alarmSelection, new Parameter("ImmediateAlarm", "Immediate Alarm Transmit", immediateAlarmTransmit));

                string urgentAlarm = "False";
                if (alarms.DcuUrgentAlarm)
                {
                    urgentAlarm = "True";
                }
                logger.logParameter(alarmSelection, new Parameter("UrgentAlarm", "DCU Urgent Alarm Transmit", urgentAlarm));

                string overlap = alarms.Overlap.ToString();
                logger.logParameter(alarmSelection, new Parameter("Overlap", "Message Overlap", overlap));

                if (this.mtu.MagneticTamper)
                {
                    string magneticTamper = "Disabled";
                    if (alarms.Magnetic)
                    {
                        magneticTamper = "Enabled";
                    }
                    logger.logParameter(alarmSelection, new Parameter("MagneticTamper", "Magnetic Tamper", magneticTamper));
                }

                if (this.mtu.RegisterCoverTamper)
                {
                    string registerCoverTamper = "Disabled";
                    if (alarms.RegisterCover)
                    {
                        registerCoverTamper = "Enabled";
                    }
                    logger.logParameter(alarmSelection, new Parameter("RegisterCoverTamper", "Register Cover Tamper", registerCoverTamper));
                }

                if (this.mtu.TiltTamper)
                {
                    string tiltTamper = "Disabled";
                    if (alarms.Tilt)
                    {
                        tiltTamper = "Enabled";
                    }
                    logger.logParameter(alarmSelection, new Parameter("TiltTamper", "Tilt Tamper", tiltTamper));
                }

                if (this.mtu.ReverseFlowTamper)
                {
                    string reverseFlow = "Disabled";
                    if (alarms.ReverseFlow)
                    {
                        reverseFlow = "Enabled";
                    }
                    logger.logParameter(alarmSelection, new Parameter("ReverseFlow", "Reverse Flow Tamper", reverseFlow));
                    string flowDirection = meter.Flow.ToString();
                    logger.logParameter(alarmSelection, new Parameter("FlowDirection", "Flow Direction", flowDirection));
                }

                if (this.mtu.InterfaceTamper)
                {
                    // Add interfaace tamper log to AlarmSelection node
                    string interfaceTamper = "Disabled";
                    if (alarms.InterfaceTamper)
                    {
                        interfaceTamper = "Enabled";
                    }
                    logger.logParameter(alarmSelection, new Parameter("InterfaceTamper", "Interface Tamper", interfaceTamper));

                    // Add interface tamper log to Action node
                    logger.logParameter(this.addMtuAction, new Parameter("InterfaceTamper", "Interface Tamper", interfaceTamper));

                }

                this.addMtuAction.Add(alarmSelection);
            }

            #endregion

            // TODO (encoders)
            #region Demands

            if (MtuConditions.MtuDemand)
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

                logger.logParameter(this.addMtuAction, new Parameter("GPS_Y", "Lat", form.GPSLat.getValue ()));
                logger.logParameter(this.addMtuAction, new Parameter("GPS_X", "Long", form.GPSLon.getValue ()));
                logger.logParameter(this.addMtuAction, new Parameter("Altitude", "Elevation", form.GPSAlt.getValue ()));
            }

            if ( ! ( form.OptionalParams.getValue() is string ) )
            {
                List<Parameter> optionalParams = (List<Parameter>)form.OptionalParams.getValue();

                if (optionalParams != null)
                    foreach (Parameter p in optionalParams)
                        logger.logParameter(this.addMtuAction, p);
            }

            #endregion
        }

        public void LogReadMtu(ActionResult result)
        {
            logger.addAtrribute(this.readMtuAction, "display", readMtuDisplay);
            logger.addAtrribute(this.readMtuAction, "type", readMtuType);

            /*logger.logParameter(this.readMtuAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
            {
                logger.logParameter(this.readMtuAction, new Parameter("User", "User", this.user));
            }*/

            InterfaceParameters[] parameters = Configuration.GetInstance().getLogInterfaceFields(this.mtu.Id, "ReadMTU");
            foreach (InterfaceParameters parameter in parameters)
            {
                if (parameter.Name == "Port")
                {
                    ActionResult[] ports = result.getPorts();
                    for (int i = 0; i < ports.Length; i++)
                    {
                        logger.logPort(i, this.readMtuAction, ports[i], parameter.Parameters.ToArray());
                    }
                }
                else
                {
                    logger.logComplexParameter(this.readMtuAction, result, parameter);
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
        }

        public override string ToString ()
        {
            if ( this.doc == null )
                this.Save ();

            return doc.ToString ();         
        }
    }
}
