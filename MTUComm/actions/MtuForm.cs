using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Xml;
using System.Reflection;

namespace MTUComm.actions
{
    public class MtuForm : DynamicObject
    {
        #region Conditions

        public enum FROM { GLOBALS, MTU }

        public sealed class Conditions
        {
            public class Conditions_Base : DynamicObject
            {
                public Dictionary<string,bool> dictionary { get; }
                private Global globals;
                private Mtu mtu;
                private FROM from;

                public Conditions_Base ( Mtu mtu, FROM from )
                {
                    this.dictionary = new Dictionary<string, bool>();
                    this.globals = Configuration.GetInstance().GetGlobal();
                    this.mtu = mtu;
                    this.from = from;
                }

                public override bool TrySetMember(SetMemberBinder binder, object value)
                {
                    if ( value is bool )
                    {
                        this.dictionary[binder.Name] = (bool)value;
                        return true;
                    }

                    throw new Exception();
                }

                public override bool TryGetMember(GetMemberBinder binder, out object result)
                {
                    if (this.dictionary.ContainsKey(binder.Name))
                         result = this.dictionary[binder.Name];
                    else result = false;

                    return true;
                }

                public void AddCondition ( string id_condition )
                {
                    bool value = false;
                    if ( from == FROM.GLOBALS )
                         value = (bool)this.globals.GetType().GetProperty( id_condition ).GetValue( this.globals );
                    else value = (bool)this.mtu    .GetType().GetProperty( id_condition ).GetValue( this.mtu     );

                    this.dictionary[id_condition] = value;
                }
            }

            public Conditions_Base mtu { get; }
            public Conditions_Base globals { get; }

            public Conditions ( Mtu mtu )
            {
                this.mtu     = new Conditions_Base ( mtu, FROM.MTU     );
                this.globals = new Conditions_Base ( mtu, FROM.GLOBALS );
            }
        }

        #endregion

        #region Attributes

        private Dictionary<string,Parameter> dictionary;
        public Conditions conditions { get; private set; }
        public static MTUBasicInfo mtuBasicInfo { get; private set; }
        private Mtu mtu;

        #endregion

        #region Initialization

        public MtuForm(Mtu mtu)
        {
            this.mtu = mtu;
            this.dictionary = new Dictionary<string, Parameter>();
            this.conditions = new Conditions(mtu);
        }

        #endregion

        #region Set and Get

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

            throw new Exception();
        }

        public static void SetBasicInfo(MTUBasicInfo mtuBasicInfo)
        {
            MtuForm.mtuBasicInfo = mtuBasicInfo;
        }

        #endregion

        #region Parameters

        public void AddParameter (string paramId, string customParameter, string customDisplay, dynamic value)
        {
            this.dictionary[paramId] =
                new Parameter(customParameter, customDisplay, value);
        }

        public Parameter FindParameterById(string paramId)
        {
            return GetParameter(paramId);
        }

        public Parameter[] GetParameters()
        {
            return new List<Parameter>(this.dictionary.Values).ToArray();
        }

        public Parameter GetParameter(string paramId)
        {
            if (this.dictionary.ContainsKey(paramId))
                return this.dictionary[paramId];
            
            throw new Exception();
        }

        #endregion
    }
}
