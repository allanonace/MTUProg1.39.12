using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace Xml
{
    public class InterfaceParameters// : ICloneable
    {
        public InterfaceParameters ()
        {
            this.Source = string.Empty;
            this.Length = string.Empty;
            this.Fill   = string.Empty;
            this.Format = string.Empty;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("display")]
        public string Display { get; set; }

        [XmlAttribute("log")]
        public bool Log { get; set; }

        [XmlAttribute("interface")]
        public bool Interface { get; set; }

        [XmlAttribute("conditional")]
        public string Conditional { get; set; }

        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("length")]
        public string Length { get; set; }

        [XmlAttribute("fill")]
        public string Fill { get; set; }

        [XmlAttribute("format")]
        public string Format { get; set; }

        [XmlText]
        public string Value { get; set; }

        [XmlElement("Parameter")]
        public List<InterfaceParameters> Parameters { get; set; }

        public bool HasFormat
        {
            get { return ! string.IsNullOrEmpty ( this.Format ); }
        }

        public object Clone ()
        {
            InterfaceParameters copy = this.MemberwiseClone () as InterfaceParameters;
            copy.Parameters = new List<InterfaceParameters> ();
            
            foreach ( InterfaceParameters child in this.Parameters )
                copy.Parameters.Add ( child.Clone () as InterfaceParameters );
            
            return copy;
        }
    }
}
