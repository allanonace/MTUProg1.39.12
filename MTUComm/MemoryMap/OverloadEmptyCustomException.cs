using System;

namespace MTUComm.MemoryMap
{
    public class OverloadEmptyCustomException : Exception
    {
        public OverloadEmptyCustomException()
        {
        }

        public OverloadEmptyCustomException(string message) : base(message)
        {
        }

        public OverloadEmptyCustomException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}