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

using ActionType    = MTUComm.Action.ActionType;
using FIELD         = MTUComm.actions.AddMtuForm.FIELD;
using LogDataType   = MTUComm.LogQueryResult.LogDataType;
using ParameterType = MTUComm.Parameter.ParameterType;

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
        private const int WAIT_BTW_TURNOFF     = 500;
        private const int WAIT_BTW_IC          = 1000;
        private const int WAIT_BEFORE_READ     = 1000;
        
        private const int TIMES_TURNOFF = 3;

        private const string ERROR_LOADDEMANDCONF = "DemandConfLoadException";
        private const string ERROR_LOADMETER = "MeterLoadException";
        private const string ERROR_LOADMTU = "MtuLoadException";
        private const string ERROR_LOADALARM = "AlarmLoadException";
        private const string ERROR_NOTFOUNDMTU = "MtuNotFoundException";
        private const string ERROR_LOADINTERFACE = "InterfaceLoadException";
        private const string ERROR_LOADGLOBAL = "GlobalLoadException";
        private const string ERROR_NOTFOUNDMETER = "MeterNotFoundException";
        
        private const int RESULT_OK           = 0;
        private const int RESULT_NOT_ACHIEVED = 1;
        private const int RESULT_EXCEPTION    = 2;

        #endregion

        #region Delegates and Events

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
            lexi = new Lexi.Lexi(serial, 10000);
            
            Singleton.Set = lexi;
        }

        #endregion

        #region Launch Actions

        public async void LaunchActionThread ( ActionType type, params object[] args )
        {
            try
            {
                // Avoid to load more than one time the basic info for the same action,
                // because an action can be launched multiple times because of exceptions
                // that cancel the action but not move to the main menu and could be happen
                // that perform the basic read with a different MTU
                if ( ! this.basicInfoLoaded )
                    await this.LoadMtuAndMetersBasicInfo ();
                
                Action.SetCurrentMtu ( this.latest_mtu );

                switch (type)
                {
                    case ActionType.AddMtu:
                    case ActionType.AddMtuAddMeter:
                    case ActionType.AddMtuReplaceMeter:
                    case ActionType.ReplaceMTU:
                    case ActionType.ReplaceMeter:
                    case ActionType.ReplaceMtuReplaceMeter:
                        // Interactive and Scripting
                        if ( args.Length > 1 )
                             await Task.Run(() => Task_AddMtu ( (AddMtuForm)args[0], (string)args[1], (Action)args[2] ) );
                        else await Task.Run(() => Task_AddMtu ( (Action)args[0] ) );
                        break;
                    case ActionType.ReadMtu    : await Task.Run(() => Task_ReadMtu()); break;
                    case ActionType.TurnOffMtu : await Task.Run(() => Task_TurnOnOffMtu ( false ) ); break;
                    case ActionType.TurnOnMtu  : await Task.Run(() => Task_TurnOnOffMtu ( true  ) ); break;
                    case ActionType.ReadData   : await Task.Run(() => Task_ReadDataMtu((int)args[0])); break;
                    case ActionType.BasicRead  : await Task.Run(() => Task_BasicRead()); break;
                    case ActionType.MtuInstallationConfirmation: await Task.Run(() => Task_InstallConfirmation()); break;
                    default: break;
                }
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

        #region Read Data

        public void Task_ReadDataMtu ( int NumOfDays )
        {
            DateTime start = DateTime.Now.Date.Subtract(new TimeSpan(NumOfDays, 0, 0, 0));
            DateTime end = DateTime.Now.Date.AddSeconds(86399);

            lexi.TriggerReadEventLogs(start, end);

            List<LogDataEntry> entries = new List<LogDataEntry>();

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
        }

        #endregion

        #region Install Confirmation

        public async Task Task_InstallConfirmation ()
        {
            if ( await this.InstallConfirmation_Logic () < RESULT_EXCEPTION )
                this.Task_ReadMtu ();
            else
                this.OnError ();
        }

        /// <summary>
        /// Installs the confirmation logic.
        /// </summary>
        /// <returns>0 OK, 1 Not achieved, 2 Error</returns>
        /// <param name="force">If set to <c>true</c> force.</param>
        /// <param name="time">Time.</param>
        private async Task<int> InstallConfirmation_Logic (
            bool force   = false,
            int  time    = 0 )
        {
            Global global = this.configuration.global;
        
            // DEBUG
            //this.WriteMtuBit ( 22, 0, false ); // Turn On MTU
            
            dynamic map = this.GetMemoryMap ();
            MemoryRegister<bool> regICNotSynced = map.InstallConfirmationNotSynced;
            MemoryRegister<bool> regICRequest   = map.InstallConfirmationRequest;
            
            bool wasNotAboutPuck = false;
            try
            {
                Utils.Print ( "InstallConfirmation trigger start" );
                
                await regICNotSynced.ValueWriteToMtu ( true );

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
                await regICRequest.ValueWriteToMtu ( true );

                bool fail;
                int  count = 1;
                int  wait  = 5;
                int  max   = ( int )( global.TimeSyncCountDefault / wait ); // Seconds / Seconds = Rounded max number of iterations
                
                do
                {
                    // Update interface text to look the progress
                    int progress = ( int )Math.Round ( ( decimal )( ( count * 100.0 ) / max ) );
                    OnProgress ( this, new ProgressArgs ( count, max, "Checking IC... "+ progress.ToString() + "%" ) );
                    
                    Thread.Sleep ( wait * 1000 );
                    
                    fail = await regICNotSynced.ValueReadFromMtu ();
                }
                // Is MTU not synced with DCU yet?
                while ( fail &&
                        ++count <= max );
                
                if ( fail )
                    throw new AttemptNotAchievedICException ();
            }
            catch ( Exception e )
            {
                /*
                // Action finished ok but launched a rare CRC exception = False negative
                if ( Errors.IsOwnException ( e ) &&
                     ! this.ReadMtuBit ( addressNotSynced, bitSynced ) )
                    return true;
                */
                
                if ( ! ( e is PuckCantCommWithMtuException ) &&
                     e is AttemptNotAchievedICException )
                    Errors.AddError ( e );
                // Finish
                else
                {
                    if ( ! wasNotAboutPuck )
                         Errors.LogErrorNowAndContinue ( new PuckCantCommWithMtuException () );
                    else Errors.LogErrorNowAndContinue ( e );
                    return RESULT_EXCEPTION;
                }
            
                // Retry action ( thre times = first plus two replies )
                if ( ++time < global.TimeSyncCountRepeat )
                {
                    Thread.Sleep ( WAIT_BTW_IC );
                    
                    return await this.InstallConfirmation_Logic ( force, time );
                }
                
                // Finish with error
                Errors.LogErrorNowAndContinue ( new ActionNotAchievedICException ( ( global.TimeSyncCountRepeat ) + "" ) );
                return RESULT_NOT_ACHIEVED;
            }
            
            return RESULT_OK;
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
            bool fromAdd = false,
            int  time = 0 )
        {
            try
            {
                dynamic map = this.GetMemoryMap ();
                MemoryRegister<bool> shipbit = map.Shipbit;
                
                await shipbit.ValueWriteToMtu ( ! on );         // Set state of the shipbit
                bool  read = await shipbit.ValueReadFromMtu (); // Read written value to verify
                
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
                else
                {
                    if ( ! fromAdd )
                         Errors.LogErrorNow ( new PuckCantCommWithMtuException () );
                    else throw new PuckCantCommWithMtuException ();
                    return false;
                }
                
                // Retry action ( thre times = first plus two replies )
                if ( ++time < TIMES_TURNOFF )
                {
                    Thread.Sleep ( WAIT_BTW_TURNOFF );
                    
                    return await this.TurnOnOffMtu_Logic ( on, fromAdd, time );
                }
                
                // Finish with error
                else
                {
                    if ( ! fromAdd )
                         Errors.LogErrorNow ( new ActionNotAchievedTurnOffException ( TIMES_TURNOFF + "" ) );
                    else throw new ActionNotAchievedTurnOffException ( TIMES_TURNOFF + "" );
                }
                return false;
            }
            return true;
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
                await map.ReadMeter.ValueWriteToMtu ( true );
                
                Thread.Sleep ( WAIT_BEFORE_READ );
                
                // Check if the MTU is still the same
                if ( ! await this.IsSameMtu () )
                    throw new MtuHasChangeBeforeFinishActionException ();
             
                // Generates log using the interface
                await this.OnReadMtu ( this, new ReadMtuArgs ( map, this.mtu ) );
            }
            // MtuHasChangeBeforeFinishActionException
            // System.IO.IOException = Puck is not well placed or is off
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                    Errors.LogErrorNow ( new PuckCantCommWithMtuException () );
                
                // Allow to continue rising the error
                else throw ( e );
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
            Global      global = form.global;
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
                            case FIELD.ACTIVITY_LOG_ID:
                            if ( fail = EmptyNum ( value ) )
                                msgDescription = "should be a valid numeric value";
                            break;
                            
                            case FIELD.ACCOUNT_NUMBER:
                            case FIELD.ACCOUNT_NUMBER_2:
                            if ( fail = NoEqNum ( value, global.AccountLength ) )
                                msgDescription = "should be equal to global.AccountLength (" + global.AccountLength + ")";
                            break;
                            
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
                            
                            case FIELD.MTU_ID_OLD:
                            if ( fail = NoEqNum ( value, global.MtuIdLength ) )
                                msgDescription = "should be equal to global.MtuIdLength (" + global.MtuIdLength + ")";
                            
                            // Do not use
                            if ( ! fail &&
                                 action.type != ActionType.ReplaceMTU &&
                                 action.type != ActionType.ReplaceMtuReplaceMeter )
                                form.RemoveParameter ( FIELD.MTU_ID_OLD );
                            break;
                            
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
                            
                            case FIELD.METER_TYPE:
                            case FIELD.METER_TYPE_2:
                            //...
                            break;
                            
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
                            
                            case FIELD.SNAP_READS:
                            if ( fail = EmptyNum ( value ) )
                                msgDescription = "should be a valid numeric value";
                            
                            // Do not use
                            if ( ! fail &&
                                 ( ! global.AllowDailyReads ||
                                   ! mtu.DailyReads ) )
                                form.RemoveParameter ( FIELD.SNAP_READS );
                            break;
                            
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
                            
                            case FIELD.FORCE_TIME_SYNC:
                            bool.TryParse ( value, out fail );
                            if ( fail = ! fail )
                                msgDescription = "should be 'true' or 'false'";
                            break;
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
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     Errors.LogErrorNow ( new PuckCantCommWithMtuException () );
                else Errors.LogErrorNow ( e );
                
                return;
            }

            this.Task_AddMtu ( form, action.user, action, true );
        }

        private async Task Task_AddMtu (
            dynamic form,
            string user,
            Action action,
            bool isFromScripting = false )
        {
            Mtu    mtu    = form.mtu;
            Global global = form.global;
        
            this.mtu = mtu;
            form.action = action;

            try
            {
                Logger logger = ( ! isFromScripting ) ? new Logger () : truquitoAction.logger;
                addMtuLog = new AddMtuLog ( logger, form, user, isFromScripting );

                #region Turn Off MTU

                Utils.Print ( "------TURN_OFF_START-----" );

                OnProgress ( this, new ProgressArgs ( 0, 0, "Turning Off..." ) );

                await this.TurnOnOffMtu_Logic ( false, true );
                addMtuLog.LogTurnOff ();
                
                Utils.Print ( "-----TURN_OFF_FINISH-----" );

                #endregion

                #region Add MTU

                Utils.Print ( "--------ADD_START--------" );

                OnProgress ( this, new ProgressArgs ( 0, 0, "Preparing MemoryMap..." ) );

                dynamic map = this.GetMemoryMap ();
                form.map = map;

                #region Account Number

                // Uses default value fill to zeros if parameter is missing in scripting
                map.P1MeterId = form.AccountNumber.GetValueOrDefault<ulong> ( global.AccountLength );
                if ( form.usePort2 &&
                     form.ContainsParameter ( FIELD.ACCOUNT_NUMBER_2 ) )
                    map.P2MeterId = form.AccountNumber_2.GetValueOrDefault<ulong> ( global.AccountLength );

                #endregion

                #region Meter Type

                Meter selectedMeter  = null;
                Meter selectedMeter2 = null;
                   
                if ( ! isFromScripting )
                     selectedMeter = (Meter)form.Meter.Value;
                else selectedMeter = this.configuration.getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter.Value ) );
                map.P1MeterType = selectedMeter.Id;

                if ( form.usePort2 &&
                     form.ContainsParameter ( FIELD.METER_TYPE_2 ) )
                {
                    if ( ! isFromScripting )
                         selectedMeter2 = (Meter)form.Meter_2.Value;
                    else selectedMeter2 = this.configuration.getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter_2.Value ) );
                    map.P2MeterType = selectedMeter2.Id;
                }

                #endregion

                #region Initial Reading = Meter Reading

                string p1readingStr = "0";
                string p2readingStr = "0";

                if ( form.ContainsParameter ( FIELD.METER_READING ) )
                {
                    if ( ! isFromScripting ) // No mask
                         p1readingStr = form.MeterReading.Value;
                    else p1readingStr = selectedMeter.FillLeftLiveDigits ( form.MeterReading.GetValueOrDefault<ulong> ( selectedMeter.LiveDigits ) );
                    // Uses default value fill to zeros if parameter is missing in scripting
                    
                    ulong p1reading = ( ! string.IsNullOrEmpty ( p1readingStr ) ) ? Convert.ToUInt64 ( ( p1readingStr ) ) : 0;
    
                    map.P1MeterReading = p1reading / ( ( selectedMeter.HiResScaling <= 0 ) ? 1 : selectedMeter.HiResScaling );
                }
                
                if ( form.usePort2 &&
                     form.ContainsParameter ( FIELD.METER_READING_2 ) )
                {
                    if ( ! isFromScripting ) // No mask
                         p2readingStr = form.MeterReading_2.Value;
                    else p2readingStr = selectedMeter2.FillLeftLiveDigits ( form.MeterReading_2.GetValueOrDefault<ulong> ( selectedMeter2.LiveDigits ) );
                    // Uses default value fill to zeros if parameter is missing in scripting
                    
                    ulong p2reading = ( ! string.IsNullOrEmpty ( p2readingStr ) ) ? Convert.ToUInt64 ( ( p2readingStr ) ) : 0;
    
                    map.P2MeterReading = p2reading / ( ( selectedMeter2.HiResScaling <= 0 ) ? 1 : selectedMeter2.HiResScaling );
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

                #region Overlap count

                map.MessageOverlapCount = DEFAULT_OVERLAP;
                if ( form.usePort2 )
                    map.P2MessageOverlapCount = DEFAULT_OVERLAP;

                #endregion

                #region Alarm

                if ( mtu.RequiresAlarmProfile )
                {
                    Alarm alarms = (Alarm)form.Alarm.Value;
                    if ( alarms != null )
                    {
                        try
                        {
                            //if ( mtu.CutWireDelaySetting  ) map.xxx = alarms.CutWireDelaySetting;
                            //if ( mtu.GasCutWireAlarm      ) map.P1CutWireAlarm = alarms.LastGasp;
                            //if ( mtu.GasCutWireAlarmImm   ) map.xxx = alarms.LastGaspImm;
                            //if ( mtu.InsufficentMemory    ) map.xxx = alarms.InsufficientMemory;
                            //if ( mtu.InsufficentMemoryImm ) map.xxx = alarms.InsufficientMemoryImm;
                            if ( mtu.InterfaceTamper      ) map.P1InterfaceAlarm = alarms.InterfaceTamper;
                            //if ( mtu.InterfaceTamperImm   ) map.xxx = alarms.InterfaceTamperImm;
                            //if ( mtu.LastGasp             ) map.P1CutWireAlarm = alarms.LastGasp;
                            //if ( mtu.LastGaspImm          ) map.xxx = alarms.LastGaspImm;
                            if ( mtu.MagneticTamper       ) map.P1MagneticAlarm = alarms.Magnetic;
                            if ( mtu.RegisterCoverTamper  ) map.P1RegisterCoverAlarm = alarms.RegisterCover;
                            if ( mtu.ReverseFlowTamper    ) map.P1ReverseFlowAlarm = alarms.ReverseFlow;
                            //if ( mtu.SerialComProblem     ) map.xxx = alarms.SerialComProblem;
                            //if ( mtu.SerialComProblemImm  ) map.xxx = alarms.SerialComProblemImm;
                            //if ( mtu.SerialCutWire        ) map.xxx = alarms.SerialCutWire;
                            //if ( mtu.SerialCutWireImm     ) map.xxx = alarms.SerialCutWireImm;
                            //if ( mtu.TamperPort1          ) map.xxx = alarms.TamperPort1;
                            //if ( mtu.TamperPort2          ) map.xxx = alarms.TamperPort2;
                            //if ( mtu.TamperPort1Imm       ) map.xxx = alarms.TamperPort1Imm;
                            //if ( mtu.TamperPort2Imm       ) map.xxx = alarms.TamperPort2Imm;
                            if ( mtu.TiltTamper           ) map.P1TiltAlarm = alarms.Tilt;
                        
                            // Write directly ( without conditions )
                            map.P1ImmediateAlarm = alarms.ImmediateAlarmTransmit;
                            map.P1UrgentAlarm    = alarms.DcuUrgentAlarm;
                            
                            // P2CutWireAlarm
                            
                            // Message overlap count
                            // Number of new readings to take before transmit
                            map.MessageOverlapCount = alarms.Overlap;
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
                     global.AFC &&
                     await map.MtuSoftVersion.GetValue () >= 19 )
                {
                    map.F12WAYRegister1Int  = global.F12WAYRegister1;
                    map.F12WAYRegister10Int = global.F12WAYRegister10;
                    map.F12WAYRegister14Int = global.F12WAYRegister14;
                }

                #endregion

                Utils.Print ( "--------ADD_FINISH-------" );

                // Check if the MTU is still the same
                if ( ! await this.IsSameMtu () )
                    throw new MtuHasChangeBeforeFinishActionException ();

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
                            await regAesKey.ValueWriteToMtu ( aesKey );
                            
                            Thread.Sleep ( 1000 );
                            
                            // Verify if the MTU is encrypted
                            Utils.Print ( "Read Encrypted from MTU" );
                            bool encrypted   = ( bool )await regEncrypted .ValueReadFromMtu ();
                            Utils.Print ( "Read EncryptedIndex from MTU" );
                            int  encrypIndex = ( int  )await regEncryIndex.ValueReadFromMtu ();
                            
                            if ( ! encrypted || encrypIndex <= -1 )
                                continue; // Error
                            else
                            {
                                Utils.Print ( "Read EncryptionKey (SHA) from MTU" );
                                byte[] mtuSha = ( byte[] )await regAesKey.ValueReadFromMtu ( true ); // 32 bytes
                                
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
                    
                    // Check if the MTU is still the same
                    if ( ! await this.IsSameMtu () )
                        throw new MtuHasChangeBeforeFinishActionException ();
                    
                    Utils.Print ( "----ENCRYPTION_FINISH----" );
                }

                #endregion

                Utils.Print ( "---WRITE_TO_MTU_START----" );

                OnProgress ( this, new ProgressArgs ( 0, 0, "Writing MemoryMap to MTU..." ) );

                // Write changes into MTU
                await this.WriteMtuModifiedRegisters ( map );
                await addMtuLog.LogAddMtu ( isFromScripting );
                
                Utils.Print ( "---WRITE_TO_MTU_FINISH---" );

                #endregion

                #region Turn On MTU

                Utils.Print ( "------TURN_ON_START------" );

                OnProgress ( this, new ProgressArgs ( 0, 0, "Turning On..." ) );

                await this.TurnOnOffMtu_Logic ( true, true );
                addMtuLog.LogTurnOn ();
                
                Utils.Print ( "-----TURN_ON_FINISH------" );

                #endregion

                #region Alarm #2

                if ( mtu.RequiresAlarmProfile )
                {
                    Alarm alarms = ( Alarm )form.Alarm.Value;
                    
                    // PCI Alarm needs to be set after MTU is turned on, just before the read MTU
                    // The Status will show enabled during install and actual status (triggered) during the read
                    if ( mtu.InterfaceTamper ) map.P1InterfaceAlarm = alarms.InterfaceTamper;
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
                    if ( await this.InstallConfirmation_Logic ( true ) > RESULT_OK )
                    {
                        // If IC fails by any reason, add 4 seconds delay before
                        // reading MTU Tamper Memory settings for Tilt Alarm
                        Thread.Sleep ( 4000 );
                    }
                    
                    // Check if the MTU is still the same
                    if ( ! await this.IsSameMtu () )
                        throw new MtuHasChangeBeforeFinishActionException ();
                        
                    Utils.Print ( "--------IC_FINISH--------" );
                }

                #endregion

                #region Read MTU

                Utils.Print ( "----FINAL_READ_START-----" );

                OnProgress ( this, new ProgressArgs ( 0, 0, "Reading from MTU..." ) );

                await lexi.Write(64, new byte[] { 1 });
                Thread.Sleep(1000);

                byte[] buffer = new byte[ map.Length ];

                System.Buffer.BlockCopy( await lexi.Read(0, 255), 0, buffer, 0, 255);
                
                // Check if the MTU is still the same
                if ( ! await this.IsSameMtu () )
                    throw new MtuHasChangeBeforeFinishActionException ();
               
                if ( map.Length > 255)
                {
                    System.Buffer.BlockCopy( await lexi.Read(256, 64), 0, buffer, 256, 64);
                    System.Buffer.BlockCopy( await lexi.Read(318, 2), 0, buffer, 318, 2);
                }
                if ( map.Length > 320)
                {
                    //System.Buffer.BlockCopy(lexi.Read(320, 64), 0, buffer, 320, 64);
                    //System.Buffer.BlockCopy(lexi.Read(384, 64), 0, buffer, 384, 64);
                    //System.Buffer.BlockCopy(lexi.Read(448, 64), 0, buffer, 448, 64);
                    //System.Buffer.BlockCopy(lexi.Read(512, 64), 0, buffer, 512, 64);
                }
                if ( map.Length > 960)
                {
                    System.Buffer.BlockCopy( await lexi.Read(960, 64), 0, buffer, 960, 64);
                }

                // Check if the MTU is still the same
                if ( ! await this.IsSameMtu () )
                    throw new MtuHasChangeBeforeFinishActionException ();

                // Third parameter ( false ) is for avoiding update/read values from the MTU
                MemoryMap.MemoryMap readMap = new MemoryMap.MemoryMap ( buffer, map.Family, false );

                List<string> diff = new List<string> ( map.GetModifiedRegistersDifferences ( readMap ) );
                if ( diff.Count >  1 ||
                     diff.Count == 1 && !diff.Contains ( "EncryptionKey" ) )
                {
                    // ERROR
                }
                else
                {
                    // OK
                }
                
                Utils.Print ( "----FINAL_READ_FINISH----" );

                #endregion

                // Generate log to show on device screen
                await this.OnAddMtu ( this, new AddMtuArgs ( readMap, mtu, form, addMtuLog ) );
            }
            catch ( Exception e )
            {
                // Is not own exception
                if ( ! Errors.IsOwnException ( e ) )
                     Errors.LogErrorNow ( new PuckCantCommWithMtuException () );
                else Errors.LogErrorNow ( e );
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
                await modifiedRegisters[ i ].ValueWriteToMtu ( this.lexi );
            
            //foreach ( dynamic r in modifiedRegisters )
            //    await r.ValueWriteToMtu ( this.lexi );

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

        #region AuxiliaryFunctions
        
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

        private async Task<bool> IsSameMtu ()
        {
            byte[] read;
            try
            {
                read = await lexi.Read ( SAME_MTU_ADDRESS, SAME_MTU_DATA );
            }
            catch ( Exception e )
            {
                Errors.LogErrorNow ( new PuckCantCommWithMtuException () );
                return false;
            }

            uint mtuType = read[ 0 ];

            byte[] id_stream = new byte[ 4 ];
            Array.Copy ( read, 6, id_stream, 0, 4 );
            uint mtuId = BitConverter.ToUInt32 ( id_stream, 0 );

            return mtuType == latest_mtu.Type &&
                   mtuId   == latest_mtu.Id;
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
