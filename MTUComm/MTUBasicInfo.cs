using System;
using System.Collections.Generic;
using System.Text;

namespace MTUComm
{
    public class MTUBasicInfo
    {
        #region Attributes

        private uint mtu_type;
        private uint mtu_id;

        private Boolean mEncoder = true;

        private int shipbit;

        private int p1enabled;
        private int p2enabled;
        
        public  int version { get; }

        #endregion

        public enum Model
        {
            A,
            C,
            D,
            D2,
            F,
            Z,
            MET
        }

        public MTUBasicInfo(byte[] buffer)
        {
            mtu_type = buffer[0];

            byte[] id_stream = new byte[4];
            Array.Copy ( buffer, 6, id_stream, 0, 4 );

            mtu_id = BitConverter.ToUInt32(id_stream, 0);

            byte mask = 1;
            shipbit = buffer[22];
            shipbit &= mask;

            p1enabled = buffer[28];
            p1enabled &= 1;

            p2enabled = buffer[28];
            p2enabled &= 2;
            
            // If new soft version is equal to 255, use old soft version register
            this.version = ( buffer[ 32 ] == 255 ) ? buffer[ 1 ] : buffer[ 32 ];
        }

        public bool P1Enabled
        {
            get
            {
                return (p1enabled > 0);
            }
        }

        public bool P2Enabled
        {
            get
            {
                return (p2enabled > 0);
            }
        }

        public bool Shipbit
        {
            get
            {
                return (shipbit > 0) ? true : false;
            }
        }

        public uint Type
        {
            get
            {
                return mtu_type;
            }
        }

        public uint Id
        {
            get
            {
                return mtu_id;
            }
        }

        public Boolean isEncoder
        {
            get { return mEncoder; }
        }
    }
}
