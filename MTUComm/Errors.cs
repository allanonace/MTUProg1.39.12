using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Xml;

namespace MTUComm
{
    public class Errors
    {
        #region Nested class

        public class ErrorArgs : EventArgs
        {
            public int    Status     { get; private set; }
            public String Message    { get; private set; }
            public String LogMessage { get; private set; }

            public ErrorArgs ( int status, String message = "", String logMessage = "" )
            {
                Status     = status;
                Message    = message;
                LogMessage = logMessage;
            }

            public ErrorArgs ( int status, String[] messages ) : this ( status, messages[ 0 ], messages[ 1 ] ) { }
        }

        #endregion

        #region Attributes

        private const string FILE_NAME = "Errors.xml";

        private static Errors instance;

        #endregion

        #region Initialization

        private Dictionary<int,string[]> messages;

        public ErrorArgs this[ int status ]
        {
            get
            {
                string[] msgs = messages[ status ];
                return new ErrorArgs ( status, msgs[ 0 ], msgs[ 1 ] );
            }
        }

        public Errors ( string pathUnityTest = "" )
        {
            bool isUnityTest = ! string.IsNullOrEmpty ( pathUnityTest );

            Configuration config = Configuration.GetInstance ( isUnityTest, pathUnityTest );

            string path = Path.Combine (
                ( ( ! isUnityTest ) ? config.GetBasePath() : pathUnityTest ), FILE_NAME );

            XmlSerializer serializer = new XmlSerializer ( typeof ( ErrorList ) );

            using ( TextReader reader = new StreamReader ( path ) )
            {
                ErrorList list = Validations.DeserializeXml<ErrorList> ( reader );

                this.messages = new Dictionary<int,string[]> ();

                if ( list.Errors != null )
                    foreach ( Error xmlError in list.Errors )
                        this.messages.Add ( xmlError.Status, new string[] { xmlError.Message, xmlError.LogMessage } );
            }
        }

        #endregion

        public ErrorArgs Get ( int status, params string[] args )
        {
            if ( this.messages.ContainsKey ( status ) )
            {
                string[] messages = this.messages[ status ];
                this.ReplaceArguments ( ref messages[ 0 ], args );
                this.ReplaceArguments ( ref messages[ 1 ], args );

                return new ErrorArgs ( status, messages );
            }
            return new ErrorArgs ( -1 );
        }

        private void ReplaceArguments ( ref string message, params string[] args )
        {
            int i = 1;
            foreach ( string arg in args )
            {
                string id = "_" + i++ + "_";

                if ( message.Contains ( id ) )
                    message = message.Replace ( id, arg );
            }
        }
    }
}
