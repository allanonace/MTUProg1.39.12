using System;
using System.Collections.Generic;
using System.Text;
using Xml;
using Library.Exceptions;
using System.Linq;

using ParameterType = MTUComm.Parameter.ParameterType;
using Library;

namespace MTUComm.actions
{
    public class AddMtuForm : MtuForm
    {
        public enum FIELD
        {
            NOTHING,
        
            MTU_ID_OLD,
            
            ACCOUNT_NUMBER,
            ACCOUNT_NUMBER_2,
            
            WORK_ORDER,
            WORK_ORDER_2,
            
            ACTIVITY_LOG_ID,
            
            METER_NUMBER,
            METER_NUMBER_2,
            METER_NUMBER_OLD,
            METER_NUMBER_OLD_2,
            
            METER_READING,
            METER_READING_2,
            METER_READING_OLD,
            METER_READING_OLD_2,
            
            METER_WORKING_OLD,
            METER_WORKING_OLD_2,
            REPLACE_METER_REG,
            REPLACE_METER_REG_2,
            
            METER_TYPE,
            METER_TYPE_2,
            
            NUMBER_OF_DIALS,
            NUMBER_OF_DIALS_2,
            DRIVE_DIAL_SIZE,
            DRIVE_DIAL_SIZE_2,
            UNIT_MEASURE,
            UNIT_MEASURE_2,
            
            READ_INTERVAL,
            SNAP_READS,
            TWO_WAY,
            ALARM,
            DEMAND,
            
            GPS_LATITUDE,
            GPS_LONGITUDE,
            GPS_ALTITUDE,
            OPTIONAL_PARAMS,
            FORCE_TIME_SYNC,
            
            TEST
        }

        private const string PORT_2_SUFIX = "_2";

        private Dictionary<ParameterType,FIELD> IdsAclara =
            new Dictionary<ParameterType,FIELD> ()
            {
                { ParameterType.OldMtuId,             FIELD.MTU_ID_OLD        },
                { ParameterType.AccountNumber,        FIELD.ACCOUNT_NUMBER    },
                { ParameterType.ActivityLogId,        FIELD.ACTIVITY_LOG_ID   },
                { ParameterType.WorkOrder,            FIELD.WORK_ORDER        },
                { ParameterType.MeterType,            FIELD.METER_TYPE        },
                { ParameterType.NumberOfDials,        FIELD.NUMBER_OF_DIALS   },
                { ParameterType.DriveDialSize,        FIELD.DRIVE_DIAL_SIZE   },
                { ParameterType.UnitOfMeasure,        FIELD.UNIT_MEASURE      },
                { ParameterType.SnapRead,             FIELD.SNAP_READS        },
                { ParameterType.Custom,               FIELD.OPTIONAL_PARAMS   },
                { ParameterType.ReadInterval,         FIELD.READ_INTERVAL     },
                { ParameterType.Alarm,                FIELD.ALARM             },
                { ParameterType.ForceTimeSync,        FIELD.FORCE_TIME_SYNC   },
                
                { ParameterType.MeterSerialNumber,    FIELD.METER_NUMBER      },
                { ParameterType.NewMeterSerialNumber, FIELD.METER_NUMBER      },
                { ParameterType.OldMeterSerialNumber, FIELD.METER_NUMBER_OLD  },
                
                { ParameterType.MeterReading,         FIELD.METER_READING     },
                { ParameterType.NewMeterReading,      FIELD.METER_READING     },
                { ParameterType.OldMeterReading,      FIELD.METER_READING_OLD }
            };

        // Elements array
        // 0. Parameter ID     = dynamicMap.id
        // 1. Custom parameter = <name>
        // 2. Custom display   = <display>
        private Dictionary<FIELD, string[]> Texts;

        public bool usePort2;
        private Dictionary<FIELD,Parameter> dictionary;

        public int NumOfParamRegistered
        {
            get { return this.dictionary.Count; }
        }

        public Dictionary<FIELD,Parameter> RegisteredParamsByField
        {
            get { return new Dictionary<FIELD,Parameter> ( this.dictionary ); }
        }

