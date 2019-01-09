using Lexi.Interfaces;
using MTUComm.actions;
using MTUComm.MemoryMap;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Xml;

using LogDataType = MTUComm.LogQueryResult.LogDataType;
using ActionType  = MTUComm.Action.ActionType;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Collections;

namespace MTUComm
{
    public class MTUComm
    {
        #region Constants

        private const int BASIC_READ_ADDRESS = 0;
        private const int BASIC_READ_DATA    = 32;
        private const int DEFAULT_OVERLAP    = 6;
        private const int DEFAULT_LENGTH_AES = 16;

        private const string ERROR_LOADDEMANDCONF = "DemandConfLoadException";
        private const string ERROR_LOADMETER = "MeterLoadException";
        private const string ERROR_LOADMTU = "MtuLoadException";
        private const string ERROR_LOADALARM = "AlarmLoadException";
        private const string ERROR_NOTFOUNDMTU = "MtuNotFoundException";
        private const string ERROR_LOADINTERFACE = "InterfaceLoadException";
        private const string ERROR_LOADGLOBAL = "GlobalLoadException";
        private const string ERROR_NOTFOUNDMETER = "MeterNotFoundException";

        #endregion

        #region Delegates and Events

        public delegate void ReadMtuHandler(object sender, ReadMtuArgs e);
        public event ReadMtuHandler OnReadMtu;

        public delegate void TurnOffMtuHandler(object sender, TurnOffMtuArgs e);
        public event TurnOffMtuHandler OnTurnOffMtu;

        public delegate void TurnOnMtuHandler(object sender, TurnOnMtuArgs e);
        public event TurnOnMtuHandler OnTurnOnMtu;

        public delegate void ReadMtuDataHandler(object sender, ReadMtuDataArgs e);
        public event ReadMtuDataHandler OnReadMtuData;

        public delegate ActionResult AddMtuHandler(object sender, AddMtuArgs e);
        public event AddMtuHandler OnAddMtu;

        public delegate void BasicReadHandler(object sender, BasicReadArgs e);
        public event BasicReadHandler OnBasicRead;

        public delegate void ErrorHandler(object sender, ErrorArgs e);
        public event ErrorHandler OnError;

        public delegate void ProgresshHandler(object sender, ProgressArgs e);
        public event ProgresshHandler OnProgress;

        #endregion

        #region Class Args

        public class ErrorArgs : EventArgs
        {
            public int Status { get; private set; }

            public String Message { get; private set; }

            public String LogMessage { get; private set; }

            public ErrorArgs(int status, String message, String logMessage)
            {
                Status = status;
                Message = message;
                LogMessage = logMessage;
            }

            public ErrorArgs(int status, String message)
            {
                Status = status;
                Message = message;
                LogMessage = message;
            }

            public ErrorArgs(String message, String logMessage)
            {
                Status = -1;
                Message = message;
                LogMessage = logMessage;
            }

            public ErrorArgs(String message)
            {
                Status = -1;
                Message = message;
                LogMessage = message;
            }
        }

        public class ReadMtuArgs : EventArgs
        {
            public AMemoryMap MemoryMap { get; private set; }

            public Mtu MtuType { get; private set; }

            public ReadMtuArgs(AMemoryMap memorymap, Mtu mtype)
            {
                MemoryMap = memorymap;
                MtuType = mtype;
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
            public uint MtuId { get; }
            public TurnOffMtuArgs(uint MtuId)
            {
                this.MtuId = MtuId;
            }
        }

