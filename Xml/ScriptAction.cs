using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
namespace Xml
{
    public class ScriptAction
    {
        private Dictionary<string,string> additionalParameters;
        
        public ScriptAction ()
        {
            this.additionalParameters = new Dictionary<string,string> ();
        }
        
        [XmlIgnore]
        public Dictionary<string,string> AdditionalParameters
        {
            get { return this.additionalParameters; }
        }

        public void AddAdditionParameter (
            string id,
            string value )
        {
            if ( ! this.additionalParameters.ContainsKey ( id ) )
                this.additionalParameters.Add ( id, value );
        }
    
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("ActivityLogId")]
        public ActionParameter ActivityLogId { get; set; }

        [XmlElement("Port2Disabled")]
        public ActionParameter Port2Disabled { get; set; }

        [XmlElement("ProvidingHandFactor")]
        public ActionParameter ProvidingHandFactor { get; set; }

        [XmlElement("OldMtuId")]
        public ActionParameter OldMtuId { get; set; }

        [XmlElement("ReadInterval")]
        public ActionParameter ReadInterval { get; set; }
        
        [XmlElement("SnapRead")]
        public ActionParameter SnapRead { get; set; }

        [XmlElement("ForceTimeSync")]
        public ActionParameter ForceTimeSync { get; set; }

        [XmlElement("LiveDigits")]
        public ActionParameter LiveDigits { get; set; }

        [XmlElement("TempReadInterval")]
        public ActionParameter TempReadInterval { get; set; }

        [XmlElement("Alarm")]
        public ActionParameter Alarm { get; set; }

        [XmlElement("TempReadDays")]
        public ActionParameter TempReadDays { get; set; }

        [XmlElement("Custom")]
        public ActionParameter Custom { get; set; }

        // Port fields

        [XmlElement("AccountNumber")]
        public ActionParameter[] AccountNumber { get; set; }

        [XmlElement("WorkOrder")]
        public ActionParameter[] WorkOrder { get; set; }

        [XmlElement("UnitOfMeasure")]
        public ActionParameter[] UnitOfMeasure { get; set; }

        [XmlElement("NumberOfDials")]
        public ActionParameter[] NumberOfDials { get; set; }

        [XmlElement("DriveDialSize")]
        public ActionParameter[] DriveDialSize { get; set; }

        [XmlElement("OldMeterSerialNumber")]
        public ActionParameter[] OldMeterSerialNumber { get; set; }

        [XmlElement("OldMeterReading")]
        public ActionParameter[] OldMeterReading { get; set; }

        [XmlElement("MeterSerialNumber")]
        public ActionParameter[] MeterSerialNumber { get; set; }


        [XmlElement("NewMeterSerialNumber")]
        public ActionParameter[] NewMeterSerialNumber { get; set; }

     
        [XmlElement("NewMeterReading")]
        public ActionParameter[] NewMeterReading { get; set; }
       
        
        [XmlElement("MeterReading")]
        public ActionParameter[] MeterReading { get; set; }

        
        [XmlElement("MeterType")]
        public ActionParameter[] MeterType { get; set; }
    }
}
