using Lexi.Interfaces;
using MTUComm.actions;
using MTUComm.MemoryMap;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Xml;
using static MTUComm.LogQueryResult;
using System.Collections.Generic;

namespace MTUComm
{
    public class MTUComm
    {
        private const int DEFAULT_OVERLAP = 6;
        private const int DEFAULT_LENGTH_AES = 16;

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        private Lexi.Lexi lexi;

        private MTUBasicInfo latest_mtu;

        private Boolean mtu_changed;

        private Configuration configuration;

        private Boolean isPulse = false;

        private Mtu mtuType;

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


        public MTUComm(ISerial serial, Configuration configuration)
        {
            this.configuration = configuration;
            latest_mtu = new MTUBasicInfo(new byte[25]);
            lexi = new Lexi.Lexi(serial, 10000);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Internaly saves internal MTU type and ID used for communication logic</remarks>
        private void getMTUBasicInfo()
        {
            try
            {
                MTUBasicInfo mtu_info = new MTUBasicInfo(lexi.Read(0, 25));
                mtu_changed = !((mtu_info.Id == latest_mtu.Id) && (mtu_info.Type == latest_mtu.Type));
                latest_mtu = mtu_info;
                MtuForm.SetBasicInfo(latest_mtu);
            }
            catch (Exception e)
            {
                OnError(this, new ErrorArgs(510, e.Message, "getMTU info returned 0 bytes."));
            }

        }

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

        public void ReadMTUdata(int NumOfDays)
        {

            Task.Factory.StartNew(() => ReadMTUDataTask(NumOfDays));
        }

        public void ReadMTUDataTask(int NumOfDays)
        {
            getMTUBasicInfo();

            //If MTU has changed or critical settings/configuration force detection rutine
            if (this.changedMTUSettings)
            {
                DetectMeters();
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


        public void InstallConfirmation()
        {

            Task.Factory.StartNew(InstallConfirmationTask);
        }

        public void InstallConfirmationTask()
        {
            getMTUBasicInfo();

            //If MTU has changed or critical settings/configuration force detection rutine
            if (this.changedMTUSettings)
            {
                DetectMeters();
            }

            if (latest_mtu.Shipbit)
            {
                string message = "Current MTU ID: " + latest_mtu.Id + " type " + latest_mtu.Type + " is OFF";
                OnError(this, new ErrorArgs(112, message, "Installation Confirmation Cancellation - "+ message));
            }
            else
            {
                String memory_map_type = configuration.GetMemoryMapTypeByMtuId(mtuType.Id);
                MemRegister register = configuration.getFamilyRegister(memory_map_type, "InstallationConfirmationRequest");
                
                Console.WriteLine("InstallConfirmation trigger start");

                byte mask = 2;
                uint inConFlag = 116;

                if(register != null)
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
                Console.WriteLine("Value to write: " + systemFlags.ToString() + " Value written: " + valueWritten.ToString());
                Console.WriteLine("InstallConfirmation trigger end");

                valueWritten &= mask;
                int max_iters = 3;

                while (valueWritten > 0 && max_iters > 0)
                {
                    OnProgress(this, new ProgressArgs(4 - max_iters, 3, "Install Confirmation("+ max_iters.ToString() + ")..."));
                    Thread.Sleep(60000);
                    valueWritten = (lexi.Read(inConFlag, 1))[0];
                    valueWritten &= mask;
                    max_iters--;
                }
            }
            ReadMTUTask(true);
        }


        public void ReadMTU(){

            Task.Factory.StartNew(ReadMTUTask);
        }

        public void ReadMTUTask()
        {
            ReadMTUTask(false);
        }


        public void ReadMTUTask(bool forcedetect)
        {
            //Gets MTU casci info (type and id)
            getMTUBasicInfo();

            //If MTU has changed or critical settings/configuration force detection rutine
            if (this.changedMTUSettings || forcedetect)
            {
                DetectMeters();
            }


            String memory_map_type = configuration.GetMemoryMapTypeByMtuId(mtuType.Id);
            int memory_map_size = configuration.GetmemoryMapSizeByMtuId(mtuType.Id);

            lexi.Write(64, new byte[] { 1 });
            Thread.Sleep(1000);

            byte[] buffer = new byte[1024];

            System.Buffer.BlockCopy(lexi.Read(0, 255), 0, buffer, 0, 255);

            try
            {
                if(memory_map_size > 255)
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

                
            }
            catch (Exception e) {
                
            }

            try
            {
                ReadMtuArgs args = new ReadMtuArgs(new MemoryMap.MemoryMap(buffer, memory_map_type), mtuType);
                OnReadMtu(this, args);
            }
            catch (Exception e)
            {
                OnError(this, TranslateException(e));
            }
          
        }


        private ErrorArgs TranslateException(Exception e)
        {
            int status = -1;
            string message = e.Message;
            string logmessage = e.Message;
            Type ex_type = e.GetType();
            switch (ex_type.Name)
            {
                case "DemandConfLoadException":
                    break;
                case "MeterLoadException":
                    break;
                case "MtuLoadException":
                    break;
                case "AlarmLoadException":
                    break;
                case "MtuNotFoundException":
                    break;
                case "InterfaceLoadException":
                    break;
                case "GlobalLoadException":
                    break;
                case "MeterNotFoundException":
                    break;
            }
            return new ErrorArgs(status, message, logmessage);
        }

        private void DetectMeters()
        {
            mtuType = configuration.GetMtuTypeById((int)latest_mtu.Type);

            for (int i = 0; i < mtuType.Ports.Count; i++)
            {
                latest_mtu.setPortType(i, mtuType.Ports[i].Type);
            }

            if (latest_mtu.isEncoder)
            {

            }

        }

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

            public AddMtuArgs(AMemoryMap memorymap, Mtu mtype)
            {
                MemoryMap = memorymap;
                MtuType = mtype;
            }
        }

        public class BasicReadArgs : EventArgs
        {
            public BasicReadArgs()
            {
            }
        }

        public void TurnOffMtu()
        {
            Task.Factory.StartNew(TurnOffMtuTask);
        }

        public void TurnOffMtuTask()
        {
            Console.WriteLine("TurnOffMtu start");

            byte mask = 1;
            byte systemFlags = (lexi.Read(22, 1))[0];
            systemFlags |= mask; // set bit 0
            lexi.Write(22, new byte[] { systemFlags });
            byte valueWritten = (lexi.Read(22, 1))[0];
            Console.WriteLine("Value to write: " + systemFlags.ToString() + " Value written: " + valueWritten.ToString());
            Console.WriteLine("TurnOnMtu end");

            if (systemFlags != valueWritten)
            {
                // TODO: handle error condition
            }

            getMTUBasicInfo();
            Console.WriteLine("MTU ID: " + latest_mtu.Id);

            TurnOffMtuArgs args = new TurnOffMtuArgs(latest_mtu.Id);
            OnTurnOffMtu(this, args);
        }
   
        public void TurnOnMtu()
        {
            Task.Factory.StartNew(TurnOnMtuTask);
        }

        public void TurnOnMtuTask()
        {
            Console.WriteLine("TurnOnMtu start");

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

            getMTUBasicInfo();
            Console.WriteLine("MTU ID: " + latest_mtu.Id);

            TurnOnMtuArgs args = new TurnOnMtuArgs(latest_mtu.Id);
            OnTurnOnMtu(this, args);
        }

        public void AddMtu(AddMtuForm addMtuForm, string user)
        {
            Task.Factory.StartNew(() => AddMtuTask(addMtuForm, user));
        }

        private void AddMtuTask(dynamic form, string user)
        {
            Logger logger = new Logger(Configuration.GetInstance());
            AddMtuLog addMtuLog = new AddMtuLog(logger, form, user);

            #region Turn Off MTU

            byte mask = 1;
            byte systemFlags = (lexi.Read(22, 1))[0];
            systemFlags |= mask; // set bit 0
            lexi.Write(22, new byte[] { systemFlags });
            byte valueWritten = (lexi.Read(22, 1))[0];

            addMtuLog.LogTurnOff();

            #endregion

            #region Add MTU

            Mtu mtu = form.mtu;
            dynamic MtuConditions = form.conditions.mtu;
            dynamic GlobalsConditions = form.conditions.globals;

            // Prepare memory map
            String memory_map_type = configuration.GetMemoryMapTypeByMtuId((int)MtuForm.mtuBasicInfo.Type);
            int memory_map_size = configuration.GetmemoryMapSizeByMtuId((int)MtuForm.mtuBasicInfo.Type);

            byte[] memory = new byte[memory_map_size];
            dynamic map = new MemoryMap.MemoryMap ( memory, memory_map_type);

            // meter type
            Meter selectedMeter = (Meter)form.Meter.getValue();
            map.P1MeterType = selectedMeter.Id;
            if (MtuConditions.TwoPorts)
            {
                Meter selectedMeter2 = (Meter)form.Meter2.getValue();
                map.P2MeterType = selectedMeter2.Id;
            }

            // service port id, account number
            map.P1MeterId = form.ServicePortId.getValue();
            if (MtuConditions.TwoPorts)
            {
                map.P2MeterId = form.ServicePortId2.getValue();
            }

            // reading interval
            if (GlobalsConditions.IndividualReadInterval)
            {
                map.ReadIntervalMinutes = form.ReadInterval.getValue();
                if (MtuConditions.TwoPorts)
                {
                    map.P2ReadInterval = form.ReadInterval2.getValue();
                }
            }

            // overlap
            map.MessageOverlapCount = DEFAULT_OVERLAP;
            if (MtuConditions.TwoPorts)
            {
                map.P2MessageOverlapCount = DEFAULT_OVERLAP;
            }

            // initial reading
            map.P1Reading = form.InitialReading.getValue();
            if (MtuConditions.TwoPorts)
            {
                map.P2Reading = form.InitialReading2.getValue();
            }

            // alarms
            if (MtuConditions.RequiresAlarmProfile)
            {
                Alarm alarms = (Alarm)form.Alarm.getValue();

                // Overlap
                map.MessageOverlapCount = alarms.Overlap;

                // P1ImmediateAlarm
                if (alarms.ImmediateAlarmTransmit)
                {
                    map.P1ImmediateAlarm = true;
                }

                // P1UrgentAlarm
                if (alarms.DcuUrgentAlarm)
                {
                    map.P1UrgentAlarm = true;
                }

                // P1MagneticAlarm
                if (mtu.MagneticTamper)
                {
                    map.P1MagneticAlarm = alarms.Magnetic;
                }

                // P1RegisterCoverAlarm
                if (mtu.RegisterCoverTamper)
                {
                    map.P1RegisterCoverAlarm = alarms.RegisterCover;
                }

                // P1ReverseFlowAlarm
                if (mtu.ReverseFlowTamper)
                {
                    map.P1ReverseFlowAlarm = alarms.ReverseFlow;
                }

                // P1TiltAlarm
                if (mtu.TiltTamper)
                {
                    map.P1TiltAlarm = alarms.Tilt;
                }

                // P1InterfaceAlarm
                if (mtu.InterfaceTamper)
                {
                    map.P1InterfaceAlarm = alarms.InterfaceTamper;
                }

                if (MtuConditions.TwoPorts)
                {
                    // Overlap
                    map.P2MessageOverlapCount = alarms.Overlap;

                    // P2ImmediateAlarm
                    if (alarms.ImmediateAlarmTransmit)
                    {
                        map.P2ImmediateAlarm = true;
                    }

                    // P2UrgentAlarm
                    if (alarms.DcuUrgentAlarm)
                    {
                        map.P2UrgentAlarm = true;
                    }

                    // P2MagneticAlarm
                    if (mtu.MagneticTamper)
                    {
                        map.P2MagneticAlarm = alarms.Magnetic;
                    }

                    // P2RegisterCoverAlarm
                    if (mtu.RegisterCoverTamper)
                    {
                        map.P2RegisterCoverAlarm = alarms.RegisterCover;
                    }

                    // P2ReverseFlowAlarm
                    if (mtu.ReverseFlowTamper)
                    {
                        map.P2ReverseFlowAlarm = alarms.ReverseFlow;
                    }

                    // P2TiltAlarm
                    if (mtu.TiltTamper)
                    {
                        map.P2TiltAlarm = alarms.Tilt;
                    }

                    // P2InterfaceAlarm
                    if (mtu.InterfaceTamper)
                    {
                        map.P2InterfaceAlarm = alarms.InterfaceTamper;
                    }
                }
            }

            // Encryption key
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] aesKey = new byte[ DEFAULT_LENGTH_AES ];
            rng.GetBytes ( aesKey );
            map.EncryptionKey = aesKey;
            for (int i = 0; i < 15; i++)
            {
                if (aesKey[i] != memory[256+i])
                {
                    throw new Exception("AES key does not match");
                }
            }
            // Encrypted
            // EncryptionIndex

            // fast message (not in pulse)
            // encoder digits to drop (not in pulse)

            // Write changes into MTU
            WriteModifiedRegisters(map);

            addMtuLog.LogAction();

            #endregion

            #region Turn On MTU

            mask = 1;
            systemFlags = (lexi.Read(22, 1))[0];
            systemFlags &= (byte)~mask; // clear bit 0
            lexi.Write(22, new byte[] { systemFlags });
            valueWritten = (lexi.Read(22, 1))[0];

            addMtuLog.LogTurnOn();

            #endregion

            #region Read MTU

            lexi.Write(64, new byte[] { 1 });
            Thread.Sleep(1000);

            byte[] buffer = new byte[1024];

            System.Buffer.BlockCopy(lexi.Read(0, 255), 0, buffer, 0, 255);

            try
            {
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


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            MemoryMap.MemoryMap readMemoryMap = new MemoryMap.MemoryMap(buffer, memory_map_type);
            
            try
            {
                AddMtuArgs addMtuArgs = new AddMtuArgs(readMemoryMap, mtu);
                ActionResult result = OnAddMtu(this, addMtuArgs);
                addMtuLog.LogReadMtu(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                OnError(this, TranslateException(e));
            }

            #endregion

            addMtuLog.Save();
        }

        public void WriteModifiedRegisters(MemoryMap.MemoryMap map)
        {
            List<dynamic> modifiedRegisters = map.GetModifiedRegisters ().GetAllElements ();
            foreach ( dynamic r in modifiedRegisters )
                this.writeParam((uint)r.address, map.memory, (uint)r.size);
        }

        public void writeParam(uint addr, byte[] memory, uint length)
        {
            byte[] tmp = new byte[length];
            Array.Copy(memory, addr, tmp, 0, length);
            Console.WriteLine("Addr {0} | Value {1} | Length {2}", addr, BitConverter.ToString(tmp), length);
            lexi.Write(addr, tmp);
        }

        public byte[] readComplete(byte addr, uint length)
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

        public void BasicRead()
        {
            getMTUBasicInfo();
            BasicReadArgs args = new BasicReadArgs();
            OnBasicRead(this, args);
        }
    }
}
