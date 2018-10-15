using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("Memories")]
    public class Memories
    {

        [XmlElement("Memory")]
        public List<Memory> MemoryMaps { get; set; }

        [XmlElement("MtuMemory")]
        public List<MtuMemory> MtuMemories { get; set; }


        public List<Memory> FindByMtuID(int mtu_id)
        {

            List<MtuMemory> mtu_memories = MtuMemories.FindAll(x => (x.Id == mtu_id));
            if (mtu_memories == null)
            {
                throw new MemoryMapNotFoundException("Mtu not found");
            }

            List<Memory> memories = MemoryMaps.FindAll(x => (x.Id == mtu_memories[0].Memory));
            if (memories == null)
            {
                throw new MemoryMapNotFoundException("Memory Map not found");
            }

            return memories;
        }
    }

    
}
