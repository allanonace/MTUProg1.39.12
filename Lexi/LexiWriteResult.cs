using System;

namespace Lexi
{
    public class LexiWriteResult
    {
        public readonly byte[] Bytes; // Echo + ACKx2 [ + Response/Result ]
        public readonly int    ResponseOffset; // Echo length

        public byte[] Response // ACKx2 + Response/Result
        {
            get
            {
                byte[] response = new byte[ Bytes.Length - ResponseOffset ];
                Array.Copy ( Bytes, ResponseOffset, response, 0, response.Length );

                return response;
            }
        }

        public byte[] ResponseOnlyData
        {
            get
            {
                byte[] response = new byte[ Bytes.Length - ResponseOffset - 4 ]; // - ACKx2 - CRCx2
                Array.Copy ( Bytes, ResponseOffset + 2, response, 0, response.Length );

                return response;
            }
        }

        public LexiWriteResult (
            byte[] bytes,
            int responseOffset )
        {
            this.Bytes = bytes;
            this.ResponseOffset = responseOffset;
        }
    }
}
