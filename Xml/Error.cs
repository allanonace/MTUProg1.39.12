using System.Xml.Serialization;

namespace Xml
{
    public class Error
    {
        public const int ERROR_VAL = -1;

        [XmlIgnore]
        public int Status { get; set; }

        [XmlElement("Status")]
        public string Status_AllowEmptyField
        {
            get { return this.Status.ToString(); }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    int v;
                    if (int.TryParse(value, out v))
                         this.Status = v;
                    else this.Status = ERROR_VAL;
                }
                else this.Status = ERROR_VAL;
            }
        }

        [XmlElement("Message")]
        public string Message { get; set; }

        [XmlElement("LogMessage")]
        public string LogMessage { get; set; }
    }
}
