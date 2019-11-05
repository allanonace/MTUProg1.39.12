using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Xml
{
    /// <summary>
    /// Class used to map the 'Mtu' entries present in the the Mtu.xml configuration file
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="Mtu">
    /// <item>
    ///   <term>BroadCast</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>CutWireDelaySetting</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>DataRead</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>DailyReads</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>DigitsToDrop</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Ecoder</term>
    ///   <description>Ecoder Only MTUs</description>
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
    ///   <term>FastMessageConfig</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Flow</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>GasCutWireAlarm</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>GasCutWireAlarmImm</term>
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
    ///   <term>MtuDemand</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>NodeDiscovery</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>OnTimeSync</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Ports</term>
    ///   <description>List of <see cref="Port"/> entries</description>
    /// </item>
    /// <item>
    ///   <term>Port1</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Port2</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>PulseCountOnly</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>RegisterCoverTamper</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>RequiresAlarmProfile</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>ReverseFlowTamper</term>
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
    ///   <term>STAREncryptionType</term>
    ///   <description>[None,AES128,AES256]</description>
    /// </item>
    /// <item>
    ///   <term>SpecialSet</term>
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
    /// <item>
    ///   <term>TiltTamper</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>TimeToSync</term>
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
    /// <seealso cref="MtuTypes"/>
    public class Mtu
    {
        #region Constants

        public enum VERSION
        {
            NEW,
            ARCH
        };

        public enum ENCRYPTION
        {
            NONE,
            AES128,
            AES256
        };

        public enum Family
        {
            NOTHING,
            _31xx32xx,
            _33xx,
            _342x,
            _35xx36xx
        }

        private const int DEF_FLOW = -1;

        #endregion

        #region Attributes

        private Family    family;

        #endregion
    
        public Mtu ()
        {
            this.BroadCast                  = false;
            this.CutWireDelaySetting        = false;
            this.DataRead                   = false;
            this.DailyReads                 = true;
            this.DigitsToDrop               = false;
            this.Ecoder                     = false;     // Ecoder Only MTUs
            this.ECoderDaysNoFlow           = false;
            this.ECoderDaysOfLeak           = false;
            this.ECoderLeakDetectionCurrent = false;
            this.ECoderReverseFlow          = false;
            this.EnergizerLastGasp          = false;
            this.EnergizerLastGaspImm       = false;
            this.FastMessageConfig          = false;
            this.Flow                       = DEF_FLOW;
            this.GasCutWireAlarm            = false;
            this.GasCutWireAlarmImm         = false;
            this.InsufficientMemory         = false;
            this.InsufficientMemoryImm      = false;
            this.InterfaceTamper            = false;
            this.InterfaceTamperImm         = false;
            this.LastGasp                   = false;
            this.LastGaspImm                = false;
            this.MagneticTamper             = false;
            this.MagneticTamperImm          = false;
            this.MemoryMapError             = false;
            this.MemoryMapErrorImm          = false;
            this.MtuDemand                  = false;
            this.MoistureDetect             = false;
            this.MoistureDetectImm          = false;
            this.NodeDiscovery              = false;
            this.OnTimeSync                 = false;
            this.ProgramMemoryError         = false;
            this.ProgramMemoryErrorImm      = false;
            this.PulseCountOnly             = false;
            this.RegisterCoverTamper        = false;
            this.RegisterCoverTamperImm     = false;
            this.RequiresAlarmProfile       = false;
            this.ReverseFlowTamper          = false;
            this.ReverseFlowTamperImm       = false;
            this.SerialComProblem           = false;
            this.SerialComProblemImm        = false;
            this.SerialCutWire              = false;
            this.SerialCutWireImm           = false;
            this.STAREncryptionType         = ENCRYPTION.AES256;
            this.SpecialSet                 = false;
            this.TamperPort1                = false;
            this.TamperPort1Imm             = false;
            this.TamperPort2                = false;
            this.TamperPort2Imm             = false;
            this.TiltTamper                 = false;
            this.TiltTamperImm              = false;
            this.TimeToSync                 = false;
        }

        #region Elements

        [XmlElement("BroadCast")]
        public bool BroadCast { get; set; }

        // NOTE: Is the Mtu Type ( 171, 177,... ) and not the Serial Number or Mtu ID
        [XmlAttribute("ID")]
        public int Id { get; set; }

        [XmlElement("DataRead")]
        public bool DataRead { get; set; }
    
        [XmlElement("DailyReads")]
        public bool DailyReads { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("DigitsToDrop")]
        public bool DigitsToDrop { get; set; }

        [XmlElement("Ecoder")]
        public bool Ecoder { get; set; }

        [XmlElement("ECoderDaysNoFlow")]
        public bool ECoderDaysNoFlow { get; set; }

        [XmlElement("ECoderDaysOfLeak")]
        public bool ECoderDaysOfLeak { get; set; }

        [XmlElement("ECoderLeakDetectionCurrent")]
        public bool ECoderLeakDetectionCurrent { get; set; }

        [XmlElement("ECoderReverseFlow")]
        public bool ECoderReverseFlow { get; set; }

        [XmlElement("FastMessageConfig")]
        public bool FastMessageConfig { get; set; }
        
        [XmlIgnore]
        public int Flow { get; set; }
        
        [XmlElement("Flow")]
        public string Flow_AllowEmptyField
        {
            get { return this.Flow.ToString (); }
            set
            {
                if ( ! string.IsNullOrEmpty ( value ) )
                {
                    int v;
                    if ( int.TryParse ( value, out v ) )
                         this.Flow = v;
                    else this.Flow = DEF_FLOW;
                }
                else this.Flow = DEF_FLOW;
            }
        }

        [XmlElement("HexNum")]
        public string HexNum { get; set; }

        [XmlIgnore]
        public bool IsFamily31xx32xx
        {
            get
            {
                if ( this.HasFamilySet )
                    return this.family == Family._31xx32xx;

                string hexnum = this.HexNum.ToLower ();
            
                return hexnum.StartsWith ( "31" ) ||
                       hexnum.StartsWith ( "32" );
            }
        }
        
        [XmlIgnore]
        public bool IsFamily33xx
        {
            get
            {
                if ( this.HasFamilySet )
                    return this.family == Family._33xx;

                return this.HexNum.ToLower ().StartsWith ( "33" );
            }
        }
        
        [XmlIgnore]
        public bool IsFamily342x
        {
            get
            {
                if ( this.HasFamilySet )
                    return this.family == Family._342x;

                return this.HexNum.ToLower ().StartsWith ( "342" );
            }
        }

        [XmlIgnore]
        public bool IsFamily35xx36xx
        {
            get
            {
                if ( this.HasFamilySet )
                    return this.family == Family._35xx36xx;

                string hexnum = this.HexNum.ToLower ();
            
                return hexnum.StartsWith ( "35" ) ||
                       hexnum.StartsWith ( "36" );
            }
        }

        [XmlElement("Model")]
        public string Model { get; set; }

        [XmlElement("MtuDemand")]
        public bool MtuDemand { get; set; }
        
        [XmlElement("NodeDiscovery")]
        public bool NodeDiscovery { get; set; }

        [XmlElement("OnTimeSync")]
        public bool OnTimeSync { get; set; }

        [XmlElement("Port")]
        public List<Port> Ports { get; set; }
        
        [XmlIgnore]
        public Port Port1
        {
            get { return this.Ports[ 0 ]; }
        }
        
        [XmlIgnore]
        public Port Port2
        {
            get { return this.Ports[ 1 ]; }
        }
        
        [XmlElement("PulseCountOnly")]
        public bool PulseCountOnly { get; set; }
        
        [XmlElement("RequiresAlarmProfile")]
        public bool RequiresAlarmProfile { get; set; }
        
        [XmlIgnore]
        public ENCRYPTION STAREncryptionType { get; set; }

        [XmlElement("STAREncryptionType")]
        public string STAREncryptionType_AllowEmptyField
        {
            get { return this.STAREncryptionType.ToString (); }
            set
            {
                ENCRYPTION v;
                bool ok = Enum.TryParse<ENCRYPTION>( value, true, out v );
                this.STAREncryptionType = ( ok ) ? v : ENCRYPTION.NONE;
            }
        }
        
        [XmlElement("SpecialSet")]
        public bool SpecialSet { get; set; }

        [XmlElement("TimeToSync")]
        public bool TimeToSync { get; set; }
        
        [XmlElement("Version")]
        public string VersionString { get; set; }

        #region Tampers

        [XmlElement("CutWireDelaySetting")]
        public bool CutWireDelaySetting { get; set; }
        
        [XmlElement("EnergizerLastGasp")]
        public bool EnergizerLastGasp { get; set; }

        [XmlElement("EnergizerLastGaspImm")]
        public bool EnergizerLastGaspImm { get; set; }

        [XmlElement("GasCutWireAlarm")]
        public bool GasCutWireAlarm { get; set; }
        
        [XmlElement("GasCutWireAlarmImm")]
        public bool GasCutWireAlarmImm { get; set; }
        
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
        
        [XmlElement("MagneticTamper")]
        public bool MagneticTamper { get; set; }
        
        [XmlElement("MagneticTamperImm")]
        public bool MagneticTamperImm { get; set; }

        [XmlElement("MemoryMapError")]
        public bool MemoryMapError { get; set; }

        [XmlElement("MemoryMapErrorImm")]
        public bool MemoryMapErrorImm { get; set; }

        [XmlElement("MoistureDetect")]
        public bool MoistureDetect { get; set; }

        [XmlElement("MoistureDetectImm")]
        public bool MoistureDetectImm { get; set; }

        [XmlElement("ProgramMemoryError")]
        public bool ProgramMemoryError { get; set; }

        [XmlElement("ProgramMemoryErrorImm")]
        public bool ProgramMemoryErrorImm { get; set; }

        [XmlElement("RegisterCoverTamper")]
        public bool RegisterCoverTamper { get; set; }

        [XmlElement("RegisterCoverTamperImm")]
        public bool RegisterCoverTamperImm { get; set; }
        
        [XmlElement("ReverseFlowTamper")]
        public bool ReverseFlowTamper { get; set; }

        [XmlElement("ReverseFlowTamperImm")]
        public bool ReverseFlowTamperImm { get; set; }
        
        [XmlElement("SerialComProblem")]
        public bool SerialComProblem { get; set; }
        
        [XmlElement("SerialComProblemImm")]
        public bool SerialComProblemImm { get; set; }
        
        [XmlElement("SerialCutWire")]
        public bool SerialCutWire { get; set; }

        [XmlElement("SerialCutWireImm")]
        public bool SerialCutWireImm { get; set; }
        
        [XmlElement("TamperPort1")]
        public bool TamperPort1 { get; set; }

        [XmlElement("TamperPort2")]
        public bool TamperPort2 { get; set; }

        [XmlElement("TamperPort1Imm")]
        public bool TamperPort1Imm { get; set; }

        [XmlElement("TamperPort2Imm")]
        public bool TamperPort2Imm { get; set; }
        
        [XmlElement("TiltTamper")]
        public bool TiltTamper { get; set; }

        [XmlElement("TiltTamperImm")]
        public bool TiltTamperImm { get; set; }

        #endregion

        #endregion

        #region Logic

        public String GetProperty ( String Name )
        {
            return this.GetType ().GetProperty ( Name ).GetValue ( this, null ).ToString ();
        }

        [XmlIgnore]
        public bool TwoPorts
        {
            get
            {
                return ( this.Ports.Count > 1 );
            }
        }
        
        [XmlIgnore]
        public VERSION Version
        {
            get { return ( this.VersionString.ToLower ().Equals ( VERSION.NEW.ToString ().ToLower () ) ) ? VERSION.NEW : VERSION.ARCH; }
        }
        
        [XmlIgnore]
        public bool IsArchVersion
        {
            get { return this.Version == VERSION.ARCH; }
        }

        [XmlIgnore]
        public bool IsNewVersion
        {
            get { return this.Version == VERSION.NEW; }
        }

        [XmlIgnore]
        public bool HasFamilySet
        {
            get { return this.family != Family.NOTHING; }
        }

        public void SetFamily (
            string family )
        {
            Enum.TryParse<Family> ( "_" + family, out this.family );
        }

        public string GetFamily ()
        {
            if ( this.HasFamilySet )
                return this.family.ToString ().Substring ( 1 ).ToLower ();

            return string.Empty;
        }

        public object SimulateRddInPortTwoIfNeeded ()
        {
            if ( ! this.TwoPorts &&
                 this.Port1.IsSetFlow )
            {
                Mtu copy = this.MemberwiseClone () as Mtu;
                copy.Ports = new List<Port> ();
                copy.Ports.Add ( this.Port1.Clone () as Port );
                copy.Ports.Add ( this.Port1.Clone () as Port );
                copy.Port2.Number++;

                return copy;
            }
            return this;
        }

        #endregion
    }
}
