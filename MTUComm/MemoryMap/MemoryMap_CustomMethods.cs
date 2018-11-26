using System;

namespace MTUComm.MemoryMap
{
    public partial class MemoryMap : AMemoryMap
    {
        #region Overloads

        public string DailySnap_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            int timeDiff = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            int curTime = MemoryRegisters.DailyGMTHourRead.Value + timeDiff;

            if (curTime < 0)
                curTime = 24 + curTime;
            if (curTime == 0)
                return "MidNight";
            if (curTime <= 11)
                return curTime.ToString() + " AM";
            if (curTime == 12)
                return "Noon";
            if (curTime > 12 && curTime < 24)
                return (curTime - 12).ToString() + " PM";
            else
                return "Off";
        }

        public string MtuStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return MemoryRegisters.Shipbit.Value ? "OFF" : "ON";
        }

        public string ReadInterval_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return TimeFormatter(MemoryRegisters.ReadIntervalMinutes.Value);
        }

        public string XmitInterval_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return TimeFormatter(MemoryRegisters.ReadIntervalMinutes.Value * MemoryRegisters.MessageOverlapCount.Value);
        }

        public string PCBNumber_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string tempString = string.Empty;
            //ASCII RANGE FOR PCBSupplierCode
            if (MemoryRegisters.PCBSupplierCode.Value >=65 && MemoryRegisters.PCBSupplierCode.Value <= 90)
            {
                tempString = tempString + Convert.ToChar(MemoryRegisters.PCBSupplierCode.Value) + "-";
            }

            if(MemoryRegisters.PCBCoreNumber.Value >= 0)
            {
                tempString = tempString + string.Format("{0:000000000}", MemoryRegisters.PCBCoreNumber.Value);
            }

            if (MemoryRegisters.PCBProductRevision.Value >= 65 && MemoryRegisters.PCBProductRevision.Value <= 90)
            {
                tempString = tempString + "-" +Convert.ToChar(MemoryRegisters.PCBProductRevision.Value);
            }
            return tempString.Equals(String.Empty) ? "Not Available" : tempString;
        }

        public string MtuSoftware_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            //if (MemoryRegisters)
            if(MemoryOverload.registerIds.Length > 1)
            {
                return string.Format("Version {0:00}.{1:00}.{2:0000}", 
                    MemoryRegisters.MTUFirmwareVersionMaior.Value,
                    MemoryRegisters.MTUFirmwareVersionMinor.Value,
                    MemoryRegisters.MTUFirmwareVersionBuild.Value);
            }
            else
            {
                return string.Format("Version {0:00}", MemoryRegisters.MTUFirmwareVersionFormatFlag.Value);
            }
        }

        public string Encryption_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return MemoryRegisters.Encrypted.Value ? "Yes" : "No";
        }

        public string MtuVoltageBattery_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return ((MemoryRegisters.MtuMiliVoltageBattery.Value * 1.0) / 1000).ToString("0.00 V");
        }

        public string P1ReadingError_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return TranslateErrorCodes(MemoryRegisters.P1ReadingErrorCode.Value);
        }

        public string P2ReadingError_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return TranslateErrorCodes(MemoryRegisters.P2ReadingErrorCode.Value);
        }

        public string InterfaceTamperStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return GetTemperStatus(MemoryRegisters.P1InterfaceAlarm.Value, MemoryRegisters.ProgrammingCoilInterfaceTamper.Value);
        }

        public string TiltTamperStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return GetTemperStatus(MemoryRegisters.P1TiltAlarm.Value, MemoryRegisters.TiltTamper.Value);
        }

        public string MagneticTamperStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return GetTemperStatus(MemoryRegisters.P1MagneticAlarm.Value, MemoryRegisters.MagneticTamper.Value);
        }

        public string RegisterCoverTamperStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return GetTemperStatus(MemoryRegisters.P1RegisterCoverAlarm.Value, MemoryRegisters.RegisterCoverTamper.Value);
        }

        public string ReverseFlowTamperStatus_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return GetTemperStatus(MemoryRegisters.P1ReverseFlowAlarm.Value, MemoryRegisters.ReverseFlowTamper.Value);
        }

        public string FastMessagingMode_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return MemoryRegisters.Fast2Way.Value ? "Fast" : "Slow";
        }

        public string LastGasp_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return MemoryRegisters.LastGaspTamper.Value ? "Enabled" : "Triggered";
        }

        public string InsufficentMemory_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return MemoryRegisters.InsufficentMemoryTamper.Value ? "Enabled" : "Triggered";
        }

        public string P1Status_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return GetPortStatus(MemoryRegisters.P1StatusFlag.Value);
        }

        public string P2Status_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return GetPortStatus(MemoryRegisters.P2StatusFlag.Value);
        }

        public string F12WAYRegister1_Get(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return HEX_PREFIX + MemoryRegisters.F12WAYRegister1Int.Value.ToString("X8");
        }

        public string F12WAYRegister10_Get(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return HEX_PREFIX + MemoryRegisters.F12WAYRegister10Int.Value.ToString("X8");
        }

        public string F12WAYRegister14_Get(MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            return HEX_PREFIX + MemoryRegisters.F12WAYRegister14Int.Value.ToString("X8");
        }

        #endregion

        #region Registers

        public int ReadIntervalMinutes_Set ( MemoryRegister<int> MemoryRegister, dynamic inputValue )
        {
            string[] readIntervalArray = ((string)inputValue).Split(' ');
            string readIntervalStr = readIntervalArray[0].ToLower ();
            string timeUnit = readIntervalArray[1];
            int timeIntervalMins = Int32.Parse(readIntervalStr);

            if (timeUnit is "hours")
                timeIntervalMins = timeIntervalMins * 60;

            return timeIntervalMins;
        }

        // Use with <CustomGet>method:ULongToBcd</CustomGet>
        public ulong BcdToULong ( MemoryRegister<ulong> MemoryRegister )
        {
            return this.BcdToULong_Logic ( ( ulong )MemoryRegister.ValueRaw );
        }

        // Use with <CustomSet>method:ULongToBcd</CustomSet>
        public ulong ULongToBcd ( MemoryRegister<ulong> MemoryRegister, dynamic inputValue )
        {
            if ( inputValue is string )
                return this.ULongToBcd_Logic ( inputValue );
            return this.ULongToBcd_Logic ( ( ulong )inputValue );
        }

        #endregion

        #region e-Coder

        public string BackFlowState_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string reply = string.Empty;
            string param = Convert.ToString(MemoryRegisters.FlowState.Value, 2).PadLeft(8, '0').Substring(6);
            switch (param)
            {
                case "00":
                    reply = "No reverse Flow Event in last 35 days";
                    break;
                case "01":
                    reply = "Small Reverse Flow Event in last 35 days";
                    break;
                case "10":
                    reply = "Large Reverse Flow Event in last 35 days";
                    break;
            }
            return reply;
        }

        public string DaysOfNoFlow_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string reply = string.Empty;
            string param = Convert.ToString(MemoryRegisters.FlowState.Value, 2).PadLeft(8, '0').Substring(3, 3);
            switch (param)
            {
                case "000":
                    reply = "0";
                    break;
                case "001":
                    reply = "1-2";
                    break;
                case "010":
                    reply = "3-7";
                    break;
                case "011":
                    reply = "8-14";
                    break;
                case "100":
                    reply = "15-21";
                    break;
                case "101":
                    reply = "22-34";
                    break;
                case "110":
                    reply = "35 (ALL)";
                    break;
            }

            return reply + " days of no consumption";
        }

        public string LeakDetection_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string reply = string.Empty;
            string param = Convert.ToString(MemoryRegisters.LeakState.Value, 2).PadLeft(8, '0').Substring(5, 2);
            switch (param)
            {
                case "00":
                    reply = "Less than 50 15-minute intervals";
                    break;
                case "01":
                    reply = "Between 50 and 95 15-minute intervals";
                    break;
                case "10":
                    reply = "Greater than 96 15-minute intervals";
                    break;
            }
            return reply;
        }

        public string DaysOfLeak_Get (MemoryOverload<string> MemoryOverload, dynamic MemoryRegisters)
        {
            string reply = string.Empty;
            string param = Convert.ToString(MemoryRegisters.LeakState.Value, 2).PadLeft(8, '0').Substring(2, 3);
            switch (param)
            {
                case "000":
                    reply = "0";
                    break;
                case "001":
                    reply = "1-2";
                    break;
                case "010":
                    reply = "3-7";
                    break;
                case "011":
                    reply = "8-14";
                    break;
                case "100":
                    reply = "15-21";
                    break;
                case "101":
                    reply = "22-34";
                    break;
                case "110":
                    reply = "35 (ALL)";
                    break;
            }

            return reply + " days of leak detection";
        }

        #endregion

        #region AuxiliaryFunctions

        private string TranslateErrorCodes (int encoderErrorcode)
        {
            if (encoderErrorcode == 0xFF)
                return "ERROR - Check Meter";
            if (encoderErrorcode == 0xFE)
                return "ERROR - Bad Digits";
            if (encoderErrorcode == 0xFD)
                return "ERROR - Delta Overflow";
            if (encoderErrorcode == 0xFC)
                return "ERROR - Readings Purged";
            return "";
        }

        private string TimeFormatter (int time)
        {
            switch (time)
            {
                case 2880: return "48 Hrs";
                case 2160: return "36 Hrs";
                case 1440: return "24 Hrs";
                case 720: return "12 Hrs";
                case 480: return "8 Hrs";
                case 360: return "6 Hrs";
                case 240: return "4 Hrs";
                case 180: return "3 Hrs";
                case 120: return "2 Hrs";
                case 90: return "1 Hr 30 Min";
                case 60: return "1 Hr";
                case 30: return "30 Min";
                case 15: return "15 Min";
                case 10: return "10 Min";
                case 5: return "5 Min";
                default: // KG 3.10.2010 add HR-Min calc:
                    if (time % 60 == 0)
                        return (time / 60).ToString() + " Hrs";
                    else
                        if (time < 60)
                        return (time % 60).ToString() + " Min";
                    else if (time < 120)
                        return (time / 60).ToString() + " Hr " + (time % 60).ToString() + " Min";
                    else
                        return (time / 60).ToString() + " Hrs " + (time % 60).ToString() + " Min";
                    //return xMit.ToString() + " Min";//"BAD READ";

            }
        }

        private string GetTemperStatus (bool alarm, bool temper)
        {
            if (alarm)
            {
                if (temper)
                {
                    return "Triggered";
                }
                else
                {
                    return "Enabled";
                }
            }
            else
            {
                return "Disabled";
            }
        }

        private string GetPortStatus (bool status)
        {
            return status ? "Enabled" : "Disabled";
        }

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

        public ulong ULongToBcd_Logic ( string value )
        {
            return ulong.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }

        public ulong ULongToBcd_Logic ( ulong value )
        {
            return this.ULongToBcd_Logic ( value.ToString () );
        }

        #endregion
    }
}
