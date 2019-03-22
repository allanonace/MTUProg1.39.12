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
            List<InterfaceParameters> parameters = Parameters.FindAll(x => x.Log );
            
            List<InterfaceParameters> portParams;
            foreach ( InterfaceParameters iParam in parameters )
            {
                portParams = iParam.Parameters;
            
                for ( int i = portParams.Count - 1; i >= 0; i-- )
                    if ( ! portParams[ i ].Log )
                        portParams.RemoveAt ( i );
            }
            
            return parameters.ToArray();
        }

        public InterfaceParameters[] getUserInterfaces()
        {
            List<InterfaceParameters> parameters = Parameters.FindAll(x => x.Interface );
            
            List<InterfaceParameters> portParams;
            foreach ( InterfaceParameters iParam in parameters )
            {
                portParams = iParam.Parameters;
            
                for ( int i = portParams.Count - 1; i >= 0; i-- )
                    if ( ! portParams[ i ].Interface )
                        portParams.RemoveAt ( i );
            }
            
            return parameters.ToArray();
        }

        public InterfaceParameters[] getAllInterfaces()
        {
            return Parameters.ToArray ();
        }

    }
}
