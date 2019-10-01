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

using NodeDiscoveryResult = MTUComm.MTUComm.NodeDiscoveryResult;

namespace MTUComm
{
    /// <summary>
    /// Generic representation of the supported actions, with all the information
    /// and events required to be able to execute the action and stablish a bidirectional
    /// communicate between the view ( UI or script ) and the controller ( logic ).
    /// <para>
    /// See <see cref="ActionType"/> for a list of available actions.
    /// </para>
    /// </summary>
    /// <seealso cref="MTUComm"/>
    public class Action
    {
        #region Nested class

        /// <summary>
        /// Nested class used during the evaluation of the conditions listed in the parameters in the XML interface file.
        /// <para>
        /// See <see cref="ValidateCondition"/> for the validation method.
        /// </para>
        /// <para>&#160;</para>
        /// <para>
        /// The operand on the right side of the conditional sentence should always be a specific value, not the name of a member,
        /// and the addition operator ( + ) should be used to concatenate ( AND, && ) more than one condition or "|" to separate
        /// different condition sentences ( OR, || ). Three type of value are admitted, true and false for boolean cases, positive
        /// and negative numbers, always parsing to float type to recover the decimal digits, and strings will be converted to lowercase.
        /// </para>
        /// <para>
        /// <example>
        /// <code>
        /// <Parameter name="DailyGMTHourRead" display="Daily GMT Hour Read" log="true" interface="false"
        /// conditional="MemoryMap.Shipbit=false + MemoryMap.DailyGMTHourRead gt -1 + MemoryMap.DailyGMTHourRead lt 24"/>
        /// </code>
        /// </example>
        /// </para>
        /// <para>&#160;</para>
        /// <para>
        /// Operators
        /// <list type="Conditions">
        /// <item>
        ///     <term>Equal</term>
        ///     <description>The member value must be equal to the specified value ( e.g. "Port.Number eq 1" )</description>
        /// </item>
        /// <item>
        ///     <term>Lower than</term>
        ///     <description>The member value must be less than the specified value ( e.g. "MemoryMap.P1MeterReading lt 999" )</description>
        /// </item>
        /// <item>
        ///     <term>Biger than</term>
        ///     <description>The member value must be greater than the specified value ( e.g. "MemoryMap.P1MeterReading gt 0" )</description>
        /// </item>
        /// <item>
        ///     <term>Different</term>
        ///     <description>The member value must be different from the specified value ( e.g. "Meter.Utility ! Water" )</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        private class ConditionObjet
        {
            public String CondConcatenate { get; private set; }
            public String CondOperation  { get; private set; }
            public String Key        { get; private set; } // Type/Class.Property
            public String Value      { get; private set; }

            public bool IsAnd        { get { return this.CondConcatenate.Equals ( IFACE_AND ); } }
            public bool IsOr         { get { return this.CondConcatenate.Equals ( IFACE_OR  ); } }
            public bool IsEqual      { get { return this.CondOperation.Equals ( "="  ); } }
            public bool IsLess       { get { return this.CondOperation.Equals ( "lt" ); } }
            public bool IsGreater    { get { return this.CondOperation.Equals ( "gt" ); } }
            public bool IsNot        { get { return this.CondOperation.Equals ( "!"  ); } }
            public bool InitNewBlock { private set; get; }
		    public bool FinishBlock  { private set; get; }

            public ConditionObjet (
                string condConcatenate,
                string openBlock,
                string closeBlock,
                string key,
                string condOperation,
                string value )
            {
                this.InitNewBlock = openBlock .Equals ( "(" );
			    this.FinishBlock  = closeBlock.Equals ( ")" );

                if ( ! condConcatenate.Equals ( IFACE_AND ) &&
                     ! condConcatenate.Equals ( IFACE_OR ) )
                     CondConcatenate = IFACE_OR;
                else CondConcatenate = condConcatenate;

                if ( ! this.InitNewBlock &&
                     ! this.FinishBlock )
                {
                    this.CondOperation = condOperation;
                    this.Key   = key;
                    this.Value = value;

                    if ( this.Value.StartsWith ( "Data." ) )
                        this.Value = Data.Get[ this.Value.Split ( '.' )[ 1 ] ];
                }
            }
        }

        #endregion

        #region Constants

        private const string DOTNET_IDS      = @"[_a-zA-Z][_a-zA-Z0-9]+";
        private const string REGEX_VALUES    = @"-?[0-9]+|[_a-zA-Z0-9]+";
        //private const string REGEX_IFS       = @"(\+|\|)?\s*(\()?\s*(" + DOTNET_IDS + @"." + DOTNET_IDS + @")\s*(=|lt|gt|!)\s*(" + REGEX_VALUES + @")\s*(\))?";
        private const string REGEX_IFS       = @"\s*(\+|\|)?\s*(?:(\()|(\))|(" + DOTNET_IDS + @"." + DOTNET_IDS + @")\s*(=|lt|gt|!)\s*(" + REGEX_VALUES + @"))";
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
        
        /// <summary>
        /// Types of actions supported by the applicaton working with MTUs and Meters.
        /// <para>&#160;</para>
        /// </para>
        /// <list type="ActionType">
        /// <item>
        ///     <term>ActionType.ReadMtu</term>
        ///     <description>Read an MTU</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.AddMtu</term>
        ///     <description>Installation of a MTU</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.ReplaceMTU</term>
        ///     <description>Installation of a MTU replacing current MTU</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.ReplaceMeter</term>
        ///     <description>Installation of a MTU replacing current Meter</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.AddMtuAddMeter</term>
        ///     <description>It is equal to ActionType.AddMtu</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.AddMtuReplaceMeter</term>
        ///     <description>It is equal to ActionType.ReplaceMTU</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.ReplaceMtuReplaceMeter</term>
        ///     <description>It is a combination of ActionType.ReplaceMTU and ActionType.ReplaceMeter</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.TurnOffMtu</term>
        ///     <description>Turn off the MTU if it is not already turned off</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.TurnOnMtu</term>
        ///     <description>Turn on the MTU if it is not already turned on</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.DataRead</term>
        ///     <description>Recover ang generates a log with MeterRead events</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.MtuInstallationConfirmation</term>
        ///     <description>Installation confirmation process a.k.a. RFCheck</description>
        /// </item>
        /// <item>
        ///     <term>ActionType.ReadFabric</term>
        ///     <description>Fast test method to know if the app can read from an MTU</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
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
            RemoteDisconnect,
            MtuInstallationConfirmation,
            BasicRead,
            ReadFabric,
        }

