using System;
using System.Xml.Serialization;

namespace Xml
{
    public class Demand
    {
        [XmlAttribute("MTUType")]
        public int MTUType { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("BlockTime")]
        public int BlockTime { get; set; }

        [XmlElement("IntervalTime")]
        public int IntervalTime { get; set; }

        [XmlElement("AutoClear")]
        public bool AutoClear { get; set; }

        [XmlElement("ConfigReportInterval")]
        public int ConfigReportInterval { get; set; }

        [XmlElement("ConfigReportItems")]
        public string ConfigReportItemsSerialize { get; set; }

        [XmlIgnore]
        public byte[] ConfigReportItems
        {
            get
            {
                byte[] configReportItems = new byte[70];

                /* Initialize to 255 */
                for (int i = 0; i < configReportItems.Length; i++)
                {
                    configReportItems[i] = 255;
                }

                string[] items = ConfigReportItemsSerialize.Split('-');

                /* Fill with read values */
                for (int i = 0; i < items.Length; i++)
                {
                    configReportItems[i] = Convert.ToByte(items[i]);
                }

                return configReportItems;
            }
        }

        [XmlElement("MtuNumLowPriorityMsg")]
        public int MtuNumLowPriorityMsg { get; set; }

        [XmlElement("MtuPrimaryWindowInterval")]
        public int MtuPrimaryWindowInterval { get; set; }

        [XmlElement("MtuPrimaryWindowIntervalB")]
        public int MtuPrimaryWindowIntervalB { get; set; }

        [XmlElement("MtuPrimaryWindowOffset")]
        public int MtuPrimaryWindowOffset { get; set; }

        [XmlElement("MtuWindowAStart")]
        public int MtuWindowAStart { get; set; }

        [XmlElement("MtuWindowBStart")]
        public int MtuWindowBStart { get; set; }

        [XmlElement("ReadRqst01Item")]
        public int ReadRqst01Item { get; set; }

        [XmlElement("ReadRqst02Item")]
        public int ReadRqst02Item { get; set; }

        [XmlElement("ReadRqst03Item")]
        public int ReadRqst03Item { get; set; }

        [XmlElement("ReadRqst04Item")]
        public int ReadRqst04Item { get; set; }

        [XmlElement("ReadRqst05Item")]
        public int ReadRqst05Item { get; set; }

        [XmlElement("ReadRqst06Item")]
        public int ReadRqst06Item { get; set; }

        [XmlElement("ReadRqst07Item")]
        public int ReadRqst07Item { get; set; }

        [XmlElement("ReadRqst08Item")]
        public int ReadRqst08Item { get; set; }

        [XmlElement("TrendMode")]
        public string TrendModeSerialize { get; set; }

        [XmlIgnore]
        public bool TrendMode
        {
            get
            {
                return ! string.IsNullOrEmpty ( this.TrendModeSerialize ) &&
                       this.TrendModeSerialize.ToLower ().Equals ( "enable" );
            }
        }

        [XmlElement("TrendModeReadInterval")]
        public int TrendModeReadInterval { get; set; }

        [XmlElement("TrendModeTrig1")]
        public int TrendModeTrig1 { get; set; }

        [XmlElement("TrendModeTrig2")]
        public int TrendModeTrig2 { get; set; }
    }
}
