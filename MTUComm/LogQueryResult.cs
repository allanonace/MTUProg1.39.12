using System;
using System.Collections.Generic;
using System.Text;

namespace MTUComm
{
    public class LogQueryResult
    {
        #region Constants

        public enum LogDataType
        {
            Busy,
            NewEventLog,
            LastEventLog,
        }
        
        private const int BYTE_ACKINFO = 1;
        private const int BYTE_RESULT  = 2;
        private const int BYTE_NUMLOGS = 3;
        private const int BYTE_CURRENT = 5;

        #endregion

        #region Attributes

        private LogDataType status;
        private LogDataEntry eventLogEntry;
        private int totalEntries;
        private int indexCurrentEntry;

        #endregion

        #region Properties

        public LogDataType Status
        {
            get { return status; }
        }

        public LogDataEntry EventLogEntry
        {
            get { return eventLogEntry; }
        }

        public int TotalEntries
        {
            get { return totalEntries; }
        }

        public int IndexCurrentEntry
        {
            get { return indexCurrentEntry; }
        }

        #endregion

        #region Initialization

        public LogQueryResult (
            byte[] response )
        {
            // Response – ACK with no log entry
            if ( response[ BYTE_ACKINFO ] == 1 )
            {
                this.status = ( response[ BYTE_RESULT ] == 1 ) ? LogDataType.LastEventLog : LogDataType.Busy;
            }
            // Response – ACK with log entry
            else
            {
                this.status            = LogDataType.NewEventLog;
                this.totalEntries      = response[ BYTE_NUMLOGS ] + ( response[ BYTE_NUMLOGS + 1 ] << 8 );
                this.indexCurrentEntry = response[ BYTE_CURRENT ] + ( response[ BYTE_CURRENT + 1 ] << 8 );
                this.eventLogEntry     = new LogDataEntry ( response );
            }
        }

        #endregion
    }
}
