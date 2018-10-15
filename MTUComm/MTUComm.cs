﻿using System;
using System.Threading.Tasks;
using Lexi.Interfaces;
using Xml;

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
                    args = new ReadMtuArgs(new MemoryMap31xx32xx(lexi.Read(0, 255)), mtuType);
                    break;
                case "33":
                    break;
                case "34":
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

        public void TurnOffMtu()
        {
            Console.WriteLine("TurnOffMtu start");
            byte valueToWrite = 1;
            lexi.Write(22, new byte[] { valueToWrite });
            byte valueWritten = (lexi.Read(22, 1))[0];
            Console.WriteLine("Value to write: " + valueToWrite.ToString() + " Value written: " + valueWritten.ToString());
            Console.WriteLine("TurnOffMtu end");

            if (valueToWrite != valueWritten)
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
            Console.WriteLine("TurnOnMtu start");
            byte valueToWrite = 0;
            lexi.Write(22, new byte[] { valueToWrite });
            byte valueWritten = (lexi.Read(22, 1))[0];
            Console.WriteLine("Value to write: " + valueToWrite.ToString() + " Value written: " + valueWritten.ToString());
            Console.WriteLine("TurnOnMtu end");

            if (valueToWrite != valueWritten)
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