        public AddMtuForm ( Mtu mtu ) : base ( mtu )
        {
            this.dictionary = new Dictionary<FIELD,Parameter> ();

            Global global = Singleton.Get.Configuration.Global;

            this.Texts =
            new Dictionary<FIELD, string[]>()
            {
                #region Service Port ID = Account Number = Functl Loctn
                {
                    FIELD.ACCOUNT_NUMBER,
                    new string[]
                    {
                        "AccountNumber",
                        "AccountNumber",
                        global.AccountLabel

                    }
                },
                {
                    FIELD.ACCOUNT_NUMBER_2,
                    new string[]
                    {
                        "AccountNumber_2",
                        "AccountNumber",
                        global.AccountLabel
                    }
                },
                #endregion
                #region Field Order = Work Order
                {
                    FIELD.WORK_ORDER,
                    new string[]
                    {
                        "WorkOrder",
                        "WorkOrder",
                        "Field Order"
                    }
                },
                {
                    FIELD.WORK_ORDER_2,
                    new string[]
                    {
                        "WorkOrder_2",
                        "WorkOrder",
                        "Field Order"
                    }
                },
                #endregion
                #region Old MTU ID
                {
                    FIELD.MTU_ID_OLD,
                    new string[]
                    {
                        "OldMtuId",
                        "OldMtuId",
                        "Old MTU ID"
                    }
                },
                #endregion
                #region Activity Log ID
                {
                    FIELD.ACTIVITY_LOG_ID,
                    new string[]
                    {
                        "ActivityLogId",
                        "ActivityLogID",
                        "Activity Log ID"
                    }
                },
                #endregion
                #region Meter Serial Number
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
                    FIELD.METER_NUMBER_2,
                    new string[]
                    {
                        "MeterNumber_2",
                        "NewMeterSerialNumber",
                        "New Meter Serial Number"
                    }
                },
                {
                    FIELD.METER_NUMBER_OLD,
                    new string[]
                    {
                        "MeterNumberOld",
                        "OldMeterSerialNumber",
                        "Old Meter Serial Number"
                    }
                },
                {
                    FIELD.METER_NUMBER_OLD_2,
                    new string[]
                    {
                        "MeterNumberOld_2",
                        "OldMeterSerialNumber",
                        "Old Meter Serial Number"
                    }
                },
                #endregion
                #region Initial Reading = Meter Reading
                {
                    FIELD.METER_READING,
                    new string[]
                    {
                        "MeterReading",
                        "NewMeterReading",
                        "New Meter Reading"
                    }
                },
                {
                    FIELD.METER_READING_2,
                    new string[]
                    {
                        "MeterReading_2",
                        "NewMeterReading",
                        "New Meter Reading"
                    }
                },
                {
                    FIELD.METER_READING_OLD,
                    new string[]
                    {
                        "MeterReadingOld",
                        "OldMeterReading",
                        "Old Meter Reading"
                    }
                },
                {
                    FIELD.METER_READING_OLD_2,
                    new string[]
                    {
                        "MeterReadingOld_2",
                        "OldMeterReading",
                        "Old Meter Reading"
                    }
                },
                #endregion
                #region Meter Type ( Meter ID )
                {
                    FIELD.METER_TYPE,
                    new string[]
                    {
                        "Meter",
                        "SelectedMeterId",
                        "Selected Meter ID"
                    }
                },
                {
                    FIELD.METER_TYPE_2,
                    new string[]
                    {
                        "Meter_2",
                        "SelectedMeterId",
                        "Selected Meter ID"
                    }
                },
                #endregion

                #region Old Meter Working
                {
                    FIELD.METER_WORKING_OLD,
                    new string[]
                    {
                        "OldMeterWorking",
                        "OldMeterWorking",
                        "Old Meter Working"
                    }
                },
                {
                    FIELD.METER_WORKING_OLD_2,
                    new string[]
                    {
                        "OldMeterWorking_2",
                        "OldMeterWorking",
                        "Old Meter Working"
                    }
                },
                #endregion
                #region Replace Meter|Register
                {
                    FIELD.REPLACE_METER_REG,
                    new string[]
                    {
                        "ReplaceMeterRegister",
                        "MeterRegisterStatus",
                        "Meter Register Status"
                    }
                },
                {
                    FIELD.REPLACE_METER_REG_2,
                    new string[]
                    {
                        "ReplaceMeterRegister_2",
                        "MeterRegisterStatus",
                        "Meter Register Status"
                    }
                },
                #endregion

                #region Number of Dials
                {
                    FIELD.NUMBER_OF_DIALS,
                    new string[]
                    {
                        "NumberOfDials",
                        "NumberOfDials",
                        "Number of Dials"
                    }
                },
                {
                    FIELD.NUMBER_OF_DIALS_2,
                    new string[]
                    {
                        "NumberOfDials_2",
                        "NumberOfDials",
                        "Number of Dials"
                    }
                },
                #endregion
                #region Drive Dial Size
                {
                    FIELD.DRIVE_DIAL_SIZE,
                    new string[]
                    {
                        "DriveDialSize",
                        "DriveDialSize",
                        "Drive Dial Size"
                    }
                },
                {
                    FIELD.DRIVE_DIAL_SIZE_2,
                    new string[]
                    {
                        "DriveDialSize_2",
                        "DriveDialSize",
                        "Drive Dial Size"
                    }
                },
                #endregion
                #region Unit of Measure
                {
                    FIELD.UNIT_MEASURE,
                    new string[]
                    {
                        "UnitOfMeasure",
                        "UnitOfMeasure",
                        "Unit of Measure"
                    }
                },
                {
                    FIELD.UNIT_MEASURE_2,
                    new string[]
                    {
                        "UnitOfMeasure_2",
                        "UnitOfMeasure",
                        "Unit of Measure"
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
                },
                #endregion
                #region Force TimeSync -> Install Confirmation
                {
                    FIELD.FORCE_TIME_SYNC,
                    new string[]
                    {
                        "ForceTimeSync",
                        "ForceTimeSync",
                        "Force TimeSync"
                    }
                },
                #endregion

                #region TEST
                {
                    FIELD.TEST,
                    new string[]
                    {
                        "Test",
                        "Test",
                        "Test"
                    }
                },
                #endregion
            };
        }

