using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lexi.Interfaces;
using Library;
using MTUComm.actions;
using Library.Exceptions;
using MTUComm.MemoryMap;
using Lexi;
using Xml;

using ActionType               = MTUComm.Action.ActionType;
using FIELD                    = MTUComm.actions.AddMtuForm.FIELD;
using APP_FIELD                = MTUComm.ScriptAux.APP_FIELD;
using EventLogQueryResult      = MTUComm.EventLogList.EventLogQueryResult;
using ParameterType            = MTUComm.Parameter.ParameterType;
using ENCRYPTION               = Xml.Mtu.ENCRYPTION;
using LexiAction               = Lexi.Lexi.LexiAction;
using LogFilterMode            = Lexi.Lexi.LogFilterMode;
using LogEntryType             = Lexi.Lexi.LogEntryType;
using NodeType                 = Lexi.Lexi.NodeType;
using NodeDiscoveryQueryResult = MTUComm.NodeDiscoveryList.NodeDiscoveryQueryResult;

namespace MTUComm
{
    /// <summary>
    /// Class that contains most of the logic of the application, with the methods used to
    /// perform all the supported actions, working with MTUs and Meters connected to them.
    /// <para>
    /// See <see cref="Action.ActionType"/> for a list of available actions.
    /// </para>
    /// </summary>
    /// <seealso cref="Action"/>
    public class MTUComm
    {
        #region Constants

        /// <summary>
        /// All possible results for the Node Discovery process.
        /// <para>&#160;</para>
        /// </para>
        /// <list type="NodeDiscoveryResult">
        /// <item>
        ///     <term>NodeDiscoveryResult.GOOD</term>
        ///     <description>The Node Discovery process was completed validating at least the minimum nodes/DCUs required</description>
        /// </item>
        /// <item>
        ///     <term>NodeDiscoveryResult.EXCELLENT</term>
        ///     <description>The Node Discovery process was completed validating the same or more than the desired nodes/DCUs</description>
        /// </item>
        /// <item>
        ///     <term>NodeDiscoveryResult.NOT_ACHIEVED</term>
        ///     <description>The Node Discovery process has finished validating fewer nodes/DCUs than the minimum required</description>
        /// </item>
        ///  <item>
        ///     <term>NodeDiscoveryResult.EXCEPTION</term>
        ///     <description>Some uncontrollable exception occurred during the execution of the Node Discovery process</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public enum NodeDiscoveryResult
        {
            GOOD,
            EXCELLENT,
            NOT_ACHIEVED,
            EXCEPTION
        }

        private const int BASIC_READ_1_ADDRESS    = 0;
        private const int BASIC_READ_1_DATA       = 32;
        private const int BASIC_READ_2_ADDRESS    = 244;
        private const int BASIC_READ_2_DATA       = 1;
        private const int DEFAULT_OVERLAP         = 6;
        private const int DEFAULT_LENGTH_AES      = 16;
        private const int SAME_MTU_ADDRESS        = 0;
        private const int SAME_MTU_DATA           = 10;
        private const int IC_OK                   = 0;
        private const int IC_NOT_ACHIEVED         = 1;
        private const int IC_EXCEPTION            = 2;
        private const int WAIT_BTW_TURNOFF        = 500;
        private const int WAIT_BTW_IC             = 1000;
        private const int WAIT_IC_FAILS           = 4000;
        private const int WAIT_BEFORE_READ        = 1000;
        private const int TIMES_TURNOFF           = 3;
        public static readonly int N_ATTEMPTS_LEXI        = 2;
        public static readonly int WAIT_BTW_ATTEMPTS_LEXI = 1;
        private const int ONLY_ONE_ATTEMPT_CMD    = 1;
        private const int N_ATTEMPTS_CMD          = 2;
        private const int WAIT_BTW_ATTEMPTS_CMD   = 1;
        private const int DATA_READ_END_DAYS      = 60; // In STARProgrammer code is used .AddSeconds ( 86399 ) -> 86399 / 60 / 24 = 59,999 = 60 days
        private const byte CMD_INIT_EVENT_LOGS    = 0x13; // 19
        private const byte CMD_NEXT_EVENT_LOG     = 0x14; // 20
        private const byte CMD_REPE_EVENT_LOG     = 0x15; // 21
        private const int CMD_NEXT_EVENT_RES_1    = 25;   // Response ACK with log entry [0-24] = 25 bytes
        private const int CMD_NEXT_EVENT_RES_2    = 5;    // Response ACK with no data [0-4] = 5 bytes
        private const int CMD_BYTE_RES            = 2;    // Response second byte is result or status ( 0 = Log entry, 1 or 2 = No data )
        private const byte CMD_NEXT_EVENT_DATA    = 0x00; // ACK with log entry
        private const byte CMD_NEXT_EVENT_EMPTY   = 0x01; // ACK without log entry ( query complete )
        private const byte CMD_NEXT_EVENT_BUSY    = 0x02; // ACK without log entry ( MTU is busy or some error trying to recover next log entry )
        private const int WAIT_BEFORE_LOGS        = 10000; // The host device should delay for at least 2 seconds to give the MTU time to begin the query
        private const int WAIT_BTW_LOG_ERRORS     = 1000;
        private const int WAIT_BTW_LOGS           = 100;
        private const int WAIT_AFTER_EVENT_LOGS   = 1000;
        private const int CMD_VSWR                = 0x23; // 35 VSWR Test
        private const int CMD_INIT_NODE_DISC      = 0x18; // 24 Node discovery initiation command
        private const int CMD_INIT_NODE_DISC_RES  = 5; // Response ACK with result [0-4] = 5 bytes
        private const int CMD_INIT_NODE_DISC_NOT  = 0x00; // Node discovery not initiated
        private const int CMD_INIT_NODE_DISC_INI  = 0x01; // Node discovery initiated
        private const int CMD_QUERY_NODE_DISC     = 0x19; // 25 Start/Reset node discovery response query
        private const int CMD_QUERY_NODE_DISC_RES = 5; // Response ACK with result [0-4] = 5 bytes
        private const int CMD_QUERY_NODE_DISC_NOT = 0x00; // The MTU is busy
        private const int CMD_QUERY_NODE_DISC_INI = 0x01; // The MTU is ready for query
        private const int CMD_NEXT_NODE_DISC      = 0x1A; // 26 Get next node discovery response
        private const int CMD_NEXT_NODE_1         = 10; // ACK with general information [0-9] = 10 bytes
        private const int CMD_NEXT_NODE_2         = 26; // ACK with log entry [0-25] = 26 bytes
        private const int CMD_NEXT_NODE_3         = 5; // ACK without log entry [0-4] = 5 bytes
        private const byte CMD_NEXT_NODE_DATA     = 0x00; // ACK with node entry
        private const byte CMD_NEXT_NODE_EMPTY    = 0x01; // ACK without node entry ( query complete )
        private const int WAIT_BEFORE_START_NODE  = 3000;
        private const int WAIT_BEFORE_GET_NODES   = 1000;
        private const int WAIT_BTW_NODE_ERRORS    = 1000;
        private const int WAIT_BTW_NODES          = 100;
        private const int CMD_ENCRYP_MAX          = 3;
        private const int CMD_ENCRYP_OLD_MAX      = 5;
        private const int CMD_LOAD_ENCRYP         = 0x1B;
        private const int CMD_READ_ENCRYP         = 0x1C;
        private const int CMD_READ_ENCRYP_RES_2   = 64;
        private const int CMD_READ_ENCRYP_RES_3   = 32;
        private const int CMD_GEN_ENCRYP_KEYS     = 0x1D;
        private const string ERROR_LOADDEMANDCONF = "DemandConfLoadException";
        private const string ERROR_LOADMETER      = "MeterLoadException";
        private const string ERROR_LOADMTU        = "MtuLoadException";
        private const string ERROR_LOADALARM      = "AlarmLoadException";
        private const string ERROR_NOTFOUNDMTU    = "MtuNotFoundException";
        private const string ERROR_LOADINTERFACE  = "InterfaceLoadException";
        private const string ERROR_LOADGLOBAL     = "GlobalLoadException";
        private const string ERROR_NOTFOUNDMETER  = "MeterNotFoundException";

        #endregion

        #region Events

        /// <summary>
        /// Event invoked only if the <see cref="Action.ActionType"/>.BasicRead
        /// action completes successfully, with no exceptions.
        /// <para>
        /// See <see cref="Action.OnBasicRead"/> for the associated method ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.ActionHandler OnBasicRead;

        /// <summary>
        /// Event invoked only if the <see cref="Action.ActionType"/>.ReadFabric
        /// action completes successfully, with no exceptions.
        /// <para>
        /// See <see cref="Action.OnReadFabric"/> for the associated method ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.ActionHandler OnReadFabric;

        /// <summary>
        /// Event invoked only if the <see cref="Action.ActionType"/>.ReadMtu
        /// action completes successfully, with no exceptions.
        /// <para>
        /// See <see cref="Action.OnReadMtu"/> for the associated method ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.ActionHandler OnReadMtu;

        public event Delegates.ActionHandler OnNodeDiscovery;

        /// <summary>
        /// Event invoked only if the <see cref="Action.ActionType"/>.TurnOffMtu
        /// action completes successfully, with no exceptions.
        /// <para>
        /// See <see cref="Action.OnTurnOnOffMtu"/> for the associated method ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.ActionHandler OnTurnOffMtu;

        /// <summary>
        /// Event invoked only if the <see cref="Action.ActionType"/>.TurnOnMtu
        /// action completes successfully, with no exceptions.
        /// <para>
        /// See <see cref="Action.OnTurnOnOffMtu"/> for the associated method ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.ActionHandler OnTurnOnMtu;

        /// <summary>
        /// Event invoked only if the <see cref="Action.ActionType"/>.DataRead
        /// action completes successfully, with no exceptions.
        /// <para>
        /// See <see cref="Action.OnDataRead"/> for the associated method ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.ActionHandler OnDataRead;

        /// <summary>
        /// Event invoked only if the writing ( <see cref="ActionType"/>.Add|Replace )
        /// action completes successfully, with no exceptions.
        /// <para>
        /// See <see cref="Action.OnAddMtu"/> for the associated method ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.ActionHandler OnAddMtu;

        /// <summary>
        /// Event that can be invoked during the execution of any action, for
        /// example to update the visual feedback by modifying the text of a label control.
        /// <para>
        /// See <see cref="Action.OnProgress"/> for the event associated ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.ProgresshHandler OnProgress;

        /// <summary>
        /// Event invoked if the action does not complete successfully or if it launches an exception.
        /// <para>
        /// See <see cref="Action.OnError"/> for the event associated ( XAML <- Action <- MTUComm ).
        /// </para>
        /// </summary>
        public event Delegates.Empty OnError;

        #endregion

        #region Attributes

        private Lexi.Lexi lexi;
        private Configuration configuration;
        private Global global;
        private MTUBasicInfo mtuBasicInfo;
        private Mtu mtu;
        private Boolean isPulse = false;
        private Boolean mtuHasChanged;
        private bool basicInfoLoaded = false;
        private AddMtuLog addMtuLog;

        #endregion

        #region Initialization

        public MTUComm(ISerial serial, Configuration configuration)
        {
            this.configuration = configuration;
            this.global = this.configuration.Global;
            mtuBasicInfo = new MTUBasicInfo(new byte[BASIC_READ_1_DATA + BASIC_READ_2_DATA]);
            lexi = new Lexi.Lexi(serial, Data.Get.IsIOS ? 10000 : 20000 );
            
            Singleton.Set = lexi;
        }

        #endregion

        #region Launch Actions

        /// <summary>
        /// The entry point of the action logic, loading the basic MTU data required,
        /// acting as the distributor, invoking the correct method for each action.
        /// <para>
        /// Also, is the highest point where exceptions can bubble up/arise, because
        /// it is easier to control how to manage exceptions at a single point in the app.
        /// </para>
        /// <para>
        /// See <see cref="LoadMtuAndMetersBasicInfo"/> to recover basic data from the MTU.
        /// </para>
        /// </summary>
        /// <param name="type">Current action type ( AddMtu, ReplaceMeter,.. )</param>
        /// <param name="args">Arguments required for some actions</param>
        /// <seealso cref="AddMtu(Action)"/>
        /// <seealso cref="AddMtu(dynamic, string, Action)"/>
        /// <seealso cref="BasicRead"/>
        /// <seealso cref="DataRead"/>
        /// <seealso cref="DataRead(Action)"/>
        /// <seealso cref="InstallConfirmation"/>
        /// <seealso cref="ReadFabric"/>
        /// <seealso cref="ReadMtu"/>
        /// <seealso cref="TurnOnOffMtu(bool)"/>
        public async void LaunchActionThread (
            ActionType type,
            params object[] args )
        {
            try
            {
                // Avoid to kill app on error message
                Data.Set ( "ActionInitialized", true );

                // Avoid to load more than one time the basic info for the same action,
                // because an action can be launched multiple times because of exceptions
                // that cancel the action but not move to the main menu and could be happen
                // that perform the basic read with a different MTU
                if ( ! this.basicInfoLoaded )
                {
                    await this.LoadMtuAndMetersBasicInfo ();
                    
                    if ( Singleton.Has<Action> () )
                        Singleton.Get.Action.SetCurrentMtu ( this.mtuBasicInfo );
                }

                switch ( type )
                {
                    case ActionType.AddMtu:
                    case ActionType.AddMtuAddMeter:
                    case ActionType.AddMtuReplaceMeter:
                    case ActionType.ReplaceMTU:
                    case ActionType.ReplaceMeter:
                    case ActionType.ReplaceMtuReplaceMeter:
                        // Interactive and Scripting
                        if ( args.Length > 1 )
                             await Task.Run ( () => AddMtu ( ( AddMtuForm )args[ 0 ], ( string )args[ 1 ], ( Action )args[ 2 ] ) );
                        else await Task.Run ( () => AddMtu ( ( Action )args[ 0 ] ) );
                        break;
                    case ActionType.DataRead:
                        // Scripting and Interactive
                        if ( args.Length == 1 )
                             await Task.Run ( () => DataRead ( ( Action )args[ 0 ] ) );
                        else await Task.Run ( () => DataRead () );
                        break;
                    case ActionType.MtuInstallationConfirmation: await Task.Run ( () => InstallConfirmation () ); break;
                    case ActionType.ReadFabric : await Task.Run ( () => ReadFabric () ); break;
                    case ActionType.ReadMtu    : await Task.Run ( () => ReadMtu () ); break;
                    case ActionType.TurnOffMtu : await Task.Run ( () => TurnOnOffMtu ( false ) ); break;
                    case ActionType.TurnOnMtu  : await Task.Run ( () => TurnOnOffMtu ( true  ) ); break;
                    case ActionType.BasicRead  : await Task.Run ( () => BasicRead () ); break;
                    default: break;
                }

                // Reset initialization status
                Data.Set ( "ActionInitialized", false );
            }
            // MTUComm.Exceptions.MtuTypeIsNotFoundException
            catch ( Exception e )
            {
                Errors.LogRemainExceptions ( e );
                
                this.OnError ();
            }
        }

        #endregion

        #region Actions

        #region AutoDetection Encoders

