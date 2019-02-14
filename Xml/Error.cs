using System;
using System.Xml.Serialization;

namespace Xml
{
    public class Error : ICloneable
    {
        private const int EMPTY_VAL = -1;
    
        public Error ()
        {
            this.Port     = 1;
            this.Id       = EMPTY_VAL;
            this.DotNetId = EMPTY_VAL;
        }
        
        public Error ( string message )
        : this ()
        {
            this.Message = message;
        }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlIgnore]
        public int DotNetId { get; set; }
        
        [XmlAttribute("dotnet")]
        public string DotNetId_AllowEmptyField
        {
            get { return this.DotNetId.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int v;
                    if (int.TryParse(value, out v))
                         this.DotNetId = v;
                    else this.DotNetId = EMPTY_VAL;
                }
                else this.DotNetId = EMPTY_VAL;
            }
        }

        [XmlAttribute("message")]
        public string Message { get; set; }
        
        public int Port;
        public Exception Exception;

        public object Clone ()
        {
            return this.MemberwiseClone ();
        }
    }
}
