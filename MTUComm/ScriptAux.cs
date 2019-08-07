using System;
using System.Collections.Generic;
using System.Text;
using Library;
using Library.Exceptions;
using Xml;

using ActionType    = MTUComm.Action.ActionType;
using ParameterType = MTUComm.Parameter.ParameterType;

namespace MTUComm
{
    public class ScriptAux
    {
        #region Constants

        private const string PT2_SUFIX  = "_2";
        private const string HOUR       = " Hour";
        private const string HOURS      = " Hours";
        private const string MIN        = " Min";
        private const string MSG_EMPTY  = "cannot be empty";
        private const string MSG_NUMBER = "should be a valid numeric value";
        private const string MSG_ELTHAN = "should be equal to or less than {0} ({1})";
        private const string MSG_ELONLY = "should be equal to or less than {0}";
        private const string MSG_EQUAL  = "should be equal to {0} ({1})";
        private const string MSG_HSMINS = "should be one of the possible values and using Hr/s or Min";
        private const string MSG_BOOL   = "should be 'true' or 'false'";
        private const string MSG_DAYS   = "should be one of the possible values ( 1, 8, 32, 64 or 96 )";

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
            SnapReads,
            TwoWay,
            Alarm,
            //Demand,
            
            ForceTimeSync,

            NumOfDays
        }

