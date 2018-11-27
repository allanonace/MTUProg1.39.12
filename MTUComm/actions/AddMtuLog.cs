using MTUComm.actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xml;

namespace MTUComm
{
    class AddMtuLog
    {
        private Logger logger;
        private string user;
        private dynamic form;
        private MTUBasicInfo mtuBasicInfo;
        private string logUri;

        private const string addMtuDisplay = "Add MTU";
        private const string addMtuType = "Program MTU";
        private const string addMtuReason = "AddMtu";

        private const string turnOffDisplay = "Turn Off MTU";
        private const string turnOffType = "TurnOffType";

        private const string turnOnDisplay = "Turn On MTU";
        private const string turnOnType = "TurnOnType";

        private const string readMtuDisplay = "Read MTU";
        private const string readMtuType = "ReadMTU";

        private XElement addMtuAction;
        private XElement turnOffAction;
        private XElement turnOnAction;
        private XElement readMtuAction;

        public AddMtuLog(Logger logger, dynamic form, string user)
        {
            this.logger = logger;
            this.form = form;
            this.user = user;
            this.mtuBasicInfo = MtuForm.mtuBasicInfo;
            this.logUri = this.logger.CreateFileIfNotExist();

            this.addMtuAction = new XElement("Action");
            this.turnOffAction = new XElement("Action");
            this.turnOnAction = new XElement("Action");
            this.readMtuAction = new XElement("Action");
        }

