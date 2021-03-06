using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Library;
using Library.Exceptions;
using Xml;

using ActionType    = MTUComm.Action.ActionType;
using ParameterType = MTUComm.Parameter.ParameterType;
using RDDCmd        = MTUComm.RDDStatusResult.RDDCmd;

namespace MTUComm
{
    public class ScriptAux
    {
        #region Constants

        private const string REGEX_DAYS = @"^(1|8|32|64|96)$";
        private const int    MAX_RDD_FW = 12;
        private const string PT2_SUFFIX = "_2";
        private const string HOUR       = " Hour";
        private const string HOURS      = " Hours";
        private const string MIN        = " Min";
        private const string MSG_EMPTY  = "cannot be empty";
        private const string MSG_NUMBER = "should be a valid numeric value";
        private const string MSG_ELTHAN = "should be equal to or less than {0} ( {1} )";
        private const string MSG_ELONLY = "should be equal to or less than {0}";
        private const string MSG_EQUAL  = "should be a numerical value with length equal to {0} ( {1} )";
        private const string MSG_DAILY  = "should be a valid numeric value within the range [0,23]";
        private const string MSG_HSMINS = "should be one of the possible values and using Hr/s or Min";
        private const string MSG_BOOL   = "should be 'True' or 'False'";
        private const string MSG_DAYS   = "should be one of the possible values ( 1, 8, 32, 64, 96 )";
        private const string MSG_RDD    = "should be one of the possible values ( 'CLOSE', 'OPEN', 'PARTIAL OPEN' )";
        private const string MSG_ELRDD  = "should be equal to or less than {0} or there is no default value in Global";
        private const string MSG_OMW    = "should be one of the possible values ( 'Yes', 'No', 'Broken' )";
        private const string MSG_MRR    = "should be one of the possible values ( 'Meter', 'Register', 'Both' )";
        private const string MSG_TWO    = "should be one of the possible values ( 'True' for Fast, 'False' for Slow )";

        /* Enum: OldMeterWorking
            YES    - The Meter was working correctly
            NO     - The Meter was not working correctly
            BROKEN - The Meter was damaged
        */
        public enum OldMeterWorking
        {
            YES,
            NO,
            BROKEN
        }

        /* Enum: MeterRegisterRecording
            METER    - Meter unit
            REGISTER - Register unit
            BOTH     - Both units, Meter and Register
        */
        public enum MeterRegisterRecording
        {
            METER,
            REGISTER,
            BOTH
        }

        /* Enum: TwoWay
            SLOW - Mode 0 for Slow or Off
            FAST - Mode 1 for Fast or On
        */
        public enum TwoWay
        {
            SLOW,
            FAST
        }

        /* Enum: APP_FIELD
            OldMtuId               - Old MTU ID
            AccountNumber          - Account Number
            AccountNumber_2        - Account number on the second port
            WorkOrder              - Work Order
            WorkOrder_2            - Work Order on the second port
            ActivityLogId          - Activity Log ID
            MeterNumber            - Meter Serial Number
            MeterNumber_2          - Meter Serial Number on the second port
            MeterNumberOld         - Old Meter Serial Number
            MeterNumberOld_2       - Old Meter Serial Number on the second port
            MeterReading           - Meter Reading
            MeterReading_2         - Meter Reading on the second port
            MeterReadingOld        - Old Meter Reading
            MeterReadingOld_2      - Old Meter Reading on the second port
            OldMeterWorking        - Old Meter Working
            OldMeterWorking_2      - Old Meter Working on the second port
            ReplaceMeterRegister   - Replace Meter Register
            ReplaceMeterRegister_2 - Replace Meter Register on the second port
            Meter                  - Meter Type
            Meter_2                - Meter Type on the second port
            NumberOfDials          - Number of Dials
            NumberOfDials_2        - Number of Dials on the second port
            DriveDialSize          - Drive Dial Size
            DriveDialSize_2        - Drive Dial Size on the second port
            UnitOfMeasure          - Unit of Measure
            UnitOfMeasure_2        - Unit of Measure on the second port
            ReadInterval           - Read Internval
            SnapRead               - Snap Read
            TwoWay                 - Two-Way
            Alarm                  - Alarm
            Demand                 - Demand
            ForceTimeSync          - Force TimeSync
            GpsLatitude            - GPS Latitude
            GpsLongitude           - GPS Longitude
            GpsAltitude            - GPS Altitude
            NumOfDays              - Number of Days
            RDDPosition            - Desired RDD Position
            RDDFirmware            - RDD Firmware
        */
        public enum APP_FIELD
        {
            NOTHING,

            OldMtuId,
            
            AccountNumber,
            AccountNumber_2,
            
            WorkOrder,
            WorkOrder_2,
            
            ActivityLogId,
            
            MeterNumber,
            MeterNumber_2,
            MeterNumberOld,
            MeterNumberOld_2,
            
            MeterReading,
            MeterReading_2,
            MeterReadingOld,
            MeterReadingOld_2,
            
            OldMeterWorking,
            OldMeterWorking_2,
            ReplaceMeterRegister,
            ReplaceMeterRegister_2,
            
            Meter,
            Meter_2,
            
            NumberOfDials,
            NumberOfDials_2,
            DriveDialSize,
            DriveDialSize_2,
            UnitOfMeasure,
            UnitOfMeasure_2,
            
            ReadInterval,
            SnapRead,
            TwoWay,
            Alarm,
            Demand,
            
            ForceTimeSync,

            GpsLatitude,
            GpsLongitude,
            GpsAltitude,

            NumOfDays,

            RDDPosition,
            RDDFirmware,

            Optional,
        }
        public enum APP_FIELD_MTU
        {
            OldMtuId,
            ActivityLogId,

