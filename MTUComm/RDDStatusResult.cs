using System;
using Library;

namespace MTUComm
{
    /// <summary>
    /// Result of the LExI command Request RDD Status during the <see cref="MTUComm.RemoteDisconnect"/> process.
    /// </summary>
    public class RDDStatusResult
    {
        /*
        * +------------+---------------+------------------------------------------------------+
        * | Byte Index |  Field        |                          Notes                       |
        * +------------+---------------+------------------------------------------------------+
        * | 0          | ACK           | 0x06 Operation successful                            |
        * | 1          | ACK Info Size | 0x0E ( 14 ) bytes of data                            |
        * | 2..15      | Data          | Data bytes with the RDD Status                       |
        * | 16..17     | CRC           | Byte 0: 0x06 ACK, 0x15 NAK                           |
        * +------------+---------------+------------------------------------------------------+
        * Data..
        * +------------+---------------+------------------------------------------------------+
        * | Byte Index |  Field        |                          Notes                       |
        * +------------+---------------+------------------------------------------------------+
        * | 2..10      | Serial Number | Serial Number of the RDD                             |
        * | 11         | Valve Pos.    | Current valve position                               |
        * |            |               | 0 - Position unknown, 1 - Closed, 2 - Open           |
        * |            |               | 3 - Partial open, 4 - In transition                  |
        * | 12         | Battery       | Current battery status                               |
        * | 13         | Prev. Source  | Previous command source                              |
        * |            |               | NOTE: Not implemented so always = 0xFF               |
        * | 14         | Prev. Cmd     | Previous command used                                |
        * |            |               | 1 = Close, 2 = Open, 3 = Partial Open                |
        * |            |               | 4 = Sediment Turn, 0xFF = Unknown                    |
        * | 15         | Prev Status   | Previous command status                              |
        * |            |               | 0 = Failed, 1 = Success, 0xFF = Unknown              |
        * +------------+---------------+------------------------------------------------------+
        */

        #region Constants

        public enum RDDStatus {
            DISABLED,
            BUSY,
            ERROR_ON_LAST_OPERATION,
            IDLE
        }

        public enum RDDCmdSource {
            BLUETOOTH = 1,
            THREE_WIRE_PORT,
            TIMED_SEDIMENT_TURN,
            UNKNOWN = 0xFF
        }

        public enum RDDCmd {
            CLOSE = 1,
            OPEN,
            PARTIAL_OPEN,
            SEDIMENT_TURN,
            UNKNOWN = 0xFF
        }

        public enum RDDValveStatus {
            UNKNOWN,
            CLOSED,
            OPEN,
            PARTIAL_OPEN,
            IN_TRANSITION
        }

        public enum RDDBatteryStatus {
            LOW,
            GOOD,
            UNKNOWN = 0xFF
        }

        public  const int MAX_LENGTH_FIRMWARE     = 12;
        private const int BYTE_SERIAL_NUMBER      = 2;
        private const int NUM_BYTES_SERIAL_NUMBER = 9;
        private const int BYTE_VALVE_POSITION     = 11;
        private const int BYTE_BATTERY            = 12;
        private const int BYTE_PREV_CMD_SOURCE    = 13;
        private const int BYTE_PREV_CMD           = 14;
        private const int BYTE_PREV_CMD_STATUS    = 15;

        #endregion

        #region Attributes
        
        private long             serialNumber;
        private RDDValveStatus   valvePosition;
        private RDDBatteryStatus battery;
        private RDDCmdSource     prevCmdSource;
        private RDDCmd           prevCmd;
        private bool             prevCmdStatus;

        #endregion

        #region Properties

        public long SerialNumber
        {
            get { return this.serialNumber; }
        }

        public RDDValveStatus ValvePosition
        {
            get { return this.valvePosition; }
        }

        public RDDBatteryStatus Battery
        {
            get { return this.battery; }
        }

        public RDDCmdSource PreviousCmdSource
        {
            get { return this.prevCmdSource; }
        }

        // Previous command different from the current state / position of the valve
        public RDDCmd PreviousCmd
        {
            get { return this.prevCmd; }
        }

        public bool PreviousCmdSuccess
        {
            get { return this.prevCmdStatus; }
        }

        #endregion

        #region Initialization

        public RDDStatusResult (
            byte[] response )
        {
            this.serialNumber  = Utils.CalculateNumericFromBytes<long>  ( response, BYTE_SERIAL_NUMBER, NUM_BYTES_SERIAL_NUMBER );               // 2 to 10
            this.valvePosition = Utils.ParseIntToEnum<RDDValveStatus>   ( ( int )response[ BYTE_VALVE_POSITION  ], RDDValveStatus  .UNKNOWN ); // 11
            this.battery       = Utils.ParseIntToEnum<RDDBatteryStatus> ( ( int )response[ BYTE_BATTERY         ], RDDBatteryStatus.UNKNOWN ); // 12
            this.prevCmdSource = Utils.ParseIntToEnum<RDDCmdSource>     ( ( int )response[ BYTE_PREV_CMD_SOURCE ], RDDCmdSource    .UNKNOWN ); // 13
            this.prevCmd       = Utils.ParseIntToEnum<RDDCmd>           ( ( int )response[ BYTE_PREV_CMD        ], RDDCmd          .UNKNOWN ); // 14
            this.prevCmdStatus = ( int )response[ BYTE_PREV_CMD_STATUS ] == 1;                                                                 // 15
        }

        #endregion
    }
}
