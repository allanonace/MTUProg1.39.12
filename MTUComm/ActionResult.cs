using System;
using System.Collections.Generic;
using System.Linq;
using Xml;

using ActionType = MTUComm.Action.ActionType;

namespace MTUComm
{
    public class ActionResult
    {
        private ActionType         actionType;
        private List<Parameter>    parameters;
        private List<ActionResult> ports;

        public ActionType ActionType
        {
            get { return this.actionType; }
            set { this.actionType = value; }
        }

        public ActionResult (
            ActionType actionType = ActionType.ReadMtu )
        {
            this.actionType = actionType;
            this.parameters = new List<Parameter> ();
            this.ports      = new List<ActionResult> ();
        }

        public void AddParameter (
            Parameter parameter )
        {
            parameters.Add(parameter);
        }

        public Parameter[] getParameters ()
        {
            return parameters.ToArray();
        }

        public int NumParameters
        {
            get { return this.parameters.Count; }
        }

        public void addPort (
            ActionResult port )
        {
            ports.Add(port);
        }

        public ActionResult[] getPorts ()
        {
            return ports.ToArray ();
        }

        public int NumPorts
        {
            get { return this.ports.Count; }
        }

        public Parameter getParameterByTag (
            string tag,
            string source,
            int    port = 0 )
        {
            // The first element that matches the conditions defined by the specified predicate,
            // if found, returns the default value for type T
            return parameters.Find ( param => param.CustomParameter.Equals ( tag ) &&
                                              ( param.Source.Equals ( source ) || string.IsNullOrEmpty ( source ) ) &&
                                              param.Port == port );
        }
    
        public void SimulateRddInPortTwoIfNeeded (
            Mtu mtu )
        {
            if ( ! mtu.TwoPorts &&
                 mtu.Port1.IsSetFlow )
            {
                this.ports.Add ( this.ClonePort1 () );
                this.ports[ 0 ] = this.CreateFalsePort1 ();
            }
        }

        private ActionResult ClonePort1 ()
        {
            ActionResult firstToSecond = this.ports[ 0 ];

            foreach ( Parameter param in firstToSecond.parameters )
                param.setPort ( 1 ); // Second port

            return firstToSecond;
        }

        private ActionResult CreateFalsePort1 ()
        {
            ActionResult port1 = new ActionResult ();
            port1.AddParameter ( new Parameter ( "Status", "Status", "Disabled", string.Empty, 0 ) );

            return port1;
        }
    }
}