            ReadInterval,
            SnapRead,
            TwoWay,
            Alarm,
            Demand,

            ForceTimeSync,

            GpsLatitude,
            GpsLongitude,
            GpsAltitude,

            NumOfDays,

            RDDPosition,
            RDDFirmware
        }

        public readonly static Dictionary<ParameterType,APP_FIELD> IdsAclara =
            new Dictionary<ParameterType,APP_FIELD> ()
            {
                { ParameterType.OldMtuId,             APP_FIELD.OldMtuId        },
                { ParameterType.AccountNumber,        APP_FIELD.AccountNumber   },
                { ParameterType.ActivityLogId,        APP_FIELD.ActivityLogId   },
                { ParameterType.WorkOrder,            APP_FIELD.WorkOrder       },
                { ParameterType.MeterType,            APP_FIELD.Meter           },
                { ParameterType.NumberOfDials,        APP_FIELD.NumberOfDials   },
                { ParameterType.DriveDialSize,        APP_FIELD.DriveDialSize   },
                { ParameterType.UnitOfMeasure,        APP_FIELD.UnitOfMeasure   },
                { ParameterType.SnapRead,             APP_FIELD.SnapRead       },
                { ParameterType.ReadInterval,         APP_FIELD.ReadInterval    },
                { ParameterType.Fast2Way,             APP_FIELD.TwoWay          },
                { ParameterType.TwoWay,               APP_FIELD.TwoWay          },
                { ParameterType.Alarm,                APP_FIELD.Alarm           },
                { ParameterType.ForceTimeSync,        APP_FIELD.ForceTimeSync   },
                { ParameterType.Custom,               APP_FIELD.Optional        },
                
                { ParameterType.MeterSerialNumber,    APP_FIELD.MeterNumber     },
                { ParameterType.NewMeterSerialNumber, APP_FIELD.MeterNumber     },
                { ParameterType.OldMeterSerialNumber, APP_FIELD.MeterNumberOld  },
                
                { ParameterType.MeterReading,         APP_FIELD.MeterReading    },
                { ParameterType.NewMeterReading,      APP_FIELD.MeterReading    },
                { ParameterType.OldMeterReading,      APP_FIELD.MeterReadingOld },
                
                { ParameterType.OldMeterWorking,      APP_FIELD.OldMeterWorking },
                { ParameterType.ReplaceMeterRegister, APP_FIELD.ReplaceMeterRegister },
                
                { ParameterType.DaysOfRead,           APP_FIELD.NumOfDays       },
                
                { ParameterType.RDDPosition,          APP_FIELD.RDDPosition     },
                { ParameterType.RDDFirmwareVersion,   APP_FIELD.RDDFirmware     },
            };

        #endregion

        #region Logic

        /// <summary>
        /// Verifies if the indicated name is present in the <see cref="APP_FIELD"/>
        /// enumeration and returns the name associated in the Aclara nomenclature,
        /// represented by the <see cref="Parameter.ParameterType"/> enumeration.
        /// </summary>
        /// <param name="name">Name of the parameter in the application nomenclature</param>
        /// <returns>Name of the parameter in the Aclara nomenclature.</returns>
        private static string GetAclaraName (
            APP_FIELD name )
        {
            string result       = name.ToString ();
            bool   namePort2    = result.EndsWith ( "_2" );
            string nameWithout2 = result.Replace  ( "_2", string.Empty );

            if ( Enum.TryParse ( nameWithout2, out APP_FIELD appField ) &&
                 IdsAclara.ContainsValue ( appField ) )
            {
                result = IdsAclara.First ( e => e.Value == appField ).Key.ToString ();

                if ( namePort2 )
                    result += "_2";
            }

            return result;
        }

        /// <summary>
        /// Translates the parameters in scripting mode, from the identifiers
        /// listed in the <see cref="Parameter.ParameterType"/> enumeration
        /// to the those listed in the <see cref="APP_FIELD"/> enumeration.
        /// </summary>
        /// <returns>List of parameters with translated identifiers.</returns>
        public static ( bool UsePort2InScript, Dictionary<APP_FIELD,( dynamic Value, int Port )> Data ) TranslateAclaraParams (
            Parameter[] scriptParams )
        {
            // Parameter type, value and port ( by default 0 = port 1 )
            Dictionary<APP_FIELD,( dynamic Value, int Port )> translatedParams =
                new Dictionary<APP_FIELD,( dynamic Value, int Port )> ();

            bool ok = true;
            bool usePort2 = false;
            StringBuilder stbr = new StringBuilder ();
            foreach ( Parameter param in scriptParams )
            {
                APP_FIELD paramTranslated = TranslateAclaraParam ( param );
                if ( paramTranslated != APP_FIELD.NOTHING )
                {
                    if ( ! translatedParams.ContainsKey ( paramTranslated ) )
                    {
                        translatedParams.Add ( paramTranslated, ( param.Value, param.Port ) );

                        if ( param.Port == 1 )
                            usePort2 = true;
                    }
                    // Some parameter appears more than once for the same port
                    else
                    {
                        ok = false;

                        string typeStr = paramTranslated.ToString ();

                        // XXX_2 -> P2.XXX
                        if ( typeStr.EndsWith ( "_2" ) )
                            typeStr = "P2." + typeStr.Substring ( 0, typeStr.Length - 2 );

                        stbr.Append ( ( ( stbr.Length > 0 ) ? ", " : string.Empty ) + typeStr );
                    }
                }
            }

            if ( ! ok )
            {
                string msgError = stbr.ToString ();

                int index;
                if ( ( index = msgError.LastIndexOf ( ',' ) ) > -1 )
                {
                    msgError = msgError.Substring ( 0, index ) +
                               " and" +
                               msgError.Substring ( index + 1 );
                }

                throw new ScriptRepeatedParametersException ( msgError );
            }

            return ( usePort2, translatedParams );
        }

