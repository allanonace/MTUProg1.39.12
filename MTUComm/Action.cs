using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lexi.Interfaces;
using MTUComm.actions;
using Xml;

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
        private const string IFACE_GLOBAL    = "Global";
        private const string IFACE_MEMORYMAP = "MemoryMap";
        private const string IFACE_FORM      = "Form";
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
            ReadData,
            MtuInstallationConfirmation,
            Diagnosis,
            BasicRead
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
            {ActionType.ReadData,"Read Data Log" },
            {ActionType.MtuInstallationConfirmation,"Install Confirmation" },
            {ActionType.Diagnosis, string.Empty }
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
            {ActionType.ReadData, "ReadDataLog" },
            {ActionType.MtuInstallationConfirmation,"InstallConfirmation" },
            {ActionType.Diagnosis, string.Empty }
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
            {ActionType.ReadData, "DataRead" },
            {ActionType.MtuInstallationConfirmation,"InstallConfirmation" },
            {ActionType.Diagnosis, string.Empty }
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

        public static Mtu currentMtu { private set; get; }
        
        public static void SetCurrentMtu (
            MTUBasicInfo mtuBasic )
        {
            currentMtu = Configuration.GetInstance ().GetMtuTypeById ( ( int )mtuBasic.Type );
        }
        
        private string lastLogCreated;

        public MTUComm comm { get; private set; }
        public ActionType type { get; }
        private List<Parameter> mparameters = new List<Parameter>();
        private Boolean canceled = false;
        public  String user { get; private set; }
        public  Logger logger;
        private Configuration configuration;
        private List<Action> sub_actions = new List<Action>();
        public  int order = 0;
        //public Func<object, object, object> OnFinish;
        
        public static bool IsFromScripting;
        
        public static Action currentAction;

        #endregion

        #region Properties

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

        public Action(Configuration config, ISerial serial, ActionType type, String user = "", String outputfile = "", bool isFromScripting = false )
        {
            // outputfile = new FileInfo ( outputfile ).Name; // NO
            // System.IO.Path.GetFileName(outputfile)); // NO

            configuration = config;
            logger = new Logger(outputfile.Substring(outputfile.LastIndexOf('\\') + 1) ); 
            comm = new MTUComm(serial, config);
            this.type = type;
            this.user = user;
            comm.OnError += Comm_OnError;
            
            currentAction = this;
        }

        #endregion

        #region Parameters

        public void AddParameter (Parameter parameter)
        {
            mparameters.Add(parameter);
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

                switch (type)
                {
                    case ActionType.ReadMtu:
                        comm.OnReadMtu  -= Comm_OnReadMtu;
                        comm.OnReadMtu  += Comm_OnReadMtu;
                        comm.OnProgress -= Comm_OnProgress;
                        comm.OnProgress += Comm_OnProgress;
                        break;

                    case ActionType.AddMtu:
                    case ActionType.AddMtuAddMeter:
                    case ActionType.AddMtuReplaceMeter:
                    case ActionType.ReplaceMTU:
                    case ActionType.ReplaceMeter:
                    case ActionType.ReplaceMtuReplaceMeter:
                        comm.OnAddMtu   -= Comm_OnAddMtu;
                        comm.OnProgress -= Comm_OnProgress;
                        comm.OnAddMtu   += Comm_OnAddMtu;
                        comm.OnProgress += Comm_OnProgress;
                        // Interactive and Scripting
                        if (mtuForm != null)
                             parameters.AddRange(new object[] { (AddMtuForm)mtuForm, this.user, this });
                        else parameters.Add(this);
                        break;

                    case ActionType.TurnOffMtu:
                    case ActionType.TurnOnMtu:
                        comm.OnTurnOffMtu -= Comm_OnTurnOnOffMtu;
                        comm.OnTurnOffMtu += Comm_OnTurnOnOffMtu;
                        break;

                    case ActionType.MtuInstallationConfirmation:
                        comm.OnReadMtu  -= Comm_OnReadMtu;
                        comm.OnProgress -= Comm_OnProgress;
                        comm.OnReadMtu  += Comm_OnReadMtu;
                        comm.OnProgress += Comm_OnProgress;
                        break;

                    case ActionType.ReadData:
                        Parameter param = mparameters.Find(x => (x.Type == Parameter.ParameterType.DaysOfRead));
                        if (param == null)
                        {
                            this.OnError (); //this, new ActionErrorArgs("Days Of Read parameter Not Defined or Invalid"));
                            break;
                        }
                        int DaysOfRead = 0;
                        if (!Int32.TryParse(param.Value, out DaysOfRead) || DaysOfRead <= 0)
                        {
                            this.OnError (); //this, new ActionErrorArgs("Days Of Read parameter Invalid"));
                            break;
                        }
                        comm.OnReadMtuData -= Comm_OnReadMtuData;
                        comm.OnReadMtuData += Comm_OnReadMtuData;
                        parameters.Add(DaysOfRead);
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
        }

        private void Comm_OnReadMtuData(object sender, MTUComm.ReadMtuDataArgs e)
        {
            ActionProgressArgs args;
            switch (e.Status)
            {
                case LogQueryResult.LogDataType.Bussy:
                    args = new ActionProgressArgs(0, 0);
                    OnProgress(this, args);
                    break;
                case LogQueryResult.LogDataType.NewPacket:
                    args = new ActionProgressArgs(e.CurrentEntry, e.TotalEntries);
                    OnProgress(this, args);
                    break;
                case LogQueryResult.LogDataType.LastPacket:
                    Mtu mtu_type = configuration.GetMtuTypeById((int)e.MtuType.Type);
                    ActionResult result = ReadMTUData(e.Start, e.End, e.Entries, e.MtuType, mtu_type);
                    logger.ReadData(this, result, mtu_type);
                    ActionFinishArgs f_args = new ActionFinishArgs(null);
                    OnFinish(this, f_args);
                    break;
            }
        }

        private void Comm_OnReadMtu(object sender, MTUComm.ReadMtuArgs e)
        {
            ActionResult result = CreateActionResultUsingInterface ( e.MemoryMap, e.Mtu );
            this.lastLogCreated = logger.ReadMTU ( this, result, e.Mtu );
            ActionFinishArgs args = new ActionFinishArgs ( result );

            OnFinish ( this, args );
        }

        private void Comm_OnTurnOnOffMtu ( object sender, MTUComm.TurnOffMtuArgs e )
        {
            ActionResult result = getBasciInfoResult ();
            this.lastLogCreated = logger.TurnOnOff ( this, e.Mtu, result.getParameters()[2].Value );
            ActionFinishArgs args = new ActionFinishArgs ( result );

            OnFinish ( this, args );
        }

        private ActionResult Comm_OnAddMtu(object sender, MTUComm.AddMtuArgs e)
        {
            ActionResult result = CreateActionResultUsingInterface ( e.MemoryMap, e.MtuType, e.Form );
            ActionFinishArgs args = new ActionFinishArgs ( result );
            e.AddMtuLog.LogReadMtu ( result );
            this.lastLogCreated = e.AddMtuLog.Save ();
            
            args.FormLog = e.AddMtuLog;

            OnFinish ( this, args );
            return result;
        }

        private void Comm_OnBasicRead(object sender, MTUComm.BasicReadArgs e)
        {
            ActionResult result = new ActionResult();
            ActionFinishArgs args = new ActionFinishArgs(result);
            OnFinish(this, args);
        }

        #endregion

        #region Reads

        private ActionResult ReadMTUData ( DateTime start, DateTime end, List<LogDataEntry> Entries, MTUBasicInfo mtuInfo, Mtu mtu )
        {
            ActionResult result = new ActionResult();

            string log_path = logger.ReadDataEntries(mtuInfo.Id.ToString("d15"), start, end, Entries);

            InterfaceParameters[] parameters = configuration.getAllInterfaceFields(mtu, ActionType.ReadData );
            foreach (InterfaceParameters parameter in parameters)
            {
                if (parameter.Name.Equals("Port"))
                {
                    for (int i = 0; i < mtu.Ports.Count; i++)
                    {
                        foreach (InterfaceParameters port_parameter in parameter.Parameters)
                        {
                            if (port_parameter.Source != null && port_parameter.Source.StartsWith("ActionParams"))
                            {
                                Parameter sel_parameter = GetParameterByTag(port_parameter.Name, i + 1);
                                if (sel_parameter != null)
                                {
                                    result.AddParameter(new Parameter(port_parameter.Name, port_parameter.Display, sel_parameter.Value));
                                }
                            }

                        }


                    }
                }
                else
                {
                    try
                    {

                        if (parameter.Source != null && parameter.Source.StartsWith("ActionParams"))
                        {
                            Parameter sel_parameter = GetParameterByTag(parameter.Name);
                            if (sel_parameter != null)
                            {
                                result.AddParameter(new Parameter(parameter.Name, parameter.Display, sel_parameter.Value));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "\r\n" + e.StackTrace);
                    }
                }

            }

            result.AddParameter(new Parameter("ReadRequest", "Number Read Request Days", ""));
            result.AddParameter(new Parameter("ReadResult", "Read Result", "Number of Reads " + Entries.Count.ToString() + " for Selected Period From " + start.ToString("dd/MM/yyyy") + " 0:00:00 Till " + end.ToString("dd/MM/yyyy") + " 23:59:59"));
            result.AddParameter(new Parameter("ReadResultFile", "Read Result File", log_path));

            return result;
        }

        private ActionResult getBasciInfoResult ()
        {
            ActionResult result = new ActionResult ();
            MTUBasicInfo basic  = comm.GetBasicInfo ();
            
            result.AddParameter(new Parameter("Date", "Date/Time", GetProperty("Date")));
            result.AddParameter(new Parameter("User", "User", GetProperty("User")));
            result.AddParameter(new Parameter("MtuId", "MTU ID", basic.Id));
    
            return result;
        }

        #endregion

        #region Interface

        private ActionResult CreateActionResultUsingInterface (
            dynamic map  = null,
            Mtu     mtu  = null,
            MtuForm form = null,
            ActionType actionType = ActionType.ReadMtu )
        {
            Parameter paramToAdd;
            Global       global = Configuration.GetInstance ().GetGlobal ();
            Type         gType  = global.GetType ();
            ActionResult result = new ActionResult ();
            InterfaceParameters[] parameters = configuration.getAllInterfaceFields ( mtu, actionType );
            
            foreach ( InterfaceParameters parameter in parameters )
            {
                if ( parameter.Name.Equals ( IFACE_PORT ) )
                    for ( int i = 0; i < mtu.Ports.Count; i++ )
                        result.addPort ( ReadPort ( i + 1, parameter.Parameters.ToArray (), map, mtu ) );
                else
                {
                    try
                    {
                        if ( ValidateCondition ( parameter.Conditional, map, mtu ) )
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
                            switch ( sourceWhere )
                            {
                                case IFACE_ACTION   : value      = this.GetProperty  ( sourceProperty ); break;
                                case IFACE_MTU      : value      = mtu .GetProperty  ( sourceProperty ); break;
                                case IFACE_FORM     : paramToAdd = form.GetParameter ( sourceProperty ); break;
                                //case IFACE_MEMORYMAP: value      = map .GetProperty  ( sourceProperty ).Value.ToString (); break; // MemoryMap.SourceProperty
                                default             : value      = map .GetProperty  ( sourceProperty ).Value.ToString (); break; // MemoryMap.ParameterName
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
                                result.AddParameter ( paramToAdd );
                        }
                    }
                    catch ( Exception e )
                    {
                        //...
                    }
                }
            }
            return result;
        }

        private ActionResult ReadPort (
            int indexPort,
            InterfaceParameters[] parameters,
            dynamic map,
            Mtu mtu )
        {
            ActionResult result   = new ActionResult ();
        
            try
            {
        
            
            Port         portType = mtu.Ports[ indexPort - 1 ];
            Global       global   = Configuration.GetInstance ().GetGlobal ();
            Type         gType    = global.GetType ();

            // Meter Serial Number
            int meterId = map.GetProperty ( PORT_PREFIX + indexPort + "MeterType" ).Value;

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
                        if ( ValidateCondition ( parameter.Conditional, map, mtu, indexPort ) )
                        {
                            try
                            {
                                string meter_reading_error = map.GetProperty ( PORT_PREFIX + indexPort + IFACE_READERROR ).Value.ToString ();
                                if ( meter_reading_error.Length < 1 )
                                {
                                    ulong meter_reading  = map.GetProperty ( PORT_PREFIX + indexPort + IFACE_MREADING ).Value;
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
                        try
                        {
                            if ( ValidateCondition ( parameter.Conditional, map, mtu, indexPort ) )
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

                                switch ( sourceWhere )
                                {
                                    case IFACE_PORT     : value = portType .GetProperty ( sourceProperty ); break;
                                    case IFACE_MTU      : value = mtu      .GetProperty ( sourceProperty ); break;
                                    case IFACE_METER    : value = meter    .GetProperty ( sourceProperty ); break;
                                    //case IFACE_MEMORYMAP: value = map.GetProperty ( PORT_PREFIX + indexPort + sourceProperty ).Value.ToString (); break; // MemoryMap.SourceProperty
                                    default             : value = map.GetProperty ( PORT_PREFIX + indexPort + sourceProperty ).Value.ToString (); break; // MemoryMap.ParameterName
                                }
                                
                                if ( ! string.IsNullOrEmpty ( value ) )
                                {
                                    string display = ( parameter.Display.ToLower ().StartsWith ( "global." ) ) ?
                                                       gType.GetProperty ( parameter.Display.Split ( new char[] { '.' } )[ 1 ] ).GetValue ( global, null ).ToString () :
                                                       parameter.Display;
                                    result.AddParameter ( new Parameter ( parameter.Name, display, value, parameter.Source, indexPort - 1 ) );
                                }
                            }
                        }
                        catch ( Exception e )
                        {
                            //...
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
            
            

            }
            catch ( Exception ex )
            {

            }
            
            return result;
        }

        private bool ValidateCondition (
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
                Global global = Configuration.GetInstance ().GetGlobal ();
                Type   gType  = global.GetType ();
                
                foreach ( ConditionObjet condition in conditions )
                {
                    int      result       = 0;
                    string   currentValue = string.Empty;
                    string[] condMembers  = condition.Key.Split ( new char[]{ '.' } ); // Class.Property
                    string   condProperty = ( condMembers.Length > 1 ) ? condMembers[ 1 ] : condMembers[ 0 ]; // Property

                    // Class or Type
                    switch ( condMembers[ 0 ] )
                    {
                        case IFACE_PORT  : currentValue = port; break; // P1 or P2
                        case IFACE_ACTION: currentValue = GetProperty ( condProperty ); break; // User, Date or Type
                        case IFACE_METER : break;
                        case IFACE_MTU   : currentValue = mtu.GetProperty ( condProperty ); break; // Mtu class
                        case IFACE_GLOBAL: currentValue = gType.GetProperty ( condProperty ).GetValue ( global, null ).ToString(); break; // Global class
                        default: // Dynamic MemoryMap
                            // Recover register from MTU memory map
                            // Some registers have port sufix but other not
                            if ( map.ContainsMember ( port + condProperty ) )
                                currentValue = map.GetProperty ( port + condProperty ).Value.ToString ();
                            else if ( map.ContainsMember ( condProperty ) )
                                currentValue = map.GetProperty ( condProperty ).Value.ToString ();
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
