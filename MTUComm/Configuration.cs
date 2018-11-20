﻿using System;
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
        private const string SUBPATHFILES = "Android/data/com.aclara.mtu.programmer/files/";
        private const string XML_MTUS     = "Mtu.xml";
        private const string XML_METERS   = "Meter.xml";
        private const string XML_GLOBAL   = "Global.xml";
        private const string XML_INTERFACE   = "Interface.xml";
        private const string XML_ALARMS = "Alarm.xml";
        private const string XML_DEMANDS = "DemandConf.xml";

        private String mbase_path;
        private MtuTypes mtuTypes;
        private MeterTypes meterTypes;
        private Global global;
        private InterfaceConfig interfaces;
        private AlarmList alarms;
        private DemandConf demands;

        private string device;
        private string deviceUUID;
        private string version;
        private string appName;
        private static Configuration instance;

        private enum PATHS
        {
            STORAGE_EMULATED_ZERO,
            STORAGE_EMULATED_LEGACY,
            STORAGE_SDCARD_ZERO,
            SDCARD_MNT,
            SDCARD
        }

        private string[] paths =
        {
            "/storage/emulated/0/",      // Espacio de trabajo del usuario cero/0
            "/storage/emulated/legacy/", // Enlace simbolico a "/storage/emulated/0/"
            "/storage/sdcard0/",         // Android >= 4.0
            "/mnt/sdcard/",              // Android < 4.0
            "/sdcard/"                   // Enlace simbolico a "/storage/sdcard0/" y "/mnt/sdcard/"
        };

        private string GetPath ( PATHS ePath )
        {
            return paths[ (int)ePath ];
        }

        private Configuration ()
        {
            mbase_path = Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments );

            if ( Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android )
            {
                // Search the first valid path to recover XML files
                PATHS  ePath;
                string path;
                string[] names = Enum.GetNames(typeof(PATHS));
                for (int i = 0; i < names.Length; i++)
                {
                    Enum.TryParse<PATHS> ( names[i], out ePath );
                    path = this.GetPath ( ePath );

                    if ( Directory.Exists ( this.GetPath ( ePath ) ) &&
                         File.Exists ( path + SUBPATHFILES + XML_MTUS ) )
                    {
                        mbase_path = path + SUBPATHFILES;
                        break;
                    }
                }
            }

            device = "PC";
            Config config = new Config();

            mtuTypes = config.GetMtu(Path.Combine(mbase_path, XML_MTUS ));
            meterTypes = config.GetMeters(Path.Combine(mbase_path, XML_METERS ));
            global = config.GetGlobal(Path.Combine(mbase_path, XML_GLOBAL ));
            interfaces = config.GetInterfaces(Path.Combine(mbase_path, XML_INTERFACE));
            alarms = config.GetAlarms(Path.Combine(mbase_path, XML_ALARMS));
            demands = config.GetDemandConf(Path.Combine(mbase_path, XML_DEMANDS));
        }

        public Configuration(String base_path)
        {
            mbase_path = base_path;
            device = "PC";
            Config config = new Config();

            mtuTypes = config.GetMtu(Path.Combine(mbase_path, XML_MTUS ));
            meterTypes = config.GetMeters(Path.Combine(mbase_path, XML_METERS ));
            global = config.GetGlobal(Path.Combine(mbase_path, XML_GLOBAL ));
            interfaces = config.GetInterfaces(Path.Combine(mbase_path, XML_INTERFACE));
            alarms = config.GetAlarms(Path.Combine(mbase_path, XML_ALARMS));
            demands = config.GetDemandConf(Path.Combine(mbase_path, XML_DEMANDS));
        }

        public static Configuration GetInstance()
        {
            if (instance == null)
            {
                instance = new Configuration();
                //instance = new Configuration(@"C:\Users\i.perezdealbeniz.BIZINTEK\Desktop\log_parse\run_basepath");// @"C: \Users\i.perezdealbeniz.BIZINTEK\Desktop\log_parse\codelog");
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
