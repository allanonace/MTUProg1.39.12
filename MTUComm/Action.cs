using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lexi.Interfaces;
using Library;
using MTUComm.actions;
using Xml;
using Library.Exceptions;
using System.IO;

namespace MTUComm
{
    public class Action
    {
        #region Nested class

        private class ConditionObjet
        {
            public String Condition { get; private set; }
            public String Operator  { get; private set; }
            public String Key       { get; private set; } // Can be Id or Id.Property
            public String Value     { get; private set; }

            public bool IsEqual   { get { return this.Operator.Equals ( "=" ); } }
            public bool IsLower   { get { return this.Operator.Equals ( "lt" ); } }
            public bool IsGreater { get { return this.Operator.Equals ( "gt" ); } }
            public bool IsNot     { get { return this.Operator.Equals ( "!" ); } }

            public ConditionObjet (
                string condition,
                string key,
                string conditionOperator,
                string value )
            {
                if ( ! condition.Equals ( IFACE_AND ) &&
                     ! condition.Equals ( IFACE_OR ) )
                     Condition = IFACE_OR;
                else Condition = condition;

                this.Operator = conditionOperator;
                this.Key      = key;
                this.Value    = value;
            }
        }

        #endregion

        #region Constants

        private const string NET_IDS   = @"[_a-zA-Z][_a-zA-Z0-9]+";

        // [ + or | ] Type/Class[.Property] +|<|>|! Value -> e.g. MemoryMap.ShipBit=false|Mtu.InterfaceTamper=true
        private const string REGEX_IFS = @"(\+|\|)?(" + NET_IDS + @"(?:." + NET_IDS + @")?)(=|lt|gt|!)(-?[0-9]+|[_a-zA-Z0-9]+)";

        private const string IFACE_AND       = "+";
        private const string IFACE_OR        = "|";
        private const string PORT_PREFIX     = "P";
        private const string IFACE_PORT      = "Port";
        private const string IFACE_ACTION    = "Action";
        private const string IFACE_METER     = "Meter";
        private const string IFACE_MTU       = "Mtu";
        private const string IFACE_PUCK      = "Puck";
        private const string IFACE_GLOBAL    = "Global";
        private const string IFACE_MEMORYMAP = "MemoryMap";
        private const string IFACE_FORM      = "Form";
        private const string IFACE_DATA      = "Data";
        private const string IFACE_MREADING  = "MeterReading";
        private const string IFACE_READERROR = "ReadingError";
        
        public enum ActionType
        {
            ReadMtu,
            AddMtu,
            ReplaceMTU,
            AddMtuAddMeter,
            AddMtuReplaceMeter,
            ReplaceMtuReplaceMeter,
            ReplaceMeter,
            TurnOffMtu,
            TurnOnMtu,
            DataRead,
            MtuInstallationConfirmation,
            Diagnosis,
            BasicRead,
            ReadFabric
        }

        public static Dictionary<ActionType, String> displays = new Dictionary<ActionType, String>()
        {
            {ActionType.BasicRead,"Basic Read" },
            {ActionType.ReadMtu,"Read MTU" },
            {ActionType.AddMtu,"Add MTU" },
            {ActionType.ReplaceMTU,"Replace MTU" },
            {ActionType.AddMtuAddMeter,"Add MTU/Meter" },
            {ActionType.AddMtuReplaceMeter,"Add MTU/Replace Meter" },
            {ActionType.ReplaceMtuReplaceMeter,"Replace MTU/Meter" },
            {ActionType.ReplaceMeter,"Replace Meter" },
            {ActionType.TurnOffMtu,"Turn Off MTU" },
            {ActionType.TurnOnMtu,"Turn On MTU" },
            {ActionType.DataRead,"Read Data Log" },
            {ActionType.MtuInstallationConfirmation,"Install Confirmation" },
            {ActionType.Diagnosis, string.Empty },
            {ActionType.ReadFabric, "Read Fabric" }
        };

        public static Dictionary<ActionType, String> tag_types = new Dictionary<ActionType, String>()
        {
            {ActionType.BasicRead,"BasicRead" },
            {ActionType.ReadMtu,"ReadMTU" },
            {ActionType.AddMtu,"Program MTU" },
            {ActionType.ReplaceMTU,"Program MTU" },
            {ActionType.AddMtuAddMeter,"Program MTU" },
            {ActionType.AddMtuReplaceMeter,"Program MTU" },
            {ActionType.ReplaceMtuReplaceMeter,"Program MTU" },
            {ActionType.ReplaceMeter,"Program MTU" },
            {ActionType.TurnOffMtu,"TurnOffMtu" },
            {ActionType.TurnOnMtu,"TurnOnMtu" },
            {ActionType.DataRead, "ReadDataLog" },
            {ActionType.MtuInstallationConfirmation,"InstallConfirmation" },
            {ActionType.Diagnosis, string.Empty },
            {ActionType.ReadFabric, "ReadFabric" }
        };

