using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace MTUComm
{
    public sealed class Singleton : DynamicObject
    {
        private static Singleton instance;
        
        private Dictionary<string,dynamic> dictionary;
        
        private Singleton ()
        {
            this.dictionary = new Dictionary<string,dynamic> ();
        }
        
        public override bool TrySetMember ( SetMemberBinder binder, object value )
        {
            this.AddClass ( binder.Name, value );
            
            return true;
        }
        
        private void AddClass ( string name, object value )
        {
            if ( value.GetType ().GetTypeInfo ().IsClass &&
                 ! this.dictionary.ContainsKey ( name ) )
                this.dictionary.Add ( name, value );
        }

        public override bool TryGetMember ( GetMemberBinder binder, out object result )
        {           
            if ( this.dictionary.ContainsKey ( binder.Name ) )
            {
                result = this.dictionary[ binder.Name ];
                return true;
            }

            result = null;
            return false;
        }
        
        public static bool Has<T> ()
        where T : class
        {
            Singleton s = Get;
            
            return s.dictionary.ContainsKey ( typeof ( T ).Name );
        }
        
        public static dynamic Get
        {
            get
            {
                if ( instance == null )
                    instance = new Singleton ();
                
                return instance;
            }
        }

        public static dynamic Set
        {
            set
            {
                // NOTA: No se puede poner "Get.GetType...Invoke ( instance,..."
                Singleton s = Get;
                
                s.GetType()
                   .GetMethod ( "AddClass", BindingFlags.NonPublic | BindingFlags.Instance )
                   .Invoke ( s, new object[] { value.GetType ().Name, value } );
            }
        }
        
        public static bool Remove<T> (
            string customName = "" )
        where T : class
        {
            Singleton s = Get;
            
            if ( string.IsNullOrEmpty ( customName ) )
                customName = typeof ( T ).Name;
            
            if ( s.dictionary.ContainsKey ( customName ) )
            {
                s.dictionary.Remove ( customName );
                return true;
            }
            return false;
        }
        
        public static void SetCustom ( string name, object value )
        {
            Singleton s = Get;

            s.GetType()
                .GetMethod ( "AddClass", BindingFlags.NonPublic | BindingFlags.Instance )
                .Invoke ( s, new object[] { name, value } );
        }
    }
}
