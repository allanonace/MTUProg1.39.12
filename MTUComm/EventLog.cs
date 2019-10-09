using System;
using Library;

using LogEntryType = Lexi.Lexi.LogEntryType;

namespace MTUComm
{
    public class EventLog
    {
        /*
        * +------------+---------------+------------------------------------------------------+
        * | Byte Index |  Field        |                          Notes                       |
        * +------------+---------------+------------------------------------------------------+
        * | 0          | ACK           | 0x06 Operation successful                            |
        * | 1          | ACK Info Size | 0x15 ( 21 ) bytes of data if Result 0, otherwise 1   |
        * | 2          | Result        | 0 = Data included, 1 = No more data, 2 = MTU busy    |
        * | 3..4       | Num. Results  | Total number of query results                        |--+
        * | 5..6       | Current Item  | Current result number ( 1 based )                    |  |--- Result with log entry
        * | 7..22      | Data          | Data bytes fog the log item                          |--+
        * | 23..24     | CRC           | Byte 0: 0x06 ACK, 0x15 NAK                           |
        * +------------+---------------+------------------------------------------------------+
        * Data..
        * +------------+---------------+------------------------------------------------------+
        * | Byte Index |  Field        |                          Notes                       |
        * +------------+---------------+------------------------------------------------------+
        * | 7          | Entry Type    | int , LogEntryType                                   |
        * | 8          | Format Vers.  | int                                                  |
        * | 9..12      | SecsTimeStamp | long , 01/01/1970 + SecsTimeStamp = TimeStamp        |
        * | 13..14     | Flags         | int                                                  |
        * | 15..16     | Read Interval | int                                                  |
        * | 17..21     | Meter Read    | long                                                 |
        * | 22         | Error Status  | int                                                  |
        * +------------+---------------+------------------------------------------------------+
        */

        #region Constants

        public enum ReadReason
        {
            Scheduled, // Normally scheduled read
            OnDemand,  // OTA request for a read
            TaskFlag   // Bit set using the coil interface
        }

        // Maximum lengths
        public  const int BYTES_REQUIRED_DATA    = 25;

        // ACK with Remote Disconnect Response
        private const int BYTE_RESULT            = 2;
        private const int BYTE_NUMLOGS           = 3; // and 4
        private const int NUM_BYTES_NUMLOGS      = 2;
        private const int BYTE_CURRENT           = 5; // and 6
        private const int NUM_BYTES_CURRENT      = 2;
        private const int BYTE_TYPE              = 7;
        private const int BYTE_FORMAT            = 8;
        private const int BYTE_SECSTIME          = 9;
        private const int NUM_BYTES_SECSTIME     = 4;
        private const int BYTE_FLAGS             = 13;
        private const int NUM_BYTES_FLAGS        = 2;
        private const int BYTE_READINTERVAL      = 15;
        private const int NUM_BYTES_READINTERVAL = 2;
        private const int BYTE_METERREAD         = 17;
        private const int NUM_BYTES_METERREAD    = 5;
        private const int BYTE_ERROR             = 22;

        #endregion

        #region Attributes

        private LogEntryType logType;
        private int          index;
        private int          totalLogs;
        private int          formatVersion;
        private DateTime     timeStamp;
        private int          readInterval;
        private long         meterRead;
        private int          flags;
        private int          errorStatus;

        #endregion

        #region Properties

        public LogEntryType LogType
        {
            get { return this.logType; }
        }

        public int Index
        {
            get { return this.index; }
        }
        
        public int TotalEntries
        {
            get { return this.totalLogs; }
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

        public int ErrorStatus
        {
            get { return errorStatus; }
        }

        public int PortNumber
        {
            get { return ( ( this.flags & 3 ) == 0 ) ? 1 : ( ( ( this.flags & 3 ) == 1 ) ? 2 : -1 ); }
        }

        // Gets a value indicating whether the read was a daily read.
        public bool IsDailyRead
        {
            get { return ( this.flags & 4 ) != 0; }
        }

        // Gets a value indicating whether the read was a top of the hour read.
        public bool IsTopOfHourRead
        {
            get { return ( this.flags & 8 ) != 0; }
        }

        // Gets the reason for the reading.
        public ReadReason ReasonForRead
        {
            get { return ( ReadReason )( ( this.flags >> 4 ) & 7 ); }
        }

        // Gets a value indicating whether the time was synchronized when the reading was taken.
        public bool IsSynchronized
        {
            get { return ( this.flags & 0x80 ) != 0; }
        }

        #endregion

        #region Initialization

        public EventLog (
            byte[] response )
        {
            this.logType       = ( LogEntryType )response[ BYTE_TYPE ]; // 7
            this.formatVersion = ( int )response[ BYTE_FORMAT ];        // 8
            this.errorStatus   = ( int )response[ BYTE_ERROR  ];        // 22
            this.totalLogs     = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_NUMLOGS,      NUM_BYTES_NUMLOGS      ); // 3 and 4
            this.index         = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_CURRENT,      NUM_BYTES_CURRENT      ); // 5 and 6
            long secsTimeStamp = Utils.GetNumericValueFromBytes<long> ( response, BYTE_SECSTIME,     NUM_BYTES_SECSTIME     ); // 9, 10, 11 and 12
            this.flags         = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_FLAGS,        NUM_BYTES_FLAGS        ); // 13 and 14
            this.readInterval  = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_READINTERVAL, NUM_BYTES_READINTERVAL ); // 15 and 16
            this.meterRead     = Utils.GetNumericValueFromBytes<long> ( response, BYTE_METERREAD,    NUM_BYTES_METERREAD    ); // 17, 18, 19, 20 and 21
            this.timeStamp     = new DateTime ( 1970, 1, 1, 0, 0, 0 ).AddSeconds ( secsTimeStamp );
        }

        #endregion
    }
}
