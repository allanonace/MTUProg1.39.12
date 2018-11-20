using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MTUComm
{
    class AddMtu
    {
        private Configuration config;
        private MTUComm comm;
        private Logger logger;
        private string user;
        Parameter[] addMtuParams;
        uint mtuId;

        public AddMtu(Configuration config, MTUComm comm, Logger logger, string user, Parameter[] addMtuParams)
        {
            this.config = config;
            this.comm = comm;
            this.logger = logger;
            this.user = user;
            this.addMtuParams = addMtuParams;
            this.mtuId = 1; // TODO: add real
        }

        public void Run()
        {
            Task.Factory.StartNew(() =>
            {
                TurnOffAction();
                AddMtuAction();
                TurnOnAction();
                ReadMtuAction();
                Log();
            });
        }

        public void Log()
        {
            string logfile = logger.CreateFileIfNotExist();
            XDocument doc = XDocument.Load(logfile);

            AddMtuLog(doc);

            doc.Save(logfile);
        }

        // turn off action
        private void TurnOffAction()
        {
            //comm.NewTurnOffMtu();
        }

        // add mtu action
        private void AddMtuAction()
        {
            //comm.NewAddMtu(this.addMtuParams);
        }

        // turn on action
        private void TurnOnAction()
        {
            //comm.NewTurnOnMtu();
        }

        // read mtu action
        private void ReadMtuAction()
        {

        }
        
        // add mtu log
        private void AddMtuLog(XDocument doc)
        {
            XElement addMtuAction = new XElement("Action");

            logger.addAtrribute(addMtuAction, "display", "Add MTU");
            logger.addAtrribute(addMtuAction, "type", "Program MTU");

            logger.logParameter(addMtuAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (this.user != null)
            {
                logger.logParameter(addMtuAction, new Parameter("User", "User", this.user));
            }

            logger.logParameter(addMtuAction, new Parameter("MtuId", "MTU ID", "063004810")); // TODO: replace real value
            logger.logParameter(addMtuAction, new Parameter("MtuType", "MTU Type", "171")); // TODO: replace real value

            XElement port = new XElement("Port");
            logger.addAtrribute(port, "display", "Port 1");
            logger.addAtrribute(port, "number", "1");
            logger.logParameter(port, new Parameter("AccountNumber", "Service Pt. ID", "1234567890")); // TODO: replace real value
            logger.logParameter(port, new Parameter("WorkOrder", "Field Order", "12345678901234567890")); // TODO: replace real value
            logger.logParameter(port, new Parameter("NewMeterSerialNumber", "New Meter Serial Number", "123456789012")); // TODO: replace real value
            logger.logParameter(port, new Parameter("MeterType", "Meter Type", "(1112) NEPT T10 3/4 E-Coder 0.01CuFt")); // TODO: replace real value
            logger.logParameter(port, new Parameter("MeterTypeId", "Meter Type ID", "1112")); // TODO: replace real value
            logger.logParameter(port, new Parameter("MeterVendor", "Meter Vendor", "SCHLUM/NEPTUNE")); // TODO: replace real value
            logger.logParameter(port, new Parameter("MeterModel", "Meter Model", "T-10")); // TODO: replace real value
            addMtuAction.Add(port);

            logger.logParameter(addMtuAction, new Parameter("ReadInterval", "Read Interval", "15 Min")); // TODO: replace real value
            logger.logParameter(addMtuAction, new Parameter("Fast-2-Way", "Fast Message Config", "Fast")); // TODO: replace real value
            logger.logParameter(addMtuAction, new Parameter("DailyGMTHourRead", "GMT Daily Reads", "Disable")); // TODO: replace real value
            logger.logParameter(addMtuAction, new Parameter("DailyReads", "Daily Reads", "Disable")); // TODO: replace real value

            XElement alarmSelection = new XElement("AlarmSelection");
            logger.addAtrribute(alarmSelection, "display", "Alarm Selection");
            logger.logParameter(alarmSelection, new Parameter("AlarmConfiguration", "Alarm Configuration Name", "All")); // TODO: replace real value
            logger.logParameter(alarmSelection, new Parameter("Overlap", "Message Overlap", "6")); // TODO: replace real value
            logger.logParameter(alarmSelection, new Parameter("LastGaspImm", "Last Gasp Imm", "Enabled")); // TODO: replace real value
            logger.logParameter(alarmSelection, new Parameter("InterfaceTamperImm", "SerialComProblem Imm", "Enabled")); // TODO: replace real value
            logger.logParameter(alarmSelection, new Parameter("InsufficentMemoryImm", "Insufficent Memory Imm", "Enabled")); // TODO: replace real value
            logger.logParameter(alarmSelection, new Parameter("LastGasp", "Last Gasp", "Enabled")); // TODO: replace real value
            logger.logParameter(alarmSelection, new Parameter("InsufficentMemory", "Insufficent Memory", "Enabled")); // TODO: replace real value
            logger.logParameter(alarmSelection, new Parameter("InterfaceTamper", "Interface Tamper", "Enabled")); // TODO: replace real value
            addMtuAction.Add(alarmSelection);

            XElement demandConf = new XElement("DemandConfiguration");
            logger.addAtrribute(demandConf, "display", "Demand Configuration");
            logger.logParameter(demandConf, new Parameter("ConfigurationName", "Configuration Name", "Default")); // TODO: replace real value
            logger.logParameter(demandConf, new Parameter("MtuNumLowPriorityMsg", "Mtu Num Low Priority Msg", "2")); // TODO: replace real value
            logger.logParameter(demandConf, new Parameter("MtuPrimaryWindowInterval", "Mtu Primary WindowInterval", "180")); // TODO: replace real value
            logger.logParameter(demandConf, new Parameter("MtuWindowAStart", "Mtu Window A Start", "0")); // TODO: replace real value
            logger.logParameter(demandConf, new Parameter("MtuWindowBStart", "Mtu Window B Start", "0")); // TODO: replace real value
            logger.logParameter(demandConf, new Parameter("MtuPrimaryWindowIntervalB", "Mtu Primary WindowInterval B", "3600")); // TODO: replace real value
            logger.logParameter(demandConf, new Parameter("MtuPrimaryWindowOffset", "Mtu Primary Window Offset", "51")); // TODO: replace real value
            addMtuAction.Add(demandConf);

            // TODO: log real optional params
            logger.logParameter(addMtuAction, new Parameter("MTU_Location_Data", "MTU Location", "Inside", true));
            logger.logParameter(addMtuAction, new Parameter("LocationInfo", "Meter Location", "Inside", true));
            logger.logParameter(addMtuAction, new Parameter("Construction_Type", "Construction", "Vinyl", true));
            logger.logParameter(addMtuAction, new Parameter("GPS_Y", "Lat", "43.8", true));
            logger.logParameter(addMtuAction, new Parameter("GPS_X", "Long", "23.2", true));
            logger.logParameter(addMtuAction, new Parameter("Altitude", "Elevation", "1", true));

            logger.logParameter(addMtuAction, new Parameter("InterfaceTamper", "Interface Tamper", "Enabled"));

            TurnOffLog(addMtuAction);
            TurnOnLog(addMtuAction);
            ReadMtuLog(addMtuAction);

            doc.Add(addMtuAction);
        }

        // turn off log
        private void TurnOffLog(XElement parent)
        {
            XElement turnOffAction = new XElement("Action");

            logger.addAtrribute(turnOffAction, "display", "Turn Off MTU");
            logger.addAtrribute(turnOffAction, "type", "TurnOffMtu");

            logger.logParameter(turnOffAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (this.user != null)
            {

                logger.logParameter(turnOffAction, new Parameter("User", "User", this.user));
            }

            logger.logParameter(turnOffAction, new Parameter("MtuId", "MTU ID", this.mtuId.ToString()));

            parent.Add(turnOffAction);
        }

        // turn on log
        private void TurnOnLog(XElement parent)
        {
            XElement turnOnAction = new XElement("Action");

            logger.addAtrribute(turnOnAction, "display", "Turn On MTU");
            logger.addAtrribute(turnOnAction, "type", "TurnOnMTU");

            logger.logParameter(turnOnAction, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (this.user != null)
            {
                logger.logParameter(turnOnAction, new Parameter("User", "User", this.user));
            }

            logger.logParameter(turnOnAction, new Parameter("MtuId", "MTU ID", this.mtuId.ToString()));

            parent.Add(turnOnAction);
        }

        // read mtu log
        private void ReadMtuLog(XElement parent)
        {

        }
    }
}
