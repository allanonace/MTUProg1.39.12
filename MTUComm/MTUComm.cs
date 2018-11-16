using Xml;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Generic;
using MTUComm.MemoryMap;
using Lexi.Interfaces;
using MTUComm.actions;
using FIELD = MTUComm.actions.AddMtuForm.FIELD;

namespace MTUComm
{
    public class MTUComm
    {
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

        public delegate void AddMtuHandler(object sender, AddMtuArgs e);
        public event AddMtuHandler OnAddMtu;

        public delegate void BasicReadHandler(object sender, BasicReadArgs e);
        public event BasicReadHandler OnBasicRead;

        private const int DEFAULT_OVERLAP = 6;


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

            ReadMtuArgs args = null;

            switch (mtuType.HexNum.Substring(0, 2))
            {
                case "31":
                case "32":
                    break;
                case "33":
                case "34":
                    lexi.Write(64, new byte[] { 1 });
                    Thread.Sleep(1000);
                    break;
            }

            switch (mtuType.HexNum.Substring(0, 2))
            {
                case "31":
                case "32":
                    args = new ReadMtuArgs(new MemoryMap31xx32xx(lexi.Read(0, 255)), mtuType);
                    break;
                case "33":
                    args = new ReadMtuArgs(new MemoryMap33xx(lexi.Read(0, 255)), mtuType);
                    break;
                case "34":
                    byte[] buffer = new byte[512];

                    System.Buffer.BlockCopy(lexi.Read(0, 255), 0, buffer, 0, 255);
                    System.Buffer.BlockCopy(lexi.Read(256, 65), 0, buffer, 256, 65);

                    args = new ReadMtuArgs(new MemoryMap342x(buffer), mtuType);
                    break;
            }
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
            public IMemoryMap MemoryMap { get; private set; }

            public Mtu MtuType { get; private set; }

            public ReadMtuArgs(IMemoryMap memorymap, Mtu mtype)
            {
                MemoryMap = memorymap;
                MtuType = mtype;
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
            public AddMtuArgs()
            {
            }
        }

