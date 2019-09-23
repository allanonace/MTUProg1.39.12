using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lexi.Exceptions;
using Lexi.Interfaces;
using Library;
using Library.Exceptions;

namespace Lexi
{
    /// <summary>
    /// Contains all methods required to implement the Lexi ( Local EXternal Interface ) Protocol V2.
    /// </summary>
    public class Lexi
    {
        #region Constants

        /// <summary>
        /// Types of actions supported by our implementation of the LExI protocol.
        /// <para>&#160;</para>
        /// </para>
        /// <list type="LexiAction">
        /// <item>
        ///     <term>LexiAction.Read</term>
        ///     <description>Reads data from the physical memory of the MTU</description>
        /// </item>
        /// <item>
        ///     <term>LexiAction.Write</term>
        ///     <description>Writes data to the physical memory of the MTU</description>
        /// </item>
        /// <item>
        ///     <term>LexiAction.OperationRequest</term>
        ///     <description>Writes data to the physical memory of the MTU, requesting a specific operation</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public enum LexiAction
        {
            Read,
            Write,
            OperationRequest
        }

        /// <summary>
        /// Filters for the log requests operation.
        /// </summary>
        /// <remarks>
        /// NOTE: Copied from Aclara source code.
        /// </remarks>
        public enum LogFilterMode
        {
            /// <summary>
            /// Do not filter results.
            /// </summary>
            None = 0,

            /// <summary>
            /// Only return matching entries.
            /// </summary>
            Match = 1,

            /// <summary>
            /// Return entries that are not of the specified type.
            /// </summary>
            DontMatch = 2
        }

        /// <summary>
        /// Types of log events to recover using the log requests operation.
        /// </summary>
        /// <remarks>
        /// NOTE: Copied from Aclara source code.
        /// </remarks>
        public enum LogEntryType : byte
        {
            /// <summary>
            /// Standard meter reading.
            /// </summary>
            MeterRead = 0x01,

            /// <summary>
            /// Autodetect for a meter was run.
            /// </summary>
            MeterAutodetect = 0x02,

            /// <summary>
            /// Entered or exited ship mode.
            /// </summary>
            ShipModeWrite = 0x03,

            /// <summary>
            /// The MTU reset.
            /// </summary>
            MtuReset = 0x04,

            /// <summary>
            /// The time on the MTU was changed.
            /// </summary>
            TimeAdjustment = 0x05,

            /// <summary>
            /// A time sync was requested from the DCU.
            /// </summary>
            TimeSyncReqTransmit = 0x06,

            /// <summary>
            /// The MTU firmware was upgraded.
            /// </summary>
            FwUpgrade = 0x07,

            /// <summary>
            /// The MTU encountered an alarm condition.
            /// </summary>
            Alarm = 0x08,

            /// <summary>
            /// The AFC averaging adjustment was performed.
            /// </summary>
            AfcAdjustment = 0x09,

            /// <summary>
            /// A custom, user defined, message.
            /// </summary>
            CustomLexiLog = 0x0A,

            /// <summary>
            /// The event log was read.
            /// </summary>
            LexiQuery = 0x0B,

            /// <summary>
            /// Sent a health message.
            /// </summary>
            HealthMessage = 0x0C,

            /// <summary>
            /// Port 1 information during wakeup.
            /// </summary>
            WakeupPort1 = 0x0D,

            /// <summary>
            /// Port 2 information during wakeup.
            /// </summary>
            WakeupPort2 = 0x0E,

            /// <summary>
            /// Write to a memory map location.
            /// </summary>
            MemoryMapWrite = 0x0F,

            /// <summary>
            /// Min/Max and averages of general system information.
            /// </summary>
            SystemStatusReport = 0x10,

            /// <summary>
            /// Start of the node discovery process.
            /// </summary>
            DiscoveryStart = 0x11,

            /// <summary>
            /// Discovery response information from a DCU.
            /// </summary>
            DiscoveryResponse = 0x12,

            /// <summary>
            /// Information about received messages.
            /// </summary>
            GeneralRfReceive = 0x13,

            /// <summary>
            /// Moisture measurement information.
            /// </summary>
            Moisture = 0x14,

            /// <summary>
            /// Significant VSWR change event information.
            /// </summary>
            Vswr = 0x15,

            /// <summary>
            /// General information about a watchdog related crash.
            /// </summary>
            WatchdogCrash = 0x16,

            /// <summary>
            /// Contains priority list information related to a watchdog crash.
            /// </summary>
            PriorityListHead = 0x17,

