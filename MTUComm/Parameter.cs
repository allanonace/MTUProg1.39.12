using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUComm
{
    /// <summary>
    /// Data structure used in multiple places of the application, which allows
    /// to store data in a format similar to that used in the activity logs.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Parameters supported by the applicaton in scripted mode.
        /// <para>&#160;</para>
        /// </para>
        /// <list type="ParameterType">
        /// <item>
        ///     <term>ParameterType.ActivityLogId</term>
        ///     <description>It is just to add in the activity log</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.MeterType</term>
        ///     <description>Meter Type or Meter ID is used to select a specific Meter</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.UnitOfMeasure</term>
        ///     <description>It is used to automatically detect a compatible Meter</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.DriveDialSize</term>
        ///     <description>It is used to automatically detect a compatible Meter</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.NumberOfDials</term>
        ///     <description>It is used to automatically detect a compatible Meter</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.AccountNumber</term>
        ///     <description>Account Number or Service Port ID</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.ReadInterval</term>
        ///     <description>It is ...</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.ForceTimeSync</term>
        ///     <description>It is used to force the execution of the InstallConfirmation process or RFCheck</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.WorkOrder</term>
        ///     <description>Work Order or Field Order is just to add in the activity log</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.MeterSerialNumber</term>
        ///     <description>Meter Serial Number or Meter ID is just to add in the activity log</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.OldMeterSerialNumber</term>
        ///     <description>Is just to add in the activity log while replacing an old Meter with a new one</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.NewMeterSerialNumber</term>
        ///     <description>It is equal to ParameterType.MeterSerialNumber</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.MeterReading</term>
        ///     <description>Meter Reading is used working with Pulse MTUs, overriding port previous read value</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.OldMeterReading</term>
        ///     <description>It is just to add in the activity log while replacing an old Meter with a new one</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.NewMeterReading</term>
        ///     <description>It is equal to ParameterType.MeterReading</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.SnapRead</term>
        ///     <description>It is ...</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.OldMtuId</term>
        ///     <description>Is just to add in the activity log while replacing an old MTU with a new one</description>
        /// </item>
        /// <item>
        ///     <term>ParameterType.DaysOfRead</term>
        ///     <description>It is used to select the specific number of days for the DataRead process</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        public enum ParameterType
        {
            ActivityLogId,
            MeterType,
            UnitOfMeasure,
            AccountNumber,
            ReadInterval,
            Fast2Way, // Fast-2-Way
            ForceTimeSync,
            WorkOrder,
            Alarm,
            MeterSerialNumber,
            NumberOfDials,
            MeterReading,
            DriveDialSize,
            SnapRead,
            OldMtuId,
            OldMeterSerialNumber,
            NewMeterSerialNumber,
            OldMeterReading,
            NewMeterReading,
            OldMeterWorking,
            ReplaceMeterRegister,
            DaysOfRead,
            RDDPosition,
            RDDFirmwareVersion,
            Custom,
            TwoWay
            //Port2Disabled
            //ProvidingHandFactor
            //LiveDigits
            //TempReadDays
            //TempReadInterval
        };

        private class ParameterDefine {

            public Boolean memory_present;
            public Boolean log_generation;
            public String action_tag;
            public String action_display;

            public ParameterDefine(Boolean present, Boolean generates_log, String tag, String display)
            {
                memory_present = present;
                log_generation = generates_log;
                action_tag = tag;
                action_display = display;
            }
        };

        private Dictionary<ParameterType, ParameterDefine> paremeter_defines = new Dictionary<ParameterType, ParameterDefine>()
        {
            {ParameterType.ActivityLogId, new ParameterDefine(true, true, "ActivityLogId", "") },
            {ParameterType.MeterType, new ParameterDefine(true, true, "Port{0}MeterType", "Meter Type")},
            {ParameterType.UnitOfMeasure, new ParameterDefine(true, true, "UnitOfMeasure", "")},
            //{ParameterType.Port2Disabled, new ParameterDefine(true, true, "Port2Disabled", "")},
            {ParameterType.AccountNumber, new ParameterDefine(true, true, "Port{0}AccountNumber", "Service Pt. ID")},
            //{ParameterType.ProvidingHandFactor, new ParameterDefine(true, true, "ProvidingHandFactor", "")},
            {ParameterType.ReadInterval, new ParameterDefine(true, true, "ReadInterval", "Read Interval")},
            {ParameterType.ForceTimeSync, new ParameterDefine(true, true, "ForceTimeSync", "")},
            {ParameterType.WorkOrder, new ParameterDefine(true, true, "WorkOrder", "Field Order")},
            //{ParameterType.LiveDigits, new ParameterDefine(true, true, "LiveDigits", "")},
            //{ParameterType.TempReadInterval, new ParameterDefine(true, true, "TempReadInterval", "")},
            {ParameterType.Alarm, new ParameterDefine(true, true, "Alarm", "")},
            {ParameterType.MeterSerialNumber, new ParameterDefine(true, true, "MeterSerialNumber", "Meter Number")},
            {ParameterType.NumberOfDials, new ParameterDefine(true, true, "NumberOfDials", "")},
            //{ParameterType.TempReadDays, new ParameterDefine(true, true, "TempReadDays", "")},
            {ParameterType.MeterReading, new ParameterDefine(true, true, "MeterReading", "Meter Reading")},
            {ParameterType.DriveDialSize, new ParameterDefine(true, true, "DriveDialSize", "")},
            {ParameterType.SnapRead, new ParameterDefine(true, true, "SnapRead", "")},
            {ParameterType.OldMtuId, new ParameterDefine(true, true, "OldMtuId", "Old MTU ID")},
            {ParameterType.OldMeterSerialNumber, new ParameterDefine(true, true, "Port{0}OldMeterSerialNumber", "Old Meter Serial Number")},
            {ParameterType.NewMeterSerialNumber, new ParameterDefine(true, true, "Port{0}NewMeterSerialNumber", "New Meter Serial Number")},
            {ParameterType.OldMeterReading, new ParameterDefine(true, true, "Port{0}OldMeterReading", "Old Meter Reading")},
            {ParameterType.NewMeterReading, new ParameterDefine(true, true, "Port{0}NewMeterReading", "")},
            //{ParameterType.DaysOfRead, new ParameterDefine(true, true, "DaysOfRead", "DaysOfRead")},
            {ParameterType.Custom, new ParameterDefine(true, true, "{1}", "{1}")},
            {ParameterType.TwoWay, new ParameterDefine(true, true, "TwoWay", "Two Way")}
        };

        private Boolean has_port = false;
        private int port = 0;

        private String mCustomParameter;
        private String mCustomDisplay = null;

        private ParameterType mParameterType;

        private dynamic mValue;

        private bool optional = false;

        public Parameter ()
        {
            mValue = null;
        }

        public Parameter(ParameterType type, String value)
        {
            mParameterType = type;
            mValue = value;
        }

        // From scrimpting, with port in base 1
        public Parameter ( ParameterType type, String value, int port )
        {
            mParameterType = type;
            mValue = value;
            
            if ( port == 2 )
                 setPort ( 1 );
            else setPort ( 0 );
        }

        // Port in base zero
        public Parameter(String custom_parameter, String custom_display, dynamic value, string source = "", int port = 0, bool optional = false )
        {
            mParameterType   = ParameterType.Custom;
            mCustomParameter = custom_parameter;
            mCustomDisplay   = custom_display;
            this.Source      = source;
            mValue           = value;
            this.optional    = optional;
            
            this.setPort ( port );
        }

        public ParameterType Type
        {
            get
            {
                return mParameterType;
            }
        }

        /// <summary>
        /// The zero base index of MTU port associated to the parameter, that by default is zero/0.
        /// </summary>
        /// <remarks>
        /// NOTE: Remember that base zero is zero/0 for first port and one/1 for second port.
        /// </remarks>
        public int Port
        {
            get
            {
                if (hasPort())
                {
                    return port;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Boolean isInMemoryMap()
        {
            return paremeter_defines[mParameterType].memory_present;
        }

        /// <summary>
        /// Modifies the port associated to the parameter.
        /// </summary>
        /// <remarks>
        /// NOTE: Remember that base zero is zero/0 for first port and one/1 for second port. 
        /// </remarks>
        /// <param name="port">Port index</param>
        public void setPort(int port)
        {
            this.port = port;
            has_port = true;
        }

        /// <summary>
        /// Indicates if the parameter has assigned a specific port.
        /// <para>
        /// If not, is because the parameter is not for a port but at MTU level.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public Boolean hasPort()
        {
            return has_port;
        }

        public String getLogTag()
        {
            return String.Format(paremeter_defines[mParameterType].action_tag, port, mCustomParameter);
        }

        public String getLogDisplay()
        {
            String display = String.Format(paremeter_defines[mParameterType].action_display, port,  mCustomDisplay);
            if(display.Length > 0)
            {
                return display;
            }

            return null;
        }

        public Boolean doesGenerateLog()
        {
            return paremeter_defines[mParameterType].log_generation;
        }

        /// <summary>
        /// Indicates if the parameter is an optional or additional parameter,
        /// just to add in the activity log.
        /// </summary>
        public bool Optional
        {
            set
            {
                optional = value;
            }
            get
            {
                return this.optional;
            }
        }

        /// <summary>
        /// Returns the string used as 'name' in the logs.
        /// </summary>
        public string CustomParameter
        {
            get
            {
                return mCustomParameter;
            }
            set
            {
                mCustomParameter = value;
            }
        }

        /// <summary>
        /// Returns the string used as 'display' in the logs.
        /// </summary>
        public string CustomDisplay
        {
            get
            {
                return mCustomDisplay;
            }
        }

        /// <summary>
        /// Value of the parameter.
        /// </summary>
        public dynamic Value
        {
            get
            {
                //if ( this.mParameterType == ParameterType.MeterReading ||
                //     this.mCustomParameter.Equals ( "MeterReading" ) )


                return mValue;
            }
            set
            {
                mValue = value;
            }
        }

        public string Source { get; set; }

        /// <summary>
        /// Returns current value of the parameter, filling in up to number of characters specified,
        /// or a string filled with zeros or spaces with the specific length if the parameter does not have value.
        /// <para>
        /// If T is an string the character used to fill in the
        /// string will be space ( ' ' ), in other case will be zero/0.
        /// </para>
        /// </summary>
        /// <remarks>
        /// TODO: Divide this method in two, one for numeric values and other for strings.
        /// </remarks>
        /// <param name="numCharacters">Number of characters desired for the output string</param>
        /// <typeparam name="T">Type of the default</typeparam>
        /// <returns>Current value or default string</returns>
        public string GetValueOrDefault<T> (
            int numCharacters )
        {
            char   val   = ( typeof ( T ) == typeof ( string ) ) ? ' ' : '0';
            string value = this.Value.ToString ();
        
            if ( string.IsNullOrEmpty ( value ) )
                value = "".PadRight ( numCharacters, val );
            else
            {
                if ( value.Length > numCharacters )
                     value = value.Substring ( 0, numCharacters );                      // Get only n first characters
                else value = "".PadRight ( numCharacters - value.Length, val ) + value; // Fill in left side to default character value
            }

            return value;
        }

        public override string ToString()
        {
            return CustomParameter + ": " + Value;
        }
    }

    public static class ParameterListExtension
    {
        private const int PARAMETER_INDEX = 1;

        public static Parameter FindByParamId(
            this List<Parameter> paramList, dynamic paramId, dynamic texts = null)
        {
            if ( texts == null )
                return paramList.Find(x => string.Equals(x.CustomParameter, paramId));
            
            return paramList.Find(x => string.Equals(x.CustomParameter, texts[ paramId ][ PARAMETER_INDEX ]));
        }
    }
}