        private static APP_FIELD TranslateAclaraParam (
            Parameter param )
        {
            ParameterType typeAclara;

            string nameTypeAclara = param.Type.ToString ();

            // Verify if the parameter type exists, without using the port2 suffix because
            // all parameters with type for second port, have also for the first one
            if ( ! Enum.TryParse<ParameterType> ( nameTypeAclara, out typeAclara ) ||
                 ! IdsAclara.ContainsKey ( typeAclara ) )
                return APP_FIELD.NOTHING;
            else
            {
                APP_FIELD typeOwn = IdsAclara[ typeAclara ];

                // If is for port two, find the correct enum element adding two ( "_2" ) as suffix
                if ( param.Port == 1 && !Enum.TryParse<APP_FIELD_MTU>(typeOwn.ToString(), out _))
                    Enum.TryParse<APP_FIELD> ( typeOwn.ToString () + PT2_SUFFIX, out typeOwn );

                return typeOwn;
            }
        }

        /// <summary>
        /// Verify the value of each of the parameters present in the script file.
        /// <para>
        /// If any parameters are not required in the current action type, they will be removed to avoid unexpected errors.
        /// </para>
        /// </summary>
        /// <param name="mtu"><see cref="Mtu"/> that represents the current MTU</param>
        /// <param name="action"><see cref="Action"/> that represents the current MTU</param>
        /// <returns>List of verified or updated parameters.</returns>
        public static Dictionary<APP_FIELD,dynamic> ValidateParams (
            Mtu mtu,
            Action action,
            ( bool usePort2InScript, Dictionary<APP_FIELD,( dynamic Value, int Port )> Data ) translatedParams,
            bool mtuPort2Enabled )
        {
            Configuration config           = Singleton.Get.Configuration;
            Global        global           = config.Global;
            ActionType    actionType       = action.Type;
            var           data             = translatedParams.Data;
            bool          usePort2InScript = translatedParams.usePort2InScript;

            List<Meter> meters;
            Port  port           = null;
            Meter meterPort1     = null;
            Meter meterPort2     = null;
            bool  autoMeterPort1 = false;
            bool  autoMeterPort2 = false;
            bool isNotWrite      = ! action.IsWrite; // These actions do not need to get the meter types
            bool isReplaceMeter  = action.IsReplaceMeter;
            bool isReplaceMtu    = action.IsReplaceMtu;

            #region Get or Auto-detect Meters

            // Only write actions ( Add and Replace ) need to detect Meters
            if ( isNotWrite )
                goto AFTER_GET_METERS;

            // Script is for one port but MTU has two and second is enabled
            if ( ! usePort2InScript &&
                 mtuPort2Enabled    && // Return true in a one port 138 MTU
                 mtu.TwoPorts )        // and for that reason I have to check also this
                throw new ScriptForOnePortButTwoEnabledException ();
            
            // Script is for two ports but MTU has not second port or is disabled
            else if ( usePort2InScript &&
                      ( ! mtu.TwoPorts ||
                        ! mtuPort2Enabled ) )
                throw new ScriptForTwoPortsButMtuOnlyOneException ();

            #region Port 1

            // Auto-detect Meter using NumberOfDials, DriveDialSize and UnitOfMeasure
            if ( ! data.ContainsKey ( APP_FIELD.Meter ) )
            {
                // Missing tags
                if ( ! data.ContainsKey ( APP_FIELD.NumberOfDials ) )
                    Errors.AddError ( new NumberOfDialsTagMissingScript () );
                
                if ( ! data.ContainsKey ( APP_FIELD.DriveDialSize ) )
                    Errors.AddError ( new DriveDialSizeTagMissingScript () );
                    
                if ( ! data.ContainsKey ( APP_FIELD.UnitOfMeasure ) )
                    Errors.AddError ( new UnitOfMeasureTagMissingScript () );
                
                if ( data.ContainsKey ( APP_FIELD.NumberOfDials ) &&
                     data.ContainsKey ( APP_FIELD.DriveDialSize ) &&
                     data.ContainsKey ( APP_FIELD.UnitOfMeasure ) )
                {
                    autoMeterPort1 = true;

                    int nod, dds;
                    string nodStr = data[ APP_FIELD.NumberOfDials ].Value;
                    string ddsStr = data[ APP_FIELD.DriveDialSize ].Value;
                    bool ok = int.TryParse ( nodStr, out nod );
                    ok     &= int.TryParse ( ddsStr, out dds );
                
                    if ( ok )
                    {
                        meters = config.MeterTypes.FindByDialDescription (
                            nod,
                            dds,
                            data[ APP_FIELD.UnitOfMeasure ].Value,
                            mtu.Flow );
                    }
                    else meters = new List<Meter> ();
    
                    // At least one Meter was found
                    if ( meters.Count > 0 )
                    {
                        meterPort1 = meters[ 0 ];
                        port       = mtu.Port1;
                
                        // Current MTU does not support selected Meter
                        if ( ! port.IsThisMeterSupported ( meterPort1 ) )
                            throw new ScriptingAutoDetectNotSupportedException ();

                        data.Add ( APP_FIELD.Meter, ( meterPort1, 0 ) );
                    }
                    
                    // No meter was found using the selected parameters
                    else throw new ScriptingAutoDetectMeterException ();
                }
                // Script does not contain some of the needed tags ( NumberOfDials,... )
                else throw new ScriptingAutoDetectTagsMissingScript ();
            }
            // Get the MTU with the selected MTU ID
            else
            {
                // NOTE: Compiling in Windows it is possible to use "int.TryParse ( data[ APP_FIELD.Meter ].Value, out int id )",
                // NOTE: but it seems that the CLang compiler used in Unix does not support any dynamic value as first argument,
                // NOTE: because it is also not allowed to use "var idStr = data[ APP_FIELD.Meter ].Value; int.TryParse ( idStr, out int id )"
                string idStr = data[ APP_FIELD.Meter ].Value;
                bool ok = int.TryParse ( idStr, out int id );

                if ( ok )
                {
                    meterPort1 = config.getMeterTypeById ( id );
                    port       = mtu.Port1;
                }
                
                // Is not valid Meter ID ( not present in Meter.xml )
                if ( ! ok ||
                     meterPort1.IsEmpty )
                    throw new ScriptingAutoDetectMeterMissing ();
                
                // Current MTU does not support selected Meter
                else if ( ! port.IsThisMeterSupported ( meterPort1 ) )
                    throw new ScriptingAutoDetectNotSupportedException ();

                data[ APP_FIELD.Meter ] = ( meterPort1, 0 );
                
                // Set values for the Meter selected
                port.MeterProtocol   = meterPort1.EncoderType;
                port.MeterLiveDigits = meterPort1.LiveDigits;
            }

            #endregion

            #region Port 2

            // Auto-detect Meter using NumberOfDials, DriveDialSize and UnitOfMeasure
            if ( mtu.TwoPorts &&
                 mtuPort2Enabled )
            {
                if ( ! data.ContainsKey ( APP_FIELD.Meter_2 ) )
                {
                    // Missing tags
                    if ( ! data.ContainsKey ( APP_FIELD.NumberOfDials_2 ) )
                        Errors.AddError ( new NumberOfDialsTagMissingScript ( string.Empty, 2 ) );
                    
                    if ( ! data.ContainsKey ( APP_FIELD.DriveDialSize_2 ) )
                        Errors.AddError ( new DriveDialSizeTagMissingScript ( string.Empty, 2 ) );
                        
                    if ( ! data.ContainsKey ( APP_FIELD.UnitOfMeasure_2 ) )
                        Errors.AddError ( new UnitOfMeasureTagMissingScript ( string.Empty, 2 ) );
                
                    if ( data.ContainsKey ( APP_FIELD.NumberOfDials_2 ) &&
                         data.ContainsKey ( APP_FIELD.DriveDialSize_2 ) &&
                         data.ContainsKey ( APP_FIELD.UnitOfMeasure_2 ) )
                    {
                        autoMeterPort2 = true;

                        int nod, dds;
                        string nodStr = data[ APP_FIELD.NumberOfDials_2 ].Value;
                        string ddsStr = data[ APP_FIELD.DriveDialSize_2 ].Value;
                        bool ok = int.TryParse ( nodStr, out nod );
                        ok     &= int.TryParse ( ddsStr, out dds );
                    
                        if ( ok )
                        {
                            meters = config.MeterTypes.FindByDialDescription (
                                nod,
                                dds,
                                data[ APP_FIELD.UnitOfMeasure_2 ].Value,
                                mtu.Flow );
                        }
                        else meters = new List<Meter> ();
                        
                        // At least one Meter was found
                        if ( meters.Count > 0 )
                        {
                            meterPort2 = meters[ 0 ];
                            port       = mtu.Port2;
                
                            // Current MTU does not support selected Meter
                            if ( ! port.IsThisMeterSupported ( meterPort2 ) )
                                throw new ScriptingAutoDetectNotSupportedException ();

                            data.Add ( APP_FIELD.Meter_2, ( meterPort2, 0 ) );
                        }
                        
                        // No meter was found using the selected parameters
                        else throw new ScriptingAutoDetectMeterException ( string.Empty, 2 );
                    }
                    // Script does not contain some of the needed tags ( NumberOfDials,... )
                    else throw new ScriptingAutoDetectTagsMissingScript ( string.Empty, 2 );
                }
                // Get the MTU with the selected MTU ID
                else
                {
                    string idStr = data[ APP_FIELD.Meter_2 ].Value;
                    bool ok = int.TryParse ( idStr, out int id );

                    if ( ok )
                    {
                        meterPort2 = config.getMeterTypeById ( id );
                        port       = mtu.Port2;
                    }
                    
                    // Is not valid Meter ID ( not present in Meter.xml )
                    if ( ! ok ||
                         meterPort1.IsEmpty )
                        throw new ScriptingAutoDetectMeterMissing ( string.Empty, 2 );
                    
                    // Current MTU does not support selected Meter
                    else if ( ! port.IsThisMeterSupported ( meterPort2 ) )
                        throw new ScriptingAutoDetectNotSupportedException ( string.Empty, 2 );
                    
                    data[ APP_FIELD.Meter_2 ] = ( meterPort2, 1 );
                    
                    // Set values for the Meter selected
                    port.MeterProtocol   = meterPort2.EncoderType;
                    port.MeterLiveDigits = meterPort2.LiveDigits;
                }
            }

            #endregion

            AFTER_GET_METERS:

            #endregion

            #region Validation

            #region Methods

            dynamic Empty = new Func<string,bool> ( ( v ) =>
                                string.IsNullOrEmpty ( v ) );
    
            dynamic EmptyNum = new Func<string,bool> ( ( v ) =>
                                    ! Validations.IsNumeric ( v ) );

            // Value equals to maximum length
            dynamic NoEqNum = new Func<string,int,bool> ( ( v, maxLength ) =>
                                ! Validations.NumericText ( v, maxLength ) );
                                
            dynamic NoEqTxt = new Func<string,int,bool> ( ( v, maxLength ) =>
                                ! Validations.Text ( v, maxLength ) );

            // Value equals or lower to maximum length
            dynamic NoELNum = new Func<string,int,bool> ( ( v, maxLength ) =>
                                ! Validations.NumericText ( v, maxLength, 1, true, true, false ) );
                                
            dynamic NoELTxt = new Func<string,int,bool> ( ( v, maxLength ) =>
                                ! Validations.Text ( v, maxLength, 1, true, true, false ) );

            #endregion
        
            // Validate each parameter and remove those that are not going to be used
            
            bool          rddIn1         = mtu.Port1.IsSetFlow;
            bool          rddIn2         = mtu.TwoPorts && mtu.Port2.IsSetFlow;
            string        valueStr       = string.Empty;
            dynamic       valueDyn       = null;
            string        msgDescription = string.Empty;
            StringBuilder msgError       = new StringBuilder ();
            StringBuilder msgErrorPopup  = new StringBuilder ();
            Dictionary<APP_FIELD,dynamic> entriesSelected = new Dictionary<APP_FIELD,dynamic> ();
            foreach ( var entry in data )
            {
                APP_FIELD type = entry.Key;
                if ( type == APP_FIELD.NOTHING )
                    continue;
                
                var  value         = entry.Value.Value;
                int  portIndex     = entry.Value.Port;
                bool fail          = false;
                bool paramPort1    = ( portIndex == 0 );
                bool HasRDD = rddIn1 || rddIn2;

                bool rddInThisPort =   paramPort1 && rddIn1 ||
                                     ! paramPort1 && rddIn2;

                // Verify if instances are null and use Empty for the strings
                // This step is important because leaving a parameter empty is strange
                // and it is preferable to notify it, not to use the default value
                if ( fail = value is null ||
                            value is string && Empty ( value ) )
                    msgDescription = MSG_EMPTY;
                else
                {
                    valueStr = value.ToString ().Trim ();

                    // Validates each parameter before continue with the action
                    // NOTE: The default values ​​were ( when necessary ) already set in MTUComm.ValidateRequiredParams
                    switch ( type )
                    {
                        // General
                        #region Activity Log Id
                        case APP_FIELD.ActivityLogId:
                        if ( fail = EmptyNum ( valueStr ) )
                            msgDescription = MSG_NUMBER;
                        break;
                        #endregion

                        // Installation - General
                        #region Two-Way [ Only Writing ]
                        case APP_FIELD.TwoWay:
                        // Do not use
                        if ( isNotWrite ||
                             ! global.TimeToSync ||
                             ! mtu.TimeToSync    ||
                             ! mtu.FastMessageConfig )
                            continue;

                        // Verify the content -> True or False or default value if ! Global.IndividualFastMessageConfig
                        if ( ! ( fail = ! PrepareTwoWay ( mtu, ref valueStr ) ) )
                        {
                            // In interactive the value is retrieved from a dropdownlist
                            // control, using the selection index ( 0=Fast, 1=Slow )
                            valueStr = ( ( valueStr.ToUpper ().Equals ( "TRUE" ) ) ?
                                ScriptAux.TwoWay.FAST : ScriptAux.TwoWay.SLOW ).ToString ();
                        }
                        else
                            msgDescription = MSG_TWO;
                        break;
                        #endregion

                        // Installation - Port 1 and 2
                        #region Account Number [ Only Writing ]
                        case APP_FIELD.AccountNumber:
                        case APP_FIELD.AccountNumber_2:
                        // Do not use
                        if ( isNotWrite )
                            continue;
                        
                        // Verify the content -> Numeric value
                        else if ( fail = EmptyNum ( valueStr ) )
                            msgDescription = MSG_NUMBER;

                        // NOTE: In scripted mode not taking into account global.AccountLength
                        //if ( fail = NoEqNum ( value, global.AccountLength ) )
                        //    msgDescription = "should be equal to global.AccountLength (" + global.AccountLength + ")";
                        break;
                        #endregion
                        #region Work Order [ Only Writing ]
                        case APP_FIELD.WorkOrder:
                        case APP_FIELD.WorkOrder_2:
                        // Do not use
                        if ( isNotWrite ||
                             ! global.WorkOrderRecording )
                            continue;

                        // Verify the content -> Length [1,global.WorkOrderLength]
                        else if ( fail = NoELTxt ( valueStr, global.WorkOrderLength ) )
                            msgDescription =
                                String.Format ( MSG_ELTHAN, "Global.WorkOrderLength", global.WorkOrderLength );
                        break;
                        #endregion

                        // Installation - General - No RDD
                        #region Read Interval [ Only Writing, No RDD port 1 ]
                        case APP_FIELD.ReadInterval:
                        // Do not use
                        if ( rddIn1 ||
                             isNotWrite )
                            continue;

                        // Verify the content -> Value in the generated list or default value if ! Global.IndividualReadInterval
                        else if ( fail = ! PrepareReadIntervalList ( mtu, ref valueStr ) )
                            msgDescription = MSG_HSMINS;
                        break;
                        #endregion
                        #region Snap Read [ Only Writing, No RDD port 1 ]
                        case APP_FIELD.SnapRead:
                        // Do not use
                        if ( rddIn1 ||
                             mtu.IsFamily33xx ||
                             isNotWrite ||
                             ! global.AllowDailyReads ||
                             ! mtu.DailyReads )
                            continue;

                        // Verify the content -> Value [0,23] or default value if ! Global.IndividualDailyReads
                        else if ( fail = ! PrepareDailyReadValue ( mtu, ref valueStr ) )
                            msgDescription = MSG_DAILY;
                        break;
                        #endregion
                        
                        // Installation - General - No RDD - Replace MTU
                        #region Old MTU Id [ Only Writing, No RDD port 1 ]
                        case APP_FIELD.OldMtuId:
                        // Do not use
                        if ( rddIn1 ||
                             ! isReplaceMtu )
                            continue;
                        
                        // Verify the content -> Length = global.MtuIdLength
                        else if ( fail = NoEqNum ( valueStr, global.MtuIdLength ) )
                            msgDescription =
                                String.Format ( MSG_EQUAL, "Global.MtuIdLength", global.MtuIdLength );
                        break;
                        #endregion

                        // Installation - Port 1 and 2 - No RDD
                        #region Meter Serial Number [ Only Writing, No RDD ]
                        case APP_FIELD.MeterNumber:
                        case APP_FIELD.MeterNumber_2:
                        // Do not use
                        if ( rddInThisPort ||
                             isNotWrite ||
                             ! global.UseMeterSerialNumber )
                            continue;
                        
                        // Verify the content -> Length [1,global.MeterNumberLength]
                        else if ( fail = NoELTxt ( valueStr, global.MeterNumberLength ) )
                            msgDescription =
                                String.Format ( MSG_ELTHAN, "Global.MeterNumberLength", global.MeterNumberLength );
                        break;
                        #endregion
                        #region Meter Reading [ Only Writing, Pulse ]
                        case APP_FIELD.MeterReading:
                        case APP_FIELD.MeterReading_2:
                        // Do not use
                        if ( isNotWrite ||
                               paramPort1 && ! mtu.Port1.IsForPulse ||
                             ! paramPort1 && ! mtu.Port2.IsForPulse )
                            continue;

                        Meter meter         = ( paramPort1 ) ? meterPort1     : meterPort2;
                        bool  autoMeterPort = ( paramPort1 ) ? autoMeterPort1 : autoMeterPort2;

                        // In interactive, the value length must be equal to Meter.LiveDigits ( Range: [1-12] )

                        // In scripted mode and specifying the Meter ID, the value length must be equal to or
                        // less than Meter.LiveDigits and if necessary fill in left to 0's up to LiveDigits
                        // e.g. Meter 3101 LiveDigits 6 Value 1234 = 001234
                        if ( ! autoMeterPort )
                            valueStr = meter.FillLeftLiveDigits ( valueStr );

                        // In scripted mode and auto-detecting the Meter, the value length must be equal to or
                        // less than Meter.NumberOfDials and if necessary fill in left to 0's up to NumberOfDials
                        // and then add Meter.Mask at the end and the resulting length
                        // In the mask the 'X' character is replaced by the value
                        // e.g. NumberOfDials 4 Value 1234 Mask X00 -> 1234 + Mask = 123400
                        // e.g. NumberOfDials 4 Value 12 Mask X00 -> Fill left to 0's = 0012 + Mask = 001200
                        else if ( meter.NumberOfDials > -1 &&
                                  ! NoELNum ( valueStr, meter.NumberOfDials ) )
                        {
                            valueStr = meter.FillLeftNumberOfDials ( valueStr );

                            // Only apply the mask when the length of the resulted
                            // value of the previous step is less than LiveDigits
                            // e.g.
                            // Value 12
                            // 3105: NoD 5, X00, LD 7 -> Fill 00012  -> Length 5 < 7 -> Mask 0001200
                            // 3108: NoD 6, X00, LD 6 -> Fill 000012 -> Length 6 < 6 -> Mask not necessary
                            if ( valueStr.Length < meter.LiveDigits )
                                valueStr = meter.ApplyReadingMask ( valueStr );
                        }
                        
                        // The resulting value length will be equal to LiveDigits
                        if ( fail = NoEqNum ( valueStr, meter.LiveDigits ) )
                        {
                            if ( ! autoMeterPort )
                                 msgDescription = String.Format ( MSG_ELTHAN, "Meter.LiveDigits",    meter.LiveDigits    );
                            else msgDescription = String.Format ( MSG_ELTHAN, "Meter.NumberOfDials", meter.NumberOfDials );
                        }
                        break;
                        #endregion

                        // Installation - Port 1 and 2 - No RDD - Replace Meter
                        #region Old Meter Serial Number [ Only Writing, No RDD ]
                        case APP_FIELD.MeterNumberOld:
                        case APP_FIELD.MeterNumberOld_2:
                        // Do not use
                        if ( rddInThisPort ||
                             ! isReplaceMeter ||
                             ! global.UseMeterSerialNumber )
                            continue;
                        
                        // Verify the content -> Length [1,global.MeterNumberLength]
                        else if ( fail = NoELTxt ( valueStr, global.MeterNumberLength ) )
                            msgDescription =
                                String.Format ( MSG_ELTHAN, "Global.MeterNumberLength", global.MeterNumberLength );
                        break;
                        #endregion
                        #region Old Meter Reading [ Only Writing, No RDD ]
                        case APP_FIELD.MeterReadingOld:
                        case APP_FIELD.MeterReadingOld_2:
                        // Do not use
                        if ( rddInThisPort ||
                             ! isReplaceMeter ||
                             ! global.OldReadingRecording )
                            continue;
                        
                        // Very the content -> Length [1,12]
                        else if ( fail = NoELNum ( valueStr, 12 ) )
                            msgDescription = String.Format ( MSG_ELONLY, "12" );
                        break;
                        #endregion
                        #region Old Meter Working [ Only Writing, No RDD ]
                        case APP_FIELD.OldMeterWorking:
                        case APP_FIELD.OldMeterWorking_2:
                        // Do not use
                        if ( rddInThisPort ||
                             ! isReplaceMeter ||
                             ! global.MeterWorkRecording )
                            continue;
                        
                        // Very the content -> Value { Yes, No, Broken }
                        else if ( fail = ! ( Enum.TryParse<OldMeterWorking> (
                                                valueStr.ToUpper (), out var _ ) ) )
                            msgDescription = MSG_OMW;

                        // Prepare the string with only the first character as uppercase
                        else valueStr = Utils.OnlyFirstCharToCapital ( valueStr );

                        break;
                        #endregion
                        #region Replace Meter|Register [ Only Writing, No RDD ]
                        case APP_FIELD.ReplaceMeterRegister:
                        case APP_FIELD.ReplaceMeterRegister_2:
                        // Do not use
                        if ( rddInThisPort ||
                             ! isReplaceMeter ||
                             ! global.RegisterRecording )
                            continue;

                        // Verify the content -> Value { Meter, Register, Both }
                        else if ( fail = ! ( Enum.TryParse<MeterRegisterRecording> (
                                                valueStr.ToUpper (), out var _ ) ) )
                            msgDescription = MSG_MRR;

                        // Prepare the string with only the first character as uppercase
                        else valueStr = Utils.OnlyFirstCharToCapital ( valueStr );

                        break;
                        #endregion
                        
                        // Installation - Meter Type
                        #region Meter Type [ Only Writing ]
                        case APP_FIELD.Meter:
                        case APP_FIELD.Meter_2:
                        // Do not use
                        if ( isNotWrite )
                            continue;
                        valueDyn = value; // It is an instance/object
                        break;
                        #endregion
                        // NOTE: It is not necessary to verify these parameters because they
                        // NOTE: are already used in the previous region "Get or Auto-detect Meters"
                        #region Auto-detect Meter [ Only Writing ]
                        case APP_FIELD.NumberOfDials:
                        case APP_FIELD.NumberOfDials_2:
                        case APP_FIELD.DriveDialSize:
                        case APP_FIELD.DriveDialSize_2:
                        continue;
                        /*
                        // Do not use
                        if ( isNotWrite )
                            continue;

                        if ( fail = EmptyNum ( valueStr ) )
                            msgDescription = MSG_NUMBER;
                        break;
                        */
                        
                        case APP_FIELD.UnitOfMeasure:
                        case APP_FIELD.UnitOfMeasure_2:
                        continue;
                        /*
                        // Do not use
                        if ( isNotWrite )
                            continue;
                        break;
                        */
                        #endregion

                        // RF-Check
                        #region Force Time Sync [ Only Writing ]
                        case APP_FIELD.ForceTimeSync:
                        // Do not use
                        if ( isNotWrite ||
                             ! global.TimeToSync ||
                             ! mtu.TimeToSync    ||
                             ! mtu.OnTimeSync )
                            continue;

                        // Verify the content -> True or False
                        else if ( fail = ! bool.TryParse ( valueStr, out bool _ ) )
                            msgDescription = MSG_BOOL;
                        break;
                        #endregion
                        
                        // Historical Read
                        #region Number of Days [ Only DataRead ]
                        case APP_FIELD.NumOfDays:
                        // Do not use
                        if ( actionType != ActionType.DataRead ||
                             ! mtu.MtuDemand ||
                             ! mtu.DataRead )
                            continue;

                        // Verify the content -> Value { 1, 8, 32, 64, 96 }
                        else if ( fail = ( EmptyNum ( valueStr ) ||
                                  ! Regex.IsMatch ( valueStr, REGEX_DAYS ) ) )
                            msgDescription = MSG_DAYS;
                        break;
                        #endregion
                        
                        // Installation or RemoteDisconnect
                        #region Valve Position [ Writing or RemoteDisconnect ]
                        case APP_FIELD.RDDPosition:
                        // Do not use
                        if ( ! HasRDD ||
                             isNotWrite &&
                             actionType != ActionType.ValveOperation )
                            continue;

                        // Verify the content -> Value { CLOSE, OPEN, PARTIAL_OPEN }
                        else if ( fail = ! ( Enum.TryParse<RDDCmd> (
                                            valueStr.ToUpper ().Replace ( " ", "_" ), out var cmd_rdd ) &&
                                        cmd_rdd != RDDCmd.SEDIMENT_TURN &&
                                        cmd_rdd != RDDCmd.UNKNOWN ) )
                            msgDescription = MSG_RDD;
                        break;
                        #endregion
                        #region RDD Firmware [ Writing or RemoteDisconnect ]
                        case APP_FIELD.RDDFirmware:
                        // Do not use
                        if ( ! HasRDD ||
                             isNotWrite &&
                             actionType != ActionType.ValveOperation )
                            continue;
                        
                        // Verify the content -> Length [1,12]
                        else if ( fail = NoELTxt ( valueStr, MAX_RDD_FW ) )
                            msgDescription = String.Format ( MSG_ELRDD, MAX_RDD_FW );
                        break;
                        #endregion
                    }
                }

                // Error validating a parameter
                if ( fail )
                {
                    fail = false;
                    
                    string typeStr = GetAclaraName ( type );

                    // XXX_2 -> P2.XXX
                    if ( typeStr.EndsWith ( "_2" ) )
                        typeStr = "P2." + typeStr.Substring ( 0, typeStr.Length - 2 );

                    msgErrorPopup.Append ( ( ( msgError.Length > 0 ) ? ", " : string.Empty ) +
                                           typeStr + " " + msgDescription );

                    msgError.Append ( ( ( msgError.Length > 0 ) ? ", " : string.Empty ) + typeStr );
                }
                // Parameter validated and selected
                else
                    entriesSelected.Add ( type, ( valueDyn == null ) ? valueStr : valueDyn );
                
                valueDyn = null;
            }

            if ( msgError.Length > 0 )
            {
                string msgErrorStr      = msgError     .ToString ();
                string msgErrorPopupStr = msgErrorPopup.ToString ();
                msgError     .Clear ();
                msgErrorPopup.Clear ();
                msgError      = null;
                msgErrorPopup = null;
            
                int index;
                if ( ( index = msgErrorStr.LastIndexOf ( ',' ) ) > -1 )
                {
                    msgErrorStr = msgErrorStr.Substring ( 0, index ) +
                                  " and" +
                                  msgErrorStr.Substring ( index + 1 );
                    
                    index = msgErrorPopupStr.LastIndexOf ( ',' );
                    msgErrorPopupStr = msgErrorPopupStr.Substring ( 0, index ) +
                                       " and" +
                                       msgErrorPopupStr.Substring ( index + 1 );
                }

                throw new ProcessingParamsScriptException ( msgErrorStr.TrimEnd (), 1, msgErrorPopupStr.TrimEnd () );
            }

            #endregion

            #region Alarms and Demands

            // Only write actions ( Add and Replace ) need to detect Meters
            if ( isNotWrite )
                goto AFTER_ALARMS_DEMANDS;
            
            // Auto-detect scripting Alarm profile
            if ( mtu.RequiresAlarmProfile )
            {
                Alarm alarm = config.Alarms.FindByMtuType_Scripting ( ( int )Data.Get.MtuBasicInfo.Type );
                if ( alarm != null )
                    entriesSelected.Add ( APP_FIELD.Alarm, alarm );
                
                // For the current MTU does not exist "Scripting" profile inside Alarm.xml
                else throw new ScriptingAlarmForCurrentMtuException ();
            }

            // Auto-detect scripting Demand profile
            if ( mtu.MtuDemand &&
                 mtu.FastMessageConfig )
            {
                Demand demand = config.Demands.FindByMtuType_Scripting ( ( int )Data.Get.MtuBasicInfo.Type );
                if ( demand != null )
                    entriesSelected.Add ( APP_FIELD.Demand, demand );
                
                // For the current MTU does not exist "Scripting" profile inside DemandConf.xml
                else throw new ScriptingDemandForCurrentMtuException ();
            }

            AFTER_ALARMS_DEMANDS:

            #endregion

            return entriesSelected;
        }

