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
        public byte AlarmMask1 { get; set; }

        [XmlElement("AlarmMask2")]
        public byte AlarmMask2 { get; set; }

        [XmlElement("AlarmMessages")]
        public byte AlarmMessages { get; set; }

        [XmlElement("CutAlarmCable")]
        public bool CutAlarmCable { get; set; }

        [XmlElement("CutWireAlarmImm")]
        public bool CutWireAlarmImm { get; set; }

        [XmlElement("CutWireDelaySetting")]
        public byte CutWireDelaySetting { get; set; }

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

        [XmlElement("HighAlarmValue")]
        public int HighAlarmValue { get; set; }

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
        public byte MessageSendTime { get; set; }

        [XmlElement("OutageMessageCount")]
        public byte OutageMessageCount { get; set; }

        [XmlElement("Overlap")]
        public string Overlap { get; set; }

        [XmlElement("PowerOutDelay")]
        public byte PowerOutDelay { get; set; }

        [XmlElement("PowerRestoreDelay")]
        public byte PowerRestoreDelay { get; set; }

        [XmlElement("RegisterCover")]
        public bool RegisterCover { get; set; }

        [XmlElement("Response")]
        public string Response { get; set; }

        [XmlElement("RestoreMessageCount")]
        public byte RestoreMessageCount { get; set; }

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
        public byte SyncDrift { get; set; }

        [XmlElement("SyncTimeout")]
        public byte SyncTimeout { get; set; }

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
