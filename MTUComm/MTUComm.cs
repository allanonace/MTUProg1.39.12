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

        public delegate void AddMtuHandler(object sender, AddMtuArgs e);
        public event AddMtuHandler OnAddMtu;

        public delegate void BasicReadHandler(object sender, BasicReadArgs e);
        public event BasicReadHandler OnBasicRead;

        
        public MTUComm(ISerial serial, Configuration configuration)
        {
            this.configuration = configuration;
            latest_mtu = new MTUBasicInfo(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            lexi = new Lexi.Lexi(serial, 10000);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Internaly saves internal MTU type and ID used for communication logic</remarks>
        private void getMTUBasicInfo()
        {
            MTUBasicInfo mtu_info = new MTUBasicInfo(lexi.Read(0, 10));
            mtu_changed = !((mtu_info.Id == latest_mtu.Id) && (mtu_info.Type == latest_mtu.Type));
            latest_mtu = mtu_info;
            MtuForm.SetBasicInfo(latest_mtu);
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


        public void ReadMTU(){

            Task.Factory.StartNew(ReadMTUTask);
        }

        public void ReadMTUTask()
        {
            //Gets MTU casci info (type and id)
            getMTUBasicInfo();

            //If MTU has changed or critical settings/configuration force detection rutine
            if (this.changedMTUSettings)
            {
                DetectMeters();
            }


            String memory_map_type = configuration.GetMemoryMapTyeByMtuId(mtuType.Id);

            lexi.Write(64, new byte[] { 1 });
            Thread.Sleep(1000);

            byte[] buffer = new byte[512];

            System.Buffer.BlockCopy(lexi.Read(0, 255), 0, buffer, 0, 255);


            try
            {
                System.Buffer.BlockCopy(lexi.Read(256, 64), 0, buffer, 256, 64);
                System.Buffer.BlockCopy(lexi.Read(318, 2), 0, buffer, 318, 2);
            }
            catch (Exception e) { }

            ReadMtuArgs args = new ReadMtuArgs(new MemoryMap.MemoryMap(buffer, memory_map_type), mtuType);

            OnReadMtu(this, args);
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
            public AddMtuForm form;

            public AddMtuArgs(AddMtuForm form)
            {
                this.form = form;
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
            Console.WriteLine("TurnOffMtu end");

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

        public void AddMtu(AddMtuForm addMtuForm)
        {
            Task.Factory.StartNew(() => AddMtuTask(addMtuForm));
        }

        private void AddMtuTask(dynamic form)
        {
            Mtu mtu = form.mtu;
            // Prepare memory map
            byte[] memory = new byte[400];
            dynamic map = new MemoryMap.MemoryMap ( memory, "31xx32xx" ); // TODO: identify map by mtu type

            // meter type
            map.P1MeterType = form.MeterNumber.getValue();
            // P2MeterType

            // service port id, account number
            map.P1MeterId = form.ServicePortId.getValue();
            // P2MeterId

            // reading interval
            /*string[] readIntervalArray = form.ReadInterval.getValue().Split(' ');
            string readIntervalStr = readIntervalArray[0];
            string timeUnit = readIntervalArray[1];
            int timeIntervalMins = Int32.Parse(readIntervalStr);
            if (timeUnit is "Hours")
                timeIntervalMins = timeIntervalMins * 60;

            map.ReadInterval = timeIntervalMins; // In minutes*/

            map.ReadInterval = form.ReadInterval.getValue();

            // P2ReadInterval

            // overlap
            map.MessageOverlapCount = DEFAULT_OVERLAP;
            // P2MessageOverlapCount

            // initial reading
            map.P1Reading = 0;
            // P2Reading

            // alarms
            if (form.GetCondition(AddMtuForm.FIELD_CONDITIONS.MTU_REQUIRES_ALARM_PROFILE))
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
                // P2ImmediateAlarm
                // P2UrgentAlarm
                // P2MagneticAlarm
                // P2RegisterCoverAlarm
                // P2ReverseFlowAlarm
                // P2TiltAlarm
                // P2InterfaceAlarm
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

            string testAES = map.EncryptionKey;

            // fast message (not in pulse)
            // encoder digits to drop (not in pulse)

            // Write changes into MTU
            WriteModifiedRegisters(map);

            AddMtuArgs args = new AddMtuArgs (form);
            OnAddMtu ( this, args );
        }

        public void NewAddMtu(Parameter[] p)
        {

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