        public void LogTurnOff()
        {
            logger.addAtrribute(this.turnOffAction, "display", turnOffDisplay);
            logger.addAtrribute(this.turnOffAction, "type", turnOffType);

            logger.logParameter(this.turnOffAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
            {
                logger.logParameter(this.turnOffAction, new Parameter("User", "User", this.user));
            }

            logger.logParameter(this.turnOffAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogAction()
        {
            Mtu mtu = form.mtu;
            dynamic MtuConditions = form.conditions.mtu;
            dynamic GlobalsConditions = form.conditions.globals;
            Meter meter = (Meter)form.Meter.getValue();

            // TODO
            #region General
            logger.addAtrribute(this.addMtuAction, "display", addMtuDisplay);
            logger.addAtrribute(this.addMtuAction, "type", addMtuType);
            logger.addAtrribute(this.addMtuAction, "reason", addMtuReason);

            logger.logParameter(this.addMtuAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
            {
                logger.logParameter(this.addMtuAction, new Parameter("User", "User", this.user));
            }

            logger.logParameter(this.addMtuAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
            logger.logParameter(this.addMtuAction, new Parameter("MtuType", "MTU Type", this.mtuBasicInfo.Type));

            if (GlobalsConditions.IndividualReadInterval)
            {
                string readInterval = form.ReadInterval.getValue();
                logger.logParameter(this.addMtuAction, new Parameter("ReadInterval", "Read Interval", readInterval));
            }

            if (MtuConditions.FastMessageConfig)
            {
                string fastTwoWay = form.TwoWay.getValue();
                logger.logParameter(this.addMtuAction, new Parameter("Fast-2-Way", "Fast Message Config", fastTwoWay));
            }

            if (MtuConditions.DailyReads)
            {
                string dailyReads = "Disable";
                string dailyGmtHourRead = "Disable";

                if (GlobalsConditions.IndividualDailyReads) // TODO: check values
                {
                    dailyReads = form.SnapReads.getValue();
                    dailyGmtHourRead = form.SnapReads.getValue();

                }
                logger.logParameter(this.addMtuAction, new Parameter("DailyGMTHourRead", "GMT Daily Reads", dailyGmtHourRead));
                logger.logParameter(this.addMtuAction, new Parameter("DailyReads", "Daily Reads", dailyReads));
            }

            string afc = "Off";
            if (Configuration.GetInstance().global.AFC)
            {
                afc = "Set";
            }
            logger.logParameter(this.addMtuAction, new Parameter("AFC", "AFC", afc));

            #endregion

            #region Port 1
            XElement port = new XElement("Port");
            logger.addAtrribute(port, "display", "Port 1");
            logger.addAtrribute(port, "number", "1");

            string servicePtId = form.ServicePortId.getValue().ToString();
            logger.logParameter(port, new Parameter("AccountNumber", "Service Pt. ID", servicePtId));

            if (GlobalsConditions.WorkOrderRecording)
            {
                string fieldOrder = form.FieldOrder.getValue().ToString();
                logger.logParameter(port, new Parameter("WorkOrder", "Field Order", fieldOrder));
            }

            string meterType = string.Format("({0}) {1}", meter.Id, meter.Display);
            logger.logParameter(port, new Parameter("MeterType", "Meter Type", meterType));

            string meterTypeId = meter.Id.ToString();
            logger.logParameter(port, new Parameter("MeterTypeId", "Meter Type ID", meterTypeId));

            string meterVendor = meter.Vendor;
            logger.logParameter(port, new Parameter("MeterVendor", "Meter Vendor", meterVendor));

            string meterModel = meter.Model;
            logger.logParameter(port, new Parameter("MeterModel", "Meter Model", meterModel));

            string initialReading = form.InitialReading.getValue().ToString();
            logger.logParameter(port, new Parameter("NewMeterSerialNumber", "New Meter Serial Number", initialReading));

            this.addMtuAction.Add(port);
            #endregion

            #region Port 2
            if (MtuConditions.TwoPorts)
            {
                Meter meter2 = (Meter)form.Meter2.getValue();
                port = new XElement("Port");
                logger.addAtrribute(port, "display", "Port 2");
                logger.addAtrribute(port, "number", "2");
                string servicePtId2 = form.ServicePortId2.getValue().ToString();
                logger.logParameter(port, new Parameter("AccountNumber", "Service Pt. ID", servicePtId2));

                if (GlobalsConditions.WorkOrderRecording)
                {
                    string fieldOrder2 = form.FieldOrder2.getValue().ToString();
                    logger.logParameter(port, new Parameter("WorkOrder", "Field Order", fieldOrder2));
                }

                string meterType2 = string.Format("({0}) {1}", meter2.Id, meter2.Display);
                logger.logParameter(port, new Parameter("MeterType", "Meter Type", meterType2));

                string meterTypeId2 = meter2.Id.ToString();
                logger.logParameter(port, new Parameter("MeterTypeId", "Meter Type ID", meterTypeId2));

                string meterVendor2 = meter2.Vendor;
                logger.logParameter(port, new Parameter("MeterVendor", "Meter Vendor", meterVendor2));

                string meterModel2 = meter2.Model;
                logger.logParameter(port, new Parameter("MeterModel", "Meter Model", meterModel2));

                string initialReading2 = form.InitialReading2.getValue().ToString();
                logger.logParameter(port, new Parameter("NewMeterSerialNumber", "New Meter Serial Number", initialReading2));

                this.addMtuAction.Add(port);
            }
            #endregion

            #region Alarms
            if (MtuConditions.RequiresAlarmProfile)
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

                if (mtu.MagneticTamper)
                {
                    string magneticTamper = "Disabled";
                    if (alarms.Magnetic)
                    {
                        magneticTamper = "Enabled";
                    }
                    logger.logParameter(alarmSelection, new Parameter("MagneticTamper", "Magnetic Tamper", magneticTamper));
                }

                if (mtu.RegisterCoverTamper)
                {
                    string registerCoverTamper = "Disabled";
                    if (alarms.RegisterCover)
                    {
                        registerCoverTamper = "Enabled";
                    }
                    logger.logParameter(alarmSelection, new Parameter("RegisterCoverTamper", "Register Cover Tamper", registerCoverTamper));
                }

                if (mtu.TiltTamper)
                {
                    string tiltTamper = "Disabled";
                    if (alarms.Tilt)
                    {
                        tiltTamper = "Enabled";
                    }
                    logger.logParameter(alarmSelection, new Parameter("TiltTamper", "Tilt Tamper", tiltTamper));
                }

                if (mtu.ReverseFlowTamper)
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

                if (mtu.InterfaceTamper)
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
            List<Parameter> optionalParams = (List<Parameter>)form.OptionalParams.getValue();
            foreach (Parameter p in optionalParams)
            {
                logger.logParameter(this.addMtuAction, p);
            }
            #endregion
        }

        public void LogTurnOn()
        {
            logger.addAtrribute(this.turnOnAction, "display", turnOnDisplay);
            logger.addAtrribute(this.turnOnAction, "type", turnOnType);

            logger.logParameter(this.turnOnAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
            {
                logger.logParameter(this.turnOnAction, new Parameter("User", "User", this.user));
            }

            logger.logParameter(this.turnOnAction, new Parameter("MtuId", "MTU ID", this.mtuBasicInfo.Id));
        }

        public void LogReadMtu()
        {
            logger.addAtrribute(this.readMtuAction, "display", readMtuDisplay);
            logger.addAtrribute(this.readMtuAction, "type", readMtuType);

            logger.logParameter(this.readMtuAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (!string.IsNullOrEmpty(this.user))
            {
                logger.logParameter(this.readMtuAction, new Parameter("User", "User", this.user));
            }
        }

        public void Save()
        {
            this.addMtuAction.Add(this.turnOffAction);
            this.addMtuAction.Add(this.turnOnAction);
            this.addMtuAction.Add(this.readMtuAction);

            XDocument doc = XDocument.Load(logUri);
            XElement mtus = doc.Root.Element("Mtus");
            mtus.Add(this.addMtuAction);

            doc.Save(logUri);
        }
    }
}
