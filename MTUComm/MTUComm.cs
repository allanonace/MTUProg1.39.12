using Lexi.Interfaces;
using MTUComm.MemoryMap;
using System.Threading.Tasks;
using System.Threading;
using System;
using Xml;
using static MTUComm.LogQueryResult;
using System.Collections.Generic;

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

        public delegate void ReadMtuDataHandler(object sender, ReadMtuDataArgs e);
        public event ReadMtuDataHandler OnReadMtuData;

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

        public void ReadMTUdata(int NumOfDays)
        {

            Task.Factory.StartNew(() => ReadMTUDataTask(NumOfDays));
        }

        public void ReadMTUDataTask(int NumOfDays)
        {
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
                        OnReadMtuData(this, new ReadMtuDataArgs(response.Status, entries));
                        break;
                    case LogDataType.Bussy:
                        OnReadMtuData(this, new ReadMtuDataArgs(response.Status));
                        Thread.Sleep(100);
                        break;
                    case LogDataType.NewPacket:
                        entries.Add(response.Entry);
                        OnReadMtuData(this, new ReadMtuDataArgs(response.Status, response.TotalEntries, response.CurrentEntry));
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

            public List<LogDataEntry> Entries { get; private set; }

            public ReadMtuDataArgs(LogDataType status)
            {
                Status = status;
                TotalEntries = 0;
                CurrentEntry = 0;
            }

            public ReadMtuDataArgs(LogDataType status, List<LogDataEntry> entries)
            {
                Status = status;
                TotalEntries = entries.Count;
                CurrentEntry = entries.Count;
                Entries = entries;
            }

            public ReadMtuDataArgs(LogDataType status, int totalEntries, int currentEntry)
            {
                Status = status;
                TotalEntries = totalEntries;
                CurrentEntry = currentEntry;
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
    }
}
