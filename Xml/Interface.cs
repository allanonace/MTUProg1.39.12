using System.Collections.Generic;
using System.Xml.Serialization;
using Library.Exceptions;

namespace Xml
{
    public class Interface
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlAttribute("memorymap")]
        public string Memorymap { get; set; }

        [XmlAttribute("memorysize")]
        public int MemorymapSize { get; set; }

        [XmlElement("Action")]
        public List<ActionInterface> Actions { get; set; }

        public ActionInterface GetInterfaceActionType ( string actionType )
        {
            return Actions.Find ( x => x.Type.ToLower ().Equals ( actionType.ToLower () ) );
        }
    }
}
