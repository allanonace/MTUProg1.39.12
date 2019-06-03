using System;
using System.Collections;
using System.Collections.Generic;
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
using Xml;

using ActionType          = MTUComm.Action.ActionType;
using FIELD               = MTUComm.actions.AddMtuForm.FIELD;
using EventLogQueryResult = MTUComm.EventLogList.EventLogQueryResult;
using ParameterType       = MTUComm.Parameter.ParameterType;
using LexiAction          = Lexi.Lexi.LexiAction;

namespace MTUComm
{
    public class MTUComm
    {
        #region Constants

        private const int BASIC_READ_1_ADDRESS = 0;
        private const int BASIC_READ_1_DATA    = 32;
        private const int BASIC_READ_2_ADDRESS = 244;
        private const int BASIC_READ_2_DATA    = 1;
        private const int DEFAULT_OVERLAP      = 6;
        private const int DEFAULT_LENGTH_AES   = 16;
        private const int SAME_MTU_ADDRESS     = 0;
        private const int SAME_MTU_DATA        = 10;
        private const int IC_OK                = 0;
        private const int IC_NOT_ACHIEVED      = 1;
        private const int IC_EXCEPTION         = 2;
        private const int WAIT_BTW_TURNOFF     = 500;
        private const int WAIT_BTW_IC          = 1000;
        private const int WAIT_BEFORE_READ     = 1000;
        private const int TIMES_TURNOFF        = 3;
        private const int DATA_READ_END_DAYS   = 60;
        private const byte CMD_START_LOGS      = 0x13;
        private const byte CMD_NEXT_LOG        = 0x14;
        private const int CMD_NEXT_RESULT_1    = 25;
        private const int CMD_NEXT_RESULT_2    = 5;
        private const int CMD_NEXT_BYTE_RESULT = 2;
        private const byte CMD_NEXT_WITH_DATA  = 0x00;
        private const byte CMD_NEXT_NO_DATA    = 0x01;
        private const byte CMD_NEXT_BUSY       = 0x02;
        private const int WAIT_BEFORE_LOGS     = 2000;
        private const int WAIT_BETWEEN_LOGS    = 1000;

        private const string ERROR_LOADDEMANDCONF = "DemandConfLoadException";
        private const string ERROR_LOADMETER      = "MeterLoadException";
        private const string ERROR_LOADMTU        = "MtuLoadException";
        private const string ERROR_LOADALARM      = "AlarmLoadException";
        private const string ERROR_NOTFOUNDMTU    = "MtuNotFoundException";
        private const string ERROR_LOADINTERFACE  = "InterfaceLoadException";
        private const string ERROR_LOADGLOBAL     = "GlobalLoadException";
        private const string ERROR_NOTFOUNDMETER  = "MeterNotFoundException";

        #endregion

        #region Delegates and Events

        public delegate Task ReadFabricHandler(object sender);
        public event ReadFabricHandler OnReadFabric;

        public delegate Task ReadMtuHandler(object sender, ReadMtuArgs e);
        public event ReadMtuHandler OnReadMtu;

        public delegate void TurnOffMtuHandler(object sender, TurnOffMtuArgs e);
        public event TurnOffMtuHandler OnTurnOffMtu;

        public delegate void TurnOnMtuHandler(object sender, TurnOnMtuArgs e);
        public event TurnOnMtuHandler OnTurnOnMtu;

        public delegate void ReadMtuDataHandler(object sender, ReadMtuDataArgs e);
        public event ReadMtuDataHandler OnReadMtuData;

        public delegate Task AddMtuHandler(object sender, AddMtuArgs e);
        public event AddMtuHandler OnAddMtu;

        public delegate void BasicReadHandler(object sender, BasicReadArgs e);
        public event BasicReadHandler OnBasicRead;

        public delegate void ErrorHandler ();
        public event ErrorHandler OnError;

        public delegate void ProgresshHandler(object sender, ProgressArgs e);
        public event ProgresshHandler OnProgress;

        #endregion

        #region Class Args

        /*
        public class ErrorArgs : EventArgs
        {
            public Exception exception { private set; get; }

            public ErrorArgs () { }

            public ErrorArgs ( Exception e )
            {
                this.exception = e;
            }
        }
        */

        public class ReadMtuArgs : EventArgs
        {
            public AMemoryMap MemoryMap { get; private set; }

            public Mtu Mtu { get; private set; }

            public ReadMtuArgs(AMemoryMap memorymap, Mtu mtype)
            {
                MemoryMap = memorymap;
                Mtu = mtype;
            }
        }

