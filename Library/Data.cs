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
            Console.WriteLine ( "Data: Init 1" );
        
            this.dictionary = new Dictionary<string,( dynamic value, bool forReset )> ();
            
            Console.WriteLine ( "Data: Init 2" );
        }
        
        public override bool TrySetMember ( SetMemberBinder binder, object value )
        {
            Console.WriteLine ( "Data: TrySetMember 1 " + binder.Name );
        
            this.AddElement ( binder.Name, value, false );
            
            Console.WriteLine ( "Data: TrySetMember 2 " + binder.Name );
            
            return true;
        }
        
        private void AddElement (
            string name,
            object value,
            bool forReset )
        {
            Console.WriteLine ( "Data: AddElement " + name + " [ ForReset: " + forReset + " ]" );
        
            if ( ! this.dictionary.ContainsKey ( name ) )
            {
                Console.WriteLine ( "Data: AddElement 1" );
            
                this.dictionary.Add ( name, ( value, forReset ) );
                
                Utils.Print ( "Data: Add \"" + name + "\"" );
            }
            else
            {
                Console.WriteLine ( "Data: AddElement 2" );
            
                this.dictionary[ name ] = ( value, forReset );
                
                Utils.Print ( "Data: Replace \"" + name + "\"" );
            }
        }
        
        public override bool TryGetMember ( GetMemberBinder binder, out object result )
        {
            Console.WriteLine ( "Data: TryGetMember 1 " + binder.Name );
        
            if ( this.dictionary.ContainsKey ( binder.Name ) )
                 result = this.dictionary[ binder.Name ].Value;
            else result = null;
            
            Console.WriteLine ( "Data: TryGetMember 2 " + binder.Name );
            
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
            object value,
            bool   forReset = false )
        {
            Console.WriteLine ( "Data: Set 1 " + name );
        
            Data d = Get;
            
            Console.WriteLine ( "Data: Set 2 " + name );
            
            return d.GetType()
               .GetMethod ( "AddElement", BindingFlags.NonPublic | BindingFlags.Instance )
               .Invoke ( d, new object[] { name, value, forReset } );
        }

        public static void ResetAll ()
        {
            Data d = Get;

            d.dictionary.Clear ();
        }

        public static void Reset ()
        {
            Console.WriteLine ( "Data: Reset 1" );
        
            Data d = Get;

            Console.WriteLine ( "Data: Reset 2: " + ( d.dictionary is null ) + " " + d.dictionary.Count );
            
            Console.WriteLine ( "Before" );
            foreach ( var entry in d.dictionary )
                Console.WriteLine ( entry.Key );

            // Regenerate dictionary only with entries without flag activated
            d.dictionary = d.dictionary
                .Where ( entry => ! entry.Value.ForReset )
                .ToDictionary ( entry => entry.Key, entry => entry.Value );
            
            Console.WriteLine ( "After" );
            foreach ( var entry in d.dictionary )
                Console.WriteLine ( entry.Key );
            
            Console.WriteLine ( "Data: Reset 3" );
        }
    }
}