        public void AddParameter ( FIELD fieldType, dynamic value, int port = 0 )
        {
            string[] texts = Texts[ fieldType ];
            Parameter param = AddParameter ( texts[ 0 ], texts[ 1 ], texts[ 2 ], value, port ); // base method
            
            if ( ! this.dictionary.ContainsKey ( fieldType ) )
                this.dictionary.Add ( fieldType, param );
            else
                throw new SameParameterRepeatScriptException ();
        }

        public void UpdateParameter ( FIELD fieldType, dynamic value, int port = 0 )
        {
            if ( this.dictionary.ContainsKey ( fieldType ) )
                this.dictionary[ fieldType ].Value = value;
        }

        public void AddParameterTranslatingAclaraXml ( Parameter parameter )
        {
            ParameterType typeAclara;

            string nameTypeAclara = parameter.Type.ToString ();

            // Translate aclara tag/parameter to app tag
            if ( ! Enum.TryParse<ParameterType> ( nameTypeAclara, out typeAclara ) ||
                 ! IdsAclara.ContainsKey ( typeAclara ) )
            {
                // Allow non-reserved tags
                //AddAdditionalParameter ( nameTypeAclara, parameter.Value, parameter.Port );
                
                Errors.LogErrorNow ( new ProcessingParamsScriptException () );
            }
            else
            {
                FIELD typeOwn = IdsAclara[ typeAclara ];

                // If is for port two, find the correct enum element adding two ( "_2" ) as sufix
                if ( parameter.Port == 1 )
                    Enum.TryParse<FIELD> ( typeOwn.ToString () + PORT_2_SUFIX, out typeOwn );

                this.AddParameter ( typeOwn, parameter.Value, parameter.Port );
            }
        }

        public Parameter FindById ( FIELD field_type )
        {
            string[] texts = Texts[field_type];
            return base.FindParameterById(texts[0]);
        }

        public bool ContainsParameter ( FIELD fieldType )
        {
            return base.ContainsParameter ( Texts[ fieldType ][ 0 ] );
        }
        
        public void RemoveParameter ( FIELD fieldType )
        {
            base.RemoveParameter ( Texts[ fieldType ][ 0 ] );
            this.dictionary.Remove ( fieldType );
        }
        
        public void RemoveParameters ()
        {
            base.RemoveParameters ();
            this.dictionary.Clear ();
        }
    }
}
