using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Xml
{
    /// <summary>
    /// Class used to map the Interface.xml file used to easily know which information
    /// to log, filtered by action type and running mode ( interactive or scripted ).
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="Port">
    /// <item>
    ///   <term>UserName</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>LogFile</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Actions</term>
    ///   <description>List of <see cref="ScriptAction"/> entries</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>&#160;</para>
    /// </summary>
    /// <remarks>
    /// NOTE: The values set in the constructor of the class are the default
    /// values that are used when a tag is not present in the configuration file.
    /// </remarks>
    [XmlRoot("MtuScript")]
    public class Script
    {
        [XmlElement("userName")]
        public string UserName { get; set; }

        [XmlElement("logFile")]
        public string LogFile { get; set; }

        [XmlElement("action")]
        public List<ScriptAction> Actions { get; set; }
    }
}