        public class TurnOnMtuArgs : EventArgs
        {
            public uint MtuId { get; }
            public TurnOnMtuArgs(uint MtuId)
            {
                this.MtuId = MtuId;
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
        private MTUBasicInfo latest_mtu;
        private Mtu mtuType;
        private Boolean isPulse = false;
        private Boolean mtu_changed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether latest connected MTU has changed or MTU critical settings/configuration has changed.
        /// </summary>
        /// <value>
        ///   <see langword="true" /> if ; otherwise, <see langword="false" />.</value>
        /// <remarks>This flag is used to force Meter/Encoders/MemoryMap redetection </remarks>
        private Boolean changedMTUSettings
        {
            get
            {
                return mtu_changed;
            }
        }

        private AddMtuLog addMtuLog;

        #endregion

        #region Initialization

        public MTUComm(ISerial serial, Configuration configuration)
        {
            this.configuration = configuration;
            latest_mtu = new MTUBasicInfo(new byte[BASIC_READ_DATA]);
            lexi = new Lexi.Lexi(serial, 10000);
        }

        #endregion

        #region Launch Actions

        public async void LaunchActionThread ( ActionType type, params object[] args )
        {
            //System.Action actionToLaunch = null;

            try
            {

                //Gets MTU casci info ( type and id )
                // TODO: Descubrir porque al hacer un segundo basic read en la accion de AddMtu,
                // cuando se pulsa el boton, habiendo sido el primero el que se hace nada mas cargar
                // el formulario, la lectura casca. Ahora mismo esta condicion es para evitar que en
                // la accion AddMtu se haga una segunda lectura basica en modo interactivo pero si que
                // se permite hacer la primera en modo scripting
                if ( type != ActionType.AddMtu || type == ActionType.AddMtu && args.Length == 1 )
                    LoadMtuBasicInfo ();
                //if ( ! LoadMtuBasicInfo () )
                //    return;

                /*
                switch ( type )
                {
                    // NOTE: TaskFactory uses Action without parameters and elegant
                    // form to be able to do it, is using a lambda expression
                    case ActionType.ReadMtu   : actionToLaunch = ( () => Task_ReadMtu () ); break;
                    case ActionType.AddMtu    : actionToLaunch = ( () => Task_AddMtu ( ( AddMtuForm )args[ 0 ], ( string )args[ 1 ] ) ); break;
                    case ActionType.TurnOffMtu: actionToLaunch = ( () => Task_TurnOffMtu () ); break;
                    case ActionType.TurnOnMtu: actionToLaunch = ( () => Task_TurnOnMtu () ); break;
                    case ActionType.ReadData: actionToLaunch = ( () => Task_ReadDataMtu((int)args[0])); break;
                    case ActionType.BasicRead: actionToLaunch = ( () => Task_BasicRead () ); break;
                    case ActionType.MtuInstallationConfirmation: actionToLaunch = ( () => Task_InstallConfirmation() ); break;
                    default: break;
                }
                */

                switch (type)
                {
                    // NOTE: TaskFactory uses Action without parameters and elegant
                    // form to be able to do it, is using a lambda expression
                    case ActionType.ReadMtu: await Task.Run(() => Task_ReadMtu()); break;
                    case ActionType.AddMtu:
                        // Interactive and Scripting
                        if ( args.Length == 2 )
                             await Task.Run(() => Task_AddMtu((AddMtuForm)args[0], (string)args[1]));
                        else await Task.Run(() => Task_AddMtu((Action)args[0]));
                        break;
                    case ActionType.TurnOffMtu: await Task.Run(() => Task_TurnOffMtu()); break;
                    case ActionType.TurnOnMtu: await Task.Run(() => Task_TurnOnMtu()); break;
                    case ActionType.ReadData: await Task.Run(() => Task_ReadDataMtu((int)args[0])); break;
                    case ActionType.BasicRead: await Task.Run(() => Task_BasicRead()); break;
                    case ActionType.MtuInstallationConfirmation: await Task.Run(() => Task_InstallConfirmation()); break;
                    default: break;
                }
            }
            catch (Exception e)
            {
                OnError(this, TranslateException(e));
            }
        }

        #endregion

        #region Actions

        public void Task_ReadDataMtu ( int NumOfDays )
        {
            //If MTU has changed or critical settings/configuration force detection rutine
            if (this.changedMTUSettings)
            {
                RecoverMeterByMtuType();
            }

            DateTime start = DateTime.UtcNow.Date.Subtract(new TimeSpan(NumOfDays, 0, 0, 0));
            DateTime end = DateTime.UtcNow.Date.AddSeconds(86399);

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

        public void Task_InstallConfirmation ()
        {
            ErrorArgs errorArgs = this.InstallConfirmation_Logic ();

            if ( errorArgs == null )
                this.Task_ReadMtu ( true );
            else
                this.OnError ( this, errorArgs );
        }

        private ErrorArgs InstallConfirmation_Logic ()
        {
            //If MTU has changed or critical settings/configuration force detection rutine
            //if ( this.changedMTUSettings )
                this.RecoverMeterByMtuType ();

            if ( this.latest_mtu.Shipbit )
            {
                string message = "Current MTU ID: " + this.latest_mtu.Id + " type " + this.latest_mtu.Type + " is OFF";
                return new ErrorArgs ( 112, message, "Installation Confirmation Cancellation - " + message );
            }
            else
            {
                try
                {
                    MemRegister register = configuration.getFamilyRegister(mtuType.Id, "InstallationConfirmationRequest");

                    Console.WriteLine("InstallConfirmation trigger start");

                    byte mask = 2;
                    uint inConFlag = 116;

                    if (register != null)
                    {
                        mask = (byte)Math.Pow(2, register.Size);
                        inConFlag = (uint)register.Address;
                    }

                    byte systemFlags = (lexi.Read(inConFlag, 1))[0];
                    systemFlags |= mask; // set bit 0
                    lexi.Write(116, new byte[] { systemFlags });

                    byte sync_mask = 4;
                    byte sync_systemFlags = (lexi.Read(65, 1))[0];
                    sync_systemFlags |= sync_mask; // set bit 0
                    lexi.Write(65, new byte[] { sync_systemFlags });

                    byte valueWritten = (lexi.Read(inConFlag, 1))[0];
                    valueWritten &= mask;

                    int max_iters = 1;
                    while (valueWritten > 0 && max_iters <= 36)
                    {
                        int loop = (int)Math.Round((decimal)((max_iters * 100.0) / 36.0));

                        OnProgress(this, new ProgressArgs(max_iters, 36, "Checking Install Confirmation... ("+ loop.ToString() + "%)"));
                        Thread.Sleep(5000);
                        valueWritten = (lexi.Read(inConFlag, 1))[0];
                        valueWritten &= mask;
                        max_iters++;
                    }
                }
                catch ( Exception e )
                {
                    return TranslateException ( e );
                }
            }

            return null; // OK!
        }

        public void Task_TurnOffMtu ()
        {
            try
            {
                Console.WriteLine("TurnOffMtu start");

                this.TurnOffMtu_Logic();

                Console.WriteLine("MTU ID: " + latest_mtu.Id);

                TurnOffMtuArgs args = new TurnOffMtuArgs(latest_mtu.Id);
                OnTurnOffMtu(this, args);
            }
            catch ( Exception e )
            {
                OnError(this, TranslateException(e));
            }
        }

        private void TurnOffMtu_Logic ( int time = 0 )
        {
            byte mask = 1;
            byte systemFlags = (lexi.Read(22, 1))[0];
            systemFlags |= mask; // set bit 0
            lexi.Write(22, new byte[] { systemFlags });
            byte valueWritten = (lexi.Read(22, 1))[0];

            Console.WriteLine("Value to write: " + systemFlags.ToString() + " Value written: " + valueWritten.ToString());
            Console.WriteLine("TurnOnMtu end");

            // Fail to turn off MTU
            if ( systemFlags != valueWritten )
            {
                Thread.Sleep ( 500 );

                // Retry action ( thre times = first plus two replies )
                if ( time < 2 )
                    TurnOffMtu_Logic ( ++time );
                else
                    throw new Exception ( "TurnOff was not possible" );
            }
        }

        public void Task_TurnOnMtu ()
        {
            try
            {
                Console.WriteLine("TurnOnMtu start");

                this.TurnOnMtu_Logic();

                Console.WriteLine("MTU ID: " + latest_mtu.Id);

                TurnOnMtuArgs args = new TurnOnMtuArgs(latest_mtu.Id);
                OnTurnOnMtu(this, args);
            }
            catch ( Exception e )
            {
                OnError(this, TranslateException(e));
            }
        }

        private void TurnOnMtu_Logic ()
        {
            byte mask = 1;
            byte systemFlags = (lexi.Read(22, 1))[0];
            systemFlags &= (byte)~mask; // clear bit 0
            lexi.Write(22, new byte[] { systemFlags });
            byte valueWritten = (lexi.Read(22, 1))[0];

            Console.WriteLine("Value to write: " + systemFlags.ToString() + " Value written: " + valueWritten.ToString());
            Console.WriteLine("TurnOffMtu end");

            if (systemFlags != valueWritten)
            {
                // TODO: handle error condition
            }
        }

        public void Task_ReadMtu ( bool forcedetect = false )
        {
            try
            {
                //If MTU has changed or critical settings/configuration force detection rutine
                if (this.changedMTUSettings || forcedetect)
                    RecoverMeterByMtuType ();

                String memory_map_type = configuration.GetMemoryMapTypeByMtuId(mtuType.Id);
                int memory_map_size = configuration.GetmemoryMapSizeByMtuId(mtuType.Id);

                lexi.Write(64, new byte[] { 1 });
                Thread.Sleep(1000);

                byte[] buffer = new byte[1024];

                System.Buffer.BlockCopy(lexi.Read(0, 255), 0, buffer, 0, 255);

                if (memory_map_size > 255)
                {
                    System.Buffer.BlockCopy(lexi.Read(256, 64), 0, buffer, 256, 64);
                    System.Buffer.BlockCopy(lexi.Read(318, 2), 0, buffer, 318, 2);
                }

                if (memory_map_size > 320)
                {
                    //System.Buffer.BlockCopy(lexi.Read(320, 64), 0, buffer, 320, 64);
                    //System.Buffer.BlockCopy(lexi.Read(384, 64), 0, buffer, 384, 64);
                    //System.Buffer.BlockCopy(lexi.Read(448, 64), 0, buffer, 448, 64);
                    //System.Buffer.BlockCopy(lexi.Read(512, 64), 0, buffer, 512, 64);
                }

                if (memory_map_size > 960)
                    System.Buffer.BlockCopy(lexi.Read(960, 64), 0, buffer, 960, 64);

                ReadMtuArgs args = new ReadMtuArgs( new MemoryMap.MemoryMap ( buffer, memory_map_type ), mtuType );
                OnReadMtu(this, args);
            }
            catch (Exception e)
            {
                OnError(this, TranslateException(e));
            }
        }

        private Action truquitoAction;

        private void Task_AddMtu ( Action addMtuAction )
        {
            truquitoAction = addMtuAction;

            Mtu     mtu  = configuration.mtuTypes.FindByMtuId ( (int)MtuForm.mtuBasicInfo.Type );
            dynamic form = new AddMtuForm ( mtu );

            try
            {
                foreach ( Parameter parameter in addMtuAction.GetParameters () )
                    form.AddParameterTranslatingAclaraXml ( parameter );

                // Auto-detect Meter
                if ( ! form.ContainsParameter ( AddMtuForm.FIELD.SELECTED_METER  ) &&
                       form.ContainsParameter ( AddMtuForm.FIELD.NUMBER_OF_DIALS ) &&
                       form.ContainsParameter ( AddMtuForm.FIELD.DRIVE_DIAL_SIZE ) &&
                       form.ContainsParameter ( AddMtuForm.FIELD.UNIT_MEASURE    ) )
                {
                    List<Meter> meters = configuration.meterTypes.FindByDialDescription (
                        int.Parse ( form.NumberOfDials.Value ),
                        int.Parse ( form.DriveDialSize.Value ),
                        form.UnitOfMeasure.Value,
                        mtu.Flow );

                    if ( meters.Count > 0 )
                        form.AddParameter ( AddMtuForm.FIELD.SELECTED_METER, meters[ 0 ].Id.ToString () );
                    else
                    {
                        OnError ( this, new ErrorArgs ( 100,
                            "Meter.xml file does not exist or does not have selected Meter Type",
                            "Meter.xml file does not exist or does not have selected Meter Type" ) );
                        return;
                    }
                }

                // Auto-detect scripting Alarm profile
                List<Alarm> alarms = configuration.alarms.FindByMtuType ( (int)MtuForm.mtuBasicInfo.Type );
                if ( alarms.Count > 0 )
                {
                    Alarm alarm = alarms.Find ( a => string.Equals ( a.Name.ToLower (), "scripting" ) );
                    if ( alarm != null &&
                         form.mtu.RequiresAlarmProfile )
                        form.AddParameter ( AddMtuForm.FIELD.ALARM, alarm );
                }
            }
            catch ( Exception e )
            {
                OnError ( this, new ErrorArgs ( 113, "Error in parsing Trigger File", "Error in parsing Trigger File" ) );
                return;
            }

            this.Task_AddMtu ( form, addMtuAction.user, true );
        }

        private void Task_AddMtu ( dynamic form, string user, bool isFromScripting = false )
        {
            Mtu    mtu    = form.mtu;
            Global global = form.global;

            try
            {
                Logger logger = ( ! isFromScripting ) ? new Logger ( this.configuration ) : truquitoAction.logger;
                addMtuLog = new AddMtuLog ( logger, form, user, isFromScripting );

                #region Turn Off MTU

                this.TurnOffMtu_Logic();
                addMtuLog.LogTurnOff();

                #endregion

                #region Add MTU

                // Prepare memory map
                String memory_map_type = configuration.GetMemoryMapTypeByMtuId ( ( int )MtuForm.mtuBasicInfo.Type );
                int    memory_map_size = configuration.GetmemoryMapSizeByMtuId ( ( int )MtuForm.mtuBasicInfo.Type );

                byte[] memory = new byte[ memory_map_size ];
                dynamic map = new MemoryMap.MemoryMap ( memory, memory_map_type );

                bool useTwoPorts = mtu.TwoPorts;

                #region Meter Type

                Meter selectedMeter  = null;
                Meter selectedMeter2 = null;
                   
                if ( ! isFromScripting )
                     selectedMeter = (Meter)form.Meter.Value;
                else selectedMeter = this.configuration.getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter.Value ) );
                map.P1MeterType = selectedMeter.Id;

                if ( useTwoPorts )
                {
                    if ( ! isFromScripting )
                         selectedMeter2 = (Meter)form.Meter2.Value;
                    else selectedMeter2 = this.configuration.getMeterTypeById ( Convert.ToInt32 ( ( string )form.Meter2.Value ) );
                    map.P2MeterType = selectedMeter2.Id;
                }

                #endregion

                #region Service Port = Account Number = Activity Log ID

                map.P1MeterId = form.ServicePortId.Value;
                if ( useTwoPorts )
                    map.P2MeterId = form.ServicePortId2.Value;

                #endregion

                #region Reading Interval

                if ( global.IndividualReadInterval )
                {
                        // If not present in scripted mode, set default value to one/1 hour
                    map.ReadIntervalMinutes = ( form.ContainsParameter ( AddMtuForm.FIELD.READ_INTERVAL ) ) ?
                                                form.ReadInterval.Value : "1 Hr";
                    if ( useTwoPorts )
                        map.P2ReadInterval = ( form.ContainsParameter ( AddMtuForm.FIELD.READ_INTERVAL2 ) ) ?
                                               form.ReadInterval2.Value : "1 Hr";
                }

                #endregion

                #region Overlap count

                map.MessageOverlapCount = DEFAULT_OVERLAP;
                if (useTwoPorts)
                    map.P2MessageOverlapCount = DEFAULT_OVERLAP;

                #endregion

                #region Initial Reading = Meter Reading

                string mask = selectedMeter.MeterMask;
                string p1readingStr = "0";
                string p2readingStr = "0";

                if ( form.ContainsParameter ( AddMtuForm.FIELD.INITIAL_READING ) )
                {
                    if ( ! isFromScripting ||
                         string.IsNullOrEmpty ( mask ) ) // No mask
                    {
                        p1readingStr = form.InitialReading .Value;
                        p2readingStr = form.InitialReading2.Value;
                    }
                    else
                    {
                        p1readingStr = this.ApplyInitialReadMask ( mask, form.InitialReading .Value, selectedMeter.LiveDigits );
                        p2readingStr = this.ApplyInitialReadMask ( mask, form.InitialReading2.Value, selectedMeter.LiveDigits );
                    }
                }

                ulong p1reading = ( ! string.IsNullOrEmpty ( p1readingStr ) ) ? Convert.ToUInt64 ( (  p1readingStr ) ) : 0;
                ulong p2reading = ( ! string.IsNullOrEmpty ( p2readingStr ) ) ? Convert.ToUInt64 ( (  p1readingStr ) ) : 0;

                map.P1Reading = p1reading / ( ( selectedMeter.HiResScaling <= 0 ) ? 1 : selectedMeter.HiResScaling );
                if ( useTwoPorts )
                    map.P2Reading = p2reading / ( ( selectedMeter2.HiResScaling <= 0 ) ? 1 : selectedMeter2.HiResScaling );

                #endregion

                #region Alarm

                if ( mtu.RequiresAlarmProfile )
                {
                    Alarm alarms = (Alarm)form.Alarm.Value;
                    if ( alarms != null )
                    {
                        // Overlap
                        map.MessageOverlapCount = alarms.Overlap;

                        // P1ImmediateAlarm
                        map.P1ImmediateAlarm = alarms.ImmediateAlarmTransmit;

                        // P1UrgentAlarm
                        map.P1UrgentAlarm = alarms.DcuUrgentAlarm;

                        // P1MagneticAlarm
                        if ( mtu.MagneticTamper)
                            map.P1MagneticAlarm = alarms.Magnetic;

                        // P1RegisterCoverAlarm
                        if (mtu.RegisterCoverTamper)
                            map.P1RegisterCoverAlarm = alarms.RegisterCover;

                        // P1ReverseFlowAlarm
                        if (mtu.ReverseFlowTamper)
                            map.P1ReverseFlowAlarm = alarms.ReverseFlow;

                        // P1TiltAlarm
                        if (mtu.TiltTamper)
                            map.P1TiltAlarm = alarms.Tilt;

                        // P1InterfaceAlarm
                        if (mtu.InterfaceTamper)
                            map.P1InterfaceAlarm = alarms.InterfaceTamper;

                        if (useTwoPorts)
                        {
                            // Overlap
                            map.P2MessageOverlapCount = alarms.Overlap;

                            // P2ImmediateAlarm
                            map.P2ImmediateAlarm = alarms.ImmediateAlarmTransmit;

                            // P2UrgentAlarm
                            map.P2UrgentAlarm = alarms.DcuUrgentAlarm;

                            // P2MagneticAlarm
                            if (mtu.MagneticTamper)
                                map.P2MagneticAlarm = alarms.Magnetic;

                            // P2RegisterCoverAlarm
                            if (mtu.RegisterCoverTamper)
                                map.P2RegisterCoverAlarm = alarms.RegisterCover;

                            // P2ReverseFlowAlarm
                            if (mtu.ReverseFlowTamper)
                                map.P2ReverseFlowAlarm = alarms.ReverseFlow;

                            // P2TiltAlarm
                            if (mtu.TiltTamper)
                                map.P2TiltAlarm = alarms.Tilt;

                            // P2InterfaceAlarm
                            if (mtu.InterfaceTamper)
                                map.P2InterfaceAlarm = alarms.InterfaceTamper;
                        }
                
                    }
                }

                #endregion

                #region Encription Key

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider ();
                byte[] aesKey = new byte[ DEFAULT_LENGTH_AES ];
                rng.GetBytes ( aesKey );
                map.EncryptionKey = aesKey;
                for ( int i = 0; i < 15; i++ )
                    if ( aesKey[ i ] != memory[ 256 + i ] )
                        throw new Exception ( "AES key does not match" );

                #endregion

                // Encrypted
                // EncryptionIndex

                // fast message (not in pulse)
                // encoder digits to drop (not in pulse)

                // Write changes into MTU
                WriteMtuModifiedRegisters( map );
                addMtuLog.LogAddMtu ( isFromScripting );

                #endregion

                #region Turn On MTU

                this.TurnOnMtu_Logic();
                addMtuLog.LogTurnOn();

                #endregion

                #region Install Confirmation

                // If field ForceTimeSync is true inside Global,
                // after TurnOn it has to be performed an InstallConfirmation
                if ( global.ForceTimeSync )
                {
                    ErrorArgs errorArgs = this.InstallConfirmation_Logic ();
                    if ( errorArgs != null )
                        this.OnError ( this, errorArgs );
                }

                #endregion

                #region Read MTU

                lexi.Write(64, new byte[] { 1 });
                Thread.Sleep(1000);

                byte[] buffer = new byte[1024];

                System.Buffer.BlockCopy(lexi.Read(0, 255), 0, buffer, 0, 255);
               
                if (memory_map_size > 255)
                {
                    System.Buffer.BlockCopy(lexi.Read(256, 64), 0, buffer, 256, 64);
                    System.Buffer.BlockCopy(lexi.Read(318, 2), 0, buffer, 318, 2);
                }
                if (memory_map_size > 320)
                {
                    //System.Buffer.BlockCopy(lexi.Read(320, 64), 0, buffer, 320, 64);
                    //System.Buffer.BlockCopy(lexi.Read(384, 64), 0, buffer, 384, 64);
                    //System.Buffer.BlockCopy(lexi.Read(448, 64), 0, buffer, 448, 64);
                    //System.Buffer.BlockCopy(lexi.Read(512, 64), 0, buffer, 512, 64);
                }
                if (memory_map_size > 960)
                {
                    System.Buffer.BlockCopy(lexi.Read(960, 64), 0, buffer, 960, 64);
                }

                MemoryMap.MemoryMap readMemoryMap = new MemoryMap.MemoryMap(buffer, memory_map_type);

                List<string> diff = new List<string>(map.GetModifiedRegistersDifferences(readMemoryMap));
                if (diff.Count > 1 || (diff.Count == 1 && !diff.Contains("EncryptionKey")))
                {
                    // ERROR
                }
                else
                {
                    // OK
                }

                //if ( ! isFromScripting )
                    //form.logger.fixed_name = "";

                #endregion

                // Generate log to show on device screen
                AddMtuArgs addMtuArgs = new AddMtuArgs ( readMemoryMap, mtu, form, addMtuLog );
                this.OnAddMtu ( this, addMtuArgs );

                //ActionResult result = this.OnAddMtu ( this, addMtuArgs );
                //addMtuLog.LogReadMtu(result);

                // Generate xml log file and save on device
                //addMtuLog.Save ();
            }
            catch ( Exception e )
            {
                if ( isFromScripting )
                     OnError ( this, new ErrorArgs ( 113, "Error in parsing Trigger File", "Error in parsing Trigger File" ) );
                else OnError ( this, this.TranslateException ( e ) );
            }
        }

