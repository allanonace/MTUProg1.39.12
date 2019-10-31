using System;
using System.Collections.Generic;
using System.IO;
using Xml;
using Library.Exceptions;
using System.Xml.Serialization;
using Library;
using System.Linq;
using System.Text;

using ActionType = MTUComm.Action.ActionType;
using System.Diagnostics;

namespace MTUComm
{
    public class Configuration
    {
        private const string XML_MTUS      = "mtu.xml";
        private const string XML_METERS    = "meter.xml";
        public const string XML_GLOBAL     = "global.xml";
        private const string XML_INTERFACE = "Interface.xml";
        private const string XML_ALARMS    = "alarm.xml";
        private const string XML_DEMANDS   = "demandconf.xml";
        private const string XML_USERS     = "user.xml";

        public Global Global { private set; get; }

        private string device;
        private string deviceUUID;
        private string version;
        private string appName;
        public AlarmList Alarms { get; }

        public MtuTypes MtuTypes { get; set; }
        public MeterTypes MeterTypes { get; set; }
        public InterfaceConfig Interfaces { get; set; }
        public User[] Users { get; set; }
        public DemandConf Demands { get; set; }

        private Configuration ( string customPath = "", bool avoidXmlError = false )
        {
            Data.Set ( "UNIT_TEST", ! string.IsNullOrEmpty ( customPath ) );
            string configPath = ( ! Data.Get.UNIT_TEST ) ? Mobile.ConfigPath : customPath;

            device = "PC";

            try
            {
                // Load configuration files ( xml's )

                MtuTypes   = Utils.DeserializeXml<MtuTypes>        ( Path.Combine ( configPath, XML_MTUS      ) );
                MeterTypes = Utils.DeserializeXml<MeterTypes>      ( Path.Combine ( configPath, XML_METERS    ) );
                Global     = Utils.DeserializeXml<Global>          ( Path.Combine ( configPath, XML_GLOBAL    ) );
                Alarms     = Utils.DeserializeXml<AlarmList>       ( Path.Combine ( configPath, XML_ALARMS    ) );
                Demands    = Utils.DeserializeXml<DemandConf>      ( Path.Combine ( configPath, XML_DEMANDS   ) );
                Users      = Utils.DeserializeXml<UserList>        ( Path.Combine ( configPath, XML_USERS     ) ).List;
                
                Interfaces = Utils.DeserializeXml<InterfaceConfig> ( XML_INTERFACE, true ); // From resources
                
                // Preload port types, because some ports use a letter but other a list of Meter IDs
                // Done here because Xml project has no reference to MTUComm ( cross references )
                List<string> portTypes;
                StringBuilder allTypes = new StringBuilder ();
                foreach ( Mtu mtu in MtuTypes.Mtus )
                {
                    foreach ( Port port in mtu.Ports )
                    {
                       
                        if (string.IsNullOrEmpty(port.Type))
                            throw new PortTypeMissingMTUException(mtu.Id.ToString());

                        bool isNumeric = MtuAux.GetPortTypes ( port.Type, out portTypes );

                        // Some Meters have numeric type ( e.g. 122 ) and some of them appears
                        // twice in meter.xml, one for a Meter ID and other for a Meter type
                        port.IsSpecialCaseNumType = MeterTypes.ContainsNumericType ( portTypes[ 0 ] );

                        // Set if this Mtu only supports certain Meter IDs
                        if ( isNumeric &&
                             ! port.IsSpecialCaseNumType )
                            port.CertainMeterIds.AddRange ( portTypes );

                        // Type is string or is an special numeric case ( e.g. 122, 123,... )
                        if ( ! isNumeric ||
                             port.IsSpecialCaseNumType )
                            port.TypeString = string.Join ( string.Empty, portTypes );
                        
                        // Type is a number or list of numbers/IDs supported
                        // Recover Meter searching for the first supported Meter and get its type
                        else
                        {
                            foreach ( string id in portTypes )
                            {
                                string types = MeterTypes.FindByMterId ( int.Parse ( id ) ).Type;

                                // Get all different types from all supported Meters
                                // Type 1: ABC
                                // Type 2: DRE
                                // Type 3: MFR
                                // Type 4: ACC
                                // Type 5: ROL
                                // Result: ABCDREMFOL
                                foreach ( char c in types.ToList ().Except ( allTypes.ToString ().ToList () ) )
                                    allTypes.Append ( c );
                            }

                            port.TypeString = allTypes.ToString ();
                            allTypes.Clear ();
                        }
                        
                       // Utils.Print ( "MTU " + mtu.Id + ": Type " + port.TypeString );
                    }
                }
                allTypes = null;

                // Regenerate certificate from base64 string
                Mobile.configData.GenerateCertFromStore();
                //Mobile.configData.GenerateCert ();
                //Mobile.configData.LoadCertFromKeychain ();
                
                // Check global min date allowed
                if ( ! string.IsNullOrEmpty ( Global.MinDate ) &&
                     DateTime.Compare ( DateTime.ParseExact ( Global.MinDate, "MM/dd/yyyy", null ), DateTime.Today ) < 0 )
                    throw new DeviceMinDateAllowedException ();
            }
            catch ( Exception e )
            {
                if ( ! avoidXmlError )
                {
                    if (Errors.IsOwnException(e))
                        throw e;
                    else if (e is FileNotFoundException)
                        throw new ConfigurationFilesNotFoundException();
                    else
                    {
                        throw new ConfigurationFilesCorruptedException();
                    }
                }
            }
        }

