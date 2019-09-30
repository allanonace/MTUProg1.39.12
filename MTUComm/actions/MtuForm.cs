using System;
using System.Collections.Generic;
using System.Dynamic;
using Library;
using Xml;
using System.Linq;
using Library.Exceptions;

namespace MTUComm.actions
{
    public class MtuForm : DynamicObject
    {
        #region Attributes

        private Dictionary<string,Parameter> dictionary;
        protected List<Parameter> additionalParams;
        public Mtu mtu { get; }
        public dynamic map;

        #endregion

        public Parameter[] AdditionalParameters
        {
            get { return this.additionalParams.ToArray (); }
        }

        #region Initialization

        public MtuForm ( Mtu mtu )
        {
            this.dictionary = new Dictionary<string,Parameter> ();
            this.additionalParams = new List<Parameter> ();
            //this.conditions = new Conditions ( mtu );
            //this.global  = Singleton.Get.Configuration.Global;
            this.mtu     = mtu;
        }

        #endregion

        #region Set and Get Parameter

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!this.dictionary.ContainsKey(binder.Name))
            {
                this.dictionary[binder.Name] = (Parameter)value;
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

            // IF PARAMETER NOT EXIST FOR THE MOMENT RETURN EMPTY/NEW PARAMETER
            // AND SET VALUE TO EMPTY STRING
            result = new Parameter ();
            return true;

            //throw new Exception();
        }

        #endregion

        #region Parameters

        public Parameter AddParameter (string paramId, string customParameter, string customDisplay, dynamic value, int port, string source = "" )
        {
            Parameter param = new Parameter ( customParameter, customDisplay, value, source, port );
            this.dictionary[paramId] = param;
            
            return param;
        }

        public void AddParameter ( Parameter parameter )
        {
            this.dictionary[ parameter.Type.ToString () ] = parameter;
        }

        public void AddAdditionalParameter ( string id, dynamic value, int port = 0 )
        {
            if ( ! this.additionalParams.Any ( p => p.CustomDisplay.Equals ( id ) ) )
                this.additionalParams.Add ( new Parameter ( id, id, value, "", port ) );
            else
                throw new SameParameterRepeatScriptException ();
        }

        public Parameter FindParameterById(string paramId)
        {
            return GetParameter(paramId);
        }

        public Parameter[] GetParameters ()
        {
            return new List<Parameter>(this.dictionary.Values).ToArray();
        }

        public Parameter GetParameter ( string paramId )
        {
            if ( this.dictionary.ContainsKey ( paramId ) )
                return this.dictionary[ paramId ];
            
            return null;
        }

        public bool ContainsParameter ( string paramId )
        {
            return this.dictionary.ContainsKey ( paramId );
        }
        
        public void RemoveParameter ( string paramId )
        {
            if ( this.dictionary.ContainsKey ( paramId ) )
                this.dictionary.Remove ( paramId );
        }

        public void RemoveParameters ()
        {
            this.dictionary.Clear ();
            this.additionalParams.Clear ();
        }

        #endregion
    }
}