            /// <summary>
            /// Contains information about the event block related to a watchdog crash.
            /// </summary>
            EventStatus = 0x18,

            /// <summary>
            /// Contains information about transnmissions by the MTU.
            /// </summary>
            GeneralRfTransmission = 0x19,

            /// <summary>
            /// Contains information about RDD actions and status.
            /// </summary>
            Rdd = 0x1A,

            /// <summary>
            /// Contains information about GPS events.
            /// </summary>
            Gps = 0x1B,

            /// <summary>
            /// Contains information about a Zonescan event.
            /// </summary>
            ZonescanEvent = 0x1C,

            /// <summary>
            /// Contains information about a Zonescan event.
            /// </summary>
            ZonescanWakeup = 0x1D,

            /// <summary>
            /// Contains information about a Zonescan event.
            /// </summary>
            ZonescanWakeupExt = 0x1E,

            /// <summary>
            /// Contains information about a radio failure
            /// </summary>
            RadioFailure = 0x1F,

            /// <summary>
            /// Entry for when a message is scheduled for transmit.
            /// </summary>
            TxScheduled = 0x20,

            /// <summary>
            /// Entry for a function call log.
            /// </summary>
            FnCall = 0xFE,

            /// <summary>
            /// There is no event - this is a blank log item.
            /// </summary>
            Unused = 0xFF
        }

        public enum NodeType : byte
        {
            All      = 0x00,
            DCUsOnly = 0x01,
            MTUsOnly = 0x02,
            Zonescan = 0x03
        }

        /// <summary>
        /// Precalculated CRC table that is used by CRC validation process to make it faster.
        /// </summary>
        static uint[] CRCTable = {0, 4489, 8978, 12955, 17956, 22445, 25910, 29887, 35912, 40385, 44890,
                                  48851, 51820, 56293, 59774, 63735, 4225, 264, 13203, 8730, 22181,
                                  18220, 30135, 25662, 40137, 36160, 49115, 44626, 56045, 52068, 63999,
                                  59510, 8450, 12427, 528, 5017, 26406, 30383, 17460, 21949, 44362,
                                  48323, 36440, 40913, 60270, 64231, 51324, 55797, 12675, 8202, 4753,
                                  792, 30631, 26158, 21685, 17724, 48587, 44098, 40665, 36688, 64495,
                                  60006, 55549, 51572, 16900, 21389, 24854, 28831, 1056, 5545, 10034,
                                  14011, 52812, 57285, 60766, 64727, 34920, 39393, 43898, 47859, 21125,
                                  17164, 29079, 24606, 5281, 1320, 14259, 9786, 57037, 53060, 64991,
                                  60502, 39145, 35168, 48123, 43634, 25350, 29327, 16404, 20893, 9506,
                                  13483, 1584, 6073, 61262, 65223, 52316, 56789, 43370, 47331, 35448,
                                  39921, 29575, 25102, 20629, 16668, 13731, 9258, 5809, 1848, 65487,
                                  60998, 56541, 52564, 47595, 43106, 39673, 35696, 33800, 38273, 42778,
                                  46739, 49708, 54181, 57662, 61623, 2112, 6601, 11090, 15067, 20068,
                                  24557, 28022, 31999, 38025, 34048, 47003, 42514, 53933, 49956, 61887,
                                  57398, 6337, 2376, 15315, 10842, 24293, 20332, 32247, 27774, 42250,
                                  46211, 34328, 38801, 58158, 62119, 49212, 53685, 10562, 14539, 2640,
                                  7129, 28518, 32495, 19572, 24061, 46475, 41986, 38553, 34576, 62383,
                                  57894, 53437, 49460, 14787, 10314, 6865, 2904, 32743, 28270, 23797,
                                  19836, 50700, 55173, 58654, 62615, 32808, 37281, 41786, 45747, 19012,
                                  23501, 26966, 30943, 3168, 7657, 12146, 16123, 54925, 50948, 62879,
                                  58390, 37033, 33056, 46011, 41522, 23237, 19276, 31191, 26718, 7393,
                                  3432, 16371, 11898, 59150, 63111, 50204, 54677, 41258, 45219, 33336,
                                  37809, 27462, 31439, 18516, 23005, 11618, 15595, 3696, 8185, 63375,
                                  58886, 54429, 50452, 45483, 40994, 37561, 33584, 31687, 27214, 22741,
                                  18780, 15843, 11370, 7921, 3960};

        #endregion

        #region Attributes