        public static Dictionary<ActionType, String> tag_reasons = new Dictionary<ActionType, String>()
        {
            {ActionType.BasicRead, "BasicRead" },
            {ActionType.ReadMtu, "ReadMtu" },
            {ActionType.AddMtu,"AddMtu" },
            {ActionType.ReplaceMTU,"ReplaceMtu" },
            {ActionType.AddMtuAddMeter,"AddMtuAddMeter" },
            {ActionType.AddMtuReplaceMeter,"AddMtuReplaceMeter" },
            {ActionType.ReplaceMtuReplaceMeter,"ReplaceMtuReplaceMeter" },
            {ActionType.ReplaceMeter,"ReplaceMeter" },
            {ActionType.TurnOffMtu, "TurnOffMtu" },
            {ActionType.TurnOnMtu, "TurnOnMtu" },
            {ActionType.DataRead, "DataRead" },
            {ActionType.MtuInstallationConfirmation,"InstallConfirmation" },
            {ActionType.Diagnosis, string.Empty },
            {ActionType.ReadFabric, "ReadFabric" }
        };

        #endregion

        #region Events and Delegates

        public delegate void ActionProgresshHandler(object sender, ActionProgressArgs e);
        public event ActionProgresshHandler OnProgress;

        public delegate void ActionFinishHandler(object sender, ActionFinishArgs e);
        public event ActionFinishHandler OnFinish;

        public delegate void ActionErrorHandler ();
        public event ActionErrorHandler OnError;

        #endregion

        #region Args

        public class ActionProgressArgs : EventArgs
        {
            public int Step { get; private set; }
            public int TotalSteps { get; private set; }
            public string Message { get; private set; }

            public ActionProgressArgs(int step, int totalsteps)
            {
                Step = step;
                TotalSteps = totalsteps;
                Message = "";
            }

            public ActionProgressArgs(int step, int totalsteps, string message)
            {
                Step = step;
                TotalSteps = totalsteps;
                Message = message;
            }
        }

        public class ActionFinishArgs : EventArgs
        {
            public ActionResult Result { get; private set; }
            public AddMtuLog FormLog;
            public Mtu Mtu;

            public ActionFinishArgs(ActionResult result )
            {
                Result = result;
            }
        }

        public class ActionErrorArgs : EventArgs
        {
            public ActionErrorArgs () { }

            public int Status { get; private set; }

            public String Message { get; private set; }

            public ActionErrorArgs(int status, String message)
            {
                Status = status;
                Message = message;
            }

            public ActionErrorArgs(String message)
            {
                Status = -1;
                Message = message;
            }
        }

        #endregion

        #region Attributes
        
        private string lastLogCreated;

        private Configuration config;
        public Mtu CurrentMtu { private set; get; }
        public MTUComm comm { get; private set; }
        public ActionType type { get; }
        private List<Parameter> mparameters = new List<Parameter>();
        private List<Parameter> additionalParameters = new List<Parameter>();
        private Boolean canceled = false;
        public  String user { get; private set; }
        public  Logger logger;
        private Configuration configuration;
        private List<Action> sub_actions = new List<Action>();
        public  int order = 0;
        //public Func<object, object, object> OnFinish;
        
        #endregion

        #region Properties

        public List<Parameter> AdditionalParameters
        {
            get { return this.additionalParameters; }
        }

        public String DisplayText
        {
            get { return displays[type]; }
        }

        public String LogText
        {
            get { return tag_types[this.type]; }
        }

        public String Reason
        {
            get { return tag_reasons[this.type]; }
        }

        public bool IsWrite
        {
            get
            {
                return this.type == ActionType.AddMtu             ||
                       this.type == ActionType.AddMtuAddMeter     ||
                       this.type == ActionType.AddMtuReplaceMeter ||
                       this.type == ActionType.ReplaceMTU         ||
                       this.type == ActionType.ReplaceMeter       ||
                       this.type == ActionType.ReplaceMtuReplaceMeter;
            }
        }
        
