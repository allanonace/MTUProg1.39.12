using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Xml;

namespace MTUComm
{
    public class Configuration
    {
        private const string XML_MTUS      = "Mtu.xml";
        private const string XML_METERS    = "Meter.xml";
        private const string XML_GLOBAL    = "Global.xml";
        private const string XML_INTERFACE = "Interface.xml";
        private const string XML_ALARMS    = "Alarm.xml";
        private const string XML_DEMANDS   = "DemandConf.xml";

        private String mbase_path;
        public MtuTypes mtuTypes;
        public MeterTypes meterTypes;
        public Global global;
        public InterfaceConfig interfaces;
        public AlarmList alarms;
        public DemandConf demands;
        
        private string device;
        private string deviceUUID;
        private string version;
        private string appName;
        private static Configuration instance;

        private Configuration ( string path = "" )
        {
            mbase_path = ( string.IsNullOrEmpty ( path ) ) ? Mobile.GetPath () : path;

            device = "PC";
            Config config = new Config ();

            mtuTypes   = config.GetMtu        ( Path.Combine(mbase_path, XML_MTUS      ) );
            meterTypes = config.GetMeters     ( Path.Combine(mbase_path, XML_METERS    ) );
            global     = config.GetGlobal     ( Path.Combine(mbase_path, XML_GLOBAL    ) );
            interfaces = config.GetInterfaces ( Path.Combine(mbase_path, XML_INTERFACE ) );
            alarms     = config.GetAlarms     ( Path.Combine(mbase_path, XML_ALARMS    ) );
            demands    = config.GetDemandConf ( Path.Combine(mbase_path, XML_DEMANDS   ) );
        }

        public static Configuration GetInstance ( string path = "" )
        {
            if ( instance == null )
            {
                instance = new Configuration ( path );
                //instance = new Configuration(@"C:\Users\i.perezdealbeniz.BIZINTEK\Desktop\log_parse\run_basepath");// @"C: \Users\i.perezdealbeniz.BIZINTEK\Desktop\log_parse\codelog");
            }
            return instance;
        }

        public static void SetInstance ( Configuration configuration )
        {
            instance = configuration;
        }

        public String GetBasePath()
        {
            return mbase_path;
        }

        public Global GetGlobal()
        {
            return global;

        }

        public Mtu[] GetMtuTypes()
        {
            return mtuTypes.Mtus.ToArray();

        }

        public Mtu GetMtuTypeById(int mtuId)
        {
            return mtuTypes.FindByMtuId(mtuId);
        }

        public Meter[] GetMeterType()
        {
            return meterTypes.Meters.ToArray();
        }

        public MeterTypes GetMeterTypes()
        {
            return meterTypes;
        }

        public Meter getMeterTypeById(int meterId)
        {
            return meterTypes.FindByMterId(meterId);
        }

        public InterfaceParameters[] getAllInterfaceFields(int mtuid, string Action)
        {
            return interfaces.GetInterfaceByMtuIdAndAction(mtuid, Action).getAllInterfaces();
        }

        public InterfaceParameters[] getLogInterfaceFields(int mtuid, string Action)
        {
            return interfaces.GetInterfaceByMtuIdAndAction(mtuid, Action).getLogInterfaces();
        }

        public InterfaceParameters[] getUserInterfaceFields(int mtuid, string Action)
        {
            return interfaces.GetInterfaceByMtuIdAndAction(mtuid, Action).getUserInterfaces();
        }

        public string GetMemoryMapTypeByMtuId(int mtuid)
        {
            return interfaces.GetmemoryMapTypeByMtuId(mtuid);
        }

        public int GetmemoryMapSizeByMtuId(int mtuid)
        {
            return interfaces.GetmemoryMapSizeByMtuId(mtuid);
        }

        public MemRegister getFamilyRegister(int mtuid, string regsiter_name)
        {
            try
            {
                return getFamilyRegister(interfaces.GetmemoryMapTypeByMtuId(mtuid), regsiter_name);
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        public MemRegister getFamilyRegister(string family, string regsiter_name)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MemRegisterList));
                using (TextReader reader = new StreamReader(Path.Combine(mbase_path, "family_" + family + ".xml")))
                {
                    MemRegisterList list = serializer.Deserialize(reader) as MemRegisterList;
                    if (list.Registers != null)
                    {
                        foreach (MemRegister xmlRegister in list.Registers)
                        {
                            if (xmlRegister.Id.ToLower().Equals(regsiter_name.ToLower()))
                            {
                                return xmlRegister;
                            }
                        }
                    }
                }
            }catch (Exception e) { }

            return null;
        }

        public List<string>  GetVendorsFromMeters()
        {
            return meterTypes.GetVendorsFromMeters(meterTypes.Meters);
        }

        public List<string> GetModelsByVendorFromMeters(String vendor)
        {
            return meterTypes.GetModelsByVendorFromMeters(meterTypes.Meters, vendor);
        }

        public Boolean useDummyDigits()
        {
            return !global.LiveDigitsOnly;
        }

        public String GetDeviceUUID()
        {
            string return_str = "";

            return_str = deviceUUID;

            /*
            if (device.Equals("PC"))
            {
                return_str = "ACLARATECH-CLE5478L-KGUILER";
            }else
                
            if (device.Equals("Android") || device.Equals("iOS") )
            {
                return_str = deviceUUID;
            }
            */

            return return_str; //get UUID from Xamarin
        }

        public String GetApplicationVersion()
        {

            string return_str = "";

            if (device.Equals("PC"))
            {
                return_str = "2.2.5.0";
            }
            else

            if (device.Equals("Android") || device.Equals("iOS"))
            {
                return_str = version;
            }

            return return_str; //get UUID from Xamarin

        }

        public AlarmList Alarms
        {
            get
            {
                return this.alarms;
            }
        }

        public String getApplicationName()
        {

            string return_str = "";

            if (device.Equals("PC"))
            {
                return_str = "AclaraStarSystemMobileR";
            }
            else

            if (device.Equals("Android") || device.Equals("iOS"))
            {
                return_str = appName;
            }


            return return_str; //get UUID from Xamarin
        }

        public void setPlatform(string device_os)
        {
            
            device = device_os;
        }

        public void setDeviceUUID(string UUID)
        {
            deviceUUID = UUID; 
        }

        public void setVersion(string VERSION)
        {
            version = VERSION;
        }

        public void setAppName(string NAME)
        {
            appName = NAME;
        }
    }
}