        private ISerial m_serial; // Serial port interface used to communicate through Lexi

        private int m_timeout; // Timout limit to wait for MTU response.

        #endregion

        #region Initialization

        public Lexi()
        {
            //set default read wait to response timeout to 400ms
            m_timeout = 400;
        }

        public Lexi(ISerial serial, int timeout)
        {
            m_serial = serial;
            m_timeout = timeout;
        }

        #endregion

        #region Read and Write

        /// <summary>
        /// Prepares and executes a read action from the physical memory of the MTU.
        /// </summary>
        /// <param name="addres">Address in the physical memory of the MTU</param>
        /// <param name="data">Additional data to be sent with the LExI command</param>
        /// <returns>Response from the MTU.</returns>
        /// <seealso cref="Write(uint, byte[], uint[], LexiFiltersResponse, LexiAction)"/>
        public async Task<byte[]> Read (
            UInt32 addres,
            uint data )
        {
            if ( m_serial == null )
                throw new ArgumentNullException ( "No Serial interface defined" );

            return await Read ( m_serial, addres, data, m_timeout );
        }

        private async Task<byte[]> Read (
            ISerial serial,
            UInt32 address,
            uint bytesToRead,
            int timeout )
        {
            int TEST = new Random ().Next ( 0, 999 );
            Utils.PrintDeep ( Environment.NewLine + "--------LEXI_READ-------| " + TEST + " |--" );
            Utils.PrintDeep ( "Lexi.Read = Write + UpdateBuffer + Read" );
        
            try
            {
                byte[] stream;
                var info = GeneratePackage ( LexiAction.Read, out stream, address, null, bytesToRead );
                
                Utils.PrintDeep ( "Lexi.Read.. " +
                "Stream = " +
                "0x" + info.Header + " ( " + Convert.ToInt32 ( info.Header, 16 ) + " ) + " +
                "WriteCmd 0x" + info.Cmd + " ( " + Convert.ToInt32 ( info.Cmd, 16 ) + " ) + " +
                "Address 0x" + info.StartAddress + " ( " + Convert.ToInt32 ( info.StartAddress, 16 ) + " ) + " +
                "Checksum 0x" + info.Checksum + " ( " + Convert.ToInt32 ( info.Checksum, 16 ) + " )" );
    
                Utils.PrintDeep ( "Lexi.Read.. " + Utils.ByteArrayToString ( stream ).Trim () + " [ Length " + stream.Length + " ]" );
    
                // Send Lexi Read command
                await serial.Write ( stream, 0, stream.Length );
                
                Utils.PrintDeep ( "------BUFFER_START-------" );
                
                //define response buffer size
                byte[] rawBuffer = new byte[0];
                int headerOffset = ( serial.isEcho () ) ? stream.Length : 0;
                Array.Resize ( ref rawBuffer, headerOffset + ( int )bytesToRead + 2 ); // Package + Data + ACK
                
                Utils.PrintDeep ( "Lexi.Read -> Array.Resize.." +
                    " Echo " + serial.isEcho () +
                    " [ " + rawBuffer.Length + " = " + bytesToRead + " + Header " + headerOffset + " + CRC 2 ]" );

                // Whait untill the response buffer data is available or timeout limit is reached
                long timeout_limit = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (timeout);
                await Task.Run(() =>
                {
                    while (checkResponseOk(serial, rawBuffer))
                    {
                        if (DateTimeOffset.Now.ToUnixTimeMilliseconds() > timeout_limit)
                        {
                            int num = serial.BytesReadCount();

                            Utils.PrintDeep("Lexi.Read -> BytesToRead: " + num);

                            if (num <= headerOffset)
                            {
                                Utils.PrintDeep("Lexi.Read -> CheckResponseOk IOException");

                                throw new IOException();
                            }
                            else
                            {
                                Utils.PrintDeep("Lexi.Read -> CheckResponseOk TimeoutException");

                                throw new TimeoutException();
                            }
                        }
                        Thread.Sleep(10);
                    }
                });
    
                Utils.PrintDeep ( "------BUFFER_FINISH------" );
    
                //read the response buffer
                serial.Read ( rawBuffer, 0, rawBuffer.Length );
                
                /*
                 * +------------+-------+---------------+--------------------------------------------------------+
                 * | Byte Index | Field |   Value(s)    |                         Notes                          |
                 * +------------+-------+---------------+--------------------------------------------------------+
                 * | 0..N-1     | Data  | Read from RAM | N = number of bytes specified in Data field of Request |
                 * | N..N+1     | CRC   | Calculated    | CRC over all data bytes.  See section 7.2              |
                 * +------------+-------+---------------+--------------------------------------------------------+
                 */
    
                // Removes header and get only the ACK
                byte[] response = new byte[ 2 ];
                Array.Resize ( ref response, response.Length + ( int )bytesToRead );
                Array.Copy ( rawBuffer, headerOffset, response, 0, response.Length );
                
                Utils.PrintDeep ( "Lexi.Read ->" +
                    " CheckCRC " + Utils.ByteArrayToString ( response ) +
                    " [ " + response.Length + " = BytesToRead " + bytesToRead + " + ACK 2 ]" );
    
                // Validates CRC and if everything is ok, returns recovered data
                return validateReadResponse ( response, bytesToRead );
            }
            catch ( Exception e )
            {
                throw new LexiReadingException ();
            }
            finally
            {
                Utils.PrintDeep ( "----LEXI_READ_FINISH----| " + TEST + " |--" + Environment.NewLine );
            }
        }

