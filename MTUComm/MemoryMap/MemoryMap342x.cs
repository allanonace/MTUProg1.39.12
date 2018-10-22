using System;

namespace MTUComm.MemoryMap
{
    public class MemoryMap342x: MemoryMap
    {
        private const string FAMILY = "342xx";

        public MemoryMap342x(byte[] memory) : base ( memory, FAMILY )
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
        // time sync hour
        public int TimeSyncHour
        {
            get
            {
                int timeSyncHour = memory[1];
                return timeSyncHour;
            }
        }

        // addr 2
        // time sync minute
        public int TimeSyncMinute
        {
            get
            {
                int timeSyncMinute = memory[2];
                return timeSyncMinute;
            }
        }

        // addr 3
        // time sync second
        public int TimeSyncSecond
        {
            get
            {
                int timeSyncSecond = memory[3];
                return timeSyncSecond;
            }
        }

        // addr 4
        // time sync request timeout
        public int TimeSyncRequestTimeout
        {
            get
            {
                int timeSyncRequestTimeout = memory[4];
                return timeSyncRequestTimeout;
            }
        }

        // addr 5
        // reserved for temperature correction on gas mtus

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

        // addr 22 bit 4
        // system flags - Factory Test Mode
        public bool FactoryTestMode
        {
            get
            {
                byte systemFlags = memory[22];
                bool factoryTestMode = (((systemFlags >> 4) & 1) == 1);
                return factoryTestMode;
            }
        }

        // addr 23
        // number of wakeup/alarm messages to send out
        public int WakeupAlarmMessageNumber
        {
            get
            {
                int wakeupAlarmMessageNumber = memory[23];
                return wakeupAlarmMessageNumber;
            }
        }

        // addr 24
        // number of tx between rx
        public int NumberTxBetweenRx
        {
            get
            {
                int numberTxBetweenRx = memory[24];
                return numberTxBetweenRx;
            }
        }

