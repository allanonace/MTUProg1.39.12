using System.Xml.Serialization;
using System.ComponentModel;

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
        private const int DEF_ADDRESS = 0;
        private const int DEF_SIZE = 1;

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
                    else this.Size = DEF_SIZE;
                }
                else this.Size = DEF_SIZE;
            }
        }

        [XmlElement("Custom")]
        public string Custom { get; set; }
    }
}