        public static Dictionary<ParameterType,APP_FIELD> IdsAclara =
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
                { ParameterType.SnapRead,             APP_FIELD.SnapReads       },
                { ParameterType.ReadInterval,         APP_FIELD.ReadInterval    },
                { ParameterType.Alarm,                APP_FIELD.Alarm           },
                { ParameterType.ForceTimeSync,        APP_FIELD.ForceTimeSync   },
                { ParameterType.MeterSerialNumber,    APP_FIELD.MeterNumber     },
                { ParameterType.NewMeterSerialNumber, APP_FIELD.MeterNumber     },
                { ParameterType.OldMeterSerialNumber, APP_FIELD.MeterNumberOld  },
                { ParameterType.MeterReading,         APP_FIELD.MeterReading    },
                { ParameterType.NewMeterReading,      APP_FIELD.MeterReading    },
                { ParameterType.OldMeterReading,      APP_FIELD.MeterReadingOld },
                { ParameterType.DaysOfRead,           APP_FIELD.NumOfDays       }
            };

        #endregion

        #region Logic

        public static ( bool UsePort2, Dictionary<APP_FIELD,( dynamic Value, int Port )> Data ) TranslateAclaraParams (
            Parameter[] scriptParams )
        {
            Dictionary<APP_FIELD,( dynamic Value, int Port )> translatedParams = new Dictionary<APP_FIELD,( dynamic Value, int Port )> ();

            bool usePort2 = false;
            foreach ( Parameter param in scriptParams )
            {
                APP_FIELD paramTranslated = TranslateAclaraParam ( param );
                if ( paramTranslated != APP_FIELD.NOTHING )
                {
                    translatedParams.Add ( paramTranslated, ( param.Value, param.Port ) );

                    if ( param.Port == 1 )
                        usePort2 = true;
                }
            }

            return ( usePort2, translatedParams );
        }

        private static APP_FIELD TranslateAclaraParam (
            Parameter param )
        {
            ParameterType typeAclara;

            string nameTypeAclara = param.Type.ToString ();

            // Translate aclara tag/parameter to app tag
            if ( ! Enum.TryParse<ParameterType> ( nameTypeAclara, out typeAclara ) ||
                 ! IdsAclara.ContainsKey ( typeAclara ) )
            {
                // Allow non-reserved tags
                //AddAdditionalParameter ( nameTypeAclara, parameter.Value, parameter.Port );
                
                //Errors.LogErrorNow ( new ProcessingParamsScriptException () );

                return APP_FIELD.NOTHING;
            }
            else
            {
                APP_FIELD typeOwn = IdsAclara[ typeAclara ];

                // If is for port two, find the correct enum element adding two ( "_2" ) as sufix
                if ( param.Port == 1 )
                    Enum.TryParse<APP_FIELD> ( typeOwn.ToString () + PT2_SUFIX, out typeOwn );

                return typeOwn;
            }
        }

        public static Dictionary<APP_FIELD,string> ValidateParams (
            bool port2Activated,
            Mtu mtu,
            MTUBasicInfo mtuBasicInfo,
            Action action,
            ( bool UsePort2, Dictionary<APP_FIELD,( dynamic Value, int Port )> Data ) translatedParams )
        {
            Configuration config         = Singleton.Get.Configuration;
            Global        global         = config.Global;
            ActionType    actionType     = action.Type;
            var           data           = translatedParams.Data;
            bool          scriptUsePort2 = translatedParams.UsePort2;

            List<Meter>  meters;
            List<string> portTypes;
            Meter meterPort1 = null;
            Meter meterPort2 = null;
            bool isAutodetectMeter = false;

            // These actions do not need to get the meter types
            bool isNotWrite = actionType == ActionType.ReadMtu ||
                              actionType == ActionType.TurnOffMtu ||
                              actionType == ActionType.TurnOnMtu ||
                              actionType == ActionType.MtuInstallationConfirmation ||
                              actionType == ActionType.DataRead;
            
            // Action is about Replace Meter
            bool isReplaceMeter = (
                action.Type == ActionType.ReplaceMeter ||
                action.Type == ActionType.ReplaceMtuReplaceMeter ||
                action.Type == ActionType.AddMtuReplaceMeter );

            // Action is about Replace MTU
            bool isReplaceMtu = (
                action.Type == ActionType.ReplaceMTU ||
                action.Type == ActionType.ReplaceMtuReplaceMeter );

            #region Get Meters

            // Only write actions ( Add and Replace ) need to detect Meters
            if ( isNotWrite )
                goto AFTER_GET_METERS;

            // Script is for one port but MTU has two and second is enabled
            if ( ! scriptUsePort2 &&
                 port2Activated   && // Return true in a one port 138 MTU
                 mtu.TwoPorts )      // and for that reason I have to check also this
                throw new ScriptForOnePortButTwoEnabledException ();
            
            // Script is for two ports but MTU has not second port or is disabled
            else if ( scriptUsePort2 &&
                      ! port2Activated )
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
                    isAutodetectMeter = true;
                
                    meters = config.meterTypes.FindByDialDescription (
                        int.Parse ( data[ APP_FIELD.NumberOfDials ].Value ),
                        int.Parse ( data[ APP_FIELD.DriveDialSize ].Value ),
                        data[ APP_FIELD.UnitOfMeasure ].Value,
                        mtu.Flow );
    
                    // At least one Meter was found
                    if ( meters.Count > 0 )
                        data.Add ( APP_FIELD.Meter, ( ( meterPort1 = meters[ 0 ] ).Id.ToString (), 0 ) );
                    
                    // No meter was found using the selected parameters
                    else throw new ScriptingAutoDetectMeterException ();
                }
                // Script does not contain some of the needed tags ( NumberOfDials,... )
                else throw new ScriptingAutoDetectTagsMissingScript ();
            }
            // Get the MTU with the selected MTU ID
            else
            {
                meterPort1 = config.getMeterTypeById ( int.Parse ( data[ APP_FIELD.Meter ].Value ) );
                Port port  = mtu.Port1;
                
                // Is not valid Meter ID ( not present in Meter.xml )
                if ( meterPort1.IsEmpty )
                    throw new ScriptingAutoDetectMeterMissing ();
                
                // Current MTU does not support selected Meter
                else if ( ! port.IsThisMeterSupported ( meterPort1 ) )
                    throw new ScriptingAutoDetectNotSupportedException ();
                
                // Set values for the Meter selected InterfaceTamper the script
                port.MeterProtocol   = meterPort1.EncoderType;
                port.MeterLiveDigits = meterPort1.LiveDigits;
            }

            #endregion

            #region Port 2

            // Auto-detect Meter using NumberOfDials, DriveDialSize and UnitOfMeasure
            if ( mtu.TwoPorts &&
                 port2Activated )
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
                        isAutodetectMeter = true;

                        meters = config.meterTypes.FindByDialDescription (
                            int.Parse ( data[ APP_FIELD.NumberOfDials_2 ].Value ),
                            int.Parse ( data[ APP_FIELD.DriveDialSize_2 ].Value ),
                            data[ APP_FIELD.UnitOfMeasure_2 ].Value,
                            mtu.Flow );
                        
                        // At least one Meter was found
                        if ( meters.Count > 0 )
                            data.Add ( APP_FIELD.Meter_2, ( ( meterPort2 = meters[ 0 ] ).Id.ToString (), 1 ) );
                        
                        // No meter was found using the selected parameters
                        else throw new ScriptingAutoDetectMeterException ( string.Empty, 2 );
                    }
                    // Script does not contain some of the needed tags ( NumberOfDials,... )
                    else throw new ScriptingAutoDetectTagsMissingScript ( string.Empty, 2 );
                }
                // Get the MTU with the selected MTU ID
                else
                {
                    meterPort2 = config.getMeterTypeById ( int.Parse ( data[ APP_FIELD.Meter_2 ].Value ) );
                    Port port  = mtu.Port2;
                    
                    // Is not valid Meter ID ( not present in Meter.xml )
                    if ( meterPort2.IsEmpty )
                        throw new ScriptingAutoDetectMeterMissing ( string.Empty, 2 );
                    
                    // Current MTU does not support selected Meter
                    else if ( ! port.IsThisMeterSupported ( meterPort2 ) )
                        throw new ScriptingAutoDetectNotSupportedException ( string.Empty, 2 );
                    
                    // Set values for the Meter selected InterfaceTamper the script
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
                                    string.IsNullOrEmpty ( v ) || ! Validations.IsNumeric ( v ) );

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
            
            string        valueStr       = string.Empty;
            string        msgDescription = string.Empty;
            StringBuilder msgError       = new StringBuilder ();
            StringBuilder msgErrorPopup  = new StringBuilder ();
            Dictionary<APP_FIELD,string> entriesSelected = new Dictionary<APP_FIELD,string> ();
            foreach ( var entry in data )
            {
                APP_FIELD type = entry.Key;
                if ( type == APP_FIELD.NOTHING )
                    continue;
                
                var  value = entry.Value.Value;
                int  port  = entry.Value.Port;
                bool fail  = false;
                
                if ( fail = Empty ( value ) )
                    msgDescription = MSG_EMPTY;
                else
                {
                    valueStr = value.ToString ();

                    // Validates each parameter before continue with the action
                    switch ( type )
                    {
                        #region Activity Log Id
                        case APP_FIELD.ActivityLogId:
                        if ( fail = EmptyNum ( valueStr ) )
                            msgDescription = MSG_NUMBER;
                        break;
                        #endregion
                        #region Account Number [ Only Writing ]
                        case APP_FIELD.AccountNumber:
                        case APP_FIELD.AccountNumber_2:
                        // Param totally useless in this action type
                        if ( isNotWrite )
                            continue;
                        
                        else if ( fail = EmptyNum ( valueStr ) )
                            msgDescription = MSG_NUMBER;

                        // NOTE: In scripted mode not taking into account global.AccountLength
                        //if ( fail = NoEqNum ( value, global.AccountLength ) )
                        //    msgDescription = "should be equal to global.AccountLength (" + global.AccountLength + ")";
                        break;
                        #endregion
                        #region Work Order
                        case APP_FIELD.WorkOrder:
                        case APP_FIELD.WorkOrder_2:
                        // Do not use
                        if ( ! global.WorkOrderRecording )
                            continue;

                        else if ( fail = NoELTxt ( valueStr, global.WorkOrderLength ) )
                            msgDescription =
                                String.Format ( MSG_ELTHAN, "global.WorkOrderLength", global.WorkOrderLength );
                        break;
                        #endregion
                        #region MTU Id Old [ Only Writing ]
                        case APP_FIELD.OldMtuId:
                        // Param totally useless in this action type
                        if ( ! isReplaceMtu )
                            continue;
                        
                        else if ( fail = NoEqNum ( valueStr, global.MtuIdLength ) )
                            msgDescription =
                                String.Format ( MSG_EQUAL, "global.MtuIdLength", global.MtuIdLength );
                        break;
                        #endregion
                        #region Meter Serial Number [ Only Writing ]
                        case APP_FIELD.MeterNumber:
                        case APP_FIELD.MeterNumber_2:
                        case APP_FIELD.MeterNumberOld:
                        case APP_FIELD.MeterNumberOld_2:
                        // Param totally useless in this action type
                        // Do not use
                        if ( isNotWrite ||
                             ! global.UseMeterSerialNumber )
                            continue;
                        
                        else if ( fail = NoELTxt ( valueStr, global.MeterNumberLength ) )
                            msgDescription =
                                String.Format ( MSG_ELTHAN, "global.MeterNumberLength", global.MeterNumberLength );
                        break;
                        #endregion
                        #region Meter Reading [ Only Writing ]
                        case APP_FIELD.MeterReading:
                        case APP_FIELD.MeterReading_2:
                        // Param totally useless in this action type
                        if ( isNotWrite )
                            continue;
                        
                        // Do not ask for new Meter reading if the port is for Encoders/Ecoders
                        else if ( port == 0 && meterPort1.IsForEncoderOrEcoder ||
                                  port == 1 && meterPort2.IsForEncoderOrEcoder )
                            continue;

                        else if ( ! isAutodetectMeter )
                        {
                            // If necessary fill left to 0's up to LiveDigits
                            if ( port == 0 )
                                 valueStr = meterPort1.FillLeftLiveDigits ( valueStr );
                            else valueStr = meterPort2.FillLeftLiveDigits ( valueStr );
                        }
                        else
                        {
                            if ( port == 0 )
                            {
                                if ( ! ( fail = meterPort1.NumberOfDials <= -1 || 
                                     NoELNum ( valueStr, meterPort1.NumberOfDials ) ) )
                                {
                                    // If value is lower than NumberOfDials, fill left to 0's up to NumberOfDials
                                    if ( NoEqNum ( valueStr, meterPort1.NumberOfDials ) )
                                        valueStr = meterPort1.FillLeftNumberOfDials ( valueStr );
                                    
                                    // Apply Meter mask
                                    valueStr = meterPort1.ApplyReadingMask ( valueStr );
                                }
                                else break;
                            }
                            else
                            {
                                if ( ! ( fail = meterPort2.NumberOfDials <= -1 ||
                                     NoELNum ( valueStr, meterPort2.NumberOfDials ) ) )
                                {
                                    // If value is lower than NumberOfDials, fill left to 0's up to NumberOfDials
                                    if ( NoEqNum ( valueStr, meterPort2.NumberOfDials ) )
                                        valueStr = meterPort2.FillLeftNumberOfDials ( valueStr );
                                    
                                    // Apply Meter mask
                                    valueStr = meterPort2.ApplyReadingMask ( valueStr );
                                }
                                else break;
                            }
                        }
                        
                        Meter meter = ( port == 0 ) ? meterPort1 : meterPort2;
                        fail = NoEqNum ( valueStr, meter.LiveDigits );
                        
                        if ( fail )
                        {
                            if ( ! isAutodetectMeter )
                                 msgDescription = String.Format ( MSG_ELTHAN, "Meter.LiveDigits",    meter.LiveDigits    );
                            else msgDescription = String.Format ( MSG_ELTHAN, "Meter.NumberOfDials", meter.NumberOfDials );
                        }
                        break;
                        #endregion
                        #region Meter Reading Old [ Only Writing ]
                        case APP_FIELD.MeterReadingOld:
                        case APP_FIELD.MeterReadingOld_2:
                        // Param totally useless in this action type
                        // Do not use
                        if ( ! isReplaceMeter ||
                             ! global.OldReadingRecording )
                            continue;
                        
                        else if ( fail = NoELNum ( valueStr, 12 ) )
                            msgDescription = String.Format ( MSG_ELONLY, "12" );
                        break;
                        #endregion
                        #region Meter Type [ Only Writing ]
                        case APP_FIELD.Meter:
                        case APP_FIELD.Meter_2:
                        // Param totally useless in this action type
                        if ( isNotWrite )
                            continue;
                        break;
                        #endregion
                        #region Read Interval [ Only Writing ]
                        case APP_FIELD.ReadInterval:
                        // Param totally useless in this action type
                        // Do not use
                        if ( isNotWrite ||
                             ! global.IndividualReadInterval )
                            continue;

                        List<string> readIntervalList;
                        if ( mtuBasicInfo.version >= global.LatestVersion )
                        {
                            readIntervalList = new List<string>()
                            {
                                "24" + HOURS,
                                "12" + HOURS,
                                 "8" + HOURS,
                                 "6" + HOURS,
                                 "4" + HOURS,
                                 "3" + HOURS,
                                 "2" + HOURS,
                                 "1" + HOURS,
                                "30" + MIN,
                                "20" + MIN,
                                "15" + MIN
                            };
                        }
                        else
                        {
                            readIntervalList = new List<string>()
                            {
                                 "1" + HOUR,
                                "30" + MIN,
                                "20" + MIN,
                                "15" + MIN
                            };
                        }
                        
                        // TwoWay MTU reading interval cannot be less than 15 minutes
                        if ( ! mtu.TimeToSync )
                            readIntervalList.AddRange ( new string[]{
                                "10" + MIN,
                                 "5" + MIN
                            });
                        
                        valueStr = valueStr.ToLower ()
                                        .Replace ( "hr", "hour" )
                                        .Replace ( "h", "H" )
                                        .Replace ( "m", "M" );
                        
                        if ( fail = Empty ( valueStr ) ||
                             ! readIntervalList.Contains ( valueStr ) )
                            msgDescription = MSG_HSMINS;
                        break;
                        #endregion
                        #region Snap Reads [ Only Writing ]
                        case APP_FIELD.SnapReads:
                        // Param totally useless in this action type
                        // Do not use
                        if ( isNotWrite ||
                             ! global.AllowDailyReads ||
                             ! mtu.DailyReads ||
                             mtu.IsFamilly33xx )
                            continue;

                        if ( fail = EmptyNum ( valueStr ) )
                            msgDescription = MSG_NUMBER;
                        break;
                        #endregion
                        #region Auto-detect Meter [ Only Writing ]
                        case APP_FIELD.NumberOfDials:
                        case APP_FIELD.NumberOfDials_2:
                        case APP_FIELD.DriveDialSize:
                        case APP_FIELD.DriveDialSize_2:
                        // Param totally useless in this action type
                        if ( isNotWrite )
                            continue;

                        if ( fail = EmptyNum ( valueStr ) )
                            msgDescription = MSG_NUMBER;
                        break;
                        
                        case APP_FIELD.UnitOfMeasure:
                        case APP_FIELD.UnitOfMeasure_2:
                        // Param totally useless in this action type
                        if ( isNotWrite )
                            continue;
                        break;
                        #endregion
                        #region Force Time Sync [ Only Writing ]
                        case APP_FIELD.ForceTimeSync:
                        // Param totally useless in this action type
                        if ( isNotWrite )
                            continue;

                        bool.TryParse ( valueStr, out fail );
                        if ( fail = ! fail )
                            msgDescription = MSG_BOOL;
                        break;
                        #endregion
                        #region Number of Days [ Only DataRead ]
                        case APP_FIELD.NumOfDays:
                        // Param totally useless in other action types
                        if ( actionType != ActionType.DataRead )
                            continue;

                        switch ( valueStr )
                        {
                            case "1":
                            case "8":
                            case "32":
                            case "64":
                            case "96":
                            // Ok
                            break;
                            default:
                            fail = true;
                            msgDescription = MSG_DAYS;
                            break;
                        }
                        break;
                        #endregion
                    }
                }

                // Error validating a parameter
                if ( fail )
                {
                    fail = false;
                    
                    msgErrorPopup.Append ( ( ( msgError.Length > 0 ) ? ", " : string.Empty ) +
                                           type + " " + msgDescription );

                    msgError.Append ( ( ( msgError.Length > 0 ) ? ", " : string.Empty ) + type );
                }
                // Parameter validated and selected
                else
                    entriesSelected.Add ( type, valueStr );
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

                throw new ProcessingParamsScriptException ( msgErrorStr, 1, msgErrorPopupStr );
            }

            #endregion

            return entriesSelected;
        }
    
        #endregion
    }
}