        public bool IsReplace
        {
            get
            {
                return this.type == ActionType.AddMtuReplaceMeter ||
                       this.type == ActionType.ReplaceMTU         ||
                       this.type == ActionType.ReplaceMeter       ||
                       this.type == ActionType.ReplaceMtuReplaceMeter;
            }
        }
        
        public bool IsInstallConfirmation
        {
            get
            {
                return this.type == ActionType.MtuInstallationConfirmation;
            }
        }
        
        public bool IsRead
        {
            get
            {
                return this.type == ActionType.ReadMtu;
            }
        }
        
        public bool IsTurnOnOff
        {
            get
            {
                return this.type == ActionType.TurnOnMtu ||
                       this.type == ActionType.TurnOffMtu;
            }
        }

        #endregion

        #region Initialization

        public Action(Configuration config, ISerial serial, ActionType type, String user = "", String outputfile = "" )
        {
            // outputfile = new FileInfo ( outputfile ).Name; // NO
            // System.IO.Path.GetFileName(outputfile)); // NO

            configuration = config;
            logger = new Logger(outputfile.Substring(outputfile.LastIndexOf('\\') + 1) ); 
            comm = new MTUComm(serial, config);
            this.type = type;
            this.user = user;
            comm.OnError += Comm_OnError;
            
            this.config = Singleton.Get.Configuration;

            this.additionalParameters = new List<Parameter> ();
            
            // Only save reference for the current action,
            // not for nested or auxiliary actions ( as BasicRead )
            if ( this.type != ActionType.BasicRead &&
                 ! Singleton.Has<Action> () )
            {
                Singleton.Set = this;
                
                // Reset temp data but not all data, because some data is created
                // only one time and is needed during the life of the application
                Data.Reset ();
            }
        }
        
        public void SetCurrentMtu (
            MTUBasicInfo mtuBasic )
        {
            CurrentMtu = this.config.GetMtuTypeById ( ( int )mtuBasic.Type );
        }

        #endregion

        #region Parameters

        public void AddParameter (Parameter parameter)
        {
            mparameters.Add(parameter);
        }

        public void AddAdditionalParameter ( Parameter parameter )
        {
            this.additionalParameters.Add ( parameter );
        }

        public void AddParameter ( MtuForm form )
        {
            Parameter[] addMtuParams = form.GetParameters ();
            foreach ( Parameter parameter in addMtuParams )
                mparameters.Add (parameter);
        }

        public Parameter[] GetParameters()
        {
            return mparameters.ToArray();
        }

        public Parameter GetParameterByTag(string tag, int port = -1)
        {
            return mparameters.Find(x => x.getLogTag().Equals(tag) && ( port == -1 || x.Port == port ) );
        }

        #endregion

        #region Actions

        public void AddActions(Action action)
        {
            sub_actions.Add(action);
        }

        public Action[] GetSubActions()
        {
            return sub_actions.ToArray();
        }

        #endregion

        #region Gets

        public String GetProperty ( string name )
        {
            switch (name)
            {
                case "User": return user;
                case "Date": return DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");
                case "Type": return type.ToString();
                default    : return "";
            }
        }

        public String GetResultXML ( ActionResult result )
        {
            return this.lastLogCreated;
        }

        #endregion

        #region Execution

