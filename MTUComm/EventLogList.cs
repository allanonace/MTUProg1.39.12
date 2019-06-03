using System;
using System.Collections.Generic;
using Library;

namespace MTUComm
{
    public class EventLogList
    {
        #region Constants

        public enum EventLogQueryResult
        {
            NextRead,
            LastRead,
            Busy,
            Empty
        }

        private const int  BYTE_ACKINFO = 1;
        private const byte HAS_DATA     = 0x00;
        private const byte HAS_NOT_DATA = 0x01;

        #endregion

        #region Attributes

        private List<EventLog> entries;

        #endregion

        #region Properties

        public int Count
        {
            get { return this.entries.Count; }
        }

        public EventLog[] Entries
        {
            get { return this.entries.ToArray (); }
        }

        #endregion

        #region Initialization

        public EventLogList ()
        {
            this.entries = new List<EventLog> ();
        }

        #endregion

        public EventLogQueryResult TryToAdd (
            byte[] response )
        {
            switch ( response[ BYTE_ACKINFO ] )
            {
                // ACK without log entry
                case byte value when value == HAS_NOT_DATA:
                    return ( value == HAS_NOT_DATA ) ? EventLogQueryResult.Empty : EventLogQueryResult.Busy;

                // ACK with log entry
                case HAS_DATA:
                    this.entries.Add ( new EventLog ( response ) );
                    break;
            }

            return ( this.entries[ this.entries.Count - 1 ].IsLast ) ?
                EventLogQueryResult.LastRead : EventLogQueryResult.NextRead;
        }
    }
}
