﻿using Lexi.Interfaces;
using MTUComm.actions;
using MTUComm.MemoryMap;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xml;

namespace MTUComm
{

    public class Action
    {
        public enum ActionType
        {
            ReadMtu,
            AddMtu,
            ReplaceMTU,
            AddMtuAddMeter,
            AddMtuReplaceMeter,
            ReplaceMtuReplaceMeter,
            ReplaceMeter,
            TurnOffMtu,
            TurnOnMtu,
            Diagnosis,
            BasicRead
        }

        private Dictionary<ActionType, String> displays = new Dictionary<ActionType, String>()
        {
            {ActionType.ReadMtu,"Read MTU" },
            {ActionType.AddMtu,"Add MTU" },
            {ActionType.ReplaceMTU,"Replace MTU" },
            {ActionType.AddMtuAddMeter,"Add MTU/Meter" },
            {ActionType.AddMtuReplaceMeter,"Add MTU/Replace Meter" },
            {ActionType.ReplaceMtuReplaceMeter,"Replace MTU/Meter" },
            {ActionType.ReplaceMeter,"Replace Meter" },
            {ActionType.TurnOffMtu,"Turn Off MTU" },
            {ActionType.TurnOnMtu,"Turn On MTU" },
            {ActionType.Diagnosis, "" }
        };

        private Dictionary<ActionType, String> tag_types = new Dictionary<ActionType, String>()
        {
            {ActionType.ReadMtu,"ReadMTU" },
            {ActionType.AddMtu,"Program MTU" },
            {ActionType.ReplaceMTU,"Program MTU" },
            {ActionType.AddMtuAddMeter,"Program MTU" },
            {ActionType.AddMtuReplaceMeter,"Program MTU" },
            {ActionType.ReplaceMtuReplaceMeter,"Program MTU" },
            {ActionType.ReplaceMeter,"Program MTU" },
            {ActionType.TurnOffMtu,"TurnOffMtu" },
            {ActionType.TurnOnMtu,"TurnOnMTU" },
            {ActionType.Diagnosis, "" }
        };

        private Dictionary<ActionType, String> tag_reasons = new Dictionary<ActionType, String>()
        {
            {ActionType.ReadMtu,null },
            {ActionType.AddMtu,"AddMtu" },
            {ActionType.ReplaceMTU,"ReplaceMtu" },
            {ActionType.AddMtuAddMeter,"AddMtuAddMeter" },
            {ActionType.AddMtuReplaceMeter,"AddMtuReplaceMeter" },
            {ActionType.ReplaceMtuReplaceMeter,"ReplaceMtuReplaceMeter" },
            {ActionType.ReplaceMeter,"ReplaceMeter" },
            {ActionType.TurnOffMtu, null },
            {ActionType.TurnOnMtu, null },
            {ActionType.Diagnosis, "" }
        };

        private ActionType mActionType;
        private List<Parameter> mparameters = new List<Parameter>();
        private Boolean canceled = false;
        private String mUser;

        private Logger logger;
        private Configuration configuration;

        private List<Action> sub_actions = new List<Action>();

        private int order = 0;


        private MTUComm comm;


        public delegate void ActionFinishHandler(object sender, ActionFinishArgs e);
        public event ActionFinishHandler OnFinish;


        public delegate void ActionErrorHandler(object sender, ActionErrorArgs e);
        public event ActionErrorHandler OnError;


        public Action(Configuration config, ISerial serial, ActionType actiontype)
        {
            configuration = config;
            logger = new Logger(config);
            comm = new MTUComm(serial, config);
            mActionType = actiontype;
            mUser = null;
        }

        public Action(Configuration config, ISerial serial, ActionType actiontype, String user)
        {
            configuration = config;
            logger = new Logger(config);
            comm = new MTUComm(serial, config);
            mActionType = actiontype;
            mUser = user;
        }

        public Action(Configuration config, ISerial serial, ActionType actiontype, String user, String outputfile)
        {
            configuration = config;
            logger = new Logger(config, outputfile);
            comm = new MTUComm(serial, config);
            mActionType = actiontype;
            mUser = user;
        }

        public void addParameter(Parameter parameter)
        {
            mparameters.Add(parameter);
        }

        public void addActions(Action action)
        {
            sub_actions.Add(action);
        }

        public Parameter[] getParameters()
        {
            return mparameters.ToArray();
        }

        public Action[] getSubActions()
        {
            return sub_actions.ToArray();
        } 

