using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MtuComm
{
    public class Mtu
    {
        private byte[] memory;

        public Mtu(byte[] memory)
        {
            this.memory = memory;
        }

        public string Status
        {
            get
            {
                byte systemFlags = memory[22];
                string mtuStatus = "Off";
                if ((systemFlags & 1) != 1)
                {
                    mtuStatus = "On";
                }
                return mtuStatus;
            }
        }

        public int SerialNumber
        {
            get
            {
                if (this.MtuType == 171)
                {
                    return (int)(memory[6] + (memory[7] << 8) + (memory[8] << 16) + (memory[9] << 24));
                }
                else if (this.MtuType == 138)
                {
                    return (int)(memory[6] + (memory[7] << 8) + (memory[8] << 16) + (memory[9] << 24));
                }
                else
                {
                    return 0;
                }
            }
        }

        public string Encrypted
        {
            get
            {
                return "Yes";
            }
        }

        public string MeterType
        {
            get
            {
                if (this.MeterTypeId == 3101)
                {
                    return "Pos 4D PF2 CCF";
                }
                else if (this.MeterTypeId == 1092)
                {
                    return "NEPT T10 5/8 E-Coder 0.1Gals";
                }
                else
                {
                    return "";
                }
            }
        }

        public int MeterTypeId
        {
            get
            {
                if (this.MtuType == 171)
                {
                    return (int)(memory[28] + (memory[29] << 8));
                }
                else if (this.MtuType == 138)
                {
                    return (int)(memory[32] + (memory[33] << 8));
                }
                else
                {
                    return 0;
                }
            }
        }

        public string Port1Desc
        {
            get
            {
                if (this.MtuType == 138)
                {
                    return "MTU PLS HRR ASO";
                }
                else if (this.MtuType == 171)
                {
                    return "MTU Water Single Port On-Demand Encoder ER";
                }
                else
                {
                    return "";
                }
            }
        }

        public ulong ServicePtId
        {
            get
            {
                if (this.MtuType == 171)
                {
                    return BcdToLong(
                    ((ulong)(memory[36])) +
                    ((ulong)memory[37] << 8) +
                    ((ulong)memory[38] << 16) +
                    ((ulong)memory[39] << 24) +
                    ((ulong)memory[40] << 32) +
                    ((ulong)memory[41] << 40));
                }
                else if (this.MtuType == 138)
                {
                    return BcdToLong(
                    ((ulong)(memory[34])) +
                    ((ulong)memory[35] << 8) +
                    ((ulong)memory[36] << 16) +
                    ((ulong)memory[37] << 24) +
                    ((ulong)memory[38] << 32) +
                    ((ulong)memory[39] << 40));
                }
                else
                {
                    return 0;
                }
            }
        }

        public static ulong BcdToLong(ulong inNum)
        {
            // Define powers of 10 for the BCD conversion routines.
            ulong powers = 1;
            ulong outNum = 0;
            byte tempNum;

            for (int offset = 0; offset < 7; offset++)
            {
                tempNum = (byte)((inNum >> offset * 8) & 0xff);
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

        public int MeterReading
        {
            get
            {
                if (this.MtuType == 171)
                {
                    int reading = memory[80] +
                        (memory[81] << 8) +
                        (memory[82] << 16) +
                        (memory[83] << 24) +
                        (memory[84] << 32) +
                        (memory[85] << 40) +
                        (memory[86] << 48) +
                        (memory[87] << 56);
                    return reading;
                }
                else if (this.MtuType == 138)
                {
                    int reading = memory[96] +
                        (memory[97] << 8) +
                        (memory[98] << 16) +
                        (memory[99] << 24) +
                        (memory[100] << 32) +
                        (memory[101] << 40) +
                        (memory[102] << 48) +
                        (memory[103] << 56);
                    return reading * 2; // TODO: subs by real HiResScaling value
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Digits
        {
            get
            {
                int digits = (int) memory[32];
                return digits;
            }
        }

        public int XmitInterval
        {
            get
            {
                if (this.MtuType == 171)
                {
                    int overlap = memory[25];
                    int readInterval = memory[26] + (memory[27] << 8);
                    int xmitInterval = overlap * readInterval;
                    return xmitInterval;
                }
                else if (this.MtuType == 138)
                {
                    int overlap = memory[25];
                    int readInterval = memory[26] + (memory[27] << 8);
                    int xmitInterval = overlap * readInterval;
                    return xmitInterval;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int ReadInterval
        {
            get
            {
                if (this.MtuType == 171)
                {
                    int readInterval = memory[26] + (memory[27] << 8);
                    return readInterval;
                }
                else if (this.MtuType == 138)
                {
                    int readInterval = memory[26] + (memory[27] << 8);
                    return readInterval;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string BatteryVoltage
        {
            get
            {
                if (this.MtuType == 171)
                {
                    double batteryVoltage = 1500 + (memory[110] * 10); // 1500mV offset + 10mV count read
                    batteryVoltage = batteryVoltage / 1000;
                    return batteryVoltage.ToString("0.00 V");
                }
                else if (this.MtuType == 138)
                {
                    double batteryVoltage = (double) (memory[113] * 9.766 * 2) + 250;
                    batteryVoltage = batteryVoltage / 1000;
                    return batteryVoltage.ToString("0.00 V");
                }
                else
                {
                    return "";
                }
            }
        }

        public string TwoWay
        {
            get
            {
                byte fastMessageConfig = memory[163];
                string twoWay = "Slow";
                if ((fastMessageConfig & 1) == 1)
                {
                    twoWay = "Fast";
                }
                return twoWay;
            }
        }

        public int OnDemandCount
        {
            get
            {
                int onDemandCount = memory[144] + (memory[145] << 8);
                return onDemandCount;
            }
        }

        public int DataReqCount
        {
            get
            {
                int dataReqCount = memory[146] + (memory[147] << 8);
                return dataReqCount;
            }
        }

        public int FotaCount
        {
            get
            {
                int fotaCount = memory[148];
                return fotaCount;
            }
        }

        public int FotcCount
        {
            get
            {
                int fotcCount = memory[149];
                return fotcCount;
            }
        }

        public int MtuType
        {
            get
            {
                int mtuType = memory[0];
                return mtuType;
            }
        }

        public string Software
        {
            get
            {
                if (this.MtuType == 171)
                {
                    // MTU Software: Version 01.04.0008
                    int mtuFirmwareVersionBuild = memory[240] + (memory[241] << 8);
                    int mtuFirmwareVersionMajor = memory[245];
                    int mtuFirmwareVersionMinor = memory[246];
                    string mtuSoftware = string.Format("Version {0:00}.{1:00}.{2:0000}",
                        mtuFirmwareVersionMajor,
                        mtuFirmwareVersionMinor,
                        mtuFirmwareVersionBuild);
                    return mtuSoftware;
                }
                else if (this.MtuType == 138)
                {
                    // MTU Software: Version 41
                    int mtuFirmwareVersion = memory[244]; 
                    string mtuSoftware = string.Format("Version {0:00}",
                        mtuFirmwareVersion);
                    return mtuSoftware;

                }
                else
                {
                    return "";
                }
            }
        }

        public string PcbNumber
        {
            get
            {
                if (this.MtuType == 171)
                {
                    return "0";
                }
                else if (this.MtuType == 138)
                {
                    char supplierCode = Convert.ToChar(memory[232]);
                    char productRevision = Convert.ToChar(memory[237]);
                    int pcbNumber = memory[233] +
                        (memory[234] << 8) +
                        (memory[235] << 16) +
                        (memory[236] << 24);
                    return string.Format("{0}-{1:000000000}-{2}",
                        supplierCode,
                        pcbNumber,
                        productRevision);
                }
                else
                {
                    return "";
                }
            }
        }

        public string TilTamp
        {
            get
            {
                if (this.MtuType == 138)
                {
                    string tilTamp = "Disabled";
                    byte p1mode = memory[42];
                    byte tampers = memory[71];
                    if (((p1mode >> 0) & 1) == 1)
                    {
                        if (((tampers >> 0) & 1) == 1)
                        {
                            tilTamp = "Triggered";
                        }
                        else
                        {
                            tilTamp = "Enabled";
                        }
                    }
                    return tilTamp;
                }
                else
                {
                    return "";
                }
            }
        }

        public string MagneticTamp
        {
            get
            {
                if (this.MtuType == 138)
                {
                    string magneticTamp = "Disabled";
                    byte p1mode = memory[42];
                    byte tampers = memory[71];
                    if (((p1mode >> 1) & 1) == 1)
                    {
                        if (((tampers >> 1) & 1) == 1)
                        {
                            magneticTamp = "Triggered";
                        }
                        else
                        {
                            magneticTamp = "Enabled";
                        }
                    }
                    return magneticTamp;
                }
                else
                {
                    return "";
                }
            }
        }

        public string InterfaceTamp
        {
            get
            {
                if (this.MtuType == 138)
                {
                    string interfaceTamp = "Disabled";
                    byte p1mode = memory[42];
                    byte tampers = memory[71];
                    if (((p1mode >> 4) & 1) == 1)
                    {
                        if (((tampers >> 4) & 1) == 1)
                        {
                            interfaceTamp = "Triggered";
                        }
                        else
                        {
                            interfaceTamp = "Enabled";
                        }
                    }
                    return interfaceTamp;
                }
                else if (this.MtuType == 171)
                {
                    string interfaceTamp = "Enabled";
                    byte tampers = memory[109];
                    if (((tampers >> 4) & 1) == 1)
                    {
                        interfaceTamp = "Triggered";
                    }
                    return interfaceTamp;
                }
                else
                {
                    return "";
                }
            }
        }

        public string InsfMem
        {
            get
            {
                if (this.MtuType == 171)
                {
                    string insfMem = "Disabled";
                    byte tampers = memory[108];
                    if (((tampers >> 0) & 1) == 1)
                    {
                        insfMem = "Triggered";
                    }
                    return insfMem;
                }
                else
                {
                    return "";
                }
            }
        }

        public string LastGasp
        {
            get
            {
                if (this.MtuType == 171)
                {
                    string lastGasp = "Disabled";
                    byte tampers = memory[108];
                    if (((tampers >> 4) & 1) == 1)
                    {
                        lastGasp = "Triggered";
                    }
                    return lastGasp;
                }
                else
                {
                    return "";
                }
            }
        }

        public string RegCover
        {
            get
            {
                if (this.MtuType == 138)
                {
                    string regCover = "Disabled";
                    byte p1mode = memory[42];
                    byte tampers = memory[71];
                    if (((p1mode >> 5) & 1) == 1)
                    {
                        if (((tampers >> 5) & 1) == 1)
                        {
                            regCover = "Triggered";
                        }
                        else
                        {
                            regCover = "Enabled";
                        }
                    }
                    return regCover;
                }
                else
                {
                    return "";
                }
            }
        }

        public string RevFlTamp
        {
            get
            {
                if (this.MtuType == 138)
                {
                    string revFlTamp = "Disabled";
                    byte p1mode = memory[42];
                    byte tampers = memory[71];
                    if (((p1mode >> 6) & 1) == 1)
                    {
                        if (((tampers >> 6) & 1) == 1)
                        {
                            revFlTamp = "Triggered";
                        }
                        else
                        {
                            revFlTamp = "Enabled";
                        }
                    }
                    return revFlTamp;
                }
                else
                {
                    return "";
                }
            }
        }

        public string DailySnap
        {
            get
            {
                int timeDiff = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
                int curTime = memory[198] + timeDiff;

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
        }
    }
}