        public class ReadMtuDataArgs : EventArgs
        {
            /*
            public LogDataType Status { get; private set; }

            public int TotalEntries { get; private set; }
            public int CurrentEntry { get; private set; }

            public DateTime Start { get; private set; }
            public DateTime End { get; private set; }

            public MTUBasicInfo MtuType { get; private set; }


            public List<LogDataEntry> Entries { get; private set; }

            public ReadMtuDataArgs(LogDataType status, DateTime start, DateTime end, MTUBasicInfo mtype)
            {
                Status = status;
                TotalEntries = 0;
                CurrentEntry = 0;
                MtuType = mtype;
            }

            public ReadMtuDataArgs(LogDataType status, DateTime start, DateTime end, MTUBasicInfo mtype, List<LogDataEntry> entries)
            {
                Status = status;
                TotalEntries = entries.Count;
                CurrentEntry = entries.Count;
                Entries = entries;
                MtuType = mtype;
            }

            public ReadMtuDataArgs(LogDataType status, DateTime start, DateTime end, MTUBasicInfo mtype, int totalEntries, int currentEntry)
            {
                Status = status;
                TotalEntries = totalEntries;
                CurrentEntry = currentEntry;
                MtuType = mtype;
                Start = start;
                End = end;
            }
            */
        }

        public class ProgressArgs : EventArgs
        {
            public int Step { get; private set; }
            public int TotalSteps { get; private set; }
            public string Message { get; private set; }

            public ProgressArgs(int step, int totalsteps)
            {
                Step = step;
                TotalSteps = totalsteps;
                Message = "";
            }

            public ProgressArgs(int step, int totalsteps, string message)
            {
                Step = step;
                TotalSteps = totalsteps;
                Message = message;
            }
        }

        public class TurnOffMtuArgs : EventArgs
        {
            public Mtu Mtu { get; }
            public TurnOffMtuArgs ( Mtu Mtu )
            {
                this.Mtu = Mtu;
            }
        }

        public class TurnOnMtuArgs : EventArgs
        {
            public Mtu Mtu { get; }
            public TurnOnMtuArgs ( Mtu Mtu )
            {
                this.Mtu = Mtu;
            }
        }

        public class AddMtuArgs : EventArgs
        {
            public AMemoryMap MemoryMap { get; private set; }
            public Mtu MtuType { get; private set; }
            public MtuForm Form { get; private set; }
            public AddMtuLog AddMtuLog { get; private set; }

            public AddMtuArgs(AMemoryMap memorymap, Mtu mtype, MtuForm form, AddMtuLog addMtuLog )
            {
                MemoryMap = memorymap;
                MtuType = mtype;
                Form = form;
                AddMtuLog = addMtuLog;
            }
        }

        public class BasicReadArgs : EventArgs
        {
            public BasicReadArgs()
            {
            }
        }

        #endregion

        #region Attributes

        private Lexi.Lexi lexi;
        private Configuration configuration;
        private MTUBasicInfo _latest_mtu;
        private Mtu mtu;
        private Boolean isPulse = false;
        private Boolean mtuHasChanged;
        private bool basicInfoLoaded = false;
        private AddMtuLog addMtuLog;
        
        private MTUBasicInfo latest_mtu
        {
            get { return _latest_mtu; }
            set
            {
                _latest_mtu = value;
            }
        }

        #endregion

        #region Initialization

        public MTUComm(ISerial serial, Configuration configuration)
        {
            this.configuration = configuration;
            latest_mtu = new MTUBasicInfo(new byte[BASIC_READ_1_DATA + BASIC_READ_2_DATA]);
            lexi = new Lexi.Lexi(serial, Data.Get.IsIOS ? 10000 : 20000 );
            
            Singleton.Set = lexi;
        }

        #endregion

        #region Launch Actions

        public async void LaunchActionThread ( ActionType type, params object[] args )
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
                        Singleton.Get.Action.SetCurrentMtu ( this.latest_mtu );
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
                             await Task.Run ( () => Task_AddMtu ( ( AddMtuForm )args[ 0 ], ( string )args[ 1 ], ( Action )args[ 2 ] ) );
                        else await Task.Run ( () => Task_AddMtu ( ( Action )args[ 0 ] ) );
                        break;
                    //case ActionType.ReadMtu    : await Task.Run ( () => Task_DataRead () ); break;
                    case ActionType.ReadMtu    : await Task.Run ( () => Task_ReadMtu () ); break;
                    case ActionType.TurnOffMtu : await Task.Run ( () => Task_TurnOnOffMtu ( false ) ); break;
                    case ActionType.TurnOnMtu  : await Task.Run ( () => Task_TurnOnOffMtu ( true  ) ); break;
                    case ActionType.ReadData   : await Task.Run ( () => Task_DataRead () ); break;
                    case ActionType.BasicRead  : await Task.Run ( () => Task_BasicRead () ); break;
                    case ActionType.MtuInstallationConfirmation: await Task.Run ( () => Task_InstallConfirmation () ); break;
                    case ActionType.ReadFabric: await Task.Run ( () => Task_ReadFabric () ); break;
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

