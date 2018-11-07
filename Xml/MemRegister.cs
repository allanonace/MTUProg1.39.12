using System.Xml.Serialization;

namespace Xml
{
    /// <summary>
    /// Deserializing default attribute is not used, only during serializing,
    /// for that reason integer elements launch exception when field is empty
    /// The workaround to solve this problem is to manage element as string
    /// using second property, marking main property to be ignore during XML
    /// processing, and if field is empty or value is not parsed ok to integer,
    /// default constant value is assigned to the element
    /// </summary>
    public class MemRegister
    {
        public  const int    DEF_ADDRESS = 0;
        public  const int    DEF_SIZE    = 1;
        public  const int    DEF_BIT     = 0;
        public  const bool   DEF_WRITE   = false;
        private const string STR_BOOL    = "bool";

        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlIgnore]
        public int Address { get; set; }

        [XmlElement("Address")]
        public string AddressAllowEmptyField
        {
            get { return this.Address.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int v;
                    if (int.TryParse(value, out v))
                        this.Address = v;
                    else this.Address = DEF_ADDRESS;
                }
                else this.Address = DEF_ADDRESS;
            }
        }

        [XmlIgnore]
        public int Size { get; set; }

        [XmlElement("Size")]
        public string SizeAllowEmptyField
        {
            get { return this.Size.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int v;
                    if (int.TryParse(value, out v))
                         this.Size = v;
                    else this.Size = ( string.Equals ( this.Type, STR_BOOL ) ) ? DEF_BIT : DEF_SIZE;
                }
                else this.Size = ( string.Equals ( this.Type, STR_BOOL ) ) ? DEF_BIT : DEF_SIZE;
            }
        }

        [XmlIgnore]
        public bool Write { get; set; }

        [XmlElement("Write")]
        public string WriteAllowEmptyField
        {
            get { return this.Write.ToString().ToLower (); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    bool v;
                    if (bool.TryParse(value, out v))
                         this.Write = v;
                    else this.Write = DEF_WRITE;
                }
                else this.Write = DEF_WRITE;
            }
        }

        [XmlElement("Custom")]
        public string Custom { get; set; }
    }
}
