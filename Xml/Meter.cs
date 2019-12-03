using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace Xml
{
    /// <summary>
    /// Class used to map the 'Meter' entries present in the the Meter.xml configuration file.
    /// <para>&#160;</para>
    /// <para>
    /// Properties
    /// <list type="Meter">
    /// <item>
    ///   <term>Id</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Display</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Type</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>IsForEncoderOrEcode</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>MeterMask</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Utility</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>IsForGas</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>IsForWater</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Vendor</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Model</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>LiveDigits</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>DummyDigits</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>PaintedDigits</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>LeadingDummy</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Scale</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>EncoderType</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>PulseLowTime</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>PulseHiTime</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>HiResScaling</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>MtuMode</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>Flow</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>MeterTypeFlow</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>NumberOfDials</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>DriveDialSiz</term>
    ///   <description></description>
    /// </item>
    /// <item>
    ///   <term>UnitOfMeasure</term>
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
    /// <seealso cref="MeterTypes"/>
    public class Meter
    {
        public Meter ()
        {
            this.Display             = string.Empty;
            this.DummyDigits         = 0;
            this.EncoderDigitsToDrop = 0x0;
            this.EncoderType         = 0x0;
            this.Flow                = 0;
            this.HiResScaling        = 0m;
            this.Id                  = 0;
            this.LeadingDummy        = 0;
            this.LiveDigits          = 0;
            this.MeterMask           = string.Empty;
            this.Model               = string.Empty;
            this.MtuMode             = 0;
            this.PaintedDigits       = 0;
            this.PulseLowTime        = 0x0;
            this.PulseHiTime         = 0x0;
            this.Scale               = 0;
            this.Type                = string.Empty;
            this.Utility             = string.Empty;
            this.Vendor              = string.Empty;
        }

        #region Properties

        [XmlElement("Display")]
        public string Display { get; set; }

        private int dummyDigits;

        [XmlElement("DummyDigits")]
        public int DummyDigits
        {
            get { return this.dummyDigits; }
            set { this.dummyDigits = ( value >= 0 ) ? value : 0; }
        }

        private byte encoderDigitsToDrop;

        [XmlAttribute("EncoderDigitsToDrop")]
        public byte EncoderDigitsToDrop
        {
            get { return this.encoderDigitsToDrop; }
            set { this.encoderDigitsToDrop = ( value >= 0 && value <= 255 ) ? value : ( byte )0; }
        }

        private byte encoderType;

        [XmlElement("EncoderType")]
        public byte EncoderType
        {
            get { return this.encoderType; }
            set { this.encoderType = ( value >= 0 && value <= 255 ) ? value : ( byte )0; }
        }

        private int flow;

        [XmlElement("Flow")]
        public int Flow
        {
            get { return this.flow; }
            set { this.flow = ( value >= 0 && value <= 1 ) ? value : 0; }
        }

        private decimal hiResScaling;

        [XmlElement("HiResScaling")]
        public decimal HiResScaling
        {
            get { return this.hiResScaling; }
            set { this.hiResScaling = ( value >= 0 ) ? value : 0m; }
        }

        private int id;

        [XmlAttribute("ID")]
        public int Id
        {
            get { return this.id; }
            set { this.id = ( value >= 0 ) ? value : 0; }
        }

        private int leadingDummy;

        [XmlElement("LeadingDummy")]
        public int LeadingDummy
        {
            get { return this.leadingDummy; }
            set { this.leadingDummy = ( value >= 0 ) ? value : 0; }
        }

        private int liveDigits;

        [XmlElement("LiveDigits")]
        public int LiveDigits
        {
            get { return this.liveDigits; }
            set { this.liveDigits = ( value >= 0 ) ? value : 0; }
        }

        [XmlElement("MeterMask")]
        public string MeterMask { get; set; }

        [XmlElement("Model")]
        public string Model { get; set; }

        [XmlElement("MtuMode")]
        public int MtuMode { get; set; }

        private int paintedDigits;

        [XmlElement("PaintedDigits")]
        public int PaintedDigits
        {
            get { return this.paintedDigits; }
            set { this.paintedDigits = ( value >= 0 ) ? value : 0; }
        }

        private byte pulseLowTime;

        [XmlElement("PulseLowTime")]
        public byte PulseLowTime
        {
            get { return this.pulseLowTime; }
            set { this.pulseLowTime = ( value >= 0 && value <= 255 ) ? value : ( byte )0; }
        }

        private byte pulseHiTime;

        [XmlElement("PulseHiTime")]
        public byte PulseHiTime
        {
            get { return this.pulseHiTime; }
            set { this.pulseHiTime = ( value >= 0 && value <= 255 ) ? value : ( byte )0; }
        }

        private int scale;

        [XmlElement("Scale")]
        public int Scale
        {
            get { return this.scale; }
            set { this.scale = ( value >= 0 ) ? value : 0; }
        }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Utility")]
        public string Utility { get; set; }

        [XmlElement("Vendor")]
        public string Vendor { get; set; }

        #endregion

        #region Logic

        public bool IsEmpty
        {
            get { return this.LiveDigits <= 0; }
        }
        
        [XmlIgnore]
        public bool IsForEncoderOrEcoder
        {
            get { return Type.Equals ( "E" ); }
        }

        [XmlIgnore]
        public bool IsForGas
        {
            get { return this.IsFor ( "Gas" ); }
        }

        [XmlIgnore]
        public bool IsForWater
        {
            get { return this.IsFor ( "Water" ); }
        }

        private bool IsFor (
            string type )
        {
            return ! string.IsNullOrEmpty ( this.Utility ) &&
                   ! string.IsNullOrEmpty ( type ) &&
                   this.Utility.ToLower ().Equals ( type.ToLower () );
        }

        [XmlIgnore]
        public string MeterTypeFlow
        {
            get { return GetDisplayData<string> ( 1, String.Empty ); }
        }

        [XmlIgnore]
        public int NumberOfDials
        {
            get { return GetDisplayData<int> ( 2, -1 ); }
        }

        [XmlIgnore]
        public int DriveDialSize
        {
            get { return GetDisplayData<int> ( 3, -1 ); }
        }

        [XmlIgnore]
        public string UnitOfMeasure
        {
            get { return GetDisplayData<string> ( 4, string.Empty ); }
        }

        private T GetDisplayData<T> (
            int groupIndex,
            T defValue )
        {
            Match match = Regex.Match (
                            this.Display,
                            @"(\w+) (\d+)D PF(\d+) (\w+)",
                            RegexOptions.IgnoreCase | RegexOptions.Singleline |
                            RegexOptions.CultureInvariant | RegexOptions.Compiled );

            if ( match.Success )
                return ( T )Convert.ChangeType ( match.Groups[ groupIndex ].Value, typeof( T ) );
            return defValue;
        }
        
        public string ApplyReadingMask (
            string value )
        {
            if ( ! string.IsNullOrEmpty ( this.MeterMask ) &&
                 value.Length < this.LiveDigits )
            {
                string mask  = this.MeterMask.ToLower ();
                int    index = mask.IndexOfAny ( new Char[] { 'x' } );
                if ( index >= 0 )
                {
                    string leadingRead  = mask.Substring ( 0, index );
                    string trailingRead = mask.Substring ( index + 1 );
                    value = leadingRead + value + trailingRead;
                }
            }
            
            return value;
        }

        public string FillLeftLiveDigits (
            string value = "" )
        {
            return value.PadLeft ( this.LiveDigits, '0' );
        }
        
        public string FillLeftNumberOfDials (
            string value = "" )
        {
            return value.PadLeft ( this.NumberOfDials, '0' );
        }

        public String GetProperty (
            string name )
        {
            if ( string.IsNullOrEmpty ( name ) )
                return string.Empty;

            return this.GetType ()
                       .GetProperty ( name )
                       .GetValue ( this, null )
                       .ToString ();
        }

        #endregion
    }
}