        /// <summary>
        /// Process performed only by MTU with ports compatible with Encoder or E-Coders,
        /// to automatically recover the Meter protocol and live digits that will
        /// be used to filter the Meter list and during selected Meter validation.
        /// <para>
        /// See <see cref="CheckSelectedEncoderMeter(int)"/> to validate a Meter for current MTU.
        /// </para>
        /// </summary>
        /// <param name="mtu">Current MTU</param>
        /// <param name="portIndex">Port number to read from the MTU</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Indicates whether the auto-detection worked or not.
        /// </para>
        /// </returns>
        /// <exception cref="EncoderAutodetectNotAchievedException">( Used internally, not bubbling up )</exception>
        /// <exception cref="EncoderAutodetectException">( Used internally, not bubbling up )</exception>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        public async Task<bool> AutodetectMeterEncoders (
            Mtu mtu,
            int portIndex = 1 )
        {
            this.mtu     = mtu;
            dynamic map  = this.GetMemoryMap ();
            bool isPort1 = ( portIndex == 1 );

            // Check if the port is enabled
            if (!await map[$"P{portIndex}StatusFlag"].GetValue())
                return false;

            try
            {
                // Clear/reset previous auto-detected values
                if ( isPort1 )
                {
                    await map.P1EncoderProtocol  .SetValueToMtu ( 0 );
                    await map.P1EncoderLiveDigits.SetValueToMtu ( 0 );
                }
                else
                {
                    await map.P2EncoderProtocol  .SetValueToMtu ( 0 );
                    await map.P2EncoderLiveDigits.SetValueToMtu ( 0 );
                }
            
                // Force MTU to run Meter/Enconder auto-detection for selected ports
                if ( isPort1 )
                     await map.P1EncoderAutodetect.ResetByteAndSetValueToMtu ( true );
                else await map.P2EncoderAutodetect.ResetByteAndSetValueToMtu ( true );
            
                // Check until recover data from the MTU, but no more than 50 seconds
                // MTU returns two values, Protocol and LiveDigits
                int  count      = 1;
                int  wait       = 2;
                int  time       = 50;
                int  max        = ( int )( time / wait );  // Seconds / Seconds = Rounded max number of iterations
                int  protocol   = -1;
                int  liveDigits = -1;
                bool ok;
                do
                {
                    try
                    {
                        if ( isPort1 )
                             protocol = await map.P1EncoderProtocol.GetValueFromMtu (); // Encoder Type: 1=ARBV, 2=ARBVI, 4=ABB, 8=Sensus
                        else protocol = await map.P2EncoderProtocol.GetValueFromMtu ();
                        
                        if ( isPort1 )
                             liveDigits = await map.P1EncoderLiveDigits.GetValueFromMtu (); // Encoder number of digits
                        else liveDigits = await map.P2EncoderLiveDigits.GetValueFromMtu ();
                    }
                    catch ( Exception e )
                    {
                        Utils.Print ( "AutodetectMetersEcoders: " + e.GetType ().Name );
                    }
                    finally
                    {
                        Utils.Print ( "AutodetectMetersEcoders: Protocol " + protocol + " LiveDigits " + liveDigits );
                    }
                    
                    // It is usual for LiveDigits to take value 8 but only for a moment
                    // and then reset to zero before take the final/real value
                    if ( ! ( ok = ( protocol == 1 || protocol == 2 || protocol == 4 || protocol == 8 ) && liveDigits > 0 ) )
                        await Task.Delay ( wait * 1000 );
                }
                while ( ! ok &&
                        ++count <= max );
                
                if ( ok )
                {
                    Port port = ( isPort1 ) ? mtu.Port1 : mtu.Port2;
                    port.MeterProtocol   = protocol;
                    port.MeterLiveDigits = liveDigits;
                
                    return true;
                }
                else throw new EncoderAutodetectNotAchievedException ( time.ToString () );
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     Errors.LogErrorNowAndContinue ( new EncoderAutodetectException (), portIndex );
                else Errors.LogErrorNowAndContinue ( e, portIndex );
            }
            
            return false;
        }
        
        /// <summary>
        /// Logic of Meters auto-detection process extracted from AutodetectMetersEcoders
        /// method, for an easy and readable reuse of the code for the two MTUs ports.
        /// <para>
        /// See <see cref="AutodetectMeterEncoders(Mtu,int)"/> to detect automatically
        /// the Meter protocol and live digits of compatible Meters for current MTU.
        /// </para>
        /// </summary>
        /// <param name="portIndex">Port number to read from the MTU</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <exception cref="EncoderMeterFFException"></exception>
        /// <exception cref="EncoderMeterFEException"></exception>
        /// <exception cref="EncoderMeterFDException"></exception>
        /// <exception cref="EncoderMeterFCException"></exception>
        /// <exception cref="EncoderMeterFBException"></exception>
        /// <exception cref="EncoderMeterUnknownException"></exception>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error )</exception>
        private async Task CheckSelectedEncoderMeter (
            int portIndex = 1 )
        {
            // Only for MTU Encoders ( Check MTU ports, Meters supported, to know that -> Type "E" )
            dynamic map = this.GetMemoryMap ();
            
            Port port = ( portIndex == 1 ) ? this.mtu.Port1 : this.mtu.Port2;
            int protocol   = port.MeterProtocol;
            int liveDigits = port.MeterLiveDigits;
            
            try
            {
                // Write back values to the MTU
                if ( portIndex == 1 )
                     await map.P1EncoderProtocol.SetValueToMtu ( protocol );
                else await map.P2EncoderProtocol.SetValueToMtu ( protocol );
                
                if ( portIndex == 1 )
                     await map.P1EncoderLiveDigits.SetValueToMtu ( liveDigits );
                else await map.P2EncoderLiveDigits.SetValueToMtu ( liveDigits );
                
                // Activates flag to read Meter
                await map.ReadMeter.SetValueToMtu ( true );
                
                await Task.Delay ( WAIT_BEFORE_READ );
                
                // Check for errors
                byte erCode;
                if ( portIndex == 1 )
                     erCode = Convert.ToByte ( await map.P1ReadingErrorCode.GetValueFromMtu () );
                else erCode = Convert.ToByte ( await map.P2ReadingErrorCode.GetValueFromMtu () );
                
                switch ( erCode )
                {
                    case 0xFF: throw new EncoderMeterFFException ( "", portIndex ); // No reading / No response from Encoder
                    case 0xFE: throw new EncoderMeterFEException ( "", portIndex ); // Encoder has bad digit in reading
                    case 0xFD: throw new EncoderMeterFDException ( "", portIndex ); // Delta overflow
                    case 0xFC: throw new EncoderMeterFCException ( "", portIndex ); // Deltas purged / New install / Reset
                    case 0xFB: throw new EncoderMeterFBException ( "", portIndex ); // Encoder clock shorted
                    case 0x00: break; // No error
                    default  : throw new EncoderMeterUnknownException ( "", portIndex ); // Unknown error code
                }
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     throw new PuckCantCommWithMtuException ( "", portIndex );
                else throw e;
            }
        }

        #endregion

        #region Historical Read ( previously Data Read )

        /// <summary>
        /// In scripted mode this method overload is called before the main method,
        /// because it is necessary to translate the script parameters from Aclara into
        /// the app terminology and validate their values, removing unnecessary ones
        /// to avoid headaches.
        /// <para>
        /// See <see cref="DataRead"/> for the DataRead logic.
        /// </para>
        /// </summary>
        /// <param name="action">Current action type ( AddMtu, ReplaceMeter,.. )</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <exception cref="ScriptForOnePortButTwoEnabledException">( From ScriptAux.ValidateParams )</exception>
        /// <exception cref="ScriptForTwoPortsButMtuOnlyOneException">( From ScriptAux.ValidateParams )</exception>
        /// <exception cref="ScriptingAutoDetectMeterException">( From ScriptAux.ValidateParams )</exception>
        /// <exception cref="NumberOfDialsTagMissingScript">( Used internally, not bubbling up )</exception>
        /// <exception cref="DriveDialSizeTagMissingScript">( Used internally, not bubbling up )</exception>
        /// <exception cref="UnitOfMeasureTagMissingScript">( Used internally, not bubbling up )</exception>
        /// <exception cref="ScriptingAutoDetectTagsMissingScript">( From ScriptAux.ValidateParams )</exception>
        /// <exception cref="ScriptingAutoDetectMeterMissing">( From ScriptAux.ValidateParams )</exception>
        /// <exception cref="ScriptingAutoDetectNotSupportedException">( From ScriptAux.ValidateParams )</exception>
        /// <exception cref="ProcessingParamsScriptException">( From ScriptAux.ValidateParams )</exception>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error )</exception>
        /// <exception cref="DataRead () exceptions..."></exception>
        private async Task DataRead (
            Action action )
        {
            try
            {
                // Translate Aclara parameters ID into application's nomenclature
                var translatedParams = ScriptAux.TranslateAclaraParams ( action.GetParameters () );

                dynamic map = this.GetMemoryMap ( true );

                // Check if the second port is enabled
                bool port2enabled = await map.P2StatusFlag.GetValue ();

                // Validate script parameters ( removing the unnecessary ones )
                Dictionary<APP_FIELD,string> psSelected = ScriptAux.ValidateParams (
                    port2enabled, this.mtu, this.mtuBasicInfo, action, translatedParams );

                // Add parameters to Library.Data
                foreach ( var entry in psSelected )
                    Data.Set ( entry.Key.ToString (), entry.Value, true );

                var MtuId = await map.MtuSerialNumber.GetValue();
                var MtuStatus = await map.MtuStatus.GetValue();
                var accName = await map.P1MeterId.GetValue();

                Data.Set("AccountNumber", accName, true);
                Data.Set("MtuId", MtuId.ToString(), true);
                Data.Set("MtuStatus", MtuStatus, true);

                // Init DataRead logic using translated parameters
                await this.DataRead ();
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     throw new PuckCantCommWithMtuException ();
                else throw e;
            }
        }

        /// <summary>
        /// Process performed only by MTUs with the tag MtuDemand set to true in mtu.xml file,
        /// recovering all the MeterReading events saved in the MTU for the number of days indicated.
        /// <para>
        /// See <see cref="DataRead(Action)"/> for the entry point of the DataRead process in scripted mode.
        /// </para>
        /// <para>&#160;</para>
        /// <para/><para>
        /// MTU_Datalogging ( DataRead )
        /// </para>
        /// <list type="MTU_Datalogging">
        /// <item>
        ///     <term>Page 33 - 3.4</term>
        ///     <description>Accessing Log Information via the Local External Interface ( LExI )</description>
        /// </item>
        /// </list>
        /// <para>&#160;</para>
        /// <para>
        /// Y61063-DSD-Rev_G-Local_External_Interface_Specification
        /// </para>
        /// <list type="Y61063">
        /// <item>
        ///     <term>Page 37 - 4.2.3.18</term>
        ///     <description>Start Event Log Query</description>
        /// </item>
        /// <item>
        ///     <term>Page 38 - 4.2.3.19</term>
        ///     <description>Get Next Event Log Response</description>
        /// </item>
        /// <item>
        ///     <term>Page 39 - 4.2.3.20</term>
        ///     <description>Get Repeat Last Event Log Response</description>
        /// </item>
        /// </list>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <exception cref="MtuIsNotOnDemandCompatibleDevice"></exception>
        /// <exception cref="AttemptNotAchievedGetEventsLogException">( Used internally, not bubbling up )</exception>
        /// <exception cref="MtuIsBusyToGetEventsLogException">( Not in use )</exception>
        /// <exception cref="ActionNotAchievedGetEventsLogException"></exception>
        /// <exception cref="MtuHasChangeBeforeFinishActionException">( From CheckIsTheSameMTU )</exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error )</exception>
        /// <exception cref="ReadMtu_Logic exceptions..."></exception>
        /// <seealso cref="OnDataRead"/>
        public async Task DataRead ()
        {
            try
            {
                // Check if the MTU is an OnDemand MTU
                if ( ! this.mtu.MtuDemand )
                    throw new MtuIsNotOnDemandCompatibleDevice ();

                OnProgress ( this, new Delegates.ProgressArgs ( "Requesting event logs..." ) );

                DateTime end   = DateTime.UtcNow;
                DateTime start = end.Subtract ( new TimeSpan ( int.Parse ( Data.Get.NumOfDays ), 0, 0, 0 ) );

                byte[] data = new byte[ 10 ]; // 1+1+4x2
                data[ 0 ] = ( byte )LogFilterMode.Match;    // Only return logs that matches the Log Entry Filter Field specified
                data[ 1 ] = ( byte )LogEntryType.MeterRead; // The log entry filter to use
                Array.Copy ( Utils.GetTimeSinceDate ( start ), 0, data, 2, 4 ); // Start time
                Array.Copy ( Utils.GetTimeSinceDate ( end   ), 0, data, 6, 4 ); // Stop time

                // Start new event log query
                await this.lexi.Write (
                    CMD_INIT_EVENT_LOGS,
                    data,
                    N_ATTEMPTS_CMD,
                    WAIT_BTW_ATTEMPTS_CMD,
                    null,
                    null,
                    LexiAction.OperationRequest );

                await Task.Delay ( WAIT_BEFORE_LOGS );

                // Recover all logs registered in the MTU for the specified date range
                bool retrying        = false;
                int  maxAttempts     = ( Data.Get.IsFromScripting ) ? 20 : 5;
                int  maxAttemptsEr   = 2; // maxAttempts is for when the Mtu is busy and maxAttemptsEr for exceptions ( LExI,... )
                int  countAttempts   = 0;
                int  countAttemptsEr = 0;
                EventLogList eventLogList = new EventLogList ( start, end, ( LogFilterMode )data[ 0 ], ( LogEntryType )data[ 1 ] );
                //( byte[] bytes, int responseOffset ) fullResponse = ( null, 0 ); // echo + response
                LexiWriteResult fullResponse;
                while ( true )
                {
                    try
                    {
                        // Get next event log response command or Get repeat last event log response command
                        // NOTE: In MTU_Datalogging ( DataRead 3.4 ) indicates that Get repeat command has only two
                        // NOTE: possible responses, but if it is the same as relaunch the last Get next, should be has three
                        fullResponse =
                            await this.lexi.Write (
                                ( ! retrying ) ? CMD_NEXT_EVENT_LOG : CMD_REPE_EVENT_LOG,
                                null,
                                ONLY_ONE_ATTEMPT_CMD,
                                WAIT_BTW_ATTEMPTS_CMD,
                                new uint[]{ CMD_NEXT_EVENT_RES_1, CMD_NEXT_EVENT_RES_2 }, // ACK with log entry or without
                                new LexiFiltersResponse ( new ( int,int,byte )[] {
                                    ( CMD_NEXT_EVENT_RES_1, CMD_BYTE_RES, CMD_NEXT_EVENT_DATA  ), // Entry data included
                                    ( CMD_NEXT_EVENT_RES_2, CMD_BYTE_RES, CMD_NEXT_EVENT_EMPTY ), // Complete but without data
                                    ( CMD_NEXT_EVENT_RES_2, CMD_BYTE_RES, CMD_NEXT_EVENT_BUSY  )  // The MTU is busy, response not ready yet
                                } ),
                                LexiAction.OperationRequest );
                    }
                    catch ( Exception e )
                    {
                        // Is not own exception
                        if ( ! Errors.IsOwnException ( e ) )
                            throw new PuckCantCommWithMtuException ();

                        // Finish without perform the action
                        else if ( ++countAttemptsEr >= maxAttemptsEr )
                            throw new ActionNotAchievedGetEventsLogException ();

                        await Task.Delay ( WAIT_BTW_LOG_ERRORS );

                        Utils.Print ( "DataRead: Error trying to recover an event [ Attempts " + countAttemptsEr + " / " + maxAttemptsEr + " ]" );

                        // Try one more time
                        Errors.AddError ( new AttemptNotAchievedGetEventsLogException () );

                        // Try again, using this time Get Repeat Last Event Log Response command
                        // NOTE: Is very strange how works the MTU is a LExI command fails and you use the
                        // process RepeatLast because some times it recovers events previous to the current one
                        retrying = true;

                        continue;
                    }

                    // Reset exceptions counter
                    countAttemptsEr = 0;

                    // Check if some event log was recovered
                    var queryResult = eventLogList.TryToAdd ( fullResponse.Response );
                    switch ( queryResult.Result )
                    {
                        // Finish because the MTU has not event logs for specified date range
                        case EventLogQueryResult.Empty:
                            goto BREAK;

                        // Try one more time to recover an event log
                        case EventLogQueryResult.Busy:
                            if ( ++countAttempts > maxAttempts )
                                throw new ActionNotAchievedGetEventsLogException ();
                            else
                            {
                                //Errors.AddError ( new MtuIsBusyToGetEventsLogException () );

                                await Task.Delay ( WAIT_BTW_LOG_ERRORS );

                                // Try again, using this time Get Repeat Last Event Log Response command
                                retrying = true;
                            }
                            break;

                        // Wait a bit and try to read/recover the next log
                        case EventLogQueryResult.NextRead:
                            OnProgress ( this, new Delegates.ProgressArgs ( "Requesting event logs... " + queryResult.Index + "/" + eventLogList.TotalEntries ) );
                            
                            await Task.Delay ( WAIT_BTW_LOGS );
                            countAttempts = 0; // Reset accumulated fails after reading ok
                            retrying      = false; // And use Get Next Event Log Response command
                            break;

                        // Was last event log
                        case EventLogQueryResult.LastRead:
                            OnProgress ( this, new Delegates.ProgressArgs ( "All event logs requested" ) );
                            goto BREAK; // Exit from infinite while
                    }
                }

                BREAK:

                await Task.Delay ( WAIT_AFTER_EVENT_LOGS );

                await this.CheckIsTheSameMTU ();

                Utils.Print ( "DataRead Finished: " + eventLogList.Count );

                // Load memory map and prepare to read from Meters
                var map = await ReadMtu_Logic ();
                
                await this.CheckIsTheSameMTU ();

                OnProgress ( this, new Delegates.ProgressArgs ( "Reading from MTU..." ) );

                // Generates log using the interface
                await this.OnDataRead ( new Delegates.ActionArgs ( this.mtu, map, eventLogList ) );
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     throw new PuckCantCommWithMtuException ();
                else throw e;
            }
        }

