using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Xml
{
    public class Config
    {
        public UserList GetUsers (string path)
        {
            UserList users;
            XmlSerializer s = new XmlSerializer(typeof(UserList));
        
            using (StreamReader streamReader = new StreamReader(path))
            {
                string fileContent = NormalizeBooleans(streamReader.ReadToEnd());
                using (StringReader reader = new StringReader(fileContent))
                {
                    users = (UserList)s.Deserialize(reader);
                }
            }
 
            return users;
        }
    
        public DemandConf GetDemandConf(string path)
        {
            DemandConf demandConf;
            XmlSerializer s = new XmlSerializer(typeof(DemandConf));

            using (StreamReader streamReader = new StreamReader(path))
            {
                string fileContent = NormalizeBooleans(streamReader.ReadToEnd());
                using (StringReader reader = new StringReader(fileContent))
                {
                    demandConf = (DemandConf)s.Deserialize(reader);
                }
            }
 
            return demandConf;
        }

        public MeterTypes GetMeters(string path)
        {
            MeterTypes meterTypes;
            XmlSerializer s = new XmlSerializer(typeof(MeterTypes));

            using (StreamReader streamReader = new StreamReader(path))
            {
                string fileContent = NormalizeBooleans(streamReader.ReadToEnd());
                using (StringReader reader = new StringReader(fileContent))
                {
                    meterTypes = (MeterTypes)s.Deserialize(reader);
                }
            }

            return meterTypes;
        }
 
        public MtuTypes GetMtu(string path)
        {
            MtuTypes mtuTypes = new MtuTypes();
            XmlSerializer s = new XmlSerializer(typeof(MtuTypes));

            using (StreamReader streamReader = new StreamReader(path))
            {
                string fileContent = NormalizeBooleans(streamReader.ReadToEnd());
                using (StringReader reader = new StringReader(fileContent))
                {
                    mtuTypes = (MtuTypes)s.Deserialize(reader);
                }
            }

            return mtuTypes;
        }

        public AlarmList GetAlarms(string path)
        {
            AlarmList alarms;
            XmlSerializer s = new XmlSerializer(typeof(AlarmList));

            using (StreamReader streamReader = new StreamReader(path))
            {
                string fileContent = NormalizeBooleans(streamReader.ReadToEnd());
                using (StringReader reader = new StringReader(fileContent))
                {
                    alarms = (AlarmList)s.Deserialize(reader);
                }
            }

            return alarms;
        }

        public InterfaceConfig GetInterfaces(string path)
        {
            InterfaceConfig interfaces;
            XmlSerializer s = new XmlSerializer(typeof(InterfaceConfig));

            using (StreamReader streamReader = new StreamReader(path))
            {
                string fileContent = NormalizeBooleans(streamReader.ReadToEnd());
                using (StringReader reader = new StringReader(fileContent))
                {
                    interfaces = (InterfaceConfig)s.Deserialize(reader);
                }
            }

            return interfaces;
        }

        public Global GetGlobal(string path)
        {
            Global global;
            XmlSerializer s = new XmlSerializer(typeof(Global));

            using (StreamReader streamReader = new StreamReader(path))
            {
                string fileContent = NormalizeBooleans(streamReader.ReadToEnd());
                using (StringReader reader = new StringReader(fileContent))
                {
                    global = (Global)s.Deserialize(reader);
                    
                    var cancel = global.Cancel;
                    var cancel_def = global.Cancel_Default;
                    var cancel_des = global.Cancel_Deserialized;
                    
                    var o = global.Options;
                    var odef = global.Options_Default;
                    var odes = global.Options_Deserialized;
                }
            }

            return global;
        }

        public static string NormalizeBooleans(string input)
        {
            string       pattern = "(?:\"|>)(?i)(#)(?:\"|<)";
            List<string> words   = new List<string> () { "true", "false" };

            return Regex.Replace ( input,
                pattern.Replace ( "#", String.Join ( "|", words ) ),
                entry => entry.Value.ToLower() );
        }
    }
}
