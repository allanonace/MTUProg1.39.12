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

        private string message;
        private string messageDebug;
        private string messageFooter;
        private string messagePopup;

        [XmlAttribute("message")]
        public string Message
        {
            get
            {
                if ( this.Exception != null )
                     return message.Replace ( "_var_", this.Exception.Message );
                else return message;
            }
            set { this.message = value; }
        }
        
        [XmlIgnore]
        public int MessageRelease { get; set; }
        
        [XmlAttribute("message_release")]
        public string MessageRelease_AllowEmptyField
        {
            get { return this.MessageRelease.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int v;
                    if (int.TryParse(value, out v))
                         this.MessageRelease = v;
                    else this.MessageRelease = EMPTY_VAL;
                }
                else this.MessageRelease = EMPTY_VAL;
            }
        }
        
        [XmlAttribute("footer")]
        public string MessageFooter
        {
            get
            {
                return "Error " + this.Id + ": " + this.messageFooter;
            }
            set { this.messageFooter = value; }
        }
        
        [XmlIgnore]
        public string MessagePopup
        {
            get
            {
                if ( this.Exception != null )
                     return messagePopup.Replace ( "_var_", this.Exception.Message );
                else return messagePopup;
            }
            set { this.messagePopup = value; }
        }

        public int Port;
        public Exception Exception;

        public object Clone ()
        {
            return this.MemberwiseClone ();
        }
    }
}
