using System;
using Xml;

using RegType = MTUComm.MemoryMap.MemoryMap.RegType;

namespace MTUComm.MemoryMap
{
    public class MemoryRegister<T>
    {
        public Func<T> funcGet;
        public Action<T> funcSet;
        public string id { get; }
        public string description { get; }
        public RegType type { get; }
        public int address { get; }
        public int size { get; }
        public bool write { get; }
        public string custom { get; }
        public bool used;

        // Value size ( number of consecutive bytes ) is also used for bit with bool type
        public int bit { get { return this.size; } }

        public MemoryRegister(
            string id,
            RegType type,
            string description,
            int address,
            int size = MemRegister.DEF_SIZE,
            bool write = MemRegister.DEF_WRITE,
            string custom = "" )
        {
            this.id = id;
            this.type = type;
            this.description = description;
            this.address = address;
            this.size = size;
            this.custom = custom;
        }

        public T Value
        {
            get
            {
                Console.WriteLine("RegisterObj: Get");

                return (T)this.funcGet();
            }
            set
            {
                Console.WriteLine("RegisterObj: Set");

                this.funcSet((T)value);
            }
        }
    }
}
