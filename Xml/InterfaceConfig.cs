using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("InterfaceConfig")]
    public class InterfaceConfig
    {
        public static int currentIndexType;
    
        [XmlElement("MtuInterface")]
        public List<MtuInterface> MtuInterfaces { get; set; }

        [XmlElement("Interface")]
        public List<Interface> Interfaces { get; set; }

        public ActionInterface GetInterfaceByMtuIdAndAction ( Mtu mtu, string actionType )
        {
            Interface mtu_interface = Interfaces.Find ( x => x.Id == currentIndexType );
            if (mtu_interface == null)
                throw new InterfaceNotFoundException("Meter not found");

            ActionInterface action_interface = mtu_interface.GetInterfaceActionType ( actionType );
            if (action_interface == null)
                throw new ActionInterfaceNotFoundException("Meter not found");

            return action_interface;
        }
    }
}