        /// <summary>
        /// Prepares and executes a write action in the physical memory of the MTU.
        /// </summary>
        /// <param name="address">Address in the physical memory of the MTU</param>
        /// <param name="data">Additional data to be sent with the LExI command</param>
        /// <param name="bytesResponse">Will store the created stream/package</param>
        /// <param name="filtersResponse">Custom filters to accept specific responses</param>
        /// <param name="lexiAction">Action to perform using <see cref="LexiAction"/> enumeration ( lexiAction.OperationRequest or lexiAction.Write )</param>
        /// <returns>Response from the MTU.</returns>
        /// <seealso cref="Read(uint, uint)"/>
        public async Task<LexiWriteResult> Write (
            uint   addressOrLexiCmd,
            byte[] data            = null,
            int    attempts        = 1,
            int    secsBtwAttempts = 1,
            uint[] bytesResponse   = null, // By default is +2 ACK
            LexiFiltersResponse filtersResponse = null, // It is used when multiple responses are possible ( base 0 )
            LexiAction lexiAction  = LexiAction.Write )
        {
            if ( m_serial == null )
                throw new ArgumentNullException ( "No Serial interface defined" );

            if ( bytesResponse == null )
                bytesResponse = new uint[] { 2 }; // ACK + ACK Info Size

            // Some Operation Request commands do not have more data than the header
            if ( data is null )
                data = new byte[ 0 ]; // Empty data array

            // Try the specified time of attempts
            LexiWriteResult result = null;
            if ( attempts        <= 0 ) attempts        = 1;
            if ( secsBtwAttempts <= 0 ) secsBtwAttempts = 1;
            int count = 0;
            do
            {
                Utils.PrintDeep ( Environment.NewLine + "-------LEXI_WRITE--------| Attempt " + ++count );

                try
                {
                    result = await Write (
                        m_serial,
                        addressOrLexiCmd,
                        data,
                        bytesResponse,
                        filtersResponse,
                        m_timeout,
                        lexiAction );

                    break;
                }
                catch ( Exception e )
                {
                    if ( --attempts > 0 )
                        await Task.Delay ( secsBtwAttempts * 1000 );
                    else
                        throw e;
                }
            }
            while ( attempts > 0 );

            return result;
        }
        