        #endregion

        #region RFCheck ( previously Install Confirmation )

        /// <summary>
        /// This method is called only executing the Installation Confirmation action but
        /// the logic is in a different method, which allows to reuse it from the writing
        /// logic without mixing the processing of the result of the process.
        /// <para>
        /// See <see cref="InstallConfirmation_Logic(bool,int)"/> for the Install Confirmation logic.
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="OnReadMtu"/>
        public async Task InstallConfirmation ()
        {
            if ( await this.InstallConfirmation_Logic () < IC_EXCEPTION )
                 await this.ReadMtu ();
            else this.OnError ();
        }

        /// <summary>
        /// Process performed only if the MTU is switch on ( not in ship mode ) and all
        /// related tags ( Global.TimeToSync, Mtu.TimeToSync ) are set to true, to confirm
        /// the correct communication between the MTU and a DCU.
        /// <para>
        /// See <see cref="InstallConfirmation"/> for the entry point of the Installation Confirmation process executed directly.
        /// </para>
        /// </summary>
        /// <param name="force">Forces the Install Confirmation needed during installations,
        /// because sometimes the shipbit does not yet updated, resulting in a false positive</param>
        /// <param name="time">Internally used by the method to control the max number of attempts</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Integer value that indicates if the Installation
        /// Confirmation has worked ( 0 ) or not ( 1 Not achieved, 2 Error )
        /// </para>
        /// </returns>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        /// <exception cref="MtuIsAlreadyTurnedOffICException">( Used internally, not bubbling up )</exception>
        /// <exception cref="MtuIsNotTwowayICException">( Used internally, not bubbling up )</exception>
        /// <exception cref="AttemptNotAchievedICException">( Used internally, not bubbling up )</exception>
        /// <exception cref="ActionNotAchievedICException">( Used internally, not bubbling up )</exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error and used internally, not bubbling up )</exception>
        private async Task<int> InstallConfirmation_Logic (
            bool force = false,
            int  time  = 0 )
        {
            // DEBUG
            //this.WriteMtuBit ( 22, 0, false ); // Turn On MTU
            
            dynamic map = this.GetMemoryMap ();
            MemoryRegister<bool> regICNotSynced = map.InstallConfirmationNotSynced;
            MemoryRegister<bool> regICRequest   = map.InstallConfirmationRequest;

            int  result = IC_OK;
            bool wasNotAboutPuck = false;
            try
            {
                Utils.Print ( "InstallConfirmation trigger start" );
                
                await regICNotSynced.SetValueToMtu ( true );

                // MTU is turned off
                if ( ! force &&
                     this.mtuBasicInfo.Shipbit )
                {
                    wasNotAboutPuck = true;
                    throw new MtuIsAlreadyTurnedOffICException ();
                }
                
                // MTU does not support two-way or client does not want to perform it
                if ( ! global.TimeToSync ||
                     ! this.mtu.TimeToSync )
                {
                    wasNotAboutPuck = true;
                    throw new MtuIsNotTwowayICException ();
                }

                // Set to true/one this flag to request a time sync
                await regICRequest.SetValueToMtu ( true );

                bool fail;
                int  count = 1;
                int  wait  = 3;
                int  max   = ( int )( global.TimeSyncCountDefault / wait ); // Seconds / Seconds = Rounded max number of iterations
                
                do
                {
                    // Update interface text to look the progress
                    int progress = ( int )Math.Round ( ( decimal )( ( count * 100.0 ) / max ) );
                    OnProgress ( this, new Delegates.ProgressArgs ( count, max, "Checking IC... "+ progress.ToString() + "%" ) );
                    
                    await Task.Delay ( wait * 1000 );
                    
                    fail = await regICNotSynced.GetValueFromMtu ();
                }
                // Is MTU not synced with DCU yet?
                while ( fail &&
                        ++count <= max );
                
                if ( fail )
                    throw new AttemptNotAchievedICException ();
            }
            catch ( Exception e )
            {
                if ( ! ( e is PuckCantCommWithMtuException ) &&
                     e is AttemptNotAchievedICException )
                    Errors.AddError ( e );
                // Finish
                else
                {
                    if ( ! wasNotAboutPuck )
                         Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                    else Errors.LogErrorNowAndContinue ( e );

                    return IC_EXCEPTION;
                }
            
                // Retry action
                if ( ++time < global.TimeSyncCountRepeat )
                {
                    await Task.Delay ( WAIT_BTW_IC );
                    
                    result = await this.InstallConfirmation_Logic ( force, time );

                    // If this is not the first iteration, we need it to
                    // returns the result up to the initial invocation
                    if ( result > 0 )
                        return result;
                }
                
                // Finish with error
                Errors.LogErrorNowAndContinue ( new ActionNotAchievedICException ( ( global.TimeSyncCountRepeat ) + "" ) );
                result = IC_NOT_ACHIEVED;
            }
            
            // Node Discovery with OnDemand 1.2 MTUs
            if ( result == IC_OK         &&
                 this.global.AutoRFCheck &&
                 this.mtu.MtuDemand      &&
                 this.mtu.NodeDiscovery )
            {
                switch ( await this.NodeDiscovery ( map ) )
                {
                    case NodeDiscoveryResult.EXCELLENT:
                    case NodeDiscoveryResult.GOOD:
                    return IC_OK;

                    case NodeDiscoveryResult.NOT_ACHIEVED:
                    return IC_NOT_ACHIEVED;

                    case NodeDiscoveryResult.EXCEPTION:
                    return IC_EXCEPTION;
                }
            }
            // Result of the IC only
            return result;
        }

        /// <summary>
        /// The Node Discovery process is only performed working with OnDemand compatible MTUs,
        /// verifying that the MTU will be able to communicate over the F1 and F2 channels with
        /// enough DCUs to be able ensure that readings messages will be properly sent to the head-end.
        /// <para>
        /// The goal is to be able to get a Install Confirmation and verify the communications with in one minute.
        /// </para>
        /// <para>
        /// See <see cref="NodeDiscoveryList"/> for the auxiliar class that will contain all the nodes/DCUs detected.
        /// </para>
        /// </summary>
        /// <param name="map"><see cref="MemoryMap"/> used in the Install Confirmation process</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// A <see cref="NodeDiscoveryResult"/> value that indicates
        /// whether the Node Discovery process has worked or not.
        /// </para>
        /// </returns>
        private async Task<NodeDiscoveryResult> NodeDiscovery (
            dynamic map )
        {
            // List of all nodes detected for each attempt performed
            NodeDiscoveryList nodeList = new NodeDiscoveryList ( NodeType.DCUsOnly );
            NodeDiscoveryResult result = NodeDiscoveryResult.NOT_ACHIEVED;
            double  vswr          = -1;
            decimal successF1     = 0m;
            decimal successF2     = 0m;
            Stopwatch nodeCounter = null;

            try
            {
                // VSWR Test
                // NOTE: It can take up to one second to return an answer with data
                // NOTE: If the size of the data to be answered is not specified, the accepted answer will be ACK 6 and ACK Info Size 0
                LexiWriteResult fullResponse = await this.lexi.Write (
                        CMD_VSWR,
                        null,
                        N_ATTEMPTS_CMD,
                        WAIT_BTW_ATTEMPTS_CMD,
                        new uint[] { 6 },
                        null,
                        LexiAction.OperationRequest );

                vswr = Utils.GetNumericValueFromBytes<double> ( fullResponse.Response, 2, 2 );

                // Node Discovery ( Initiation + Start/Reset + Get Nodes )
                float maxTimeND = this.global.MaxTimeRFCheck * 1000;
                nodeCounter     = new Stopwatch ();
                nodeCounter.Start ();

                while ( true )
                {
                    #region Step 1 - Init

                    OnProgress ( this, new Delegates.ProgressArgs ( "Node Discovery... Init" ) );
                
                    // Node discovery initiation command
                    byte[] data = new byte[ 8 ]; // 1+4+1+1+1
                    data[ 0 ] = ( byte )NodeType.DCUsOnly;
                    data[ 1 ] = 0x00; // Target node ID LSB
                    data[ 2 ] = 0x00; // ...
                    data[ 3 ] = 0x00; // ...
                    data[ 4 ] = 0x00; // Target node ID MSB
                    data[ 5 ] = 0x0A; // Max dither time in seconds
                    data[ 6 ] = 0x00; // Min request send time in seconds
                    data[ 7 ] = 0x03; // RF Channels bitmap up to 8 channels ( 4 = 0000.0100 = Channel 3 )

                    // Response: Byte 2 { 0 = Node discovery not initiated, 1 = Node discovery initiated }
                    fullResponse = await this.lexi.Write (
                        CMD_INIT_NODE_DISC,
                        data,
                        N_ATTEMPTS_CMD,
                        WAIT_BTW_ATTEMPTS_CMD,
                        new uint[] { CMD_INIT_NODE_DISC_RES }, // ACK with response
                        new LexiFiltersResponse ( new ( int,int,byte )[] {
                            ( CMD_INIT_NODE_DISC_RES, CMD_BYTE_RES, CMD_INIT_NODE_DISC_NOT ), // Node discovery not initiated
                            ( CMD_INIT_NODE_DISC_RES, CMD_BYTE_RES, CMD_INIT_NODE_DISC_INI )  // Node discovery initiated
                        } ),
                        LexiAction.OperationRequest );
                    
                    #endregion

                    // Node discovery mode NOT initiated in the MTU
                    if ( fullResponse.Response[ CMD_BYTE_RES ] != CMD_INIT_NODE_DISC_INI )
                    {
                        Errors.LogErrorNowAndContinue ( new NodeDiscoveryNotInitializedException () );
                        goto BREAK_FAIL;
                    }
                    // Node discovery mode successfully initiated in the MTU
                    else
                    {
                        #region Step 2 - Start/Reset

                        OnProgress ( this, new Delegates.ProgressArgs ( "Node Discovery... Start/Reset" ) );

                        // Start/Reset node discovery response query
                        bool lexiTimeOut;
                        bool timeOut = false;
                        do
                        {
                            await Task.Delay ( WAIT_BEFORE_START_NODE );
                            
                            lexiTimeOut = true;
                            
                            // NOTE: After testing several times, the MTU generally does not respond
                            // NOTE: if is busy, not returning all requested bytes ( LExI timeout )
                            // ACLARA: It looks like bytes are lost.  That can happen if the MTU is busy doing something.
                            // It is a limitation of the coil and will happen with any command.  The coil isn’t a UART and
                            // doesn’t have any buffering so if the MTU is too busy to respond in time then the communication
                            // is going to get garbled and he just has to handle that by trying again.
                            try
                            {
                                // Response: Byte 2 { 0 = The MTU is busy, 1 = The MTU is ready for query }
                                fullResponse = await this.lexi.Write (
                                    CMD_QUERY_NODE_DISC,
                                    null,
                                    N_ATTEMPTS_CMD,
                                    WAIT_BTW_ATTEMPTS_CMD,
                                    new uint[] { CMD_QUERY_NODE_DISC_RES }, // ACK with response
                                    new LexiFiltersResponse ( new ( int,int,byte )[] {
                                        ( CMD_QUERY_NODE_DISC_RES, CMD_BYTE_RES, CMD_QUERY_NODE_DISC_NOT ), // The MTU is busy
                                        ( CMD_QUERY_NODE_DISC_RES, CMD_BYTE_RES, CMD_QUERY_NODE_DISC_INI )  // The MTU is ready for query
                                    } ),
                                    LexiAction.OperationRequest );
                                
                                lexiTimeOut = false;
                            }
                            catch ( Exception ) { }
                        }
                        while ( ( lexiTimeOut ||
                                  fullResponse.Response[ CMD_BYTE_RES ] == CMD_QUERY_NODE_DISC_NOT ) &&
                                ! ( timeOut = nodeCounter.ElapsedMilliseconds >= maxTimeND ) );
                        
                        // Node discovery mode not started/ready for query
                        if ( fullResponse.Response[ CMD_BYTE_RES ] == CMD_QUERY_NODE_DISC_NOT &&
                             timeOut )
                        {
                            Errors.LogErrorNowAndContinue ( new NodeDiscoveryNotStartedException () );
                            goto BREAK_FAIL;
                        }

                        #endregion

                        #region Step 3 - Get Next

                        OnProgress ( this, new Delegates.ProgressArgs ( "Node Discovery... Get Next" ) );

                        await Task.Delay ( WAIT_BEFORE_GET_NODES );

                        // Get next node discovery response
                        int maxAttemptsEr   = 2;
                        int countAttemptsEr = 0;
                        nodeList.StartNewAttempt (); // Saves nodes from the previous attempt for the log
                        while ( true )
                        {
                            try
                            {
                                fullResponse = await this.lexi.Write (
                                    CMD_NEXT_NODE_DISC,
                                    null,
                                    ONLY_ONE_ATTEMPT_CMD,
                                    WAIT_BTW_ATTEMPTS_CMD,
                                    new uint[] {
                                        CMD_NEXT_NODE_1,
                                        CMD_NEXT_NODE_2,
                                        CMD_NEXT_NODE_3 },
                                    new LexiFiltersResponse ( new ( int,int,byte )[] {
                                        ( CMD_NEXT_NODE_1, CMD_BYTE_RES, CMD_NEXT_NODE_DATA  ), // General information
                                        ( CMD_NEXT_NODE_2, CMD_BYTE_RES, CMD_NEXT_NODE_DATA  ), // Entry data included
                                        ( CMD_NEXT_NODE_3, CMD_BYTE_RES, CMD_NEXT_NODE_EMPTY )  // Complete but without data
                                    } ),
                                    LexiAction.OperationRequest );
                            }
                            catch ( Exception e )
                            {
                                // Finish because it is not own exception
                                if ( ! Errors.IsOwnException ( e ) )
                                    throw new PuckCantCommWithMtuException ();

                                // Finish without perform the action
                                else if ( ++countAttemptsEr >= maxAttemptsEr )
                                {
                                    Errors.LogErrorNowAndContinue ( new ActionNotAchievedICException ( maxAttemptsEr + "" ) );
                                    goto BREAK_FAIL;
                                }

                                await Task.Delay ( WAIT_BTW_NODE_ERRORS );

                                Utils.Print ( "Node Discovery: Error trying to recover a node [ Attempts " + countAttemptsEr + " / " + maxAttemptsEr + " ]" );

                                // Try one more time
                                Errors.AddError ( new AttemptNotAchievedNodeDiscoveryException () );

                                continue;
                            }

                            // Reset exceptions counter
                            countAttemptsEr = 0;

                            // Check if some node was recovered
                            var queryResult = nodeList.TryToAdd ( fullResponse.Response );
                            switch ( queryResult.Result )
                            {
                                // Wait a bit and try to read/recover the next node
                                case NodeDiscoveryQueryResult.NextRead:
                                    OnProgress ( this, new Delegates.ProgressArgs ( 
                                        "Requesting nodes... " + queryResult.Index + "/" + nodeList.CurrentAttemptTotalEntries ) );
                                    
                                    await Task.Delay ( WAIT_BTW_NODES );
                                    break;

                                // Was the last node or no node was recovered
                                case NodeDiscoveryQueryResult.LastRead:
                                case NodeDiscoveryQueryResult.Empty:
                                    OnProgress ( this, new Delegates.ProgressArgs ( "All nodes requested" ) );
                                    goto BREAK_OK; // Exit from switch + infinite while
                            }
                        }

                        #endregion

                        BREAK_OK:

                        #region Validation

                        bool    isF1;
                        bool    first           = true;
                        int     bestRssiResponse = -150;
                        string  freq1wayStr     = await map.Frequency1Way  .GetValue ();
                        string  freq2wayTxStr   = await map.Frequency2WayTx.GetValue ();
                        // NOTE: Parsing to double is important to take into account the separator symbol ( . or , ),
                        // NOTE: because parse "123,456" returns "123456" and use CultureInfo.InvariantCulture is not an universal solution
                        CultureInfo usCulture   = new CultureInfo("en-US");
                        double  freq1           = double.Parse ( freq1wayStr  .Replace ( ',', '.' ), usCulture.NumberFormat );
                        double  freq2           = double.Parse ( freq2wayTxStr.Replace ( ',', '.' ), usCulture.NumberFormat );
                        foreach ( NodeDiscovery node in nodeList.CurrentAttemptEntries )
                        {
                            // The first entry is only a LExI response
                            // with general information of the process
                            if ( first )
                            {
                                first = false;
                                continue;
                            }
                        
                            // Channel / Frequency
                            // NOTE: In the custom methods Frequency1Way_Get and Frequency2WayTx_Get the value returned is trimmed to three decimal digits
                            double freq = double.Parse ( ( ( node.FreqChannelRequest * 6250 + 450000000 ) / 1000000.0 ).ToString ( "F3" ) );
                            if ( ! ( isF1 = freq.Equals ( freq1 ) ) &&
                                 ! freq.Equals ( freq2 ) )
                                continue;

                            node.SetAsValidated ( isF1 );

                            // Document "RF_Connectivity_Test.docx"
                            // F1 Reliability estimate ( Page 14 )
                            // · The F1 Reliability Estimate is a calculation of the probability that a single
                            //   packet transmitted by the MTU will be received by at least one DCU on the F1 frequency
                            // · MTU -> DCU
                            //   · P( packet transmitted by the MTU on F1 is received by any DCU ) = P( MTU TX Success )
                            //     · P( packet transmitted by the MTU on F1 is received by DCU_1 ) = f_rssi( RSSI_1 )
                            //     · P( packet transmitted by the MTU on F1 is received by DCU_2 ) = f_rssi( RSSI_2 )
                            //     · P( packet transmitted by the MTU on F1 is received by DCU_N ) = f_rssi( RSSI_N )
                            //   · P( MTU TX Success ) = 100% - ( 100% - f_rssi( RSSI_1 ) ) * ( 100% - f_rssi( RSSI_2 ) ) * ... * ( 100% - f_rssi( RSSI_N ) )
                            // F2 Reliability estimate ( Page 17 )
                            // · DCU -> MTU
                            //   · P( packet sent by the DCU with the strongest RSSI on F2 is received by the MTU ) = P( DCU TX Success )
                            //     · P( DCU TX Success ) = f_rssi ( RSSI_Best_DCU )
                            // · MTU -> DCU
                            //   · P( packet transmitted by the MTU on F2 is received by any DCU ) = P( MTU TX Success )
                            //     · P( packet transmitted by the MTU on F2 is received by DCU_1 ) = f_rssi( RSSI_1 )
                            //     · P( packet transmitted by the MTU on F2 is received by DCU_2 ) = f_rssi( RSSI_2 )
                            //     · P( packet transmitted by the MTU on F2 is received by DCU_N ) = f_rssi( RSSI_N )
                            //   · P( MTU TX Success ) = 100% - ( 100% - f_rssi( RSSI_1 ) ) * ( 100% - f_rssi( RSSI_2 ) ) * ... * ( 100% - f_rssi( RSSI_N ) )
                            // · P( two way transaction is successful ) = P( TWO WAY )
                            //   · P( TWO WAY ) = 100% - { 100% - [ P( DCU TX Success ) * P( MTU TX Success ) ] }^3

                            // Highest signal strength ( DCU -> MTU )
                            if ( ! isF1 &&
                                 node.RSSIResponse > bestRssiResponse )
                                bestRssiResponse = node.RSSIResponse;
                        }

                        // Calculate validation success using the average RSSI for each DCU
                        successF1 = nodeList.CalculateMtuSuccess ( true );
                        successF2 = nodeList.CalculateTwoWaySuccess ( bestRssiResponse );

                        // Number of nodes validated so far, those discovered in any iteration
                        int numNodesValidated = nodeList.CountUniqueNodesValidated;

                        // Excellent
                        if ( numNodesValidated >= global.GoodNumDCU &&
                             successF1 >= global.GoodF1Rely/100 &&
                             successF2 >= global.GoodF2Rely/100 )
                            result = NodeDiscoveryResult.EXCELLENT;
                        
                        // Minimum
                        else if ( numNodesValidated >= global.MinNumDCU &&
                                  successF1 >= global.MinF1Rely/100 &&
                                  successF2 >= global.MinF2Rely/100 )
                            result = NodeDiscoveryResult.GOOD;

                        // Finish process only if the result is excellent or time is over
                        if ( result == NodeDiscoveryResult.EXCELLENT )
                            break; // Exit from infinite while

                        #endregion
                    }

                    BREAK_FAIL:

                    // The max time to perform Node Discovery process has expired
                    if ( nodeCounter.ElapsedMilliseconds >= maxTimeND )
                    {
                        // Finish process only if the result is excellent or time is over,
                        // and it can end after consuming all time but with "good" as result
                        if ( result > NodeDiscoveryResult.EXCELLENT )
                            result = NodeDiscoveryResult.NOT_ACHIEVED;

                        break; // Exit from infinite while
                    }
                }
            }
            catch ( Exception e )
            {
                if ( ! Errors.IsOwnException ( e ) )
                     Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                else Errors.LogErrorNowAndContinue ( e );
                
                result = NodeDiscoveryResult.EXCEPTION;
            }
            finally
            {
                if ( nodeCounter != null )
                {
                    nodeCounter.Stop ();
                    nodeCounter = null;
                }
            }

            // Generates entries for activity log and nodes log file
            await this.OnNodeDiscovery (
                new Delegates.ActionArgs ( this.mtu, map, result, nodeList, successF1, successF2, vswr ) );

            return result;
        }