        public void Run ( MtuForm mtuForm = null )
        {
            if (canceled)
            {
                throw new Exception("Canceled Action can not be Executed");
            }
            else
            {
                switch (mActionType)
                {
                    case ActionType.ReadMtu:
                        comm.OnReadMtu += Comm_OnReadMtu;
                        comm.ReadMTU();
                        break;
                    case ActionType.AddMtu:
                        comm.OnAddMtu += Comm_OnAddMtu;
                        comm.AddMtu(( AddMtuForm )mtuForm); // this.getParameters());
                        break;
                    case ActionType.TurnOffMtu:
                        comm.OnTurnOffMtu += Comm_OnTurnOffMtu;
                        comm.TurnOffMtu();
                        break;
                    case ActionType.TurnOnMtu:
                        comm.OnTurnOnMtu += Comm_OnTurnOnMtu;
                        comm.TurnOnMtu();
                        break;
                    case ActionType.BasicRead:
                        comm.OnBasicRead += Comm_OnBasicRead;
                        comm.BasicRead();
                        break;
                    default:
                        ActionRunSimulator();
                        break;
                }

            }

        }

        public int Order
        {
            get
            {
                return this.order;
            }

            set
            {
                this.order = value;
            }
        }

        private ActionResult ReadPort(int portnumber, IMemoryMap memory, Mtu mtutype)
        {
            ActionResult result = new ActionResult();

            Port portType = mtutype.Ports[portnumber];
            result.AddParameter(new Parameter("Description", "Description", portType.Description));

            int meterid = (int)memory.GetType().GetProperty("P" + (portnumber + 1) + "MeterType").GetValue(memory, null);

            if(meterid != 0) {
                Meter metertype = configuration.getMeterTyoeById(meterid);
                result.AddParameter(new Parameter("MeterType", "Meter Type", metertype.Display));
                result.AddParameter(new Parameter("MeterTypeId", "Meter Type ID", meterid.ToString()));


                switch (mtutype.HexNum.Substring(0, 2))
                {
                    case "31":
                    case "32":
                        MemoryMap31xx32xx mmap31xx32xx = (MemoryMap31xx32xx)memory;

                        Match match = Regex.Match(metertype.Display,
                                @"(\w+) (\d+)D PF(\d+) (\w+)",
                                RegexOptions.IgnoreCase | RegexOptions.Singleline |
                                RegexOptions.CultureInvariant | RegexOptions.Compiled);
                        if (match.Success)
                        {
                            String NumberOfDials = match.Groups[2].Value;
                            String DriveDialSize = match.Groups[3].Value;
                            String UnitOfMeasure = match.Groups[4].Value;

                            result.AddParameter(new Parameter("NumberOfDials", "Number Of Dials", NumberOfDials));
                            result.AddParameter(new Parameter("DriveDialSize", "Drive Dial Size", DriveDialSize));
                            result.AddParameter(new Parameter("UnitOfMeasure", "Unit Of Measure", UnitOfMeasure));
                        }


                        break;
                    case "33":
                        break;
                    case "34":
                        result.AddParameter(new Parameter("NumberOfDigits", "Digits #", metertype.LiveDigits.ToString()));
                        break;
                }

                ulong meter_id = (ulong) memory.GetType().GetProperty("P" + (portnumber + 1) + "MeterId").GetValue(memory, null);
                result.AddParameter(new Parameter("AcctNumber", "Service Pt. ID", meter_id.ToString()));

                

                string meter_reading_error = (string)memory.GetType().GetProperty("P" + (portnumber + 1) + "ReadingError").GetValue(memory, null);
                if (meter_reading_error.Length < 1)
                {
                    uint meter_reading = 0;
                    try
                    {
                        meter_reading = (uint)memory.GetType().GetProperty("P" + (portnumber + 1) + "Reading").GetValue(memory, null);
                    }
                    catch (Exception e) { }

                    uint tempReadingVal = 0;
                    if (mtutype.PulseCountOnly)
                    {
                        tempReadingVal = meter_reading * (uint)metertype.HiResScaling;
                    }
                    else
                    {
                        tempReadingVal = meter_reading;
                    }


                    String tempReading = tempReadingVal.ToString();
                    if (metertype.LiveDigits < tempReading.Length)
                    {
                        tempReading = tempReading.Substring(tempReading.Length - metertype.LiveDigits - (tempReading.IndexOf('.') > -1 ? 1 : 0));
                    }
                    else
                    {
                        tempReading = tempReading.PadLeft(metertype.LiveDigits, '0');
                    }
                    if (metertype.LeadingDummy > 0) // KG 12/08/2008
                        tempReading = tempReading.PadLeft(tempReading.Length + metertype.LeadingDummy, configuration.useDummyDigits() ? 'X' : '0');
                    if (metertype.DummyDigits > 0)  // KG 12/08/2008
                        tempReading = tempReading.PadRight(tempReading.Length + metertype.DummyDigits, configuration.useDummyDigits() ? 'X' : '0');
                    if (metertype.Scale > 0 && tempReading.IndexOf(".") == -1) // 8.12.2011 KG add for F1 Pulse
                        tempReading = tempReading.Insert(tempReading.Length - metertype.Scale, ".");
                    if (metertype.PaintedDigits > 0 && configuration.useDummyDigits()) // KG 12/08/2008
                        tempReading = tempReading.PadRight(tempReading.Length + metertype.PaintedDigits, '0').Insert(tempReading.Length, " - ");


                    result.AddParameter(new Parameter("MeterReading", "Meter Reading", tempReading));
                }
                else
                {
                    result.AddParameter(new Parameter("MeterReading", "Meter Reading", meter_reading_error));
                }

                
            }
            else
            {
                result.AddParameter(new Parameter("MeterType", "Meter Type", "Not Installed"));
                result.AddParameter(new Parameter("MeterTypeId", "Meter Type ID", meterid.ToString()));
                result.AddParameter(new Parameter("AcctNumber", "Service Pt. ID", "000000000"));
                result.AddParameter(new Parameter("MeterReading", "Meter Reading", "Bad Reading"));
            }

            return result;
        }

