using System;
using System.Linq;
using Library;

using NodeType = Lexi.Lexi.NodeType;

namespace MTUComm
{
    /// <summary>
    /// Node or DCU unit detected during the <see cref="MTUComm.NodeDiscovery"/> process.
    /// </summary>
    public class NodeDiscovery : ICloneable
    {
        /*
        * +------------+---------------+------------------------------------------------------+
        * | Byte Index |  Field        |                          Notes                       |
        * +------------+---------------+------------------------------------------------------+
        * | 0          | ACK           | 0x06 Operation successful                            |
        * | 1          | ACK Info Size | 0x16 ( 22 ) bytes of data if Result 0, otherwise 1   |
        * | 2          | Result        | 0 = Data included, 1 = Complete and No more data     |
        * | 3          | Num. Results  | Total number of node discovery responses             |--+
        * | 4          | Current Item  | Current response number ( 1 based )                  |  |--- Result with log entry
        * | 5..23      | Data          | Data bytes fog the log item                          |--+
        * | 24..25     | CRC           | Byte 0: 0x06 ACK, 0x15 NAK                           |
        * +------------+---------------+------------------------------------------------------+
        * Data..
        * +------------+---------------+------------------------------------------------------+
        * | Byte Index |  Field        |                          Notes                       |
        * +------------+---------------+------------------------------------------------------+
        * | 5          | Node Type     | Responding node type , NodeType                      |
        * | 6..9       | Node ID       | Responding node ID                                   |
        * | 10..11     | RSSI Request  | RSSI of Node Discovery                               |
        * | 12..13     | Freq. Er. Rq. | Freq. Error of Node Discovery                        |
        * | 14..15     | RSSI Response | RSSI of Node Response                                |
        * | 16..17     | Freq. Er. Rs. | Freq. Error of Node Discovery                        |
        * | 18         | TimeDelta Rq. | Time delta between req. and responder's RTC clock    |
        * | 19         | TimeDelta Rs. | Time delta between resp. and requestor's RTC clock   |
        * | 20         | Responder ID  | Subcomponent ID of a Node that received the request  |
        * | 21..22     | Freq. Ch. Rq. | Freq. channel on which the request was received      |
        * | 23         | Noise Floor   | Responding node noise floor estimate                 |
        * +------------+---------------+------------------------------------------------------+
        * The first message should be an ACK with discovery general information
        * +------------+---------------+------------------------------------------------------+
        * | Byte Index |  Field        |                          Notes                       |
        * +------------+---------------+------------------------------------------------------+
        * | 0          | ACK           | 0x06 Operation successful                            |
        * | 1          | ACK Info Size | 0x6 ( 6 )                                            |
        * | 2          | Result        | 0 = Data included, 1 = Complete and No more data     |
        * | 3          | Num. Results  | Total number of node discovery responses             |
        * | 4          | Current Item  | 1                                                    |
        * | 5..6       | Freq. Ch. Rs. | Freq. channel of the requestor node receiving freq.  |
        * | 7          | Noise Floor   | Requestor node measured noise floor estimate         |
        * | 8..9       | CRC           | Byte 0: 0x06 ACK, 0x15 NAK                           |
        * +------------+---------------+------------------------------------------------------+
        */

        #region Constants

        // Maximum lengths
        public  const int BYTES_REQUIRED_DATA_1  = 10;
        public  const int BYTES_REQUIRED_DATA_2  = 26;

        // ACK with Node Discovery Response
        private const int BYTE_RESULT            = 2;
        private const int BYTE_NUMRESULTS        = 3;
        private const int BYTE_CURRENT           = 4;
        private const int BYTE_TYPE              = 5;
        private const int BYTE_NODEID            = 6; // and 7, 8, 9
        private const int NUM_BYTES_NODEID       = 4;
        private const int BYTE_RSSIREQ           = 10; // and 11
        private const int NUM_BYTES_RSSIREQ      = 2;
        private const int BYTE_FREQERRORREQ      = 12; // and 13
        private const int NUM_BYTES_FREQERRORREQ = 2;
        private const int BYTE_RSSIRES           = 14; // and 15
        private const int NUM_BYTES_RSSIRES      = 2;
        private const int BYTE_FREQERRORRES      = 16; // and 17
        private const int NUM_BYTES_FREQERRORRES = 2;
        private const int BYTE_TIMEREQ           = 18;
        private const int BYTE_TIMERES           = 19;
        private const int BYTE_RESID             = 20;
        private const int BYTE_FREQCHREQ         = 21; // and 22
        private const int NUM_BYTES_FREQCHREQ    = 2;
        private const int BYTE_NOISERES          = 23;

        // ACK with Node Discovery General Information
        private const int BYTE_FREQCHRES         = 5; // and 6
        private const int NUM_BYTES_FREQCHRES    = 2;
        private const int BYTE_NOISEREQ          = 7;

        #endregion

        #region Attributes
        
        private bool        isF1;
        private byte        index;
        private byte        totalEntries;
        private NodeType    nodeType;
        private uint        nodeId;
        private short       rssiRequest;            // MTU -> DCU
        private short       rssiResponse;           // DCU -> MTU
        private short       freqErrorRequest;
        private short       freqErrorResponse;
        private sbyte       timeDeltaRequest;
        private sbyte       timeDeltaResponse;
        private byte        responderId;
        private ushort      freqChannelRequest;
        private ushort      freqChannelResponse;
        private byte        noiseFloorRequest;
        private byte        noiseFloorResponse;
        private bool        validated;

        #endregion

        #region Properties

