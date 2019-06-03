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
    /// Lexi Protocol Class.
    /// Contains all methods of Lexi Protocol V2: Read, Write and Operations.
    /// <see cref="Lexi.Lexi" /> Protocol Class.
    /// Contains all methods of Lexi Protocol V2: Read, Write and Operations.
    /// </summary>
    public class Lexi
    {
        #region Constants

        public enum LexiAction
        {
            Read,
            Write,
            OperationRequest
        }

        /// <summary>
        /// Precalculated CRC Table
        /// </summary>
        /// <remarks>This table is used by CRC validation function and it makes CRC calculation fast</remarks>
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

        /// <summary>
        /// Serial port interface used to communicate through Lexi
        /// </summary>
        /// <remarks>User should iplement custom serial that inherist from ISearial</remarks>
        private ISerial m_serial;

        /// <summary>
        /// Timout limit to wait for MTU response.
        /// </summary>
        /// <remarks>Once request is sent, timeot defines the time to wait for a response from MTU</remarks>
        private int m_timeout;

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="Lexi.Lexi" /> class. 
        /// </summary>
        /// <remarks></remarks>
        public Lexi()
        {
            //set default read wait to response timeout to 400ms
            m_timeout = 400;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lexi.Lexi" /> class.
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="timeout"></param>
        public Lexi(ISerial serial, int timeout)
        {
            m_serial = serial;
            m_timeout = timeout;
        }

        #endregion

        #region Read and Write

        /// <summary>
        /// Adds two integers and returns the result.
        /// </summary>
        /// <param name="addres">A double precision number.</param>
        /// <param name="data">A double precision number.</param>
        /// <returns>
        /// The sum of two integers.
        /// </returns>
        /// <example>
        ///   <code><![CDATA[
        /// lx.Read(0, 1)
        /// ]]></code>
        /// </example>
        /// <seealso cref="Write(byteaddres,byte[]data)" />
        /// <seealso cref="Write(ISerialserial,UInt32addres,byte[]data,inttimeout)" />
        /// <exception cref="System.ArgumentNullException">Thrown when no Serial is
        /// defines</exception>
        /// <exception cref="System.TimeoutException">Thrown response Timeout is
        /// reached</exception>
        /// <exception cref="System.IO.InvalidDataException">Thrown when response data or
        /// CRC is not valid</exception>
        /// <see cref="Read(ISerial serial, UInt32 addres, uint data, int timeout)" />
        public async Task<byte[]> Read(UInt32 addres, uint data)
        {
            if ( m_serial == null )
                throw new ArgumentNullException("No Serial interface defined");

            return await Read(m_serial, addres, data, m_timeout);
        }

        /// <summary>
        /// Request to read bytes from memory map
        /// </summary>
        /// <param name="serial">A double precision number.</param>
        /// <param name="addres">A double precision number.</param>
        /// <param name="bytesToRead">A double precision number.</param>
        /// <param name="timeut">A double precision number.</param>
        /// <returns>
        /// Memory Map values from read address request
        /// </returns>
        /// <example>
        /// <code>
        /// lx.Read(new USBSerial("COM5"), 0, 1, 400)
        /// </code>
        /// </example>
        /// <exception cref="System.TimeoutException">Thrown response Timeout is reached </exception>
        /// <exception cref="System.IO.InvalidDataException">Thrown when response data or CRC is not valid </exception>
        /// See <see cref="Read(UInt32 addres, uint data)"/> to add doubles.
        /// <seealso cref="Write(byte addres, byte[] data)"/>
        /// <seealso cref="Write(ISerial serial, UInt32 addres, byte[] data, int timeout)"/>
        private async Task<byte[]> Read(ISerial serial, UInt32 address, uint bytesToRead, int timeout)
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
                "0x" + info.header + " ( " + Convert.ToInt32 ( info.header, 16 ) + " ) + " +
                "WriteCmd 0x" + info.cmd + " ( " + Convert.ToInt32 ( info.cmd, 16 ) + " ) + " +
                "Address 0x" + info.startAddress + " ( " + Convert.ToInt32 ( info.startAddress, 16 ) + " ) + " +
                "Checksum 0x" + info.checksum + " ( " + Convert.ToInt32 ( info.checksum, 16 ) + " )" );
    
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

        public async Task<(byte[] bytes, int responseOffset)> Write (
            uint   address,
            byte[] data           = null,
            uint[] bytesResponse  = null, // By default is +2 ACK
            ( int responseBytes, int indexByte, byte value )[] filtersResponse = null, // It is used when multiple responses are possible ( base 0 )
            LexiAction lexiAction = LexiAction.Write )
        {
            if ( m_serial == null )
                throw new ArgumentNullException("No Serial interface defined");

            if ( bytesResponse == null )
                bytesResponse = new uint[] { 2 }; // ACK

            // Some Operation Request commands do not have more data than the header
            if ( data is null )
                data = new byte[ 0 ]; // Empty data array

            return await Write ( m_serial, address, data, bytesResponse, filtersResponse, m_timeout, lexiAction );
        }

        private async Task<(byte[] bytes, int responseOffset)> Write (
            ISerial serial,
            UInt32 address,
            byte[] data,
            uint[] bytesResponse,
            ( int responseBytes, int indexByte, byte value )[] filtersResponse,
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
                var info = GeneratePackage ( lexiAction, out stream, address, data );

                Utils.PrintDeep ( "Lexi.Write.. " +
                "Stream = " +
                "0x" + info.header + " ( " + Convert.ToInt32 ( info.header, 16 ) + " ) + " +
                "WriteCmd 0x" + info.cmd + " ( " + Convert.ToInt32 ( info.cmd, 16 ) + " ) + " +
                "Address 0x" + info.startAddress + " ( " + Convert.ToInt32 ( info.startAddress, 16 ) + " ) + " +
                "NumBytesToWrite 0x" + data.Length + " ( " + data.Length + " ) + " +
                "Checksum 0x" + info.checksum + " ( " + Convert.ToInt32 ( info.checksum, 16 ) + " ) + " +
                "Data [ " + Utils.ByteArrayToString ( data ) + " ] + " +
                "CRC [ " + Utils.ByteArrayToString ( info.crc.Take ( 2 ).ToArray () ) + " ]" );
    
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
                    for ( int i = 0; i < filtersResponse.Length; i++ )
                        filtersResponse[ i ].responseBytes += responseOffset;
                
                Utils.PrintDeep ( "Lexi.Write.. " +
                    "Echo " + serial.isEcho ().ToString ().ToUpper () +
                    " | StreamFromMTU = Stream x" + stream.Length + " + Max.Response x" + maxByteResponse + " = " + rawBuffer.Length + " bytes" );
    
                // Whait untill the response buffer data is available or timeout limit is reached
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
                                var filters = filtersResponse.Where ( entry => entry.responseBytes == bytesRead ).ToArray ();
                                for ( int i = 0; i < filters.Length; i++ )
                                {
                                    var filter = filters[ i ];
                                    bool conditionOk = arBytesRead[ responseOffset + filter.indexByte ] == filter.value;

                                    Utils.PrintDeep ( "Lexi.Write.. Condition ( " + arBytesRead[ responseOffset + filter.indexByte ] + " == " + filter.value + " ) = " + conditionOk.ToString ().ToUpper () );

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
                            // TODO: COMPROBAR TRABAJANDO CON OPERATIONS SI EL PENULTIMO BYTE ES 0X06,
                            // PORQUE POR EJEMPLO GET NEXT EVENT LOG RESPONSE TIENE TRES POSIBLES RESPUESTAS

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
                    return ( bytes: rawBuffer, responseOffset: responseOffset );
                }
            }
            catch ( Exception e )
            {
                throw new LexiWritingException ();
            }
        }

        private ( string header, string cmd, string startAddress, string checksum, byte[] crc ) GeneratePackage (
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
                * 
                * Get Next Event Log Response: Op.Cmd 0x14 ( 20 )
                * +------------+---------------+------------------------------------------------------+
                * | Byte Index |  Field        |                          Notes                       |
                * +------------+---------------+------------------------------------------------------+
                * | 0          | Header        | 0x25                                                 |--+
                * | 1          | Command       | 0xFE                                                 |  |
                * | 2          | Request Code  | 0x14                                                 |  |--- Header
                * | 3          | Size          | 0x00 ( No data )                                     |  |
                * | 4          | Checksum      | 2’s complement of sum: header + cmd + code + size    |--+
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
            
            return (
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

        public byte[] GetNextLogQueryResult()
        {
            return GetNextLogQueryResult(m_serial, m_timeout);
        }

        public byte[] GetNextLogQueryResult(ISerial serial, int timeout)
        {
            List<byte> list = new List<byte>(5);
            list.Add(37);
            list.Add(254);
            list.Add(20);
            list.Add(0);
            list.Add(GetChecksum(list));

            byte[] stream = list.ToArray();

            serial.Write(stream, 0, stream.Length);

            byte[] rawBuffer = new byte[10];
            int response_offest = 0;
            if (serial.isEcho()) { response_offest = stream.Length; }
            Array.Resize(ref rawBuffer, (int)(response_offest + 5));

            long timeout_limit = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (timeout);
            while (serial.BytesReadCount() < rawBuffer.Length )
            {
                if (DateTimeOffset.Now.ToUnixTimeMilliseconds() > timeout_limit)
                {
                    //if even no data response no puck error..

                    if (serial.BytesReadCount() <= response_offest)
                    {
                        throw new IOException();
                    }
                    else
                    {
                        throw new TimeoutException();
                    }
                }
                Thread.Sleep(10);
            }

            serial.Read(rawBuffer, 0, rawBuffer.Length);
            byte[] response = new byte[5];
            if(rawBuffer[0] != 37)
            {
                response_offest++;
            }
            Array.Copy(rawBuffer, response_offest, response, 0, rawBuffer.Length - response_offest);

            if (response[0] != 0x06)
            {
                throw new LexiWriteException(response);
            }

            if(response[1] == 1)
            {
                return response;
            }
            else
            {
                byte[] full_response = new byte[25];
                Array.Copy(rawBuffer, response_offest, full_response, 0, rawBuffer.Length - response_offest);
                int moredata_to_read = 25 - (rawBuffer.Length - response_offest);
                Array.Resize(ref rawBuffer, moredata_to_read);
                while (serial.BytesReadCount() < moredata_to_read)
                {
                    if (DateTimeOffset.Now.ToUnixTimeMilliseconds() > timeout_limit)
                    {
                        //if even no data response no puck error..

                        if (serial.BytesReadCount() <= response_offest)
                        {
                            throw new IOException();
                        }
                        else
                        {
                            throw new TimeoutException();
                        }
                    }
                    Thread.Sleep(10);
                }
                serial.Read(rawBuffer, 0, rawBuffer.Length);
                Array.Copy(rawBuffer, 0 , full_response, 25 - moredata_to_read, moredata_to_read);

                return full_response; 
            }

        }

        public void GetLastLogQueryResult()
        {
            GetLastLogQueryResult(m_serial, m_timeout);
        }

        public void GetLastLogQueryResult(ISerial serial, int timeout)
        {
            List<byte> list = new List<byte>(5);
            list.Add(37);
            list.Add(254);
            list.Add(21);
            list.Add(0);
            list.Add(GetChecksum(list));

            byte[] stream = list.ToArray();

            serial.Write(stream, 0, stream.Length);

            byte[] rawBuffer = new byte[10];
            int response_offest = 0;
            if (serial.isEcho()) { response_offest = stream.Length; }
            Array.Resize(ref rawBuffer, (int)(response_offest + 5));

            long timeout_limit = DateTimeOffset.Now.ToUnixTimeMilliseconds() + (timeout);
            while (serial.BytesReadCount() < rawBuffer.Length)
            {
                if (DateTimeOffset.Now.ToUnixTimeMilliseconds() > timeout_limit)
                {
                    //if even no data response no puck error..

                    if (serial.BytesReadCount() <= response_offest)
                    {
                        throw new IOException();
                    }
                    else
                    {
                        throw new TimeoutException();
                    }
                }
                Thread.Sleep(10);
            }

            serial.Read(rawBuffer, 0, rawBuffer.Length);

            byte[] response = new byte[5];
            Array.Copy(rawBuffer, response.Length, response, 0, response.Length);

            if (response[0] != 0x06)
            {
                throw new LexiWriteException(response);
            }


        }

        private byte[] GetTimeSinceEventEpoch(DateTime time)
        {
            long value = (long)time.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            if (BitConverter.IsLittleEndian)
            {
                return BitConverter.GetBytes(value);
            }
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }
    }
}