        private async Task<LexiWriteResult> Write (
            ISerial serial,
            UInt32 addressOrLexiCmd,
            byte[] data,
            uint[] bytesResponse,
            LexiFiltersResponse filtersResponse,
            int timeout,
            LexiAction lexiAction )
        {
            int TEST = new Random ().Next ( 0, 999 );
            Utils.PrintDeep ( Environment.NewLine + "-------LEXI_WRITE--------| " + TEST + " |--" );
            Utils.PrintDeep ( "Lexi.Write = Write + UpdateBuffer + ReadBuffer" );
    
            try
            {
                // Write normal: 25, WriteCmd, Address, Data.Length, Checksum, Data, CRC
                //   Response: PACKAGE + ACK
                // Operation 19: 25, 0xFE, OpCmd ( 0x13 ), Data.Length ( 10 ), Checksum, Data, CRC
                //   Response: PACKAGE + ACK
                // Operation 20: 25, 0xFE, OpCmd ( 0x14 ), Data.Length ( 0 ), Checksum
                //   Response: PACKAGE + ACK [ + Data ( 21 bytes ) ]
                byte[] stream;
                var info = GeneratePackage ( lexiAction, out stream, addressOrLexiCmd, data );

                Utils.PrintDeep ( "Lexi.Write.. " +
                "Stream = " +
                "0x" + info.Header + " ( " + Convert.ToInt32 ( info.Header, 16 ) + " ) + " +
                "WriteCmd 0x" + info.Cmd + " ( " + Convert.ToInt32 ( info.Cmd, 16 ) + " ) + " +
                "Address 0x" + info.StartAddress + " ( " + Convert.ToInt32 ( info.StartAddress, 16 ) + " ) + " +
                "NumBytesToWrite 0x" + data.Length + " ( " + Convert.ToInt32 ( data.Length + "", 16 ) + " ) + " +
                "Checksum 0x" + info.Checksum + " ( " + Convert.ToInt32 ( info.Checksum, 16 ) + " ) + " +
                "Data [ " + Utils.ByteArrayToString ( data ) + " ] + " +
                "CRC [ " + Utils.ByteArrayToString ( info.CRC.Take ( 2 ).ToArray () ) + " ]" );
    
                Utils.PrintDeep ( "Lexi.Write.. " + Utils.ByteArrayToString ( stream ).Trim () + " [ Length " + stream.Length + " ]" );
    
                // Send Lexi Write command
                await serial.Write ( stream, 0, stream.Length );
    
                Utils.PrintDeep ( "------BUFFER_START-------" );
    
                //define response buffer size
                byte[] rawBuffer    = new byte[0];
                int responseOffset  = ( serial.isEcho () ) ? stream.Length : 0;
                int maxByteResponse = ( int )bytesResponse.Max ();
                Array.Resize ( ref rawBuffer, responseOffset + maxByteResponse ); // Echo + Response

                // Update possible responses and filters adding echo length
                bytesResponse = bytesResponse.Select ( entry => entry + ( uint )responseOffset ).ToArray ();

                bool hasFilters = ( filtersResponse != null );
                if ( hasFilters )
                    for ( int i = 0; i < filtersResponse.Count; i++ )
                        filtersResponse[ i ].ResponseBytes += responseOffset;
                
                Utils.PrintDeep ( "Lexi.Write.. " +
                    "Echo " + serial.isEcho ().ToString ().ToUpper () +
                    " | StreamFromMTU = Echo x" + stream.Length +
                    " + Max.Response x" + maxByteResponse +
                    " = " + rawBuffer.Length + " bytes" );
    
                // Wait until the response buffer data is available or timeout limit is reached
                int  bytesRead = 0;
                long timeout_limit = DateTimeOffset.Now.ToUnixTimeMilliseconds () + timeout;
                await Task.Run ( () =>
                {
                    while ( ( bytesRead = serial.BytesReadCount () ) < rawBuffer.Length - 1 )
                    {
                        Utils.PrintDeep ( "Lexi.Write.. BytesRead: " + bytesRead + "/" + rawBuffer.Length +
                            " | " + Utils.ByteArrayToString ( serial.BytesRead () ) +
                            " [ " + DateTimeOffset.Now.ToUnixTimeMilliseconds() + " > " + timeout_limit + " ]" );

                        Utils.PrintDeep ( "Lexi.Write.. Check " + bytesRead + " in Responses [ " + Utils.ArrayToString ( bytesResponse ) + " ]" );

                        // Maybe already recovered data is a valid response
                        if ( bytesResponse.Contains ( ( uint )( bytesRead ) ) )
                        {
                            Utils.PrintDeep ( "Lexi.Write.. Number of bytes are equal to some response [ " + bytesRead + " ]" );
                            
                            // This response could have some conditions to validate
                            if ( hasFilters )
                            {
                                bool ok = false;
                                byte[] arBytesRead = serial.BytesRead ();
                                var filters = filtersResponse.Entries.Where ( entry => entry.ResponseBytes == bytesRead ).ToArray ();
                                for ( int i = 0; i < filters.Length; i++ )
                                {
                                    var filter = filters[ i ];
                                    bool conditionOk = arBytesRead[ responseOffset + filter.IndexByte ] == filter.Value;

                                    Utils.PrintDeep ( "Lexi.Write.. Condition ( " + arBytesRead[ responseOffset + filter.IndexByte ] +
                                        " == " + filter.Value + " ) = " + conditionOk.ToString ().ToUpper () );

                                    if ( conditionOk )
                                    {
                                        ok = true;
                                        break;
                                    }
                                }
                                
                                // No condition was validated
                                if ( ! ok )
                                    goto CONTINUE;

                                Utils.PrintDeep ( "Lexi.Write.. Conditions validated and response accepted" );
                            }

                            // Remove tail of zeros
                            Array.Resize ( ref rawBuffer, bytesRead );

                            break;
                        }
                        
                        CONTINUE:

                        if ( DateTimeOffset.Now.ToUnixTimeMilliseconds() > timeout_limit )
                        {
                            if ( bytesRead <= responseOffset )
                            {
                                Utils.PrintDeep("Lexi.Write -> CheckResponseOk IOException");

                                throw new IOException();
                            }
                            else
                            {
                                Utils.PrintDeep("Lexi.Write -> CheckResponseOk TimeoutException");

                                throw new TimeoutException();
                            }
                        }
                        Thread.Sleep ( 10 );
                    }
                });
                
                Utils.PrintDeep ( "Lexi.Read.. BytesRead: " + bytesRead + " / " + rawBuffer.Length );
                
                Utils.PrintDeep ( "------BUFFER_FINISH------" );

                serial.Read ( rawBuffer, 0, rawBuffer.Length );
    
                // First two bytes always are the ACK
                byte[] response = new byte[ 2 ];
                Array.Copy ( rawBuffer, responseOffset, response, 0, response.Length );
                
                Utils.PrintDeep ( "Lexi.Write.." +
                    " RawBuffer " + Utils.ByteArrayToString ( rawBuffer ) +
                    " | ACK " + Utils.ByteArrayToString ( response ) );
    
                // If first byte of the recovered ACK is 6, everything has gone OK
                if ( response[0] != 0x06 )
                    throw new LexiWriteException ( response );
                else
                {
                    Utils.PrintDeep ( "----LEXI_WRITE_FINISH----| " + TEST + " |--" + Environment.NewLine );

                    // Return MTU response
                    //return ( bytes: rawBuffer, responseOffset: responseOffset );
                    return new LexiWriteResult ( rawBuffer, responseOffset );
                }
            }
            catch ( Exception e )
            {
                throw new LexiWritingException ();
            }
        }

