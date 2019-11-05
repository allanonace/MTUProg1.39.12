using System;
using System.Linq;
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

        public List<Demand> FindByMtuType (
            int mtuType )
        {
            List<Demand> demands = Demands.FindAll ( x => x.MTUType == mtuType );
            
            if ( demands == null ||
                 demands.Count <= 0 )
                throw new DemandNotFoundException_Internal ();
                
            return demands;
        }

        public Demand FindByMtuTypeAndName (
            int mtuType,
            string name )
        {
            return this.FindByMtuType ( mtuType )
                .Find ( a => a.Name.ToLower ().Equals ( name.ToLower () ) );
        }

        public Demand[] FindByMtuType_Interactive (
            int mtuType )
        {
            List<Demand> demands = this.FindByMtuType ( mtuType );

            if ( demands.Count > 0 )
                return demands.Where ( a => ! string.Equals ( a.Name.ToLower (), "scripting" ) ).ToArray ();
            
            return default ( Demand[] );
        }

        public Demand FindByMtuType_Scripting (
            int mtuType )
        {
            return this.FindByMtuType ( mtuType )
                .Find ( a => string.Equals ( a.Name.ToLower (), "scripting" ) );
        }
    }
}