        #endregion

        #region Valve Operation ( previously Remote Disconnect )

        private void RemoteDisconnect ()
        {
            // TODO: Crear clase RDDResult a la que pasar la respuesta del LExI write ( fullResponse.Response )
            // TODO: Crear todas las constantes necesarias para la lógica
            // TODO: Terminar de implementar el pseudocodigo
            // TODO: Añadir una nueva accion a la lista de posibles

            /*
            Aclara:
            - STAR Programmer MtuComm.cs Line 833
            - Lexi Mtu.cs Line 1268 -> Y61063 Page 52 Request RDD Action
            - Lexi Mtu.cs Line 1283 -> Y61063 Page 53 Request RDD Status

            try
            {
                Stopwatch nodeCounter = null;

                if ( Port1|2.type == "SETFLOW" )
                {
                    // Reads RDD Status
                    int status = -1;
                    MemoryRegister<int> rddStatus = map.RddStatus;

                    Action CheckStatus = (() =>
                    {
                        for ( int i = 0; i < RDD_MAX_ATTEMPS; i++ )
                        {
                            status = await rddStatus.GetValueFromMtu ();
                            switch ( status )
                            {
                                case RDD_DISABLED: // 0
                                await Task.Delay ( WAIT_RDD_DISABLED ); // 5000
                                message = "The RDD is disabled";
                                goto END_LOOP;

                                case RDD_BUSY: // 1
                                await Task.Delay ( WAIT_RDD_BUSY ); // 1000
                                message = "Remote Disconnect..." + ( ( i > 0 ) ? " Attempt " + ( i + 1 ) : string.Empty );
                                continue;

                                case RDD_ERROR: // 2
                                case RDD_IDLE: // 3
                                goto END_LOOP;
                            }
                        }

                        END_LOOP:

                        // The RDD Meter is not configured/installed
                        if ( status == RDD_DISABLED )
                            throw new Exception ();
                    });

                    // Checks current status
                    CheckStatus ();

                    // Request an action to the RDD 
                    await this.lexi.Write (
                        CMD_REQUEST_RDD, // 0x21
                        new byte[] { Data.RddActionType }, // 1 Close, 2 Open, 3 Partial Open
                        N_ATTEMPTS_CMD,
                        WAIT_BTW_ATTEMPTS_CMD,
                        null,
                        null,
                        LexiAction.OperationRequest );
                    
                    // Checks status after launsh the LExI command
                    CheckStatus ();

                    // Waits until the status of the RDD changes
                    bool lexiTimeOut;
                    bool timeOut = false;
                    nodeCounter  = new Stopwatch ();
                    nodeCounter.Start ();
                    do
                    {
                        await Task.Delay ( WAIT_BEFORE_RDD_STATUS );
                        
                        lexiTimeOut = true;
                        
                        try
                        {
                            fullResponse = await this.lexi.Write (
                                CMD_REQUEST_RDD, // 0x22
                                null,
                                N_ATTEMPTS_CMD,
                                WAIT_BTW_ATTEMPTS_CMD,
                                new uint[] { 18 },
                                null,
                                LexiAction.OperationRequest );
                            
                            lexiTimeOut = false;
                        }
                        catch ( Exception ) { }
                    }
                    while ( ( lexiTimeOut ||
                            fullResponse.Response[ CMD_BYTE_RES ] == CMD_RSS_IN_TRANS ) &&
                            ! ( timeOut = nodeCounter.ElapsedMilliseconds > CMD_RSS_MAX_TIME ) );
                    
                    RDDResponse response = new RDDResponse ( fullResponse.Response );
                    if ( response.PrevCmdStatus != CMD_RDD_SUCCESS &&
                        timeOut )
                    {
                        throw new Exception ();
                    }

                    // The status of the RDD has changed correctly
                    
                }
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     throw new PuckCantCommWithMtuException ();
                else throw e;
            }
            finally
            {
                if ( nodeCounter != null )
                {
                    nodeCounter.Stop ();
                    nodeCounter = null;
                }
            }

            */
        }

        #endregion

        #region Turn On|Off

        /// <summary>
        /// This method is called only executing the Switch Off action
        /// but the logic is in different method, which allows to use it from the
        /// writing logic without mixing the processing of the result of the process.
        /// <para>
        /// See <see cref="TurnOnOffMtu_Logic(bool,int)"/> for the Turn On/Off logic
        /// </para>
        /// </summary>
        /// <param name="on">Desired status of the MTU, true for run mode
        /// and false for ship mode ( turned off )</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <seealso cref="OnTurnOffMtu"/>
        /// <seealso cref="OnTurnOnMtu"/>
        public async Task TurnOnOffMtu (
            bool on )
        {        
            if ( on )
                 Utils.Print ( "TurnOffMtu start" );
            else Utils.Print ( "TurnOnMtu start"  );

            if ( await this.TurnOnOffMtu_Logic ( on ) )
            {
                if ( on )
                     this.OnTurnOnMtu  ( new Delegates.ActionArgs ( this.mtu ) );
                else this.OnTurnOffMtu ( new Delegates.ActionArgs ( this.mtu ) );
            }
        }

        /// <summary>
        /// Logic of the switch on or off of the MTU, changing between rune
        /// mode and ship mode respectively.
        /// <para>
        /// See <see cref="TurnOnOffMtu(bool)"/> for the entry point of the Turn On/Off process executed directly.
        /// </para>
        /// </summary>
        /// <param name="on">Desired status of the MTU, true for run mode
        /// and false for ship mode ( turned off )</param>
        /// <param name="time">Internally used by the method to control the max number of attempts</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Boolean that indicates if the auto-detection has worked or not.
        /// </para>
        /// </returns>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        /// <exception cref="AttemptNotAchievedTurnOffException">( Used internally, not bubbling up )</exception>
        /// <exception cref="ActionNotAchievedTurnOffException"></exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error )</exception>
        private async Task<bool> TurnOnOffMtu_Logic (
            bool on,
            int  time = 0 )
        {
            try
            {
                dynamic map = this.GetMemoryMap ();
                MemoryRegister<bool> shipbit = map.Shipbit;
                
                await shipbit.SetValueToMtu ( ! on );          // Set state of the shipbit
                bool  read = await shipbit.GetValueFromMtu (); // Read written value to verify
                
                // Fail turning off MTU
                if ( read == on )
                    throw new AttemptNotAchievedTurnOffException ();
            }
            // System.IO.IOException = Puck is not well placed or is off
            catch ( Exception e )
            {
                if ( Errors.IsOwnException ( e ) )
                    Errors.AddError ( e );
                
                // Finish
                else throw new PuckCantCommWithMtuException ();
                
                // Retry action ( three times = first plus two replies )
                if ( ++time < TIMES_TURNOFF )
                {
                    await Task.Delay ( WAIT_BTW_TURNOFF );
                    
                    return await this.TurnOnOffMtu_Logic ( on, time );
                }
                
                // Finish with error
                throw new ActionNotAchievedTurnOffException ( TIMES_TURNOFF + "" );
            }
            return true;
        }

        #endregion

        #region Read Fabric