        /// <summary>
        /// Depending on the process to be executed ( write or read ) and the LExI command,
        /// this method prepares the package to be sent to the MTU, avoiding to have dupled
        /// code inside read and write methods.
        /// <para>
        /// See <see cref="Read(uint, uint)"/> to send read command to the MTU.
        /// </para>
        /// <para>
        /// See <see cref="Write(uint, byte[], uint[], (int responseBytes, int indexByte, byte value)[], LexiAction)"/>
        /// to send write command to the MTU.
        /// </para>
        /// </summary>
        /// <param name="lexiAction">Action to perform using <see cref="LexiAction"/> enumeration</param>
        /// <param name="array">Will store the created stream/package</param>
        /// <param name="address">Address in the physical memory of the MTU</param>
        /// <param name="data">Additional data to be sent with the LExI command</param>
        /// <param name="arguments">Number of bytes to read ( only used reading )</param>
        /// <returns></returns>
        private LexiPackage GeneratePackage (
            LexiAction lexiAction,
            out byte[] array,
            uint address,
            byte[] data = null,
            params object[] arguments )
        {
            byte[] header = new byte[ 4 ];
            byte[] crc    = new byte[ 0 ];

            switch ( lexiAction )
            {
                case LexiAction.Read:
                /*
                * +------------+----------+----------------------------------------------------------+
                * | Byte Index |  Field   |                          Notes                           |
                * +------------+----------+----------------------------------------------------------+
                * | 0          | Header   | This field is always 0x25                                |
                * | 1          | Command  | 0x80, 0x82, 0x84, 0x86                                   |
                * | 2          | Address  | Address of first byte to read                            |
                * | 3          | Size     | Number of bytes to read                                  |
                * | 4          | Checksum | 2’s complement of sum: header + command + address + size |
                * +------------+----------+----------------------------------------------------------+
                * Command
                * -------
                * block 0: addres >=   0 & < 256 = 0x80 => 128
                * block 1: addres >= 256 & < 512 = 0x82 => 130
                * block 2: addres >= 512 & < 768 = 0x84 => 132
                * block 3: addres >=         768 = 0x86 => 134
                * -------
                * Address
                * -------
                * Address become offset from 256 block
                * Example: Addres 600 --> block 2 --> offset 600 - 512 = 88
                */
                header[ 0 ] = 0x25;
                header[ 1 ] = ( byte )( uint )(     128 + ( ( address / 256 ) *   2 ) );
                header[ 2 ] = ( byte )( uint )( address - ( ( address / 256 ) * 256 ) );
                header[ 3 ] = ( byte )( uint )arguments[ 0 ];
                header      = checkSum ( header );
                break;

                case LexiAction.Write:
                /*
                * +------------+----------+----------------------------------------------------------+
                * | Byte Index |  Field   |                          Notes                           |
                * +------------+----------+----------------------------------------------------------+
                * | 0          | Header   | This field is always 0x25                                |
                * | 1          | Command  | 0x81, 0x83, 0x85, 0x87                                   |
                * | 2          | Address  | Address of first byte to write                           |
                * | 3          | Size     | Number of bytes to write                                 |
                * | 4          | Checksum | 2’s complement of sum: header + command + address + size |
                * | 5..4+N     | Data     | Data array                                               |
                * | 6+N..7+N   | CRC      | Byte 0: 0x06 ACK, 0x15 NAK | Byte 1: NAK Reason          |
                * +------------+----------+----------------------------------------------------------+
                * Command
                * -------
                * block 0: addres >=   0 & < 256 = 0x81 => 129
                * block 1: addres >= 256 & < 512 = 0x83 => 131
                * block 2: addres >= 512 & < 768 = 0x85 => 133
                * block 3: addres >=         768 = 0x87 => 135
                */
                header[ 0 ] = 0x25;
                header[ 1 ] = ( byte )( uint )(     129 + ( ( address / 256 ) *   2 ) );
                header[ 2 ] = ( byte )( uint )( address - ( ( address / 256 ) * 256 ) );
                header[ 3 ] = ( byte )data.Length;
                header      = checkSum ( header );
                break;

                case LexiAction.OperationRequest:
                /*
                * Base package format
                * +------------+---------------+------------------------------------------------------+
                * | Byte Index |  Field        |                          Notes                       |
                * +------------+---------------+------------------------------------------------------+
                * | 0          | Header        | This field is always 0x25                            |
                * | 1          | Command       | 0xFE                                                 |
                * | 2          | Request Code  | Identifies specific request                          |
                * | 3          | Size          | Number of bytes of argument data to follow           |
                * | 4          | Checksum      | 2’s complement of sum: header + cmd + address + size |
                * | 5..4+N     | Argument Data | Data array                                           |
                * | 6+N..7+N   | CRC           | Byte 0: 0x06 ACK, 0x15 NAK                           |
                * +------------+---------------+------------------------------------------------------+
                * 
                * Start Event Log Query: Op.Cmd 0x13 ( 19 )
                * Request..
                * +------------+---------------+------------------------------------------------------+
                * | Byte Index |  Field        |                          Notes                       |
                * +------------+---------------+------------------------------------------------------+
                * | 0          | Header        | 0x25                                                 |--+
                * | 1          | Command       | 0xFE                                                 |  |
                * | 2          | Request Code  | 0x13                                                 |  |--- Header
                * | 3          | Size          | 0x0A ( 10 )                                          |  |
                * | 4          | Checksum      | 2’s complement of sum: header + cmd + code + size    |--+
                * | 5          | Mode          | Filter mode to use 0 = None, 1 = Match, 2 = NotMatch |--+
                * | 6          | Type          | Log entry type                                       |  |--- Data
                * | 7..10      | Start Time    | NCC time for first event - uint, 4 bytes             |  |
                * | 11..14     | Stop Time     | NCC time for last event - uint, 4 bytes              |--+
                * | 15..16     | CRC           | Byte 0: 0x06 ACK, 0x15 NAK                           |
                * +------------+---------------+------------------------------------------------------+
                * Response..
                * +------------+---------------+------------------------------------------------------+
                * | Byte Index |  Field        |                          Notes                       |
                * +------------+---------------+------------------------------------------------------+
                * | 0          | ACK           | 0x06 Operation successful                            |
                * | 1          | ACK Info Size | 0x00 No ACK Info to follow                           |
                * +------------+---------------+------------------------------------------------------+
                * 
                * Get Next Event Log Response: Op.Cmd 0x14 ( 20 )
                * Request..
                * +------------+---------------+------------------------------------------------------+
                * | Byte Index |  Field        |                          Notes                       |
                * +------------+---------------+------------------------------------------------------+
                * | 0          | Header        | 0x25                                                 |--+
                * | 1          | Command       | 0xFE                                                 |  |
                * | 2          | Request Code  | 0x14                                                 |  |--- Header
                * | 3          | Size          | 0x00 ( No data )                                     |  |
                * | 4          | Checksum      | 2’s complement of sum: header + cmd + code + size    |--+
                * +------------+---------------+------------------------------------------------------+
                * Response..
                * ACK with log entry ( Result 0 ): 25 bytes
                * ACK with no log entry ( Result 1 or 2 ): 5 bytes = ACK + ACK Info Size + Result + CRC
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
                */
                header[ 0 ] = 0x25;
                header[ 1 ] = 0xFE;
                header[ 2 ] = ( byte )address;
                header[ 3 ] = ( byte )data.Length;
                header      = checkSum ( header );
                break;
            }

            // Concatenate..
            array = null;
            switch ( lexiAction )
            {
                case LexiAction.Read:
                // It does not require anything else
                array = header;
                break;
                
                case LexiAction.Write:
                case LexiAction.OperationRequest:
                // Some Operation Request commands do not have more data than the header
                bool hasData = data.Length > 0;

                // Header [ + Data to write + Calculated CRC ]
                array = new byte[ header.Length + data.Length + ( ( hasData ) ? 2 : 0 ) ];
                Array.Copy ( header, 0, array, 0, header.Length );
                
                if ( hasData )
                {
                    crc = BitConverter.GetBytes ( calcCRC ( data, data.Length ) );
                    Array.Copy ( data, 0, array, header.Length, data.Length );
                    Array.Copy ( crc, 0, array, header.Length + data.Length, 2 );
                }
                break;
            }
            
            return new LexiPackage (
                header      : String.Format ( "{0:x2}", array[ 0 ] ),
                cmd         : String.Format ( "{0:x2}", array[ 1 ] ),
                startAddress: String.Format ( "{0:x2}", array[ 2 ] ),
                checksum    : String.Format ( "{0:x2}", array[ 4 ] ),
                crc         : crc );
        }

