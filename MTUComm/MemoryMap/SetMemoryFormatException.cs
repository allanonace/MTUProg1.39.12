using System;

namespace MTUComm.MemoryMap
{
    public class SetMemoryFormatException : Exception
    {
        public SetMemoryFormatException ()
        {
        }

        public SetMemoryFormatException(string message) : base(message)
        {
        }

        public SetMemoryFormatException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