        /// <summary>
        /// Index of the node/DCU in the list of units retrieved during the NodeDiscovery process.
        /// </summary>
        /// <remarks>
        /// LExI command "GET NEXT NODE DISCOVERY RESPONSE": Second Response ( ACK with Node Discovery Response )
        /// </remarks>
        public uint Index
        {
            get { return this.index; }
        }
        
        public bool IsF1
        {
            get { return this.isF1; }
        }

        public bool IsF2
        {
            get { return ! this.isF1; }
        }

        /// <summary>
        /// The total number of units detected in the NodeDiscovery process.
        /// </summary>
        /// <remarks>
        /// LExI command "GET NEXT NODE DISCOVERY RESPONSE": First response ( ACK with Node Discovery General Information )
        /// </remarks>
        public uint TotalEntries
        {
            get { return this.totalEntries; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLast
        {
            get { return this.index >= this.totalEntries; }
        }

        public NodeType NodeType
        {
            get { return this.nodeType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public uint NodeId
        {
            get { return this.nodeId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public short RSSIRequest
        {
            get { return this.rssiRequest; }
        }

        /// <summary>
        /// 
        /// </summary>
        public short RSSIResponse
        {
            get { return this.rssiResponse; }
        }

        /// <summary>
        /// 
        /// </summary>
        public short FreqErrorRequest
        {
            get { return this.freqErrorRequest; }
        }

        /// <summary>
        /// 
        /// </summary>
        public short FreqErrorResponse
        {
            get { return this.freqErrorResponse; }
        }

        /// <summary>
        /// 
        /// </summary>
        public sbyte TimeDeltaRequest
        {
            get { return this.timeDeltaRequest; }
        }

        /// <summary>
        /// 
        /// </summary>
        public sbyte TimeDeltaResponse
        {
            get { return this.timeDeltaResponse; }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte ResponderId
        {
            get { return this.responderId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort FreqChannelRequest
        {
            get { return this.freqChannelRequest; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort FreqChannelResponse
        {
            get { return this.freqChannelResponse; }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte NoiseFloorRequest
        {
            get { return this.noiseFloorRequest; }
        }

        /// <summary>
        /// 
        /// </summary>
        public byte NoiseFloorResponse
        {
            get { return this.noiseFloorResponse; }
        }

        /// <summary>
        /// Indicates whether the node/DCU is already validated or not.
        /// <para>
        /// See <see cref="SetAsValidated"/> to set a node/DCU validated.
        /// </para>
        /// </summary>
        public bool IsValidated
        {
            get { return this.validated; }
        }

        #endregion

        #region Initialization

        public NodeDiscovery (
            byte[] response )
        {
            this.totalEntries  = ( byte )response[ BYTE_NUMRESULTS ]; // 3
            this.index         = ( byte )response[ BYTE_CURRENT    ]; // 4

            // ACK with Node Discovery General Information
            if ( this.index == 1 )
            {
                this.freqChannelResponse = Utils.ConvertToNumericFromBytes<ushort> ( response, BYTE_FREQCHRES, NUM_BYTES_FREQCHRES ); // 5 and 6
                this.noiseFloorRequest   = ( byte )response[ BYTE_NOISEREQ ];                                                         // 7
            }
            // ACK with Node Discovery Response
            else
            {
                this.nodeType           = ( NodeType )response[ BYTE_TYPE ];                                                              // 5
                this.nodeId             = Utils.ConvertToNumericFromBytes<uint>  ( response, BYTE_NODEID, NUM_BYTES_NODEID );             // 6, 7, 8 and 9
                this.rssiRequest        = Utils.ConvertToNumericFromBytes<short> ( response, BYTE_RSSIREQ, NUM_BYTES_RSSIREQ );           // 10 and 11
                this.freqErrorRequest   = Utils.ConvertToNumericFromBytes<short> ( response, BYTE_FREQERRORREQ, NUM_BYTES_FREQERRORREQ ); // 12 and 13
                this.rssiResponse       = Utils.ConvertToNumericFromBytes<short> ( response, BYTE_RSSIRES, NUM_BYTES_RSSIRES );           // 14 and 15
                this.freqErrorResponse  = Utils.ConvertToNumericFromBytes<short> ( response, BYTE_FREQERRORRES, NUM_BYTES_FREQERRORRES ); // 16 and 17
                this.timeDeltaRequest   = ( sbyte )response[ BYTE_TIMEREQ ];                                                              // 18
                this.timeDeltaResponse  = ( sbyte )response[ BYTE_TIMERES ];                                                              // 19
                this.responderId        = ( byte  )response[ BYTE_RESID   ];                                                              // 20
                this.freqChannelRequest = Utils.ConvertToNumericFromBytes<ushort> ( response, BYTE_FREQCHREQ, NUM_BYTES_FREQCHREQ );      // 21 and 22
                this.noiseFloorResponse = ( byte  )response[ BYTE_NOISERES ];                                                             // 23

                // Converts the RSSI values ​​to the desired range
                //this.rssiRequest  = Math.Abs ( this.rssiRequest  / 2 - 130 );
                //this.rssiResponse = Math.Abs ( this.rssiResponse / 2 - 130 );
            }
        }

        #endregion

        /// <summary>
        /// Marks the node as detected and validated, verifying that the channel or transmission frequency is
        /// equal to the one way ( unidirectional ) or two way ( bidirectional ) frequency set in the MTU memory map.
        /// </summary>
        public void SetAsValidated (
            bool isF1 )
        {
            this.validated = true;
            this.isF1      = isF1;
        }
    
        public object Clone ()
        {
            // All attributes are by value
            return this.MemberwiseClone ();
        }
    }
}
