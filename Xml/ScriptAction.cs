using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
namespace Xml
{
    /// <summary>
    /// Class used to map the 'action' entries present in the the script file.
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="ScriptAction">
    /// <item>
    ///   <term>Type</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ActivityLogId</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>DaysOfRead</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>OldMtuId</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ReadInterval</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>SnapRead</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ForceTimeSync</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Alarm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Custom</term>
    ///   <description></description>
    /// </item>
    /// </list>
    /// </para>
    /// <para>&#160;</para>
    /// </summary>
    /// <remarks>
    /// NOTE: The values set in the constructor of the class are the default
    /// values that are used when a tag is not present in the configuration file.
    /// </remarks>
    /// <seealso cref="Script"/>
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

        //[XmlElement("Port2Disabled")]
        //public ActionParameter Port2Disabled { get; set; }

        //[XmlElement("ProvidingHandFactor")]
        //public ActionParameter ProvidingHandFactor { get; set; }

        [XmlElement("DaysOfRead")]
        public ActionParameter DaysOfRead { get; set; }

        [XmlElement("OldMtuId")]
        public ActionParameter OldMtuId { get; set; }

        [XmlElement("ReadInterval")]
        public ActionParameter ReadInterval { get; set; }
        
        [XmlElement("SnapRead")]
        public ActionParameter SnapRead { get; set; }

        [XmlElement("ForceTimeSync")]
        public ActionParameter ForceTimeSync { get; set; }

        //[XmlElement("LiveDigits")]
        //public ActionParameter LiveDigits { get; set; }

        //[XmlElement("TempReadInterval")]
        //public ActionParameter TempReadInterval { get; set; }

        [XmlElement("Alarm")]
        public ActionParameter Alarm { get; set; }

        //[XmlElement("TempReadDays")]
        //public ActionParameter TempReadDays { get; set; }

        // Remote Disconnect

        [XmlElement("RDDPosition")]
        public ActionParameter RDDPosition { get; set; }

        [XmlElement("RDDFirmwareVersion")]
        public ActionParameter RDDFirmwareVersion { get; set; }

        // Custom field

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