        public async Task<bool> AutodetectMetersEcoders (
            Mtu mtu,
            int portIndex = 1 )
        {
            this.mtu     = mtu;
            dynamic map  = this.GetMemoryMap ();
            bool isPort1 = ( portIndex == 1 );
            
            // NOTE: If process fails, Aclara STAR Programmer writes "ERROR: Check Meter" or "Bad Reading"
            // MTU ID: 63004810 -> ERROR: Check Meter ( Meter connected | AMCO C-700 5/8 )
            // MTU ID: 63021489 -> Bad Reading ( without Meter )
            // MTU ID: 63004809 -> Error Reading MTU ( without Meter )
            // MTU ID: 958029   -> Invalidad MTU Type Found ( Meter 1112 | NEPT T10 3/4 )
            
            try
            {
                // Clear previous auto-detection values
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
            
                // Force MTU to run enconder auto-detection for selected ports
                if ( isPort1 )
                     await map.P1EncoderAutodetect.ResetByteAndSetValueToMtu ( true );
                else await map.P2EncoderAutodetect.ResetByteAndSetValueToMtu ( true );
            
                // Check until recover data from the MTU, but no more than 40 seconds
                // MTU returns two values, Protocol and an echo ( it's not important )
                int  count = 1;
                int  wait  = 2;
                int  time  = 50;
                int  max   = ( int )( time / wait ); // Seconds / Seconds = Rounded max number of iterations
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
                
                // Set flag in MTU forcing to read meter
                map.ReadMeter.SetValueToMtu ( true );
                
                await Task.Delay ( 4 * 1000 );
                
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

        #region Data Read

        public async Task Task_DataRead ()
        {
            Global global = this.configuration.Global;

            try
            {
                // NOTE: Is impossible to save a full datetime in only four bytes,
                // because by definition it requires eight. This approach using only
                // three bytes implemented by Aclara, is only valid until 2255 ( byte = 8 bits = [0-255] )
                DateTime start = DateTime.Now.Date.Subtract ( new TimeSpan ( 30, 0, 0, 0 ) ); //global.NumOfDays, 0, 0, 0 ) );
                DateTime end   = DateTime.Now.Date.AddDays ( DATA_READ_END_DAYS );

                byte[] data = new byte[ 10 ]; // 1+1+4*2
                data[ 0 ] = 0x01; // Filter mode [ LogFilterMode.Match ]
                data[ 1 ] = 0x01; // Log entry type [ LogEntryType.MeterRead ]
                Array.Copy ( Utils.DateTimeToFourBytes ( start ), 0, data, 2, 4 ); // Start time
                Array.Copy ( Utils.DateTimeToFourBytes ( end   ), 0, data, 6, 4 ); // Stop time

                // Use address parameter to set request code
                
                await this.lexi.Write ( CMD_START_LOGS, data, null, null, LexiAction.OperationRequest ); // Return +2 ACK

                await Task.Delay ( WAIT_BEFORE_LOGS );

                // Recover all logs registered in the MTU for the specified date range
                int maxAttempts   = ( Data.Get.IsFromScripting ) ? 20 : 5;
                int countAttempts = 0;
                EventLogList eventLogList = new EventLogList ();
                ( byte[] bytes, int responseOffset ) fullResponse = ( null, 0 ); // echo + response
                while ( true )
                {
                    try
                    {
                        fullResponse =
                            await this.lexi.Write ( CMD_NEXT_LOG, null,
                                new uint[]{ CMD_NEXT_RESULT_1, CMD_NEXT_RESULT_2 }, // ACK with log entry or without
                                new ( int,int,byte )[] {
                                    ( CMD_NEXT_RESULT_1, CMD_NEXT_BYTE_RESULT, CMD_NEXT_WITH_DATA ), // Entry data included
                                    ( CMD_NEXT_RESULT_2, CMD_NEXT_BYTE_RESULT, CMD_NEXT_NO_DATA   ), // Complete but without data
                                    ( CMD_NEXT_RESULT_2, CMD_NEXT_BYTE_RESULT, CMD_NEXT_BUSY      )  // The MTU is busy
                                },
                                LexiAction.OperationRequest );
                    }
                    catch ( Exception e )
                    {
                        // Is not own exception
                        if ( ! Errors.IsOwnException ( e ) )
                            throw new PuckCantCommWithMtuException ();

                        // Finish without perform the action
                        else if ( ++countAttempts >= maxAttempts )
                            throw new ActionNotAchievedGetEventsLogException ();

                        // Try one more time
                        Errors.LogErrorNowAndContinue ( new AttemptNotAchievedGetEventsLogException () );
                        continue;
                    }

                    // Check if some event log was recovered, but first removed echo bytes
                    byte[] response = new byte[ fullResponse.bytes.Length - fullResponse.responseOffset ];
                    Array.Copy ( fullResponse.bytes, fullResponse.responseOffset, response, 0, response.Length );

                    EventLogQueryResult queryResult = eventLogList.TryToAdd ( response );
                    switch ( queryResult )
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
                                Errors.LogErrorNowAndContinue ( new MtuIsBusyToGetEventsLogException () );
                                await Task.Delay ( WAIT_BETWEEN_LOGS );
                            }
                            break;

                        // Wait a bit and try to read/recover the next log
                        case EventLogQueryResult.NextRead:
                            await Task.Delay ( WAIT_BETWEEN_LOGS );
                            break;

                        // Was last event log
                        case EventLogQueryResult.LastRead:
                            goto BREAK;
                    }
                }

                BREAK:

                Utils.Print ( "DataRead Finished: " + eventLogList.Count );
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     throw new PuckCantCommWithMtuException ();
                else throw e;
            }

            // if 

            /*
            bool last_packet = false;
            while (!last_packet)
            {
                LogQueryResult response = new LogQueryResult(lexi.GetNextLogQueryResult());
                switch (response.Status)
                {
                    case LogDataType.LastPacket:
                        last_packet = true;
                        OnReadMtuData(this, new ReadMtuDataArgs(response.Status, start, end, latest_mtu, entries));
                        break;
                    case LogDataType.Bussy:
                        OnReadMtuData(this, new ReadMtuDataArgs(response.Status, start, end, latest_mtu));
                        Thread.Sleep(100);
                        break;
                    case LogDataType.NewPacket:
                        entries.Add(response.Entry);
                        OnReadMtuData(this, new ReadMtuDataArgs(response.Status, start, end, latest_mtu, response.TotalEntries, response.CurrentEntry));
                        break;
                }
            }
            */
        }

