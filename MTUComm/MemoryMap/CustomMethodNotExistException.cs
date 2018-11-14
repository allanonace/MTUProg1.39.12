using System;

namespace MTUComm.MemoryMap
{
    public class CustomMethodNotExistException : Exception
    {
        public CustomMethodNotExistException()
        {
        }

        public CustomMethodNotExistException(string message) : base(message)
        {
        }

        public CustomMethodNotExistException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}