        private String getAlarmStatus(Boolean alarmenabled, Boolean alarmTriggered)
        {
            if (alarmenabled)
            {
                if (alarmTriggered)
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


        private ActionResult ReadMTU(IMemoryMap memory, Mtu mtutype)
        {
            ActionResult result = new ActionResult();

            result.AddParameter(new Parameter("MtuStatus", "MTU Status", memory.Shipbit ? "OFF" : "ON"));
            result.AddParameter(new Parameter("MtuSerialNumber", "MTU Ser No", memory.MtuId.ToString()));


           if (!memory.Shipbit)
            {
                result.AddParameter(new Parameter("DailySnap", "Daily Snap", memory.DailySnap ));
                result.AddParameter(new Parameter("DailyGMTHourRead", "Daily GMT Hour Read", memory.DailyRead.ToString()));
            }


            for (int i = 0; i < mtutype.Ports.Count; i++)
            {

                result.addPort(ReadPort(i, memory, mtutype));
            }

            result.AddParameter(new Parameter("XmitInterval", "Xmit Interval", timeFormatter(memory.MessageOverlapCount * memory.ReadInterval) ));
            result.AddParameter(new Parameter("ReadInterval", "Read Interval", timeFormatter(memory.ReadInterval)));
            result.AddParameter(new Parameter("ReadIntervalMinutes", "ReadIntervalMinutes", memory.ReadInterval.ToString()));
            result.AddParameter(new Parameter("MtuVoltageBattery", "Battery", ((memory.BatteryVoltage*1.0)/1000).ToString("0.00 V")));

            result.AddParameter(new Parameter("MtuType", "MTU Type", memory.MtuType.ToString()));
            result.AddParameter(new Parameter("MtuSoftware", "MTU Software", memory.MtuFirmwareVersion));
            result.AddParameter(new Parameter("MTUFirmwareVersionFormatFlag", "MTU firmware version format flag", memory.MtuFirmwareVersionFormatFlag.ToString()));

            switch (mtutype.HexNum.Substring(0, 2))
            {
                case "31": //31xx MTU Family
                case "32"://32xx MTU Family
                    dynamic registers = ( MemoryMap31xx32xx )memory;

                    if (!memory.Shipbit)
                    {
                        result.AddParameter(new Parameter("InterfaceTamperStatus", "Interface Tamp", getAlarmStatus(registers.P1PciAlarm, registers.ProgrammingCoilInterfaceTamper)));
                        result.AddParameter(new Parameter("TiltTamperStatus", "Tilt Tamp", getAlarmStatus(registers.P1TiltAlarm, registers.TiltTamper)));
                        result.AddParameter(new Parameter("MagneticTamperStatus", "Magnetic Tamp", getAlarmStatus(registers.P1MagneticAlarm, registers.MagneticTamper)));
                        result.AddParameter(new Parameter("RegisterCoverTamperStatus", "Reg. Cover", getAlarmStatus(registers.P1RegisterCoverAlarm, registers.RegisterCoverTamper)));
                        result.AddParameter(new Parameter("ReverseFlowTamperStatus", "Rev. Fl Tamp", getAlarmStatus(registers.P1ReverseFlowAlarm, registers.ReverseFlowTamper)));
                    }

                    result.AddParameter(new Parameter("RxInterval", "Rx Interval", registers.RxInterval.ToString()));
                    result.AddParameter(new Parameter("F12WAYRegister1", "F12WAYRegister1", "0x"+registers.F12WAYRegister1.ToString("X8")));
                    result.AddParameter(new Parameter("F12WAYRegister10", "F12WAYRegister10", "0x" + registers.F12WAYRegister10.ToString("X8")));
                    result.AddParameter(new Parameter("F12WAYRegister14", "F12WAYRegister14", "0x" + registers.F12WAYRegister14.ToString("X8")));


                    break;
                case "33"://33xx MTU Family
                    dynamic mmap33xx = (MemoryMap33xx)memory;


                    if (!memory.Shipbit)
                    {
                        result.AddParameter(new Parameter("BackFlowState", "Backflow State", mmap33xx.BackFlowState ));
                        result.AddParameter(new Parameter("DaysOfNoFlow", "Days of No Flow", mmap33xx.DaysOfNoFlow));
                        result.AddParameter(new Parameter("LeakDetection", "Leak Detection", mmap33xx.LeakDetection));
                        result.AddParameter(new Parameter("DaysOfLeak", "Days of Leak", mmap33xx.DaysOfLeak));

                    }

                    result.AddParameter(new Parameter("RxInterval", "Rx Interval", mmap33xx.RxInterval.ToString()));
                    result.AddParameter(new Parameter("F12WAYRegister1", "F12WAYRegister1", "0x" + mmap33xx.F12WAYRegister1.ToString("X8")));
                    result.AddParameter(new Parameter("F12WAYRegister10", "F12WAYRegister10", "0x" + mmap33xx.F12WAYRegister10.ToString("X8")));
                    result.AddParameter(new Parameter("F12WAYRegister14", "F12WAYRegister14", "0x" + mmap33xx.F12WAYRegister14.ToString("X8")));
                    
                    break;
                case "34"://34xx MTU Family
                    dynamic mmap342x = (MemoryMap342x)memory;

                    if (!memory.Shipbit)
                    {
                        result.AddParameter(new Parameter("InterfaceTamperStatus", "Interface Tamp",getAlarmStatus(mmap342x.ProgrammingCoilInterfaceTamper, mmap342x.TampersProgrammingCoilInterfaceTamper)));
                        result.AddParameter(new Parameter("LastGasp", "Last Gasp", getAlarmStatus(mmap342x.MtuLastGasp, mmap342x.TampersMtuLastGasp)));
                        result.AddParameter(new Parameter("InsufficentMemory", "Insf. Mem", getAlarmStatus(mmap342x.InsufficientMemory, mmap342x.TampersInsufficientMemory)));
                        result.AddParameter(new Parameter("Cut1WireTamperStatus", "Cut 1 Wire Tamp", getAlarmStatus(mmap342x.CutWirePort1, mmap342x.TampersCutWirePort1)));
                        result.AddParameter(new Parameter("CutWireDelaySetting", "Cut Wire Delay", mmap342x.CutWireDelaySetting.ToString()));
                    }



                    result.AddParameter(new Parameter("Encryption", "Encrypted", mmap342x.EncryptionEnabled ? "Yes" : "No"));
                    result.AddParameter(new Parameter("EncryptionIndex", "Encryption Index", mmap342x.EncryptionIndex.ToString()));
                    result.AddParameter(new Parameter("Fast-2-Way", "2-Way", mmap342x.FastMessagingMode? "Fast" : "Slow"));
                    result.AddParameter(new Parameter("MtuMsgProtocolConfig", "Mtu Msg Protocol Config", mmap342x.MtuMsgProtocolConfig.ToString()));
                    result.AddParameter(new Parameter("MtuPrimaryWindowInterval", "Mtu Primary Window Interval", mmap342x.MtuPrimaryWindowInterval.ToString()));
                    result.AddParameter(new Parameter("MtuWindowAStart", "Mtu Window A Start", mmap342x.MtuWindowAStart.ToString()));
                    result.AddParameter(new Parameter("MtuWindowBStart", "Mtu Window B Start", mmap342x.MtuWindowBStart.ToString()));
                    result.AddParameter(new Parameter("MtuPrimaryWindowIntervalB", "Mtu Primary Window Interval B", mmap342x.MtuPrimaryWindowIntervalB.ToString()));
                    result.AddParameter(new Parameter("MtuPrimaryWindowOffset", "Mtu Primary Window Offset", mmap342x.MtuPrimaryWindowOffset.ToString()));
                    result.AddParameter(new Parameter("MtuNumLowPriorityMsg", "Mtu Num Low Priority Msg", mmap342x.MtuNumLowPriorityMsg.ToString()));
                    result.AddParameter(new Parameter("OnDemandCount", "On Demand Cnt", mmap342x.OnDemandCount.ToString()));
                    result.AddParameter(new Parameter("DataRequestCount", "Data Req Cnt", mmap342x.DataRequestCount.ToString()));
                    result.AddParameter(new Parameter("FOTACount", "FOTA Cnt", mmap342x.FOTACount.ToString()));
                    result.AddParameter(new Parameter("FOTCCount", "FOTC Cnt", mmap342x.FOTCCount.ToString()));
                    result.AddParameter(new Parameter("TxBetweenRx", "Tx Between Rx", mmap342x.NumberTxBetweenRx.ToString()));
                    break;
            }

            result.AddParameter(new Parameter("MtuPartNumber", "Mtu Part Number", mtutype.HexNum));
            result.AddParameter(new Parameter("PCBNumber", "PCB Number", memory.PcbNumber));


            return result;

        }

        private String timeFormatter(int time)
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



        private void Comm_OnReadMtu(object sender, MTUComm.ReadMtuArgs e)
        {
            ActionResult result = ReadMTU(e.MemoryMap, e.MtuType);
            logger.logReadResult(this, result);
            ActionFinishArgs args = new ActionFinishArgs(result);
            OnFinish(this, args);
        }

        private void Comm_OnTurnOffMtu(object sender, MTUComm.TurnOffMtuArgs e)
        {
            logger.logTurnOffResult(this, e.MtuId);
            ActionFinishArgs args = new ActionFinishArgs(null); // TODO: add turn off result
            OnFinish(this, args);
        }

        private void Comm_OnTurnOnMtu(object sender, MTUComm.TurnOnMtuArgs e)
        {
            logger.logTurnOnResult(this, e.MtuId);
            ActionFinishArgs args = new ActionFinishArgs(null); // TODO: add turn on result
            OnFinish(this, args);
        }

        private void Comm_OnAddMtu(object sender, MTUComm.AddMtuArgs e)
        {
            logger.logAddMtuResult(this);
            ActionFinishArgs args = new ActionFinishArgs(null); // TODO: add add mtu result
            OnFinish(this, args);
        }

        private void Comm_OnBasicRead(object sender, MTUComm.BasicReadArgs e)
        {
            uint MtuId = e.MtuType;
            ActionResult result = new ActionResult();
            result.AddParameter(new Parameter("MtuId", "MTU ID", MtuId.ToString()));
            ActionFinishArgs args = new ActionFinishArgs(result);
            OnFinish(this, args);
        }

        public String getUser()
        {
            return mUser;
        }

        public void Cancel()
        {
            canceled = true;
            logger.logCancel(this, "User Cancelled", "410 DR Defective Register");

        }

        public String getDisplay()
        {
            return displays[mActionType];
        }

        public String getLogType()
        {
            return tag_types[mActionType];
        }

        public String getReason()
        {
            return tag_reasons[mActionType];
        }

        public ActionType GetActionType
        {
            get
            {
                return mActionType;
            }
        }

        //
        public class ActionFinishArgs : EventArgs
        {
            public ActionResult Result { get; private set; }

            public ActionFinishArgs(ActionResult result)
            {
                Result = result;
            }
        }

        //
        public class ActionErrorArgs : EventArgs
        {
            public int Status { get; private set; }

            public String Message { get; private set; }

            public ActionErrorArgs(int status, String message)
            {
                Status = status;
                Message = message;
            }

            public ActionErrorArgs(String message)
            {
                Status = -1;
                Message = message;
            }
        }


        private void successAction()
        {
            logger.logAction(this);
            ActionFinishArgs args = new ActionFinishArgs(new ActionResult());
            OnFinish(this, args);
        }

        private void errorAction()
        {
            
            ActionErrorArgs args = new ActionErrorArgs(240, "No valid meter types were found for MTU type 152");

            logger.LogError(args.Status, args.Message);
            OnError(this, args);
        }

        //MOK FUNCTIONS FOR DEV
        //Simulates Rando Success/Fail writin on MTU
        private void ActionRunSimulator()
        {
            var task = Task.Run(() => {
                Random random = new Random();
                Thread.Sleep(random.Next(4000, 5500));
            });
            if (task.Wait(TimeSpan.FromSeconds(5)))
                successAction();
            else
                errorAction();
        }

    }
}