        private string ApplyInitialReadMask ( string mask, string value, int liveDigits )
        {
            if ( mask != string.Empty )
            {
                if (mask.IndexOfAny(new Char[] { 'X', 'x' }) >= 0)
                {
                    string leadingRead = mask.Substring(0, mask.IndexOfAny(new Char[] { 'X', 'x' }));
                    string trailingRead = mask.Substring(mask.IndexOfAny(new Char[] { 'X', 'x' }) + 1);
                    value = leadingRead + value + trailingRead; // por tanto seria ""+Port1Reading+"00"

                    if ( liveDigits < value.Length )
                        value = value.Substring ( value.Length - liveDigits );
                }
            }

            return value;
        }

        public string GetResultXML ()
        {
            return addMtuLog.ToString ();
        }

        public void Task_BasicRead ()
        {
            BasicReadArgs args = new BasicReadArgs();
            OnBasicRead(this, args);
        }

        #endregion

        #region Write to MTU

        public void WriteMtuModifiedRegisters ( MemoryMap.MemoryMap map )
        {
            List<dynamic> modifiedRegisters = map.GetModifiedRegisters ().GetAllElements ();
            foreach ( dynamic r in modifiedRegisters )
                this.WriteMtuRegister ( ( uint )r.address, map.memory, ( uint )r.size );

            modifiedRegisters.Clear ();
            modifiedRegisters = null;
        }

