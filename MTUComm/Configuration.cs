using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Xml;
using MTUComm.Exceptions;

using ActionType = MTUComm.Action.ActionType;

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
        private const string XML_USERS     = "User.xml";

        public MtuTypes mtuTypes;
        public MeterTypes meterTypes;
        public Global global;
        public InterfaceConfig interfaces;
        public AlarmList alarms;
        public DemandConf demands;
        public User[] users;
        
        private string device;
        private string deviceUUID;
        private string version;
        private string appName;
        private static Configuration instance;

        private Configuration ( string path = "" )
        {
            string configPath = Mobile.GetPathConfig ();

            device = "PC";
            Config config = new Config ();

            /* TEST - Override configuration file to force parsing error
            string PRIVATE = Environment.GetFolderPath ( Environment.SpecialFolder.Resources );
            File.WriteAllText ( Path.Combine ( PRIVATE, ".Config/Mtu.xml" ), "TEST" );
            */

            try
            {
                mtuTypes   = config.GetMtu        ( Path.Combine ( configPath, XML_MTUS      ) );
                meterTypes = config.GetMeters     ( Path.Combine ( configPath, XML_METERS    ) );
                global     = config.GetGlobal     ( Path.Combine ( configPath, XML_GLOBAL    ) );
                interfaces = config.GetInterfaces ( Path.Combine ( configPath, XML_INTERFACE ) );
                alarms     = config.GetAlarms     ( Path.Combine ( configPath, XML_ALARMS    ) );
                demands    = config.GetDemandConf ( Path.Combine ( configPath, XML_DEMANDS   ) );
                users      = config.GetUsers      ( Path.Combine ( configPath, XML_USERS     ) ).List;
            }
            catch ( Exception e )
            {
                if ( e is FileNotFoundException )
                     throw new ConfigurationFilesNotFoundException ();
                else throw new ConfigurationFilesCorruptedException ();
            }
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

        public Global GetGlobal()
        {
            return global;

        }

        public Mtu[] GetMtuTypes()
        {
            return mtuTypes.Mtus.ToArray();

        }

        public Mtu GetMtuTypeById ( int mtuId )
        {
            Mtu mtu = mtuTypes.FindByMtuId ( mtuId );
            
            if ( mtu == null )
                Errors.LogErrorNow ( new MtuTypeIsNotFoundException () );
            
            return mtu;
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

        public InterfaceParameters[] getAllInterfaceFields ( int mtuid, ActionType actionType )
        {
            return interfaces.GetInterfaceByMtuIdAndAction ( mtuid, actionType.ToString () ).getAllInterfaces ();
        }

        public InterfaceParameters[] getLogInterfaceFields ( int mtuid, ActionType actionType )
        {
            return interfaces.GetInterfaceByMtuIdAndAction ( mtuid, actionType.ToString () ).getLogInterfaces ();
        }

        public InterfaceParameters[] getUserInterfaceFields ( int mtuid, ActionType actionType )
        {
            return interfaces.GetInterfaceByMtuIdAndAction ( mtuid, actionType.ToString () ).getUserInterfaces ();
        }

        public string GetMemoryMapTypeByMtuId ( Mtu mtu )
        {
            return InterfaceAux.GetmemoryMapTypeByMtuId ( mtu );
        }

        public int GetmemoryMapSizeByMtuId ( Mtu mtu )
        {
            return InterfaceAux.GetmemoryMapSizeByMtuId ( mtu );
        }

        public MemRegister getFamilyRegister( Mtu mtu, string regsiter_name)
        {
            try
            {
                return getFamilyRegister ( InterfaceAux.GetmemoryMapTypeByMtuId ( mtu ), regsiter_name);
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
                using (TextReader reader = new StreamReader ( Path.Combine ( Mobile.GetPathConfig (), "family_" + family + ".xml")))
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