        /// <summary>
        /// Simple and quick method to verify if the app can read from an MTU,
        /// reading from the physical memory only the first register, that corresponds
        /// to the MTU type, and the process is successful if no exception is launched.
        /// <para>
        /// See <see cref="ReadMtu"/> to perform a full read of the MTU.
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error )</exception>
        /// <seealso cref="OnReadFabric"/>
        public async Task ReadFabric ()
        {
            try
            {
                OnProgress ( this, new Delegates.ProgressArgs ( "Testing puck..." ) );

                // Only read all required registers once
                var map = this.GetMemoryMap ( true );

                // Activates flag to read Meter
                int MtuType = await  map.MtuType.GetValueFromMtu ();

                OnProgress ( this, new Delegates.ProgressArgs ( "Successful MTU read (" + MtuType.ToString() + ")" ) );

                await OnReadFabric ();
            }
            catch ( Exception )
            {
                Errors.LogErrorNow ( new PuckCantCommWithMtuException () );
            }
        }

        #endregion

        #region Read MTU

        /// <summary>
        /// This method is called only executing the Read MTU action
        /// but the logic is in different method, which allows to use it from the
        /// writing logic without mixing the processing of the result of the process.
        /// <para>
        /// See <see cref="ReadMtu_Logic"/> for the ReadMtu logic.
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <exception cref="MtuHasChangeBeforeFinishActionException">( From CheckIsTheSameMTU )</exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error )</exception>
        /// <exception cref="ReadMtu_Logic exceptions..."></exception>
        /// <seealso cref="OnReadMtu"/>
        public async Task ReadMtu ()
        {
            try
            {
                OnProgress ( this, new Delegates.ProgressArgs ( "Reading from MTU..." ) );
            
                // Load memory map and prepare to read from Meters
                var map = await ReadMtu_Logic ();
                
                await this.CheckIsTheSameMTU ();
             
                // Generates log using the interface
                await this.OnReadMtu ( new Delegates.ActionArgs ( this.mtu, map ) );
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     throw new PuckCantCommWithMtuException ();
                else throw e;
            }
        }

        /// <summary>
        /// Generates an instance of the dynamic MemoryMap that represents the memory
        /// of the MTU used, prepared to read values from there and caching the data,
        /// only recovering/reading one time from MTU physical memory.
        /// <para>
        /// See <see cref="ReadMtu"/> for the entry point of the ReadMtu process executed directly,
        /// not as a sub-action within another action.
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Instance of a dynamic memory map for current MTU.
        /// </para>
        /// </returns>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        private async Task<dynamic> ReadMtu_Logic ()
        {
            // Only read all required registers once
            var map = this.GetMemoryMap ( true );
            
            // Activates flag to read Meter
            await map.ReadMeter.SetValueToMtu ( true );
            
            await Task.Delay ( WAIT_BEFORE_READ );

            return map;
        }

        #endregion

        #region Write MTU

        private Action truquitoAction;

        /// <summary>
        /// In scripted mode this method overload is called before the main method,
        /// because it is necessary to translate the script parameters from Aclara into
        /// the app terminology and validate their values, removing unnecessary ones
        /// to avoid headaches.
        /// <para>
        /// See <see cref="AddMtu(dynamic, string, Action)"/> for the writing logic.
        /// </para>
        /// </summary>
        /// <param name="action">Current action type ( AddMtu, ReplaceMeter,.. )</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <exception cref="ScriptForOnePortButTwoEnabledException"></exception>
        /// <exception cref="ScriptForTwoPortsButMtuOnlyOneException"></exception>
        /// <exception cref="ScriptingAutoDetectMeterException"></exception>
        /// <exception cref="NumberOfDialsTagMissingScript">( Used internally, not bubbling up )</exception>
        /// <exception cref="DriveDialSizeTagMissingScript">( Used internally, not bubbling up )</exception>
        /// <exception cref="UnitOfMeasureTagMissingScript">( Used internally, not bubbling up )</exception>
        /// <exception cref="ScriptingAutoDetectTagsMissingScript"></exception>
        /// <exception cref="ScriptingAutoDetectMeterMissing"></exception>
        /// <exception cref="ScriptingAutoDetectNotSupportedException"></exception>
        /// <exception cref="ProcessingParamsScriptException"></exception>
        /// <exception cref="ScriptingAlarmForCurrentMtuException"></exception>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error )</exception>
        /// <exception cref="AddMtu (dynamic,string,Action) exceptions..."></exception>
        private async Task AddMtu ( Action action )
        {
            truquitoAction   = action;
            Parameter[] ps   = action.GetParameters ();
            dynamic     form = new AddMtuForm ( this.mtu );
            form.usePort2    = false;
            bool scriptUseP2 = false;

            // Action is about Replace Meter
            bool isReplaceMeter = (
                action.Type == ActionType.ReplaceMeter ||
                action.Type == ActionType.ReplaceMtuReplaceMeter ||
                action.Type == ActionType.AddMtuReplaceMeter );

            // Action is about Replace MTU
            bool isReplaceMtu = (
                action.Type == ActionType.ReplaceMTU ||
                action.Type == ActionType.ReplaceMtuReplaceMeter );
            
            List<Meter> meters;
            Meter meterPort1 = null;
            Meter meterPort2 = null;
            
            try
            {
                bool port2IsActivated = await this.GetMemoryMap ( true ).P2StatusFlag.GetValue ();
    
                // Recover parameters from script and translate from Aclara nomenclature to our own
                foreach ( Parameter parameter in ps )
                {
                    // Launches exception 'TranslatingParamsScriptException'
                    // Launches exception 'SameParameterRepeatScriptException'
                    form.AddParameterTranslatingAclaraXml ( parameter );
                    
                    if ( parameter.Port == 1 )
                        form.usePort2 = true;
                }
   
                scriptUseP2    = form.usePort2;
                form.usePort2 &= this.mtu.TwoPorts;

                #region Mandatory Meter Serial Number [ DEACTIVATED ]

                /*
                // The parameters MeterSerialNumber and NewMeterSerialNumber are mapped to FIELD.METER_NUMBER
                // NOTE: In scripted mode does not should be used global.UseMeterSerialNumber, doing MeterSerialNumber mandatory
                if ( ! form.ContainsParameter ( FIELD.METER_NUMBER ) )
                    throw new MandatoryMeterSerialHiddenScriptException ();
                
                if ( action.IsReplace &&
                     ! form.ContainsParameter ( FIELD.METER_NUMBER_OLD ) )
                    throw new MandatoryMeterSerialHiddenScriptException ();
                
                if ( scriptUseP2 )
                {
                    if ( ! form.ContainsParameter ( FIELD.METER_NUMBER_2 ) )
                        throw new MandatoryMeterSerialHiddenScriptException ();
                    
                    if ( action.IsReplace &&
                         ! form.ContainsParameter ( FIELD.METER_NUMBER_OLD_2 ) )
                        throw new MandatoryMeterSerialHiddenScriptException ();
                }
                */

                #endregion

                #region Auto-detect Meters

                // Script is for one port but MTU has two and second is enabled
                if ( ! scriptUseP2    &&
                     port2IsActivated && // Return true in a one port 138 MTU
                     this.mtu.TwoPorts ) // and for that reason I have to check also this
                    throw new ScriptForOnePortButTwoEnabledException ();
                
                // Script is for two ports but MTU has not second port or is disabled
                else if ( scriptUseP2 &&
                          ! port2IsActivated )
                    throw new ScriptForTwoPortsButMtuOnlyOneException ();
    
                bool isAutodetectMeter = false;
    
                // Port 1
                if ( ! form.ContainsParameter ( FIELD.METER_TYPE ) )
                {
                    // Missing tags
                    if ( ! form.ContainsParameter ( FIELD.NUMBER_OF_DIALS ) )
                        Errors.AddError ( new NumberOfDialsTagMissingScript () );
                    
                    if ( ! form.ContainsParameter ( FIELD.DRIVE_DIAL_SIZE ) )
                        Errors.AddError ( new DriveDialSizeTagMissingScript () );
                        
                    if ( ! form.ContainsParameter ( FIELD.UNIT_MEASURE ) )
                        Errors.AddError ( new UnitOfMeasureTagMissingScript () );
                    
                    if ( form.ContainsParameter ( FIELD.NUMBER_OF_DIALS ) &&
                         form.ContainsParameter ( FIELD.DRIVE_DIAL_SIZE ) &&
                         form.ContainsParameter ( FIELD.UNIT_MEASURE    ) )
                    {
                        isAutodetectMeter = true;
                    
                        meters = configuration.meterTypes.FindByDialDescription (
                            int.Parse ( form.NumberOfDials.Value ),
                            int.Parse ( form.DriveDialSize.Value ),
                            form.UnitOfMeasure.Value,
                            this.mtu.Flow );
        
                        // At least one Meter was found
                        if ( meters.Count > 0 )
                            form.AddParameter ( FIELD.METER_TYPE, ( meterPort1 = meters[ 0 ] ).Id.ToString () );
                        
                        // No meter was found using the selected parameters
                        else throw new ScriptingAutoDetectMeterException ();
                    }
                    // Script does not contain some of the needed tags ( NumberOfDials,... )
                    else throw new ScriptingAutoDetectTagsMissingScript ();
                }
                // Check if the selected Meter exists and current MTU support it
                else
                {
                    meterPort1 = configuration.getMeterTypeById ( int.Parse ( form.Meter.Value ) );
                    Port port  = this.mtu.Port1;
                    
                    // Is not valid Meter ID ( not present in Meter.xml )
                    if ( meterPort1.IsEmpty )
                        throw new ScriptingAutoDetectMeterMissing ();

                    // Check if current MTU supports the selected Meter
                    else if ( ! port.IsThisMeterSupported ( meterPort1 ) )
                        throw new ScriptingAutoDetectNotSupportedException ();
                    
                    // Set values for the Meter selected InterfaceTamper the script
                    this.mtu.Port1.MeterProtocol   = meterPort1.EncoderType;
                    this.mtu.Port1.MeterLiveDigits = meterPort1.LiveDigits;
                }
    
                // Port 2
                if ( this.mtu.TwoPorts &&
                     port2IsActivated )
                {
                    if ( ! form.ContainsParameter ( FIELD.METER_TYPE_2 ) )
                    {
                        // Missing tags
                        if ( ! form.ContainsParameter ( FIELD.NUMBER_OF_DIALS_2 ) )
                            Errors.AddError ( new NumberOfDialsTagMissingScript ( string.Empty, 2 ) );
                        
                        if ( ! form.ContainsParameter ( FIELD.DRIVE_DIAL_SIZE_2 ) )
                            Errors.AddError ( new DriveDialSizeTagMissingScript ( string.Empty, 2 ) );
                            
                        if ( ! form.ContainsParameter ( FIELD.UNIT_MEASURE_2 ) )
                            Errors.AddError ( new UnitOfMeasureTagMissingScript ( string.Empty, 2 ) );
                    
                        if ( form.ContainsParameter ( FIELD.NUMBER_OF_DIALS_2 ) &&
                             form.ContainsParameter ( FIELD.DRIVE_DIAL_SIZE_2 ) &&
                             form.ContainsParameter ( FIELD.UNIT_MEASURE_2    ) )
                        {
                            meters = configuration.meterTypes.FindByDialDescription (
                                int.Parse ( form.NumberOfDials_2.Value ),
                                int.Parse ( form.DriveDialSize_2.Value ),
                                form.UnitOfMeasure_2.Value,
                                this.mtu.Flow );
                            
                            // At least one Meter was found
                            if ( meters.Count > 0 )
                                form.AddParameter ( FIELD.METER_TYPE_2, ( meterPort2 = meters[ 0 ] ).Id.ToString () );
                                
                            // No meter was found using the selected parameters
                            else throw new ScriptingAutoDetectMeterException ( string.Empty, 2 );
                        }
                        // Script does not contain some of the needed tags ( NumberOfDials,... )
                        else throw new ScriptingAutoDetectTagsMissingScript ( string.Empty, 2 );
                    }
                    // Check if the selected Meter exists and current MTU support it
                    else
                    {
                        meterPort2 = configuration.getMeterTypeById ( int.Parse ( form.Meter_2.Value ) );
                        Port port  = this.mtu.Port2;
                        
                        // Is not valid Meter ID ( not present in Meter.xml )
                        if ( meterPort2.IsEmpty )
                            throw new ScriptingAutoDetectMeterMissing ( string.Empty, 2 );
                        
                        // Current MTU does not support selected Meter
                        else if ( ! port.IsThisMeterSupported ( meterPort2 ) )
                            throw new ScriptingAutoDetectNotSupportedException ( string.Empty, 2 );
                            
                        // Set values for the Meter selected InterfaceTamper the script
                        this.mtu.Port2.MeterProtocol   = meterPort2.EncoderType;
                        this.mtu.Port2.MeterLiveDigits = meterPort2.LiveDigits;
                    }
                }

                #endregion
    
                #region Validation

                #region Methods
    
                dynamic Empty = new Func<string,bool> ( ( v ) =>
                                        string.IsNullOrEmpty ( v ) );
    
                dynamic EmptyNum = new Func<string,bool> ( ( v ) =>
                                        string.IsNullOrEmpty ( v ) || ! Validations.IsNumeric ( v ) );

                // Value equals to maximum length
                dynamic NoEqNum = new Func<string,int,bool> ( ( v, maxLength ) =>
                                    ! Validations.NumericText ( v, maxLength ) );
                                    
                dynamic NoEqTxt = new Func<string,int,bool> ( ( v, maxLength ) =>
                                    ! Validations.Text ( v, maxLength ) );
    
                // Value equals or lower to maximum length
                dynamic NoELNum = new Func<string,int,bool> ( ( v, maxLength ) =>
                                    ! Validations.NumericText ( v, maxLength, 1, true, true, false ) );
                                    
                dynamic NoELTxt = new Func<string,int,bool> ( ( v, maxLength ) =>
                                    ! Validations.Text ( v, maxLength, 1, true, true, false ) );

                #endregion
            
                // Validate each parameter and remove those that are not going to be used

                string value = string.Empty;
                string msgDescription  = string.Empty;
                StringBuilder msgError = new StringBuilder ();
                StringBuilder msgErrorPopup = new StringBuilder ();
                foreach ( KeyValuePair<FIELD,Parameter> item in form.RegisteredParamsByField )
                {
                    FIELD type = item.Key;
                    Parameter parameter = item.Value;
                
                    bool fail = false;
                    
                    if ( fail = Empty ( parameter.Value ) )
                        msgDescription = "cannot be empty";
                    else
                    {
                        value = parameter.Value.ToString ();
                    
                        // Validates each parameter before continue with the action
                        switch ( type )
                        {
                            #region Activity Log Id
                            case FIELD.ACTIVITY_LOG_ID:
                            if ( fail = EmptyNum ( value ) )
                                msgDescription = "should be a valid numeric value";
                            break;
                            #endregion
                            #region Account Number
                            case FIELD.ACCOUNT_NUMBER:
                            case FIELD.ACCOUNT_NUMBER_2:
                            // In scripted mode not taking into account global.AccountLength
                            if ( fail = EmptyNum ( value ) )
                                msgDescription = "should be a valid numeric value";

                            //if ( fail = NoEqNum ( value, global.AccountLength ) )
                            //    msgDescription = "should be equal to global.AccountLength (" + global.AccountLength + ")";
                            break;
                            #endregion
                            #region Work Order
                            case FIELD.WORK_ORDER:
                            case FIELD.WORK_ORDER_2:
                            // Do not use
                            if ( ! global.WorkOrderRecording )
                            {
                                if ( parameter.Port == 0 )
                                     form.RemoveParameter ( FIELD.WORK_ORDER   );
                                else form.RemoveParameter ( FIELD.WORK_ORDER_2 );

                                continue;
                            }

                            else if ( fail = NoELTxt ( value, global.WorkOrderLength ) )
                                msgDescription =
                                    "should be equal to or less than global.WorkOrderLength (" + global.WorkOrderLength + ")";
                            break;
                            #endregion
                            #region MTU Id Old
                            case FIELD.MTU_ID_OLD:
                            // Do not use
                            if ( ! isReplaceMtu )
                            {
                                form.RemoveParameter ( FIELD.MTU_ID_OLD );

                                continue;
                            }

                            else if ( fail = NoEqNum ( value, global.MtuIdLength ) )
                                msgDescription =
                                    "should be equal to global.MtuIdLength (" + global.MtuIdLength + ")";
                            break;
                            #endregion
                            #region Meter Serial Number
                            case FIELD.METER_NUMBER:
                            case FIELD.METER_NUMBER_2:
                            case FIELD.METER_NUMBER_OLD:
                            case FIELD.METER_NUMBER_OLD_2:
                            // Do not use
                            if ( ! global.UseMeterSerialNumber )
                            {
                                if ( parameter.Port == 0 )
                                {
                                    switch ( parameter.Type )
                                    {
                                        case ParameterType.MeterSerialNumber:
                                        case ParameterType.NewMeterSerialNumber:
                                        form.RemoveParameter ( FIELD.METER_NUMBER );
                                        break;
                                        
                                        case ParameterType.OldMeterSerialNumber:
                                        form.RemoveParameter ( FIELD.METER_NUMBER_OLD );
                                        break;
                                    }
                                }
                                else
                                {
                                    switch ( parameter.Type )
                                    {
                                        case ParameterType.MeterSerialNumber:
                                        case ParameterType.NewMeterSerialNumber:
                                        form.RemoveParameter ( FIELD.METER_NUMBER_2 );
                                        break;
                                        
                                        case ParameterType.OldMeterSerialNumber:
                                        form.RemoveParameter ( FIELD.METER_NUMBER_OLD_2 );
                                        break;
                                    }
                                }

                                continue;
                            }

                            else if ( fail = NoELTxt ( value, global.MeterNumberLength ) )
                                msgDescription =
                                    "should be equal to or less than global.MeterNumberLength (" + global.MeterNumberLength + ")";
                            break;
                            #endregion
                            #region Meter Reading
                            case FIELD.METER_READING:
                            case FIELD.METER_READING_2:
                            // Do not ask for new Meter reading if the port is for Encoders/Ecoders
                            if ( parameter.Port == 0 && meterPort1.IsForEncoderOrEcoder ||
                                 parameter.Port == 1 && meterPort2.IsForEncoderOrEcoder )
                            {
                                form.RemoveParameter ( ( parameter.Port == 0 ) ?
                                    FIELD.METER_READING : FIELD.METER_READING_2 );
                                
                                continue;
                            }
                            else if ( ! isAutodetectMeter )
                            {
                                // If necessary fill left to 0's up to LiveDigits
                                if ( parameter.Port == 0 )
                                     value = meterPort1.FillLeftLiveDigits ( value );
                                else value = meterPort2.FillLeftLiveDigits ( value );
                            }
                            else
                            {
                                if ( parameter.Port == 0 )
                                {
                                    if ( ! ( fail = meterPort1.NumberOfDials <= -1 || 
                                                    NoELNum ( value, meterPort1.NumberOfDials ) ) )
                                    {
                                        // If value is lower than NumberOfDials, fill left to 0's up to NumberOfDials
                                        if ( NoEqNum ( value, meterPort1.NumberOfDials ) )
                                            value = meterPort1.FillLeftNumberOfDials ( value );
                                        
                                        // Apply Meter mask
                                        value = meterPort1.ApplyReadingMask ( value );
                                    }
                                    else break;
                                }
                                else
                                {
                                    if ( ! ( fail = meterPort2.NumberOfDials <= -1 ||
                                                    NoELNum ( value, meterPort2.NumberOfDials ) ) )
                                    {
                                        // If value is lower than NumberOfDials, fill left to 0's up to NumberOfDials
                                        if ( NoEqNum ( value, meterPort2.NumberOfDials ) )
                                            value = meterPort2.FillLeftNumberOfDials ( value );
                                        
                                        // Apply Meter mask
                                        value = meterPort2.ApplyReadingMask ( value );
                                    }
                                    else break;
                                }
                            }
                            
                            Meter meter = ( parameter.Port == 0 ) ? meterPort1 : meterPort2;
                            fail = NoEqNum ( value, meter.LiveDigits );
                            
                            if ( fail )
                            {
                                if ( ! isAutodetectMeter )
                                     msgDescription = "should be equal to or less than Meter.LiveDigits (" + meter.LiveDigits + ")";
                                else msgDescription = "should be equal to or less than Meter.NumberOfDials (" + meter.NumberOfDials + ")";
                            }
                            break;
                            #endregion
                            #region Meter Reading Old
                            case FIELD.METER_READING_OLD:
                            case FIELD.METER_READING_OLD_2:
                            // Param totally useless in this action type
                            // Do not use
                            if ( ! isReplaceMeter ||
                                 ! global.OldReadingRecording )
                            {
                                form.RemoveParameter ( ( parameter.Port == 0 ) ?
                                    FIELD.METER_READING_OLD : FIELD.METER_READING_OLD_2 );

                                continue;
                            }

                            else if ( fail = NoELNum ( value, 12 ) )
                                msgDescription = "should be equal to or less than 12";
                            break;
                            #endregion
                            #region Meter Type
                            case FIELD.METER_TYPE:
                            case FIELD.METER_TYPE_2:
                            //...
                            break;
                            #endregion
                            #region Read Interval
                            case FIELD.READ_INTERVAL:
                            // Do not use
                            if ( ! global.IndividualReadInterval )
                            {
                                form.RemoveParameter ( FIELD.WORK_ORDER );

                                continue;
                            }

                            List<string> readIntervalList;
                            if ( MtuForm.mtuBasicInfo.version >= global.LatestVersion )
                            {
                                readIntervalList = new List<string>()
                                {
                                    "24 Hours",
                                    "12 Hours",
                                    "8 Hours",
                                    "6 Hours",
                                    "4 Hours",
                                    "3 Hours",
                                    "2 Hours",
                                    "1 Hour",
                                    "30 Min",
                                    "20 Min",
                                    "15 Min"
                                };
                            }
                            else
                            {
                                readIntervalList = new List<string>()
                                {
                                    "1 Hour",
                                    "30 Min",
                                    "20 Min",
                                    "15 Min"
                                };
                            }
                            
                            // TwoWay MTU reading interval cannot be less than 15 minutes
                            if ( ! this.mtu.TimeToSync )
                                readIntervalList.AddRange ( new string[]{
                                    "10 Min",
                                    "5 Min"
                                });
                            
                            value = value.ToLower ()
                                         .Replace ( "hr", "hour" )
                                         .Replace ( "h", "H" )
                                         .Replace ( "m", "M" );
                            if ( fail = Empty ( value ) ||
                                 ! readIntervalList.Contains ( value ) )
                                msgDescription = "should be one of the possible values and using Hr/s or Min";
                            break;
                            #endregion
                            #region Snap Reads
                            case FIELD.SNAP_READS:
                            // Do not use
                            if ( ! global.AllowDailyReads ||
                                 ! mtu.DailyReads ||
                                 mtu.IsFamilly33xx )
                            {
                                form.RemoveParameter ( FIELD.SNAP_READS );

                                continue;
                            }

                            else if ( fail = EmptyNum ( value ) )
                                msgDescription = "should be a valid numeric value";
                            break;
                            #endregion
                            #region Auto-detect Meter
                            case FIELD.NUMBER_OF_DIALS:
                            case FIELD.NUMBER_OF_DIALS_2:
                            case FIELD.DRIVE_DIAL_SIZE:
                            case FIELD.DRIVE_DIAL_SIZE_2:
                            if ( fail = EmptyNum ( value ) )
                                msgDescription = "should be a valid numeric value";
                            break;
                            
                            case FIELD.UNIT_MEASURE:
                            case FIELD.UNIT_MEASURE_2:
                            //...
                            break;
                            #endregion
                            #region Force Time Sync
                            case FIELD.FORCE_TIME_SYNC:
                            bool.TryParse ( value, out fail );
                            if ( fail = ! fail )
                                msgDescription = "should be 'true' or 'false'";
                            break;
                            #endregion
                        }
                    }
    
                    if ( fail )
                    {
                        fail = false;
                        
                        string typeStr = ( form.Texts as Dictionary<FIELD,string[]> )[ type ][ 2 ];
                        
                        msgErrorPopup.Append ( ( ( msgError.Length > 0 ) ? ", " : string.Empty ) +
                                               typeStr + " " + msgDescription );

                        msgError.Append ( ( ( msgError.Length > 0 ) ? ", " : string.Empty ) +
                                          typeStr );
                    }
                    else
                        parameter.Value = value;
                }

                if ( msgError.Length > 0 )
                {
                    string msgErrorStr      = msgError     .ToString ();
                    string msgErrorPopupStr = msgErrorPopup.ToString ();
                    msgError     .Clear ();
                    msgErrorPopup.Clear ();
                    msgError      = null;
                    msgErrorPopup = null;
                
                    int index;
                    if ( ( index = msgErrorStr.LastIndexOf ( ',' ) ) > -1 )
                    {
                        msgErrorStr = msgErrorStr.Substring ( 0, index ) +
                                      " and" +
                                      msgErrorStr.Substring ( index + 1 );
                        
                        index = msgErrorPopupStr.LastIndexOf ( ',' );
                        msgErrorPopupStr = msgErrorPopupStr.Substring ( 0, index ) +
                                           " and" +
                                           msgErrorPopupStr.Substring ( index + 1 );
                    }
    
                    throw new ProcessingParamsScriptException ( msgErrorStr, 1, msgErrorPopupStr );
                }
    
                #endregion
    
                #region Auto-detect Alarm
    
                // Auto-detect scripting Alarm profile
                List<Alarm> alarms = configuration.alarms.FindByMtuType ( (int)MtuForm.mtuBasicInfo.Type );
                if ( alarms.Count > 0 )
                {
                    Alarm alarm = alarms.Find ( a => string.Equals ( a.Name.ToLower (), "scripting" ) );
                    if ( alarm != null &&
                         form.mtu.RequiresAlarmProfile )
                        form.AddParameter ( FIELD.ALARM, alarm );
                        
                    // For current MTU does not exist "Scripting" profile inside Alarm.xml
                    else throw new ScriptingAlarmForCurrentMtuException ();
                }

                #endregion
            
                await this.AddMtu ( form, action.User, action );
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     throw new PuckCantCommWithMtuException ();
                else throw e;
            }
        }

        /// <summary>
        /// Installation process is used to install a new MTU or Meter unit or replace an old one,
        /// configuring the MTU physical memory with the new values set in the application form or
        /// read from the script file.
        /// <para>
        /// See <see cref="AddMtu(Action)"/> for the entry point of the DataRead process in scripted mode.
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <exception cref="SelectedAlarmForCurrentMtuException"></exception>
        /// <exception cref="ActionNotAchievedEncryptionException"></exception>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        /// <exception cref="MtuHasChangeBeforeFinishActionException">( From CheckIsTheSameMTU )</exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error )</exception>
        /// <exception cref="TurnOnOffMtu_Logic () exceptions..."></exception>
        /// <exception cref="CheckSelectedEncoderMeter () exceptions..."></exception>
        /// <seealso cref="OnAddMtu"/>
        private async Task AddMtu (
            dynamic form,
            string user,
            Action action )
        {
            Mtu mtu  = form.mtu;
            this.mtu = mtu;

            try
            {
                Logger logger = ( ! Data.Get.IsFromScripting ) ? new Logger () : truquitoAction.Logger;
                addMtuLog = new AddMtuLog ( logger, form, user );

                #region Turn Off MTU

                Utils.Print ( "------TURN_OFF_START-----" );

                OnProgress ( this, new Delegates.ProgressArgs ( "Turning Off..." ) );

                await this.TurnOnOffMtu_Logic ( false );
                addMtuLog.LogTurnOff ();
                
                Utils.Print ( "-----TURN_OFF_FINISH-----" );

                #endregion
                
                await this.CheckIsTheSameMTU ();

                #region Check Meter for Encoder

                if ( this.mtu.Port1.IsForEncoderOrEcoder ||
                     form.usePort2 &&
                     this.mtu.Port2.IsForEncoderOrEcoder )
                {
                    Utils.Print ( "------CHECK_ENCODER_START-----" );

                    OnProgress ( this, new Delegates.ProgressArgs ( "Checking Encoder..." ) );

                    // Check if selected Meter is supported for current MTU
                    if ( this.mtu.Port1.IsForEncoderOrEcoder )
                        await this.CheckSelectedEncoderMeter ();

                    if ( form.usePort2 &&
                         this.mtu.Port2.IsForEncoderOrEcoder )
                        await this.CheckSelectedEncoderMeter ( 2 );

                    Utils.Print ( "------CHECK_ENCODER_FINISH-----" );
                
                    await this.CheckIsTheSameMTU ();
                }

                #endregion

                #region Add MTU

                Utils.Print ( "--------ADD_START--------" );

                OnProgress ( this, new Delegates.ProgressArgs ( "Preparing MemoryMap..." ) );

                dynamic map = this.GetMemoryMap ( true );
                form.map = map;

                #region Account Number

                // Uses default value fill to zeros if parameter is missing in scripting
                // Only first 12 numeric characters are recorded in MTU memory
                // F1 electric can have 20 alphanumeric characters but in the activity log should be written all characters
                map.P1MeterId = form.AccountNumber.GetValueOrDefault<ulong> ( 12 );
                if ( form.usePort2 &&
                     form.ContainsParameter ( FIELD.ACCOUNT_NUMBER_2 ) )
                    map.P2MeterId = form.AccountNumber_2.GetValueOrDefault<ulong> ( 12 );

                #endregion

                #region Meter Type

                Meter selectedMeter  = null;
                Meter selectedMeter2 = null;
                   
                if ( ! Data.Get.IsFromScripting )
                     selectedMeter = (Meter)form.Meter.Value;
                else selectedMeter = this.configuration.getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter.Value ) );
                map.P1MeterType = selectedMeter.Id;

                if ( form.usePort2 &&
                     form.ContainsParameter ( FIELD.METER_TYPE_2 ) )
                {
                    if ( ! Data.Get.IsFromScripting )
                         selectedMeter2 = (Meter)form.Meter_2.Value;
                    else selectedMeter2 = this.configuration.getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter_2.Value ) );
                    map.P2MeterType = selectedMeter2.Id;
                }

                #endregion

                #region ( Initial or New ) Meter Reading

                string p1readingStr = "0";
                string p2readingStr = "0";

                if ( form.ContainsParameter ( FIELD.METER_READING ) )
                {
                    p1readingStr = form.MeterReading.Value;
                    ulong p1reading = ( ! string.IsNullOrEmpty ( p1readingStr ) ) ? Convert.ToUInt64 ( ( p1readingStr ) ) : 0;
    
                    map.P1MeterReading = p1reading / ( ( selectedMeter.HiResScaling <= 0 ) ? 1 : selectedMeter.HiResScaling );
                }
                else if ( this.mtu.Port1.IsForPulse )
                {
                    // If meter reading was not present, fill in to zeros up to length equals to selected Meter live digits
                    p1readingStr = selectedMeter.FillLeftLiveDigits ();

                    form.AddParameter ( FIELD.METER_READING, p1readingStr );
                    map.P1MeterReading = p1readingStr;
                }
                
                if ( form.usePort2 )
                {
                    if ( form.ContainsParameter ( FIELD.METER_READING_2 ) )
                    {
                        p2readingStr = form.MeterReading_2.Value;
                        ulong p2reading = ( ! string.IsNullOrEmpty ( p2readingStr ) ) ? Convert.ToUInt64 ( ( p2readingStr ) ) : 0;
        
                        map.P2MeterReading = p2reading / ( ( selectedMeter2.HiResScaling <= 0 ) ? 1 : selectedMeter2.HiResScaling );
                    }
                    else if ( this.mtu.Port2.IsForPulse )
                    {
                        p2readingStr = selectedMeter2.FillLeftLiveDigits ();

                        form.AddParameter ( FIELD.METER_READING_2, p2readingStr );
                        map.P2MeterReading = p2readingStr;
                    }
                }

                #endregion

                #region Reading Interval

                if ( global.IndividualReadInterval )
                {
                    // If not present in scripted mode, set default value to one/1 hour
                    map.ReadIntervalMinutes = ( form.ContainsParameter ( FIELD.READ_INTERVAL ) ) ?
                                                form.ReadInterval.Value : "1 Hr";
                }

                #endregion

                #region Snap Reads

                if ( global.AllowDailyReads &&
                     mtu.DailyReads &&
                     form.ContainsParameter ( FIELD.SNAP_READS ) ) // &&
                     //map.ContainsMember ( "DailyGMTHourRead" ) )
                {
                    map.DailyGMTHourRead = form.SnapReads.Value;
                }

                #endregion

                #region Time of day for TimeSync

                if ( global.TimeToSync &&
                     mtu.TimeToSync    &&
                     mtu.IsNewVersion )
                {
                    map.TimeToSyncHr  = global.TimeToSyncHR;
                    map.TimeToSyncMin = global.TimeToSyncMin;
                    map.TimeToSyncSec = 30;
                }

                #endregion

                #region Alarm

                if ( mtu.RequiresAlarmProfile )
                {
                    Alarm alarms = (Alarm)form.Alarm.Value;
                    if ( alarms != null )
                    {
                        try
                        {
                            // Set alarms [ Alarm Message Transmission ]
                            if ( mtu.InsufficientMemory     ) map.InsufficientMemoryAlarm    = alarms.InsufficientMemory;
                            if ( mtu.GasCutWireAlarm        ) map.GasCutWireAlarm            = alarms.CutAlarmCable;
                            if ( form.usePort2 &&
                                 mtu.GasCutWireAlarm        ) map.P2GasCutWireAlarm          = alarms.CutAlarmCable;
                            if ( mtu.SerialComProblem       ) map.SerialComProblemAlarm      = alarms.SerialComProblem;
                            if ( mtu.LastGasp               ) map.LastGaspAlarm              = alarms.LastGasp;
                            if ( mtu.TiltTamper             ) map.TiltAlarm                  = alarms.Tilt;
                            if ( mtu.MagneticTamper         ) map.MagneticAlarm              = alarms.Magnetic;
                            if ( mtu.RegisterCoverTamper    ) map.RegisterCoverAlarm         = alarms.RegisterCover;
                            if ( mtu.ReverseFlowTamper      ) map.ReverseFlowAlarm           = alarms.ReverseFlow;
                            if ( mtu.SerialCutWire          ) map.SerialCutWireAlarm         = alarms.SerialCutWire;
                            if ( mtu.TamperPort1            ) map.P1CutWireAlarm             = alarms.TamperPort1;
                            if ( form.usePort2 &&
                                 mtu.TamperPort2            ) map.P2CutWireAlarm             = alarms.TamperPort2;

                            // Set immediate alarms [ Alarm Message Immediate ]
                            if ( mtu.InsufficientMemoryImm  ) map.InsufficientMemoryImmAlarm = alarms.InsufficientMemoryImm;
                            if ( mtu.MoistureDetectImm      ) map.MoistureImmAlarm           = alarms.MoistureDetectImm;
                            if ( mtu.ProgramMemoryErrorImm  ) map.ProgramMemoryImmAlarm      = alarms.ProgramMemoryErrorImm;
                            if ( mtu.MemoryMapErrorImm      ) map.MemoryMapImmAlarm          = alarms.MemoryMapErrorImm;
                            if ( mtu.EnergizerLastGaspImm   ) map.EnergizerLastGaspImmAlarm  = alarms.EnergizerLastGaspImm;
                            if ( mtu.GasCutWireAlarmImm     ) map.GasCutWireImmAlarm         = alarms.CutWireAlarmImm;
                            if ( mtu.SerialComProblemImm    ) map.SerialComProblemImmAlarm   = alarms.SerialComProblemImm;
                            if ( mtu.LastGaspImm            ) map.LastGaspImmAlarm           = alarms.LastGaspImm;
                            if ( mtu.TiltTamperImm          ) map.TiltImmAlarm               = alarms.TiltTamperImm;
                            if ( mtu.MagneticTamperImm      ) map.MagneticImmAlarm           = alarms.MagneticTamperImm;
                            if ( mtu.RegisterCoverTamperImm ) map.RegisterCoverImmAlarm      = alarms.RegisterCoverTamperImm;
                            if ( mtu.ReverseFlowTamperImm   ) map.ReverseFlowImmAlarm        = alarms.ReverseFlowTamperImm;
                            if ( mtu.SerialCutWireImm       ) map.SerialCutWireImmAlarm      = alarms.SerialCutWireImm;
                            if ( mtu.TamperPort1Imm         ) map.P1CutWireImmAlarm          = alarms.TamperPort1Imm;
                            if ( form.usePort2 &&
                                 mtu.TamperPort2Imm         ) map.P2CutWireImmAlarm          = alarms.TamperPort2Imm;

                            // Ecoder alarms
                            // NOTE: Same register is used to set both ports working with E-coder alarms
                            if ( mtu.Ecoder )
                            {
                                if ( mtu.ECoderLeakDetectionCurrent ) map.ECoderLeakDetectionCurrent = alarms.ECoderLeakDetectionCurrent;
                                if ( mtu.ECoderDaysOfLeak           ) map.ECoderDaysOfLeak           = alarms.ECoderDaysOfLeak;
                                if ( mtu.ECoderDaysNoFlow           ) map.ECoderDaysNoFlow           = alarms.ECoderDaysNoFlow;
                                if ( mtu.ECoderReverseFlow          ) map.ECoderReverseFlow          = alarms.ECoderReverseFlow;
                            }

                            // OnDemand 1.2 alarms
                            if ( mtu.MtuDemand )
                            {
                                // NOTE: VSWR alarm is set in the factory
                                if ( mtu.MoistureDetect     ) map.MoistureAlarm          = alarms.MoistureDetect;
                                if ( mtu.ProgramMemoryError ) map.ProgramMemoryAlarm     = alarms.ProgramMemoryError;
                                if ( mtu.MemoryMapError     ) map.MemoryMapAlarm         = alarms.MemoryMapError;
                                if ( mtu.EnergizerLastGasp  ) map.EnergizerLastGaspAlarm = alarms.EnergizerLastGasp;
                            }

                            // Write directly ( without conditions )
                            map.ImmediateAlarm = alarms.ImmediateAlarmTransmit;
                            if ( map.ContainsMember ( "UrgentAlarm" ) )
                                map.UrgentAlarm = alarms.DcuUrgentAlarm;
                            
                            // Overlap count
                            map.MessageOverlapCount = alarms.Overlap;
                            if ( form.usePort2 )
                                map.P2MessageOverlapCount = alarms.Overlap;

                            // For the moment only for the family 33xx
                            if ( map.ContainsMember ( "AlarmMask1" ) ) map.AlarmMask1 = false; // Set '0'
                            if ( map.ContainsMember ( "AlarmMask2" ) ) map.AlarmMask2 = false;
                        }
                        catch ( Exception )
                        {

                        }
                    }
                    // No alarm profile was selected before launch the action
                    else throw new SelectedAlarmForCurrentMtuException ();
                }

                #endregion

                #region Frequencies

                if ( global.AFC       &&
                     mtu.TimeToSync   &&
                     mtu.IsNewVersion &&
                     await map.MtuSoftVersion.GetValue () >= 19 )
                {
                    map.F12WAYRegister1Int  = global.F12WAYRegister1;
                    map.F12WAYRegister10Int = global.F12WAYRegister10;
                    map.F12WAYRegister14Int = global.F12WAYRegister14;
                }

                #endregion

                Utils.Print ( "--------ADD_FINISH-------" );

                #region Encryption

                // Only encrypt MTUs with SpecialSet set
                if ( mtu.SpecialSet )
                {
                    if ( ! mtu.IsFamilly35xx36xx )
                        await this.Encrypt_Old ( map );
                    
                    else if ( mtu.STAREncryptionType != ENCRYPTION.NONE )
                        await this.Encrypt_OnDemand12 ( map );
                }

                #endregion

                #region Write to MTU

                Utils.Print ( "---WRITE_TO_MTU_START----" );

                OnProgress ( this, new Delegates.ProgressArgs ( "Writing MemoryMap to MTU..." ) );

                // Write changes into MTU
                await this.WriteMtuModifiedRegisters ( map );
                await addMtuLog.LogAddMtu ();
                
                Utils.Print ( "---WRITE_TO_MTU_FINISH---" );

                #endregion

                #endregion

                await this.CheckIsTheSameMTU ();

                #region Turn On MTU

                Utils.Print ( "------TURN_ON_START------" );

                OnProgress ( this, new Delegates.ProgressArgs ( "Turning On..." ) );

                await this.TurnOnOffMtu_Logic ( true );
                addMtuLog.LogTurnOn ();
                
                Utils.Print ( "-----TURN_ON_FINISH------" );

                #endregion

                await this.CheckIsTheSameMTU ();

                #region Alarm #2

                if ( mtu.RequiresAlarmProfile )
                {
                    Alarm alarms = ( Alarm )form.Alarm.Value;
                    
                    // PCI Alarm needs to be set after MTU is turned on, just before the read MTU
                    // The Status will show enabled during install and actual status (triggered) during the read
                    if ( mtu.InterfaceTamper    ) await map.InterfaceAlarm   .SetValueToMtu ( alarms.InterfaceTamper    );
                    if ( mtu.InterfaceTamperImm ) await map.InterfaceImmAlarm.SetValueToMtu ( alarms.InterfaceTamperImm );
                }

                #endregion

                #region RFCheck

                // After TurnOn has to be performed an InstallConfirmation
                // if certain tags/registers are validated/true
                if ( global.TimeToSync && // Indicates that is a two-way MTU and enables TimeSync request
                     mtu.TimeToSync    && // Indicates that is a two-way MTU and enables TimeSync request
                     mtu.OnTimeSync    && // MTU can be force during installation to perform a TimeSync/IC
                     // If script contains ForceTimeSync, use it but if not use value from Global
                     ( ! form.ContainsParameter ( FIELD.FORCE_TIME_SYNC ) &&
                       global.ForceTimeSync ||
                       form.ContainsParameter ( FIELD.FORCE_TIME_SYNC ) &&
                       form.ForceTimeSync ) )
                {
                    Utils.Print ( "--------IC_START---------" );
                
                    OnProgress ( this, new Delegates.ProgressArgs ( "Install Confirmation..." ) );
                
                    // Force to execute Install Confirmation avoiding problems
                    // with MTU shipbit, because MTU is just turned on
                    if ( await this.InstallConfirmation_Logic ( true ) > IC_OK )
                    {
                        // If IC fails by any reason, add 4 seconds delay before
                        // reading MTU Tamper Memory settings for Tilt Alarm
                        await Task.Delay ( WAIT_IC_FAILS );
                    }
                    
                    Utils.Print ( "--------IC_FINISH--------" );
                }

                #endregion

                await this.CheckIsTheSameMTU ();

                #region Read MTU

                Utils.Print ( "----FINAL_READ_START-----" );
                
                OnProgress ( this, new Delegates.ProgressArgs ( "Reading from MTU..." ) );
                
                // Checks if all data was write ok, and then to generate the
                // final log without read again from the MTU the registers already read
                if ( ( await map.GetModifiedRegistersDifferences ( this.GetMemoryMap ( true ) ) ).Length > 0 )
                    throw new PuckCantCommWithMtuException ();

                // It is necessary for Encoders and E-coders, which should read the reading from the the meter
                // NOTE: This flag should be activated after the the previous map comparison, to avoid
                // NOTE: false positive error when comparing the meter reading and the value not inserted by the user ( zero )
                if ( this.mtu.Port1.IsForEncoderOrEcoder )
                {
                    // Reset register cache
                    map.P1MeterReading.readedFromMtu = false;
                    map.P2MeterReading.readedFromMtu = false;
                
                    // Activates flag to read Meter
                    await map.ReadMeter.SetValueToMtu ( true );
                    
                    await Task.Delay ( WAIT_BEFORE_READ );
                }

                Utils.Print ( "----FINAL_READ_FINISH----" );

                #endregion

                await this.CheckIsTheSameMTU ();

                // Generate log to show on device screen
                await this.OnAddMtu ( new Delegates.ActionArgs ( this.mtu, map, form, addMtuLog ) );
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     throw new PuckCantCommWithMtuException ();
                else throw e;
            }
        }

        #region Encryption

        private async Task Encrypt_Old (
            dynamic map )
        {
            Utils.Print ( "----ENCRYPTION_START-----" );
                
            OnProgress ( this, new Delegates.ProgressArgs ( "Encrypting..." ) );

            MemoryRegister<string> regAesKey     = map[ "EncryptionKey"   ];
            MemoryRegister<bool>   regEncrypted  = map[ "Encrypted"       ];
            MemoryRegister<int>    regEncryIndex = map[ "EncryptionIndex" ];
        
            bool   ok     = false;
            byte[] aesKey = new byte[ regAesKey.size    ]; // 16 bytes
            byte[] sha    = new byte[ regAesKey.sizeGet ]; // 32 bytes
            
            try
            {
                // Generate random key
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider ();
                rng.GetBytes ( aesKey );
                
                // Calculate SHA for the new random key
                // To use .Net API does not give the same result
                /*
                using ( SHA256 mySHA256 = SHA256.Create () )
                {
                    sha = mySHA256.ComputeHash ( aesKey );
                }
                */
                
                // Using Aclara/StarProgrammer class
                // Note: Generates different result than using .Net SHA256 class
                MtuSha256 crypto = new MtuSha256 ();
                crypto.GenerateSHAHash ( aesKey, out sha );

                // Current encrypted index
                int curEncrypIndex = ( int )await regEncryIndex.GetValueFromMtu ();
                
                // Try to write and validate AES encryption key up to five times
                for ( int i = 0; i < CMD_ENCRYP_OLD_MAX; i++ )
                {
                    // Writes key in the MTU
                    Utils.Print ( "Write key to MTU" );
                    await regAesKey.SetValueToMtu ( aesKey );
                    
                    Thread.Sleep ( 1000 );
                    
                    // Verifies if the MTU is encrypted
                    Utils.Print ( "Read Encrypted from MTU" );
                    bool encrypted   = ( bool )await regEncrypted .GetValueFromMtu ();
                    Utils.Print ( "Read EncryptedIndex from MTU" );
                    int  encrypIndex = ( int  )await regEncryIndex.GetValueFromMtu ();
                    
                    if ( ! encrypted ||
                         encrypIndex <= 0 ||
                         encrypIndex <= curEncrypIndex )
                        continue; // Error
                    else
                    {
                        Utils.Print ( "Read EncryptionKey (SHA) from MTU" );
                        byte[] mtuSha = ( byte[] )await regAesKey.GetValueFromMtu ( true ); // 32 bytes
                        
                        Thread.Sleep ( 100 );

                        // Compare local sha and sha generate reading key from MTU
                        if ( ! sha.SequenceEqual ( mtuSha ) )
                                continue; // Error
                        else
                        {
                            ok = true;
                            break;
                        }
                    }
                }
            }
            catch ( Exception )
            {
                //...
            }
            finally
            {
                if ( ok )
                {
                    Mobile.ConfigData data = Mobile.configData;
                    
                    data.lastRandomKey    = new byte[ aesKey.Length ];
                    data.lastRandomKeySha = new byte[ sha   .Length ];
                
                    // Save data to log
                    Array.Copy ( aesKey, data.lastRandomKey,    aesKey.Length );
                    Array.Copy ( sha,    data.lastRandomKeySha, sha.Length    );
                }
                
                // Always clear temporary random key from memory, and then after generate the
                // activity log also will be cleared random key and its sha save in Mobile.configData
                Array.Clear ( aesKey, 0, aesKey.Length );
                Array.Clear ( sha,    0, sha.Length    );
            }
            
            // MTU encryption has failed
            if ( ! ( Mobile.configData.isMtuEncrypted = ok ) )
                throw new ActionNotAchievedEncryptionException ( CMD_ENCRYP_OLD_MAX + "" );
            
            await this.CheckIsTheSameMTU ();

            Utils.Print ( "----ENCRYPTION_FINISH----" );
        }

        private async Task Encrypt_OnDemand12 (
            dynamic map )
        {
            Utils.Print ( "----ENCRYPTION_START-----" );
                
            OnProgress ( this, new Delegates.ProgressArgs ( "Encrypting..." ) );

            MemoryRegister<string> regAesKey     = map[ "EncryptionKey"   ];
            MemoryRegister<bool>   regEncrypted  = map[ "Encrypted"       ];
            MemoryRegister<int>    regEncryIndex = map[ "EncryptionIndex" ];

            // Look for the public key
            if ( string.IsNullOrEmpty ( this.global.PublicKey ) )
                throw new ODEncryptionPublicKeyNotSetException ();
            
            // Look for the broadcast key
            if ( this.mtu.BroadCast &&
                 string.IsNullOrEmpty ( this.global.BroadcastSet ) )
                throw new ODEncryptionBroadcastKeyNotSetException ();

            bool publicKeyInBase64;
            bool broadKeyInBase64;
            string publicKey = Utils.StringFromBase64 ( this.global.PublicKey,    out publicKeyInBase64 );
            string broadKey  = Utils.StringFromBase64 ( this.global.BroadcastSet, out broadKeyInBase64  );

            // Removes first eight characters and there should be exactly 64 remaining
            publicKey = publicKey.Substring ( 8 );

            // Checks public key format
            if ( ! publicKeyInBase64 ||
                 publicKey.Length != 64 )
                throw new ODEncryptionPublicKeyFormatException ();
            
            // Checks broadcast key format
            if ( this.mtu.BroadCast &&
                 ( ! broadKeyInBase64 ||
                   broadKey.Length != 32 ) )
                throw new ODEncryptionBroadcastKeyFormatException ();

            // Prepares all for random number generation
            byte[] randomKey = new byte[ regAesKey.sizeGet ]; // 32 bytes
            MtuSha256 mtusha = new MtuSha256 ();

            // Prepares the data for the LExI commands "Loads Encryption Item"
            byte[] data4 = new byte[ 32 + 1 ];
            if ( this.mtu.BroadCast )
            {
                byte[] arBRK = Encoding.UTF8.GetBytes ( broadKey );
                Array.Copy ( arBRK, 0, data4, 1, arBRK.Length );
                data4[ 0 ] = 0x04; // Broadcast Key
            }

            byte[] data1 = new byte[ randomKey.Length + 1 ];
            data1[ 0 ] = 0x01; // Head End Random Number

            byte[] data0 = new byte[ 64 + 1 ];
            byte[] arPKI = Encoding.UTF8.GetBytes ( publicKey );
            Array.Copy ( arPKI, 0, data0, 1, arPKI.Length );
            data0[ 0 ] = 0x00; // Head End Public Key

            // Prepares the data for the LExI commands "Reads Encryption Item"
            byte[] data3 = { 0x03 }; // MTU Random Number
            byte[] data2 = { 0x02 }; // MTU Public Key

            // Current encrypted index
            int curEncrypIndex = ( int )await regEncryIndex.GetValueFromMtu ();

            LexiWriteResult fullResponse;
            for ( int i = 0; i < CMD_ENCRYP_MAX; i++ )
            {
                try
                {
                    int step = 1;

                    // Generates the random number and prepares LExI array
                    randomKey = mtusha.RandomBytes ( randomKey.Length );
                    Array.Copy ( randomKey, 0, data1, 1, randomKey.Length );

                    if ( this.mtu.BroadCast )
                    {
                        OnProgress ( this, new Delegates.ProgressArgs ( "Encrypting... Step " + step++ ) );

                        // Loads Encryption Item - Type 4: Broadcast Key 
                        fullResponse = await this.lexi.Write (
                            CMD_LOAD_ENCRYP,
                            data4,
                            N_ATTEMPTS_CMD,
                            WAIT_BTW_ATTEMPTS_CMD,
                            null,
                            null,
                            LexiAction.OperationRequest );
                    }

                    OnProgress ( this, new Delegates.ProgressArgs ( "Encrypting... Step " + step++ ) );

                    // Loads Encryption Item - Type 1: Head End Random Number
                    fullResponse = await this.lexi.Write (
                        CMD_LOAD_ENCRYP,
                        data1,
                        N_ATTEMPTS_CMD,
                        WAIT_BTW_ATTEMPTS_CMD,
                        null,
                        null,
                        LexiAction.OperationRequest );
                    
                    string serverRND = Convert.ToBase64String ( randomKey );
                    
                    OnProgress ( this, new Delegates.ProgressArgs ( "Encrypting... Step " + step++ ) );

                    // Loads Encryption Item - Type 0: Head End Public Key
                    fullResponse = await this.lexi.Write (
                        CMD_LOAD_ENCRYP,
                        data0,
                        N_ATTEMPTS_CMD,
                        WAIT_BTW_ATTEMPTS_CMD,
                        null,
                        null,
                        LexiAction.OperationRequest );
                    
                    OnProgress ( this, new Delegates.ProgressArgs ( "Encrypting... Step " + step++ ) );

                    // Generates Encryptions Keys
                    fullResponse = await this.lexi.Write (
                        CMD_GEN_ENCRYP_KEYS,
                        null,
                        N_ATTEMPTS_CMD,
                        WAIT_BTW_ATTEMPTS_CMD,
                        null,
                        null,
                        LexiAction.OperationRequest );

                    // Verifies if the MTU is encrypted
                    Utils.Print ( "Read Encrypted from MTU" );
                    bool encrypted   = ( bool )await regEncrypted .GetValueFromMtu ();
                    Utils.Print ( "Read EncryptedIndex from MTU" );
                    int  encrypIndex = ( int  )await regEncryIndex.GetValueFromMtu ();
                    
                    if ( ! encrypted ||
                         encrypIndex <= 0 ||
                         encrypIndex <= curEncrypIndex )
                        continue; // Error

                    OnProgress ( this, new Delegates.ProgressArgs ( "Encrypting... Step " + step++ ) );

                    // Reads Encryption Item - Type 3: MTU Random Number
                    fullResponse = await this.lexi.Write (
                        CMD_READ_ENCRYP,
                        data3,
                        N_ATTEMPTS_CMD,
                        WAIT_BTW_ATTEMPTS_CMD,
                        new uint[] { CMD_READ_ENCRYP_RES_3 },
                        null,
                        LexiAction.OperationRequest );

                    string clientRnd = Convert.ToBase64String ( fullResponse.Response );

                    OnProgress ( this, new Delegates.ProgressArgs ( "Encrypting... Step " + step++ ) );

                    // Reads Encryption Item - Type 2: MTU Public Key
                    fullResponse = await this.lexi.Write (
                        CMD_READ_ENCRYP,
                        data2,
                        N_ATTEMPTS_CMD,
                        WAIT_BTW_ATTEMPTS_CMD,
                        new uint[] { CMD_READ_ENCRYP_RES_2 },
                        null,
                        LexiAction.OperationRequest );

                    string mtuPublicKey = Convert.ToBase64String ( fullResponse.Response );

                    // Saves data that will be use to create the activity log
                    Data.Set ( "ServerRND",    serverRND,    true );
                    Data.Set ( "ClientRND",    clientRnd,    true );
                    Data.Set ( "MtuPublicKey", mtuPublicKey, true );

                    // Always clear temporary random key from memory
                    Array.Clear ( randomKey, 0, randomKey.Length );
                    Array.Clear ( data0,     0, data0.Length     );

                    await this.CheckIsTheSameMTU ();

                    Utils.Print ( "----ENCRYPTION_FINISH----" );

                    return;
                }
                catch ( Exception e )
                {
                    // Is not own exception
                    if ( ! Errors.IsOwnException ( e ) )
                        throw new PuckCantCommWithMtuException ();
                }
            }

            throw new ActionNotAchievedEncryptionException ( CMD_ENCRYP_MAX + "" );
        }

        #endregion

        #endregion

        #region Basic Read

        /// <summary>
        /// It is an action without logic, that only performs the initial MTU basic information reading.
        /// </summary>
        /// <seealso cref="OnBasicRead"/>
        public void BasicRead ()
        {
            this.OnBasicRead ();
        }

        #endregion

        #endregion

        #region Read|Write from|to MTU

        /// <summary>
        /// Writes to the MTU physical memory only the registers that have been modified
        /// during the writing action performed, optimizing the process and lengthening
        /// the MTU memory useful life.
        /// </summary>
        /// <param name="map">MemoryMap used in the writing process</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        public async Task WriteMtuModifiedRegisters ( MemoryMap.MemoryMap map )
        {
            List<dynamic> modifiedRegisters = map.GetModifiedRegisters ().GetAllElements ();
            
            for ( int i = 0; i < modifiedRegisters.Count; i++ )
                await modifiedRegisters[ i ].SetValueToMtu ();
            
            //foreach ( dynamic r in modifiedRegisters )
            //    await r.SetValueToMtu ();

            modifiedRegisters.Clear ();
            modifiedRegisters = null;
        }

        /// <summary>
        /// Writes to the MTU physical memory only modifying a bit of the indicated address.
        /// </summary>
        /// <remarks>
        /// NOTE: This method is deprecated and should be used the dynamic MemoryMap and
        /// the <see cref="MTUComm.MemoryMap.MemoryRegisters">registers</see> methods.
        /// </remarks>
        /// <param name="address">Address of the register to modify in the MTU memory</param>
        /// <param name="bit">Bit to modify in the select address/byte</param>
        /// <param name="active">Desired status ( true = 1/one, false = 0/zero )</param>
        /// <param name="verify">Optionally the writing process can be validated reading
        /// the same register and comparing with desired status</param>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Boolean that indicates if the write action has completed successfully.
        /// </para>
        /// </returns>
        public async Task<bool> WriteMtuBitAndVerify ( uint address, uint bit, bool active, bool verify = true )
        {
            // Read current value
            byte systemFlags = ( await lexi.Read ( address, 1 ) )[ 0 ];

            // Modify bit and write to MTU
            if ( active )
                 systemFlags = ( byte ) ( systemFlags |    1 << ( int )bit   );
            else systemFlags = ( byte ) ( systemFlags & ~( 1 << ( int )bit ) );
            
            await lexi.Write (
                address,
                new byte[] { systemFlags },
                N_ATTEMPTS_LEXI,
                WAIT_BTW_ATTEMPTS_LEXI );

            // Read new written value to verify modification
            if ( verify )
            {
                byte valueWritten = ( await lexi.Read ( address, 1 ) )[ 0 ];
                return ( ( ( valueWritten >> ( int )bit ) & 1 ) == ( ( active ) ? 1 : 0 ) );
            }

            // Without verification
            return true;
        }

        #endregion

        #region Auxiliary Functions
        
        /// <summary>
        /// Generates a new instance of the dynamic MemoryMap for current MTU,
        /// using the associated family to load the correct XML map, allowing
        /// to communicate ( write and read ) with the physical memory of the MTU.
        /// </summary>
        /// <param name="readFromMtuOnlyOnce">To get a register value the physical
        /// memory of the MTU is read, but sometimes it is preferable
        /// to only read once and cache the data</param>
        /// <returns>An instance of the dynamic MemoryMap.</returns>
        /// <exception cref="MemoryMapParseXmlException"></exception>
        private dynamic GetMemoryMap (
            bool readFromMtuOnlyOnce = false )
        {
            // Prepare memory map
            string memory_map_type = configuration.GetMemoryMapTypeByMtuId ( this.mtu );
            int    memory_map_size = configuration.GetmemoryMapSizeByMtuId ( this.mtu );
            
            return new MemoryMap.MemoryMap ( new byte[ memory_map_size ], memory_map_type, readFromMtuOnlyOnce );
        }
        
        /// <summary>
        /// Loads the basic information necessary to work with an MTU and be able to prepare
        /// the forms of the UI with the correct values ( compatible Meters,.. ), but the logic
        /// is in another method to create a more readable and easy to maintain source code.
        /// <para>
        /// See <see cref="LoadMtuBasicInfo"/> to recover basic data from the MTU.
        /// </para>
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <exception cref="MtuMissingException">( From Configuration.GetMtuTypeById )</exception>
        /// <exception cref="MemoryMapParseXmlException">( From GetMemoryMap )</exception>
        /// <seealso cref="MTUBasicInfo"/>
        private async Task LoadMtuAndMetersBasicInfo ()
        {
            // Actions without form have no problem, but actions that require the user to
            // complete a form before launch the action logic, should avoid to invoking this
            // event the first time, when the basic loading is done to prepare the form
            if ( OnProgress != null )
                OnProgress ( this, new Delegates.ProgressArgs ( "Initial Reading..." ) );

            if ( await this.LoadMtuBasicInfo () )
            {
                this.basicInfoLoaded = true;
            
                MtuForm.SetBasicInfo ( mtuBasicInfo );
                
                // Launches exception 'MtuTypeIsNotFoundException'

                // Get Mtu entry using numeric ID ( e.g. 138 )
                this.mtu = configuration.GetMtuTypeById ( ( int )this.mtuBasicInfo.Type );
               
                //for ( int i = 0; i < this.mtu.Ports.Count; i++ )
                //    mtuBasicInfo.setPortType ( i, this.mtu.Ports[ i ].TypeString );
                
                Data.Set("MemoryMap",GetMemoryMap(true),false);
            }
        }

        /// <summary>
        /// Loads the basic information necessary to work with an MTU and be able to
        /// prepare the forms of the UI with the correct values ( compatible Meters,.. ).
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.
        /// <para>
        /// Indicates whether the auto-detection worked or not.
        /// </para>
        /// </returns>
        /// <seealso cref="MTUBasicInfo"/>
        private async Task<bool> LoadMtuBasicInfo ()
        //    bool isAfterWriting = false )
        {
            List<byte> finalRead = new List<byte> ();
        
            try
            {
                byte[] firstRead  = await lexi.Read ( BASIC_READ_1_ADDRESS, BASIC_READ_1_DATA );
                byte[] secondRead = await lexi.Read ( BASIC_READ_2_ADDRESS, BASIC_READ_2_DATA );
                finalRead.AddRange ( firstRead  );
                finalRead.AddRange ( secondRead );
            }
            // System.IO.IOException = Puck is not well placed or is off
            catch ( Exception )
            {
                //if ( ! isAfterWriting )
                     Errors.LogErrorNow ( new PuckCantCommWithMtuException () );
                //else Errors.LogErrorNow ( new PuckCantReadFromMtuAfterWritingException () );
                
                return false;
            }

            MTUBasicInfo basicInfo = new MTUBasicInfo ( finalRead.ToArray () );
            this.mtuHasChanged = ( mtuBasicInfo.Id   == 0 ||
                                   mtuBasicInfo.Type == 0 ||
                                   basicInfo.Id   != mtuBasicInfo.Id ||
                                   basicInfo.Type != mtuBasicInfo.Type );
            
            mtuBasicInfo = basicInfo;
            
            return this.mtuHasChanged;
        }

        /// <summary>
        /// Checks if the MTU is still the same as at the beginning of the process,
        /// otherwise the process should be canceled immediately, forcing an exception.
        /// </summary>
        /// <returns>Task object required to execute the method asynchronously and
        /// for a correct exceptions bubbling.</returns>
        /// <exception cref="MtuHasChangeBeforeFinishActionException"></exception>
        /// <exception cref="PuckCantCommWithMtuException">( Generic error )</exception>
        private async Task CheckIsTheSameMTU ()
        {
            byte[] read;
            try
            {
                read = await lexi.Read ( SAME_MTU_ADDRESS, SAME_MTU_DATA );
            }
            catch ( Exception )
            {
                throw new PuckCantCommWithMtuException ();
            }

            uint mtuType = read[ 0 ];

            byte[] id_stream = new byte[ 4 ];
            Array.Copy ( read, 6, id_stream, 0, 4 );
            uint mtuId = BitConverter.ToUInt32 ( id_stream, 0 );

            if ( mtuType != mtuBasicInfo.Type ||
                   mtuId != mtuBasicInfo.Id )
                throw new MtuHasChangeBeforeFinishActionException ();
        }

        /// <summary>
        /// Returns the basic information loaded of the current MTU.
        /// <para>
        /// See <see cref="LoadMtuAndMetersBasicInfo"/> to recover basic data from the MTU.
        /// </para>
        /// </summary>
        /// <returns>Instance of the MTU basic information</returns>
        /// <seealso cref="MTUBasicInfo"/>
        public MTUBasicInfo GetBasicInfo ()
        {
            return this.mtuBasicInfo;
        }

        #endregion
    }
}
