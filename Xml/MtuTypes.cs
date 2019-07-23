using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    /// <summary>
    /// Class used to map the Mtu.xml configuration file.
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="MtuTypes">
    /// <item>
    ///   <term>FileVersion</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>FileDate</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Mtus</term>
    ///   <description>List of <see cref="Mtu"/> entries</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>&#160;</para>
    /// </summary>
    /// <remarks>
    /// NOTE: The values set in the constructor of the class are the default
    /// values that are used when a tag is not present in the configuration file.
    /// </remarks>
    /// <seealso cref="Port"/>
    [XmlRoot("MtuTypes")]
    public class MtuTypes
    {
        [XmlElement("FileVersion")]
        public string FileVersion { get; set; }

        [XmlElement("FileDate")]
        public string FileDate { get; set; }

        [XmlElement("Mtu")]
        public List<Mtu> Mtus { get; set; }

        public Mtu FindByMtuId(int mtuId)
        {
            return Mtus.Find ( x => x.Id == mtuId );
        }
    }
}
