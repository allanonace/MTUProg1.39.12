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

        private void CreateFileIfNotExist()
        {
            if (!File.Exists(Path.Combine(abs_path, getFileName())))
            { 
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Path.Combine(abs_path, getFileName()), true))
                {
                    file.WriteLine("<?xml version=\"1.0\" encoding=\"ASCII\"?>");
                    file.WriteLine("<StarSystem>");
                    file.WriteLine("    <AppInfo>");
                    file.WriteLine("        <AppName>AclaraStarSystemMobile</AppName>");
                    file.WriteLine("        <Version>2.2.5.0</Version>");
                    file.WriteLine("        <Date>"+ DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm") + "</Date>");
                    file.WriteLine("        <UTCOffset>"+ TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString() + "</UTCOffset>");
                    file.WriteLine("        <UnitId>ACLARATECH-CLE5478L-KGUILER</UnitId>");
                    file.WriteLine("        <AppType>"+(isFixedName() ? "Scripting" : "Interactive") +"</AppType>");
                    file.WriteLine("    </AppInfo>");
                    file.WriteLine("    <Message />");
                    file.WriteLine("    <Mtus />");
                    file.WriteLine("    <Warning />");
                    file.WriteLine("    <Error />");
                    file.WriteLine("</StarSystem>");
                }
            }

        }

        private XElement getRootElement()
        {
            CreateFileIfNotExist();
            XDocument doc =  XDocument.Load(Path.Combine(abs_path, getFileName()));
            XElement root = new XElement("StarSystem");

            return root;
        }

        private void addAtrribute(XElement node, String attname, String att_value)
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

        public void logReadResult(Action ref_action, ReadResult result)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            logReadResult(doc.Root.Element("Mtus"), ref_action, result);
            doc.Save(Path.Combine(abs_path, getFileName()));
        }

        public void logReadResult(XElement parent, Action ref_action, ReadResult result)
        {
            XElement action = new XElement("Action");

            addAtrribute(action, "display", ref_action.getDisplay());
            addAtrribute(action, "type", ref_action.getLogType());
            addAtrribute(action, "reason", ref_action.getReason());

            logParameter(action, new Parameter("Date", "Date/Time", DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            if (ref_action.getUser() != null)
            {
                
                logParameter(action, new Parameter("User", "User", ref_action.getUser()));
            }

            foreach(Parameter parameter in result.getParameters())
            {
                logParameter(action, parameter);
            }

            ReadResult[] ports = result.getPorts();
            for (int i= 0; i < ports.Length; i++)
            {
                logPort(i, action, ports[i]);
            }

            parent.Add(action);
        }

        private void logPort(int portnumber, XElement parent, ReadResult result)
        {


            XElement port = new XElement("Port");

            addAtrribute(port, "display", "Port "+ (portnumber+1).ToString());
            addAtrribute(port, "number", (portnumber + 1).ToString());

            foreach (Parameter parameter in result.getParameters())
            {
                logParameter(port, parameter);
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

        public void LogError(int id, String e_message)
        {
            CreateFileIfNotExist();
            XDocument doc = XDocument.Load(Path.Combine(abs_path, getFileName()));

            XElement error = new XElement("AppError");

            logParameter(error, new Parameter("Date", null, DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss")));

            XElement message = new XElement("Message", e_message);
            addAtrribute(message, "ErrorId", id.ToString());
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

        public void logParameter(XElement parent, Parameter parameter)
        {
            XElement xml_parameter = new XElement(parameter.getLogTag(), parameter.getValue());
            addAtrribute(xml_parameter, "display", parameter.getLogDisplay());
            parent.Add(xml_parameter);
        }

    }
}
