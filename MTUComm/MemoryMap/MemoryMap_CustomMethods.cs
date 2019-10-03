using System;
using System.Globalization;
using System.Threading.Tasks;
using Library;
using Xml;

using RDDStatus        = MTUComm.RDDStatusResult.RDDStatus;
using RDDValveStatus   = MTUComm.RDDStatusResult.RDDValveStatus;
using RDDBatteryStatus = MTUComm.RDDStatusResult.RDDBatteryStatus;
using RDDCmd           = MTUComm.RDDStatusResult.RDDCmd;

namespace MTUComm.MemoryMap
{
    public partial class MemoryMap : AMemoryMap
    {
        #region Constants

        private const string MIDNIGHT    = "MidNight";
        private const string NOON        = "Noon";
        private const string AM          = " AM";
        private const string PM          = " PM";
        private const string OFF         = "OFF";

        private const string STATE_ON    = "ON";
        private const string STATE_OFF   = "OFF";

        private const string YES         = "Yes";
        private const string NO          = "No";

        private const string PCBFORMAT   = "{0:000000000}";
        private const string NTAVAILABLE = "Not Available";

        private const string MTU_SOFTVERSION_LONG = "Version {0:00}.{1:00}.{2:0000}";
        private const string MTU_SOFTVERSION_SMALL = "Version {0:00}";

        private const string MTUVLFORMAT = "0.00 V";

        private const string FWAYFORMAT  = "X8";

        private const string MESAG_FAST  = "Fast";
        private const string MESAG_SLOW  = "Slow";
        private const int    ON_INT      = 1;
        private const int    OFF_INT     = 0;

        private const string HOURS       = " Hrs";
        private const string HOUR        = " Hr";
        private const string MIN         = " Min";

        private const char   ZERO        = '0';
        private const int    INDEX_STATE = 2;
        private const int    PAD_LEFT    = 8;

        public  const string ENABLED     = "Enabled";
        public  const string DISABLED    = "Disabled";
        public  const string TRIGGERED   = "Triggered";

        private const string NTCONFIRMED = "NOT CONFIRMED";
        private const string CONFIRMED   = "CONFIRMED";

        private const int    ECODE_METER = 0xFF;
        private const int    ECODE_DIGIT = 0xFE;
        private const int    ECODE_OVER  = 0xFD;
        private const int    ECODE_PURGE = 0xFC;
        private const string ERROR_METER = "ERROR - Check Meter";
        private const string ERROR_DIGIT = "ERROR - Bad Digits";
        private const string ERROR_OVER  = "ERROR - Delta Overflow";
        private const string ERROR_PURGE = "ERROR - Readings Purged";

        private const string CASE_00     = "00";
        private const string CASE_01     = "01";
        private const string CASE_10     = "10";

        private const string CASE_00_BFS = "No reverse Flow Event in last 35 days";
        private const string CASE_01_BFS = "Small Reverse Flow Event in last 35 days";
        private const string CASE_10_BFS = "Large Reverse Flow Event in last 35 days";

        private const string CASE_00_LKD = "Less than 50 15-minute intervals";
        private const string CASE_01_LKD = "Between 50 and 95 15-minute intervals";
        private const string CASE_10_LKD = "Greater than 96 15-minute intervals";

        private const string CASE_000    = "000";
        private const string CASE_001    = "001";
        private const string CASE_010    = "010";
        private const string CASE_011    = "011";
        private const string CASE_100    = "100";
        private const string CASE_101    = "101";
        private const string CASE_110    = "110";

        private const string CASE_000_TX = "0";
        private const string CASE_001_TX = "1-2";
        private const string CASE_010_TX = "3-7";
        private const string CASE_011_TX = "8-14";
        private const string CASE_100_TX = "15-21";
        private const string CASE_101_TX = "22-34";
        private const string CASE_110_TX = "35 (ALL)";
        private const string CASE_NOFLOW = " days of no consumption";
        private const string CASE_LEAK   = " days of leak detection";

