using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    /// <summary>
    /// Class used to map the 'MtuInterface' entries present in the the Interface.xml file.
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="MtuInterface">
    /// <item>
    ///   <term>Id</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Interface</term>
    ///   <description></description>
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
    public class MtuInterface
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlAttribute("interface")]
        public int Interface { get; set; }
    }
}
