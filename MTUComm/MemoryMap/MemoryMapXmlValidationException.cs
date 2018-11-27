using System;

namespace MTUComm.MemoryMap
{
    public class MemoryMapXmlValidationException : Exception
    {
        public MemoryMapXmlValidationException()
        {
        }

        public MemoryMapXmlValidationException(string message) : base(message)
        {
        }

        public MemoryMapXmlValidationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}