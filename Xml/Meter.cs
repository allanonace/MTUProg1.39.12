using System;
using System.Xml.Serialization;

namespace Xml
{
    public class Meter
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlElement("Display")]
        public string Display { get; set; }
 
        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Vendor")]
        public string Vendor { get; set; }

        [XmlElement("Model")]
        public string Model { get; set; }

        [XmlElement("LiveDigits")]
        public int LiveDigits { get; set; }

        [XmlElement("DummyDigits")]
        public int DummyDigits { get; set; }

        [XmlElement("PaintedDigits")]
        public int PaintedDigits { get; set; }

        [XmlElement("LeadingDummy")]
        public int LeadingDummy { get; set; }

        [XmlElement("Scale")]
        public int Scale { get; set; }

        [XmlElement("Prescaler")]
        public int Prescaler { get; set; }

        [XmlElement("ImmediateAlarmTransmit")]
        public int ImmediateAlarmTransmit { get; set; }

        [XmlElement("DcuUrgentAlarm")]
        public int DcuUrgentAlarm { get; set; }

        [XmlElement("ExternalTamper")]
        public int ExternalTamper { get; set; }

        [XmlElement("InternalTamper")]
        public int InternalTamper { get; set; }

        [XmlElement("ProvingHandFactor")]
        public int ProvingHandFactor { get; set; }

        [XmlElement("WdtPrescalerFollowingEdge")]
        public int WdtPrescalerFollowingEdge { get; set; }

        [XmlElement("MinimumPulseLength")]
        public int MinimumPulseLength { get; set; }

        [XmlElement("EdgePolarity")]
        public int EdgePolarity { get; set; }

        [XmlElement("ReadingType")]
        public int ReadingType { get; set; }

        [XmlElement("EncoderType")]
        public int EncoderType { get; set; }


        [XmlElement("PulseLowTime")]
        public int PulseLowTime { get; set; }

        [XmlElement("PulseHiTime")]
        public int PulseHiTime { get; set; }

        [XmlElement("HiResScaling")]
        public int HiResScaling { get; set; }

        [XmlElement("PHF")]
        public int PHF { get; set; }

        [XmlElement("MtuMode")]
        public int MtuMode { get; set; }

        [XmlElement("Flow")]
        public int Flow { get; set; }

    }
}
