using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("DebugOptions")]
    public class DebugOptions
    {
        #region Configuration Files

        // AclaraInstallPage.xaml.cs
        [XmlElement("ForceErrorConfig_New_Date")]
        public bool ForceErrorConfig_New_Date { get; set; }

        [XmlElement("ForceErrorConfig_New_Files")]
        public bool ForceErrorConfig_New_Files { get; set; }

        // AclaraViewConfig.xaml.cs
        [XmlElement("ForceErrorConfig_Init_Date")]
        public bool ForceErrorConfig_Init_Date { get; set; }

        [XmlElement("ForceErrorConfig_Init_Files")]
        public bool ForceErrorConfig_Init_Files { get; set; }

        // AclaraViewSettings.xaml.cs
        [XmlElement("ForceErrorConfig_Settings_Date")]
        public bool ForceErrorConfig_Settings_Date { get; set; }

        [XmlElement("ForceErrorConfig_Settings_Files")]
        public bool ForceErrorConfig_Settings_Files { get; set; }

        #endregion

        #region MTUs

        // InterfaceAux.cs
        [XmlElement("ForceMtu_UnknownMap")]
        public bool ForceMtu_UnknownMap { get; set; }

        #endregion
    }
}