        public void WriteMtuRegister(uint address, byte[] memory, uint length)
        {
            byte[] tmp = new byte[ length ];
            Array.Copy ( memory, address, tmp, 0, length );

            Console.WriteLine ( "Write subpart: Addr {0} | Value {1} | Length {2}", address, BitConverter.ToString(tmp), length );

            lexi.Write ( address, tmp );
        }

        public void WriteMtuRegister ( uint address, byte[] values )
        {
            Console.WriteLine ( "Write values: Addr {0} | Value {1} | Length {2}", address, BitConverter.ToString(values), values.Length );

            lexi.Write ( address, values );
        }

        public T ReadMtuRegister<T> ( uint address, uint length )
        {
            byte value = ( lexi.Read ( address, length ) )[ 0 ];

            return ( T )( object )value;
        }

        public bool ReadMtuBit ( uint address, uint bit )
        {
            byte value = ( lexi.Read ( address, 1 ) )[ 0 ];

            return ( ( ( value >> ( int )bit ) & 1 ) == 1 );
        }

        public bool WriteMtuBitAndVerify ( uint address, uint bit, bool active, bool verify = true )
        {
            // Read current value
            byte systemFlags = ( lexi.Read ( 22, 1 ) )[ 0 ];

            // Modify bit and write to MTU
            systemFlags = ( byte ) ( systemFlags | ( ( ( active ) ? 1 : 0 ) << ( int )bit ) );
            lexi.Write ( address, new byte[] { systemFlags } );

            // Read new written value to verify modification
            if ( verify )
            {
                byte valueWritten = ( lexi.Read ( address, 1 ) )[ 0 ];
                return ( ( ( valueWritten >> ( int )bit ) & 1 ) == ( ( active ) ? 1 : 0 ) );
            }

            // Without verification
            return true;
        }

