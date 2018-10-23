using System;
using System.Collections.Generic;
using System.Text;

namespace MTUComm
{
    public class ActionResult
    {
        private List<Parameter> parameters;

        private List<ActionResult> ports;


        public ActionResult()
        {
            parameters  = new List<Parameter>();
            ports = new List<ActionResult>();
        }

        public void AddParameter(Parameter parameter)
        {
            parameters.Add(parameter);
        }

        public Parameter[] getParameters()
        {
            return parameters.ToArray();
        }


        public void addPort(ActionResult port)
        {
            ports.Add(port);
        }

        public ActionResult[] getPorts()
        {
            return ports.ToArray();
        }


    }
}
