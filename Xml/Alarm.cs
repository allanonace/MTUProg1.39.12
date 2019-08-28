using System;
using System.Xml.Serialization;

namespace Xml
{
    /// <summary>
    /// Class used to map the 'Alarm' entries present in the the Alarm.xml configuration file.
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="Alarm">
    /// <item>
    ///   <term>CutAlarmCable</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>CutWireAlarmImm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>CutWireDelaySetting</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>DcuUrgentAlarm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ECoderDaysNoFlow</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ECoderDaysOfLeak</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ECoderLeakDetectionCurrent</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ECoderReverseFlow</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ImmediateAlarmTransmit</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>InsufficientMemory</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>InsufficientMemoryImm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>InterfaceTamper</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>InterfaceTamperImm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>LastGasp</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>LastGaspImm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Magnetic</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Overlap</term>
    ///   <description>[1-11]</description>
    /// </item>
    /// <item>
    ///   <term>RegisterCover</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ReverseFlow</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>SerialComProblem</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>SerialComProblemImm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>SerialCutWire</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>SerialCutWireImm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Tilt</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>TamperPort1</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>TamperPort2</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>TamperPort1Imm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>TamperPort2Imm</term>
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
    /// <seealso cref="AlarmList"/>
    public class Alarm
    {
        private const int DEF_CUTWIRE = 0;
        private const int DEF_OVERLAP = 6;
    
        public Alarm ()
        {
            //this.ImmediateTransmit        = Konstantin: Use ImmediateAlarmTransmit instead
            //this.IntervalData             = Konstantin: No need to implement

            this.CutAlarmCable              = false;
            this.CutWireAlarmImm            = false;
            this.CutWireDelaySetting        = DEF_CUTWIRE;
            this.DcuUrgentAlarm             = false;
            this.ECoderDaysNoFlow           = false;
            this.ECoderDaysOfLeak           = false;
            this.ECoderLeakDetectionCurrent = false;
            this.ECoderReverseFlow          = false;
            this.ImmediateAlarmTransmit     = false;
            this.InsufficientMemory         = false;
            this.InsufficientMemoryImm      = false;
            this.InterfaceTamper            = true;
            this.InterfaceTamperImm         = false;
            this.LastGasp                   = false;
            this.LastGaspImm                = false;
            this.Magnetic                   = true;
            this.Overlap                    = DEF_OVERLAP;  // [1-11]
            this.RegisterCover              = true;
            this.ReverseFlow                = true;
            this.SerialComProblem           = false;
            this.SerialComProblemImm        = false;
            this.SerialCutWire              = false;
            this.SerialCutWireImm           = false;
            this.Tilt                       = true;
            this.TamperPort1                = false;
            this.TamperPort2                = false;
            this.TamperPort1Imm             = false;
            this.TamperPort2Imm             = false;
        }

        #region Elements

        [XmlAttribute("MTUType")]
        public int MTUType { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlIgnore]
        public byte CutWireDelaySetting { get; set; }
        
        [XmlElement("CutWireDelaySetting")]
        public string CutWireDelaySetting_AllowEmptyField
        {
            get { return this.CutWireDelaySetting.ToString (); }
            set
            {
                if ( ! string.IsNullOrEmpty ( value ) )
                {
                    byte v;
                    if ( byte.TryParse ( value, out v ) )
                         this.CutWireDelaySetting = v;
                    else this.CutWireDelaySetting = DEF_CUTWIRE;
                }
                else this.CutWireDelaySetting = DEF_CUTWIRE;
            }
        }
        
        [XmlIgnore]
        public int Overlap { get; set; }
        
        [XmlElement("Overlap")]
        public string Overlap_AllowEmptyField
        {
            get { return this.Overlap.ToString (); }
            set
            {
                if ( ! string.IsNullOrEmpty ( value ) )
                {
                    int v;
                    if ( int.TryParse ( value, out v ) )
                         this.Overlap = v;
                    else this.Overlap = DEF_OVERLAP;
                }
                else this.Overlap = DEF_OVERLAP;
            }
        }

        #region Tampers

        [XmlElement("CutAlarmCable")]
        public bool CutAlarmCable { get; set; }
        
        [XmlElement("CutWireAlarmImm")]
        public bool CutWireAlarmImm { get; set; }
        
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

        [XmlElement("EnergizerLastGasp")]
        public bool EnergizerLastGasp { get; set; }

        [XmlElement("ImmediateAlarmTransmit")]
        public bool ImmediateAlarmTransmit { get; set; }

        [XmlElement("InsufficentMemory")]
        public bool InsufficientMemory { get; set; }

        [XmlElement("InsufficentMemoryImm")]
        public bool InsufficientMemoryImm { get; set; }
        
        [XmlElement("InterfaceTamper")]
        public bool InterfaceTamper { get; set; }

        [XmlElement("InterfaceTamperImm")]
        public bool InterfaceTamperImm { get; set; }

        [XmlElement("LastGasp")]
        public bool LastGasp { get; set; }

        [XmlElement("LastGaspImm")]
        public bool LastGaspImm { get; set; }

        [XmlElement("Magnetic")]
        public bool Magnetic { get; set; }
        
        [XmlElement("MemoryMapError")]
        public bool MemoryMapError { get; set; }

        [XmlElement("MoistureDetect")]
        public bool MoistureDetect { get; set; }

        [XmlElement("ProgramMemoryError")]
        public bool ProgramMemoryError { get; set; }

        [XmlElement("RegisterCover")]
        public bool RegisterCover { get; set; }
        
        [XmlElement("ReverseFlow")]
        public bool ReverseFlow { get; set; }
        
        [XmlElement("SerialComProblem")]
        public bool SerialComProblem { get; set; }

        [XmlElement("SerialComProblemImm")]
        public bool SerialComProblemImm { get; set; }

        [XmlElement("SerialCutWire")]
        public bool SerialCutWire { get; set; }

        [XmlElement("SerialCutWireImm")]
        public bool SerialCutWireImm { get; set; }
        
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

        #endregion

        #endregion
    }
}