        #endregion

        // NO PARECE USARSE
        public byte[] ReadComplete ( byte addr, uint length )
        {
            byte[] tmp = new byte[length];
            uint maxReadBytes = 255;
            uint readsNumber = length / maxReadBytes;
            uint additionalBytes = length % maxReadBytes;

            for (uint i = 0; i < readsNumber; i++)
            {
                uint currentAddr = i * maxReadBytes;
                Array.Copy(lexi.Read(currentAddr, maxReadBytes), 0, tmp, currentAddr, maxReadBytes);
            }

            if (additionalBytes > 0)
            {
                uint currentAddr = readsNumber * maxReadBytes;
                Array.Copy(lexi.Read(currentAddr, additionalBytes), 0, tmp, currentAddr, additionalBytes);
            }

            return tmp;
        }

        #region AuxiliaryFunctions

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Internaly saves internal MTU type and ID used for communication logic</remarks>
        private void LoadMtuBasicInfo ()
        {
            MTUBasicInfo mtu_info = new MTUBasicInfo ( lexi.Read ( BASIC_READ_ADDRESS, BASIC_READ_DATA ) );
            mtu_changed = ! ( ( mtu_info.Id == latest_mtu.Id ) && ( mtu_info.Type == latest_mtu.Type ) );
            latest_mtu = mtu_info;

            MtuForm.SetBasicInfo ( latest_mtu );
        }

