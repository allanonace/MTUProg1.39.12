using System;
using System.Linq;
using Library;

using NodeType = Lexi.Lexi.NodeType;

namespace MTUComm
{
    public class NodeDiscovery
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
        * | 20         | Respoder ID   | Subcomponent ID of a Node that received the request  |
        * | 21..22     | Freq. Ch. Rq. | Freq. channel on which the request was received      |
        * | 23         | Noise Floor   | Responding node noise floor estimate                 |
        * +------------+---------------+------------------------------------------------------+
        * The first message should be and ACK with discovery general information
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
        
        private int         index;
        private int         totalEntries;
        private NodeType    nodeType;
        private int         nodeId;
        private int         rssiRequest;
        private int         rssiResponse;
        private int         freqErrorRequest;
        private int         freqErrorResponse;
        private int         timeDeltaRequest;
        private int         timeDeltaResponse;
        private int         responderId;
        private int         freqChannelRequest;
        private int         freqChannelResponse;
        private int         noiseFloorRequest;
        private int         noiseFloorResponse;
        private bool        validated;

        #endregion

        #region Properties

        public int Index
        {
            get { return this.index; }
        }
        
        public int TotalEntries
        {
            get { return this.totalEntries; }
        }

        public bool IsLast
        {
            get { return this.index >= this.totalEntries; }
        }

        public NodeType NodeType
        {
            get { return this.nodeType; }
        }

        public int NodeId
        {
            get { return this.nodeId; }
        }

        public int RSSIRequest
        {
            get { return this.rssiRequest; }
        }

        public int RSSIResponse
        {
            get { return this.rssiResponse; }
        }

        public int FreqErrorRequest
        {
            get { return this.freqErrorRequest; }
        }

        public int FreqErrorResponse
        {
            get { return this.freqErrorResponse; }
        }

        public int TimeDeltaRequest
        {
            get { return this.timeDeltaRequest; }
        }

        public int TimeDeltaResponse
        {
            get { return this.timeDeltaResponse; }
        }

        public int ResponderId
        {
            get { return this.responderId; }
        }

        public int FreqChannelRequest
        {
            get { return this.freqChannelRequest; }
        }

        public int FreqChannelResponse
        {
            get { return this.freqChannelResponse; }
        }

        public int NoiseFloorRequest
        {
            get { return this.noiseFloorRequest; }
        }

        public int NoiseFloorResponse
        {
            get { return this.noiseFloorResponse; }
        }

        public bool IsValidated
        {
            get { return this.validated; }
        }

        #endregion

        #region Initialization

        public NodeDiscovery (
            byte[] response )
        {
            this.totalEntries  = ( int )response[ BYTE_NUMRESULTS ]; // 3
            this.index         = ( int )response[ BYTE_CURRENT    ]; // 4

            // ACK with Node Discovery General Information
            if ( this.index == 1 )
            {
                this.freqChannelResponse = Utils.GetNumericValueFromBytes<int> ( response, BYTE_FREQCHRES, NUM_BYTES_FREQCHRES ); // 5 and 6
                this.noiseFloorRequest   = ( int )response[ BYTE_NOISEREQ ];                                                      // 7
            }
            // ACK with Node Discovery Response
            else
            {
                this.nodeType            = ( NodeType )response[ BYTE_TYPE ];                                                            // 5
                this.nodeId              = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_NODEID, NUM_BYTES_NODEID );             // 6, 7, 8 and 9
                this.rssiRequest         = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_RSSIREQ, NUM_BYTES_RSSIREQ );           // 10 and 11
                this.freqErrorRequest    = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_FREQERRORREQ, NUM_BYTES_FREQERRORREQ ); // 12 and 13
                this.rssiResponse        = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_RSSIRES, NUM_BYTES_RSSIRES );           // 14 and 15
                this.freqErrorResponse   = Utils.GetNumericValueFromBytes<int>  ( response, BYTE_FREQERRORRES, NUM_BYTES_FREQERRORRES ); // 16 and 17
                this.timeDeltaRequest    = ( int )response[ BYTE_TIMEREQ ];                                                              // 18
                this.timeDeltaResponse   = ( int )response[ BYTE_TIMERES ];                                                              // 19
                this.responderId         = ( int )response[ BYTE_RESID   ];                                                              // 20
                this.freqChannelRequest  = Utils.GetNumericValueFromBytes<int> ( response, BYTE_FREQCHREQ, NUM_BYTES_FREQCHREQ );        // 21 and 22
                this.noiseFloorResponse  = ( int )response[ BYTE_NOISERES ];                                                             // 23
            }
        }

        #endregion

        public void SetAsValidated ()
        {
            this.validated = true;
        }
    }
}
