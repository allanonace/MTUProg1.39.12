using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace MTUComm.actions
{
    public class MtuForm : DynamicObject
    {
        private Dictionary<string, Parameter> dictionary;

        public MtuForm()
        {
            this.dictionary = new Dictionary<string, Parameter>();
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if ( ! this.dictionary.ContainsKey(binder.Name))
            {
                this.dictionary[binder.Name] = (Parameter) value;
                return true;
            }

            throw new Exception();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (this.dictionary.ContainsKey(binder.Name))
            {
                result = this.dictionary[binder.Name];
                return true;
            }

            throw new Exception();
        }

        public void AddParameter (string id_parameter, string custom_parameter, string custom_display, dynamic value)
        {
            this.dictionary[id_parameter] =
                new Parameter(custom_parameter, custom_display, value);
        }

        public Parameter FindById(string paramId)
        {
            return GetParameter(paramId);
        }

        public Parameter[] GetParameters()
        {
            return new List<Parameter>(this.dictionary.Values).ToArray();
        }

        public Parameter GetParameter(string id)
        {
            if (this.dictionary.ContainsKey(id))
                return this.dictionary[id];
            
            throw new Exception();
        }
    }
}
