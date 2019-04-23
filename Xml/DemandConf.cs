using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Library.Exceptions;

namespace Xml
{
    [XmlRoot("DemandConf")]
    public class DemandConf
    {
        [XmlElement("Demand")]
        public List<Demand> Demands { get; set; }

        public List<Demand> FindByMtuType(int mtuType)
        {
            List<Demand> demands = Demands.FindAll ( x => x.MTUType == mtuType );
            
            if ( demands == null )
                throw new DemandNotFoundException_Internal ();
                
            return demands;
        }
    }
}
