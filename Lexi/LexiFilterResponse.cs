using System;

namespace Lexi
{
    public class LexiFilterResponse
    {
        public int ResponseBytes;
        public readonly int IndexByte;
        public readonly byte Value;

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
