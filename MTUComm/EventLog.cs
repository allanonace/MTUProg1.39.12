﻿using System;
using System.Collections.Generic;
using System.Text;
using Library;

namespace MTUComm
{
    public class EventLog
    {
        #region Constants

        public enum ReadReason
        {
            Scheduled, // Normally scheduled read
            OnDemand,  // OTA request for a read
            TaskFlag   // Bit set using the coil interface
        }

        private const int BYTE_RESULT            = 2;
        private const int BYTE_NUMLOGS           = 3;
        private const int BYTE_CURRENT           = 5;
        private const int BYTE_FORMAT            = 8;
        private const int BYTE_SECSTIME          = 9;
        private const int BYTE_FLAGS             = 13;
        private const int BYTE_READINTERVAL      = 15;
        private const int BYTE_METERREAD         = 17;
        private const int BYTE_ERROR             = 22;
        private const int NUM_BYTES_NUMLOGS      = 2;
        private const int NUM_BYTES_CURRENT      = 2;
        private const int NUM_BYTES_SECSTIME     = 4;
        private const int NUM_BYTES_FLAGS        = 2;
        private const int NUM_BYTES_READINTERVAL = 2;
        private const int NUM_BYTES_METERREAD    = 5;

        #endregion

        #region Attributes

        private int index;
        private int totalLogs;
        private int formatVersion;
        private DateTime timeStamp;
        private int readInterval;
        private long meterRead;
        private int flags;
        private int errorStatus;
        private int portNumber;

        #endregion

        #region Properties

        public int Index
        {
            get { return this.index; }
        }

        public bool IsLast
        {
            get { return this.index >= this.totalLogs; }
        }

        public int FormatVersion
        {
            get { return formatVersion; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        public int ReadInterval
        {
            get { return readInterval; }
        }

        public long MeterRead
        {
            get { return meterRead; }
        }

        public int Flags
        {
            get { return flags; }
        }

        public int ErrorStatus
        {
            get { return errorStatus; }
        }

        public int PortNumber
        {
            get { return this.portNumber; }
        }

        /// <summary>
        /// Gets a value indicating whether the read was a daily read.
        /// </summary>
        public bool IsDailyRead
        {
            get { return ( this.flags & 4 ) != 0; }
        }

        /// <summary>
        /// Gets a value indicating whether the read was a top of the hour read.
        /// </summary>
        public bool IsTopOfHourRead
        {
            get { return ( this.flags & 8 ) != 0; }
        }

        /// <summary>
        /// Gets the reason for the reading.
        /// </summary>
        public ReadReason ReasonForRead
        {
            get { return ( ReadReason )( ( this.flags >> 4 ) & 7 ); }
        }

        /// <summary>
        /// Gets a value indicating whether the time was synchronized when the reading was taken.
        /// </summary>
        public bool IsSynchronized
        {
            get { return ( this.flags & 0x80 ) != 0; }
        }

        #endregion

        #region Initialization

        public EventLog (
            byte[] response )
        {
            this.formatVersion = ( int )response[ BYTE_FORMAT ];
            this.errorStatus   = ( int )response[ BYTE_ERROR  ];
            this.totalLogs     = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_NUMLOGS,      NUM_BYTES_NUMLOGS      ); // 3 and 4
            this.index         = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_CURRENT,      NUM_BYTES_CURRENT      ); // 5 and 6
            long secTimeStamp  = Utils.GetNumericValueFromBytes<long> ( response, BYTE_SECSTIME,     NUM_BYTES_SECSTIME     ); // 9, 10, 11 and 12
            this.flags         = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_FLAGS,        NUM_BYTES_FLAGS        ); // 13 and 14
            this.readInterval  = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_READINTERVAL, NUM_BYTES_READINTERVAL ); // 15 and 16
            this.meterRead     = Utils.GetNumericValueFromBytes<long> ( response, BYTE_METERREAD,    NUM_BYTES_METERREAD    ); // 17, 18, 19, 20 and 21
            this.timeStamp     = new DateTime ( 1970, 1, 1, 0, 0, 0 ).AddSeconds ( secTimeStamp );
            this.portNumber    = ( ( this.flags & 3 ) == 0 ) ? 1 : ( ( ( this.flags & 3 ) == 1 ) ? 2 : -1 );
        }

        #endregion
    }
}