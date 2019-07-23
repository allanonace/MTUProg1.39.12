using System;

namespace Lexi
{
    public class LexiWriteResult
    {
        public readonly byte[] Bytes;
        public readonly int    ResponseOffset;

        public LexiWriteResult (
            byte[] bytes,
            int responseOffset )
        {
            this.Bytes = bytes;
            this.ResponseOffset = responseOffset;
        }
    }
}
