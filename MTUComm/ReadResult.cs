using System;
using System.Collections.Generic;
using System.Text;

namespace MTUComm
{
    public class ReadResult
    {
        private List<Parameter> parameters;

        private List<ReadResult> ports;


        public ReadResult()
        {
            parameters  = new List<Parameter>();
            ports = new List<ReadResult>();
        }

        public void AddParameter(Parameter parameter)
        {
            parameters.Add(parameter);
        }

        public Parameter[] getParameters()
        {
            return parameters.ToArray();
        }


        public void addPort(ReadResult port)
        {
            ports.Add(port);
        }

        public ReadResult[] getPorts()
        {
            return ports.ToArray();
        }


    }
}
