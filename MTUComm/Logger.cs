using MTUComm.actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xml;

namespace MTUComm
{
    public class Logger
    {

        private String abs_path = "";
        private String fixed_name = "";

        private Configuration config;

        public Logger(Configuration config)
        {

            this.config = config;
            abs_path = config.GetBasePath(); 
        }

        public Logger(Configuration config, String Filename)
        {
            this.config = config;
            fixed_name = Filename;
            abs_path = config.GetBasePath();
        }


        private Boolean isFixedName()
        {
            if(fixed_name.Length > 0)
            {
                return true;
            }
            return false;
        }

        private String getFileName()
        {
            if (isFixedName())
            {
                return fixed_name;
            }
            else
            {
                return DateTime.Now.ToString("MMddyyyyHH")+"Log.xml";
            }
        }

        private string getBaseFileHandler()
        {
            string base_stream = "<?xml version=\"1.0\" encoding=\"ASCII\"?>";
            base_stream += "<StarSystem>";
            base_stream += "    <AppInfo>";
            base_stream += "        <AppName>" + config.getApplicationName() + "</AppName>";
            base_stream += "        <Version>" + config.GetApplicationVersion() + "</Version>";
            base_stream += "        <Date>" + DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm") + "</Date>";
            base_stream += "        <UTCOffset>" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>";
            base_stream += "        <UnitId>" + config.GetDeviceUUID() + "</UnitId>";
            base_stream += "        <AppType>" + (isFixedName() ? "Scripting" : "Interactive") + "</AppType>";
            base_stream += "    </AppInfo>";
            base_stream += "    <Message />";
            base_stream += "    <Mtus />";
            base_stream += "    <Warning />";
            base_stream += "    <Error />";
            base_stream += "</StarSystem>";
            return base_stream;
        }

        public string CreateFileIfNotExist()
        {
            string uri = Path.Combine(abs_path, getFileName());

            if (!File.Exists(uri))
            { 
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(abs_path, getFileName()), true))
                {
                    file.WriteLine(getBaseFileHandler());
                }
            }

