using System;
using System.Collections.Generic;
using Library;

using LogFilterMode = Lexi.Lexi.LogFilterMode;
using LogEntryType  = Lexi.Lexi.LogEntryType;

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

        private const int  BYTE_RESULT  = 2;
        private const byte HAS_DATA     = 0x00;
        private const byte HAS_NOT_DATA = 0x01;

        #endregion

        #region Attributes

        private List<EventLog> entries;
        private DateTime       dateStart;
        private DateTime       dateEnd;
        private LogFilterMode  filterMode;
        private LogEntryType   entryType;
        private int            acumDays     = 0;
        private uint           lastDayIndex = 0;
        private DateTime       lastDay      = new DateTime ( 1, 1, 1 );

        #endregion

        #region Properties

        public int TotalDifDays
        {
            get { return this.acumDays; }
        }

        public DateTime DateStart
        {
            get { return this.dateStart; }
        }

        public DateTime DateEnd
        {
            get { return this.dateEnd; }
        }

        public LogFilterMode FilterMode
        {
            get { return this.filterMode; }
        }

        public LogEntryType EntryType
        {
            get { return this.entryType; }
        }

        public int Count
        {
            get { return this.entries.Count; }
        }

        public EventLog[] Entries
        {
            get { return this.entries.ToArray (); }
        }

        public uint TotalEntries
        {
            get
            {
                if ( this.entries.Count > 0 )
                    return this.entries[ 0 ].TotalEntries;
                return 0;
            }
        }

        public EventLog LastEvent
        {
            get
            {
                if ( this.entries.Count > 0 )
                    return this.entries[ this.entries.Count - 1 ];
                return null;
            }
        }

        #endregion

        #region Initialization

        public EventLogList (
            DateTime start,
            DateTime end,
            LogFilterMode filterMode,
            LogEntryType entryType )
        {
            this.entries    = new List<EventLog> ();
            this.dateStart  = start;
            this.dateEnd    = end;
            this.filterMode = filterMode;
            this.entryType  = entryType;
        }

        #endregion

        public ( EventLogQueryResult Result, uint Index ) TryToAdd (
            byte[] response,
            ref bool ok )
        {
            EventLog evnt = null;
        
            switch ( response[ BYTE_RESULT ] )
            {
                // ACK without log entry
                case byte val when val != 0x00:
                    return ( ( val == HAS_NOT_DATA ) ? EventLogQueryResult.Empty : EventLogQueryResult.Busy, 0 );

                // ACK with log entry
                case HAS_DATA:
                    // NOTE: It happened once LExI returned an array of bytes without the required amount of data
                    if ( ! ( ok = ( response.Length == EventLog.BYTES_REQUIRED_DATA ) ) )
                        return ( EventLogQueryResult.Empty, 0 );

                    evnt = new EventLog ( response );

                    // Repeating entry
                    if ( this.entries.Count >= evnt.Index )
                    {
                        Utils.Print ( "Node Discovery: Processing already retrieved response/node" );

                        // Clear previous responses
                        this.entries.RemoveRange ( ( int )evnt.Index - 1, this.entries.Count - ( ( int )evnt.Index - 1 ) );
                        this.entries[ ( int )evnt.Index - 1 ] = evnt;
                    }
                    // New entry
                    else this.entries.Add ( evnt );
                    break;
            }

            DateTime dateSimplified = new DateTime (
                evnt.TimeStamp.Year,
                evnt.TimeStamp.Month,
                evnt.TimeStamp.Day );

            // Accumulate different days
            if ( evnt.Index     > lastDayIndex &&
                 dateSimplified > lastDay )
            {
                lastDayIndex = evnt.Index;
                lastDay      = dateSimplified;
                acumDays++;
            }

            return ( ( this.entries[ this.entries.Count - 1 ].IsLast ) ?
                EventLogQueryResult.LastRead : EventLogQueryResult.NextRead, evnt.Index );
        }
    }
}
