using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace Xml
{
    public class Config
    {
        public DemandConf GetDemandConf(string path)
        {
            DemandConf demandConf;
            XmlSerializer s = new XmlSerializer(typeof(DemandConf));

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    demandConf = (DemandConf)s.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                throw new DemandConfLoadException("Error loading DemandConf file");
            }
 
            return demandConf;
        }

        public MeterTypes GetMeters(string path)
        {
            MeterTypes meterTypes;
            XmlSerializer s = new XmlSerializer(typeof(MeterTypes));

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    meterTypes = (MeterTypes)s.Deserialize(reader);
                }
            }
            catch (Exception e)
            {
                throw new MeterLoadException("Error loading Meter file");
            }

            return meterTypes;
        }
 
        public MtuTypes GetMtu(string path)
        {
            MtuTypes mtuTypes = new MtuTypes();

            XmlSerializer s = new XmlSerializer(typeof(MtuTypes));

            try
            {
               
                using (StreamReader reader = new StreamReader(path))
                {
                    mtuTypes = (MtuTypes)s.Deserialize(reader);
                }

            }

              
            catch (Exception e ) 
            {
                throw new MtuLoadException("Error loading Mtu file: " + e.Message );
            }

            return mtuTypes;
        }



        public Memories GetMemory(string path)
        {
            Memories memories;
            XmlSerializer s = new XmlSerializer(typeof(Memories));

            using (StreamReader reader = new StreamReader(path))
            {
                memories = (Memories)s.Deserialize(reader);
            }

            return memories;
        }

        public AlarmList GetAlarms(string path)
        {
            AlarmList alarms;
            XmlSerializer s = new XmlSerializer(typeof(AlarmList));

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    alarms = (AlarmList)s.Deserialize(reader);
                }
            }
            catch (Exception)
            {
                throw new AlarmLoadException("Error loading Alarm file");
            }

            return alarms;
        }

        public Global GetGlobal(string path)
        {
            Global global;
            XmlSerializer s = new XmlSerializer(typeof(Global));

            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    global = (Global)s.Deserialize(reader);
                }
            }
            catch (Exception e )
            {
                Console.WriteLine(e.StackTrace);
                throw new GlobalLoadException("Error loading Global file");
            }

            return global;
        }
    }
}