        private static bool PrepareTwoWay (
            Mtu mtu,
            ref string value )
        {
            Global global = Singleton.Get.Configuration.Global;

            // Use default value
            // This case never fails, always setting a "correct" value
            if ( ! global.IndividualFastMessageConfig )
                value = global.FastMessageConfig.ToString ();
            
            // Use the value specified
            // This case can fail
            // NOTE: Method bool.TryParse is not case-sensitive
            else if ( ! bool.TryParse ( value, out bool _ ) )
                return false;

            // Valid value
            return true;
        }

        public static bool PrepareReadIntervalList (
            Mtu mtu,
            ref string value )
        {
            Global global = Singleton.Get.Configuration.Global;

            List<string> list;
            if ( Data.Get.MtuBasicInfo.version >= global.LatestVersion )
            {
                list = new List<string>()
                {
                    "24" + HOURS,
                    "12" + HOURS,
                    "8"  + HOURS,
                    "6"  + HOURS,
                    "4"  + HOURS,
                    "3"  + HOURS,
                    "2"  + HOURS,
                    "1"  + HOUR,
                    "30" + MIN,
                    "20" + MIN,
                    "15" + MIN
                };
            }
            else
            {
                list = new List<string>()
                {
                    "1"  + HOUR,
                    "30" + MIN,
                    "20" + MIN,
                    "15" + MIN
                };
            }
            
            // TwoWay MTU reading interval cannot be less than 15 minutes
            if ( ! mtu.TimeToSync )
                list.AddRange ( new string[]{
                    "10" + MIN,
                    "5"  + MIN
                });
            
            // Use default value
            // This case never fails, always setting a correct value
            if ( ! global.IndividualReadInterval )
            {
                // If tag NormXmitInterval is present inside Global,
                // its value is used as default selection
                string normXmitInterval = global.NormXmitInterval;
                if ( ! string.IsNullOrEmpty ( normXmitInterval ) )
                {
                    // Convert "Hr/s" to "Hour/s"
                    normXmitInterval = normXmitInterval.ToLower ()
                        .Replace ( "hr", "hour" )
                        .Replace ( "h" , "H" )
                        .Replace ( "m" , "M" );

                    int index = list.IndexOf ( normXmitInterval );
                    value     = ( index > -1 ) ? normXmitInterval : "1 Hour";
                }
                // If tag NormXmitInterval is NOT present, use "1 Hour" as default value
                else value = "1 Hour";
            }
            // Use the value specified
            // This case can fail
            else
            {
                value = value.ToLower ()
                    .Replace ( "hr", "hour" )
                    .Replace ( "h" , "H" )
                    .Replace ( "m" , "M" );
                
                // Not a valid value
                if ( string.IsNullOrEmpty ( value ) ||
                     ! list.Contains ( value ) )
                    return false;
            }

            return true;
        }
    
        public static bool PrepareDailyReadValue (
            Mtu mtu,
            ref string value )
        {
            Global global = Singleton.Get.Configuration.Global;

            // Use default value
            // This case never fails, always setting a "correct" value
            // A custom default value could be incorrect and for that reason is the forceVerifyValue parameter
            if ( ! global.IndividualDailyReads )
            {
                int defDailyReads = global.DailyReadsDefault;
                value = ( ( defDailyReads >= Global.MIN_DAILY_READS &&
                            defDailyReads <= Global.MAX_DAILY_READS ) ?
                    defDailyReads : Global.DEF_DAILY_READS ).ToString ();
                
                return true;
            }

            // Use the value specified
            // This case can fail
            else if ( int.TryParse ( value, out int dailyReads ) &&
                      dailyReads >= Global.MIN_DAILY_READS &&
                      dailyReads <= Global.MAX_DAILY_READS )
                return true;

            // Not a valid value
            return false;
        }

        #endregion
    }
}
