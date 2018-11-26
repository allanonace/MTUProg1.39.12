using System;

namespace MTUComm.MemoryMap
{
    public class MemoryMapParseXmlException : Exception
    {
        public MemoryMapParseXmlException()
        {
        }

        public MemoryMapParseXmlException(string message) : base(message)
        {
        }

        public MemoryMapParseXmlException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}