        #endregion

        #region Overloads

        public async Task<string> RSSIStatus_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return string.Format ( "{0} dBm", await MemoryRegisters.RSSI.GetValue () ).Replace ( "-", "- " );
        }

        public async Task<string> MtuDatetime_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return string.Format ( "{0}/{1}/20{2} {3}:{4}:{5}",
                await MemoryRegisters.MtuDatetime_Month.GetValue (),
                await MemoryRegisters.MtuDatetime_Day.GetValue (),
                await MemoryRegisters.MtuDatetime_Year.GetValue (),
                await MemoryRegisters.MtuDatetime_Hour.GetValue (),
                await MemoryRegisters.MtuDatetime_Minutes.GetValue (),
                await MemoryRegisters.MtuDatetime_Seconds.GetValue () );
        }

        public async Task<string> DailySnap_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            Global global = Singleton.Get.Configuration.Global;
            Mtu    mtu    = Singleton.Get.Action.CurrentMtu;
        
            if ( ! global.AllowDailyReads ||
                 ! mtu.DailyReads )
                return DISABLED;
        
            int timeDiff = TimeZone.CurrentTimeZone.GetUtcOffset ( DateTime.Now ).Hours;
            int curTime = await MemoryRegisters.DailyGMTHourRead.GetValue () + timeDiff;

            if ( curTime < 0 )
                curTime = 24 + curTime;

            if      ( curTime ==  0 ) return MIDNIGHT;
            else if ( curTime <= 11 ) return curTime + AM;
            else if ( curTime == 12 ) return NOON;
            else if ( curTime >  12 &&
                      curTime <  24 ) return ( curTime - 12 ) + PM;
            else return DISABLED;
        }

        public async Task<string> MtuStatus_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return await MemoryRegisters.Shipbit.GetValue () ? STATE_OFF : STATE_ON;
        }

        public async Task<string> ReadInterval_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return TimeFormatter ( await MemoryRegisters.ReadIntervalMinutes.GetValue () );
        }

        public async Task<string> XmitInterval_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return TimeFormatter ( await MemoryRegisters.ReadIntervalMinutes.GetValue () *
                                   ( 12 - await MemoryRegisters.MessageOverlapCount.GetValue () ) );
        }

        public async Task<string> PCBNumber_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            string tempString = string.Empty;

            //ASCII RANGE FOR PCBSupplierCode
            if ( await MemoryRegisters.PCBSupplierCode.GetValue () >= 65 &&
                 await MemoryRegisters.PCBSupplierCode.GetValue () <= 90 )
                tempString = tempString + Convert.ToChar ( await MemoryRegisters.PCBSupplierCode.GetValue () ) + "-";

            if ( await MemoryRegisters.PCBCoreNumber.GetValue () >= 0 )
                tempString = tempString + string.Format ( PCBFORMAT, await MemoryRegisters.PCBCoreNumber.GetValue () );

            if ( await MemoryRegisters.PCBProductRevision.GetValue () >= 65 &&
                 await MemoryRegisters.PCBProductRevision.GetValue () <= 90 )
                tempString = tempString + "-" + Convert.ToChar ( await MemoryRegisters.PCBProductRevision.GetValue () );

            string result = ( string.IsNullOrEmpty ( tempString ) ) ? NTAVAILABLE : tempString;

            return result;
        }

        public async Task<string> Encryption_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return ( await MemoryRegisters.Encrypted.GetValue () ) ? YES : NO;
        }

        public async Task<string> MtuVoltageBattery_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return ( ( ( float )await MemoryRegisters.MtuMiliVoltageBattery.GetValue () ) / 1000 )
                .ToString ( MTUVLFORMAT ).Replace ( ",", "." );
        }

        public async Task<string> P1ReadingError_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return TranslateErrorCodes ( await MemoryRegisters.P1ReadingErrorCode.GetValue () );
        }

        public async Task<string> P2ReadingError_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return TranslateErrorCodes ( await MemoryRegisters.P2ReadingErrorCode.GetValue () );
        }

        public async Task<string> TamperStatus_Get ( MemoryOverload<string> MemoryOverload, dynamic[] MemoryRegisters )
        {
            return GetTamperStatus ( await MemoryRegisters[ 0 ].GetValue (),   // Alarm
                                     await MemoryRegisters[ 1 ].GetValue () ); // Tamper
        }

        public async Task<string> TamperImmStatus_Get ( MemoryOverload<string> MemoryOverload, dynamic[] MemoryRegisters )
        {
            return GetTamperStatus ( await MemoryRegisters[ 0 ].GetValue () ); // Alarm
        }

        public async Task<string> LastGasp_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return ( await MemoryRegisters.LastGaspTamper.GetValue () ) ? ENABLED : TRIGGERED;
        }

        public async Task<string> InsufficientMemory_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return ( await MemoryRegisters.InsufficientMemoryTamper.GetValue () ) ? ENABLED : TRIGGERED;
        }

        public async Task<string> P1Status_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return GetPortStatus ( await MemoryRegisters.P1StatusFlag.GetValue () );
        }

        public async Task<string> P2Status_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return GetPortStatus ( await MemoryRegisters.P2StatusFlag.GetValue () );
        }

        public async Task<string> F12WAYRegister1_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return HEX_PREFIX + ( await MemoryRegisters.F12WAYRegister1Int.GetValue () ).ToString ( FWAYFORMAT );
        }

        public async Task<string> F12WAYRegister10_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return HEX_PREFIX + ( await MemoryRegisters.F12WAYRegister10Int.GetValue () ).ToString ( FWAYFORMAT );
        }

        public async Task<string> F12WAYRegister14_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return HEX_PREFIX + ( await MemoryRegisters.F12WAYRegister14Int.GetValue () ).ToString ( FWAYFORMAT );
        }

        public async Task<string> Frequency1Way_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return String.Format ( new CultureInfo("en-us"), "{0:0.000}",
                ( await MemoryRegisters.Frequency1WayHz.GetValue () / 1000000.0 ) );
        }

        public async Task<string> Frequency2WayTx_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return String.Format ( new CultureInfo("en-us"), "{0:0.000}",
                ( await MemoryRegisters.Frequency2WayTxHz.GetValue () / 1000000.0 ) );
        }

        public async Task<string> Frequency2WayRx_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return String.Format ( new CultureInfo("en-us"), "{0:0.000}",
                ( await MemoryRegisters.Frequency2WayRxHz.GetValue () / 1000000.0 ) );
        }

        public async Task<string> InstallConfirmationStatus_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return ( await MemoryRegisters.InstallConfirmationNotSynced.GetValue () ) ? NTCONFIRMED : CONFIRMED;
        }

        public async Task<int> MtuSoftVersion_Get ( MemoryOverload<int> memoryOverload, dynamic MemoryRegisters )
        {
            int newVersion;
            if ( ( newVersion = await MemoryRegisters.MtuSoftVersionNew.GetValue () ) == 255 )
                return await MemoryRegisters.MtuSoftVersionLegacy.GetValue ();
            return newVersion;
        }

        public async Task<string> MtuSoftVersionString_Get ( MemoryOverload<string> memoryOverload, dynamic MemoryRegisters )
        {
            int mtuSoftVersion = await MemoryRegisters.MtuSoftVersion.GetValue ();
        
            // NOTE: MtuSoftVersion in Encoders/E-coders seems to be always 254 and
            // the bytes for registers used creating MTU_SOFTVERSION_LONG are the same,
            // changing only the name ( e.g. MtuSoftRevYear and MtuSoftVersionMajor both 245 )
            // For example mtuSoftVersion for MTUs..
            // For 138, 149, 151 is 41
            // For 146, 148 is 50
            // For 159 is 47
            if ( mtuSoftVersion == 254 )
                return string.Format ( MTU_SOFTVERSION_LONG,
                    await MemoryRegisters.MtuSoftRevYear    .GetValue (),
                    await MemoryRegisters.MtuSoftRevMonth   .GetValue (),
                    await MemoryRegisters.MtuSoftBuildNumber.GetValue () );
            
            return string.Format ( MTU_SOFTVERSION_SMALL, mtuSoftVersion );
        }

        // TODO: This method ( overload ) can be removed and get directly the register value
        public async Task<int> MtuSoftVersion342x_Get ( MemoryOverload<int> memoryOverload, dynamic MemoryRegisters )
        {
            return await MemoryRegisters.MtuSoftFormatFlag.GetValue ();
        }

        public async Task<string> MtuSoftVersionString342x_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return string.Format ( MTU_SOFTVERSION_LONG, 
                await MemoryRegisters.MtuSoftVersionMajor.GetValue (),
                await MemoryRegisters.MtuSoftVersionMinor.GetValue (),
                await MemoryRegisters.MtuSoftBuildNumber .GetValue () );
        }

        public async Task<int> FastMessagingMode_Get ( MemoryOverload<int> MemoryOverload, dynamic MemoryRegisters )
        {
            return ( await MemoryRegisters.FastMessagingConfigMode.GetValue () ) ? ON_INT : OFF_INT;
        }

        public async Task<string> FastMessagingFrequency_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return ( await MemoryRegisters.FastMessagingConfigFreq.GetValue () ) ? MESAG_FAST : MESAG_SLOW;
        }

        public async Task<int> MinutesToHours_Get ( MemoryOverload<int> memoryOverload, dynamic[] memoryRegisters )
        {
            // NOTE: 1440 minutes / 60 minutes one hour = 24 hours
            return ( int ) ( await memoryRegisters[ 0 ].GetValue () / 1440 ); // Days
        }

        public async Task<string> RDDStatus_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            int val = await MemoryRegisters.RDDStatusInt.GetValue ();
            return Utils.ParseIntToEnum<RDDStatus> ( val, RDDStatus.DISABLED ).ToString ();
        }

        public async Task<string> RDDValvePosition_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            int val = await MemoryRegisters.RDDValvePositionInt.GetValue ();
            return Utils.ParseIntToEnum<RDDValveStatus> ( val, RDDValveStatus.UNKNOWN ).ToString ();
        }

        public async Task<string> RDDBatteryStatus_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            int val = await MemoryRegisters.RDDBatteryStatusInt.GetValue ();
            return Utils.ParseIntToEnum<RDDBatteryStatus> ( val, RDDBatteryStatus.UNKNOWN ).ToString ();
        }

        public async Task<string> RDDPreviousCmd_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            int val = await MemoryRegisters.RDDPreviousCmdInt.GetValue ();
            return Utils.ParseIntToEnum<RDDCmd> ( val, RDDCmd.UNKNOWN ).ToString ();
        }

        public async Task<string> RDDLastCmdSuccess_Get ( MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters )
        {
            return ( await MemoryRegisters.RDDLastCmdSuccessInt.GetValue () == 1 ) ? YES : NO;
        }

        #endregion

        #region Registers

        public async Task<int> ReadIntervalMinutes_Set ( MemoryRegister<int> MemoryRegister, dynamic inputValue )
        {
            string[] readIntervalArray = ((string)inputValue).Split(' ');
            string readIntervalStr = readIntervalArray[0].ToLower ();
            string timeUnit = readIntervalArray[1].ToLower ();
            int timeIntervalMins = Int32.Parse(readIntervalStr);

            if ( timeUnit.StartsWith ( "hour" ) ||
                 timeUnit.StartsWith ( "hr"   ) )
                timeIntervalMins = timeIntervalMins * 60;

            return timeIntervalMins;
        }

        // Use with <CustomGet>method:ULongToBcd</CustomGet>
        public async Task<ulong> BcdToULong ( MemoryRegister<ulong> MemoryRegister )
        {
            byte[] bytes  = await MemoryRegister.GetValueByteArray ();
            string outNum = string.Empty;
            
            foreach ( byte b in bytes )
                outNum += b.ToString ( "X" );
            outNum = outNum.TrimEnd ( new char[] { 'F' } );

            outNum = outNum
                .Replace ( "A", "10" )
                .Replace ( "B", "11" )
                .Replace ( "C", "12" )
                .Replace ( "D", "13" )
                .Replace ( "E", "14" )
                .Replace ( "F", "15" );

            return ulong.Parse ( outNum );
        }

        // Use with <CustomSet>method:ULongToBcd</CustomSet>
        public async Task<byte[]> ULongToBcd ( MemoryRegister<ulong> MemoryRegister, dynamic inputValue )
        {
            return this.ULongToBcd_Logic ( inputValue.ToString (), MemoryRegister.size );
        }

        // Convert hexadecimal number to integer value
        // Use with <CustomSet>method:HexToInt</CustomSet>
        public async Task<int> HexToInt ( MemoryRegister<int> MemoryRegister, dynamic inputValue )
        {
            if ( inputValue is string ) // Removes 0x prefix
                 return int.Parse ( inputValue.Substring ( 2 ), NumberStyles.HexNumber );
            else return ( int ) inputValue;
        }

        #endregion

        #region e-Coder

        public async Task<string> BackFlowState_Get ( MemoryOverload<string> MemoryOverload, dynamic[] MemoryRegisters )
        {
            string reply = string.Empty;
            string param = Convert.ToString ( await MemoryRegisters[ 0 ].GetValue (), INDEX_STATE )
                .PadLeft(PAD_LEFT,ZERO)
                .Substring(6);
            switch (param)
            {
                case CASE_00: reply = CASE_00_BFS; break;
                case CASE_01: reply = CASE_01_BFS; break;
                case CASE_10: reply = CASE_10_BFS; break;
            }
            return reply;
        }

        public async Task<string> DaysOfNoFlow_Get ( MemoryOverload<string> MemoryOverload, dynamic[] MemoryRegisters )
        {
            string reply = string.Empty;
            string param = Convert.ToString ( await MemoryRegisters[ 0 ].GetValue (), INDEX_STATE )
                .PadLeft(PAD_LEFT,ZERO)
                .Substring(3,3);
            switch (param)
            {
                case CASE_000: reply = CASE_000_TX; break;
                case CASE_001: reply = CASE_001_TX; break;
                case CASE_010: reply = CASE_010_TX; break;
                case CASE_011: reply = CASE_011_TX; break;
                case CASE_100: reply = CASE_100_TX; break;
                case CASE_101: reply = CASE_101_TX; break;
                case CASE_110: reply = CASE_110_TX; break;
            }
            return reply + CASE_NOFLOW;
        }

        public async Task<string> LeakDetection_Get ( MemoryOverload<string> MemoryOverload, dynamic[] MemoryRegisters )
        {
            string reply = string.Empty;
            string param = Convert.ToString ( await MemoryRegisters[ 0 ].GetValue (), INDEX_STATE )
                .PadLeft(PAD_LEFT,ZERO)
                .Substring(5,2);
            switch (param)
            {
                case CASE_00: reply = CASE_00_LKD; break;
                case CASE_01: reply = CASE_01_LKD; break;
                case CASE_10: reply = CASE_10_LKD; break;
            }
            return reply;
        }

        public async Task<string> DaysOfLeak_Get ( MemoryOverload<string> MemoryOverload, dynamic[] MemoryRegisters )
        {
            string reply = string.Empty;
            string param = Convert.ToString ( await MemoryRegisters[ 0 ].GetValue (), INDEX_STATE )
                .PadLeft(PAD_LEFT,ZERO)
                .Substring(2, 3);
            switch (param)
            {
                case CASE_000: reply = CASE_000_TX; break;
                case CASE_001: reply = CASE_001_TX; break;
                case CASE_010: reply = CASE_010_TX; break;
                case CASE_011: reply = CASE_011_TX; break;
                case CASE_100: reply = CASE_100_TX; break;
                case CASE_101: reply = CASE_101_TX; break;
                case CASE_110: reply = CASE_110_TX; break;
            }
            return reply + CASE_LEAK;
        }

        #endregion

        #region AuxiliaryFunctions

        private string TranslateErrorCodes (int encoderErrorcode)
        {
            if (encoderErrorcode == ECODE_METER) return ERROR_METER;
            if (encoderErrorcode == ECODE_DIGIT) return ERROR_DIGIT;
            if (encoderErrorcode == ECODE_OVER ) return ERROR_OVER;
            if (encoderErrorcode == ECODE_PURGE) return ERROR_PURGE;
            return string.Empty;
        }

        private string TimeFormatter ( int time )
        {
            switch ( time )
            {
                case 2880: return 48 + HOURS;
                case 2160: return 36 + HOURS;
                case 1440: return 24 + HOURS;
                case 720 : return 12 + HOURS;
                case 480 : return 8  + HOURS;
                case 360 : return 6  + HOURS;
                case 240 : return 4  + HOURS;
                case 180 : return 3  + HOURS;
                case 120 : return 2  + HOURS;
                case 90  : return 1  + HOUR + " " + 30 + MIN;
                case 60  : return 1  + HOUR;
                case 30  : return 30 + MIN;
                case 15  : return 15 + MIN;
                case 10  : return 10 + MIN;
                case 5   : return 5  + MIN;
                default: // KG 3.10.2010 add HR-Min calc:
                    if  ( time % 60 == 0 ) return ( time / 60 ).ToString() + HOURS;
                    else if ( time <  60 ) return ( time % 60 ).ToString() + MIN;
                    else if ( time < 120 ) return ( time / 60 ).ToString() + HOUR + " " + (time % 60).ToString() + MIN;
                    else return ( time / 60 ).ToString() + HOURS + " " + (time % 60).ToString() + MIN;
            }
        }

        private string GetTamperStatus ( bool alarm, bool tamper = false )
        {
            if ( alarm )
                return ( tamper ) ? TRIGGERED : ENABLED;
            return DISABLED;
        }

        private string GetPortStatus (bool status)
        {
            return ( status ) ? ENABLED : DISABLED;
        }

        /*
        private ulong BcdToULong_Logic ( ulong valueInBCD )
        {
            // Define powers of 10 for the BCD conversion routines.
            ulong powers = 1;
            ulong outNum = 0;
            byte tempNum;

            for (int offset = 0; offset < 7; offset++)
            {
                tempNum = (byte)((valueInBCD >> offset * 8) & 0xff);
                if ((tempNum & 0x0f) > 9)
                {
                    break;
                }
                outNum += (ulong)(tempNum & 0x0f) * powers;
                powers *= 10;
                if ((tempNum >> 4) > 9)
                {
                    break;
                }
                outNum += (ulong)(tempNum >> 4) * powers;
                powers *= 10;
            }

            return outNum;
        }
        */

        public byte[] ULongToBcd_Logic ( string value, int size )
        {
            var convertedBytes = new byte[ size ];
            var strNumber      = value;
            var currentNumber  = string.Empty;

            for ( var i = 0; i < size; i++ )
                convertedBytes[i] = 0xff;

            for ( var i = 0; i < strNumber.Length; i++ )
            {
                currentNumber += strNumber[i];

                if (i == strNumber.Length - 1 && i % 2 == 0)
                {
                    convertedBytes[i / 2] = 0xf;
                    convertedBytes[i / 2] |= (byte)((int.Parse(currentNumber) % 10) << 4);
                }

                if (i % 2 == 0) continue;
                var v = int.Parse(currentNumber);
                convertedBytes[(i - 1) / 2] = (byte) (v % 10);
                convertedBytes[(i - 1) / 2] |= (byte)((v / 10) << 4);
                currentNumber = string.Empty;
            }

            return convertedBytes;
        }

        #endregion
    }
}
