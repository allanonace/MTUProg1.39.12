using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUComm
{
    public class MemoryMap31xx32xx: IMemoryMap
    {
        private byte[] memory;

        public MemoryMap31xx32xx(byte[] memory)
        {
            this.memory = memory;
        }

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
            set
            {
                memory[25] = (byte)value;
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
            set
            {
                memory[26] = (byte)value;
                memory[27] = (byte)(value >> 8);
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

        // addr 32-33
        // P1 meter type
        public int P1MeterType
        {
            get
            {
                int p1MeterType = (memory[32] + (memory[33] << 8));
                return p1MeterType;
            }
            set
            {
                memory[32] = (byte)value;
                memory[33] = (byte)(value >> 8);
            }
        }

        // addr 34-39
        // P1 meter ID
        public ulong P1MeterId // TODO: convert from BCD format to long
        {
            get
            {
                ulong p1MeterId = ((ulong)memory[34] + ((ulong)memory[35] << 8) + ((ulong)memory[36] << 16) + ((ulong)memory[37] << 24) + ((ulong)memory[38] << 32) + ((ulong)memory[39] << 40));
                return BcdToLong(p1MeterId);
            }
            set
            {
                memory[34] = (byte)value;
                memory[35] = (byte)(value >> 8);
                memory[36] = (byte)(value >> 16);
                memory[37] = (byte)(value >> 24);
                memory[38] = (byte)(value >> 32);
                memory[39] = (byte)(value >> 40);
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
            set
            {
                memory[40] = (byte)value;
                memory[41] = (byte)(value >> 8);
            }
        }

        // addr 42
        // P1 mode
        public int P1Mode
        {
            get
            {
                int p1Mode = memory[42];
                return p1Mode;
            }
            set
            {
                memory[42] = (byte)value;
            }
        }

        // addr 42 bit 0
        // P1 mode - tilt alarm
        public bool P1TiltAlarm
        {
            get
            {
                byte p1Mode = memory[42];
                bool p1TiltAlarm = (((p1Mode >> 0) & 1) == 1);
                return p1TiltAlarm;
            }
        }

        // addr 42 bit 1
        // P1 mode - magnetic alarm
        public bool P1MagneticAlarm
        {
            get
            {
                byte p1Mode = memory[42];
                bool p1MagneticAlarm = (((p1Mode >> 1) & 1) == 1);
                return p1MagneticAlarm;
            }
        }

        // addr 42 bit 2
        // P1 mode - immediate alarm
        public bool P1ImmediateAlarm
        {
            get
            {
                byte p1Mode = memory[42];
                bool p1ImmediateAlarm = (((p1Mode >> 2) & 1) == 1);
                return p1ImmediateAlarm;
            }
        }

        // addr 42 bit 3
        // P1 mode - urgent alarm
        public bool P1UrgentAlarm
        {
            get
            {
                byte p1Mode = memory[42];
                bool p1UrgentAlarm = (((p1Mode >> 3) & 1) == 1);
                return p1UrgentAlarm;
            }
        }

        // addr 42 bit 4
        // P1 mode - PCI alarm
        public bool P1PciAlarm
        {
            get
            {
                byte p1Mode = memory[42];
                bool p1PciAlarm = (((p1Mode >> 4) & 1) == 1);
                return p1PciAlarm;
            }
        }

        // addr 42 bit 5
        // P1 mode - register cover alarm
        public bool P1RegisterCoverAlarm
        {
            get
            {
                byte p1Mode = memory[42];
                bool p1RegisterCoverAlarm = (((p1Mode >> 5) & 1) == 1);
                return p1RegisterCoverAlarm;
            }
        }

        // addr 42 bit 6
        // P1 mode - reverse flow alarm
        public bool P1ReverseFlowAlarm
        {
            get
            {
                byte p1Mode = memory[42];
                bool p1ReverseFlowAlarm = (((p1Mode >> 6) & 1) == 1);
                return p1ReverseFlowAlarm;
            }
        }

        // addr 42 bit 7
        // P1 mode - flow rotation
        public bool P1FlowRotation
        {
            get
            {
                byte p1Mode = memory[42];
                bool p1FlowRotation = (((p1Mode >> 7) & 1) == 1);
                return p1FlowRotation;
            }
        }

        // addr 43
        // P1 pulse high time
        public int P1PulseHighTime
        {
            get
            {
                int p1PulseHighTime = memory[43];
                return p1PulseHighTime;
            }
        }

        // addr 44
        // P1 pulse low time
        public int P1PulseLowtime
        {
            get
            {
                int p1PulseLowTime = memory[44];
                return p1PulseLowTime;
            }
        }

        // addr 45 bit 0
        // P1 mode2 - cut wire alarm
        public bool P1CutWireAlarm
        {
            get
            {
                byte p1Mode2 = memory[45];
                bool p1CutWireAlarm = (((p1Mode2 >> 0) & 1) == 1);
                return p1CutWireAlarm;
            }
        }

        // addr 45 bit 1
        // P1 mode2 - FormC (remote reed switch)
        public bool P1FormC
        {
            get
            {
                byte p1Mode2 = memory[45];
                bool p1FormC = (((p1Mode2 >> 1) & 1) == 1);
                return p1FormC;
            }
        }

        // addr 45 bit 2
        // P1 mode2 - Port 2 alarm
        public bool P1Port2Alarm
        {
            get
            {
                byte p1Mode2 = memory[45];
                bool p1Port2Alarm = (((p1Mode2 >> 2) & 1) == 1);
                return p1Port2Alarm;
            }
        }

        // addr 46-47
        // Unused

        // addr 48-49
        // P2 meter type
        public int P2MeterType
        {
            get
            {
                int p1MeterType = (memory[48] + (memory[49] << 8));
                return p1MeterType;
            }
            set
            {
                memory[48] = (byte)value;
                memory[49] = (byte)(value >> 8);
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
            set
            {
                memory[50] = (byte)value;
                memory[51] = (byte)(value >> 8);
                memory[52] = (byte)(value >> 16);
                memory[53] = (byte)(value >> 24);
                memory[54] = (byte)(value >> 32);
                memory[55] = (byte)(value >> 40);
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
            set
            {
                memory[56] = (byte)value;
                memory[57] = (byte)(value >> 8);
            }
        }

        // addr 58
        // P2 mode
        public int P2Mode
        {
            get
            {
                int p2Mode = memory[58];
                return p2Mode;
            }
            set
            {
                memory[58] = (byte)value;
            }
        }

        // addr 58 bit 0
        // P2 mode - tilt alarm
        public bool P2TiltAlarm
        {
            get
            {
                byte p2Mode = memory[58];
                bool p2TiltAlarm = (((p2Mode >> 0) & 1) == 1);
                return p2TiltAlarm;
            }
        }

        // addr 58 bit 1
        // P2 mode - magnetic alarm
        public bool P2MagneticAlarm
        {
            get
            {
                byte p2Mode = memory[58];
                bool p2MagneticAlarm = (((p2Mode >> 1) & 1) == 1);
                return p2MagneticAlarm;
            }
        }

        // addr 58 bit 2
        // P2 mode - immediate alarm
        public bool P2ImmediateAlarm
        {
            get
            {
                byte p2Mode = memory[58];
                bool p2ImmediateAlarm = (((p2Mode >> 2) & 1) == 1);
                return p2ImmediateAlarm;
            }
        }

        // addr 58 bit 3
        // P2 mode - urgent alarm
        public bool P2UrgentAlarm
        {
            get
            {
                byte p2Mode = memory[58];
                bool p2UrgentAlarm = (((p2Mode >> 3) & 1) == 1);
                return p2UrgentAlarm;
            }
        }

        // addr 58 bit 4
        // P2 mode - PCI alarm
        public bool P2PciAlarm
        {
            get
            {
                byte p2Mode = memory[58];
                bool p2PciAlarm = (((p2Mode >> 4) & 1) == 1);
                return p2PciAlarm;
            }
        }

        // addr 58 bit 5
        // P2 mode - register cover alarm
        public bool P2RegisterCoverAlarm
        {
            get
            {
                byte p2Mode = memory[58];
                bool p2RegisterCoverAlarm = (((p2Mode >> 5) & 1) == 1);
                return p2RegisterCoverAlarm;
            }
        }

        // addr 58 bit 6
        // P2 mode - reverse flow alarm
        public bool P2ReverseFlowAlarm
        {
            get
            {
                byte p2Mode = memory[58];
                bool p2ReverseFlowAlarm = (((p2Mode >> 6) & 1) == 1);
                return p2ReverseFlowAlarm;
            }
        }

        // addr 58 bit 7
        // P2 mode - flow rotation
        public bool P2FlowRotation
        {
            get
            {
                byte p2Mode = memory[58];
                bool p2FlowRotation = (((p2Mode >> 7) & 1) == 1);
                return p2FlowRotation;
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
        public int P2PulseLowtime
        {
            get
            {
                int p2PulseLowtime = memory[60];
                return p2PulseLowtime;
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
        // Task flags - rx mode
        public bool TaskFlagsRxMode
        {
            get
            {
                byte taskFlags = memory[64];
                bool rxMode = (((taskFlags >> 7) & 1) == 1);
                return rxMode;
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
        // Unused

        // addr 71 bit 0
        // tamper memory - tilt tamper
        public bool TiltTamper
        {
            get
            {
                byte tamperMemory = memory[71];
                bool tiltTamper = (((tamperMemory >> 0) & 1) == 1);
                return tiltTamper;
            }
        }

        // addr 71 bit 1
        // tamper memory - magnetic tamper
        public bool MagneticTamper
        {
            get
            {
                byte tamperMemory = memory[71];
                bool magneticTamper = (((tamperMemory >> 1) & 1) == 1);
                return magneticTamper;
            }
        }

        // addr 71 bit 2
        // tamper memory - not used

        // addr 71 bit 3
        // tamper memory - port 2 alarm
        public bool Port2Alarm
        {
            get
            {
                byte tamperMemory = memory[71];
                bool port2Alarm = (((tamperMemory >> 3) & 1) == 1);
                return port2Alarm;
            }
        }

        // addr 71 bit 4
        // tamper memory - programming coil interface tamper
        public bool ProgrammingCoilInterfaceTamper
        {
            get
            {
                byte tamperMemory = memory[71];
                bool programmingCoilInterfaceTamper = (((tamperMemory >> 4) & 1) == 1);
                return programmingCoilInterfaceTamper;
            }
        }

        // addr 71 bit 5
        // tamper memory - register cover tamper
        public bool RegisterCoverTamper
        {
            get
            {
                byte tamperMemory = memory[71];
                bool registerCoverTamper = (((tamperMemory >> 5) & 1) == 1);
                return registerCoverTamper;
            }
        }

        // addr 71 bit 6
        // tamper memory - reverse flow tamper
        public bool ReverseFlowTamper
        {
            get
            {
                byte tamperMemory = memory[71];
                bool reverseFlowTamper = (((tamperMemory >> 6) & 1) == 1);
                return reverseFlowTamper;
            }
        }

        // addr 71 bit 7
        // tamper memory - cut wire tamper
        public bool CutWireTamper
        {
            get
            {
                byte tamperMemory = memory[71];
                bool cutWireTamper = (((tamperMemory >> 7) & 1) == 1);
                return cutWireTamper;
            }
        }


        // addr 72
        // Unused

        // addr 73 bit 6
        // mtu errorcode - dco uncalibrated
        public bool MtuErrorCodeDcoUncalibrated
        {
            get
            {
                byte mtuErrorcode = memory[73];
                bool dcoUncalibrated = (((mtuErrorcode >> 6) & 1) == 1);
                return dcoUncalibrated;
            }
        }

        // addr 73 bit 7
        // mtu errorcode - crystal fault
        public bool MtuErrorCodeCrystalFault
        {
            get
            {
                byte mtuErrorcode = memory[73];
                bool crystalFault = (((mtuErrorcode >> 7) & 1) == 1);
                return crystalFault;
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

        // addr 96-101
        // p1 reading
        public uint P1Reading
        {
            get
            {
                uint p1Reading = (uint)memory[96] +
                    ((uint)memory[97] << 8) +
                    ((uint)memory[98] << 16) +
                    ((uint)memory[99] << 24) +
                    ((uint)memory[100] << 32) +
                    ((uint)memory[101] << 40);
                return p1Reading;
            }
            set
            {
                memory[96] = (byte)value;
                memory[97] = (byte)(value >> 8);
                memory[98] = (byte)(value >> 16);
                memory[99] = (byte)(value >> 24);
                memory[100] = (byte)(value >> 32);
                memory[101] = (byte)(value >> 40);
            }
        }

        public string P1ReadingError
        {
            get
            {

                return "";
            }
        }

        // addr 102-103
        // p1 scaler
        public int P1Scaler
        {
            get
            {
                int p1Scaler = (memory[102] + (memory[103] << 8));
                return p1Scaler;
            }
        }

        // addr 104-109
        // p2 reading
        public int P2Reading
        {
            get
            {
                int p2Reading = memory[104] +
                    (memory[105] << 8) +
                    (memory[106] << 16) +
                    (memory[107] << 24) +
                    (memory[108] << 32) +
                    (memory[109] << 40);
                return p2Reading;
            }
            set
            {
                memory[104] = (byte)value;
                memory[105] = (byte)(value >> 8);
                memory[106] = (byte)(value >> 16);
                memory[107] = (byte)(value >> 24);
                memory[108] = (byte)(value >> 32);
                memory[109] = (byte)(value >> 40);
            }
        }

        public string P2ReadingError
        {
            get
            {
                return "";
            }
        }

        // addr 110-111
        // p2 scaler
        public int P2Scaler
        {
            get
            {
                int p2Scaler = (memory[110] + (memory[111] << 8));
                return p2Scaler;
            }
        }

        // addr 112
        // Unused

        // addr 113
        // battery voltage (AD value) | battery volt = ADvalue * 9.766mV*2 + 250
        public int BatteryVoltage
        {
            get
            {
                int batteryVoltageAd = memory[113];

                return (int) (memory[113] * 9.766 * 2) + 250;

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
        // Status flags - tx_count_down_f
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
        // Status flags - rtc_not_synced_with_DCU_f
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
        // Status flags - p1_tx_f
        public bool StatusP1TxF
        {
            get
            {
                byte statusFlags = memory[116];
                bool p1TxF = (((statusFlags >> 2) & 1) == 1);
                return p1TxF;
            }
        }

        // addr 116 bit 3
        // Status flags - p2_tx_f
        public bool StatusP2TxF
        {
            get
            {
                byte statusFlags = memory[116];
                bool p2TxF = (((statusFlags >> 3) & 1) == 1);
                return p2TxF;
            }
        }

        // addr 116 bit 4
        // Status flags - alarm_time_range_f
        public bool StatusAlarmTimeRangeF
        {
            get
            {
                byte statusFlags = memory[116];
                bool alarmTimeRangeF = (((statusFlags >> 4) & 1) == 1);
                return alarmTimeRangeF;
            }
        }

        // addr 116 bit 5
        // Status flags - purge_data_f
        public bool StatusPurgeDataF
        {
            get
            {
                byte statusFlags = memory[116];
                bool purgeDataF = (((statusFlags >> 5) & 1) == 1);
                return purgeDataF;
            }
        }

        // addr 116 bit 6
        // Status flags - rf_afc_change_f
        public bool StatusRfAfcChangeF
        {
            get
            {
                byte statusFlags = memory[116];
                bool rfAfcChangeF = (((statusFlags >> 6) & 1) == 1);
                return rfAfcChangeF;
            }
        }

        // addr 117 bit 0
        // Status flags - time sync request trigger when tx cnt down
        public bool StatusTimeSyncRequestTrigger
        {
            get
            {
                byte statusFlags = memory[117];
                bool timeSyncRequest = (((statusFlags >> 0) & 1) == 1);
                return timeSyncRequest;
            }
        }

        // addr 117 bit 1
        // Status flags - AFC report trigger when tx cnt down
        public bool StatusAfcReportTrigger
        {
            get
            {
                byte statusFlags = memory[117];
                bool afcReport = (((statusFlags >> 1) & 1) == 1);
                return afcReport;
            }
        }

        // addr 117 bit 2
        // Status flags - schedule a tamper alarm tx
        public bool StatusScheduleTamperAlarmTx
        {
            get
            {
                byte statusFlags = memory[117];
                bool scheduleTamperAlarmTx = (((statusFlags >> 2) & 1) == 1);
                return scheduleTamperAlarmTx;
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

        // addr 122-124
        // Unused

        // addr 125
        // special port I/O read
        public int SpecialPortIORead
        {
            get
            {
                int specialPortIORead = memory[125];
                return specialPortIORead;
            }
        }

        // addr 126-127
        // Unused

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
        public int F12WAYRegister1
        {
            get
            {
                int reg1 = (memory[132] + (memory[133] << 8) + (memory[134] << 16) + (memory[135] << 24));
                return reg1;
            }
        }

        // addr 136-139
        // 7021 reg2
        public int F12WAYRegister2
        {
            get
            {
                int reg2 = (memory[136] + (memory[137] << 8) + (memory[138] << 16) + (memory[139] << 24));
                return reg2;
            }
        }

        // addr 140-143
        // 7021 reg3
        public int F12WAYRegister3
        {
            get
            {
                int reg3 = (memory[140] + (memory[141] << 8) + (memory[142] << 16) + (memory[143] << 24));
                return reg3;
            }
        }

        // addr 144-147
        // 7021 reg4
        public int F12WAYRegister4
        {
            get
            {
                int reg4 = (memory[144] + (memory[145] << 8) + (memory[146] << 16) + (memory[147] << 24));
                return reg4;
            }
        }

        // addr 148-151
        // 7021 reg5
        public int F12WAYRegister5
        {
            get
            {
                int reg5 = (memory[148] + (memory[149] << 8) + (memory[150] << 16) + (memory[151] << 24));
                return reg5;
            }
        }

        // addr 152-155
        // 7021 reg6
        public int F12WAYRegister6
        {
            get
            {
                int reg6 = (memory[152] + (memory[153] << 8) + (memory[154] << 16) + (memory[155] << 24));
                return reg6;
            }
        }

        // addr 156-159
        // 7021 reg7
        public int F12WAYRegister7
        {
            get
            {
                int reg7 = (memory[156] + (memory[157] << 8) + (memory[158] << 16) + (memory[159] << 24));
                return reg7;
            }
        }

        // addr 160-163
        // 7021 reg8
        public int F12WAYRegister8
        {
            get
            {
                int reg8 = (memory[160] + (memory[161] << 8) + (memory[162] << 16) + (memory[163] << 24));
                return reg8;
            }
        }

        // addr 164-167
        // 7021 reg9
        public int F12WAYRegister9
        {
            get
            {
                int reg9 = (memory[164] + (memory[165] << 8) + (memory[166] << 16) + (memory[167] << 24));
                return reg9;
            }
        }

        // addr 168-171
        // 7021 reg10
        public int F12WAYRegister10
        {
            get
            {
                int reg10 = (memory[168] + (memory[169] << 8) + (memory[170] << 16) + (memory[171] << 24));
                return reg10;
            }
        }

        // addr 172-175
        // 7021 reg11
        public int F12WAYRegister11
        {
            get
            {
                int reg11 = (memory[172] + (memory[173] << 8) + (memory[174] << 16) + (memory[175] << 24));
                return reg11;
            }
        }

        // addr 176-179
        // 7021 reg12
        public int F12WAYRegister12
        {
            get
            {
                int reg12 = (memory[176] + (memory[177] << 8) + (memory[178] << 16) + (memory[179] << 24));
                return reg12;
            }
        }

        // addr 180-183
        // 7021 reg0 rx
        public int F12WAYRegister13
        {
            get
            {
                int reg0Rx = (memory[180] + (memory[181] << 8) + (memory[182] << 16) + (memory[183] << 24));
                return reg0Rx;
            }
        }

        // addr 184-187
        // 7021 reg1 rx
        public int F12WAYRegister14
        {
            get
            {
                int reg1Rx = (memory[184] + (memory[185] << 8) + (memory[186] << 16) + (memory[187] << 24));
                return reg1Rx;
            }
        }

        // addr 188-191
        // 7021 reg15
        public int F12WAYRegister15
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
        // time difference alarm setting in seconds
        public int TimeDifferenceAlarmSetting
        {
            get
            {
                int timeDifferenceAlarmSetting = memory[197];
                return timeDifferenceAlarmSetting;
            }
        }

        // addr 198
        // daily read
        public int DailyRead
        {
            get
            {
                int dailyRead = memory[198];
                return dailyRead;
            }
            set
            {
                memory[198] = (byte)value;
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

        // addr 200-201
        // Unused

        // addr 202
        // Number of tx between rx
        public int RxInterval
        {
            get
            {
                int numberTxBetweenRx = memory[202];
                return numberTxBetweenRx;
            }
        }

        // addr 203
        // Unused

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

        // addr 205-231
        // reserved for alias

        // addr 232
        // Supplier Code
        public char SupplierCode
        {
            get
            {
                char supplierCode = Convert.ToChar(memory[232]);
                return supplierCode;
            }
        }

        // addr 233-236
        // PCB serial number
        public int PcbSerialNumber
        {
            get
            {
                int pcbSerialNumber = memory[233] +
                    (memory[234] << 8) +
                    (memory[235] << 16) +
                    (memory[236] << 24);
                return pcbSerialNumber;
            }
        }

        // addr 237
        // product revision
        public char ProductRevision
        {
            get
            {
                char productRevision = Convert.ToChar(memory[237]);
                return productRevision;
            }
        }

        public string PcbNumber
        {
            get
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
        }


        // addr 238
        // test fail count (not implemented)

        // addr 239
        // test flags

        // addr 240-241
        // firmware build number
        public int FirmwareBuildNumber
        {
            get
            {
                int firmwareBuildNumber = (memory[240] + (memory[241] << 8));
                return firmwareBuildNumber;
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
        // DCO calibration values

        // addr 256-271
        // AES encryption key
        public byte[] AesEncryptionKey
        {
            set
            {
                value.CopyTo(memory, 256);
            }
        }

        // addr 272-317
        // Unused

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

            return  outNum;
        }

    }
}
