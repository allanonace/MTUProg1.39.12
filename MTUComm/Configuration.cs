using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xml;

namespace MTUComm
{
    public class Configuration
    {
        private String mbase_path;
        private MtuTypes mtuTypes;
        private MeterTypes meterTypes;
        private Global global;
        private InterfaceConfig interfaces;

        private string device;
        private string deviceUUID;
        private string version;
        private string appName;
        private static Configuration instance;

        private Configuration()
        {
            string base_path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                if (base_path.Contains("/data/user/0/"))
                    base_path = base_path.Replace("/data/user/0/", "/storage/emulated/0/Android/data/");
                else if (base_path.Contains("/data/data/"))
                    base_path = base_path.Replace("/data/data/", "/storage/emulated/0/Android/data/");
            }

            mbase_path = base_path;
            device = "PC";
            Config config = new Config();

            mtuTypes = config.GetMtu(Path.Combine(mbase_path, "Mtu.xml"));
            meterTypes = config.GetMeters(Path.Combine(mbase_path, "Meter.xml"));
            global = config.GetGlobal(Path.Combine(mbase_path, "Global.xml"));
            interfaces = config.GetInterfaces(Path.Combine(mbase_path, "Interface.xml"));
        }

        public Configuration(String base_path)
        {
            mbase_path = base_path;
            device = "PC";
            Config config = new Config();
            mtuTypes = config.GetMtu(Path.Combine(mbase_path, "Mtu.xml"));
            meterTypes = config.GetMeters(Path.Combine(mbase_path, "Meter.xml"));
            global = config.GetGlobal(Path.Combine(mbase_path, "Global.xml"));
            interfaces = config.GetInterfaces(Path.Combine(mbase_path, "Interface.xml"));
        }

        public static Configuration GetInstance()
        {
            if (instance == null)
            {
                instance = new Configuration(@"C: \Users\i.perezdealbeniz.BIZINTEK\Desktop\log_parse\codelog");
            }
            return instance;
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

        public Meter getMeterTyoeById(int meterId)
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

        public string GetMemoryMapTyeByMtuId(int mtuid)
        {
            return interfaces.GetmemoryMapTyeByMtuId(mtuid);
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

            if (device.Equals("PC"))
            {
                return_str = "ACLARATECH-CLE5478L-KGUILER";
            }else
                
            if (device.Equals("Android") || device.Equals("iOS") )
            {
                return_str = deviceUUID;
            }

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
