using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Library
{
    public class Data : DynamicObject
    {
        private static Data instance;
        
        private Dictionary<string,dynamic> dictionary;
        
        public dynamic this[ string id ]
        {
            get
            {
                if ( this.dictionary.ContainsKey ( id ) )
                    return this.dictionary[ id ];

                // Selected dynamic member not exists
                return null;
            }
        }

        private Data ()
        {
            this.dictionary = new Dictionary<string,dynamic> ();
        }
        
        public override bool TrySetMember ( SetMemberBinder binder, object value )
        {
            this.AddElement ( binder.Name, value );
            
            return true;
        }
        
        private void AddElement (
            string name,
            object value )
        {
            if ( ! this.dictionary.ContainsKey ( name ) )
            {
                this.dictionary.Add ( name, value );
                
                Utils.Print ( "Data: Add \"" + name + "\"" );
            }
            else
            {
                this.dictionary[ name ] = value;
                
                Utils.Print ( "Data: Replace \"" + name + "\"" );
            }
        }
        
        public override bool TryGetMember ( GetMemberBinder binder, out object result )
        {           
            if ( this.dictionary.ContainsKey ( binder.Name ) )
                 result = this.dictionary[ binder.Name ];
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
            
            return d.GetType()
               .GetMethod ( "AddElement", BindingFlags.NonPublic | BindingFlags.Instance )
               .Invoke ( d, new object[] { name, value } );
        }
    }
}
