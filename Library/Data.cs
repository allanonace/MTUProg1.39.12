using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Linq;

namespace Library
{
    public class Data : DynamicObject
    {
        private static Data instance;
        
        private Dictionary<string,( dynamic Value,bool ForReset )> dictionary;
        
        public dynamic this[ string id ]
        {
            get
            {
                if ( this.dictionary.ContainsKey ( id ) )
                    return this.dictionary[ id ].Value;

                // Selected dynamic member not exists
                return null;
            }
        }

        private Data ()
        {
            this.dictionary = new Dictionary<string,( dynamic value, bool forReset )> ();
        }
        
        public override bool TrySetMember ( SetMemberBinder binder, object value )
        {
            this.AddElement ( binder.Name, value, false );

            return true;
        }
        
        private void AddElement (
            string name,
            object value,
            bool forReset )
        {
            if ( ! this.dictionary.ContainsKey ( name ) )
                this.dictionary.Add ( name, ( value, forReset ) );
            else
                this.dictionary[ name ] = ( value, forReset );
        }
        
        public override bool TryGetMember ( GetMemberBinder binder, out object result )
        {
            if ( this.dictionary.ContainsKey ( binder.Name ) )
                 result = this.dictionary[ binder.Name ].Value;
            else result = null;
            
            return true;
        }
        
        public static dynamic Get
        {
            get
            {
                if ( instance == null )
                    instance = new Data ();
                
                return instance;
            }
        }

        public static dynamic Set (
            string name,
            object value )
        {
            Data d = Get;
            
            return d.GetType ()
               .GetMethod ( "AddElement", BindingFlags.NonPublic | BindingFlags.Instance )
               .Invoke ( d, new object[] { name, value, false } );
        }

        public static dynamic SetTemp (
            string name,
            object value )
        {
            Data d = Get;
            
            return d.GetType ()
               .GetMethod ( "AddElement", BindingFlags.NonPublic | BindingFlags.Instance )
               .Invoke ( d, new object[] { name, value, true } );
        }

        public static void ResetAll ()
        {
            Data d = Get;

            d.dictionary.Clear ();
        }

        public static void Reset ()
        {
            Data d = Get;

            // Regenerate dictionary only with entries without flag activated
            d.dictionary = d.dictionary
                .Where ( entry => ! entry.Value.ForReset )
                .ToDictionary ( entry => entry.Key, entry => entry.Value );
        }

        public static bool Contains (
            string name )
        {
            Data d = Get;

            return d.dictionary.ContainsKey ( name );
        }
    }
}
