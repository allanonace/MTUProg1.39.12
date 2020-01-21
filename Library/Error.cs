using System;
using System.Xml.Serialization;
using Library.Exceptions;

namespace Library
{
    public class Error : ICloneable
    {
        private const int EMPTY_VAL = -1;
    
        private string message;
        private string messageFooter;
        private string messagePopup;

        public Error ()
        {
            this.Port         = 1;
            this.Id           = EMPTY_VAL;
            this.DotNetId     = EMPTY_VAL;
            this.MessagePopup = string.Empty;
        }
        
        public Error ( string message )
        : this ()
        {
            this.Message = message;
        }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("no_error")]
        public bool NoError { get; set; }

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
        public string Message
        {
            get
            {
                if ( this.Exception != null &&
                     this.Exception is OwnExceptionsBase )
                {
                    OwnExceptionsBase ownExc = ( OwnExceptionsBase )this.Exception;

                    if ( ! string.IsNullOrEmpty ( ownExc.VarMessage ) )
                        return this.message.Replace ( "_var_", ownExc.VarMessage );
                }
                     
                return message;
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
        
        [XmlAttribute("popup")]
        public string MessagePopup
        {
            get
            {
                string text = ( string.IsNullOrEmpty ( this.messagePopup ) ) ? this.message : this.messagePopup;

                if ( this.Exception != null &&
                     this.Exception is OwnExceptionsBase )
                {
                    OwnExceptionsBase ownExc = ( OwnExceptionsBase )this.Exception;

                    if ( ! string.IsNullOrEmpty ( ownExc.VarMessagePopup ) )
                        return text.Replace ( "_var_", ownExc.VarMessagePopup );

                    else if ( ! string.IsNullOrEmpty ( ownExc.VarMessage ) )
                        return text.Replace ( "_var_", ownExc.VarMessage );
                }
                
                return text;
            }
            set { this.messagePopup = value; }
        }

        [XmlIgnore]
        public bool HasMessagePopup
        {
            get { return ! string.IsNullOrEmpty ( this.messagePopup ); }
        }

        public int Port { get; set; }
        public Exception Exception { get; set; }

        public object Clone ()
        {
            return this.MemberwiseClone ();
        }
    }
}
