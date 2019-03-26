using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    public class ActionInterface
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("Parameter")]
        public List<InterfaceParameters> Parameters { get; set; }

        public InterfaceParameters[] getLogInterfaces()
        {
            List<InterfaceParameters> copyParameters = new List<InterfaceParameters> ();
            
            // Copy filtered parents
            Parameters.FindAll ( x => x.Log ).ForEach ( iParam => copyParameters.Add ( iParam.Clone () as InterfaceParameters ) );
            
            // Copy filtered childs
            foreach ( InterfaceParameters copyParameter in copyParameters )
                copyParameter.Parameters.RemoveAll ( x => ! x.Log );
            
            return copyParameters.ToArray ();
        }

        public InterfaceParameters[] getUserInterfaces()
        {
            List<InterfaceParameters> copyParameters = new List<InterfaceParameters> ();
            
            // Copy filtered parents
            Parameters.FindAll ( x => x.Interface ).ForEach ( iParam => copyParameters.Add ( iParam.Clone () as InterfaceParameters ) );
            
            // Copy filtered childs
            foreach ( InterfaceParameters copyParameter in copyParameters )
                copyParameter.Parameters.RemoveAll ( x => ! x.Interface );
            
            return copyParameters.ToArray ();
        }

        public InterfaceParameters[] getAllInterfaces()
        {
            return Parameters.ToArray ();
        }

    }
}