        public void Run(MtuForm mtuForm = null)
        {
            if (canceled)
            {
                throw new Exception("Canceled Action can not be Executed");
            }
            else
            {
                List<object> parameters = new List<object>();

                comm.OnProgress -= Comm_OnProgress;
                comm.OnProgress += Comm_OnProgress;

                switch (type)
                {
                    case ActionType.ReadFabric:
                        comm.OnReadFabric -= Comm_OnReadFabric;
                        comm.OnReadFabric += Comm_OnReadFabric;
                        break;

                    case ActionType.ReadMtu:
                    case ActionType.MtuInstallationConfirmation:
                        //comm.OnReadMtu -= Comm_OnReadMtu;
                        //comm.OnReadMtu += Comm_OnReadMtu;

                        // >>>> DEBUG <<<<<<<<<<<<<<<<<<<<<<<<<<
                        parameters.Add ( 1 );
                        comm.OnDataRead -= Comm_OnDataReadEvent;
                        comm.OnDataRead += Comm_OnDataReadEvent;
                        // >>>> DEBUG <<<<<<<<<<<<<<<<<<<<<<<<<<
                        break;
                       
                    case ActionType.AddMtu:
                    case ActionType.AddMtuAddMeter:
                    case ActionType.AddMtuReplaceMeter:
                    case ActionType.ReplaceMTU:
                    case ActionType.ReplaceMeter:
                    case ActionType.ReplaceMtuReplaceMeter:
                        comm.OnAddMtu -= Comm_OnAddMtu;
                        comm.OnAddMtu += Comm_OnAddMtu;
                        // Interactive and Scripting
                        if ( mtuForm != null )
                             parameters.AddRange ( new object[] { (AddMtuForm)mtuForm, this.user, this } );
                        else parameters.Add ( this );
                        break;

                    case ActionType.TurnOffMtu:
                    case ActionType.TurnOnMtu:
                        comm.OnTurnOffMtu -= Comm_OnTurnOnOffMtu;
                        comm.OnTurnOffMtu += Comm_OnTurnOnOffMtu;
                        break;

                    case ActionType.DataRead:
                        parameters.AddRange ( new object[] { ( int ) 1 }); // TODO: Use new entry created in Library.Data
                        comm.OnDataRead -= Comm_OnDataReadEvent;
                        comm.OnDataRead += Comm_OnDataReadEvent;
                        break;

                    case ActionType.BasicRead:
                        comm.OnBasicRead -= Comm_OnBasicRead;
                        comm.OnBasicRead += Comm_OnBasicRead;
                        break;
                }

                // Is more easy to control one point of invokation
                // than N, one for each action/new task to launch
                comm.LaunchActionThread(type, parameters.ToArray());
            }
        }

        public void Cancel(string cancelReason = "410 DR Defective Register")
        {
            canceled = true;
            logger.Cancel ( this, "User Cancelled", cancelReason );
        }

        #endregion

        #region OnEvents

        private void Comm_OnProgress(object sender, MTUComm.ProgressArgs e)
        {
            try
            {
                OnProgress(this, new ActionProgressArgs(e.Step, e.TotalSteps, e.Message));
            }
            catch (Exception pe)
            {

            }
        }

        private void Comm_OnError ()
        {
            this.OnError ();

            // Reset current action reference
            Singleton.Remove<Action> ();
        }