        public class BasicReadArgs : EventArgs
        {
            public uint MtuType { get; }
            public BasicReadArgs(uint MtuType)
            {
                this.MtuType = MtuType;
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

        public void AddMtu(AddMtuForm addMtuForm) // Parameter[] addMtuParams)
        {
            Task.Factory.StartNew(() => AddMtuTask(addMtuForm));
        }

        private void AddMtuTask(dynamic form) // Parameter[] addMtuParams)
        {
            /*List<Parameter> p = new List<Parameter>(addMtuParams);

            Dictionary<FIELD, string[]> Texts = AddMtuForm.Texts;
            Parameter servicePortIdParam = p.FindByParamId(FIELD.SERVICE_PORT_ID, Texts);
            Parameter fieldOrderParam = p.FindByParamId(FIELD.FIELD_ORDER, Texts);
            Parameter meterNumberParam = p.FindByParamId(FIELD.METER_NUMBER, Texts);
            Parameter selectedMeterIdParam = p.FindByParamId(FIELD.SELECTED_METER_ID, Texts);
            Parameter readIntervalParam = p.FindByParamId(FIELD.READ_INTERVAL, Texts);
            Parameter snapReadsParam = p.FindByParamId(FIELD.SNAP_READS, Texts);
            Parameter twoWayParam = p.FindByParamId(FIELD.TWO_WAY, Texts);
            Parameter alarmsParam = p.FindByParamId(FIELD.ALARM, Texts);*/

            byte[] memory = new byte[400];
            dynamic map = new MemoryMap31xx32xx(memory); // TODO: identify map by mtu type
            // meter type
            map.P1MeterType = form.MeterNumber.getValue();
            // P2MeterType;

            // service port id, account number
            map.P1MeterId = form.ServicePortId.getValue();
            // P2MeterId

            // reading interval
            string[] readIntervalArray = form.ReadInterval.getValue().Split(' ');
            string readIntervalStr = readIntervalArray[0];
            string timeUnit = readIntervalArray[1];
            int timeIntervalMins = Int32.Parse(readIntervalStr);
            if (timeUnit is "Hours")
            {
                timeIntervalMins = timeIntervalMins * 60;
            }

            map.ReadInterval = timeIntervalMins; // minutes
            // P2ReadInterval

            // overlap
            // MessageOverlapCount
            map.MessageOverlapCount = 5;// DEFAULT_OVERLAP; // TODO: parse Alarm object and get Overlap
            // P2MessageOverlapCount

            // initial reading
            // P1Reading
            map.P1Reading = 0;
            // P2Reading

            // alarms // TODO: get alarms from Alarm object
            Alarm alarms = (Alarm)form.Alarm.getValue();
            // P1ImmediateAlarm
            // P1UrgentAlarm
            // P1MagneticAlarm
            // P1RegisterCoverAlarm
            // P1ReverseFlowAlarm
            // P1TiltAlarm
            // P1InterfaceAlarm
            // P2ImmediateAlarm
            // P2UrgentAlarm
            // P2MagneticAlarm
            // P2RegisterCoverAlarm
            // P2ReverseFlowAlarm
            // P2TiltAlarm
            // P2InterfaceAlarm

            // encryption key
            // EncryptionKey
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] aesKey = new byte[16];
            rng.GetBytes(aesKey);
            map.EncryptionKey = aesKey;
            // Encrypted
            // EncryptionIndex

            string testAES = map.EncryptionKey;

            // fast message (not in pulse)
            // encoder digits to drop (not in pulse)

            /*
            //Console.WriteLine("AddMtu start");
            //byte[] data = lexi.Read(0, 255);
            //int mtuType = data[0];
            //int meterTypeId = (int)(data[32] + (data[33] << 8));
            //Console.WriteLine("MTU Type: " + mtuType);
            //Console.WriteLine("Meter Type ID: " + meterTypeId);

            //byte[] bytesToWrite = BitConverter.GetBytes(3101);
            //int meterTypeId2 = (int)(bytesToWrite[0] + (bytesToWrite[1] << 8));
            //Console.WriteLine("Meter Type ID 2: " + meterTypeId2);
            //lexi.Write(32, bytesToWrite);
            //Console.WriteLine("AddMtu end");

            byte[] memory2 = new byte[300]; // TODO: read real memory map?
            dynamic map2 = new MemoryMap31xx32xx(memory2); // TODO: identify mm by mtu type
            // 22 system flags

            // 25 message overlap count - alarm.xml
            map2.MessageOverlapCount = 6; // TODO: take value from alarm.xml
            //writeParam(25, memory, 1);

            // 26-27 read interval
            map2.ReadInterval = 11; // TODO: get real value
            //writeParam(26, memory, 2);

            // 28 ports enable

            // 32-33 p1 meter type
            map2.P1MeterType = 3101; // TODO: real value
            //writeParam(32, memory, 2);

            // 34-39 p1 meter id - Global.xml
            string p1MeterId = "9876543210";
            string p1MeterIdToWrite = "";
            if (p1MeterId.Trim().Length > 12)
            {
                p1MeterIdToWrite = p1MeterId.Substring(0, 12);
            }
            else
            {
                p1MeterIdToWrite = p1MeterId.PadLeft(12, 'F');
            }
            map2.P1MeterId = ulong.Parse(p1MeterIdToWrite, System.Globalization.NumberStyles.HexNumber);
            // 40-41 p1 pulse ratio
            //mm.P1PulseRatio = 10;
            // 42 p1 mode - alarm.xml
            //mm.P1Mode = 255; // TODO: get AlarmMask1 value from alarm.xml
            // 48-49 p2 info, p2 meter type
            //mm.P2MeterType = 2222; // TODO: real value
            // 50-55 p2 meter id - Global.xml
            string p2MeterId = "9876543210";
            string p2MeterIdToWrite = "";
            if (p2MeterId.Trim().Length > 12)
            {
                p2MeterIdToWrite = p2MeterId.Substring(0, 12);
            }
            else
            {
                p2MeterIdToWrite = p2MeterId.PadLeft(12, 'F');
            }
            //mm.P2MeterId = ulong.Parse(p2MeterIdToWrite, System.Globalization.NumberStyles.HexNumber);
            // 56-57 p2 pulse ratio
            //mm.P2PulseRatio = 20;
            // 58 p2 mode - alarm.xml
            //mm.P2Mode = 255; // TODO: get AlarmMask2 value from alarm.xml
            // 64 task flags - when reading meter
            // 65 task flags - when time sync request
            // 92-95 MTU/DCU ID of last packet received
            // 96-101 p1 reading
            //mm.P1Reading = 305;
            //byte[] newP1Reading = new byte[6];
            //Array.Copy(memory, 96, newP1Reading, 0, 6);
            //lexi.Write(96, newP1Reading);
            //writeParam(96, memory, 6);
            // 104-109 p2 reading
            //mm.P2Reading = 2002; 
            // 198 daily read global.xml
            map2.DailyRead = 17;
            // 256-271 AES encryption key
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] aesKey = new byte[16];
            rng.GetBytes(aesKey);
            //mm.AesEncryptionKey = aesKey;
            //writeParam(256, memory, 16); // ble layer does not support 16 byte writes
            //writeParam(256, memory, 6); // ble layer does not support 16 byte writes
            //writeParam(262, memory, 6); // ble layer does not support 16 byte writes
            //writeParam(268, memory, 4);
            */

            WriteModifiedRegisters(map);
            AddMtuArgs args = new AddMtuArgs();
            OnAddMtu(this, args);
        }

        public void NewAddMtu(Parameter[] p)
        {

        }

        public void WriteModifiedRegisters(MemoryMap.MemoryMap map)
        {
            var modifiedRegisters = map.GetModifiedRegisters();
            List<MemoryRegister<int>> modifiedIntRegisters = modifiedRegisters.GetElements_Int();
            foreach (var r in modifiedIntRegisters)
            {
                writeParam((uint)r.address, map.memory, (uint)r.size);
            }
            List<MemoryRegister<uint>> modifiedUIntRegisters = modifiedRegisters.GetElements_UInt();
            foreach (var r in modifiedUIntRegisters)
            {
                writeParam((uint)r.address, map.memory, (uint)r.size);
            }
            List<MemoryRegister<ulong>> modifiedULongRegisters = modifiedRegisters.GetElements_ULong();
            foreach (var r in modifiedULongRegisters)
            {
                writeParam((uint)r.address, map.memory, (uint)r.size);
            }
            List<MemoryRegister<bool>> modifiedBoolRegisters = modifiedRegisters.GetElements_Bool();
            foreach (var r in modifiedBoolRegisters)
            {
                writeParam((uint)r.address, map.memory, (uint)r.size);
            }
            List<MemoryRegister<char>> modifiedCharRegisters = modifiedRegisters.GetElements_Char();
            foreach (var r in modifiedCharRegisters)
            {
                writeParam((uint)r.address, map.memory, (uint)r.size);
            }
            List<MemoryRegister<string>> modifiedStringRegisters = modifiedRegisters.GetElements_String();
            foreach (var r in modifiedStringRegisters)
            {
                writeParam((uint)r.address, map.memory, (uint)r.size);
            }
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
            BasicReadArgs args = new BasicReadArgs(latest_mtu.Type);
            OnBasicRead(this, args);
        }
    }
}