        public static Configuration GetInstanceWithParams ( string path = "", bool avoidXmlError = false )
        {
            if ( ! Singleton.Has<Configuration> () )
            {
                Singleton.Set = new Configuration ( path, avoidXmlError );

                // NOTE: It is not possible to use Mobile static property through an instance
                //Singleton.Set = new Mobile ();
                Data.Set ( "ConfigPath", Mobile.ConfigPath );
                Data.Set ( "XmlGlobal", Configuration.XML_GLOBAL );
            }

            return Singleton.Get.Configuration;
        }

        public static bool CheckPortTypesMTU(MtuTypes mtuTypes, MeterTypes meterTypes)
        {
            try
            {
                // Preload port types, because some ports use a letter but other a list of Meter IDs
                // Done here because Xml project has no reference to MTUComm ( cross references )
                List<string> portTypes;
                StringBuilder allTypes = new StringBuilder();
                foreach (Mtu mtu in mtuTypes.Mtus)
                {
                    foreach (Port port in mtu.Ports)
                    {

                        if (string.IsNullOrEmpty(port.Type))
                            //throw new PortTypeMissingMTUException(mtu.Id.ToString());
                            return false;

                        bool isNumeric = MtuAux.GetPortTypes(port.Type, out portTypes);

                        // Some Meters have numeric type ( e.g. 122 ) and some of them appears
                        // twice in meter.xml, one for a Meter ID and other for a Meter type
                        port.IsSpecialCaseNumType = meterTypes.ContainsNumericType(portTypes[0]);

                        // Set if this Mtu only supports certain Meter IDs
                        if (isNumeric &&
                             !port.IsSpecialCaseNumType)
                            port.CertainMeterIds.AddRange(portTypes);

                        // Type is string or is an special numeric case ( e.g. 122, 123,... )
                        if (!isNumeric ||
                             port.IsSpecialCaseNumType)
                            port.TypeString = string.Join(string.Empty, portTypes);

                        // Type is a number or list of numbers/IDs supported
                        // Recover Meter searching for the first supported Meter and get its type
                        else
                        {
                            foreach (string id in portTypes)
                            {
                                string types = meterTypes.FindByMterId(int.Parse(id)).Type;

                                // Get all different types from all supported Meters
                                // Type 1: ABC
                                // Type 2: DRE
                                // Type 3: MFR
                                // Type 4: ACC
                                // Type 5: ROL
                                // Result: ABCDREMFOL
                                foreach (char c in types.ToList().Except(allTypes.ToString().ToList()))
                                    allTypes.Append(c);
                            }

                            port.TypeString = allTypes.ToString();
                            allTypes.Clear();
                        }

                        // Utils.Print ( "MTU " + mtu.Id + ": Type " + port.TypeString );
                    }
                }
                allTypes = null;
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }
        public static bool CheckLoadXML()
        {
            string configPath = Mobile.ConfigPath;
            try
            {
                // Load configuration files ( xml's )
                MtuTypes   auxMtus   = Utils.DeserializeXml<MtuTypes>   ( Path.Combine(configPath, XML_MTUS    ) );
                MeterTypes auxMeters = Utils.DeserializeXml<MeterTypes> ( Path.Combine(configPath, XML_METERS  ) );
                Global     auxGlobal = Utils.DeserializeXml<Global>     ( Path.Combine(configPath, XML_GLOBAL  ) );
                AlarmList  auxAlarm  = Utils.DeserializeXml<AlarmList>  ( Path.Combine(configPath, XML_ALARMS  ) );
                DemandConf auxDemand = Utils.DeserializeXml<DemandConf> ( Path.Combine(configPath, XML_DEMANDS ) );
                User[]     auxUsers  = Utils.DeserializeXml<UserList>   ( Path.Combine(configPath, XML_USERS   ) ).List;

                //check port types
                return CheckPortTypesMTU(auxMtus, auxMeters);

            }
            catch (Exception )
            {
                //throw new ConfigurationFilesCorruptedException();
                return false;
            }
        }

        public Mtu[] GetMtuTypes()
        {
            return MtuTypes.Mtus.ToArray();
        }

        public Mtu GetMtuTypeById ( int mtuId )
        {
            Mtu mtu = MtuTypes.FindByMtuId ( mtuId );
            
            // Is not valid MTU ID ( not present in Mtu.xml )
            if ( mtu == null )
                Errors.LogErrorNow ( new MtuMissingException () );
            
            return mtu;
        }

        public Meter[] GetMeterType()
        {
            return MeterTypes.Meters.ToArray();
        }

        public MeterTypes GetMeterTypes()
        {
            return MeterTypes;
        }

        public Meter getMeterTypeById(int meterId)
        {
            return MeterTypes.FindByMterId(meterId);
        }

        public InterfaceParameters[] getAllParamsFromInterface ( Mtu mtu, ActionType actionType )
        {
            return Interfaces.GetInterfaceByMtuIdAndAction ( mtu, actionType.ToString () ).getAllParams ();
        }

        public InterfaceParameters[] getLogParamsFromInterface ( Mtu mtu, ActionType actionType )
        {
            return Interfaces.GetInterfaceByMtuIdAndAction ( mtu, actionType.ToString () ).getLogParams ();
        }

        public InterfaceParameters[] getUserParamsFromInterface ( Mtu mtu, ActionType actionType )
        {
            return Interfaces.GetInterfaceByMtuIdAndAction ( mtu, actionType.ToString () ).getUserParams ();
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
            catch (Exception )
            {
                return null;
            }
        }

        public MemRegister getFamilyRegister(string family, string regsiter_name)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer ( typeof ( MemRegisterList ) );
                
                using ( TextReader reader = Utils.GetResourceStreamReader ( "family_" + family + ".xml" ) )
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
            }catch (Exception e ) { Console.WriteLine($"configuracion.cs_ {e.Message}"); }

            return null;
        }

        public List<string>  GetVendorsFromMeters()
        {
            return MeterTypes.GetVendorsFromMeters(MeterTypes.Meters);
        }

        public List<string> GetModelsByVendorFromMeters(String vendor)
        {
            return MeterTypes.GetModelsByVendorFromMeters(MeterTypes.Meters, vendor);
        }

        public Boolean useDummyDigits()
        {
            return !Global.LiveDigitsOnly;
        }

        public String GetDeviceUUID()
        {
            string return_str;

            return_str = deviceUUID;

            return return_str; //get UUID from Xamarin
        }

        public String GetApplicationVersion()
        {

            string return_str = "";

            if (device.Equals("Android") || device.Equals("iOS"))
            {
                return_str = version;
            }

            return return_str; //get UUID from Xamarin

        }


        public DemandConf Demands
        {
            get
            {
                return this.demands;
            }
        }

        public String getApplicationName()
        {

            string return_str = "";

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