        private async Task Comm_OnDataReadEvent ( object sender, MTUComm.DataReadArgs args )
        {
            try
            {
                // Mtu ID value formated
                dynamic map = args.MemoryMap;
                int mtuIdLength = Singleton.Get.Configuration.Global.MtuIdLength;
                string strMtuId = ( await map.MtuSerialNumber.GetValueFromMtu () ).ToString ().PadLeft ( mtuIdLength, '0' );
                Data.Set ( "MtuId", strMtuId );

                // Prepare custom values
                EventLogList eventList = args.ListEntries;

                Data.Set ( "ReadResult",
                    $"Number of Reads {eventList.Count} for Selected Period " +
                    $"From {eventList.DateStart.ToString ( "dd/MM/yyyy HH:mm:ss" )} " +
                    $"Till {eventList.DateEnd  .ToString ( "dd/MM/yyyy HH:mm:ss" )}" );

                // NOTE: STARProgrammer MtuComm.cs Line 5341
                string path = Path.Combine ( Mobile.EventPath,
                    $"MTUID{strMtuId}-{DateTime.Now.ToString ( "MMddyyyyHH" )}-" +
                    $"{ DateTime.Now.Ticks }DataLog.xml" );
                string subpath = path.Substring ( path.LastIndexOf ( "/" ) + 1 );
                Data.Set ( "ReadResultFileFull", path );
                Data.Set ( "ReadResultFile", subpath );

                // >>>> DEBUG <<<<<<<<<<<<<<<<<<<<<<<<<<
                Data.Set ( "MtuLocation", "Outside", true );
                Data.Set ( "Construction", "Wood", true );
                Data.Set ( "GpsLat", 12, true );
                Data.Set ( "GpsLon", 34, true );
                Data.Set ( "GpsAlt", 56, true );
                // >>>> DEBUG <<<<<<<<<<<<<<<<<<<<<<<<<<

                ActionResult dataRead_allParamsFromInterface = await CreateActionResultUsingInterface ( args.MemoryMap, args.Mtu, null, ActionType.DataRead );
                ActionResult readMtu_allParamsFromInterface  = await CreateActionResultUsingInterface ( args.MemoryMap, args.Mtu );
                this.lastLogCreated = logger.DataRead ( dataRead_allParamsFromInterface, readMtu_allParamsFromInterface, args.ListEntries, args.Mtu );
                ActionFinishArgs finalArgs = new ActionFinishArgs ( readMtu_allParamsFromInterface );
                finalArgs.Mtu = args.Mtu;
                
                this.Finish ( finalArgs );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        private async Task Comm_OnReadFabric(object sender)
        {
            this.Finish ( null );
        }

        private void Finish (
            ActionFinishArgs args )
        {
            OnFinish ( this, args );

            // Reset current action reference
            Singleton.Remove<Action> ();
        }

        private async Task Comm_OnReadMtu ( object sender, MTUComm.ReadMtuArgs args )
        {
            try
            {
                ActionResult resultAllInterfaces = await CreateActionResultUsingInterface ( args.MemoryMap, args.Mtu );
                this.lastLogCreated = logger.ReadMTU ( this, resultAllInterfaces, args.Mtu );
                ActionFinishArgs finalArgs = new ActionFinishArgs ( resultAllInterfaces );
                finalArgs.Mtu = args.Mtu;
                
                this.Finish ( finalArgs );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        private void Comm_OnTurnOnOffMtu ( object sender, MTUComm.TurnOffMtuArgs args )
        {
            try
            {
                ActionResult resultBasic = getBasciInfoResult ();
                this.lastLogCreated = logger.TurnOnOff ( this, args.Mtu, resultBasic );
                ActionFinishArgs finalArgs = new ActionFinishArgs ( resultBasic );

                this.Finish ( finalArgs );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        private async Task Comm_OnAddMtu ( object sender, MTUComm.AddMtuArgs args )
        {
            try
            {
                ActionResult result = await CreateActionResultUsingInterface ( args.MemoryMap, args.MtuType, args.Form );
                ActionFinishArgs finalArgs = new ActionFinishArgs ( result );
                args.AddMtuLog.LogReadMtu ( result );
                this.lastLogCreated = args.AddMtuLog.Save ();

                this.Finish ( finalArgs );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        private void Comm_OnBasicRead(object sender, MTUComm.BasicReadArgs e)
        {
            ActionResult result = new ActionResult();
            ActionFinishArgs finalArgs = new ActionFinishArgs(result);

            this.Finish ( finalArgs );
        }

        #endregion

        private ActionResult getBasciInfoResult ()
        {
            ActionResult result = new ActionResult ();
            MTUBasicInfo basic  = comm.GetBasicInfo ();
            
            result.AddParameter(new Parameter("Date", "Date/Time", GetProperty("Date")));
            result.AddParameter(new Parameter("User", "User", GetProperty("User")));
            result.AddParameter(new Parameter("MtuType", "MTU Type", basic.Type));
            result.AddParameter(new Parameter("MtuId", "MTU ID", basic.Id));
            
            foreach ( Parameter param in this.AdditionalParameters )
                result.AddParameter ( param );
    
            return result;
        }

        #region Interface

        private async Task<ActionResult> CreateActionResultUsingInterface (
            dynamic map  = null,
            Mtu     mtu  = null,
            MtuForm form = null,
            ActionType actionType = ActionType.ReadMtu )
        {
            Parameter paramToAdd;
            Global       global = this.config.Global;
            Puck         puck   = Singleton.Get.Puck;
            Type         gType  = global.GetType ();
            ActionResult result = new ActionResult ();
            InterfaceParameters[] parameters = configuration.getAllParamsFromInterface ( mtu, actionType );
            
            foreach ( InterfaceParameters parameter in parameters )
            {
                if ( parameter.Name.Equals ( IFACE_PORT ) )
                    for ( int i = 0; i < mtu.Ports.Count; i++ )
                        result.addPort ( await ReadPort ( i + 1, parameter.Parameters.ToArray (), map, mtu ) );
                else
                {
                    if ( await ValidateCondition ( parameter.Conditional, map, mtu ) )
                    {
                        string value          = string.Empty;
                        string sourceWhere    = string.Empty;
                        string sourceProperty = parameter.Name;

                        if ( ! string.IsNullOrEmpty ( parameter.Source ) &&
                                Regex.IsMatch ( parameter.Source, NET_IDS + "." + NET_IDS ) )
                        {
                            string[] sources = parameter.Source.Split(new char[] { '.' });
                            sourceWhere      = sources[ 0 ];
                            sourceProperty   = sources[ 1 ];
                        }

                        paramToAdd = null;
                        try
                        {
                            switch ( sourceWhere )
                            {
                                case IFACE_ACTION: value      = this .GetProperty  ( sourceProperty ); break; // Current action
                                case IFACE_MTU   : value      = mtu  .GetProperty  ( sourceProperty ); break; // Current MTU
                                case IFACE_PUCK  : value      = puck .GetProperty  ( sourceProperty ); break;
                                case IFACE_FORM  : paramToAdd = form .GetParameter ( sourceProperty ); break; // actions.AddMtuForm class
                                case IFACE_GLOBAL: value      = gType.GetProperty  ( sourceProperty ).GetValue ( global, null ).ToString(); break; // Global class
                                case IFACE_DATA  : value      = ( string.IsNullOrEmpty ( value = Data.Get[ sourceProperty ].ToString () ) ? string.Empty : value ); break; // Library.Data class
                                default          : value      = ( await map[ sourceProperty ].GetValue () ).ToString (); break; // MemoryMap.ParameterName
                            }
                        }
                        catch ( Exception e )
                        {
                            Utils.Print ( "Interface: Map Error: " + sourceProperty );
                            throw new Exception ();
                        }
                        
                        if ( ! sourceWhere.Equals ( IFACE_FORM ) &&
                             ! string.IsNullOrEmpty ( value ) )
                        {
                            string display = ( parameter.Display.ToLower ().StartsWith ( "global." ) ) ?
                                                gType.GetProperty ( parameter.Display.Split ( new char[] { '.' } )[ 1 ] ).GetValue ( global, null ).ToString () :
                                                parameter.Display;
                            
                            paramToAdd = new Parameter ( parameter.Name, display, value, parameter.Source );
                        }
                        // To change "name" attribute to show in IFACE_FORM case
                        else
                        {
                            paramToAdd.CustomParameter = parameter.Name;
                            paramToAdd.source          = parameter.Source;
                        }
                        
                        if ( paramToAdd != null )
                        {
                            if ( Utils.IsBool ( paramToAdd.Value ) )
                                 paramToAdd.Value = Utils.FirstCharToCapital ( paramToAdd.Value );
                            else paramToAdd.Value = this.FormatLength ( paramToAdd.Value, parameter.Length, parameter.Fill );
                            result.AddParameter ( paramToAdd );
                        }
                    }
                }
            }
            
            // Add additional parameters for all actions except for the Add
            if ( form == null )
                foreach ( Parameter param in this.AdditionalParameters )
                    result.AddParameter ( param );

            return result;
        }

        private async Task<ActionResult> ReadPort (
            int indexPort,
            InterfaceParameters[] parameters,
            dynamic map,
            Mtu mtu )
        {
            ActionResult result   = new ActionResult ();
            Port         portType = mtu.Ports[ indexPort - 1 ];
            Global       global   = this.config.Global;
            Type         gType    = global.GetType ();

            // Meter Serial Number
            int meterId = await map[ PORT_PREFIX + indexPort + "MeterType" ].GetValue ();

            // Port has installed a Meter
            if ( meterId > 0 )
            {
                Meter meter = configuration.getMeterTypeById ( meterId );
                
                // Meter type not found in database
                if ( meter.Type.Equals ( "NOTFOUND" ) )
                {
                    //logger.LogError("No valid meter types were found for MTU type " + Metertype.Id);
                    
                    return null;
                }

                // Iterate all parameters for this port
                foreach ( InterfaceParameters parameter in parameters )
                {
                    // Meter readings are treated in a special way
                    if ( parameter.Name.Equals ( IFACE_MREADING ) )
                    {
                        if ( await ValidateCondition ( parameter.Conditional, map, mtu, indexPort ) )
                        {
                            try
                            {
                                string meter_reading_error   = string.Empty;
                                string encoderReadingErrorId = PORT_PREFIX + indexPort + IFACE_READERROR;

                                bool mtuHasNotEncoderErrorCodes = ! map.ContainsMember ( encoderReadingErrorId );
                                
                                if ( ! mtuHasNotEncoderErrorCodes )
                                    meter_reading_error = ( await map[ encoderReadingErrorId ].GetValue () ).ToString ();

                                // Encoder MTUs have register to know if an error occurred while trying to read the Meter
                                if ( mtuHasNotEncoderErrorCodes ||
                                     meter_reading_error.Length < 1 )
                                {
                                    ulong meter_reading  = await map[ PORT_PREFIX + indexPort + IFACE_MREADING ].GetValue ();
                                    ulong tempReadingVal = ( ! mtu.PulseCountOnly ) ? meter_reading : meter_reading * ( ulong )meter.HiResScaling;
                                    
                                    String tempReading = tempReadingVal.ToString ();
                                    if ( meter.LiveDigits < tempReading.Length )
                                         tempReading = tempReading.Substring ( tempReading.Length - meter.LiveDigits - ( tempReading.IndexOf ('.') > -1 ? 1 : 0 ) );
                                    else tempReading = tempReading.PadLeft ( meter.LiveDigits, '0' );
                                    
                                    if ( meter.LeadingDummy > 0 )
                                        tempReading = tempReading.PadLeft (
                                            tempReading.Length + meter.LeadingDummy,
                                            configuration.useDummyDigits() ? 'X' : '0' );
                                        
                                    if ( meter.DummyDigits > 0 )
                                        tempReading = tempReading.PadRight (
                                            tempReading.Length + meter.DummyDigits,
                                            configuration.useDummyDigits() ? 'X' : '0' );
                                        
                                    if ( meter.Scale > 0 &&
                                         tempReading.IndexOf(".") == -1 )
                                        tempReading = tempReading.Insert ( tempReading.Length - meter.Scale, "." );
                                        
                                    if ( meter.PaintedDigits > 0 &&
                                         configuration.useDummyDigits () )
                                        tempReading = tempReading.PadRight (
                                            tempReading.Length + meter.PaintedDigits, '0' ).Insert ( tempReading.Length, " - " );
    
                                    if ( string.IsNullOrEmpty ( tempReading ) )
                                        tempReading = "INVALID";
                                    
                                    result.AddParameter ( new Parameter ( parameter.Name, parameter.Display, tempReading, parameter.Source, indexPort - 1 ) );
                                }
                                else
                                    result.AddParameter ( new Parameter ( parameter.Name, parameter.Display, meter_reading_error, parameter.Source, indexPort - 1 ) );
                            }
                            catch ( Exception e )
                            {
                                //...
                            }
                        }
                    }
                    else
                    {
                        if ( await ValidateCondition ( parameter.Conditional, map, mtu, indexPort ) )
                        {
                            string value          = string.Empty;
                            string sourceWhere    = string.Empty;
                            string sourceProperty = parameter.Name;

                            if ( ! string.IsNullOrEmpty ( parameter.Source ) &&
                                    Regex.IsMatch ( parameter.Source, NET_IDS + "." + NET_IDS ) )
                            {
                                string[] sources = parameter.Source.Split(new char[] { '.' });
                                sourceWhere      = sources[ 0 ];
                                sourceProperty   = sources[ 1 ];
                            }

                            try
                            {
                                switch ( sourceWhere )
                                {
                                    case IFACE_PORT  : value = portType.GetProperty ( sourceProperty ); break;
                                    case IFACE_MTU   : value = mtu     .GetProperty ( sourceProperty ); break;
                                    case IFACE_METER : value = meter   .GetProperty ( sourceProperty ); break;
                                    case IFACE_GLOBAL: value = gType   .GetProperty ( sourceProperty ).GetValue ( global, null ).ToString(); break; // Global class
                                    case IFACE_DATA  : value = ( string.IsNullOrEmpty ( value = Data.Get[ sourceProperty ].ToString () ) ? string.Empty : value ); break; // Library.Data class
                                    default          : value = ( await map[ sourceProperty = PORT_PREFIX + indexPort + sourceProperty ].GetValue () ).ToString (); break; // MemoryMap.ParameterName
                                }
                            }
                            catch ( Exception e )
                            {
                                Utils.Print ( "Interface: Map Error: " + sourceProperty );
                                throw new Exception ();
                            }
                            
                            if ( ! string.IsNullOrEmpty ( value ) )
                            {
                                if ( Utils.IsBool ( value ) )
                                     value = Utils.FirstCharToCapital ( value );
                                else value = this.FormatLength ( value, parameter.Length, parameter.Fill );

                                string display = ( parameter.Display.ToLower ().StartsWith ( "global." ) ) ?
                                                   gType.GetProperty ( parameter.Display.Split ( new char[] { '.' } )[ 1 ] ).GetValue ( global, null ).ToString () :
                                                   parameter.Display;

                                result.AddParameter ( new Parameter ( parameter.Name, display, value, parameter.Source, indexPort - 1 ) );
                            }
                        }
                    }
                }
            }
            // Port has not installed a Meter
            else
            {
                result.AddParameter(new Parameter("Status", "Status", "Not Installed"));
                result.AddParameter(new Parameter("MeterTypeId", "Meter Type ID", "000000000"));
                result.AddParameter(new Parameter("MeterReading", "Meter Reading", "Bad Reading"));
            }
            
            return result;
        }

        private string FormatLength (
            string value,
            string source,
            string fill )
        {
            Global global = this.config.Global;
            Type   gType  = global.GetType ();

            if ( ! string.IsNullOrEmpty ( source ) )
            {
                int length;
                if ( ! int.TryParse ( source, out length ) &&
                     source.ToLower ().StartsWith ( "global." ) )
                    length = ( int ) gType.GetProperty ( source.Split ( new char[] { '.' } )[ 1 ] ).GetValue ( global, null );

                if ( length > 0 && value.Length > length )
                    value = value.Substring ( 0, length );

                switch ( fill.ToLower () )
                {
                    case "left" : value = value.PadLeft  ( length, '0' ); break;
                    case "right": value = value.PadRight ( length, '0' ); break;
                }
            }

            return value;
        }

        private async Task<bool> ValidateCondition (
            string conditionStr,
            dynamic map,
            Mtu mtu,
            int portIndex = 1 )
        {
            if ( string.IsNullOrEmpty ( conditionStr ) )
                return true;

            try
            {
                List<ConditionObjet> conditions = new List<ConditionObjet> ();

                string test = conditionStr.Trim ().Replace ( " ", string.Empty );

                MatchCollection matches = Regex.Matches ( conditionStr.Trim ().Replace ( " ", string.Empty ), REGEX_IFS, RegexOptions.Compiled );
                foreach ( Match m in matches.Cast<Match> ().ToList () )
                    conditions.Add (
                        new ConditionObjet (
                            Uri.UnescapeDataString ( m.Groups[ 1 ].Value ),     // + or |
                            Uri.UnescapeDataString ( m.Groups[ 2 ].Value ),     // Type/Class[.Property]
                            Uri.UnescapeDataString ( m.Groups[ 3 ].Value ),     // = , < , > or !
                            Uri.UnescapeDataString ( m.Groups[ 4 ].Value ) ) ); // Value

                int    finalResult = 0;
                string port   = PORT_PREFIX + portIndex;
                Global global = this.config.Global;
                Type   gType  = global.GetType ();
                Type   pType  = typeof ( Port );
                
                foreach ( ConditionObjet condition in conditions )
                {
                    int      result       = 0;
                    string   currentValue = string.Empty;
                    string[] condMembers  = condition.Key.Split ( new char[]{ '.' } ); // Class.Property
                    string   condProperty = ( condMembers.Length > 1 ) ? condMembers[ 1 ] : condMembers[ 0 ]; // Property

                    // Class or Type
                    switch ( condMembers[ 0 ] )
                    {
                        case IFACE_PORT  : currentValue = pType.GetProperty ( condProperty ).GetValue ( mtu.Ports[ portIndex - 1 ] ).ToString (); break;
                        case IFACE_ACTION: currentValue = GetProperty ( condProperty ); break; // User, Date or Type
                        case IFACE_MTU   : currentValue = mtu.GetProperty ( condProperty ); break; // Mtu class
                        case IFACE_GLOBAL: currentValue = gType.GetProperty ( condProperty ).GetValue ( global, null ).ToString(); break; // Global class
                        default: // Dynamic MemoryMap
                            // Recover register from MTU memory map
                            // Some registers have port sufix but other not
                            if ( map.ContainsMember ( port + condProperty ) )
                                currentValue = ( await map[ port + condProperty ].GetValue () ).ToString ();
                            else if ( map.ContainsMember ( condProperty ) )
                                currentValue = ( await map[ condProperty ].GetValue () ).ToString ();
                            break;
                    }

                    // Compare property value with condition value
                    if ( ! string.IsNullOrEmpty ( currentValue ) )
                    {
                        if ( condition.IsEqual &&
                             currentValue.ToLower ().Equals ( condition.Value.ToLower () ) ||
                             condition.IsNot &&
                             ! currentValue.ToLower ().Equals ( condition.Value.ToLower () ) ||
                             condition.IsLower &&
                             float.Parse ( currentValue ) < float.Parse ( condition.Value ) ||
                             condition.IsGreater &&
                             float.Parse ( currentValue ) > float.Parse ( condition.Value ) )
                            result = 1; // Ok
                    }

                    // Concatenate results
                    if      ( condition.Condition.Equals ( IFACE_OR  ) ) finalResult += result; // If one condition validate, pass
                    else if ( condition.Condition.Equals ( IFACE_AND ) ) finalResult *= result; // All conditions have to validate
                }

                if ( finalResult <= 0 )
                {

                }

                return ( finalResult > 0 );
            }
            catch ( Exception e )
            {
                //...
            }

            return false;
        }

        #endregion
    }
}
