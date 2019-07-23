using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Library.Exceptions;

namespace Xml
{
    /// <summary>
    /// Class used to map the Alarm.xml configuration file.
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="AlarmList">
    /// <item>
    ///   <term>FileVersion</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>FileDate</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Customer</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Alarms</term>
    ///   <description>List of <see cref="Alarm"/> entries</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>&#160;</para>
    /// </summary>
    /// <remarks>
    /// NOTE: The values set in the constructor of the class are the default
    /// values that are used when a tag is not present in the configuration file.
    /// </remarks>
    [XmlRoot("Alarms")]
    public class AlarmList
    {
        [XmlElement("FileVersion")]
        public string FileVersion { get; set; }

        [XmlElement("FileDate")]
        public string FileDate { get; set; }
 
        [XmlElement("Customer")]
        public string Customer { get; set; }

        [XmlElement("Alarm")]
        public List<Alarm> Alarms { get; set; }

        public List<Alarm> FindByMtuType(int mtuType)
        {
            List<Alarm> alarms = Alarms.FindAll ( x => x.MTUType == mtuType );

            if ( alarms == null )
                throw new AlarmNotFoundException_Internal ();

            return alarms;
        }
    }
}
