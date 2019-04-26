using System;
using System.Collections.Generic;
using System.Dynamic;
using Library;
using Xml;

namespace MTUComm.actions
{
    public class MtuForm : DynamicObject
    {
        #region Attributes

        private Dictionary<string,Parameter> dictionary;
        public static MTUBasicInfo mtuBasicInfo { get; private set; }
        public Mtu mtu { get; }
        public dynamic map;

        #endregion

        #region Initialization

        public MtuForm ( Mtu mtu )
        {
            this.dictionary = new Dictionary<string,Parameter> ();
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

        public static void SetBasicInfo(MTUBasicInfo mtuBasicInfo)
        {
            MtuForm.mtuBasicInfo = mtuBasicInfo;
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
        }

        #endregion
    }
}