        #endregion

        #region Install Confirmation

        public async Task Task_InstallConfirmation ()
        {
            if ( await this.InstallConfirmation_Logic () < IC_EXCEPTION )
                 await this.Task_ReadMtu ();
            else this.OnError ();
        }

        /// <summary>
        /// Installs the confirmation logic.
        /// </summary>
        /// <returns>0 OK, 1 Not achieved, 2 Error</returns>
        /// <param name="force">If set to <c>true</c> force.</param>
        /// <param name="time">Time.</param>
        private async Task<int> InstallConfirmation_Logic (
            bool force = false,
            int  time  = 0 )
        {
            Global global = this.configuration.Global;
        
            // DEBUG
            //this.WriteMtuBit ( 22, 0, false ); // Turn On MTU
            
            dynamic map = this.GetMemoryMap ();
            MemoryRegister<bool> regICNotSynced = map.InstallConfirmationNotSynced;
            MemoryRegister<bool> regICRequest   = map.InstallConfirmationRequest;
            
            bool wasNotAboutPuck = false;
            try
            {
                Utils.Print ( "InstallConfirmation trigger start" );
                
                await regICNotSynced.SetValueToMtu ( true );

                // MTU is turned off
                if ( ! force &&
                     this.latest_mtu.Shipbit )
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
                int  wait  = 5;
                int  max   = ( int )( global.TimeSyncCountDefault / wait ); // Seconds / Seconds = Rounded max number of iterations
                
                do
                {
                    // Update interface text to look the progress
                    int progress = ( int )Math.Round ( ( decimal )( ( count * 100.0 ) / max ) );
                    OnProgress ( this, new ProgressArgs ( count, max, "Checking IC... "+ progress.ToString() + "%" ) );
                    
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
            
                // Retry action ( thre times = first plus two replies )
                if ( ++time < global.TimeSyncCountRepeat )
                {
                    await Task.Delay ( WAIT_BTW_IC );
                    
                    return await this.InstallConfirmation_Logic ( force, time );
                }
                
                // Finish with error
                Errors.LogErrorNowAndContinue ( new ActionNotAchievedICException ( ( global.TimeSyncCountRepeat ) + "" ) );
                return IC_NOT_ACHIEVED;
            }
            
            return IC_OK;
        }

        #endregion

        #region Turn On|Off

        public async Task Task_TurnOnOffMtu (
            bool on )
        {        
            if ( on )
                 Utils.Print ( "TurnOffMtu start" );
            else Utils.Print ( "TurnOnMtu start"  );

            // Launchs exception 'ActionNotAchievedTurnOffException'
            if ( await this.TurnOnOffMtu_Logic ( on ) )
            {
                if ( on )
                     this.OnTurnOnMtu  ( this, new TurnOnMtuArgs  ( this.mtu ) );
                else this.OnTurnOffMtu ( this, new TurnOffMtuArgs ( this.mtu ) );
            }
        }

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
                
                // Retry action ( thre times = first plus two replies )
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

        #region ReadFabric

        public async Task Task_ReadFabric ()
        {
            try
            {
                OnProgress ( this, new ProgressArgs ( 0, 0, "Testing puck..." ) );

                // Only read all required registers once
                var map = this.GetMemoryMap ( true );

                // Activates flag to read Meter
                int MtuType = await  map.MtuType.GetValueFromMtu ();

                OnProgress ( this, new ProgressArgs ( 0, 0, "Successful MTU read (" + MtuType.ToString() + ")" ) );
                await OnReadFabric ( this );

            }
            catch ( Exception e )
            {
                Errors.LogErrorNow ( new PuckCantCommWithMtuException () );
            }
        }

        #endregion

        #region Read MTU

        public async Task Task_ReadMtu ()
        {
            try
            {
                OnProgress ( this, new ProgressArgs ( 0, 0, "Reading from MTU..." ) );
            
                // Only read all required registers once
                var map = this.GetMemoryMap ( true );
                
                // Activates flag to read Meter
                await map.ReadMeter.SetValueToMtu ( true );
                
                await Task.Delay ( WAIT_BEFORE_READ );
                
                await this.CheckIsTheSameMTU ();
             
                // Generates log using the interface
                await this.OnReadMtu ( this, new ReadMtuArgs ( map, this.mtu ) );
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

        #region Write MTU

        private Action truquitoAction;

        private async Task Task_AddMtu ( Action action )
        {
            truquitoAction     = action;
            Parameter[] ps     = action.GetParameters ();
            dynamic     form   = new AddMtuForm ( this.mtu );
            Global      global = this.configuration.Global;
            form.usePort2      = false;
            bool scriptUseP2   = false;
            
            List<Meter> meters;
            List<string> portTypes;
            Meter meterPort1 = null;
            Meter meterPort2 = null;
            
            try
            {
                dynamic map = this.GetMemoryMap ();
            
                bool port2IsActivated = await map.P2StatusFlag.GetValue ();
    
                // Recover parameters from script and translante from Aclara nomenclature to our own
                foreach ( Parameter parameter in ps )
                {
                    // Launchs exception 'TranslatingParamsScriptException'
                    // Launchs exception 'SameParameterRepeatScriptException'
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
                    portTypes  = this.mtu.Ports[ 0 ].GetPortTypes ();
                    
                    // Is not valid Meter ID ( not present in Meter.xml )
                    if ( meterPort1.IsEmpty )
                        throw new ScriptingAutoDetectMeterMissing ();
                    
                    // Current MTU does not support selected Meter
                    else if ( ! portTypes.Contains ( form.Meter.Value ) && // By Meter Id = Numeric
                              ! portTypes.Contains ( meterPort1.Type ) )   // By Type = Chars
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
                        portTypes  = this.mtu.Ports[ 1 ].GetPortTypes ();
                        
                        // Is not valid Meter ID ( not present in Meter.xml )
                        if ( meterPort2.IsEmpty )
                            throw new ScriptingAutoDetectMeterMissing ( string.Empty, 2 );
                        
                        // Current MTU does not support selected Meter
                        else if ( ! portTypes.Contains ( form.Meter_2.Value ) && // By Meter Id = Numeric
                                  ! portTypes.Contains ( meterPort2.Type ) )     // By Type = Chars
                            throw new ScriptingAutoDetectNotSupportedException ( string.Empty, 2 );
                            
                        // Set values for the Meter selected InterfaceTamper the script
                        this.mtu.Port2.MeterProtocol   = meterPort2.EncoderType;
                        this.mtu.Port2.MeterLiveDigits = meterPort2.LiveDigits;
                    }
                }

                #endregion
    
                #region Validation
    
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
                            if ( fail = NoELTxt ( value, global.WorkOrderLength ) )
                                msgDescription = "should be equal to or less than global.WorkOrderLength (" + global.WorkOrderLength + ")";
                            
                            // Do not use
                            if ( ! fail &&
                                 ! global.WorkOrderRecording )
                                if ( parameter.Port == 0 )
                                     form.RemoveParameter ( FIELD.WORK_ORDER   );
                                else form.RemoveParameter ( FIELD.WORK_ORDER_2 );
                            break;
                            #endregion
                            #region MTU Id Old
                            case FIELD.MTU_ID_OLD:
                            if ( fail = NoEqNum ( value, global.MtuIdLength ) )
                                msgDescription = "should be equal to global.MtuIdLength (" + global.MtuIdLength + ")";
                            
                            // Do not use
                            if ( ! fail &&
                                 action.type != ActionType.ReplaceMTU &&
                                 action.type != ActionType.ReplaceMtuReplaceMeter )
                                form.RemoveParameter ( FIELD.MTU_ID_OLD );
                            break;
                            #endregion
                            #region Meter Serial Number
                            case FIELD.METER_NUMBER:
                            case FIELD.METER_NUMBER_2:
                            case FIELD.METER_NUMBER_OLD:
                            case FIELD.METER_NUMBER_OLD_2:
                            if ( fail = NoELTxt ( value, global.MeterNumberLength ) )
                                msgDescription = "should be equal to or less than global.MeterNumberLength (" + global.MeterNumberLength + ")";
                            
                            // Do not use
                            if ( ! fail &&
                                 ! global.UseMeterSerialNumber )
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
                            }
                            break;
                            #endregion
                            #region Meter Reading
                            case FIELD.METER_READING:
                            case FIELD.METER_READING_2:
                            if ( ! isAutodetectMeter )
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
                            if ( fail = NoELNum ( value, 12 ) )
                                msgDescription = "should be equal to or less than 12";
                            
                            // Do not use
                            if ( ! fail &&
                                 ! global.OldReadingRecording )
                            {
                                if ( parameter.Port == 0 )
                                     form.RemoveParameter ( FIELD.METER_READING_OLD   );
                                else form.RemoveParameter ( FIELD.METER_READING_OLD_2 );
                            }
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
                            {
                                readIntervalList.AddRange ( new string[]{
                                    "10 Min",
                                    "5 Min"
                                });
                            }
                            
                            value = value.ToLower ()
                                         .Replace ( "hr", "hour" )
                                         .Replace ( "h", "H" )
                                         .Replace ( "m", "M" );
                            if ( fail = Empty ( value ) || ! readIntervalList.Contains ( value ) )
                                msgDescription = "should be one of the possible values and using Hr/s or Min";
                            break;
                            #endregion
                            #region Snap Reads
                            case FIELD.SNAP_READS:
                            if ( fail = EmptyNum ( value ) )
                                msgDescription = "should be a valid numeric value";
                            
                            // Do not use
                            if ( ! fail &&
                                 ( ! global.AllowDailyReads ||
                                   ! mtu.DailyReads ) )
                                form.RemoveParameter ( FIELD.SNAP_READS );
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
            
                await this.Task_AddMtu ( form, action.user, action );
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     throw new PuckCantCommWithMtuException ();
                else throw e;
            }
        }

        private async Task Task_AddMtu (
            dynamic form,
            string user,
            Action action )
        {
            Mtu    mtu    = form.mtu;
            Global global = configuration.Global;
        
            this.mtu = mtu;

            try
            {
                Logger logger = ( ! Data.Get.IsFromScripting ) ? new Logger () : truquitoAction.logger;
                addMtuLog = new AddMtuLog ( logger, form, user );

                #region Turn Off MTU

                Utils.Print ( "------TURN_OFF_START-----" );

                OnProgress ( this, new ProgressArgs ( 0, 0, "Turning Off..." ) );

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

                    OnProgress ( this, new ProgressArgs ( 0, 0, "Checking Encoder..." ) );

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

                OnProgress ( this, new ProgressArgs ( 0, 0, "Preparing MemoryMap..." ) );

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
                    // If meter reading was not present, fill in to zeros up to length equals to selected Meter livedigits
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

                if ( global.AllowDailyReads && mtu.DailyReads &&
                     form.ContainsParameter ( FIELD.SNAP_READS ) )
                {
                    map.DailyGMTHourRead = form.SnapReads.Value;
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
                            // In family 31xx32xx the only MTU that use GasCutWireAlarm is 148

                            /*
                            This is for family 342x, not for 31xx32xx
                            if (tempEvt.tempMtu.CurrentMtu.GasCutWireAlarm)
                            {
                                / log
                                FrmMain.myglobals.log.MtuData("CutAlarmCable", myglobals.alarmList[index].CutAlarmCable ? "Enabled" : "Disabled", "Cut Alarm Cable");
                                / set
                                tempEvt.tempMtu.Port[0].SetBitInfoFromCertainByte(Mtu.ArchTamperMemorySet + 2, //2 ^ 5// 32, myglobals.alarmList[index].CutAlarmCable);
                            }
                            Mtu.ArchTamperMemorySet  = 192
                            */
                            

                            // Set alarms [ Alarm Message Transmission ]
                            if ( mtu.InsufficientMemory  ) map.InsufficientMemoryAlarm = alarms.InsufficientMemory;
                            if ( mtu.GasCutWireAlarm     ) map.GasCutWireAlarm         = alarms.CutAlarmCable;
                            if ( mtu.SerialComProblem    ) map.SerialComProblemAlarm   = alarms.SerialComProblem;
                            if ( mtu.LastGasp            ) map.LastGaspAlarm           = alarms.LastGasp;
                            if ( mtu.TiltTamper          ) map.TiltAlarm               = alarms.Tilt;
                            if ( mtu.MagneticTamper      ) map.MagneticAlarm           = alarms.Magnetic;
                            if ( mtu.RegisterCoverTamper ) map.RegisterCoverAlarm      = alarms.RegisterCover;
                            if ( mtu.ReverseFlowTamper   ) map.ReverseFlowAlarm        = alarms.ReverseFlow;
                            if ( mtu.SerialCutWire       ) map.SerialCutWireAlarm      = alarms.SerialCutWire;
                            if ( mtu.TamperPort1         ) map.P1CutWireAlarm          = alarms.TamperPort1;
                            if ( mtu.TamperPort2         ) map.P2CutWireAlarm          = alarms.TamperPort2;

                            // Set immediate alarms [ Alarm Message Immediate ]
                            if ( mtu.InsufficientMemoryImm ) map.InsufficientMemoryImmAlarm = alarms.InsufficientMemoryImm;
                            if ( mtu.GasCutWireAlarmImm    ) map.GasCutWireImmAlarm         = alarms.CutWireAlarmImm;
                            if ( mtu.SerialComProblemImm   ) map.SerialComProblemImmAlarm   = alarms.SerialComProblemImm;
                            if ( mtu.LastGaspImm           ) map.LastGaspImmAlarm           = alarms.LastGaspImm;
                            if ( mtu.TamperPort1Imm        ) map.P1CutWireImmAlarm          = alarms.TamperPort1Imm;
                            if ( mtu.TamperPort2Imm        ) map.P2CutWireImmAlarm          = alarms.TamperPort2Imm;
                            if ( mtu.InterfaceTamperImm    ) map.InterfaceImmAlarm          = alarms.InterfaceTamperImm;
                            if ( mtu.SerialCutWireImm      ) map.SerialCutWireImmAlarm      = alarms.SerialCutWireImm;

                            // Write directly ( without conditions )
                            map.ImmediateAlarm = alarms.ImmediateAlarmTransmit;
                            map.UrgentAlarm    = alarms.DcuUrgentAlarm;
                            
                            // Overlap count
                            map.MessageOverlapCount = alarms.Overlap;
                            if ( form.usePort2 )
                                map.P2MessageOverlapCount = alarms.Overlap;

                            // For the moment only for the family 33xx
                            if ( map.ContainsMember ( "AlarmMask1" ) ) map.AlarmMask1 = false; // Set '0'
                            if ( map.ContainsMember ( "AlarmMask2" ) ) map.AlarmMask2 = false;
                        }
                        catch ( Exception e )
                        {

                        }
                    }
                    // No alarm profile was selected before launch the action
                    else throw new SelectedAlarmForCurrentMtuException ();
                }

                #endregion

                #region Frequencies

                if ( mtu.TimeToSync &&
                     mtu.IsNewVersion &&
                     global.AFC &&
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
                    Utils.Print ( "----ENCRYPTION_START-----" );
                
                    OnProgress ( this, new ProgressArgs ( 0, 0, "Encrypting..." ) );
                
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
                        // Using .Net API
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
                        
                        // Try to write and validate AES encryption key up to five times
                        for ( int i = 0; i < 5; i++ )
                        {
                            // Write key in the MTU
                            Utils.Print ( "Write key to MTU" );
                            await regAesKey.SetValueToMtu ( aesKey );
                            
                            Thread.Sleep ( 1000 );
                            
                            // Verify if the MTU is encrypted
                            Utils.Print ( "Read Encrypted from MTU" );
                            bool encrypted   = ( bool )await regEncrypted .GetValueFromMtu ();
                            Utils.Print ( "Read EncryptedIndex from MTU" );
                            int  encrypIndex = ( int  )await regEncryIndex.GetValueFromMtu ();
                            
                            if ( ! encrypted || encrypIndex <= -1 )
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
                    catch ( Exception e )
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
                        throw new ActionNotAchievedEncryptionException ( "5" );
                    
                    await this.CheckIsTheSameMTU ();
                    
                    Utils.Print ( "----ENCRYPTION_FINISH----" );
                }

                #endregion

                #region Write to MTU

                Utils.Print ( "---WRITE_TO_MTU_START----" );

                OnProgress ( this, new ProgressArgs ( 0, 0, "Writing MemoryMap to MTU..." ) );

                // Write changes into MTU
                await this.WriteMtuModifiedRegisters ( map );
                await addMtuLog.LogAddMtu ();
                
                Utils.Print ( "---WRITE_TO_MTU_FINISH---" );

                #endregion

                #endregion

                await this.CheckIsTheSameMTU ();

                #region Turn On MTU

                Utils.Print ( "------TURN_ON_START------" );

                OnProgress ( this, new ProgressArgs ( 0, 0, "Turning On..." ) );

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
                    if ( mtu.InterfaceTamper ) await map.InterfaceAlarm.SetValueToMtu ( alarms.InterfaceTamper );
                }

                #endregion

                #region Install Confirmation

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
                
                    OnProgress ( this, new ProgressArgs ( 0, 0, "Install Confirmation..." ) );
                
                    // Force to execute Install Confirmation avoiding problems
                    // with MTU shipbit, because MTU is just turned on
                    if ( await this.InstallConfirmation_Logic ( true ) > IC_OK )
                    {
                        // If IC fails by any reason, add 4 seconds delay before
                        // reading MTU Tamper Memory settings for Tilt Alarm
                        await Task.Delay ( 4000 );
                    }
                    
                    Utils.Print ( "--------IC_FINISH--------" );
                }