        // addr 25
        // number of readings before transmit
        public int MessageOverlapCount
        {
            get
            {
                int readingsBeforeTransmit = memory[25];
                return readingsBeforeTransmit;
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

        // addr 28-29
        // P1 meter type
        public int P1MeterType
        {
            get
            {
                int p1MeterType = (memory[28] + (memory[29] << 8));
                return p1MeterType;
            }
        }

        // addr 30
        // encoder type
        public int P1EncoderType
        {
            get
            {
                int encoderType = memory[30];
                return encoderType;
            }
        }

        // addr 30-31
        // P1 pulse ratio
        public int P1PulseRatio
        {
            get
            {
                int p1PulseRatio = (memory[30] + (memory[31] << 8));
                return p1PulseRatio;
            }
        }

        // addr 32
        // P1 pulse high time
        public int P1PulseHighTime
        {
            get
            {
                int p1PulseHighTime = memory[32];
                return p1PulseHighTime;
            }
        }

        // addr 33
        // P1 pulse low time
        public int P1PulseLowTime
        {
            get
            {
                int p1PulseLowTime = memory[33];
                return p1PulseLowTime;
            }
        }

        // addr 34 bit 2
        // P1 mode 1 - immediate alarm
        public bool P1ImmediateAlarm
        {
            get
            {
                byte p1Mode = memory[34];
                bool immediateAlarm = (((p1Mode >> 2) & 1) == 1);
                return immediateAlarm;
            }
        }

        // addr 34 bit 3
        // P1 mode 1 - urgent alarm
        public bool P1UrgentAlarm
        {
            get
            {
                byte p1Mode = memory[34];
                bool urgentAlarm = (((p1Mode >> 3) & 1) == 1);
                return urgentAlarm;
            }
        }

        // addr 34 bit 4
        // P1 mode 1 - pulse type
        public bool P1PulseType
        {
            get
            {
                byte p1Mode = memory[34];
                bool pulseType = (((p1Mode >> 4) & 1) == 1);
                return pulseType;
            }
        }

        // addr 34 bit 5
        // P1 mode 1 - encoder type
        public bool P1EncoderTypeF
        {
            get
            {
                byte p1Mode = memory[34];
                bool encoderType = (((p1Mode >> 5) & 1) == 1);
                return encoderType;
            }
        }

        // addr 34 bit 6
        // P1 mode 1 - two wire pulse
        public bool P1TwoWirePulse
        {
            get
            {
                byte p1Mode = memory[34];
                bool twoWirePulse = (((p1Mode >> 6) & 1) == 1);
                return twoWirePulse;
            }
        }

        // addr 34 bit 7
        // P1 mode 1 - interrupt on tamper
        public bool P1InterruptOnTamper
        {
            get
            {
                byte p1Mode = memory[34];
                bool interruptOnTamper = (((p1Mode >> 7) & 1) == 1);
                return interruptOnTamper;
            }
        }

        // addr 35
        // P1 mode 2

        // addr 36-41
        // P1 meter ID
        public ulong P1MeterId // TODO: convert from BCD format to long
        {
            get
            {
                ulong p1MeterId = ((ulong)memory[36] +
                    ((ulong)memory[37] << 8) +
                    ((ulong)memory[38] << 16) +
                    ((ulong)memory[39] << 24) +
                    ((ulong)memory[40] << 32) +
                    ((ulong)memory[41] << 40));
                return (p1MeterId);
            }
        }

        // addr 42-43
        // Update Message Range
        public int UpdateMessageRange
        {
            get
            {
                int updateMessageRange = (memory[42] + (memory[43] << 8));
                return updateMessageRange;
            }
        }

        // addr 44 bit 0
        // port 1 enable
        public bool Port1Enable
        {
            get
            {
                byte portsEnable = memory[44];
                bool port1Enable = (((portsEnable >> 0) & 1) == 1);
                return port1Enable;
            }
        }

        // addr 44 bit 1
        // port 2 enable
        public bool Port2Enable
        {
            get
            {
                byte portsEnable = memory[44];
                bool port2Enable = (((portsEnable >> 1) & 1) == 1);
                return port2Enable;
            }
        }

        // addr 45-47
        // reserved for P2 timing control

        // addr 48-49
        // P2 meter type
        public int P2MeterType
        {
            get
            {
                int p2MeterType = (memory[48] + (memory[49] << 8));
                return p2MeterType;
            }
        }

        // addr 50
        // p2 encoder type
        public int P2EncoderType
        {
            get
            {
                int encoderType = memory[50];
                return encoderType;
            }
        }

        // addr 50-51
        // P2 pulse ratio
        public int P2PulseRatio
        {
            get
            {
                int p2PulseRatio = (memory[50] + (memory[51] << 8));
                return p2PulseRatio;
            }
        }

        // addr 52
        // p2 encoder number of digits
        public int P2EncoderNumberOfDigits
        {
            get
            {
                int p2EncoderNumberOfDigits = memory[52];
                return p2EncoderNumberOfDigits;
            }
        }

        // addr 52
        // p2 pulse high time
        public int P2PulseHighTime
        {
            get
            {
                int p2PulseHighTime = memory[52];
                return p2PulseHighTime;
            }
        }

        // addr 53
        // p2 pulse low time
        public int P2PulseLowTime
        {
            get
            {
                int p2PulseLowTime = memory[53];
                return p2PulseLowTime;
            }
        }


        // addr 54 bit 2
        // P2 mode 1 - immediate alarm
        public bool P2ImmediateAlarm
        {
            get
            {
                byte p2Mode = memory[54];
                bool immediateAlarm = (((p2Mode >> 2) & 1) == 1);
                return immediateAlarm;
            }
        }

        // addr 54 bit 3
        // P2 mode 1 - urgent alarm
        public bool P2UrgentAlarm
        {
            get
            {
                byte p2Mode = memory[54];
                bool urgentAlarm = (((p2Mode >> 3) & 1) == 1);
                return urgentAlarm;
            }
        }

        // addr 54 bit 4
        // P2 mode 1 - pulse type
        public bool P2PulseType
        {
            get
            {
                byte p2Mode = memory[54];
                bool pulseType = (((p2Mode >> 4) & 1) == 1);
                return pulseType;
            }
        }

        // addr 54 bit 5
        // P2 mode 1 - encoder type
        public bool P2EncoderTypeF
        {
            get
            {
                byte p2Mode = memory[54];
                bool encoderType = (((p2Mode >> 5) & 1) == 1);
                return encoderType;
            }
        }

        // addr 54 bit 6
        // P2 mode 1 - two wire pulse
        public bool P2TwoWirePulse
        {
            get
            {
                byte p2Mode = memory[54];
                bool twoWirePulse = (((p2Mode >> 6) & 1) == 1);
                return twoWirePulse;
            }
        }

        // addr 54 bit 7
        // P2 mode 1 - interrupt on tamper
        public bool P2InterruptOnTamper
        {
            get
            {
                byte p2Mode = memory[54];
                bool interruptOnTamper = (((p2Mode >> 7) & 1) == 1);
                return interruptOnTamper;
            }
        }

        // addr 55
        // reserved for p2 mode 2

        // addr 56-61
        // P1 meter ID
        public ulong P2MeterId // TODO: convert from BCD format to long
        {
            get
            {
                ulong p2MeterId = ((ulong)memory[56] +
                    ((ulong)memory[57] << 8) +
                    ((ulong)memory[58] << 16) +
                    ((ulong)memory[59] << 24) +
                    ((ulong)memory[60] << 32) +
                    ((ulong)memory[61] << 40));
                return BcdToLong(p2MeterId);
            }
        }

        // addr 62
        // Unused

        // addr 63
        // Reserved for P2 Status 1

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
        // Task flags - tx coil message
        public bool TaskFlagsTxCoilMessage
        {
            get
            {
                byte taskFlags = memory[64];
                bool txCoilMessage = (((taskFlags >> 2) & 1) == 1);
                return txCoilMessage;
            }
        }

        // addr 64 bit 3
        // Task flags - rf rx message
        public bool TaskFlagsRfRxMessage
        {
            get
            {
                byte taskFlags = memory[64];
                bool rfRxMessage = (((taskFlags >> 3) & 1) == 1);
                return rfRxMessage;
            }
        }

        // addr 64 bit 4
        // Task flags - coil rx message
        public bool TaskFlagsCoilRxMessage
        {
            get
            {
                byte taskFlags = memory[64];
                bool coilRxMessage = (((taskFlags >> 4) & 1) == 1);
                return coilRxMessage;
            }
        }

        // addr 64 bit 5
        // Task flags - run rf rx state machine
        public bool TaskFlagsRunRfRx
        {
            get
            {
                byte taskFlags = memory[64];
                bool runRfRx = (((taskFlags >> 5) & 1) == 1);
                return runRfRx;
            }
        }

        // addr 64 bit 6
        // Task flags - handle 1 second rtc update
        public bool TaskFlagsHandle1SecondRtcUpdate
        {
            get
            {
                byte taskFlags = memory[64];
                bool handle1SecondRtcUpdate = (((taskFlags >> 6) & 1) == 1);
                return handle1SecondRtcUpdate;
            }
        }

        // addr 64 bit 7
        // Task flags - rf rx mode
        public bool TaskFlagsRfRxMode
        {
            get
            {
                byte taskFlags = memory[64];
                bool rfRxMode = (((taskFlags >> 7) & 1) == 1);
                return rfRxMode;
            }
        }

        // addr 65 bit 0
        // Task flags - installF
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
        // Task flags - Compute CRC
        public bool TaskFlagsComputeCrc
        {
            get
            {
                byte taskFlags = memory[65];
                bool computeCrc = (((taskFlags >> 3) & 1) == 1);
                return computeCrc;
            }
        }

        // addr 65 bit 4
        // Task flags - RF AFC change
        public bool TaskFlagsRfAfcChange
        {
            get
            {
                byte taskFlags = memory[65];
                bool rfAfcChange = (((taskFlags >> 4) & 1) == 1);
                return rfAfcChange;
            }
        }

        // addr 65 bit 5
        // Task flags - AFC Calibration
        public bool TaskFlagsAfcCalibration
        {
            get
            {
                byte taskFlags = memory[65];
                bool afcCalibration = (((taskFlags >> 5) & 1) == 1);
                return afcCalibration;
            }
        }

        // addr 65 bit 6
        // Task flags - encoder autodetect flag
        public bool TaskFlagsEncoderAutodetectF
        {
            get
            {
                byte taskFlags = memory[65];
                bool encoderAutodetectF = (((taskFlags >> 6) & 1) == 1);
                return encoderAutodetectF;
            }
        }

        // addr 65 bit 7
        // Task flags - encoder port2 autodetect flag
        public bool TaskFlagsEncoderPort2AutodetectF
        {
            get
            {
                byte taskFlags = memory[65];
                bool encoderPort2AutodetectF = (((taskFlags >> 7) & 1) == 1);
                return encoderPort2AutodetectF;
            }
        }

        // addr 66-69
        // Unused

        // addr 70 bit 1
        // status flags - rtc_not_synced_with_DCU_f
        public bool StatusRtcNotSyncedWithDcuF
        {
            get
            {
                byte statusFlags = memory[70];
                bool rtcNotSyncedWithDcuF = (((statusFlags >> 1) & 1) == 1);
                return rtcNotSyncedWithDcuF;
            }
        }

        // addr 70 bit 5
        // status flags - purge data
        public bool StatusPurgeData
        {
            get
            {
                byte statusFlags = memory[70];
                bool purgeData = (((statusFlags >> 5) & 1) == 1);
                return purgeData;
            }
        }

        // addr 70 bit 7
        // status flags - time sync received
        public bool StatusTimeSyncReceived
        {
            get
            {
                byte statusFlags = memory[70];
                bool timeSyncReceived = (((statusFlags >> 7) & 1) == 1);
                return timeSyncReceived;
            }
        }

        // addr 71 bit 2
        // status flags - Fast Message Request
        public bool StatusFastMessageRequest
        {
            get
            {
                byte statusFlags = memory[71];
                bool fastMessageRequest = (((statusFlags >> 2) & 1) == 1);
                return fastMessageRequest;
            }
        }

        // addr 72-73
        // Unused

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

        // addr 80-87
        // P1 reading
        public uint P1Reading
        {
            get
            {
                uint p1Reading = (uint)memory[80] +
                    ((uint)memory[81] << 8) +
                    ((uint)memory[82] << 16) +
                    ((uint)memory[83] << 24) +
                    ((uint)memory[84] << 32) +
                    ((uint)memory[85] << 40) +
                    ((uint)memory[86] << 48) +
                    ((uint)memory[87] << 56);
                return p1Reading;
            }
        }

        // addr 86-87
        // P1 scaler
        public int P1Scaler
        {
            get
            {
                int p1Scaler = (memory[86] + (memory[87] << 8));
                return p1Scaler;
            }
        }

        // addr 88-95
        // P2 reading
        public int P2Reading
        {
            get
            {
                int p2Reading = memory[88] +
                    (memory[89] << 8) +
                    (memory[90] << 16) +
                    (memory[91] << 24) +
                    (memory[92] << 32) +
                    (memory[93] << 40) +
                    (memory[94] << 48) +
                    (memory[95] << 56);
                return p2Reading;
            }
        }

        // addr 94-95
        // P2 scaler
        public int P2Scaler
        {
            get
            {
                int p2Scaler = (memory[94] + (memory[95] << 8));
                return p2Scaler;
            }
        }

        // addr 96
        // Unused
        
        // addr 97
        // time since last time sync
        public int TimeSinceLastTimeSync
        {
            get
            {
                int timeSinceLastTimeSync = memory[97];
                return timeSinceLastTimeSync;
            }
        }

        // addr 98-101
        // Time Delta
        public int TimeDelta
        {
            get
            {
                int timeDelta = memory[98] +
                    (memory[99] << 8) +
                    (memory[100] << 16) +
                    (memory[101] << 24);
                return timeDelta;
            }
        }

        // addr 102-103
        // Unused

        // addr 104 bit 0
        // port 1 leak current alarm flag
        public bool Port1LeakCurrentAlarmF
        {
            get
            {
                bool port1LeakCurrentAlarmF = (((memory[104] >> 0) & 1) == 1);
                return port1LeakCurrentAlarmF;
            }
        }

        // addr 104 bit 1
        // port 2 leak current alarm flag
        public bool Port2LeakCurrentAlarmF
        {
            get
            {
                bool port2LeakCurrentAlarmF = (((memory[104] >> 1) & 1) == 1);
                return port2LeakCurrentAlarmF;
            }
        }

        // addr 104 bit 2
        // port 1 backflow alarm flag
        public bool Port1BackflowAlarmF
        {
            get
            {
                bool port1BackflowAlarmF = (((memory[104] >> 2) & 1) == 1);
                return port1BackflowAlarmF;
            }
        }

        // addr 104 bit 3
        // port 2 backflow alarm flag
        public bool Port2BackflowAlarmF
        {
            get
            {
                bool port2BackflowAlarmF = (((memory[104] >> 3) & 1) == 1);
                return port2BackflowAlarmF;
            }
        }

        // addr 104 bit 4
        // port 1 number days of leak alarm flag
        public bool Port1NumberDaysLeakAlarmF
        {
            get
            {
                bool port1NumberDaysLeakAlarmF = (((memory[104] >> 4) & 1) == 1);
                return port1NumberDaysLeakAlarmF;
            }
        }

        // addr 104 bit 5
        // port 2 number days of leak alarm flag
        public bool Port2NumberDaysLeakAlarmF
        {
            get
            {
                bool port2NumberDaysLeakAlarmF = (((memory[104] >> 5) & 1) == 1);
                return port2NumberDaysLeakAlarmF;
            }
        }

        // addr 104 bit 6
        // port 1 number days with no flow alarm flag
        public bool Port1NumberDaysNoFlowAlarmF
        {
            get
            {
                bool port1NumberDaysNoFlowAlarmF = (((memory[104] >> 6) & 1) == 1);
                return port1NumberDaysNoFlowAlarmF;
            }
        }

        // addr 104 bit 7
        // port 2 number days with no flow alarm flag
        public bool Port2NumberDaysNoFlowAlarmF
        {
            get
            {
                bool port2NumberDaysNoFlowAlarmF = (((memory[104] >> 7) & 1) == 1);
                return port2NumberDaysNoFlowAlarmF;
            }
        }

        // addr 105 bit 4
        // wire break port 2
        public bool WireBreakPort2
        {
            get
            {
                bool wireBreakPort2 = (((memory[105] >> 4) & 1) == 1);
                return wireBreakPort2;
            }
        }

        // addr 105 bit 5
        // wire break port
        public bool WireBreak
        {
            get
            {
                bool wireBreak = (((memory[105] >> 5) & 1) == 1);
                return wireBreak;
            }
        }

        // addr 105 bit 7
        // crystal fault
        public bool CrystalFault
        {
            get
            {
                bool crystalFault = (((memory[105] >> 7) & 1) == 1);
                return crystalFault;
            }
        }

        // addr 106
        // P1 Encoder errorcode
        public int P1EncoderErrorcode
        {
            get
            {
                int p1EncoderErrorcode = memory[106];
                return p1EncoderErrorcode;
            }
        }

        // addr 107
        // P2 Encoder errorcode
        public int P2EncoderErrorcode
        {
            get
            {
                int p2EncoderErrorcode = memory[107];
                return p2EncoderErrorcode;
            }
        }

        // addr 108 bit 0
        // tamper memory state - insufficient memory
        public bool TampersInsufficientMemory
        {
            get
            {
                byte tampers = memory[108];
                bool insufficientMemory = (((tampers >> 0) & 1) == 1);
                return insufficientMemory;
            }
        }

        // addr 108 bit 5
        // tamper memory state - cut alarm cable
        public bool TampersCutAlarmCable
        {
            get
            {
                byte tampers = memory[108];
                bool cutAlarmCable = (((tampers >> 5) & 1) == 1);
                return cutAlarmCable;
            }
        }

        // addr 108 bit 6
        // tamper memory state - serial com problem
        public bool TampersSerialComProblem
        {
            get
            {
                byte tampers = memory[108];
                bool serialComProblem = (((tampers >> 6) & 1) == 1);
                return serialComProblem;
            }
        }

        // addr 108 bit 7
        // tamper memory state - mtu last gasp
        public bool TampersMtuLastGasp
        {
            get
            {
                byte tampers = memory[108];
                bool mtuLastGasp = (((tampers >> 7) & 1) == 1);
                return mtuLastGasp;
            }
        }

        // addr 109 bit 0
        // tamper memory state - tilt tamper
        public bool TampersTiltTamper
        {
            get
            {
                byte tampers = memory[109];
                bool tiltTamper = (((tampers >> 0) & 1) == 1);
                return tiltTamper;
            }
        }

        // addr 109 bit 1
        // tamper memory state - magnetic tamper
        public bool TampersMagneticTamper
        {
            get
            {
                byte tampers = memory[109];
                bool magneticTamper = (((tampers >> 1) & 1) == 1);
                return magneticTamper;
            }
        }

        // addr 109 bit 3
        // tamper memory state - cut wire port 2
        public bool TampersCutWirePort2
        {
            get
            {
                byte tampers = memory[109];
                bool cutWirePort2 = (((tampers >> 3) & 1) == 1);
                return cutWirePort2;
            }
        }

        // addr 109 bit 4
        // tamper memory state - programming coil interface tamper
        public bool TampersProgrammingCoilInterfaceTamper
        {
            get
            {
                byte tampers = memory[109];
                bool programmingCoilInterfaceTamper = (((tampers >> 4) & 1) == 1);
                return programmingCoilInterfaceTamper;
            }
        }

        // addr 109 bit 5
        // tamper memory state - register cover tamper
        public bool TampersRegisterCoverTamper
        {
            get
            {
                byte tampers = memory[109];
                bool registerCoverTamper = (((tampers >> 5) & 1) == 1);
                return registerCoverTamper;
            }
        }

        // addr 109 bit 6
        // tamper memory state - reverse flow tamper
        public bool TampersReverseFlowTamper
        {
            get
            {
                byte tampers = memory[109];
                bool reverseFlowTamper = (((tampers >> 6) & 1) == 1);
                return reverseFlowTamper;
            }
        }

        // addr 109 bit 7
        // tamper memory state - cut wire port 2
        public bool TampersCutWirePort1
        {
            get
            {
                byte tampers = memory[109];
                bool cutWirePort1 = (((tampers >> 7) & 1) == 1);
                return cutWirePort1;
            }
        }

        // addr 110
        // battery voltage
        public int BatteryVoltage
        {
            get
            {
                int batteryVoltage = memory[110];
                return batteryVoltage;
            }
        }

        // addr 111
        // battery temperature
        public int BatteryTemperature
        {
            get
            {
                int batteryTemperature = memory[111];
                return batteryTemperature;
            }
        }

        // addr 112
        // minimum battery voltage
        public int MinBatteryVoltage
        {
            get
            {
                int minBatteryVoltage = memory[112];
                return minBatteryVoltage;
            }
        }

        // addr 113
        // DFW state
        public int DfwState
        {
            get
            {
                int dfwState = memory[113];
                return dfwState;
            }
        }

        // addr 114-115
        // CRC program flash
        public int CrcProgramFlash
        {
            get
            {
                int crcProgramFlash = (memory[114] + (memory[115] << 8));
                return crcProgramFlash;
            }
        }

        // addr 116
        // AFC Coarse Frequency correction
        public int AfcCoarseFrequencyCorrectionMirror
        {
            get
            {
                int afcCoarseFrequencyCorrection = memory[116];
                return afcCoarseFrequencyCorrection;
            }
        }

        // addr 117
        // AFC Fine Frequency correction
        public int AfcFineFrequencyCorrection
        {
            get
            {
                int afcFineFrequencyCorrection = memory[117];
                return afcFineFrequencyCorrection;
            }
        }

        // addr 118
        // AFC Status Last Received Time Sync
        public int AfcStatusLastReceivedTimeSync
        {
            get
            {
                int afcStatusLastReceivedTimeSync = memory[118];
                return afcStatusLastReceivedTimeSync;
            }
        }

        // addr 119
        // AFC Num Freq Error Readings Saved
        public int AfcNumFreqErrorReadingsSaved
        {
            get
            {
                int afcNumFreqErrorReadingsSaved = memory[119];
                return afcNumFreqErrorReadingsSaved;
            }
        }

        // addr 120-121
        // AFC Last Freq Error Avg
        public int AfcLastFreqErrorAvg
        {
            get
            {
                int afcLastFreqErrorAvg = (memory[120] + (memory[121] << 8));
                return afcLastFreqErrorAvg;
            }
        }

        // addr 122
        // RSSI of last received packet
        public int RssiLastReceivedPacket
        {
            get
            {
                int rssiLastReceivedPacket = memory[122];
                return rssiLastReceivedPacket;
            }
        }

        // addr 123
        // Deviation of last received packet
        public int DeviationLastReceivedPacket
        {
            get
            {
                int deviationLastReceivedPacket = memory[123];
                return deviationLastReceivedPacket;
            }
        }

        // addr 124-127
        // MTU/DCU ID of last packet received
        public int MtuDcuIdLastPacket
        {
            get
            {
                int mtuDcuIdLastPacket = (memory[124] + (memory[125] << 8) + (memory[126] << 16) + (memory[127] << 24));
                return mtuDcuIdLastPacket;
            }
        }

        // addr 128-129
        // tx/rx counter
        public int TxRxCounter
        {
            get
            {
                int txRxCounter = (memory[128] + (memory[129] << 8));
                return txRxCounter;
            }
        }

        // addr 130
        // reset count
        public int ResetCount
        {
            get
            {
                int resetCount = memory[130];
                return resetCount;
            }
        }

        // addr 131
        // POR count
        public int PorCount
        {
            get
            {
                int porCount = memory[131];
                return porCount;
            }
        }

        // addr 132
        // watchdog state from last reset
        public int WatchdogState
        {
            get
            {
                int watchdogState = memory[132];
                return watchdogState;
            }
        }

        // addr 133
        // reason for last reset
        public int ReasonLastReset
        {
            get
            {
                int reasonLastReset = memory[133];
                return reasonLastReset;
            }
        }

        // addr 134-137
        // date/time of last reset
        public int DateTimeLastReset
        {
            get
            {
                int dateTimeLastReset = (memory[134] + (memory[135] << 8) + (memory[136] << 16) + (memory[137] << 24));
                return dateTimeLastReset;
            }
        }

        // addr 138-141
        // date/time of voltage and temperature readings
        public int DateTimeVoltageTemperatureReadings
        {
            get
            {
                int dateTimeVoltageTemperatureReadings = (memory[138] + (memory[139] << 8) + (memory[140] << 16) + (memory[141] << 24));
                return dateTimeVoltageTemperatureReadings;
            }
        }

        // addr 142-143
        // Unused

        // addr 144-145
        // Number of On-Demand Requests
        public int NumberOnDemandRequests
        {
            get
            {
                int numberOnDemandRequests = (memory[144] + (memory[145] << 8));
                return numberOnDemandRequests;
            }
        }

        // addr 146-147
        // Number of Data Requests
        public int NumberDataRequests
        {
            get
            {
                int numberDataRequests = (memory[146] + (memory[147] << 8));
                return numberDataRequests;
            }
        }

        // addr 148
        // Number of OTA FW Updates
        public int NumberOtaFwUpdates
        {
            get
            {
                int numberOtaFwUpdates = memory[148];
                return numberOtaFwUpdates;
            }
        }

        // addr 149
        // Number of FOTC FW Updates
        public int NumberFotcFwUpdates
        {
            get
            {
                int numberFotcFwUpdates = memory[149];
                return numberFotcFwUpdates;
            }
        }

        // addr 150
        // Number of Low Priority Data transmissions
        public int NumberLowPriorityDataTransmissions
        {
            get
            {
                int numberLowPriorityDataTransmissions = memory[150];
                return numberLowPriorityDataTransmissions;
            }
        }

        // addr 151
        // Last Gasp Deadband
        public int LastGaspDeadband
        {
            get
            {
                int lastGaspDeadband = memory[151];
                return lastGaspDeadband;
            }
        }

        // addr 152
        // Last Gasp Level
        public int LastGaspLevel
        {
            get
            {
                int lastGaspLevel = memory[152];
                return lastGaspLevel;
            }
        }

        // addr 153
        // Battery reading quiet time
        public int BatteryReadingQuietTime
        {
            get
            {
                int batteryReadingQuietTime = memory[153];
                return batteryReadingQuietTime;
            }
        }

        // addr 154
        // Battery reading hold off
        public int BatteryReadingHoldOff
        {
            get
            {
                int batteryReadingHoldOff = memory[154];
                return batteryReadingHoldOff;
            }
        }

        // addr 155
        // STAR Inter-Packet TX Delay
        public int StarInterPacketTxDelay
        {
            get
            {
                int startInterPacketTxDelay = memory[155];
                return startInterPacketTxDelay;
            }
        }

        // addr 156-157
        // Message Retry Control
        public int MessageRetryControl
        {
            get
            {
                int messageRetryControl = (memory[156] + (memory[157] << 8));
                return messageRetryControl;
            }
        }

        // addr 158-159
        // Response Timing
        public int ResponseTiming
        {
            get
            {
                int responseTiming = (memory[158] + (memory[159] << 8));
                return responseTiming;
            }
        }

        // addr 160
        // Secondary Response Start
        public int SecondaryResponseStart
        {
            get
            {
                int secondaryResponseStart = memory[160];
                return secondaryResponseStart;
            }
        }

        // addr 161
        // Unused

        // addr 162
        // Primary Message Rx Window
        public int PrimaryMessageRxWindow
        {
            get
            {
                int primaryMessageRxWindow = memory[162];
                return primaryMessageRxWindow;
            }
        }

        // addr 163 bit 0
        // Fast Messaging Configuration - Fast messaging mode
        public bool FastMessagingMode
        {
            get
            {
                byte fastMessagingConfiguration = memory[163];
                bool fastMessaginMode = (((fastMessagingConfiguration >> 0) & 1) == 1);
                return fastMessaginMode;
            }
        }

        // addr 163 bit 1
        // Fast Messaging Configuration - Response frequency
        public bool ResponseFrequency
        {
            get
            {
                byte fastMessagingConfiguration = memory[163];
                bool responseFrequency = (((fastMessagingConfiguration >> 1) & 1) == 1);
                return responseFrequency;
            }
        }

        // addr 164-165
        // Primary Message Window Interval A
        public int PrimaryMessageWindowIntervalA
        {
            get
            {
                int primaryMessageWindowIntervalA = (memory[164] + (memory[165] << 8));
                return primaryMessageWindowIntervalA;
            }
        }

        // addr 166
        // Window A Start
        public int WindowAStart
        {
            get
            {
                int windowAStart = memory[166];
                return windowAStart;
            }
        }

        // addr 167
        // Window B Start
        public int WindowBStart
        {
            get
            {
                int windowBStart = memory[167];
                return windowBStart;
            }
        }

        // addr 168-169
        // Primary Message Window Interval B
        public int PrimaryMessageWindowIntervalB
        {
            get
            {
                int primaryMessageWindowIntervalB = (memory[168] + (memory[169] << 8));
                return primaryMessageWindowIntervalB;
            }
        }

        // addr 170
        // Primary Message Window Offset
        public int PrimaryMessageWindowOffset
        {
            get
            {
                int primaryMessageWindowOffset = memory[170];
                return primaryMessageWindowOffset;
            }
        }

        // addr 171
        // AFC Coarse Frequency correction
        public int AfcCoarseFrequencyCorrectionMaster
        {
            get
            {
                int afcCoarseFrequencyCorrection = memory[171];
                return afcCoarseFrequencyCorrection;
            }
        }

        // addr 172-173
        // AFC Scheduled Report Interval Hours
        public int AfcScheduledReportIntervalHours
        {
            get
            {
                int afcScheduledReportIntervalHours = (memory[172] + (memory[173] << 8));
                return afcScheduledReportIntervalHours;
            }
        }

        // addr 174
        // AFC no change cnt
        public int AfcNoChangeCnt
        {
            get
            {
                int afcNoChangeCnt = memory[174];
                return afcNoChangeCnt;
            }
        }

        // addr 175-183
        // Unused

        // addr 184-187
        // XcvrInitCount
        public int XcvrInitCount
        {
            get
            {
                int xcvrInitCount = (memory[184] + (memory[185] << 8) + (memory[186] << 16) + (memory[187] << 24));
                return xcvrInitCount;
            }
        }

        // addr 188-189
        // AFC Limiter
        public int AfcLimiter
        {
            get
            {
                int afcLimiter = (memory[188] + (memory[189] << 8));
                return afcLimiter;
            }
        }

        // addr 190
        // Unused

        // addr 191
        // Si446xPAPowerLevelSetting
        public int Si446xPaPowerLevelSetting
        {
            get
            {
                int si446xPaPowerLevelSetting = memory[191];
                return si446xPaPowerLevelSetting;
            }
        }

        // addr 192 bit 0
        // Alarm messages immediate 2 - Insufficient memory
        public bool InsufficientMemory
        {
            get
            {
                byte alarms = memory[192];
                bool insufficientMemory = (((alarms >> 0) & 1) == 1);
                return insufficientMemory;
            }
        }

        // addr 192 bit 5
        // Alarm messages immediate 2 - Cut alarm cable
        public bool CutAlarmCable
        {
            get
            {
                byte alarms = memory[192];
                bool cutAlarmCable = (((alarms >> 5) & 1) == 1);
                return cutAlarmCable;
            }
        }

        // addr 192 bit 6
        // Alarm messages immediate 2 - serial com problem
        public bool SerialComProblem
        {
            get
            {
                byte alarms = memory[192];
                bool serialComProblem = (((alarms >> 6) & 1) == 1);
                return serialComProblem;
            }
        }

        // addr 192 bit 7
        // Alarm messages immediate 2 - mtu last gasp
        public bool MtuLastGasp
        {
            get
            {
                byte alarms = memory[192];
                bool mtuLastGasp = (((alarms >> 7) & 1) == 1);
                return mtuLastGasp;
            }
        }

        // addr 193 bit 0
        // Alarm messages immediate - tilt tamper
        public bool TiltTamper
        {
            get
            {
                byte alarms = memory[193];
                bool tiltTamper = (((alarms >> 0) & 1) == 1);
                return tiltTamper;
            }
        }

        // addr 193 bit 1
        // Alarm messages immediate - magnetic tamper
        public bool MagneticTamper
        {
            get
            {
                byte alarms = memory[193];
                bool magneticTamper = (((alarms >> 1) & 1) == 1);
                return magneticTamper;
            }
        }

        // addr 193 bit 3
        // Alarm messages immediate - Port 2 alarm
        public bool CutWirePort2
        {
            get
            {
                byte alarms = memory[193];
                bool cutWirePort2 = (((alarms >> 3) & 1) == 1);
                return cutWirePort2;
            }
        }

        // addr 193 bit 4
        // Alarm messages immediate - programming coil interface tamper
        public bool ProgrammingCoilInterfaceTamper
        {
            get
            {
                byte alarms = memory[193];
                bool programmingCoilInterfaceTamper = (((alarms >> 4) & 1) == 1);
                return programmingCoilInterfaceTamper;
            }
        }

        // addr 193 bit 5
        // Alarm messages immediate - Register cover tamper
        public bool RegisterCoverTamper
        {
            get
            {
                byte alarms = memory[193];
                bool registerCoverTamper = (((alarms >> 5) & 1) == 1);
                return registerCoverTamper;
            }
        }

        // addr 193 bit 6
        // Alarm messages immediate - reverse flow tamper
        public bool ReverseFlowTamper
        {
            get
            {
                byte alarms = memory[193];
                bool reverseFlowTamper = (((alarms >> 6) & 1) == 1);
                return reverseFlowTamper;
            }
        }

        // addr 193 bit 7
        // Alarm messages immediate - cut wire port 1
        public bool CutWirePort1
        {
            get
            {
                byte alarms = memory[193];
                bool cutWirePort1 = (((alarms >> 7) & 1) == 1);
                return cutWirePort1;
            }
        }

        // addr 194 bit 0
        // Alarm messages transmission 2 - insufficient memory
        public bool InsufficientMemoryTx
        {
            get
            {
                byte alarms = memory[194];
                bool insufficientMemoryTx = (((alarms >> 0) & 1) == 1);
                return insufficientMemoryTx;
            }
        }

        // addr 194 bit 5
        // Alarm messages transmission 2 - cut alarm cable
        public bool CutAlarmCableTx
        {
            get
            {
                byte alarms = memory[194];
                bool cutAlarmCableTx = (((alarms >> 5) & 1) == 1);
                return cutAlarmCableTx;
            }
        }

        // addr 194 bit 6
        // Alarm messages transmission 2 - serial com problem
        public bool SerialComProblemTx
        {
            get
            {
                byte alarms = memory[194];
                bool serialComProblemTx = (((alarms >> 6) & 1) == 1);
                return serialComProblemTx;
            }
        }

        // addr 194 bit 7
        // Alarm messages transmission 2 - mtu last gasp
        public bool MtuLastGaspTx
        {
            get
            {
                byte alarms = memory[194];
                bool mtuLastGaspTx = (((alarms >> 7) & 1) == 1);
                return mtuLastGaspTx;
            }
        }

        // addr 195 bit 0
        // Alarm messages transmission - tilt tamper
        public bool TiltTamperTx
        {
            get
            {
                byte alarms = memory[195];
                bool tiltTamperTx = (((alarms >> 0) & 1) == 1);
                return tiltTamperTx;
            }
        }

        // addr 195 bit 1
        // Alarm messages transmission - magnetic tamper
        public bool MagneticTamperTx
        {
            get
            {
                byte alarms = memory[195];
                bool magneticTamperTx = (((alarms >> 1) & 1) == 1);
                return magneticTamperTx;
            }
        }

        // addr 195 bit 3
        // Alarm messages transmission - cut wire port 2
        public bool CutWirePort2Tx
        {
            get
            {
                byte alarms = memory[195];
                bool cutWirePort2Tx = (((alarms >> 3) & 1) == 1);
                return cutWirePort2Tx;
            }
        }

        // addr 195 bit 4
        // Alarm messages transmission - programming coil interface tamper
        public bool ProgrammingCoilInterfaceTamperTx
        {
            get
            {
                byte alarms = memory[195];
                bool programmingCoilInterfaceTamperTx = (((alarms >> 4) & 1) == 1);
                return programmingCoilInterfaceTamperTx;
            }
        }

        // addr 195 bit 5
        // Alarm messages transmission - register cover tamper
        public bool RegisterCoverTamperTx
        {
            get
            {
                byte alarms = memory[195];
                bool registerCoverTamperTx = (((alarms >> 5) & 1) == 1);
                return registerCoverTamperTx;
            }
        }

        // addr 195 bit 6
        // Alarm messages transmission - reverse flow tamper
        public bool ReverseFlowTamperTx
        {
            get
            {
                byte alarms = memory[195];
                bool reverseFlowTamperTx = (((alarms >> 6) & 1) == 1);
                return reverseFlowTamperTx;
            }
        }

        // addr 195 bit 7
        // Alarm messages transmission - cut wire port 1
        public bool CutWirePort1Tx
        {
            get
            {
                byte alarms = memory[195];
                bool cutWirePort1Tx = (((alarms >> 7) & 1) == 1);
                return cutWirePort1Tx;
            }
        }

        // addr 196
        // Health Message Period
        public int HealthMessagePeriod
        {
            get
            {
                int healthMessagePeriod = memory[196];
                return healthMessagePeriod;
            }
        }

        // addr 197
        // Time difference alarm setting in seconds
        public int TimeDifferenceAlarm
        {
            get
            {
                int timeDifferenceAlarm = memory[197];
                return timeDifferenceAlarm;
            }
        }

        // addr 198
        // Daily read hour
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
        // Cut wire alarm hysteresis setting
        public int CutWireAlarmHysteresisSetting
        {
            get
            {
                int cutWireAlarmHysteresisSetting = memory[199];
                return cutWireAlarmHysteresisSetting;
            }
        }

        // addr 200 bit 1
        // Leak Detection Current Alarm Enable
        public bool LeakDetectionCurrentAlarm
        {
            get
            {
                byte ecoderAlarms = memory[200];
                bool leakDetectionCurrentAlarmEnable = (((ecoderAlarms >> 1) & 1) == 1);
                return leakDetectionCurrentAlarmEnable;
            }
        }

        // addr 200 bit 2
        // Number Days of Leak Alarm Enable
        public bool NumberDaysLeakAlarmEnable
        {
            get
            {
                byte ecoderAlarms = memory[200];
                bool numberDaysLeakAlarmEnable = (((ecoderAlarms >> 2) & 1) == 1);
                return numberDaysLeakAlarmEnable;
            }
        }

        // addr 200 bit 3
        // Number Days No Flow Alarm Enable
        public bool NumberDaysNoFlowAlarmEnable
        {
            get
            {
                byte ecoderAlarms = memory[200];
                bool numberDaysNoFlowAlarmEnable = (((ecoderAlarms >> 3) & 1) == 1);
                return numberDaysNoFlowAlarmEnable;
            }
        }

        // addr 200 bit 4
        // Reverse Flow Alarm Enable
        public bool ReverseFlowAlarmEnable
        {
            get
            {
                byte ecoderAlarms = memory[200];
                bool reverseFlowAlarmEnable = (((ecoderAlarms >> 4) & 1) == 1);
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
        // p1 encoder digits to drop
        public int P1EncoderDigitsToDrop
        {
            get
            {
                int p1EncoderDigitsToDrop = memory[204];
                return p1EncoderDigitsToDrop;
            }
        }

        // addr 205
        // p2 encoder digits
        public int P2EncoderDigitsDrop
        {
            get
            {
                int p2EncoderDigitsToDrop = memory[205];
                return p2EncoderDigitsToDrop;
            }
        }

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

        // addr 208-235
        // Reserved for Alias

        // addr 236
        // Battery Install Date Month
        public int BatteryInstallDateMonth
        {
            get
            {
                int batteryInstallDateMonth = memory[236];
                return batteryInstallDateMonth;
            }
        }

        // addr 237
        // Battery Install Date Day
        public int BatteryInstallDateDay
        {
            get
            {
                int batteryInstallDateDay = memory[237];
                return batteryInstallDateDay;
            }
        }

        // addr 238
        // Battery Install Date Year
        public int BatteryInstallDateYear
        {
            get
            {
                int batteryInstallDateYear = memory[238];
                return batteryInstallDateYear;
            }
        }

        // addr 239
        // Unused


        public string PcbNumber
        {
            get
            {
                return "0";
            }
        }

        // addr 240-241
        // MTU firmware version Build
        public int MtuFirmwareVersionBuild
        {
            get
            {
                int mtuFirmwareVersionBuild = (memory[240] + (memory[241] << 8));
                return mtuFirmwareVersionBuild;
            }
        }

        // addr 242
        // Unused

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
        // MTU firmware version format flag
        public int MtuFirmwareVersionFormatFlag
        {
            get
            {
                int mtuFirmwareVersionFormatFlag = memory[244];
                return mtuFirmwareVersionFormatFlag;
            }
        }

        // addr 245
        // MTU firmware version Major
        public int MtuFirmwareVersionMajor
        {
            get
            {
                int mtuFirmwareVersionMajor = memory[245];
                return mtuFirmwareVersionMajor;
            }
        }

        // addr 246
        // MTU firmware version Minor
        public int MtuFirmwareVersionMinor
        {
            get
            {
                int mtuFirmwareVersionMinor = memory[246];
                return mtuFirmwareVersionMinor;
            }
        }

        public string MtuFirmwareVersion
        {
            get
            {
                int mtuFirmwareVersionBuild = memory[240] + (memory[241] << 8);
                int mtuFirmwareVersionMajor = memory[245];
                int mtuFirmwareVersionMinor = memory[246];
                string mtuSoftware = string.Format("Version {0:00}.{1:00}.{2:0000}",
                    mtuFirmwareVersionMajor,
                    mtuFirmwareVersionMinor,
                    mtuFirmwareVersionBuild);
                return mtuSoftware;
            }
        }

        // addr 248-249
        // Network ID
        public int NetworkId
        {
            get
            {
                int networkId = (memory[248] + (memory[249] << 8));
                return networkId;
            }
        }

        // addr 250-255
        // Unused

        // addr 256-271
        // AES encryption key

        // addr 318 bit 0
        // Encryption enabled
        public bool EncryptionEnabled
        {
            get
            {
                byte encryptionFlags = memory[318];
                bool encryptionEnabled = (((encryptionFlags >> 0) & 1) == 1);
                return encryptionEnabled;
            }
        }

        // addr 319
        // Key index
        public int KeyIndex
        {
            get
            {
                int keyIndex = memory[319];
                return keyIndex;
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
        */
    }
}
