using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Lexi.Interfaces;
using System.Threading.Tasks;
using Library;
using Library.Exceptions;
using Xml;
using Library;

using ActionType = MTUComm.Action.ActionType;

namespace MTUComm
{
    /// <summary>
    /// Interprets the content of the script files, converting them to a usable
    /// format of the application, initiating the indicated action with the
    /// selected parameters, without ask the user to enter any value.
    /// <para>
    /// This is the way designed and implemented to allow third applications
    /// to interact with this application, returning a URL formatted string,
    /// which can also be compressed, with the information of the successful
    /// result or the error information.
    /// </para>
    /// </summary>
    public class ScriptRunner
    {
        private List<Action> actions;

        /// <summary>
        /// Event that can be launched whenever we want during the action logic execution.
        /// </summary>
        public event ActionOnProgressHandler OnProgress;
        public delegate void ActionOnProgressHandler(object sender, Delegates.ProgressArgs e);

        /// <summary>
        /// Event invoked only if the action completes successfully and without launches an exception.
        /// </summary>
        public event Delegates.ActionFinishHandler OnFinish;
        
        /// <summary>
        /// Event invoked if the action does not complete successfully or if it launches an exception.
        /// </summary>        
        public event Delegates.ActionFinishHandler onStepFinish;

        public delegate void ActionErrorHandler ();
        public event ActionErrorHandler OnError;

        /// <summary>
        /// Tries to parse the content of a script to start the indicated action with
        /// the selected parameters.
        /// <para>
        /// Additional parameters, those that do not
        /// appear in <see cref="Parameter.ParameterType"/> enumeration, will be treated
        /// as information that will only be written in the log, without validating them nor taking
        /// them into account in the actions logic.
        /// </para>
        /// <para>
        /// See <see cref="Run"/> for the method invoked to initiate the actions logic.
        /// </para>
        /// </summary>
        /// <param name="serial_device">BLE interface</param>
        /// <param name="script_stream">Content of the script</param>
        /// <param name="stream_size">Number of characters of the content of the script to be used</param>
        /// <seealso cref="ble_library.BlePort"/>
        public async Task ParseScriptAndRun (
            ISerial serial_device,
            String  script_stream,
            int     stream_size )
        {
            XmlSerializer s = null;
        
            try
            {
                // Script file is empty
                if ( string.IsNullOrEmpty ( script_stream.Trim () ) )
                    throw new ScriptEmptyException ();
            
                Script script = new Script ();
                s = new XmlSerializer ( typeof ( Script ) );
    
                // Register unknown elements ( not present in Script class ) as additional parameters
                s.UnknownElement += this.UnknownElementEvent;
            
                using ( StringReader reader = new StringReader ( script_stream.Substring ( 0, stream_size ) ) )
                {
                    script = ( Script )s.Deserialize ( reader );
                }
                BuildScriptActions ( serial_device, script );
            }
            catch ( Exception e )
            {
                if ( ! Errors.IsOwnException ( e ) )
                     Errors.LogErrorNowAndContinue ( new ScriptWrongStructureException () ); // Script file has invalid format or structure
                else Errors.LogErrorNowAndContinue ( e ); // ScriptLogfileInvalidException, ScriptActionTypeInvalidException

                this.OnError ();
                
                return;
            }
            finally
            {
                s.UnknownElement -= this.UnknownElementEvent;
            }

            await this.Run ();
        }

        /// <summary>
        /// Method invoked each time a property is not recognized during the script parsing
        /// process, because it is not defined in <see cref="ScriptAction"/> as a property
        /// or if it does not have read and write privileges ( get; set; ).
        /// <para>
        /// All these elements will be treated as additional parameters that
        /// will only be written in the log.
        /// </para>
        /// </summary>
        private void UnknownElementEvent (
            object sender,
            XmlElementEventArgs e )
        {
            ScriptAction script = ( ScriptAction )e.ObjectBeingDeserialized;
            script.AddAdditionParameter ( e.Element.Name, e.Element.InnerText );
        }

