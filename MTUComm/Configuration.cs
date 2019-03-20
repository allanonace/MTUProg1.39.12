using System;
using System.Collections.Generic;
using System.IO;
using Xml;
using MTUComm.Exceptions;
using System.Xml.Serialization;

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

            //TEST - Override configuration file to force parsing error
            //File.WriteAllText ( Path.Combine ( Mobile.GetPathConfig (), "certificate.txt" ), "TEST" );

            try
            {
                // Load configuration files ( xml's )
                mtuTypes   = Aux.DeserializeXml<MtuTypes>        ( Path.Combine ( configPath, XML_MTUS      ) );
                meterTypes = Aux.DeserializeXml<MeterTypes>      ( Path.Combine ( configPath, XML_METERS    ) );
                global     = Aux.DeserializeXml<Global>          ( Path.Combine ( configPath, XML_GLOBAL    ) );
                interfaces = Aux.DeserializeXml<InterfaceConfig> ( Path.Combine ( configPath, XML_INTERFACE ) );
                alarms     = Aux.DeserializeXml<AlarmList>       ( Path.Combine ( configPath, XML_ALARMS    ) );
                demands    = Aux.DeserializeXml<DemandConf>      ( Path.Combine ( configPath, XML_DEMANDS   ) );
                users      = Aux.DeserializeXml<UserList>        ( Path.Combine ( configPath, XML_USERS     ) ).List;
                
                // Regenerate certificate from base64 string
                Mobile.configData.GenerateCert ();
                
                // Check global min date allowed
                if ( ! string.IsNullOrEmpty ( global.MinDate ) &&
                     DateTime.Compare ( DateTime.ParseExact ( global.MinDate, "MM/dd/yyyy", null ), DateTime.Today ) < 0 )
                    throw new DeviceMinDateAllowedException ();
            }
            catch ( Exception e )
            {
                Console.WriteLine ( "Remove Config.Files.." );
                Console.WriteLine ( "Num [ before ]: " + Directory.GetFiles ( Mobile.GetPathConfig () ).Length );

                // For the moment, the approach is to delete the configuration files if an exception occurs
                foreach ( string filePath in Directory.GetFiles ( Mobile.GetPathConfig () ) )
                {
                    File.Delete ( filePath );
                    Console.WriteLine ( "- " + filePath + ": " + File.Exists ( filePath ) );
                }
                
                Console.WriteLine ( "Num [ after ]: " + Directory.GetFiles ( Mobile.GetPathConfig () ).Length );

                if ( Errors.IsOwnException ( e ) )
                    throw e;
                else if ( e is FileNotFoundException )
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