        public enum MTUStatus
        {
            ReadingMtuShortTime,
            ReadingMtuData,
            Autodetect,
            CheckingEnconderLongTime,
            ProgramingMtuShortTime,
            PreparingToProgram,
            TurningOffMtu,
            ReadingMtuAgain,
            ProgramingMtu,
            VerifyingMtuData,
            CheckingEnconderShortTime,
            TurningOnMtu,
            ReadingMtu
        };

        public static Dictionary<ActionType, String> logDisplays;
        public static Dictionary<ActionType, String> logTypes;
        public static Dictionary<ActionType, String> logReasons;
        public static Dictionary<ActionType, string[]> actionsTexts;

        #endregion

        #region Events

        /// <summary>
        /// Event that can be invoked during the execution of any action, for
        /// example to update the visual feedback by modifying the text of a label control.
        /// <para>
        /// See <see cref="Action.OnProgress"/> for the associated event ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.ProgresshHandler OnProgress;

        /// <summary>
        /// Event invoked only if the action completes successfully, with no exceptions.
        /// </summary>
        public event Delegates.ActionFinishHandler OnFinish;

        /// <summary>
        /// Event invoked if the action does not complete successfully or if it launches an exception.
        /// <para>
        /// See <see cref="Action.OnError"/> for the associated event ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.Empty OnError;

        #endregion

        #region Attributes
        
        private ActionType type;
        private List<Action> subActions;
        private int order;
        private String user;
        private List<Parameter> scriptParameters;
        private List<Parameter> additionalScriptParameters;
        private Mtu currentMtu;
        private Configuration config;
        private MTUComm mtucomm;
        private Logger logger;
        private string lastLogCreated;
        private Boolean canceled = false;

        private static bool alreadyInDataMainAction;
        
        #endregion

        #region Properties

        /// <summary>
        /// Name of the user that is executing the action, that in interactive
        /// mode is who has logged-in and in scripted mode is the string set
        /// in username tag in script file.
        /// </summary>
        public string User
        {
            get { return this.user; }
        }

        /// <summary>
        /// Represents current MTU.
        /// </summary>
        public Mtu CurrentMtu
        {
            get { return this.currentMtu; }
        }

        public MTUComm MTUComm
        {
            get { return this.mtucomm; }
        }

        public  Logger Logger
        {
            get { return this.logger; }
        }

        public int Order
        {
            get { return this.order; }
            set { this.order = value; }
        }

        public ActionType Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// In scripted mode it stores the parameters read from the script file
        /// that are listed in <see cref="Parameter.ParameterType"/> enumeration.
        /// </summary>
        public List<Parameter> ScriptParameters
        {
            get { return this.scriptParameters; }
        }

        /// <summary>
        /// In scripted mode it stores the parameters read from the script file
        /// that are NOT listed in <see cref="Parameter.ParameterType"/> enumeration,
        /// treated as additional parameters and will only be loged.
        /// </summary>
        public List<Parameter> AdditionalScriptParameters
        {
            get { return this.additionalScriptParameters; }
        }

        /// <summary>
        /// Text to be used to set the "display" attribute in the header of the Action block in the activity log.
        /// <example>
        /// <code>
        /// <Action display="Add MTU" type="..." reason="...">
        /// ...
        /// </Action>
        /// </code>
        /// </example>
        /// </summary>
        public String LogDisplay
        {
            get { return logDisplays[type]; }
        }

        /// <summary>
        /// Text to be used to set the "type" attribute in the header of the Action block in the activity log.
        /// <example>
        /// <code>
        /// <Action display="..." type="Program MTU" reason="...">
        /// ...
        /// </Action>
        /// </code>
        /// </example>
        /// </summary>
        public String LogType
        {
            get { return logTypes[this.type]; }
        }

