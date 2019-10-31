using System.Collections.Generic;

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

        public void AddParameter(Parameter parameter)
        {
            parameters.Add(parameter);
        }

        public Parameter[] getParameters()
        {
            return parameters.ToArray();
        }

        public int NumParameters
        {
            get { return this.parameters.Count; }
        }

        public void addPort(ActionResult port)
        {
            ports.Add(port);
        }

        public ActionResult[] getPorts()
        {
            return ports.ToArray();
        }

        public int NumPorts
        {
            get { return this.ports.Count; }
        }

        public Parameter getParameterByTag ( string tag, string source, int port = 0 )
        {
            // The first element that matches the conditions defined by the specified predicate,
            // if found, returns the default value for type T
            return parameters.Find ( param => param.CustomParameter.Equals ( tag ) &&
                                              ( param.source.Equals ( source ) || string.IsNullOrEmpty ( source ) ) &&
                                              param.Port == port );
        }
    }
}
