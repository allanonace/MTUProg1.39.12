using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Library.Exceptions;

namespace Xml
{
    /// <summary>
    /// Class used to map the Interface.xml file used to easily know which information
    /// to log, filtered by action type and running mode ( interactive or scripted ).
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="InterfaceConfig">
    /// <item>
    ///   <term>MtuInterfaces</term>
    ///   <description>List of <see cref="MtuInterface"/> entries</description>
    /// </item>
    /// <item>
    ///   <term>Interfaces</term>
    ///   <description>List of <see cref="Interface"/> entries</description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>&#160;</para>
    /// </summary>
    /// <remarks>
    /// NOTE: The values set in the constructor of the class are the default
    /// values that are used when a tag is not present in the configuration file.
    /// </remarks>
    [XmlRoot("InterfaceConfig")]
    public class InterfaceConfig
    {
        [XmlElement("MtuInterface")]
        public List<MtuInterface> MtuInterfaces { get; set; }

        [XmlElement("Interface")]
        public List<Interface> Interfaces { get; set; }

        public ActionInterface GetInterfaceByMtuIdAndAction ( Mtu mtu, string actionType )
        {
            Interface mtu_interface = Interfaces.Find ( x => x.Family.Equals ( mtu.GetFamily () ) );
            
            if ( mtu_interface == null )
                throw new ActionInterfaceNotFoundException_Internal ();

            ActionInterface action_interface = mtu_interface.GetInterfaceActionType ( actionType );
            
            if ( action_interface == null )
                throw new ActionInterfaceNotFoundException_Internal ();

            return action_interface;
        }
    }
}
