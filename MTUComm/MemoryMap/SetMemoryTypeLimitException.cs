using System;

namespace MTUComm.MemoryMap
{
    public class SetMemoryTypeLimitException : Exception
    {
        public SetMemoryTypeLimitException()
        {
        }

        public SetMemoryTypeLimitException(string message) : base(message)
        {
        }

        public SetMemoryTypeLimitException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}