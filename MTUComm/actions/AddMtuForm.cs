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
            SERVICE_PORT_ID2,
            FIELD_ORDER,
            FIELD_ORDER2,
            METER_NUMBER,
            METER_NUMBER2,
            INITIAL_READING,
            INITIAL_READING2,
            SELECTED_METER,
            SELECTED_METER2,
            READ_INTERVAL,
            READ_INTERVAL2,
            SNAP_READS,
            SNAP_READS2,
            TWO_WAY,
            TWO_WAY2,
            ALARM,
            ALARM2,
            DEMAND,
            DEMAND2,
            OPTIONAL_PARAMS,
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
                    FIELD.SERVICE_PORT_ID2,
                    new string[]
                    {
                        "ServicePortId2",
                        "ServicePortId2",
                        "Service Port ID 2"
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
                    FIELD.FIELD_ORDER2,
                    new string[]
                    {
                        "FieldOrder2",
                        "FieldOrder2",
                        "Field Order 2"
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
                    FIELD.METER_NUMBER2,
                    new string[]
                    {
                        "MeterNumber2",
                        "MeterNumber2",
                        "Meter Number 2"
                    }
                },
                {
                    FIELD.INITIAL_READING,
                    new string[]
                    {
                        "InitialReading",
                        "InitialReading",
                        "Initial Reading"
                    }
                },
                {
                    FIELD.INITIAL_READING2,
                    new string[]
                    {
                        "InitialReading2",
                        "InitialReading2",
                        "Initial Reading 2"
                    }
                },
                {
                    FIELD.SELECTED_METER,
                    new string[]
                    {
                        "Meter",
                        "SelectedMeterId",
                        "Selected Meter ID"
                    }
                },
                {
                    FIELD.SELECTED_METER2,
                    new string[]
                    {
                        "Meter2",
                        "SelectedMeterId2",
                        "Selected Meter ID 2"
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
                    FIELD.READ_INTERVAL2,
                    new string[]
                    {
                        "ReadInterval2",
                        "ReadInterval2",
                        "Read Interval 2"
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
                    FIELD.SNAP_READS2,
                    new string[]
                    {
                        "SnapReads2",
                        "SnapReads2",
                        "Snap Reads 2"
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
                    FIELD.TWO_WAY2,
                    new string[]
                    {
                        "TwoWay2",
                        "TwoWay2",
                        "2-Way 2"
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
                {
                    FIELD.ALARM2,
                    new string[]
                    {
                        "Alarm2",
                        "Alarms2",
                        "Alarms 2"
                    }
                },
                {
                    FIELD.DEMAND,
                    new string[]
                    {
                        "Demand",
                        "Demands",
                        "Demands"
                    }
                },
                {
                    FIELD.DEMAND2,
                    new string[]
                    {
                        "Demand2",
                        "Demands2",
                        "Demands 2"
                    }
                },
                {
                    FIELD.OPTIONAL_PARAMS,
                    new string[]
                    {
                        "OptionalParams",
                        "OptionalParams",
                        "OptionalParams"
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