            return uri;
        }


        private string getEventBaseFileHandler()
        {
            string base_stream = "<?xml version=\"1.0\" encoding=\"ASCII\"?>";
            base_stream += "<Log>";
            base_stream += "    <Transfer/>";
            base_stream += "</Log>";
            return base_stream;
        }

        public string CreateEventFileIfNotExist(string mtu_id)
        {
            string file_name = "MTUID"+ mtu_id+ "-" + System.DateTime.Now.ToString("MMddyyyyHH") + "-" + DateTime.Now.Ticks.ToString() + "DataLog.xml"; 
            string uri = Path.Combine(abs_path, file_name);

            if (!File.Exists(uri))
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(uri, true))
                {
                    file.WriteLine(getEventBaseFileHandler());
                }
            }

            return uri;
        }

        private XElement getRootElement()
        {
            CreateFileIfNotExist();
            XDocument doc =  XDocument.Load(Path.Combine(abs_path, getFileName()));
            XElement root = new XElement("StarSystem");

            return root;
        }

        public void addAtrribute(XElement node, String attname, String att_value)
        {
            if(att_value != null && att_value.Length > 0)
            {
                node.Add(new XAttribute(attname, att_value));
            }
        }

        public void logLogin(String username)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            XElement error = new XElement("AppMessage");

            logParameter(error, new Parameter("Date", null, DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            XElement message = new XElement("Message", "User Login: "+ username);
            error.Add(message);

            doc.Root.Element("Message").Add(error);
            doc.Save(Path.Combine(abs_path, getFileName()));
        }

        public string logReadResultString(Action ref_action, ActionResult result)
        {
            XDocument doc = XDocument.Parse(getBaseFileHandler());
            logReadResult(doc.Root.Element("Mtus"), ref_action, result, Int32.Parse(result.getParameterByTag("MtuType").Value));
            return doc.ToString();
        }

        public void logReadResult(Action ref_action, ActionResult result, Mtu mtuType)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            logReadResult(doc.Root.Element("Mtus"), ref_action, result, mtuType.Id);
            doc.Save(Path.Combine(abs_path, getFileName()));
        }

        public void logReadResult(XElement parent, Action ref_action, ActionResult result, int mtu_type_id)
        {
            XElement action = new XElement("Action");

            addAtrribute(action, "display", ref_action.getDisplay());
            addAtrribute(action, "type", ref_action.getLogType());
            addAtrribute(action, "reason", ref_action.getReason());

            InterfaceParameters[] parameters = config.getLogInterfaceFields(mtu_type_id, "ReadMTU");
            foreach (InterfaceParameters parameter in parameters)
            {
                if(parameter.Name == "Port") {

                    ActionResult[] ports = result.getPorts();
                    for (int i = 0; i < ports.Length; i++)
                    {
                        logPort(i, action, ports[i], parameter.Parameters.ToArray());
                    }
                }
                else
                {
                    logComplexParameter(action, result, parameter);
                }
                
            }

                /*logParameter(action, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

                if (ref_action.getUser() != null)
                {

                    logParameter(action, new Parameter("User", "User", ref_action.getUser()));
                }

                foreach(Parameter parameter in result.getParameters())
                {
                    logParameter(action, parameter);
                }

                ActionResult[] ports = result.getPorts();
                for (int i= 0; i < ports.Length; i++)
                {
                    logPort(i, action, ports[i]);
                }
                */

                parent.Add(action);
        }

        private void logPort(int portnumber, XElement parent, ActionResult result, InterfaceParameters[] parameters)
        {
            XElement port = new XElement("Port");

            addAtrribute(port, "display", "Port "+ (portnumber+1).ToString());
            addAtrribute(port, "number", (portnumber + 1).ToString());

            foreach (InterfaceParameters parameter in parameters)
            {
                logComplexParameter(port, result, parameter);
            }

            parent.Add(port);
        }

        public void logTurnOffResult(Action ref_action, uint MtuId)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            logTurnOffResult(doc.Root.Element("Mtus"), ref_action, MtuId);
            doc.Save(Path.Combine(abs_path, getFileName()));
        }

        public void logTurnOffResult(XElement parent, Action ref_action, uint MtuId)
        {
            XElement action = new XElement("Action");

            addAtrribute(action, "display", ref_action.getDisplay());
            addAtrribute(action, "type", ref_action.getLogType());

            logParameter(action, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (ref_action.getUser() != null)
            {

                logParameter(action, new Parameter("User", "User", ref_action.getUser()));
            }

            logParameter(action, new Parameter("MtuId", "MTU ID", MtuId.ToString()));

            parent.Add(action);
        }

        public void logTurnOnResult(Action ref_action, uint MtuId)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            logTurnOnResult(doc.Root.Element("Mtus"), ref_action, MtuId);
            doc.Save(Path.Combine(abs_path, getFileName()));
        }

        public void logTurnOnResult(XElement parent, Action ref_action, uint MtuId)
        {
            XElement action = new XElement("Action");

            addAtrribute(action, "display", ref_action.getDisplay());
            addAtrribute(action, "type", ref_action.getLogType());

            logParameter(action, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (ref_action.getUser() != null)
            {

                logParameter(action, new Parameter("User", "User", ref_action.getUser()));
            }

            logParameter(action, new Parameter("MtuId", "MTU ID", MtuId.ToString()));

            parent.Add(action);
        }

        public void logAddMtuResult(Action ref_action, AddMtuForm form)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            logAddMtuResult(doc.Root.Element("Mtus"), ref_action, form);
            doc.Save(Path.Combine(abs_path, getFileName()));
        }

        public void logAddMtuResult(XElement parent, Action ref_action, dynamic form)
        {
            MTUBasicInfo mtu = MtuForm.mtuBasicInfo;

            XElement action = new XElement("Action");

            addAtrribute(action, "display", ref_action.getDisplay());
            addAtrribute(action, "type", ref_action.getLogType());

            logParameter(action, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (ref_action.getUser() != null)
            {
                logParameter(action, new Parameter("User", "User", ref_action.getUser()));
            }

            logParameter(action, new Parameter("MtuId", "MTU ID", mtu.Id)); // TODO: replace real value
            logParameter(action, new Parameter("MtuType", "MTU Type", mtu.Type)); // TODO: replace real value

            XElement port = new XElement("Port");
            addAtrribute(port, "display", "Port 1");
            addAtrribute(port, "number", "1");
            logParameter(port, new Parameter("AccountNumber", "Service Pt. ID", "1234567890")); // TODO: replace real value
            logParameter(port, new Parameter("WorkOrder", "Field Order", "12345678901234567890")); // TODO: replace real value
            logParameter(port, new Parameter("NewMeterSerialNumber", "New Meter Serial Number", "123456789012")); // TODO: replace real value
            logParameter(port, new Parameter("MeterType", "Meter Type", "(1112) NEPT T10 3/4 E-Coder 0.01CuFt")); // TODO: replace real value
            logParameter(port, new Parameter("MeterTypeId", "Meter Type ID", "1112")); // TODO: replace real value
            logParameter(port, new Parameter("MeterVendor", "Meter Vendor", "SCHLUM/NEPTUNE")); // TODO: replace real value
            logParameter(port, new Parameter("MeterModel", "Meter Model", "T-10")); // TODO: replace real value
            action.Add(port);

            if (form.conditions.TwoPorts)
            {
                port = new XElement("Port");
                addAtrribute(port, "display", "Port 2");
                addAtrribute(port, "number", "2");
                logParameter(port, new Parameter("AccountNumber", "Service Pt. ID", "1234567890")); // TODO: replace real value
                logParameter(port, new Parameter("WorkOrder", "Field Order", "12345678901234567890")); // TODO: replace real value
                logParameter(port, new Parameter("NewMeterSerialNumber", "New Meter Serial Number", "123456789012")); // TODO: replace real value
                logParameter(port, new Parameter("MeterType", "Meter Type", "(1112) NEPT T10 3/4 E-Coder 0.01CuFt")); // TODO: replace real value
                logParameter(port, new Parameter("MeterTypeId", "Meter Type ID", "1112")); // TODO: replace real value
                logParameter(port, new Parameter("MeterVendor", "Meter Vendor", "SCHLUM/NEPTUNE")); // TODO: replace real value
                logParameter(port, new Parameter("MeterModel", "Meter Model", "T-10")); // TODO: replace real value
                action.Add(port);
            }

            logParameter(action, new Parameter("ReadInterval", "Read Interval", "15 Min")); // TODO: replace real value
            logParameter(action, new Parameter("Fast-2-Way", "Fast Message Config", "Fast")); // TODO: replace real value
            logParameter(action, new Parameter("DailyGMTHourRead", "GMT Daily Reads", "Disable")); // TODO: replace real value
            logParameter(action, new Parameter("DailyReads", "Daily Reads", "Disable")); // TODO: replace real value

            if (form.conditions.RequiresAlarmProfile)
            {
                XElement alarmSelection = new XElement("AlarmSelection");
                addAtrribute(alarmSelection, "display", "Alarm Selection");
                logParameter(alarmSelection, new Parameter("AlarmConfiguration", "Alarm Configuration Name", "All")); // TODO: replace real value
                logParameter(alarmSelection, new Parameter("Overlap", "Message Overlap", "6")); // TODO: replace real value
                logParameter(alarmSelection, new Parameter("LastGaspImm", "Last Gasp Imm", "Enabled")); // TODO: replace real value
                logParameter(alarmSelection, new Parameter("InterfaceTamperImm", "SerialComProblem Imm", "Enabled")); // TODO: replace real value
                logParameter(alarmSelection, new Parameter("InsufficentMemoryImm", "Insufficent Memory Imm", "Enabled")); // TODO: replace real value
                logParameter(alarmSelection, new Parameter("LastGasp", "Last Gasp", "Enabled")); // TODO: replace real value
                logParameter(alarmSelection, new Parameter("InsufficentMemory", "Insufficent Memory", "Enabled")); // TODO: replace real value
                logParameter(alarmSelection, new Parameter("InterfaceTamper", "Interface Tamper", "Enabled")); // TODO: replace real value
                action.Add(alarmSelection);
            }

            if (form.conditions.MtuDemand)
            {
                XElement demandConf = new XElement("DemandConfiguration");
                addAtrribute(demandConf, "display", "Demand Configuration");
                logParameter(demandConf, new Parameter("ConfigurationName", "Configuration Name", "Default")); // TODO: replace real value
                logParameter(demandConf, new Parameter("MtuNumLowPriorityMsg", "Mtu Num Low Priority Msg", "2")); // TODO: replace real value
                logParameter(demandConf, new Parameter("MtuPrimaryWindowInterval", "Mtu Primary WindowInterval", "180")); // TODO: replace real value
                logParameter(demandConf, new Parameter("MtuWindowAStart", "Mtu Window A Start", "0")); // TODO: replace real value
                logParameter(demandConf, new Parameter("MtuWindowBStart", "Mtu Window B Start", "0")); // TODO: replace real value
                logParameter(demandConf, new Parameter("MtuPrimaryWindowIntervalB", "Mtu Primary WindowInterval B", "3600")); // TODO: replace real value
                logParameter(demandConf, new Parameter("MtuPrimaryWindowOffset", "Mtu Primary Window Offset", "51")); // TODO: replace real value
                action.Add(demandConf);
            }

            // TODO: log real optional params
            logParameter(action, new Parameter("MTU_Location_Data", "MTU Location", "Inside", true));
            logParameter(action, new Parameter("LocationInfo", "Meter Location", "Inside", true));
            logParameter(action, new Parameter("Construction_Type", "Construction", "Vinyl", true));
            logParameter(action, new Parameter("GPS_Y", "Lat", "43.8", true));
            logParameter(action, new Parameter("GPS_X", "Long", "23.2", true));
            logParameter(action, new Parameter("Altitude", "Elevation", "1", true));

            logParameter(action, new Parameter("InterfaceTamper", "Interface Tamper", "Enabled"));

            parent.Add(action);
        }

        public void logCancel(Action ref_action, String cancel, String reason)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            XElement action = new XElement("Action");

            addAtrribute(action, "display", ref_action.getDisplay());
            addAtrribute(action, "type", ref_action.getLogType());
            addAtrribute(action, "reason", ref_action.getReason());

            logParameter(action, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));
            logParameter(action, new Parameter("User", "User", ref_action.getUser()));

            logParameter(action, new Parameter("Cancel", "Cancel Action", cancel));
            logParameter(action, new Parameter("Reason", "Cancel Reason", reason));

            doc.Root.Element("Mtus").Add(action);
            doc.Save(Path.Combine(abs_path, getFileName()));

        }

        public void LogError(String e_message)
        {
            LogError(-1, e_message);
        }

       public void LogError(int id, String e_message)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            XElement error = new XElement("AppError");

            logParameter(error, new Parameter("Date", null, DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            XElement message = new XElement("Message", e_message);
            if(id >= 0)
            {
                addAtrribute(message, "ErrorId", id.ToString());
            }
            error.Add(message);

            doc.Root.Element("Error").Add(error);
            doc.Save(Path.Combine(abs_path, getFileName()));
        }

        public void logAction(Action ref_action)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            logAction(doc.Root.Element("Mtus"), ref_action);
            doc.Save(Path.Combine(abs_path, getFileName()));
    
        }

        public void logAction(XElement parent, Action ref_action)
        {

            XElement action = new XElement("Action");

            addAtrribute(action, "display", ref_action.getDisplay());
            addAtrribute(action, "type", ref_action.getLogType());
            addAtrribute(action, "reason", ref_action.getReason());


            if(ref_action.getUser() != null)
            {
                logParameter(action, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));
                logParameter(action, new Parameter("User", "User", ref_action.getUser()));
            }

            Parameter[] parameters = ref_action.getParameters();
            foreach (Parameter parameter in parameters)
            {
                if (parameter.doesGenerateLog()) {
                    logParameter(action, parameter);
                }
                
            }


            Action[] sub_actions = ref_action.getSubActions();
            foreach(Action subaction in sub_actions)
            {
                logAction(action, subaction);
            }

            parent.Add(action);

        }

        private void logDataResultEntry(XElement events, LogDataEntry entry)
        {
            XElement read_event = new XElement("MeterReadEvent");

            addAtrribute(read_event, "FormatVersion", entry.FormatVersion.ToString());

            read_event.Add(new XElement("TimeStamp", entry.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")));
            read_event.Add(new XElement("MeterRead", entry.MeterRead.ToString()));
            read_event.Add(new XElement("ErrorStatus", entry.ErrorStatus.ToString()));
            read_event.Add(new XElement("ReadInterval", "PT"+entry.ReadInterval.ToString()+"M"));
            read_event.Add(new XElement("PortNumber", "PORT"+entry.PortNumber.ToString()));
            read_event.Add(new XElement("IsDailyRead", entry.IsDailyRead.ToString()));
            read_event.Add(new XElement("IsTopOfHourRead", entry.IsTopOfHourRead.ToString()));
            read_event.Add(new XElement("ReadReason", entry.ReasonForRead.ToString()));
            read_event.Add(new XElement("IsSynchronized", entry.IsSynchronized.ToString()));


            events.Add(read_event);
        }

        public string logReadDataResultEntries(string mtu_id, DateTime start, DateTime end, List<LogDataEntry> Entries)
        {
            String path = CreateEventFileIfNotExist(mtu_id);
            XDocument doc = XDocument.Load(path);

            XElement transfer  = doc.Root.Element("Transfer");


            XElement events = new XElement("Events");

            addAtrribute(events, "FilterMode", "Match");
            addAtrribute(events, "FilterValue", "MeterRead");
            addAtrribute(events, "RangeStart", start.ToString("yyyy-MM-dd HH:mm:ss"));
            addAtrribute(events, "RangeStop", end.ToString("yyyy-MM-dd HH:mm:ss"));

            foreach (LogDataEntry entry in Entries)
            {
                logDataResultEntry(events, entry);
            }

            transfer.Add(events);

            doc.Save(path);

            return path;
        }

        private void logComplexParameter(XElement parent, ActionResult result, InterfaceParameters parameter)
        {
            Parameter param = null;

            if (parameter.Source != null)
            {
                try
                {
                    param = result.getParameterByTag(parameter.Source.Split(new char[] { '.' })[1]);
                }
                catch (Exception e) { }

            }
            if (param == null)
            {
                param = result.getParameterByTag(parameter.Name);
            }

            if (param != null)
            {
                logParameter(parent, param, parameter.Name);
            }
        }

        public void logParameter(XElement parent, Parameter parameter)
        {
            logParameter(parent, parameter, parameter.getLogTag());
        }

        public void logParameter(XElement parent, Parameter parameter, string TagName)
        {
            XElement xml_parameter = new XElement(TagName, parameter.getValue());
            addAtrribute(xml_parameter, "display", parameter.getLogDisplay());
            if (parameter.Optional)
            {
                addAtrribute(xml_parameter, "option", "1");
            }
            parent.Add(xml_parameter);
        }

    }
}
