using System;
using System.Collections.Generic;
using System.Text;
using Xml;

namespace MTUComm.actions
{
    public class AddMtuForm : MtuForm
    {
        public enum FIELD
        {
            SERVICE_PORT_ID,
            FIELD_ORDER,
            METER_NUMBER,
            SELECTED_METER_ID,
            READ_INTERVAL,
            SNAP_READS,
            TWO_WAY,
            ALARM
        }

        public static Dictionary<FIELD, string[]> Texts =
            new Dictionary<FIELD, string[]>()
            {
                {
                    FIELD.SERVICE_PORT_ID,
                    new string[]
                    {
                        "ServicePortId",
                        "ServicePortId",
                        "Service Port ID"
                    }
                },
                {
                    FIELD.FIELD_ORDER,
                    new string[]
                    {
                        "FieldOrder",
                        "FieldOrder",
                        "Field Order"
                    }
                },
                {
                    FIELD.METER_NUMBER,
                    new string[]
                    {
                        "MeterNumber",
                        "MeterNumber",
                        "Meter Number"
                    }
                },
                {
                    FIELD.SELECTED_METER_ID,
                    new string[]
                    {
                        "Meter",
                        "SelectedMeterId",
                        "Selected Meter ID"
                    }
                },
                {
                    FIELD.READ_INTERVAL,
                    new string[]
                    {
                        "ReadInterval",
                        "ReadInterval",
                        "Read Interval"
                    }
                },
                {
                    FIELD.SNAP_READS,
                    new string[]
                    {
                        "SnapReads",
                        "SnapReads",
                        "Snap Reads"
                    }
                },
                {
                    FIELD.TWO_WAY,
                    new string[]
                    {
                        "TwoWay",
                        "TwoWay",
                        "2-Way"
                    }
                },
                {
                    FIELD.ALARM,
                    new string[]
                    {
                        "Alarm",
                        "Alarms",
                        "Alarms"
                    }
                },
            };

        public AddMtuForm(Mtu mtu) : base( mtu )
        {
        }

        public void AddParameter (FIELD field_type, dynamic value)
        {
            string[] texts = Texts[field_type];
            AddParameter ( texts[ 0 ], texts[ 1 ], texts[ 2 ], value ); // base method
        }

        public Parameter FindById(FIELD field_type)
        {
            string[] texts = Texts[field_type];
            return base.FindParameterById(texts[0]);
        }
    }
}
