using System.Xml.Serialization;
using Library;

namespace Xml
{
    /*
     Deserializing default attribute is not used, only during serializing,
     for that reason integer elements launch exception when field is empty
     The workaround to solve this problem is to manage element as string
     using second property, marking main property to be ignore during XML
     processing, and if field is empty or value is not parsed ok to integer,
     default constant value is assigned to the element
    */
    public class MemRegister
    {
        public  const bool   DEF_WRITE   = false;
        private const int    BOOL_TRUE   = 1;
        private const int    BOOL_FALSE  = 0;
        //public  const int    DEF_ADDRESS = 0;
        public  const int    DEF_SIZE    = 1;
        public  const int    DEF_BIT     = 0;

        public MemRegister ()
        {
            this.SizeGet = 1;
        }

        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlIgnore]
        public int Address { get; set; }

        [XmlElement("Address")]
        public string Address_AllowEmptyField
        {
            get { return this.Address.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int v;
                    if (int.TryParse(value, out v))
                         this.Address = v;
                    else this.Address = Validations.ERROR_VAL; //DEF_ADDRESS;
                }
                else this.Address = Validations.ERROR_VAL; //DEF_ADDRESS;
            }
        }

        [XmlIgnore]
        public int Size { get; set; }

        [XmlElement("Size")]
        public string Size_AllowEmptyField
        {
            get { return this.Size.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int v;
                    if (int.TryParse(value, out v))
                         this.Size = v;
                    else this.Size = Validations.ERROR_VAL; //( string.Equals ( this.Type, STR_BOOL ) ) ? DEF_BIT : DEF_SIZE;
                }
                else this.Size = Validations.ERROR_VAL; //( string.Equals ( this.Type, STR_BOOL ) ) ? DEF_BIT : DEF_SIZE;
            }
        }
        
        [XmlIgnore]
        public int SizeGet { get; set; }

        [XmlElement("SizeGet")]
        public string SizeGet_AllowEmptyField
        {
            get { return this.SizeGet.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int v;
                    if (int.TryParse(value, out v))
                         this.SizeGet = v;
                    else this.SizeGet = 1;
                }
                else this.SizeGet = 1;
            }
        }

        // To can validate this element is mandatory to use some type of data that
        // allows to set more than two values, using one of them to marks the element
        // as not setted and fails XML validation
        [XmlIgnore]
        public int Write { get; set; }

        [XmlIgnore]
        public bool WriteAsBool
        {
            get { return ( this.Write == BOOL_TRUE ); }
        }

        [XmlElement("Write")]
        public string Write_AllowEmptyField
        {
            get { return this.WriteAsBool.ToString().ToLower (); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    bool v;
                    if (bool.TryParse ( value, out v ) )
                         this.Write = ( v ) ? BOOL_TRUE : BOOL_FALSE;
                    else this.Write = Validations.ERROR_VAL;
                }
                else this.Write = Validations.ERROR_VAL;
            }
        }

        [XmlElement("CustomGet",IsNullable=true)]
        public string Custom_Get { get; set; }

        [XmlElement("CustomSet",IsNullable=true)]
        public string Custom_Set { get; set; }
    }
}
