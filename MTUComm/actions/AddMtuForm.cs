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

        public enum FIELD_CONDITIONS
        {
            MTU_TWO_PORTS,
            MTU_REQUIRES_ALARM_PROFILE,
            MTU_MTU_DEMAND
        }

        public static Dictionary<FIELD_CONDITIONS,bool> Conditions_Mtu =
            new Dictionary<FIELD_CONDITIONS,bool>()
            {
                {
                    FIELD_CONDITIONS.MTU_TWO_PORTS,
                    false
                },
                {
                    FIELD_CONDITIONS.MTU_REQUIRES_ALARM_PROFILE,
                    false
                },
                {
                    FIELD_CONDITIONS.MTU_MTU_DEMAND,
                    false
                }
            };

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
            // PARA LAS CONDICIONES SE USA MTU Y GLOBALS
            // LO QUE SE PUEDE HACER ES RECUPERAR TODOS LOS MIEMBROS/VARIABLES
            // POR REFLEXION Y LOS QUE NO EXISTAN EN LOS OBJETOS, SERA PORQUE
            // NO SE VAYAN A USAR, AL PERTENECER A OTRAS FAMILIAS, MODELOS...

        }

        public void AddParameter (FIELD field_type, dynamic value)
        {
            string[] texts = Texts[field_type];
            AddParameter ( texts[ 0 ], texts[ 1 ], texts[ 2 ], value ); // base method
        }

        public Parameter FindById(FIELD field_type)
        {
            string[] texts = Texts[field_type];
            return base.FindById(texts[0]);
        }

        /*
        public class Condition
        {
            public bool this[ FIELD_CONDITIONS ]
            {
                get { return  }
            }
        }
        */

        public void SetCondition ( FIELD_CONDITIONS field, bool value )
        {
            Conditions_Mtu[ field ] = value;
        }

        public bool GetCondition ( FIELD_CONDITIONS field )
        {
            return Conditions_Mtu[ field ];
        }

        /*public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var p in paramsList)
            {
                builder.AppendLine(p.CustomParameter + " | " + p.CustomDisplay);
            }
            return builder.ToString();
        }*/
    }
}
