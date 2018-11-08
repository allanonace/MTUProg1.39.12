using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    public class Mtu
    {
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlElement("Model")]
        public string Model { get; set; }

        [XmlElement("Version")]
        public string Version { get; set; }

        [XmlElement("FastMessageConfig")]
        public bool FastMessageConfig { get; set; }

        [XmlElement("CutWireDelaySetting")]
        public bool CutWireDelaySetting { get; set; }

        [XmlElement("DigitsToDrop")]
        public bool DigitsToDrop { get; set; }

        [XmlElement("Port")]
        public List<Port> Ports { get; set; }

        [XmlElement("IsTables")]
        public int IsTablesSerialize { get; set; }

        [XmlIgnore]
        public bool IsTables
        {
            get
            {
                bool isTables = false;
                if (IsTablesSerialize.Equals("1"))
                {
                    isTables = true;
                }
                return isTables;
            }
        }

        [XmlElement("IsEcoder")]
        public bool IsEcoder { get; set; }

        [XmlElement("HexNum")]
        public string HexNum { get; set; }

        [XmlElement("MtuDemand")]
        public bool MtuDemand { get; set; }

        [XmlElement("RequiresAlarmProfile")]
        public bool RequiresAlarmProfile { get; set; }

        [XmlElement("TimeToSync")]
        public bool TimeToSync { get; set; }

        [XmlElement("MagneticTamper")]
        public bool MagneticTamper { get; set; }

        [XmlElement("RegisterCoverTamper")]
        public bool RegisterCoverTamper { get; set; }

        [XmlElement("ReverseFlowTamper")]
        public bool ReverseFlowTamper { get; set; }

        [XmlElement("TiltTamper")]
        public bool TiltTamper { get; set; }

        [XmlElement("InterfaceTamper")]
        public bool InterfaceTamper { get; set; }

        [XmlElement("DailyReads")]
        public bool DailyReads { get; set; }

        [XmlElement("OnTimeSync")]
        public bool OnTimeSync { get; set; }

        [XmlElement("PulseCountOnly")]
        public bool PulseCountOnly { get; set; }

        [XmlElement("DDConfig")]
        public bool DDConfig { get; set; }

        [XmlElement("Ecoder")]
        public bool Ecoder { get; set; }

        [XmlElement("SpecialSet")]
        public bool SpecialSet { get; set; }

        [XmlElement("GasCutWireAlarm")]
        public bool GasCutWireAlarm { get; set; }

        [XmlElement("Flow")]
        public int Flow { get; set; }

        [XmlElement("MeterId")]
        public string MeterId { get; set; }

        [XmlElement("CombinePorts")]
        public bool CombinePorts { get; set; }

        [XmlElement("Corrector")]
        public bool Corrector { get; set; }

        [XmlElement("SerialComProblem")]
        public bool SerialComProblem { get; set; }

        [XmlElement("LastGasp")]
        public bool LastGasp { get; set; }

        [XmlElement("SerialCutWire")]
        public bool SerialCutWire { get; set; }

        [XmlElement("SerialCutWireImm")]
        public bool SerialCutWireImm { get; set; }

        [XmlElement("LastGaspImm")]
        public bool LastGaspImm { get; set; }

        [XmlElement("SerialComProblemImm")]
        public bool SerialComProblemImm { get; set; }

        [XmlElement("InterfaceTamperImm")]
        public bool InterfaceTamperImm { get; set; }

        [XmlElement("GasCutWireAlarmImm")]
        public bool GasCutWireAlarmImm { get; set; }

        [XmlElement("InsufficentMemoryImm")]
        public bool InsufficentMemoryImm { get; set; }

        [XmlElement("InsufficentMemory")]
        public bool InsufficentMemory { get; set; }

        [XmlElement("TamperPort1")]
        public bool TamperPort1 { get; set; }

        [XmlElement("TamperPort2")]
        public bool TamperPort2 { get; set; }

        [XmlElement("TamperPort1Imm")]
        public bool TamperPort1Imm { get; set; }

        [XmlElement("TamperPort2Imm")]
        public bool TamperPort2Imm { get; set; }

        [XmlElement("ECoderLeakDetectionCurrent")]
        public bool ECoderLeakDetectionCurrent { get; set; }

        [XmlElement("ECoderDaysNoFlow")]
        public bool ECoderDaysNoFlow { get; set; }

        [XmlElement("ECoderDaysOfLeak")]
        public bool ECoderDaysOfLeak { get; set; }

        [XmlElement("ECoderReverseFlow")]
        public bool ECoderReverseFlow { get; set; }
    }
}