                #endregion

                await this.CheckIsTheSameMTU ();

                #region Read MTU

                Utils.Print ( "----FINAL_READ_START-----" );
                
                OnProgress ( this, new ProgressArgs ( 0, 0, "Reading from MTU..." ) );
                
                // Used to check if all data was write ok, and then to generate the
                // final log without read again from the MTU the registers already read
                if ( ( await map.GetModifiedRegistersDifferences ( this.GetMemoryMap ( true ) ) ).Length > 0 )
                    throw new PuckCantCommWithMtuException ();
                
                Utils.Print ( "----FINAL_READ_FINISH----" );

                #endregion

                await this.CheckIsTheSameMTU ();

                // Generate log to show on device screen
                await this.OnAddMtu ( this, new AddMtuArgs ( map, mtu, form, addMtuLog ) );
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

        public void Task_BasicRead ()
        {
            BasicReadArgs args = new BasicReadArgs();
            OnBasicRead(this, args);
        }

        #endregion

        #region Read|Write from|to MTU

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

        public async Task<bool> ReadMtuBit ( uint address, uint bit )
        {
            byte value = ( await lexi.Read ( address, 1 ) )[ 0 ];

            return ( ( ( value >> ( int )bit ) & 1 ) == 1 );
        }

        public async Task<bool> WriteMtuBitAndVerify ( uint address, uint bit, bool active, bool verify = true )
        {
            // Read current value
            byte systemFlags = ( await lexi.Read ( address, 1 ) )[ 0 ];

            // Modify bit and write to MTU
            if ( active )
                 systemFlags = ( byte ) ( systemFlags |    1 << ( int )bit   );
            else systemFlags = ( byte ) ( systemFlags & ~( 1 << ( int )bit ) );
            
            await lexi.Write ( address, new byte[] { systemFlags } );

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
        
        private dynamic GetMemoryMap (
            bool readFromMtuOnlyOnce = false )
        {
            // Prepare memory map
            string memory_map_type = configuration.GetMemoryMapTypeByMtuId ( this.mtu );
            int    memory_map_size = configuration.GetmemoryMapSizeByMtuId ( this.mtu );
            
            return new MemoryMap.MemoryMap ( new byte[ memory_map_size ], memory_map_type, readFromMtuOnlyOnce );
        }
        
        private async Task LoadMtuAndMetersBasicInfo ()
        {
            OnProgress ( this, new ProgressArgs ( 0, 0, "Initial Reading..." ) );

            if ( await this.LoadMtuBasicInfo () )
            {
                this.basicInfoLoaded = true;
            
                MtuForm.SetBasicInfo ( latest_mtu );
                
                // Launchs exception 'MtuTypeIsNotFoundException'
                this.mtu = configuration.GetMtuTypeById ( ( int )this.latest_mtu.Type );
                
                if ( this.mtuHasChanged )
                {
                    for ( int i = 0; i < this.mtu.Ports.Count; i++ )
                        latest_mtu.setPortType ( i, this.mtu.Ports[ i ].Type );
                    
                    if ( latest_mtu.isEncoder ) { }
                }
            }
        }

        private async Task<bool> LoadMtuBasicInfo (
            bool isAfterWriting = false )
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
            catch ( Exception e )
            {
                if ( ! isAfterWriting )
                     Errors.LogErrorNow ( new PuckCantCommWithMtuException () );
                else Errors.LogErrorNow ( new PuckCantReadFromMtuAfterWritingException () );
                
                return false;
            }

            MTUBasicInfo mtu_info = new MTUBasicInfo ( finalRead.ToArray () );
            this.mtuHasChanged = ( latest_mtu.Id   == 0 ||
                                   latest_mtu.Type == 0 ||
                                   mtu_info.Id     != latest_mtu.Id ||
                                   mtu_info.Type   != latest_mtu.Type );
            
            latest_mtu = mtu_info;
            
            return this.mtuHasChanged;
        }