        #endregion

        #region CRC and Checksum

        public byte[] CalcCrcNew(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            ushort num = ushort.MaxValue;
            for (int i = 0; i < data.Length; i++)
            {
                byte b = data[i];
                for (int j = 0; j < 8; j++)
                {
                    bool flag = ((b ^ num) & 1) != 0;
                    b = (byte)(b >> 1);
                    num = (ushort)(num >> 1);
                    if (flag)
                    {
                        num = (ushort)(num ^ 0x8408);
                    }
                }
            }
            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.GetBytes(num);
            }
            return BitConverter.GetBytes(num).Reverse().ToArray();
        }

        static uint calcCRC(byte[] data, int len)
        {
            uint accum = 0xffff;

            for (int i = 0; i < len; i++)
                accum = (accum >> 8) ^ CRCTable[(accum ^ data[i]) & 0x00ff];
            return accum;
        }

        private byte GetChecksum(IEnumerable<byte> values)
        {
            return (byte)(255 - (byte)values.Sum((byte x) => x) + 1);
        }

        static byte[] checkSum(byte[] data)
        {
            int chksum = 0;

            for (int i = 0; i < data.Length; i++)
                chksum += data[i];
            chksum = (chksum ^ 0xff) + 1;
            if (chksum < 0)
                chksum += 256;

            Array.Resize(ref data, data.Length + 1);
            data[data.Length - 1] = Convert.ToByte(chksum & 0x00ff); ;

            return data;
        }

        private bool checkResponseOk(ISerial serial, byte[] rawBuffer)
        {
            return serial.BytesReadCount() < rawBuffer.Length;
        }

        private byte[] validateReadResponse(byte[] response, uint response_length)
        {
            // Recover data has not last two CRC bytes
            if ( ! ( response.Length - 2 == response_length ) )
                throw new InvalidDataException ("");

            byte[] crc = new byte[2];
            byte[] response_body = new byte[0];
            Array.Resize ( ref response_body, response.Length - 2 );

            Array.Copy ( response, response.Length - 2, crc, 0, crc.Length );   // CRC
            Array.Copy ( response, 0, response_body, 0, response_body.Length ); // Data

            if ( calcCRC ( response_body, response_body.Length ) != BitConverter.ToUInt16 ( crc, 0 ) )
                throw new InvalidDataException("Bad CRC");
            
            Utils.PrintDeep ( "Lexi.ValidateResponde ->" +
                " CRC " + Utils.ByteArrayToString ( crc ) +
                " | Data " + Utils.ByteArrayToString ( response_body ) +
                " [ " + response_body.Length + " ]" );

            return response_body;
        }

        #endregion
    }
}
