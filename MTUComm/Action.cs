using Lexi.Interfaces;
using MTUComm.actions;
using MTUComm.MemoryMap;
using System;
using System.Collections.Generic;
using System.Linq;
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
            ReadData,
            MtuInstallationConfirmation,
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
            {ActionType.ReadData,"Read Data Log" },
            {ActionType.MtuInstallationConfirmation,"Install Confirmation" },
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
            {ActionType.ReadData, "Program MTU" },
            {ActionType.MtuInstallationConfirmation,"InstallConfirmation" },
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
            {ActionType.ReadData, "DataRead" },
            {ActionType.MtuInstallationConfirmation,"InstallConfirmation" },
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


        public delegate void ActionProgresshHandler(object sender, ActionProgressArgs e);
        public event ActionProgresshHandler OnProgress;

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
            comm.OnError += Comm_OnError;
        }

        public Action(Configuration config, ISerial serial, ActionType actiontype, String user)
        {
            configuration = config;
            logger = new Logger(config);
            comm = new MTUComm(serial, config);
            mActionType = actiontype;
            mUser = user;
            comm.OnError += Comm_OnError;
        }

        public Action(Configuration config, ISerial serial, ActionType actiontype, String user, String outputfile)
        {
            configuration = config;
            logger = new Logger(config, outputfile);
            comm = new MTUComm(serial, config);
            mActionType = actiontype;
            mUser = user;
            comm.OnError += Comm_OnError;
        }

        public void addParameter(Parameter parameter)
        {
            mparameters.Add(parameter);
        }

        public void AddParameter ( MtuForm form )
        {
            Parameter[] addMtuParams = form.GetParameters ();
            foreach ( Parameter parameter in addMtuParams )
                mparameters.Add (parameter);
        }

        public void addActions(Action action)
        {
            sub_actions.Add(action);
        }

        public Parameter[] getParameters()
        {
            return mparameters.ToArray();
        }


        public Parameter getParameterByTag(string tag)
        {
            return mparameters.Find(x => x.getLogTag().Equals(tag));
        }

        public Parameter getParameterByTagAndPort(string tag, int port)
        {
            return mparameters.Find(x => (x.getLogTag().Equals(tag) && x.Port == port));
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
                List<object> parameters = new List<object> ();

                switch ( mActionType )
                {
                    case ActionType.ReadMtu:
                        comm.OnReadMtu += Comm_OnReadMtu;
                        break;

                    case ActionType.AddMtu:
                        comm.OnAddMtu += Comm_OnAddMtu;
                        parameters.AddRange ( new object[] { ( AddMtuForm )mtuForm, this.getUser() } );
                        break;

                    case ActionType.TurnOffMtu:
                        comm.OnTurnOffMtu += Comm_OnTurnOffMtu;
                        break;

                    case ActionType.TurnOnMtu:
                        comm.OnTurnOnMtu += Comm_OnTurnOnMtu;
                        break;

                    case ActionType.MtuInstallationConfirmation:
                        comm.OnReadMtu  += Comm_OnReadMtu;
                        comm.OnProgress += Comm_OnProgress;
                        break;

                    case ActionType.ReadData:
                        Parameter param = mparameters.Find ( x => ( x.Type == Parameter.ParameterType.DaysOfRead ) );
                        if ( param == null )
                        {
                            OnError ( this, new ActionErrorArgs("Days Of Read parameter Not Defined or Invalid" ) );
                            break;
                        }
                        int DaysOfRead = 0;
                        if ( ! Int32.TryParse ( param.Value, out DaysOfRead ) || DaysOfRead <= 0 )
                        {
                            OnError ( this, new ActionErrorArgs("Days Of Read parameter Invalid" ) );
                            break;
                        }
                        comm.OnReadMtuData += Comm_OnReadMtuData;
                        parameters.Add ( DaysOfRead );
                        break;

                    case ActionType.BasicRead:
                        comm.OnBasicRead += Comm_OnBasicRead;
                        break;

                    default:
                        ActionRunSimulator ();
                        break;
                }

                // Is more easy to control one point of invokation
                // than N, one for each action/new task to launch
                comm.LaunchActionThread ( mActionType, parameters.ToArray () );
            }
        }

        private void Comm_OnProgress(object sender, MTUComm.ProgressArgs e)
        {

            
            try
            {
                OnProgress(this, new ActionProgressArgs(e.Step, e.TotalSteps, e.Message));
            }
            catch (Exception pe)
            {

            }
            
        }

        private void Comm_OnError(object sender, MTUComm.ErrorArgs e)
        {
            logger.LogError(e.Status, e.LogMessage);
            try
            {
                OnError(this, new ActionErrorArgs(e.Status, e.Message));
            }
            catch (Exception ee)
            {

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

        private bool validateCondition(string condition, dynamic registers, Mtu mtutype)
        {
            return validateCondition(condition, registers, mtutype, "");
        }

        private bool validateCondition(string condition, dynamic registers, Mtu mtutype, String port)
        {
            if(condition == null)
            {
                return true;
            }

            try
            {
                MatchCollection matches = Regex.Matches(condition, @"([&|]?)(([^&|=]+)=([^&|=#]*))", RegexOptions.Compiled);
                
                List < ConditionObjet > conditions = new List<ConditionObjet>();

                foreach(Match m in matches.Cast<Match>().ToList())
                {
                    conditions.Add(new ConditionObjet(Uri.UnescapeDataString(m.Groups[1].Value), Uri.UnescapeDataString(m.Groups[3].Value), Uri.UnescapeDataString(m.Groups[4].Value)));
                }

                int condition_final_result = 0;

                foreach (ConditionObjet item in conditions)
                {
                    string condition_value = "";
                    int condition_result = 0;

                    switch (item.Key.Split(new char[] { '.' })[0])
                    {
                        case "Port":
                            try
                            {
                                condition_value = port;
                            }
                            catch (Exception e) { }
                            break;
                        case "Action":
                            try
                            {
                                string action_property_name = item.Key.Split(new char[] { '.' })[1];
                                condition_value = GetProperty(action_property_name);
                            }
                            catch (Exception e) { }
                            break;
                        case "MeterType":
                            break;
                        case "MtuType":
                            try
                            {
                                string mtu_property_name = item.Key.Split(new char[] { '.' })[1];
                                condition_value = mtutype.GetProperty(mtu_property_name);
                            }
                            catch (Exception e) { }
                            break;
                        default:
                            try
                            {
                                string memory_property_name = item.Key.Split(new char[] { '.' })[1];
                                condition_value = registers.GetProperty(port+memory_property_name).Value.ToString();
                            }
                            catch (Exception e) { }
                            break;

                    }

                    if (condition_value.Length > 0 && !condition_value.ToLower().Equals(item.Value.ToLower()))
                    { 
                        condition_result = 0;
                    }
                    else
                    {
                        condition_result = 1;
                    }

                    if (item.Condition.Equals("|"))
                    {
                        condition_final_result = condition_final_result + condition_result;
                    }

                    if (item.Condition.Equals("&"))
                    {
                        condition_final_result = condition_final_result * condition_result;
                    }


                }

                if(condition_final_result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + "\r\n" + e.StackTrace);
            };

            return true;
        }


        private ActionResult ReadPort(int portnumber, InterfaceParameters[] parameters, dynamic registers, Mtu mtutype)
        {
            ActionResult result = new ActionResult();

            Port PortType = mtutype.Ports[portnumber];
           

            int meterid = registers.GetProperty("P" + (portnumber + 1) + "MeterType").Value;

            if (meterid != 0)
            {
                Meter Metertype = configuration.getMeterTypeById(meterid);
                if (Metertype.Type == "NOTFOUND")
                {
                    //logger.LogError("No valid meter types were found for MTU type " + Metertype.Id);
                }

                foreach (InterfaceParameters parameter in parameters)
                {
                    if (parameter.Name.Equals("MeterReading"))
                    {
                        if (validateCondition(parameter.Conditional, registers, mtutype, "P" + (portnumber + 1)))
                        {
                            string meter_reading_error = registers.GetProperty("P" + (portnumber + 1) + "ReadingError").Value.ToString();
                            if (meter_reading_error.Length < 1)
                            {
                                ulong meter_reading = 0;
                                try
                                {
                                    meter_reading = registers.GetProperty("P" + (portnumber + 1) + "Reading").Value;
                                }
                                catch (Exception e) { }

                                ulong tempReadingVal = 0;
                                if (mtutype.PulseCountOnly)
                                {
                                    tempReadingVal = meter_reading *  (ulong)Metertype.HiResScaling;
                                }
                                else
                                {
                                    tempReadingVal = meter_reading;
                                }


                                String tempReading = tempReadingVal.ToString();
                                if (Metertype.LiveDigits < tempReading.Length)
                                {
                                    tempReading = tempReading.Substring(tempReading.Length - Metertype.LiveDigits - (tempReading.IndexOf('.') > -1 ? 1 : 0));
                                }
                                else
                                {
                                    tempReading = tempReading.PadLeft(Metertype.LiveDigits, '0');
                                }
                                if (Metertype.LeadingDummy > 0) // KG 12/08/2008
                                    tempReading = tempReading.PadLeft(tempReading.Length + Metertype.LeadingDummy, configuration.useDummyDigits() ? 'X' : '0');
                                if (Metertype.DummyDigits > 0)  // KG 12/08/2008
                                    tempReading = tempReading.PadRight(tempReading.Length + Metertype.DummyDigits, configuration.useDummyDigits() ? 'X' : '0');
                                if (Metertype.Scale > 0 && tempReading.IndexOf(".") == -1) // 8.12.2011 KG add for F1 Pulse
                                    tempReading = tempReading.Insert(tempReading.Length - Metertype.Scale, ".");
                                if (Metertype.PaintedDigits > 0 && configuration.useDummyDigits()) // KG 12/08/2008
                                    tempReading = tempReading.PadRight(tempReading.Length + Metertype.PaintedDigits, '0').Insert(tempReading.Length, " - ");

                                if (tempReading == "")
                                {
                                    tempReading = "INVALID";
                                }
                                result.AddParameter(new Parameter(parameter.Name, parameter.Display, tempReading));
                            }
                            else
                            {
                                result.AddParameter(new Parameter(parameter.Name, parameter.Display, meter_reading_error));
                            }
                        }

                    }
                    else
                    {
                        try
                        {
                            if (validateCondition(parameter.Conditional, registers, mtutype, "P" + (portnumber + 1)))
                            {
                                if (parameter.Source == null)
                                {
                                    parameter.Source = "";
                                }
                                string val = null;
                                string property_type = "";
                                string property_name = "";
                                
                                if(parameter.Source.Contains(".") && parameter.Source.Length >= 3)
                                {
                                    property_type = parameter.Source.Split(new char[] { '.' })[0];
                                    property_name = parameter.Source.Split(new char[] { '.' })[1];
                                }

                                string name = parameter.Name;

                                switch (property_type)
                                {
                                    case "PortType":
                                        val = PortType.GetProperty(property_name);
                                        break;
                                    case "MeterType":
                                        val = Metertype.GetProperty(property_name);
                                        break;
                                    case "MtuType":
                                        val = mtutype.GetProperty(property_name);
                                        break;
                                    case "MemoryMap":
                                        val = registers.GetProperty("P" + (portnumber + 1) + property_name).Value.ToString();
                                        name = property_name;
                                        break;
                                    default:
                                        val = registers.GetProperty("P" + (portnumber + 1) + parameter.Name).Value.ToString();
                                        break;

                                }
                                if (!string.IsNullOrEmpty(val))
                                {
                                    result.AddParameter(new Parameter(name, parameter.Display, val));
                                }

                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + "\r\n" + e.StackTrace);
                        }
                    }

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

        private ActionResult ReadMTUData(DateTime start, DateTime end, List<LogDataEntry> Entries, MTUBasicInfo mtuInfo, Mtu mtutype)
        {
            ActionResult result = new ActionResult();

            string log_path = logger.logReadDataResultEntries(mtuInfo.Id.ToString("d15"), start, end , Entries);

            InterfaceParameters[] parameters = configuration.getAllInterfaceFields(mtutype.Id, "DataRead");
            foreach (InterfaceParameters parameter in parameters)
            {
                if (parameter.Name.Equals("Port"))
                {
                    for (int i = 0; i < mtutype.Ports.Count; i++)
                    {
                        foreach (InterfaceParameters port_parameter in parameter.Parameters)
                        {
                            if(port_parameter.Source != null && port_parameter.Source.StartsWith("ActionParams"))
                            {
                                Parameter sel_parameter = getParameterByTagAndPort(port_parameter.Name, i + 1);
                                if(sel_parameter != null)
                                {
                                    result.AddParameter(new Parameter(port_parameter.Name, port_parameter.Display, sel_parameter.Value));
                                }
                            }
                            
                        }


                    }
                }
                else
                {
                    try
                    {

                        if (parameter.Source != null && parameter.Source.StartsWith("ActionParams"))
                        {
                            Parameter sel_parameter = getParameterByTag(parameter.Name);
                            if (sel_parameter != null)
                            {
                                result.AddParameter(new Parameter(parameter.Name, parameter.Display, sel_parameter.Value));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "\r\n" + e.StackTrace);
                    }
                }

            }

            result.AddParameter(new Parameter("ReadRequest", "Number Read Request Days", ""));
            result.AddParameter(new Parameter("ReadResult", "Read Result", "Number of Reads "+ Entries.Count.ToString()+ " for Selected Period From "+start.ToString("dd/MM/yyyy") + " 0:00:00 Till "+ end.ToString("dd/MM/yyyy") + " 23:59:59"));
            result.AddParameter(new Parameter("ReadResultFile", "Read Result File", log_path));

            return result;
        }

        public ActionResult ReadMTU(dynamic registers, Mtu mtutype)
        {
            InterfaceParameters[] parameters = configuration.getAllInterfaceFields(mtutype.Id, "ReadMTU");

            ActionResult result = new ActionResult();
            foreach (InterfaceParameters parameter in parameters)
            {
                if (parameter.Name.Equals("Port"))
                {
                    for (int i = 0; i < mtutype.Ports.Count; i++)
                    {

                        result.addPort(ReadPort(i, parameter.Parameters.ToArray(), registers, mtutype));
                    }
                }
                else
                {
                    try
                    {
                        if (validateCondition(parameter.Conditional, registers, mtutype))
                        {
                            if (parameter.Source == null)
                            {
                                parameter.Source = "";
                            }

                            switch (parameter.Source.Split(new char[] { '.' })[0])
                            {
                                case "Action":
                                    string action_property_name = parameter.Source.Split(new char[] { '.' })[1];
                                    string action_property_value = GetProperty(action_property_name);
                                    if (action_property_value != null)
                                    {
                                        result.AddParameter(new Parameter(parameter.Name, parameter.Display, action_property_value));
                                    }
                                    break;
                                case "MeterType":
                                    break;
                                case "MtuType":
                                    string mtu_property_name = parameter.Source.Split(new char[] { '.' })[1];
                                    result.AddParameter(new Parameter(parameter.Name, parameter.Display, mtutype.GetProperty(mtu_property_name)));
                                    break;
                                case "MemoryMap":
                                    string memory_alt_property_name = parameter.Source.Split(new char[] { '.' })[1];
                                    result.AddParameter(new Parameter(memory_alt_property_name, parameter.Display, registers.GetProperty(memory_alt_property_name).Value.ToString()));
                                    break;
                                default:
                                    result.AddParameter(new Parameter(parameter.Name, parameter.Display, registers.GetProperty(parameter.Name).Value.ToString()));
                                break;

                            }

                        }
                
                    }
                    catch (Exception e) {
                        Console.WriteLine(e.Message+"\r\n"+e.StackTrace);
                    }
                }

            }
            return result;
        }

        private void Comm_OnReadMtuData(object sender, MTUComm.ReadMtuDataArgs e)
        {
            ActionProgressArgs args;
            switch (e.Status)
            {
                case LogQueryResult.LogDataType.Bussy:
                    args = new ActionProgressArgs(0,0);
                    OnProgress(this, args);
                    break;
                case LogQueryResult.LogDataType.NewPacket:
                    args = new ActionProgressArgs(e.CurrentEntry, e.TotalEntries);
                    OnProgress(this, args);
                    break;
                case LogQueryResult.LogDataType.LastPacket:
                    Mtu mtu_type = configuration.GetMtuTypeById((int)e.MtuType.Type);
                    ActionResult result = ReadMTUData(e.Start, e.End, e.Entries, e.MtuType, mtu_type);
                    logger.logReadDataResult(this, result, mtu_type);
                    ActionFinishArgs f_args = new ActionFinishArgs(null);
                    OnFinish(this, f_args);
                    break;
            }
        }


        private void Comm_OnReadMtu(object sender, MTUComm.ReadMtuArgs e)
        {
            ActionResult result = ReadMTU(e.MemoryMap, e.MtuType);
            logger.logReadResult(this, result, e.MtuType);
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

        public String GetProperty(string name)
        {
            switch (name)
            {
                case "User":
                    return mUser;
                case "Date":
                    return DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");
                default:
                    return "";
            }
            return "";
        }

        private ActionResult Comm_OnAddMtu(object sender, MTUComm.AddMtuArgs e)
        {
            ActionResult result = ReadMTU(e.MemoryMap, e.MtuType);
            ActionFinishArgs args = new ActionFinishArgs(result);
            OnFinish(this, args);
            return result;
        }

        private void Comm_OnBasicRead(object sender, MTUComm.BasicReadArgs e)
        {
            ActionResult result = new ActionResult();
            ActionFinishArgs args = new ActionFinishArgs(result);
            OnFinish(this, args);
        }

        public String getUser()
        {
            return mUser;
        }

        public void Cancel(string cancelReason="410 DR Defective Register")
        {
            canceled = true;
            logger.logCancel(this, "User Cancelled", cancelReason);

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

        public String getResultXML(ActionResult result)
        {
            return logger.logReadResultString(this, result);

        }
        
        //

        public class ActionProgressArgs : EventArgs
        {
            public int Step { get; private set; }
            public int TotalSteps { get; private set; }
            public string Message { get; private set; }

            public ActionProgressArgs(int step, int totalsteps)
            {
                Step = step;
                TotalSteps = totalsteps;
                Message = "";
            }

            public ActionProgressArgs(int step, int totalsteps, string message)
            {
                Step = step;
                TotalSteps = totalsteps;
                Message = message;
            }
        }

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

        private class ConditionObjet
        {
            public String Condition { get; private set; }
            public String Key { get; private set; }
            public String Value { get; private set; }

            public ConditionObjet(string condition, string key, string value)
            {
                if(!condition.Equals("&") && !condition.Equals("|"))
                {
                    Condition = "|";
                }
                else
                {
                    Condition = condition;
                }

                Key = key;
                Value = value;
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