        private async Task CheckIsTheSameMTU ()
        {
            byte[] read;
            try
            {
                read = await lexi.Read ( SAME_MTU_ADDRESS, SAME_MTU_DATA );
            }
            catch ( Exception e )
            {
                throw new PuckCantCommWithMtuException ();
            }

            uint mtuType = read[ 0 ];

            byte[] id_stream = new byte[ 4 ];
            Array.Copy ( read, 6, id_stream, 0, 4 );
            uint mtuId = BitConverter.ToUInt32 ( id_stream, 0 );

            if ( mtuType != latest_mtu.Type ||
                   mtuId != latest_mtu.Id )
                throw new MtuHasChangeBeforeFinishActionException ();
        }

        public MTUBasicInfo GetBasicInfo ()
        {
            return this.latest_mtu;
        }

        private byte GetByteSettingOnlyOneBit ( int bit )
        {
            BitArray bits = new BitArray ( bit + 1 );
            
            if ( bit > 0 )
                for ( int i = 0; i < bit; i++ )
                    bits[ i ] = false;
                    
            bits[ bit ] = true;
            
            // Left to right: e.g. 4 is not 100 but false false true
            
            byte[] result = new byte[ 1 ];
            bits.CopyTo ( result, 0 );
            
            return result[ 0 ];
        }

        #endregion
    }
}