        /// <summary>
        /// After parse the script content, the specific action is launched using selected parameters
        /// and registering all required events ( OnProgress, OnError and OnFinish ) before execute
        /// actions logic.
        /// </summary>
        /// <remarks>
        /// The action type should be listen in <see cref="ActionType"/> enumeration or won't be executed.
        /// </remarks>  
        /// <param name="serial_device">BLE interface</param>
        /// <param name="script">Script already parsed to a usable object in the application</param>
        private void BuildScriptActions (
            ISerial serial_device,
            Script script )
        {
            actions = new List<Action>();

            int step = 0;

            if ( string.IsNullOrEmpty ( script.UserName ) )
                throw new ScriptUserNameMissingException ();

            Mobile.LogUserPath = script.UserName;
            Mobile.EventPath   = script.UserName;
            Mobile.NodePath    = script.UserName;

            // Using invalid log file/path
            if ( string.IsNullOrEmpty ( script.LogFile ) ||
                 ! Regex.IsMatch ( script.LogFile, @"^[a-zA-Z_][a-zA-Z0-9_-]*.xml$" ) )
                throw new ScriptLogfileInvalidException ();

            else
            {
                ActionType type;
                foreach ( Xml.ScriptAction scriptAction in script.Actions )
                {
                    // Action string is not present in ActionType enum
                    if ( ! Enum.TryParse<ActionType> ( scriptAction.Type, out type ) )
                        throw new ScriptActionTypeInvalidException ( scriptAction.Type );
                
                    Action new_action = new Action ( serial_device, type, script.UserName, script.LogFile );
                    Type   actionType = scriptAction.GetType ();
    
                    Parameter.ParameterType paramTypeToAdd;
                    foreach ( PropertyInfo propertyInfo in scriptAction.GetType().GetProperties () )
                    {
                        var property   = actionType.GetProperty ( propertyInfo.Name );
                        var paramValue = property.GetValue ( scriptAction, null );
                        if ( paramValue is null )
                            continue;
                        
                        Type valueType = paramValue.GetType ();
                    
                        if ( valueType.Name.ToLower ().Contains ( "actionparameter" ) )
                        {
                            // The parameter name is not listed in the ParameterType enumeration
                            if ( ! Enum.TryParse<Parameter.ParameterType> ( propertyInfo.Name, out paramTypeToAdd ) )
                                continue;
                        
                            List<ActionParameter> list = new List<ActionParameter> ();
                            
                            // If the parameter is an Array is a field for
                            // a port, and if not is a field for the MTU
                            if ( ! paramValue.GetType ().IsArray )
                                 list.Add      ( ( ActionParameter   )paramValue );
                            else list.AddRange ( ( ActionParameter[] )paramValue );

                            foreach ( ActionParameter aParam in list )
                                new_action.AddParameter (
                                    new Parameter (
                                        paramTypeToAdd,
                                        aParam.Value,
                                        aParam.Port ) );
                        }
                    }

                    // Additional parameters
                    Parameter parameter;
                    foreach ( var entry in scriptAction.AdditionalParameters )
                    {
                        parameter = new Parameter ( entry.Key, entry.Key, entry.Value );
                        parameter.Optional = true;
                        new_action.AddAdditionalParameter ( parameter );
                    }
    
                    new_action.Order       = step;
                    new_action.OnProgress += Action_OnProgress;
                    new_action.OnFinish   += Action_OnFinish;
                    new_action.OnError    += Action_OnError;
    
                    actions.Add(new_action);
                    step++;
                }
            }
        }

        /// <summary>
        /// Initializes the logic of the action after having parsed and validated the content of the script.
        /// </summary>
        public async Task Run ()
        {
            await actions.ToArray()[0].Run ();
        }

        private void Action_OnProgress (
            object sender,
            Delegates.ProgressArgs e )
        {
            OnProgress ( sender, e );
        }

        private async Task Action_OnFinish (
            object sender,
            Delegates.ActionFinishArgs args )
        {
            Action act = (Action)sender;

            if (act.Order < (actions.Count - 1))
            {
                await onStepFinish(act, args);
                await actions.ToArray()[act.Order + 1].Run();
            }
            else
            {
                await OnFinish(act, args);
            }
        }

        private void Action_OnError ()
        {
            this.OnError ();
        }
    }
}
