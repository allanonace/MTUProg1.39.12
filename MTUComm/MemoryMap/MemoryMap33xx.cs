using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUComm.MemoryMap
{
    public class MemoryMap33xx: MemoryMap
    {
        private const string FAMILY = "33xx";

        public MemoryMap33xx(byte[] memory) : base ( memory, FAMILY )
        {

        }

        /*
        // addr 0
        // MTU type
        public int MtuType
        {
            get
            {
                int mtuType = memory[0];
                return mtuType;
            }
        }

        // addr 1
        // MTU version (OBSOLETE)

        // addr 2
        // revision year (OBSOLETE)

        // addr 3
        // revision month (OBSOLETE)

        // addr 4
        // revision day (OBSOLETE)

        // addr 5
        // temperature correction
        public int TemperatureCorrection
        {
            get
            {
                int temperatureCorrection = memory[5];
                return temperatureCorrection;
            }
        }

        // addr 6-9
        // MTU ID
        public int MtuId
        {
            get
            {
                int mtuId = (memory[6] + (memory[7] << 8) + (memory[8] << 16) + (memory[9] << 24));
                return mtuId;
            }
        }

        // addr 10-13
        // one way tx frequency
        public int OneWayTxFrequency
        {
            get
            {
                int oneWayTxFrequency = (memory[10] + (memory[11] << 8) + (memory[12] << 16) + (memory[13] << 24));
                return oneWayTxFrequency;
            }
        }

        // addr 14-17
        // two way tx frequency
        public int TwoWayTxFrequency
        {
            get
            {
                int twoWayTxFrequency = (memory[14] + (memory[15] << 8) + (memory[16] << 16) + (memory[17] << 24));
                return twoWayTxFrequency;
            }
        }

        // addr 18-21
        // two way rx frequency
        public int TwoWayRxFrequency
        {
            get
            {
                int twoWayRxFrequency = (memory[18] + (memory[19] << 8) + (memory[20] << 16) + (memory[21] << 24));
                return twoWayRxFrequency;
            }
        }

        // addr 22 bit 0
        // system flags - shipbit
        public bool Shipbit
        {
            get
            {
                byte systemFlags = memory[22];
                bool shipbit = (((systemFlags >> 0) & 1) == 1);
                return shipbit;
            }
        }

        // addr 22 bit 1
        // system flags - 2 way off / no afc
        public bool TwoWayOffNoAfc
        {
            get
            {
                byte systemFlags = memory[22];
                bool twoWayOffNoAfc = (((systemFlags >> 1) & 1) == 1);
                return twoWayOffNoAfc;
            }
        }

        // addr 22 bit 2
        // system flags - rf test mode
        public bool RfTestMode
        {
            get
            {
                byte systemFlags = memory[22];
                bool rfTestMode = (((systemFlags >> 2) & 1) == 1);
                return rfTestMode;
            }
        }

        // addr 22 bit 3
        // system flags - precharge switch
        public bool PrechargeSwitch
        {
            get
            {
                byte systemFlags = memory[22];
                bool prechargeSwitch = (((systemFlags >> 3) & 1) == 1);
                return prechargeSwitch;
            }
        }

        // addr 23
        // wakeup message constant
        public int WakeupMessageConstant
        {
            get
            {
                int wakeupMessageConstant = memory[23];
                return wakeupMessageConstant;
            }
        }

        // addr 24
        // frequency correction
        public int FrequencyCorrection
        {
            get
            {
                int frequencyCorrection = memory[24];
                return frequencyCorrection;
            }
        }

        // addr 25
        // message overlap count
        public int MessageOverlapCount
        {
            get
            {
                int messageOverlapCount = memory[25];
                return messageOverlapCount;
            }
        }

        // addr 26-27
        // read interval
        public int ReadInterval
        {
            get
            {
                int readInterval = (memory[26] + (memory[27] << 8));
                return readInterval;
            }
        }

        // addr 28 bit 0
        // port 1 enable
        public bool Port1Enable
        {
            get
            {
                byte portsEnable = memory[28];
                bool port1Enable = (((portsEnable >> 0) & 1) == 1);
                return port1Enable;
            }
        }

        // addr 28 bit 1
        // port 2 enable
        public bool Port2Enable
        {
            get
            {
                byte portsEnable = memory[28];
                bool port2Enable = (((portsEnable >> 1) & 1) == 1);
                return port2Enable;
            }
        }

        // addr 29
        // time sync hour
        public int TimeSyncHour
        {
            get
            {
                int timeSyncHour = memory[29];
                return timeSyncHour;
            }
        }

        // addr 30
        // time sync minute
        public int TimeSyncMinute
        {
            get
            {
                int timeSyncMinute = memory[30];
                return timeSyncMinute;
            }
        }

        // addr 31
        // time sync second
        public int TimeSyncSecond
        {
            get
            {
                int timeSyncSecond = memory[31];
                return timeSyncSecond;
            }
        }

        // addr 32
        // P1 info
        public int P1Info
        {
            get
            {
                int p1Info = memory[32];
                return p1Info;
            }
        }

        // addr 33
        // P1 meter type
        public int P1MeterType
        {
            get
            {
                int p1MeterType = memory[33];
                return p1MeterType;
            }
        }

        // addr 34-39
        // P1 meter ID
        public ulong P1MeterId // TODO: convert from BCD format to long
        {
            get
            {

                ulong p1MeterId = ((ulong)memory[34] +
                    ((ulong)memory[35] << 8) +
                    ((ulong)memory[36] << 16) +
                    ((ulong)memory[37] << 24) +
                    ((ulong)memory[38] << 32) +
                    ((ulong)memory[39] << 40));
                return BcdToLong(p1MeterId);
            }
        }

        // addr 40
        // Encoder type
        public int EncoderType
        {
            get
            {
                int encoderType = memory[40];
                return encoderType;
            }
        }

        // addr 40-41
        // P1 pulse ratio
        public int P1PulseRatio
        {
            get
            {
                int p1PulseRatio = (memory[40] + (memory[41] << 8));
                return p1PulseRatio;
            }
        }

        // addr 42 bit 0-1
        // P1 mode - alarm mask
        public int P1AlarmMask
        {
            get
            {
                int alarmMask = ((memory[42] & 0x03) >> 2);
                return alarmMask;
            }
        }

        // addr 42 bit 2
        // P1 mode - immediate alarm
        public bool P1ImmediateAlarm
        {
            get
            {
                byte p1Mode = memory[42];
                bool immediateAlarm = (((p1Mode >> 2) & 1) == 1);
                return immediateAlarm;
            }
        }

        // addr 42 bit 3
        // P1 mode - urgent alarm
        public bool P1UrgentAlarm
        {
            get
            {
                byte p1Mode = memory[42];
                bool urgentAlarm = (((p1Mode >> 3) & 1) == 1);
                return urgentAlarm;
            }
        }

        // addr 42 bit 4
        // P1 mode - pulse type
        public bool P1PulseType
        {
            get
            {
                byte p1Mode = memory[42];
                bool pulseType = (((p1Mode >> 4) & 1) == 1);
                return pulseType;
            }
        }

        // addr 42 bit 5
        // P1 mode - encoder type
        public bool P1EncoderType
        {
            get
            {
                byte p1Mode = memory[42];
                bool encoderType = (((p1Mode >> 5) & 1) == 1);
                return encoderType;
            }
        }

        // addr 42 bit 6
        // P1 mode - two wire pulse
        public bool P1TwoWirePulse
        {
            get
            {
                byte p1Mode = memory[42];
                bool twoWirePulse = (((p1Mode >> 6) & 1) == 1);
                return twoWirePulse;
            }
        }

        // addr 42 bit 7
        // P1 mode - interrupt on tamper
        public bool P1InterruptOnTamper
        {
            get
            {
                byte p1Mode = memory[42];
                bool interruptOnTamper = (((p1Mode >> 7) & 1) == 1);
                return interruptOnTamper;
            }
        }

        // addr 43
        // P1 pulse high time
        public int P1PulseHighTime
        {
            get
            {
                int pulseHighTime = memory[43];
                return pulseHighTime;
            }
        }

        // addr 43
        // P1 Encoder number of digits
        public int P1EncoderNumberOfDigits
        {
            get
            {
                int encoderNumberOfDigits = memory[43];
                return encoderNumberOfDigits;
            }
        }

        // addr 44
        // P1 pulse low time
        public int P1PulseLowTime
        {
            get
            {
                int pulseLowTime = memory[44];
                return pulseLowTime;
            }
        }

        // addr 45
        // Number of tx between rx
        public int NumberTxBetweenRx
        {
            get
            {
                int numberTxBetweenRx = memory[45];
                return numberTxBetweenRx;
            }
        }

        // addr 46-47
        // Time between rx in minutes
        public int TimeBetweenRx
        {
            get
            {
                int timeBetweenRx = (memory[46] + (memory[47] << 8));
                return timeBetweenRx;
            }
        }

        // addr 48
        // P2 info
        public int P2Info
        {
            get
            {
                int p2Info = memory[48];
                return p2Info;
            }
        }

        // addr 49
        // P2 meter type
        public int P2MeterType
        {
            get
            {
                int p2MeterType = memory[49];
                return p2MeterType;
            }
        }

        // addr 50-55
        // P2 meter ID
        public ulong P2MeterId
        {
            get
            {
                ulong p2MeterId = ((ulong)memory[50] + ((ulong)memory[51] << 8) + ((ulong)memory[52] << 16) + ((ulong)memory[53] << 24) + ((ulong)memory[54] << 32) + ((ulong)memory[55] << 40));
                return BcdToLong(p2MeterId);
            }
        }

        // addr 56-57
        // P2 pulse ratio
        public int P2PulseRatio
        {
            get
            {
                int p2PulseRatio = (memory[56] + (memory[57] << 8));
                return p2PulseRatio;
            }
        }

        // addr 58 bit 0-1
        // P2 mode - alarm mask
        public int P2AlarmMask
        {
            get
            {
                int alarmMask = ((memory[58] & 0x03) >> 2);
                return alarmMask;
            }
        }

        // addr 58 bit 2
        // P2 mode - immediate alarm
        public bool P2ImmediateAlarm
        {
            get
            {
                byte p2Mode = memory[58];
                bool immediateAlarm = (((p2Mode >> 2) & 1) == 1);
                return immediateAlarm;
            }
        }

        // addr 58 bit 3
        // P2 mode - urgent alarm
        public bool P2UrgentAlarm
        {
            get
            {
                byte p2Mode = memory[58];
                bool urgentAlarm = (((p2Mode >> 3) & 1) == 1);
                return urgentAlarm;
            }
        }

        // addr 58 bit 4
        // P2 mode - pulse type
        public bool P2Pulsetype
        {
            get
            {
                byte p2Mode = memory[58];
                bool pulseType = (((p2Mode >> 4) & 1) == 1);
                return pulseType;
            }
        }

        // addr 58 bit 5
        // P2 mode - encoder type
        public bool P2EncoderType
        {
            get
            {
                byte p2Mode = memory[58];
                bool encoderType = (((p2Mode >> 5) & 1) == 1);
                return encoderType;
            }
        }

        // addr 58 bit 6
        // P2 mode - two wire pulse
        public bool P2TwoWirePulse
        {
            get
            {
                byte p2Mode = memory[58];
                bool twoWirePulse = (((p2Mode >> 6) & 1) == 1);
                return twoWirePulse;
            }
        }

        // addr 58 bit 7
        // P2 mode - interrupt on tamper
        public bool P2InterruptOnTamper
        {
            get
            {
                byte p2Mode = memory[58];
                bool interruptOnTamper = (((p2Mode >> 7) & 1) == 1);
                return interruptOnTamper;
            }
        }

        // addr 59
        // P2 pulse high time
        public int P2PulseHighTime
        {
            get
            {
                int p2PulseHighTime = memory[59];
                return p2PulseHighTime;
            }
        }

        // addr 60
        // P2 pulse low time
        public int P2PulseLowTime
        {
            get
            {
                int p2PulseLowTime = memory[60];
                return p2PulseLowTime;
            }
        }

        // addr 61
        // P2 message overlap count
        public int P2MessageOverlapCount
        {
            get
            {
                int p2MessageOverlapCount = memory[61];
                return p2MessageOverlapCount;
            }
        }

        // addr 62-63
        // P2 read interval
        public int P2ReadInterval
        {
            get
            {
                int p2ReadInterval = (memory[62] + (memory[63] << 8));
                return p2ReadInterval;
            }
        }

        // addr 64 bit 0
        // Task flags - read meter
        public bool TaskFlagsReadMeter
        {
            get
            {
                byte taskFlags = memory[64];
                bool readMeter = (((taskFlags >> 0) & 1) == 1);
                return readMeter;
            }
        }

        // addr 64 bit 1
        // Task flags - tx
        public bool TaskFlagsTx
        {
            get
            {
                byte taskFlags = memory[64];
                bool tx = (((taskFlags >> 1) & 1) == 1);
                return tx;
            }
        }

        // addr 64 bit 2
        // Task flags - rtc correction
        public bool TaskFlagsRtcCorrection
        {
            get
            {
                byte taskFlags = memory[64];
                bool rtcCorrection = (((taskFlags >> 2) & 1) == 1);
                return rtcCorrection;
            }
        }

        // addr 64 bit 3
        // Task flags - afc
        public bool TaskFlagsAfc
        {
            get
            {
                byte taskFlags = memory[64];
                bool afc = (((taskFlags >> 3) & 1) == 1);
                return afc;
            }
        }

        // addr 64 bit 4
        // Task flags - coil msg
        public bool TaskFlagsCoilMsg
        {
            get
            {
                byte taskFlags = memory[64];
                bool coilMsg = (((taskFlags >> 4) & 1) == 1);
                return coilMsg;
            }
        }

        // addr 64 bit 5
        // Task flags - rx msg
        public bool TaskFlagsRxMsg
        {
            get
            {
                byte taskFlags = memory[64];
                bool rxMsg = (((taskFlags >> 5) & 1) == 1);
                return rxMsg;
            }
        }

        // addr 64 bit 6
        // Task flags - rx byte
        public bool TaskFlagsRxByte
        {
            get
            {
                byte taskFlags = memory[64];
                bool rxByte = (((taskFlags >> 6) & 1) == 1);
                return rxByte;
            }
        }

        // addr 64 bit 7
        // Task flags - enter receive mode
        public bool TaskFlagsEnterReceiveMode
        {
            get
            {
                byte taskFlags = memory[64];
                bool enterReceiveMode = (((taskFlags >> 7) & 1) == 1);
                return enterReceiveMode;
            }
        }

        // addr 65 bit 0
        // Task flags - install_f
        public bool TaskFlagsInstallF
        {
            get
            {
                byte taskFlags = memory[65];
                bool installF = (((taskFlags >> 0) & 1) == 1);
                return installF;
            }
        }

        // addr 65 bit 1
        // Task flags - tamper_f
        public bool TaskFlagsTamperF
        {
            get
            {
                byte taskFlags = memory[65];
                bool tamperF = (((taskFlags >> 1) & 1) == 1);
                return tamperF;
            }
        }

        // addr 65 bit 2
        // Task flags - time sync f
        public bool TaskFlagsTimeSyncF
        {
            get
            {
                byte taskFlags = memory[65];
                bool timeSyncF = (((taskFlags >> 2) & 1) == 1);
                return timeSyncF;
            }
        }

        // addr 65 bit 3
        // Task flags - update reg0 flag
        public bool TaskFlagsUpdateReg0
        {
            get
            {
                byte taskFlags = memory[65];
                bool updateReg0 = (((taskFlags >> 3) & 1) == 1);
                return updateReg0;
            }
        }

        // addr 65 bit 4
        // Task flags - cal temperature flag
        public bool TaskFlagsCalTemperature
        {
            get
            {
                byte taskFlags = memory[65];
                bool calTemperature = (((taskFlags >> 4) & 1) == 1);
                return calTemperature;
            }
        }

        // addr 65 bit 5
        // Task flags - test tx flag
        public bool TaskFlagsTestTx
        {
            get
            {
                byte taskFlags = memory[65];
                bool testTx = (((taskFlags >> 5) & 1) == 1);
                return testTx;
            }
        }

        // addr 65 bit 6
        // Task flags - encoder autodetect flag
        public bool TaskFlagsEncoderAutodetect
        {
            get
            {
                byte taskFlags = memory[65];
                bool encoderAutodetect = (((taskFlags >> 6) & 1) == 1);
                return encoderAutodetect;
            }
        }

        // addr 65 bit 7
        // Task flags - encoder port2 autodetect flag
        public bool TaskFlagsEncoderPort2Autodetect
        {
            get
            {
                byte taskFlags = memory[65];
                bool encoderPort2Autodetect = (((taskFlags >> 7) & 1) == 1);
                return encoderPort2Autodetect;
            }
        }

        // addr 66-67
        // tx count down
        public int TxCountDown
        {
            get
            {
                int txCountDown = (memory[66] + (memory[67] << 8));
                return txCountDown;
            }
        }

        // addr 68-69
        // read count down
        public int ReadCountDown
        {
            get
            {
                int readCountDown = (memory[68] + (memory[69] << 8));
                return readCountDown;
            }
        }

        // addr 70
        // afc correction
        public int AfcCorrection
        {
            get
            {
                int afcCorrection = memory[70];
                return afcCorrection;
            }
        }

        // addr 71 bit 0
        // tamper memory port 1
        public bool TamperOnPort1
        {
            get
            {
                byte tamperMemory = memory[71];
                bool port1Tamper = (((tamperMemory >> 0) & 1) == 1);
                return port1Tamper;
            }
        }

        // addr 71 bit 1
        // tamper memory port 2
        public bool TamperOnPort2
        {
            get
            {
                byte tamperMemory = memory[71];
                bool port2Tamper = (((tamperMemory >> 1) & 1) == 1);
                return port2Tamper;
            }
        }



        // addr 72
        // encoder errorcode
        public int EncoderErrorcode
        {
            get
            {
                int encoderErrorcode = memory[72];
                return encoderErrorcode;
            }
        }

        // addr 73 bit 4
        // wire break port2
        public bool WireBreakPort2
        {
            get
            {
                bool wireBreakPort2 = (((memory[73] >> 4) & 1) == 1);
                return wireBreakPort2;
            }
        }

        // addr 73 bit 5
        // wire break
        public bool WireBreak
        {
            get
            {
                bool wireBreak = (((memory[73] >> 5) & 1) == 1);
                return wireBreak;
            }
        }

        // addr 73 bit 6
        // dco uncalled
        public bool DcoUncalled
        {
            get
            {
                bool dcoUncalled = (((memory[73] >> 6) & 1) == 1);
                return dcoUncalled;
            }
        }

        // addr 73 bit 7
        // 32kHz crystal fail
        public bool CrystalFail
        {
            get
            {
                bool crystalFail = (((memory[73] >> 7) & 1) == 1);
                return crystalFail;
            }
        }

        // addr 74
        // rtc second
        public int RtcSecond
        {
            get
            {
                int rtcSecond = memory[74];
                return rtcSecond;
            }
        }

        // addr 75
        // rtc minute
        public int RtcMinute
        {
            get
            {
                int rtcMinute = memory[75];
                return rtcMinute;
            }
        }

        // addr 76
        // rtc hour
        public int RtcHour
        {
            get
            {
                int rtcHour = memory[76];
                return rtcHour;
            }
        }

        // addr 77
        // rtc day
        public int RtcDay
        {
            get
            {
                int rtcDay = memory[77];
                return rtcDay;
            }
        }

        // addr 78
        // rtc month
        public int RtcMonth
        {
            get
            {
                int rtcMonth = memory[78];
                return rtcMonth;
            }
        }

        // addr 79
        // rtc year
        public int RtcYear
        {
            get
            {
                int rtcYear = memory[79];
                return rtcYear;
            }
        }

        // addr 80-83
        // 7021 reg0 one way tx
        public int Reg0OneWayTx
        {
            get
            {
                int reg0OneWayTx = (memory[80] + (memory[81] << 8) + (memory[82] << 16) + (memory[83] << 24));
                return reg0OneWayTx;
            }
        }


        // addr 84-87
        // 7021 reg0 two way tx
        public int Reg0TwoWayTx
        {
            get
            {
                int reg0TwoWayTx = (memory[84] + (memory[85] << 8) + (memory[86] << 16) + (memory[87] << 24));
                return reg0TwoWayTx;
            }
        }

        // addr 88-91
        // 7021 reg0 two way rx
        public int Reg0TwoWayRx
        {
            get
            {
                int reg0TwoWayRx = (memory[88] + (memory[89] << 8) + (memory[90] << 16) + (memory[91] << 24));
                return reg0TwoWayRx;
            }
        }

        // addr 92-95
        // MTU/DCU ID of last packet received
        public int MtuDcuIdLastPacket
        {
            get
            {
                int mtuDcuIdLastPacket = (memory[92] + (memory[93] << 8) + (memory[94] << 16) + (memory[95] << 24));
                return mtuDcuIdLastPacket;
            }
        }

        // addr 96-103
        // P1 reading
        public uint P1Reading
        {
            get
            {
                uint p1Reading = (uint)memory[96] +
                    ((uint)memory[97] << 8) +
                    ((uint)memory[98] << 16) +
                    ((uint)memory[99] << 24) +
                    ((uint)memory[100] << 32) +
                    ((uint)memory[101] << 40) +
                    ((uint)memory[102] << 48) +
                    ((uint)memory[103] << 56);
                return p1Reading;
            }
        }

        // addr 102-103
        // P1 scaler
        public int P1Scaler
        {
            get
            {
                int p1Scaler = (memory[102] + (memory[103] << 8));
                return p1Scaler;
            }
        }

        // addr 104-111
        // P2 reading
        public int P2Reading
        {
            get
            {
                int p2Reading = memory[104] +
                    (memory[105] << 8) +
                    (memory[106] << 16) +
                    (memory[107] << 24) +
                    (memory[108] << 32) +
                    (memory[109] << 40) +
                    (memory[110] << 48) +
                    (memory[111] << 56);
                return p2Reading;
            }
        }

        // addr 110-111
        // P2 scaler
        public int P2Scaler
        {
            get
            {
                int p2Scaler = (memory[110] + (memory[111] << 8));
                return p2Scaler;
            }
        }

        // addr 112
        // p2 encoder errorcode
        public int P2EncoderErrorcode
        {
            get
            {
                int p2EncoderErrorcode = memory[112];
                return p2EncoderErrorcode;
            }
        }

        // addr 113
        // battery voltage (AD value) | battery volt = ADvalue * 9.766mV*2 + 250
        public int BatteryVoltage
        {
            get
            {
                int batteryVoltageAd = memory[113];
                return batteryVoltageAd;
            }
        }

        // addr 114
        // charge ready count down
        public int ChargeReadyCountDown
        {
            get
            {
                int chargeReadyCountDown = memory[114];
                return chargeReadyCountDown;
            }
        }

        // addr 115
        // watchdog state from last reset
        public int WatchDogState
        {
            get
            {
                int watchDogState = memory[115];
                return watchDogState;
            }
        }

        // addr 116 bit 0
        // status flags - tx_count_down_f
        public bool StatusTxCountDownF
        {
            get
            {
                byte statusFlags = memory[116];
                bool txCountDownF = (((statusFlags >> 0) & 1) == 1);
                return txCountDownF;
            }
        }

        // addr 116 bit 1
        // status flags - rtc_not_synced_with_DCU_f
        public bool StatusRtcNotSyncedWithDcuF
        {
            get
            {
                byte statusFlags = memory[116];
                bool rtcNotSyncedWithDcuF = (((statusFlags >> 1) & 1) == 1);
                return rtcNotSyncedWithDcuF;
            }
        }

        // addr 116 bit 2
        // status flags - p1_tx_f
        public bool StatusP1TxF
        {
            get
            {
                byte statusFlags = memory[116];
                bool p1TxF = (((statusFlags >> 2) & 1) == 1);
                return p1TxF;
            }
        }

        // addr 117 bit 0
        // sched_tsr_f
        public bool SchedTsrF
        {
            get
            {
                bool schedTsrF = (((memory[117] >> 0) & 1) == 1);
                return schedTsrF;
            }
        }

        // addr 118-119
        // crc program flash
        public int CrcProgramFlash
        {
            get
            {
                int crcProgramFlash = (memory[118] + (memory[119] << 8));
                return crcProgramFlash;
            }
        }

        // addr 120
        // RSSI of last received packet
        public int RssiLastReceivedPacket
        {
            get
            {
                int rssiLastReceivedPacket = memory[120];
                return rssiLastReceivedPacket;
            }
        }

        // addr 121
        // Deviation of last received packet
        public int DeviationLastReceivedPacket
        {
            get
            {
                int deviationLastReceivedPacket = memory[121];
                return deviationLastReceivedPacket;
            }
        }

        // addr 122 bit 0
        // port 1 leak current alarm flag
        public bool Port1LeakCurrentAlarmF
        {
            get
            {
                bool port1LeakCurrentAlarmF = (((memory[122] >> 0) & 1) == 1);
                return port1LeakCurrentAlarmF;
            }
        }

        // addr 123-124
        // Unused

        // addr 125
        // test

        // addr 126
        // Unused

        // addr 127
        // toggle switcher (not supported anymore)

        // addr 128-131
        // 7021 reg0
        public int Reg0
        {
            get
            {
                int reg0 = (memory[128] + (memory[129] << 8) + (memory[130] << 16) + (memory[131] << 24));
                return reg0;
            }
        }

        // addr 132-135
        // 7021 reg1
        public int Reg1
        {
            get
            {
                int reg1 = (memory[132] + (memory[133] << 8) + (memory[134] << 16) + (memory[135] << 24));
                return reg1;
            }
        }

        // addr 136-139
        // 7021 reg2
        public int Reg2
        {
            get
            {
                int reg2 = (memory[136] + (memory[137] << 8) + (memory[138] << 16) + (memory[139] << 24));
                return reg2;
            }
        }

        // addr 140-143
        // 7021 reg3
        public int Reg3
        {
            get
            {
                int reg3 = (memory[140] + (memory[141] << 8) + (memory[142] << 16) + (memory[143] << 24));
                return reg3;
            }
        }

        // addr 144-147
        // 7021 reg4
        public int Reg4
        {
            get
            {
                int reg4 = (memory[144] + (memory[145] << 8) + (memory[146] << 16) + (memory[147] << 24));
                return reg4;
            }
        }

        // addr 148-151
        // 7021 reg5
        public int Reg5
        {
            get
            {
                int reg5 = (memory[148] + (memory[149] << 8) + (memory[150] << 16) + (memory[151] << 24));
                return reg5;
            }
        }

        // addr 152-155
        // 7021 reg6
        public int Reg6
        {
            get
            {
                int reg6 = (memory[152] + (memory[153] << 8) + (memory[154] << 16) + (memory[155] << 24));
                return reg6;
            }
        }

        // addr 156-159
        // 7021 reg7
        public int Reg7
        {
            get
            {
                int reg7 = (memory[156] + (memory[157] << 8) + (memory[158] << 16) + (memory[159] << 24));
                return reg7;
            }
        }

        // addr 160-163
        // 7021 reg8
        public int Reg8
        {
            get
            {
                int reg8 = (memory[160] + (memory[161] << 8) + (memory[162] << 16) + (memory[163] << 24));
                return reg8;
            }
        }

        // addr 164-167
        // 7021 reg9
        public int Reg9
        {
            get
            {
                int reg9 = (memory[164] + (memory[165] << 8) + (memory[166] << 16) + (memory[167] << 24));
                return reg9;
            }
        }

        // addr 168-171
        // 7021 reg10
        public int Reg10
        {
            get
            {
                int reg10 = (memory[168] + (memory[169] << 8) + (memory[170] << 16) + (memory[171] << 24));
                return reg10;
            }
        }

        // addr 172-175
        // 7021 reg11
        public int Reg11
        {
            get
            {
                int reg11 = (memory[172] + (memory[173] << 8) + (memory[174] << 16) + (memory[175] << 24));
                return reg11;
            }
        }

        // addr 176-179
        // 7021 reg12
        public int Reg12
        {
            get
            {
                int reg12 = (memory[176] + (memory[177] << 8) + (memory[178] << 16) + (memory[179] << 24));
                return reg12;
            }
        }

        // addr 180-183
        // 7021 reg0 rx
        public int Reg0Rx
        {
            get
            {
                int reg0Rx = (memory[180] + (memory[181] << 8) + (memory[182] << 16) + (memory[183] << 24));
                return reg0Rx;
            }
        }

        // addr 184-187
        // 7021 reg1 rx
        public int Reg1Rx
        {
            get
            {
                int reg1Rx = (memory[184] + (memory[185] << 8) + (memory[186] << 16) + (memory[187] << 24));
                return reg1Rx;
            }
        }

        // addr 188-191
        // 7021 reg15
        public int Reg15
        {
            get
            {
                int reg15 = (memory[188] + (memory[189] << 8) + (memory[190] << 16) + (memory[191] << 24));
                return reg15;
            }
        }

        // addr 192-193
        // tx/rx counter
        public int TxRxCounter
        {
            get
            {
                int txRxCounter = (memory[192] + (memory[193] << 8));
                return txRxCounter;
            }
        }

        // addr 194-195
        // reset count
        public int ResetCount
        {
            get
            {
                int resetCount = (memory[194] + (memory[195] << 8));
                return resetCount;
            }
        }

        // addr 196
        // POR count
        public int PorCount
        {
            get
            {
                int porCount = memory[196];
                return porCount;
            }
        }

        // addr 197
        // Time difference alarm setting in seconds
        public int TimeDifferenceAlarmSetting
        {
            get
            {
                int timeDifferenceAlarmSetting = memory[197];
                return timeDifferenceAlarmSetting;
            }
        }

        // addr 198
        // Daily read
        public int DailyRead
        {
            get
            {
                int dailyRead = memory[198];
                return dailyRead;
            }
        }

        public String DailySnap
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

        // addr 199
        // AFC no change count
        public int AfcNoChangeCount
        {
            get
            {
                int afcNoChangeCount = memory[199];
                return afcNoChangeCount;
            }
        }

        // addr 200 bit 1
        // leak detection current alarm enable
        public bool LeakDetectionCurrentAlarmEnable
        {
            get
            {
                bool leakDetectionCurrentAlarmEnable = (((memory[200] >> 1) & 1) == 1);
                return leakDetectionCurrentAlarmEnable;
            }
        }

        // addr 200 bit 2
        // number days of leak alarm enable
        public bool NumberDaysLeakAlarmEnable
        {
            get
            {
                bool numberDaysLeakAlarmEnable = (((memory[200] >> 2) & 1) == 1);
                return numberDaysLeakAlarmEnable;
            }
        }

        // addr 200 bit 3
        // number days no flow alarm enable
        public bool NumberDaysNoFlowAlarmEnable
        {
            get
            {
                bool numberDaysNoFlowAlarmEnable = (((memory[200] >> 3) & 1) == 1);
                return numberDaysNoFlowAlarmEnable;
            }
        }

        // addr 200 bit 4
        // reverse flow alarm enable
        public bool ReverseFlowAlarmEnable
        {
            get
            {
                bool reverseFlowAlarmEnable = (((memory[200] >> 4) & 1) == 1);
                return reverseFlowAlarmEnable;
            }
        }

        // addr 201 bits 0..1
        // port 1 leak detection current min value
        public int Port1LeakDetectionCurrentMinValue
        {
            get
            {
                int port1LeakDetectionCurrentMinValue = ((memory[201] & 0x03) >> 0);
                return port1LeakDetectionCurrentMinValue;
            }
        }

        // addr 201 bits 2..4
        // port 1 days of leak min value
        public int Port1DaysOfLeakMinValue
        {
            get
            {
                int port1DaysOfLeakMinValue = ((memory[201] & 0x1c) >> 2);
                return port1DaysOfLeakMinValue;
            }
        }

        // addr 201 bits 5..7
        // port 1 days of no flow
        public int Port1DaysOfNoFlow
        {
            get
            {
                int port1DaysOfNoFlow = ((memory[201] & 0xe0) >> 5);
                return port1DaysOfNoFlow;
            }
        }

        // addr 202 bits 0..1
        // port 1 reverse flow min value
        public int Port1ReverseFlowMinValue
        {
            get
            {
                int port1ReverseFlowMinValue = ((memory[202] & 0x03) >> 0);
                return port1ReverseFlowMinValue;
            }
        }

        // addr 202 bits 2..4
        // port 2 days of leak min value
        public int Port2DaysOfLeakMinValue
        {
            get
            {
                int port2DaysOfLeakMinValue = ((memory[202] & 0x1c) >> 2);
                return port2DaysOfLeakMinValue;
            }
        }

        // addr 202 bits 5..7
        // port 2 days of no flow
        public int Port2DaysOfNoFlow
        {
            get
            {
                int port2DaysOfNoFlow = ((memory[202] & 0xe0) >> 5);
                return port2DaysOfNoFlow;
            }
        }

        // addr 203 bits 0..1
        // port 2 leak detection current min value
        public int Port2LeakDetectionCurrentMinValue
        {
            get
            {
                int port2LeakDetectionCurrentMinValue = ((memory[203] & 0x03) >> 0);
                return port2LeakDetectionCurrentMinValue;
            }
        }

        // addr 203 bits 2..3
        // port 2 reverse flow min value
        public int Port2ReverseFlowMinValue
        {
            get
            {
                int port2ReverseFlowMinValue = ((memory[203] & 0x0C) >> 2);
                return port2ReverseFlowMinValue;
            }
        }

        // addr 204
        // AFC offset
        public int AfcOffset
        {
            get
            {
                int afcOffset = memory[204];
                return afcOffset;
            }
        }

        // addr 205
        // Unused

        // addr 206-207
        // MAX_FLOW
        public int MaxFlow
        {
            get
            {
                int maxFlow = (memory[206] + (memory[207] << 8));
                return maxFlow;
            }
        }

        // addr 208-231
        // reserved for alias

        // addr 232-239
        // production test

        // addr 240-242
        // Unused


        public string PcbNumber
        {
            get
            {
                return "0";
            }
        }

        // addr 243
        // MTU hardware version
        public int MtuHardwareVersion
        {
            get
            {
                int mtuHardwareVersion = memory[243];
                return mtuHardwareVersion;
            }
        }

        // addr 244
        // MTU software version
        public int MtuFirmwareVersionFormatFlag
        {
            get
            {
                int mtuSoftwareVersion = memory[244];
                return mtuSoftwareVersion;
            }
        }

        public string MtuFirmwareVersion
        {
            get
            {
                int mtuFirmwareVersion = memory[244];
                string mtuSoftware = string.Format("Version {0:00}",
                    mtuFirmwareVersion);
                return mtuSoftware;
            }
        }

        // addr 245
        // MTU revision year
        public int MtuRevisionYear
        {
            get
            {
                int mtuRevisionYear = memory[245];
                return mtuRevisionYear;
            }
        }

        // addr 246
        // MTU revision month
        public int MtuRevisionMonth
        {
            get
            {
                int mtuRevisionMonth = memory[246];
                return mtuRevisionMonth;
            }
        }

        // addr 247
        // MTU revision day
        public int MtuRevisionDay
        {
            get
            {
                int mtuRevisionDay = memory[247];
                return mtuRevisionDay;
            }
        }

        // addr 248-255
        // DCO cal values


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
        */
    }
}
