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
