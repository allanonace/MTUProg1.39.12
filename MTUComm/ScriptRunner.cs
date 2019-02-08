using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Lexi.Interfaces;
using Xml;

namespace MTUComm
{
    public class ScriptRunner
    {



        private List<Action> actions;

        public delegate void ActionFinishHandler(object sender, Action.ActionFinishArgs e);
        public event ActionFinishHandler OnFinish;

        public delegate void ActionOnProgressHandler(object sender, Action.ActionProgressArgs e);
        public event ActionOnProgressHandler OnProgress;

        public delegate void ActionStepFinishHandler(object sender, int step, Action.ActionFinishArgs e);
        public event ActionStepFinishHandler onStepFinish;

        public delegate void ActionErrorHandler(object sender, Action.ActionErrorArgs e);
        public event ActionErrorHandler OnError;


        public ScriptRunner(String base_path, ISerial serial_device, String script_path)
        {
            Script script = new Script();

            XmlSerializer s = new XmlSerializer(typeof(Script));

            try
            {
                using (StreamReader streamReader = new StreamReader(script_path))
                {
                    string fileContent = streamReader.ReadToEnd();
                    using (StringReader reader = new StringReader(fileContent))
                    {
                        script = (Script)s.Deserialize(reader);
                    }
                }


                buildScriptActions(base_path, serial_device, script);
            }

            catch (Exception e)
            {
                throw new MtuLoadException("Error loading Script file");
            }
        }

        public ScriptRunner()
        {
        }

        public void ParseScriptAndRun ( String base_path, ISerial serial_device, String script_stream, int stream_size)
        {
            Script script = new Script();
            XmlSerializer s = new XmlSerializer(typeof(Script));

            try
            {
                using (StringReader reader = new StringReader(script_stream.Substring(0, stream_size)))
                {
                    script = (Script)s.Deserialize(reader);
                }
                buildScriptActions(base_path, serial_device, script); 
            }
            catch (Exception e)
            {
                //throw new MtuLoadException("Error loading Script file");

                OnError ( this, new Action.ActionErrorArgs ( 113, "Error in parsing Trigger File" ) );

                return;
            }

            this.Run ();
        }

        private Action.ActionType parseType(string action_type)
        {
            try
            {
                return (Action.ActionType)Enum.Parse(typeof(Action.ActionType), action_type, true);
            }
            catch (Exception e)
            {
                return Action.ActionType.ReadMtu;
            }
        }

        private Parameter.ParameterType parseParameterType(string action_type)
        {
            try
            {
                return (Parameter.ParameterType)Enum.Parse(typeof(Parameter.ParameterType), action_type, true);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private void buildScriptActions(String base_path, ISerial serial_device, Script script)
        {
            actions = new List<Action>();

            int step = 0;

            foreach (Xml.Action action in script.Actions)
            {
                Action new_action = new Action(Configuration.GetInstance(), serial_device, parseType(action.Type), script.UserName, script.LogFile);
                Type   actionType = action.GetType ();

                foreach (PropertyInfo parameter in action.GetType().GetProperties())
                {
                    try
                    {
                        var  paramValue = actionType.GetProperty ( parameter.Name ).GetValue ( action, null );
                        Type valueType  = paramValue.GetType ();
                    
                        Console.WriteLine ( "-> " + valueType.Name + " " + valueType.IsArray );
                    
                        if ( valueType.Name.ToLower ().Contains ( "actionparameter" ) )
                        {
                            List<ActionParameter> list = new List<ActionParameter> ();
                            
                            if ( ! paramValue.GetType ().IsArray )
                                 list.Add      ( ( ActionParameter   )paramValue );
                            else list.AddRange ( ( ActionParameter[] )paramValue );

                            foreach ( ActionParameter aParam in list )
                                new_action.AddParameter (
                                    new Parameter (
                                        parseParameterType ( parameter.Name ),
                                        aParam.Value,
                                        aParam.Port ) );
                        }
                    }
                    catch (Exception e)
                    {
                    
                    }
                }

                new_action.order = step;
                new_action.OnProgress += Action_OnProgress;
                new_action.OnFinish += Action_OnFinish;
                new_action.OnError += Action_OnError;

                actions.Add(new_action);
                step++;
            }
        }


        public void Run()
        {
            actions.ToArray()[0].Run();
        }


        private void Action_OnError(object sender, Action.ActionErrorArgs e)
        {
            OnError(sender, e);
        }

        private void Action_OnProgress(object sender, Action.ActionProgressArgs e)
        {
            OnProgress(sender, e);
        }

        private void Action_OnFinish(object sender, Action.ActionFinishArgs e)
        {
            Action act = (Action)sender;
            if (act.order < (actions.Count-1))
            {
                onStepFinish(act, act.order, e);
                actions.ToArray()[act.order+1].Run();
            }
            else
            {
                OnFinish(act, e);
            }
            
        }
    }
}