        /// <summary>
        /// Text to be used to set the "reason" attribute in the header of the Action block in the activity log.
        /// <example>
        /// <code>
        /// <Action display="..." type="..." reason="AddMtu">
        /// ...
        /// </Action>
        /// </code>
        /// </example>
        /// </summary>
        public String LogReason
        {
            get { return logReasons[this.type]; }
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

        static Action ()
        {
            actionsTexts =
            new Dictionary<ActionType, string[]>()
            {   
            { ActionType.RemoteDisconnect,
                new string[]
                {
                   "Valve Operation",
                   "Valve Operation",
                   "Valve Operation",
                   "read_mtu_btn.png"
                }
            },
            { ActionType.DataRead,
                new string[]
                {
                   "Data Read",
                   "Data Read",
                   "Data Read",
                   "read_mtu_btn.png"
                }
            },
            { ActionType.AddMtu,
                new string[]
                {
                   "Add MTU",
                   "Add MTU",
                   "Add MTU",
                   "add_mtu_btn.png"
                }
            },
            { ActionType.ReplaceMTU,
                new string[]
                {
                   "Replace MTU",
                   "Replace MTU",
                   "Replace MTU",
                   "rep_mtu_btn.png"
                }
            },
            { ActionType.ReplaceMeter,
                new string[]
                {
                   "Replace Meter",
                   "Replace Meter",
                   "Replace Meter",
                   "rep_meter_btn.png"
                }
            },
            { ActionType.AddMtuAddMeter,
                new string[]
                {
                   "Add MTU / Add Meter",
                   "Add MTU / Add Meter",
                   "Add MTU / Add Meter",
                   "add_mtu_meter_btn.png"
                }
            },
            { ActionType.AddMtuReplaceMeter,
                new string[]
                {
                   "Add MTU / Replace Meter",
                   "Add MTU / Replace Meter",
                   "Add MTU / Replace Meter",
                   "add_mtu_rep_meter_btn.png"
                }
            },
            { ActionType.ReplaceMtuReplaceMeter,
                new string[]
                {
                   "Replace MTU / Replace Meter",
                   "Replace MTU / Replace Meter",
                   "Replace MTU / Replace Meter",
                   "rep_mtu_rep_meter_btn.png"
                }
            },
        };


        logDisplays = new Dictionary<ActionType,String> ()
            {
                {ActionType.BasicRead,                      "Basic Read" },
                {ActionType.ReadMtu,                        "Read MTU" },
                {ActionType.AddMtu,                         "Add MTU" },
                {ActionType.ReplaceMTU,                     "Replace MTU" },
                {ActionType.AddMtuAddMeter,                 "Add MTU/Add Meter" },
                {ActionType.AddMtuReplaceMeter,             "Add MTU/Replace Meter" },
                {ActionType.ReplaceMtuReplaceMeter,         "Replace MTU/Replace Meter" },
                {ActionType.ReplaceMeter,                   "Replace Meter" },
                {ActionType.TurnOffMtu,                     "Turn Off MTU" },
                {ActionType.TurnOnMtu,                      "Turn On MTU" },
                {ActionType.DataRead,                       "Read Data Log" },
                {ActionType.MtuInstallationConfirmation,    "Install Confirmation" },
                {ActionType.ReadFabric,                     "Read Fabric" },
                {ActionType.RemoteDisconnect,               "Valve Operation" }
            };

            logTypes = new Dictionary<ActionType,String> ()
            {
                {ActionType.BasicRead,                      "BasicRead" },
                {ActionType.ReadMtu,                        "ReadMTU" },
                {ActionType.AddMtu,                         "Program MTU" },
                {ActionType.ReplaceMTU,                     "Program MTU" },
                {ActionType.AddMtuAddMeter,                 "Program MTU" },
                {ActionType.AddMtuReplaceMeter,             "Program MTU" },
                {ActionType.ReplaceMtuReplaceMeter,         "Program MTU" },
                {ActionType.ReplaceMeter,                   "Program MTU" },
                {ActionType.TurnOffMtu,                     "TurnOffMtu" },
                {ActionType.TurnOnMtu,                      "TurnOnMtu" },
                {ActionType.DataRead,                       "ReadDataLog" },
                {ActionType.MtuInstallationConfirmation,    "InstallConfirmation" },
                {ActionType.ReadFabric,                     "ReadFabric" },
                {ActionType.RemoteDisconnect,               "Valve Operation" }
            };

            logReasons = new Dictionary<ActionType,String> ()
            {
                {ActionType.BasicRead,                      "BasicRead" },
                {ActionType.ReadMtu,                        "ReadMtu" },
                {ActionType.AddMtu,                         "AddMtu" },
                {ActionType.ReplaceMTU,                     "ReplaceMtu" },
                {ActionType.AddMtuAddMeter,                 "AddMtuAddMeter" },
                {ActionType.AddMtuReplaceMeter,             "AddMtuReplaceMeter" },
                {ActionType.ReplaceMtuReplaceMeter,         "ReplaceMtuReplaceMeter" },
                {ActionType.ReplaceMeter,                   "ReplaceMeter" },
                {ActionType.TurnOffMtu,                     "TurnOffMtu" },
                {ActionType.TurnOnMtu,                      "TurnOnMtu" },
                {ActionType.DataRead,                       "DataRead" },
                {ActionType.MtuInstallationConfirmation,    "InstallConfirmation" },
                {ActionType.ReadFabric,                     "ReadFabric" },
                {ActionType.RemoteDisconnect,               "ValveOperation" }
            };
        }

        public Action (
            ISerial serial,
            ActionType type,
            String user = "",
            String outputfile = "" )
        {
            // outputfile = new FileInfo ( outputfile ).Name; // NO
            // System.IO.Path.GetFileName(outputfile)); // NO

            this.subActions                 = new List<Action> ();
            this.scriptParameters           = new List<Parameter> ();
            this.additionalScriptParameters = new List<Parameter> ();

            this.logger = new Logger ( outputfile.Substring ( outputfile.LastIndexOf ( '\\' ) + 1 ) );
            this.config = Singleton.Get.Configuration;
            
            this.type = type;
            this.user = user;

            this.mtucomm = new MTUComm ( serial, config );

            this.additionalScriptParameters = new List<Parameter> ();

            // Only save reference for the current action,
            // not for nested or auxiliary actions ( as BasicRead )
            if ( this.type == ActionType.BasicRead )
            {
                alreadyInDataMainAction = false;

                // Reset temp data but not all data, because some data is created
                // only one time and is needed during the life of the application
                Data.Reset ();
                Data.Reset ( "MemoryMap" );
                Data.Reset ( "MtuBasicInfo" );
                InterfaceAux.ResetInfo ();
            }
            else
                if (! alreadyInDataMainAction )
                {
                    alreadyInDataMainAction = true;

                    Singleton.Set = this;

                    this.currentMtu = this.config.GetMtuTypeById ( ( int )Data.Get.MtuBasicInfo.Type );
                }
        }
        
        #endregion

        #region Parameters

        public void AddParameter (Parameter parameter)
        {
            scriptParameters.Add(parameter);
        }

        public void AddAdditionalParameter ( Parameter parameter )
        {
            this.additionalScriptParameters.Add ( parameter );
        }

        public void AddParameter ( MtuForm form )
        {
            Parameter[] addMtuParams = form.GetParameters ();
            foreach ( Parameter parameter in addMtuParams )
                scriptParameters.Add (parameter);
        }

        public Parameter[] GetParameters()
        {
            return scriptParameters.ToArray();
        }

        public Parameter GetParameterByTag(string tag, int port = -1)
        {
            return scriptParameters.Find(x => x.getLogTag().Equals(tag) && ( port == -1 || x.Port == port ) );
        }

        #endregion

        #region Actions

        public void AddActions(Action action)
        {
            subActions.Add(action);
        }

        public Action[] GetSubActions()
        {
            return subActions.ToArray();
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

        public async Task<bool> RunNavValidation (
            ActionType actionTarget )
        {
            await this.Run (); // Basic Read
            return await this.mtucomm.LaunchValidationThread ( actionTarget ); // Validation
        }

        /// <summary>
        /// Prepares the events ( OnError, OnProgress and OnFinish ) for all possible situations,
        /// registers the required parameters and pass the control to <see cref="MTUComm"/> where
        /// the logic will be executed.
        /// <para>
        /// See <see cref="MTUComm.LaunchActionThread"/> for the entry point of the action logic.
        /// </para>
        /// </summary>
        /// <remarks>
        /// TODO: Modify all the write logic ( Add/Replace ) to use the log interface.
        /// </remarks>
        /// <param name="mtuForm">Write actions stores the set data in an intermediate form object</param>
        public async Task Run (
            MtuForm mtuForm = null )
        {
            if ( canceled )
            {
                throw new Exception ( "Canceled Action can not be Executed" );
            }
            else
            {
                List<object> parameters = new List<object> ();

                this.mtucomm.OnError -= OnError;
                this.mtucomm.OnError += OnError;

                this.mtucomm.OnProgress -= OnProgress;
                this.mtucomm.OnProgress += OnProgress;

                // Can be used in all writing actions ( Add/Replace ), reading and IC,
                // and for that reason it is easier and clearer to always register this event
                this.mtucomm.OnNodeDiscovery -= OnNodeDiscovery;
                this.mtucomm.OnNodeDiscovery += OnNodeDiscovery;

                switch ( type )
                {
                    case ActionType.ReadFabric:
                        this.mtucomm.OnReadFabric -= OnReadFabric;
                        this.mtucomm.OnReadFabric += OnReadFabric;
                        break;

                    case ActionType.ReadMtu:
                    case ActionType.MtuInstallationConfirmation:
                        this.mtucomm.OnReadMtu -= OnReadMtu;
                        this.mtucomm.OnReadMtu += OnReadMtu;
                        break;
                      
                    case ActionType.AddMtu:
                    case ActionType.AddMtuAddMeter:
                    case ActionType.AddMtuReplaceMeter:
                    case ActionType.ReplaceMTU:
                    case ActionType.ReplaceMeter:
                    case ActionType.ReplaceMtuReplaceMeter:
                        this.mtucomm.OnAddMtu -= OnAddMtu;
                        this.mtucomm.OnAddMtu += OnAddMtu;
                        // Interactive and Scripting
                        if ( mtuForm != null )
                             parameters.AddRange ( new object[] { (AddMtuForm)mtuForm, this.user, this } );
                        else parameters.Add ( this );
                        break;

                    case ActionType.TurnOffMtu:
                    case ActionType.TurnOnMtu:
                        this.mtucomm.OnTurnOffMtu -= OnTurnOnOffMtu;
                        this.mtucomm.OnTurnOffMtu += OnTurnOnOffMtu;
                        break;

                    case ActionType.DataRead:
                        this.mtucomm.OnDataRead -= OnDataRead;
                        this.mtucomm.OnDataRead += OnDataRead;
                        // In interactive mode value are already set in Data
                        // but in scripted mode they are stored in Action.parameters
                        if ( Data.Get.IsFromScripting )
                            parameters.Add ( this );
                        break;
                    
                    case ActionType.RemoteDisconnect:
                        this.mtucomm.OnRemoteDisconnect -= OnRemoteDisconnect;
                        this.mtucomm.OnRemoteDisconnect += OnRemoteDisconnect;
                        break;

                    case ActionType.BasicRead:
                        this.mtucomm.OnBasicRead -= OnBasicRead;
                        this.mtucomm.OnBasicRead += OnBasicRead;
                        break;
                }

                await this.mtucomm.LaunchActionThread ( type, parameters.ToArray () );
            }
        }

        public void Cancel ( string cancelReason = "410 DR Defective Register" )
        {
            canceled = true;
            logger.Cancel ( this, "User Cancelled", cancelReason );
        }

        #endregion

        #region OnEvents

        /// <summary>
        /// Method invoked after have completing correctly a <see cref="ActionType"/>.BasicRead
        /// action, without exceptions.
        /// <para>
        /// See <see cref="MTUComm.OnBasicRead"/> for the associated event ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        /// <seealso cref="MTUComm.BasicRead"/>
        private async Task OnBasicRead ( Delegates.ActionArgs args )
        {
            await this.OnFinish ( this );

        }

        /// <summary>
        /// Method invoked after have completing correctly a <see cref="ActionType"/>.ReadFabric
        /// action, without exceptions.
        /// <para>
        /// See <see cref="MTUComm.OnReadFabric"/> for the associated event ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="MTUComm.ReadFabric"/>
        private async Task OnReadFabric ( Delegates.ActionArgs args )
        {
            // Show result in the screen
            await this.OnFinish ( this );
        }

        /// <summary>
        /// Method invoked after have completing correctly a <see cref="ActionType"/>.ReadMtu
        /// action, without exceptions.
        /// <para>
        /// See <see cref="MTUComm.OnReadMtu"/> for the associated event ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="MTUComm.ReadMtu"/>
        private async Task OnReadMtu ( Delegates.ActionArgs args )
        {
            try
            {

                // Load parameters using the interface file
                ActionResult resultAllInterfaces = await CreateActionResultUsingInterface ( args.Map, args.Mtu );

                //await args.Map.LogFullMemory();

                // Write result in the activity log
                this.lastLogCreated = logger.ReadMTU ( this, resultAllInterfaces, args.Mtu );
                
                // Show result in the screen
                this.OnFinish ( this, new Delegates.ActionFinishArgs ( resultAllInterfaces, args.Mtu ) );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        /// <summary>
        /// Method invoked after have completing correctly a writing ( <see cref="ActionType"/>.Add|Replace ) action,
        /// without exceptions.
        /// <para>
        /// See <see cref="MTUComm.OnAddMtu"/> for the associated event ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="MTUComm.AddMtu(Action)"/>
        /// <seealso cref="MTUComm.AddMtu(dynamic, string, Action)"/>
        private async Task OnAddMtu ( Delegates.ActionArgs args )
        {
            try
            {
                dynamic form = args.Extra[ 0 ];
                AddMtuLog addMtuLog = args.Extra[ 1 ];

                // Load parameters using the interface file
                ActionResult result = await CreateActionResultUsingInterface ( args.Map, args.Mtu, form );

                // Write result in the activity log
                addMtuLog.LogReadMtu ( result );
                this.lastLogCreated = addMtuLog.Save ();

                // Show result in the screen
                this.OnFinish ( this, new Delegates.ActionFinishArgs ( result ) );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        /// <summary>
        /// Method invoked after have completing correctly a <see cref="ActionType"/>.TurnOffMtu
        /// or TurnOnMtu action, without exceptions.
        /// <para>
        /// See <see cref="MTUComm.OnTurnOffMtu"/> and <see cref="MTUComm.OnTurnOnMtu"/> for the events associated ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        /// <seealso cref="MTUComm.TurnOnOffMtu"/>
        private async Task OnTurnOnOffMtu ( Delegates.ActionArgs args )
        {
            try
            {
                ActionResult resultBasic = getBasciInfoResult ();

                // Write result in the activity log
                this.lastLogCreated = logger.TurnOnOff ( this, args.Mtu, resultBasic );

                // Show result in the screen
                await this.OnFinish ( this, new Delegates.ActionFinishArgs ( resultBasic ) );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        /// <summary>
        /// Method invoked after have completing correctly a <see cref="ActionType"/>.DataRead
        /// action, without exceptions.
        /// <para>
        /// See <see cref="MTUComm.OnDataRead"/> for the associated event ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="MTUComm.DataRead"/>
        /// <seealso cref="MTUComm.DataRead(Action)"/>
        private async Task OnDataRead ( Delegates.ActionArgs args )
        {
            try
            {
                EventLogList eventList = args.Extra[ 0 ];

                // Prepares custom values that will be loaded using the interface
                Data.SetTemp ( "TotalDifDays", eventList.TotalDifDays );

                Data.SetTemp ( "ProcessResult",
                    $"Number of Reads {eventList.Count} for Selected Period " +
                    $"From {eventList.DateStart.ToString ( "dd/MM/yyyy HH:mm:ss" )} " +
                    $"Till {eventList.DateEnd  .ToString ( "dd/MM/yyyy HH:mm:ss" )}" );

                // NOTE: STARProgrammer MtuComm.cs Line 5341
                string path = Path.Combine ( Mobile.EventPath,
                    $"MTUID{Data.Get.MtuId}-{DateTime.Now.ToString ( "MMddyyyyHH" )}-" +
                    $"{ DateTime.Now.Ticks }DataLog.xml" );
                string subpath = path.Substring ( path.LastIndexOf ( "/" ) + 1 );
                Data.SetTemp ( "ProcessResultFileFull", path );
                Data.SetTemp ( "ProcessResultFile", subpath );

                // Load parameters using the interface file
                ActionResult dataRead_allParamsFromInterface = await CreateActionResultUsingInterface ( args.Map, args.Mtu, null, ActionType.DataRead );
                ActionResult readMtu_allParamsFromInterface  = await CreateActionResultUsingInterface ( args.Map, args.Mtu );

                // Write result in the DataRead file
                this.lastLogCreated = logger.DataRead ( dataRead_allParamsFromInterface, readMtu_allParamsFromInterface, eventList, args.Mtu );
                
                // Show only the ReadMTU result in the screen
                await this.OnFinish ( this, new Delegates.ActionFinishArgs ( readMtu_allParamsFromInterface, args.Mtu ) );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        private async Task OnRemoteDisconnect ( Delegates.ActionArgs args )
        {
            try
            {
                // Load parameters using the interface file
                ActionResult dataRead_allParamsFromInterface = await CreateActionResultUsingInterface ( args.Map, args.Mtu, null, ActionType.RemoteDisconnect );
                ActionResult readMtu_allParamsFromInterface  = await CreateActionResultUsingInterface ( args.Map, args.Mtu );

                // Write result in the DataRead file
                this.lastLogCreated = logger.RemoteDisconnect ( dataRead_allParamsFromInterface, readMtu_allParamsFromInterface, args.Mtu );
                
                // Show only the ReadMTU result in the screen
                await this.OnFinish ( this, new Delegates.ActionFinishArgs ( readMtu_allParamsFromInterface, args.Mtu ) );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        private async Task OnNodeDiscovery ( Delegates.ActionArgs args )
        {
            try
            {
                NodeDiscoveryResult result = args.Extra[ 0 ];
                NodeDiscoveryList nodeList = args.Extra[ 1 ];
                decimal probF1 = args.Extra[ 2 ];
                decimal probF2 = args.Extra[ 3 ];
                double  vswr   = args.Extra[ 4 ];

                // Prepares custom values that will be loaded using the interface
                var MtuId = await args.Map.MtuSerialNumber.GetValue ();
                Data.SetTemp ( "MtuId", MtuId.ToString () );

                string word = "Fail"; // NOT_ACHIEVED and EXCEPTION
                switch ( result )
                {
                    case NodeDiscoveryResult.GOOD     : word = "Good";     break;
                    case NodeDiscoveryResult.EXCELLENT: word = "Excelent"; break;
                }

                Data.SetTemp ( "ProcessResult",
                    $"{ word } " +
                    $"Number of DCUs { nodeList.CountUniqueNodesValidated } " +
                    $"F1 Reliability {( probF1 * 100 ).ToString ( "F2" )} Percent " +
                    $"F2 Reliability {( probF2 * 100 ).ToString ( "F2" )} Percent" );

                Data.SetTemp ( "VSWR", vswr / 1000.0 );

                string path = Path.Combine ( Mobile.NodePath,
                    $"MTUID{Data.Get.MtuId}-{DateTime.Now.ToString ( "MMddyyyyHH" )}-" +
                    $"{ DateTime.Now.Ticks }NodeDiscoveryLog.xml" );
                string subpath = path.Substring ( path.LastIndexOf ( "/" ) + 1 );
                Data.SetTemp ( "ProcessResultFileFull", path );
                Data.SetTemp ( "ProcessResultFile", subpath );

                // Write result in the NodeDiscovery file
                if ( result != NodeDiscoveryResult.EXCEPTION )
                    logger.NodeDiscovery ( nodeList, args.Mtu );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                this.OnError ();
            }
        }

        #endregion

        private ActionResult getBasciInfoResult ()
        {
            ActionResult result = new ActionResult ();
            
            result.AddParameter(new Parameter("Date", "Date/Time", GetProperty("Date")));
            result.AddParameter(new Parameter("User", "User", GetProperty("User")));
            result.AddParameter(new Parameter("MtuType", "MTU Type", Data.Get.MtuBasicInfo.Type));
            result.AddParameter(new Parameter("MtuId", "MTU ID", Data.Get.MtuBasicInfo.Id));
            
            foreach ( Parameter param in this.AdditionalScriptParameters )
                result.AddParameter ( param );
    
            return result;
        }

        #region Interface

        /// <summary>
        /// Generates the list of ALL parameters, using the XML interface file
        /// for the family of current MTU and the action performed.
        /// <para>
        /// This method does not filter depending on the output target,
        /// not taking into account the boolean tags 'log' ( file ) and 'interface' ( UI ).
        /// </para>
        /// <para>
        /// See <see cref="ActionType"/> for a list of available actions.
        /// </para>
        /// </summary>
        /// <param name="map"><see cref="MemoryMap"/> generated during the action</param>
        /// <param name="mtu"><see cref="Mtu"/> that represents current MTU</param>
        /// <param name="form">Write actions stores the set data in an intermediate form object</param>
        /// <param name="actionType">Type of the action</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// All the parameters required for the action type and log target, interface or log file.
        /// </para>
        /// </returns>
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
            InterfaceParameters[] parameters = this.config.getAllParamsFromInterface ( mtu, actionType );
            
            foreach ( InterfaceParameters parameter in parameters )
            {
                try
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
                                    Regex.IsMatch ( parameter.Source, DOTNET_IDS + "." + DOTNET_IDS ) )
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
                                    case IFACE_DATA  : if ( ! Data.Contains ( sourceProperty ) || // Library.Data class
                                                            string.IsNullOrEmpty ( value = Data.Get[ sourceProperty ].ToString () ) )
                                                         value = string.Empty;
                                                       break; 
                                    default          : value      = ( await map[ sourceProperty ].GetValue () ).ToString (); break; // MemoryMap.ParameterName
                                }
                            }
                            catch ( Exception e )
                            {
                                Utils.Print ( "Interface: Map Error: " + sourceProperty );
                                throw new Exception ();
                            }
                            if (!string.IsNullOrEmpty(value))
                            {

                                if (!sourceWhere.Equals(IFACE_FORM))
                                {
                                    string display = (parameter.Display.ToLower().StartsWith("global.")) ?
                                                        gType.GetProperty(parameter.Display.Split(new char[] { '.' })[1]).GetValue(global, null).ToString() :
                                                        parameter.Display;

                                    paramToAdd = new Parameter(parameter.Name, display, value, parameter.Source);
                                }
                                // To change "name" attribute to show in IFACE_FORM case
                                else
                                {
                                    paramToAdd.CustomParameter = parameter.Name;
                                    paramToAdd.source = parameter.Source;
                                }
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
                catch ( Exception e )
                {
                    Utils.PrintDeep ( "Error: Interface parameter '" + parameter.Name + "'" );
                }
            }
            
            // Add additional parameters ( from script ) for all actions except for the Add
            if ( form == null )
                foreach ( Parameter param in this.AdditionalScriptParameters )
                    result.AddParameter ( param );

            // Add additional parameters ( from Global.xml ) for DataRead action
            if ( actionType == ActionType.DataRead && Data.Contains("Options"))
                foreach ( Parameter param in Data.Get.Options )
                    result.AddParameter ( param );

            return result;
        }

        /// <summary>
        /// Generates the list of parameters associated with the indicated port index, using
        /// the XML interface file for the family of current MTU and the action performed.
        /// </summary>
        /// <param name="indexPort">Index of the MTU <see cref="Port"/> to analize</param>
        /// <param name="parameters">List of parameters associated to the ports in the XML interface file to analize</param>
        /// <param name="map"><see cref="MemoryMap"/> generated during the action</param>
        /// <param name="mtu"><see cref="Mtu"/> that represents current MTU</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// All the parameters required for the action type and log target, interface or log file, for the specified port.
        /// </para>
        /// </returns>
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

            // Port is disabled
            if ( ! await map[$"P{indexPort}StatusFlag"].GetValue () )
            {
                result.AddParameter ( new Parameter ( "Status", "Status", "Disabled", string.Empty, indexPort-1 ) );
            }
            // Port has not a Meter installed
            else if ( meterId <= 0 )
            {
                result.AddParameter(new Parameter("Status", "Status", "Not Installed",string.Empty,indexPort-1));
                result.AddParameter(new Parameter("MeterTypeId", "Meter Type ID", "000000000",string.Empty,indexPort-1));
                result.AddParameter(new Parameter("MeterReading", "Meter Reading", "Bad Reading",string.Empty,indexPort-1));
            }
            else
            {
                Meter meter = this.config.getMeterTypeById ( meterId );
                
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
                                    
                                    String tempReading    = tempReadingVal.ToString ();
                                    bool   useDummyDigits = this.config.useDummyDigits ();
                                    char   dummyDigit     = ( useDummyDigits ) ? 'X' : '0';

                                    // If the reading number of digits is bigger than the live digits,
                                    // remove the difference taking away the most significative digits
                                    // e.g. Reading = 123456 , LiveDigits = 4 , Result = 3456
                                    //
                                    // Otherwise, fill in the left with zeros
                                    // e.g. Reading = 123 , LiveDigits = 4 , Result = 0123
                                    //
                                    // If the reading has a decimal part and the number of decimal digits is
                                    // equal to or bigger than LiveDigits, the whole integer part will be removed
                                    // e.g. Reading = 123456.78 , LiveDigits = 6 , Result = 3456.78
                                    // e.g. Reading = 123456.78 , LiveDigits = 4 , Result = 56.78
                                    // e.g. Reading = 1234.5678 , LiveDigits = 4 , Result = .5678
                                    if ( meter.LiveDigits < tempReading.Length )
                                        tempReading = tempReading
                                            .Substring ( tempReading.Length - meter.LiveDigits - ( tempReading.IndexOf ('.') > -1 ? 1 : 0 ) );
                                    else
                                        tempReading = tempReading
                                            .PadLeft ( meter.LiveDigits, '0' );
                                    
                                    // Fill in the left with the character selected ( X o 0/zero )
                                    // e.g. Reading 123456.78 , LeadingDummy = 2 , Result = XXX23456.78
                                    if ( meter.LeadingDummy > 0 )
                                        tempReading = tempReading
                                            .PadLeft ( tempReading.Length + meter.LeadingDummy, dummyDigit );

                                    // Fill in the left with the character selected ( X o 0/zero )
                                    if ( meter.DummyDigits > 0 )
                                        tempReading = tempReading
                                            .PadRight ( tempReading.Length + meter.DummyDigits, dummyDigit );
                                    
                                    // If the reading does not have a decimal part and the Meter scale is greater than
                                    // zero, convert the number to a floating point type, adding the point/separator
                                    // e.g. Reading 12345678 , LiveDigits = 7 , Scale = 3 , Result = 23456.78
                                    if ( meter.Scale > 0 &&
                                         tempReading.IndexOf ( "." ) == -1 )
                                        tempReading = tempReading
                                            .Insert ( tempReading.Length - meter.Scale, "." );
                                    
                                    // Fill in the right with zeros and add a hyphen in between
                                    // e.g. Reading 12345678 , LiveDigits = 7 , PaintedDigits = 2 , Result = 23456780 - 00
                                    if ( meter.PaintedDigits > 0 &&
                                         useDummyDigits )
                                        tempReading = tempReading
                                            .PadRight ( tempReading.Length + meter.PaintedDigits, '0' )
                                            .Insert ( tempReading.Length, " - " );
    
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
                        if ( await ValidateCondition ( parameter.Conditional, map, mtu, indexPort, meter ) )
                        {
                            string value          = string.Empty;
                            string sourceWhere    = string.Empty;
                            string sourceProperty = parameter.Name;

                            if ( ! string.IsNullOrEmpty ( parameter.Source ) &&
                                    Regex.IsMatch ( parameter.Source, DOTNET_IDS + "." + DOTNET_IDS ) )
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
                                    case IFACE_DATA  : if ( ! Data.Contains ( sourceProperty ) || // Library.Data class
                                                            string.IsNullOrEmpty ( value = Data.Get[ sourceProperty ].ToString () ) )
                                                         value = string.Empty;
                                                       break;
                                    default          : // MemoryMap.ParameterName
                                                       // Some registers ( e.g. RDD-related ) do not have port prefix because the MTU can only have one connnected
                                                       if ( map.ContainsMember ( sourceProperty ) )
                                                            value = ( await map[ sourceProperty ].GetValue () ).ToString ();
                                                       else value = ( await map[ sourceProperty = PORT_PREFIX + indexPort + sourceProperty ].GetValue () ).ToString ();
                                                       break;
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

        /// <summary>
        /// Evaluates the conditions listed in the parameters in the XML interface file, indicating
        /// whether each parameter should be added to the generated log or not.
        /// <para>
        /// See <see cref="ConditionObjet"/> for the nested class used to map parameters conditions.
        /// </para>
        /// </summary>
        /// <param name="conditionStr">Conditional sentence of the parameter, which can be an empty string</param>
        /// <param name="map"><see cref="MemoryMap"/> generated during the action</param>
        /// <param name="mtu"><see cref="Mtu"/> that represents current MTU</param>
        /// <param name="portIndex">Index of the MTU <see cref="Port"/> to analize</param>
        /// <param name="meter"><see cref="Meter"/> used in the specified port</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Indicates whether the parameter should be added to the log or not.
        /// </para>
        /// </returns>
        private async Task<bool> ValidateCondition (
            string conditionStr,
            dynamic map,
            Mtu mtu,
            int portIndex = 1,
            Meter meter = null )
        {
            if ( string.IsNullOrEmpty ( conditionStr ) )
                return true;

            try
            {
                /* Por no liar mucho el tema, no se tendra en cuenta el orden de las operaciones, sino unicamente
                   el anidamiento, si bien al calcular el resultado de cada nivel se operara de izquierda a derecha
                   pej. A + ( B x ( C x D ) + E ) --se-tratara-asi--> A + ( B + E x ( C x D ) )
                   Primero se calculan los valores por cada nivel de anidamiento
                   1º A | 2º B + E | 3º C x D
                   Y despues se empiezan operar de dentro hacia fuera
                   1º CD x BE | 2º CDBE + A
                  
                   Orden esperado       : B x CD + E -> 1 x 0 + 1 = 0 + 1 = 1
                   Orden por anidamiento: B + E x CD -> 1 + 1 x 0 = 2 x 0 = 0
                  
                   Pero la manera de solucionarlo es facil, haciendo uso de los parentesis
                   A + ( ( B x ( C x D ) ) + E )
                   Calculo de valores por anidamiento
                   1º A | 2º E | 3º B | 4º C x D
                   Operar de dentro hacia afuera
                   1º CD x B | 2º CDB + E | 3º CDBE + A
                */
                
                List<ConditionObjet> conditions = new List<ConditionObjet> ();
                MatchCollection matches = Regex.Matches ( conditionStr.Trim ().Replace ( " ", string.Empty ), REGEX_IFS, RegexOptions.Compiled );
                foreach ( Match m in matches.Cast<Match> ().ToList () )
                    conditions.Add (
                        new ConditionObjet (
                            Uri.UnescapeDataString ( m.Groups[ 1 ].Value ),     // + , |
                            Uri.UnescapeDataString ( m.Groups[ 2 ].Value ),     // (
                            Uri.UnescapeDataString ( m.Groups[ 3 ].Value ),     // )
                            Uri.UnescapeDataString ( m.Groups[ 4 ].Value ),     // Type/Class.Property
                            Uri.UnescapeDataString ( m.Groups[ 5 ].Value ),     // = , lt , gt , !
                            Uri.UnescapeDataString ( m.Groups[ 6 ].Value ) ) ); // Value
                
                #region Calculate using nesting

                // Calculates result for each level nesting
                // Types and info necessary to get value for each property
                string port   = PORT_PREFIX + portIndex;
                Global global = this.config.Global;
                Type   gType  = global.GetType ();
                Type   pType  = typeof ( Port );
                // Variables used to calculate each result
                int currentNestedLevel = 0;
                int currentFinalResult = 0;
                int prevFinalResult    = 0;
                List<int>  finalResults   = new List<int>  () { 0 };
                List<bool> blockCondition = new List<bool> () { false };
                List<int>  blockParent    = new List<int>  () { 0 };
                foreach ( ConditionObjet condition in conditions )
                {
                    // Init a new nested block "("...
                    if ( condition.InitNewBlock )
                    {
                        blockCondition.Add ( ! condition.CondConcatenate.Equals ( IFACE_OR ) ); // True AND'+', False OR'|'
                        finalResults  .Add ( 0 );
                        blockParent   .Add ( currentNestedLevel );
                        currentNestedLevel++;
                        prevFinalResult    = currentFinalResult;
                        currentFinalResult = finalResults.Count - 1;
                        
                        Console.WriteLine (
                            "INIT block [ New Parent: " + currentNestedLevel + " , New FinalResultIndex: " + currentFinalResult + " ] " +
                            "-> And: " + condition.IsAnd + " Or: " + condition.IsOr );
                        
                        continue;
                    }
                    // Finish current nested block ...")"
                    else if ( condition.FinishBlock )
                    {
                        currentNestedLevel--;
                        currentFinalResult = prevFinalResult--;
                        
                        Console.WriteLine ( "FINISH [ Return to Parent: " + currentNestedLevel + " , Return to FinalResultIndex: " + currentFinalResult + " ]" );
                        
                        continue;
                    }
                    
                    int      result       = 0;
                    string   currentValue = string.Empty;
                    string[] condMembers  = condition.Key.Split ( new char[]{ '.' } ); // Class.Property
                    string   condProperty = ( condMembers.Length > 1 ) ? condMembers[ 1 ] : condMembers[ 0 ]; // Property

                    #region Get value

                    switch ( condMembers[ 0 ] )
                    {
                        case IFACE_PORT  : currentValue = pType.GetProperty ( condProperty ).GetValue ( mtu.Ports[ portIndex - 1 ] ).ToString (); break;
                        case IFACE_ACTION: currentValue = this .GetProperty ( condProperty ); break; // User, Date or Type
                        case IFACE_MTU   : currentValue = mtu  .GetProperty ( condProperty ); break; // Mtu class
                        case IFACE_METER : currentValue = meter.GetProperty ( condProperty ); break; // Meter
                        case IFACE_GLOBAL: currentValue = gType.GetProperty ( condProperty ).GetValue ( global, null ).ToString(); break; // Global class
                        case IFACE_DATA  : if ( ! Data.Contains ( condProperty ) || // Library.Data class
                                                string.IsNullOrEmpty ( currentValue = Data.Get[ condProperty ].ToString () ) )
                                            currentValue = string.Empty;
                                        break;
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
                        float numCurrentValue, numValue;
                        bool   isNumberCurrent = float.TryParse ( currentValue,    out numCurrentValue );
                        bool   isNumber        = float.TryParse ( condition.Value, out numValue        );
                        bool   bothNumber      = isNumberCurrent && isNumber;
                        string lowerCurrent    = currentValue.ToLower ();
                        string lower           = condition.Value.ToLower ();

                        if ( condition.IsEqual   &&   lowerCurrent.Equals ( lower ) ||
                             condition.IsNot     && ! lowerCurrent.Equals ( lower ) ||
                             condition.IsLess    && bothNumber && numCurrentValue < numValue ||
                             condition.IsGreater && bothNumber && numCurrentValue > numValue )
                            result = 1; // Ok
                    }

                    #endregion

                    Console.WriteLine ( "   " + condition.Key + " == " + condition.Value + " -> " + result +
                        " [ Parent: " + ( currentNestedLevel - 1 ) + " , FinalResultIndex: " + currentFinalResult + " ]" );

                    // Concatenate results
                    if ( ! condition.InitNewBlock )
                    {
                        if      ( condition.IsOr  ) finalResults[ currentFinalResult ] += result; // If one condition validate, pass
                        else if ( condition.IsAnd ) finalResults[ currentFinalResult ] *= result; // All conditions have to validate
                    }
                    else finalResults[ currentFinalResult ]  = result;

                    Console.WriteLine ( "   Final = " + result + " -> FinalResult[" + currentFinalResult + "] = " + finalResults[ currentFinalResult ] );
                }
                
                #endregion

                #region Calculate inside out

                Console.WriteLine ( "----" );

                Console.WriteLine ( conditionStr );
		
		        Console.WriteLine ();
		
		        for ( int i = 0; i < finalResults.Count; i++ )
		        {
		            Console.WriteLine (
		                "Group: " + i +
		                " Parent: " + blockParent[ i ] +
		                " -> FinalResult: " + finalResults[ i ] +
		                " PreCondition: " + ( ( blockCondition[ i ] ) ? "AND" : "OR" ) );
		        }
		
		        Console.WriteLine ();

                /*
                e.g. "Level0.P1=true | ( Level1.P1=3 + ( Level2.P1=1 + Level2.P2=1 ) | ( Level2.P3=2 + Level2.P3=2 ) | Level1.P2=4 )"
                Level: 0 Parent: 0 -> FinalResult: 1 PreCondition: |
                Level: 1 Parent: 0 -> FinalResult: 2 PreCondition: |
                Level: 2 Parent: 1 -> FinalResult: 1 PreCondition: +
                Level: 3 Parent: 1 -> FinalResult: 1 PreCondition: |
                FinalResults = 1 | ( 1 + ( 1 ) | ( 1 ) | 1 ) = 1 | ( 2 + ( 1 ) | ( 1 ) )
                
                Max possible group parent n-1
                e.g. 1 + ( 1 + ( 1 + ( 1 + ( 1 + ) ) ) )
                5 niveles de animdamiento [MinIndex 0, MaxIndex 4] -> MaxParent = MaxIndex-1 = 4-1 = 3 -> Count-2 = 5-2 = 3
                
                Max parent: NumIndex 4 - 2 = 2
                Parent 2 -> ...
                Parent 1 -> 2 + 1 | 1 = 2 * 1 + 1 = 3 => Remove all entries with Parent1 as parent and modify Index1.FinalResult
                Parent 0 -> 1 | 3 = 1 + 3 = 4
                */
                
                int used        = 0;
                int finalResult = 0;
                int topParent   = ( finalResults.Count - 2 < 0 ) ? 0 : finalResults.Count - 2;
                for ( int parent = topParent; parent >= 0; parent-- )
                {
                    used        = 0;
                    finalResult = 0;
                    
                    Console.WriteLine ( "Parent: " + parent + " [ For " + parent + " >= 0 ]" );
                    
                    // Iterates all groups, first finding current parent ( travels the sentence from left to right )
                    int groupLimit = finalResults.Count;
                    for ( int group = 0; group < groupLimit; group++ )
                    {
                        Console.WriteLine ( "    Group: " + group + " [ For 0 < " + groupLimit + " ]" );
                        
                        if ( group == parent )
                        {
                            finalResult = finalResults[ group ];
                            
                            Console.WriteLine ( "        Parent: FinalResult = " + finalResult );
                        }
                        else if ( blockParent[ group ] == parent )
                        {
                            used++;
                            
                            int  entryFinalResult = finalResults  [ group ];
                            bool andCondition     = blockCondition[ group ];
                            
                            int preFinalResult = finalResult; // ONLY FOR DEBUG
                            
                            if ( ! andCondition ) finalResult += entryFinalResult; // If one condition validate, pass
                            else                  finalResult *= entryFinalResult; // All conditions have to validate
                            
                            Console.WriteLine (
                                "        Child: FinalResult = " + preFinalResult +
                                ( ( andCondition ) ? " x" : " +" ) +
                                " " + entryFinalResult + " = " +
                                finalResult + " [ Used: " + used + " ]" );
                        }
                    }
                    
                    if ( parent > 0 &&
                        used   > 0 )
                    {
                        Console.WriteLine ( "    Remove elements: " + ( finalResults.Count - 1 - used ) + " / " + finalResults.Count );
                        
                        finalResults.RemoveRange ( finalResults.Count - 1 - used, used );
                        finalResults[ parent ] = finalResult;
                        
                        Console.WriteLine (
                            "    FinalResult = " + finalResult +
                            " | FinalResults.Count = " + finalResults.Count );
                
                        int i = 0;
                        foreach ( int result in finalResults )
                            Console.WriteLine ( "        · Group " + i++ + " = " + result );
                    }
                }
                
                #endregion

                Console.WriteLine ( "----" );
                
                Console.WriteLine ( finalResult );

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
