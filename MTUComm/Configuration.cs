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
        #region Constants

        private const string XML_MTUS      = "mtu.xml";
        private const string XML_METERS    = "meter.xml";
        public const string XML_GLOBAL     = "global.xml";
        private const string XML_INTERFACE = "Interface.xml";
        private const string XML_ALARMS    = "alarm.xml";
        private const string XML_DEMANDS   = "demandconf.xml";
        private const string XML_USERS     = "user.xml";

        #endregion

        #region Attributes

        public Global Global { private set; get; }

        private string device;
        private string deviceUUID;
        private string version;
        private string appName;

        public MtuTypes MtuTypes { get; private set; }
        public MeterTypes MeterTypes { get; private set; }
        public InterfaceConfig Interfaces { get; private set; }
        public User[] Users { get; private set; }
        public DemandConf Demands { get; private set; }
        public AlarmList Alarms { get; private set; }

        #endregion

        #region Initialization

        private Configuration (
            string customPath = "" )
        {
            Data.Set ( "UNIT_TEST", ! string.IsNullOrEmpty ( customPath ) );
            string configPath = ( ! Data.Get.UNIT_TEST ) ? Mobile.ConfigPath : customPath;

            device = "PC";
        }

        private void Initialize (
            bool avoidXmlError = false )
        {
            try
            {
                // Loads configuration files ( xml's ) and
                // preloads important information for the hardware ( MTU and Meters )
                LoadAndVerifyXMLs ();

                // Regenerate certificate from base64 string
                Mobile.ConfData.GenerateCertFromStore ();
                //Mobile.configData.GenerateCert ();
                //Mobile.configData.LoadCertFromKeychain ();
                
                // Check global min date allowed
                if ( ! string.IsNullOrEmpty ( Global.MinDate ) &&
                     DateTime.Compare ( DateTime.ParseExact ( Global.MinDate, "MM/dd/yyyy", null ), DateTime.Today ) < 0 )
                    throw new DeviceMinDateAllowedException ();
            }
            catch ( Exception e ) when ( Data.SaveIfDotNetAndContinue ( e ) )
            {
                if ( ! avoidXmlError )
                {
                    if ( Errors.IsOwnException ( e ) )
                        throw e;
                    else if ( e is FileNotFoundException )
                        throw new ConfigurationFilesNotFoundException ();
                    else
                        throw new ConfigurationFilesCorruptedException ();
                }
            }
        }

        public static Configuration GetInstanceWithParams ( string path = "", bool avoidXmlError = false )
        {
            if ( ! Singleton.Has<Configuration> () )
            {
                Singleton.Set = new Configuration ( path );
                Singleton.Get.Configuration.Initialize ( avoidXmlError );

                // NOTE: It is not possible to use Mobile static property through an instance
                //Singleton.Set = new Mobile ();
                Data.Set ( "ConfigPath", Mobile.ConfigPath );
                Data.Set ( "XmlGlobal", Configuration.XML_GLOBAL );
            }

            return Singleton.Get.Configuration;
        }

        public static bool LoadAndVerifyXMLs ()
        {
            string configPath = Mobile.ConfigPath;
            MtuTypes   tempMtuTypes;
            MeterTypes tempMeterTypes;
            Global     tempGlobal;
            AlarmList  tempAlarms;
            DemandConf tempDemands;
            User[]     tempUsers;
            InterfaceConfig tempInterface;

            try
            {
                // Load configuration files ( xml's )
                tempMtuTypes   = Utils.DeserializeXml<MtuTypes>   ( Path.Combine(configPath, XML_MTUS    ) );
                tempMeterTypes = Utils.DeserializeXml<MeterTypes> ( Path.Combine(configPath, XML_METERS  ) );
                tempGlobal     = Utils.DeserializeXml<Global>     ( Path.Combine(configPath, XML_GLOBAL  ) );
                tempAlarms     = Utils.DeserializeXml<AlarmList>  ( Path.Combine(configPath, XML_ALARMS  ) );
                tempDemands    = Utils.DeserializeXml<DemandConf> ( Path.Combine(configPath, XML_DEMANDS ) );
                tempUsers      = Utils.DeserializeXml<UserList>   ( Path.Combine(configPath, XML_USERS   ) ).List;
                tempInterface  = Utils.DeserializeXml<InterfaceConfig> ( XML_INTERFACE, true ); // From resources

                // Preload important information for the hardware ( MTU and Meters )
                PreloadHardwareInfo ( tempMtuTypes, tempMeterTypes );
            }
            catch ( Exception )
            {
                //throw new ConfigurationFilesCorruptedException ();
                return false;
            }

            Configuration config = Singleton.Get.Configuration;

            // All configuration files are OK
            config.MtuTypes   = tempMtuTypes;
            config.MeterTypes = tempMeterTypes;
            config.Global     = tempGlobal;
            config.Alarms     = tempAlarms;
            config.Demands    = tempDemands;
            config.Users      = tempUsers;
            config.Interfaces = tempInterface;

            // Set ammounts ( attempts and time ) to configure the LExI communication
            Lexi.Lexi.LexiMaxAttempts = config.Global.LexiAttempts;
            Lexi.Lexi.LexiMaxTimeout  = config.Global.LexiTimeout;

            return true;
        }

        private static void PreloadHardwareInfo (
            MtuTypes   mtuTypes   = null,
            MeterTypes meterTypes = null )
        {
            #region Meter Utilities

            // Lists all utilities for each Meter type
            // Structure: <Type,Utilities>
            /*
            At the moment - ACL_PRO_2|3|4 ( OnDemand 1.2 )..
            m      : gas
            r      : gas, water, electric, norgas, steam
            p      : gas, water
            e      : water, gas, mlog
            setflow: water
            g      : water
            l      : electric
            k      : gas
            i      : electric
            w      : water
            t      : water
            b      : water
            75     : electric
            74     : electric
            91     : electric
            122    : gas
            123    : gas
            124    : gas
            125    : gas
            95     : electric
            167    : electric
            101    : electric
            92     : electric
            129    : electric
            */
            Dictionary<string,List<string>> utilities = new Dictionary<string,List<string>>();
            foreach ( Meter meter in meterTypes.Meters )
            {
                string type    = meter.Type.ToLower ();
                string utility = meter.Utility.ToLower ();

                // Special cases ( predefined string or numerical Meter Type )
                if ( type.Equals ( "setflow" ) ||
                     int.TryParse (type, out int ok ) )
                {
                    if ( ! utilities.ContainsKey ( type ) )
                        utilities.Add ( type, new List<string> () );

                    if ( ! utilities[ type ].Contains ( utility ) )
                        utilities[ type ].Add ( utility );
                }
                // Chars
                else
                {
                    if ( ! utilities.ContainsKey ( type ) )
                        utilities.Add ( type, new List<string> () );

                    if ( ! utilities[ type ].Contains ( utility ) )
                        utilities[ type ].Add ( utility );
                }
            }

            #endregion

            #region Get real Port Type and Utility 
            
            // Preload port types, because some ports use a letter but other a list of Meter IDs
            // Done here because Xml project has no reference to MTUComm ( cross references )
            List<string> portTypes;
            StringBuilder allTypes = new StringBuilder ();
            foreach ( Mtu mtu in mtuTypes.Mtus )
            {
                foreach ( Port port in mtu.Ports )
                {
                    if ( string.IsNullOrEmpty ( port.Type ) )
                        throw new PortTypeMissingMTUException ( mtu.Id.ToString () );

                    // Returns...
                    // Option A: Meter ID/s
                    //           One id alone or multiple ( e.g. "3101" or "3101|3102|3103" )
                    // Option B: Meter Type/s
                    //           We assume that will be one special numerical meter type ( e.g. <Type>122</Type> )
                    //           or a list of characters ( e.g. <Type>MR</Type> )
                    // Option C: Special predefined string
                    //           Such as "SETFLOW" or "GUT"
                    bool isNumeric = MtuAux.GetPortTypes ( port.Type.ToLower (), out portTypes );

                    // FIXME: Avoids MTU compatible with predefined Meter types ( GUT, S4K,.. ), except SETFLOW
                    if ( MtuAux.IsPredefinedType ( portTypes[ 0 ] ) &&
                         ! portTypes[ 0 ].Equals ( "setflow" ) )
                        continue;

                    // Some Meters have numeric type and some of them appears twice
                    // in meter.xml, one for a Meter ID and other for a Meter type
                    // NOTE: Use only the first one for the detection process
                    port.IsSpecialCaseNumType = meterTypes.ContainsNumericType ( portTypes[ 0 ] );

                    // This information will be used during the family assignment for each MTU to be used
                    if ( isNumeric )
                    {
                        // Use first Meter ID to retrieve its type
                        port.Utilities = utilities[ meterTypes.FindByMterId ( int.Parse ( portTypes[ 0 ] ) ).Type.ToLower () ];
                    }
                    else
                    {
                        // Use the Meter type directly
                        port.Utilities = utilities[ portTypes[ 0 ] ];
                    }

                    // Set if this Mtu only supports certain Meter IDs ( e.g. <Meter ID="122"> and there is no <Type>122</Type> )
                    if ( isNumeric &&
                         ! port.IsSpecialCaseNumType )
                        port.CertainMeterIds.AddRange ( portTypes );

                    // The Meter type is string or is a special numerical case ( e.g. <Type>MR</Type> or <Type>122</Type> )
                    if ( ! isNumeric ||
                         port.IsSpecialCaseNumType )
                        port.TypeString = string.Join ( string.Empty, portTypes ).ToUpper ();
                    
                    // Type is a number or list of IDs supported
                    else
                    {
                        foreach ( string id in portTypes )
                        {
                            string types = meterTypes.FindByMterId ( int.Parse ( id ) ).Type;

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

                        port.TypeString = allTypes.ToString ().ToUpper ();
                        allTypes.Clear ();
                    }
                }
            }

            allTypes = null;

            #endregion
        }

        #endregion

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
