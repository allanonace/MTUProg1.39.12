using System;

namespace Library.Exceptions
{
    public class OwnExceptionsBase : Exception
    {
        // Used to replace _var_ entries in error messages from the dictionary in the XML file
        private string varMessage;
        private string varMessagaPopup;
        
        public string VarMessage { get { return varMessage; }  }
        public string VarMessagePopup { get { return varMessagaPopup; }  }
        public int Port { private set; get; }
        
        public OwnExceptionsBase (
            string varMessage = "",
            int    port = 1,
            string varMessagaPopup = "" )
        {
            this.varMessage      = varMessage;
            this.varMessagaPopup = varMessagaPopup;
            this.Port            = port;
        }

    }

    public class OwnSpecialExceptionsBase<T> : OwnExceptionsBase
    {
        private object response;

        public T Response { get { return ( T )this.response; } }

        public OwnSpecialExceptionsBase (
            T response )
        {
            this.response = response;
        }
    }
}
