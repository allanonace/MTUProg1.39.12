using System;

namespace MTUComm.MemoryMap
{
    public class MemoryRegisterNotExistException : Exception
    {
        public MemoryRegisterNotExistException()
        {
        }

        public MemoryRegisterNotExistException(string message) : base(message)
        {
        }

        public MemoryRegisterNotExistException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}