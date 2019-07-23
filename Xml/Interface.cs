using System.Collections.Generic;
using System.Xml.Serialization;
using Library.Exceptions;

namespace Xml
{
    /// <summary>
    /// Class used to map the 'Interface' entries present in the the Interface.xml file.
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="Interface">
    /// <item>
    ///   <term>Id</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Memorymap</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>MemorymapSize</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Actions</term>
    ///   <description>List of ActionInterface entries</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>&#160;</para>
    /// </summary>
    /// <remarks>
    /// NOTE: The values set in the constructor of the class are the default
    /// values that are used when a tag is not present in the configuration file.
    /// </remarks>
    /// <seealso cref="InterfaceConfig"/>
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