        private void RecoverMeterByMtuType ()
        {
            this.mtuType = configuration.GetMtuTypeById ( ( int )this.latest_mtu.Type );

            for ( int i = 0; i < mtuType.Ports.Count; i++ )
                latest_mtu.setPortType ( i, mtuType.Ports[ i ].Type );

            if ( latest_mtu.isEncoder )
            {
            }
        }

        private ErrorArgs TranslateException ( Exception e )
        {

            int    status     = -1;
            string message    = TranslateExceptionMessage(e, CultureInfo.GetCultureInfo("en-US"));
            string logmessage = TranslateExceptionMessage(e, CultureInfo.GetCultureInfo("en-US"));

            switch ( e.GetType ().Name )
            {
                case ERROR_LOADDEMANDCONF:
                case ERROR_LOADMETER:
                case ERROR_LOADMTU:
                case ERROR_LOADALARM:
                case ERROR_NOTFOUNDMTU:
                case ERROR_LOADINTERFACE:
                case ERROR_LOADGLOBAL:
                case ERROR_NOTFOUNDMETER:
                    break;
            }

            return new ErrorArgs ( status, message, logmessage );
        }

        public static string TranslateExceptionMessage(Exception E, CultureInfo targetCulture)
        {
            try
            {
                Assembly a = E.GetType().Assembly;
                ResourceManager rm = new ResourceManager(a.GetName().Name, a);
                ResourceSet rsOriginal = rm.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true);
                ResourceSet rsTranslated = rm.GetResourceSet(targetCulture, true, true);
                foreach (DictionaryEntry item in rsOriginal)
                    if (item.Value.ToString() == E.Message.ToString())
                        return rsTranslated.GetString(item.Key.ToString(), false); // success

            }
            catch { }
            return E.Message; // failed (error or cause it's not intelligent enough to locale '{0}'-patterns
        }

        public MTUBasicInfo GetBasicInfo ()
        {
            return this.latest_mtu;
        }

        #endregion
    }
}
