using System;
using System.Xml.Serialization;

namespace Xml
{
    public class Alarm
    {
        [XmlAttribute("MTUType")]
        public int MTUType { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("AlarmMask1")]
        public int AlarmMask1 { get; set; }

        [XmlElement("AlarmMask2")]
        public int AlarmMask2 { get; set; }

        [XmlElement("AlarmMessages")]
        public int AlarmMessages { get; set; }

        [XmlElement("CutAlarmCable")]
        public bool CutAlarmCable { get; set; }

        [XmlElement("CutWireAlarmImm")]
        public bool CutWireAlarmImm { get; set; }

        [XmlElement("CutWireDelaySetting")]
        public int CutWireDelaySetting { get; set; }

        [XmlElement("DailyData")]
        public bool DailyData { get; set; }

        [XmlElement("DcuUrgentAlarm")]
        public bool DcuUrgentAlarm { get; set; }

        [XmlElement("ECoderDaysNoFlow")]
        public bool ECoderDaysNoFlow { get; set; }

        [XmlElement("ECoderDaysOfLeak")]
        public bool ECoderDaysOfLeak { get; set; }

        [XmlElement("ECoderLeakDetectionCurrent")]
        public bool ECoderLeakDetectionCurrent { get; set; }

        [XmlElement("ECoderReverseFlow")]
        public bool ECoderReverseFlow { get; set; }

        [XmlElement("HardErrorAlarm")]
        public bool HardErrorAlarm { get; set; }

        [XmlElement("ImmediateAlarmTransmit")]
        public bool ImmediateAlarmTransmit { get; set; }

        [XmlElement("ImmediateTransmit")]
        public bool ImmediateTransmit { get; set; }

        [XmlElement("InsufficientMemory")]
        public bool InsufficientMemory { get; set; }

        [XmlElement("InsufficientMemoryImm")]
        public bool InsufficientMemoryImm { get; set; }

        [XmlElement("InterfaceTamper")]
        public bool InterfaceTamper { get; set; }

        [XmlElement("InterfaceTamperImm")]
        public bool InterfaceTamperImm { get; set; }

        [XmlElement("IntervalData")]
        public bool IntervalData { get; set; }

        [XmlElement("LastGasp")]
        public bool LastGasp { get; set; }

        [XmlElement("LastGaspImm")]
        public bool LastGaspImm { get; set; }

        [XmlElement("Magnetic")]
        public bool Magnetic { get; set; }

        [XmlElement("MessageSendTime")]
        public int MessageSendTime { get; set; }

        [XmlElement("OutageMessageCount")]
        public int OutageMessageCount { get; set; }

        [XmlElement("Overlap")]
        public string Overlap { get; set; }

        [XmlElement("PowerOutDelay")]
        public int PowerOutDelay { get; set; }

        [XmlElement("PowerRestoreDelay")]
        public int PowerRestoreDelay { get; set; }

        [XmlElement("RegisterCover")]
        public bool RegisterCover { get; set; }

        [XmlElement("Response")]
        public string Response { get; set; }

        [XmlElement("RestoreMessageCount")]
        public int RestoreMessageCount { get; set; }

        [XmlElement("ReverseFlow")]
        public bool ReverseFlow { get; set; }

        [XmlElement("ReversePowerAlarm")]
        public bool ReversePowerAlarm { get; set; }

        [XmlElement("SerialComProblem")]
        public bool SerialComProblem { get; set; }

        [XmlElement("SerialComProblemImm")]
        public bool SerialComProblemImm { get; set; }

        [XmlElement("SerialCutWire")]
        public bool SerialCutWire { get; set; }

        [XmlElement("SerialCutWireImm")]
        public bool SerialCutWireImm { get; set; }

        [XmlElement("SyncDrift")]
        public int SyncDrift { get; set; }

        [XmlElement("SyncTimeout")]
        public int SyncTimeout { get; set; }

        [XmlElement("Tilt")]
        public bool Tilt { get; set; }

        [XmlElement("TamperPort1")]
        public bool TamperPort1 { get; set; }

        [XmlElement("TamperPort2")]
        public bool TamperPort2 { get; set; }

        [XmlElement("TamperPort1Imm")]
        public bool TamperPort1Imm { get; set; }

        [XmlElement("TamperPort2Imm")]
        public bool TamperPort2Imm { get; set; }

        [XmlElement("UrgentAlarm")]
        public bool UrgentAlarm { get; set; }

    }
}
