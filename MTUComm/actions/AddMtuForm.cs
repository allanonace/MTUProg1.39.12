using System;
using System.Collections.Generic;
using System.Text;
using Xml;

using ParameterType = MTUComm.Parameter.ParameterType;

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
            GPS_LATITUDE,
            GPS_LONGITUDE,
            GPS_ALTITUDE,
            OPTIONAL_PARAMS
        }

        public Dictionary<ParameterType,FIELD> IdsAclara =
            new Dictionary<ParameterType,FIELD> ()
            {
                { ParameterType.ActivityLogId,     FIELD.SERVICE_PORT_ID }, // SE USA
                { ParameterType.WorkOrder,         FIELD.FIELD_ORDER     },
                { ParameterType.MeterSerialNumber, FIELD.METER_NUMBER    },
                { ParameterType.MeterReading,      FIELD.INITIAL_READING }, // SE USA
                { ParameterType.MeterType,         FIELD.SELECTED_METER  }, // SE USA
                { ParameterType.SnapRead,          FIELD.SNAP_READS      },
                { ParameterType.Custom,            FIELD.OPTIONAL_PARAMS },
                { ParameterType.ReadInterval,      FIELD.READ_INTERVAL   }, // SE USA
                { ParameterType.Alarm,             FIELD.ALARM           }
            };

        // Elements array
        // 0. Parameter ID
        // 1. Custom parameter
        // 2. Custom display
        public Dictionary<FIELD, string[]> Texts =
            new Dictionary<FIELD, string[]>()
            {
                #region Service Port ID = Account Number = Activity Log ID
                {
                    FIELD.SERVICE_PORT_ID,
                    new string[]
                    {
                        "ServicePortId",
                        "AccountNumber",
                        "Service Port ID"
                    }
                },
                {
                    FIELD.SERVICE_PORT_ID2,
                    new string[]
                    {
                        "ServicePortId2",
                        "AccountNumber",
                        "Service Port ID"
                    }
                },
                #endregion
                #region Field Order
                {
                    FIELD.FIELD_ORDER,
                    new string[]
                    {
                        "FieldOrder",
                        "WorkOrder",
                        "Field Order"
                    }
                },
                {
                    FIELD.FIELD_ORDER2,
                    new string[]
                    {
                        "FieldOrder2",
                        "WorkOrder",
                        "Field Order"
                    }
                },
                #endregion
                #region Meter Number
                {
                    FIELD.METER_NUMBER,
                    new string[]
                    {
                        "MeterNumber",
                        "NewMeterSerialNumber",
                        "New Meter Serial Number"
                    }
                },
                {
                    FIELD.METER_NUMBER2,
                    new string[]
                    {
                        "MeterNumber2",
                        "NewMeterSerialNumber",
                        "New Meter Serial Number"
                    }
                },
                #endregion
                #region Initial Reading
                {
                    FIELD.INITIAL_READING,
                    new string[]
                    {
                        "InitialReading",
                        "MeterReading",
                        "Meter Reading"
                    }
                },
                {
                    FIELD.INITIAL_READING2,
                    new string[]
                    {
                        "InitialReading2",
                        "MeterReading",
                        "Meter Reading"
                    }
                },
                #endregion
                #region Selected Meter
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
                #endregion
                #region Read Interval
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
                        "ReadInterval 2",
                        "Read Interval 2"
                    }
                },
                #endregion
                #region Snap Reads
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
                #endregion
                #region 2Way
                {
                    FIELD.TWO_WAY,
                    new string[]
                    {
                        "TwoWay",
                        "Fast-2-Way",
                        "Fast Message Config"
                    }
                },
                {
                    FIELD.TWO_WAY2,
                    new string[]
                    {
                        "TwoWay2",
                        "Fast-2-Way 2",
                        "Fast Message Config 2"
                    }
                },
                #endregion
                #region Alarm
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
                #endregion
                #region Demand
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
                #endregion
                #region GPS
                {
                    FIELD.GPS_LATITUDE,
                    new string[]
                    {
                        "GPSLat",
                        "GPS_Y",
                        "Lat"
                    }
                },
                {
                    FIELD.GPS_LONGITUDE,
                    new string[]
                    {
                        "GPSLon",
                        "GPS_X",
                        "Long"
                    }
                },
                {
                    FIELD.GPS_ALTITUDE,
                    new string[]
                    {
                        "GPSAlt",
                        "Altitude",
                        "Elevation"
                    }
                },
                #endregion
                #region Optional Parameters
                {
                    FIELD.OPTIONAL_PARAMS,
                    new string[]
                    {
                        "OptionalParams",
                        "OptionalParams",
                        "OptionalParams"
                    }
                }
                #endregion
            };

        public AddMtuForm ( Mtu mtu ) : base ( mtu )
        {
            // Two Ports
            base.conditions.mtu.AddCondition("TwoPorts");

            // Field order (work order)
            base.conditions.globals.AddCondition("WorkOrderRecording");

            // Meter Number (serial number)
            base.conditions.globals.AddCondition("UseMeterSerialNumber");

            // Vendor / Model / Name
            base.conditions.globals.AddCondition("ShowMeterVendor");

            // Read Interval 
            base.conditions.globals.AddCondition("IndividualReadInterval");

            // Snap Reads
            base.conditions.globals.AddCondition("AllowDailyReads");
            base.conditions.globals.AddCondition("IndividualDailyReads");
            base.conditions.mtu.AddCondition("DailyReads");

            // 2-Way
            base.conditions.globals.AddCondition("FastMessageConfig");
            base.conditions.globals.AddCondition("Fast2Way");
            base.conditions.mtu.AddCondition("FastMessageConfig");

            // Alarms
            base.conditions.mtu.AddCondition("RequiresAlarmProfile");

            // Demands
            base.conditions.mtu.AddCondition("MtuDemand");
        }

        public void AddParameter ( FIELD fieldType, dynamic value )
        {
            string[] texts = Texts[ fieldType ];
            AddParameter ( texts[ 0 ], texts[ 1 ], texts[ 2 ], value ); // base method
        }

        public void AddParameters ( FIELD fieldType, Parameter[] parameters )
        {
            foreach ( Parameter parameter in parameters )
                this.AddParameter ( fieldType, parameter );
        }

        public void AddParameterTranslatingAclaraXml ( Parameter parameter )
        {
            ParameterType typeAclara;
            FIELD typeOwn;

            string nameTypeAclara = parameter.Type.ToString ();

            // Translate aclara tag/id to us
            if ( ! Enum.TryParse<ParameterType> ( nameTypeAclara, out typeAclara ) )
                return; //throw new Exception ();
            else
            {
                if ( IdsAclara.ContainsKey ( typeAclara ) )
                    typeOwn = IdsAclara[ typeAclara ];
                else
                    return;

                // If is for port two, find the correct enum element adding two ( '2' ) as sufix
                if ( parameter.Port == 2 )
                    Enum.TryParse<FIELD> ( typeOwn.ToString () + "2", out typeOwn );
            }

            this.AddParameter ( typeOwn, parameter.getValue () );
        }

        public Parameter FindById(FIELD field_type)
        {
            string[] texts = Texts[field_type];
            return base.FindParameterById(texts[0]);
        }

        public bool ContainsParameter ( FIELD fieldType )
        {
            return base.ContainsParameter ( Texts[ fieldType ][ 0 ] );
        }
    }
}
