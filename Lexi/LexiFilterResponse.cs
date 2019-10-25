using System;

namespace Lexi
{
    public class LexiFilterResponse
    {
        public readonly int IndexByte;
        public readonly byte Value;

        public int ResponseBytes { get; set; }

        public LexiFilterResponse (
            int responseBytes,
            int indexByte,
            byte value )
        {
            this.ResponseBytes = responseBytes;
            this.IndexByte     = indexByte;
            this.Value         = value;
        }
    }
}
