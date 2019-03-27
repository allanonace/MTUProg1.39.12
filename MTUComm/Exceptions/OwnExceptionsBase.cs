using System;

namespace MTUComm.Exceptions
{
    public class OwnExceptionsBase : Exception
    {
        private string message;
        private string messagaPopup;
        
        public override string Message { get { return message; }  }
        public string MessagePopup { get { return messagaPopup; }  }
        public int Port { private set; get; }
        
        public bool HasMessagePopup
        {
            get { return ! string.IsNullOrEmpty ( this.messagaPopup ); }
        }

        public OwnExceptionsBase (
            string message = "",
            int    port    = 1,
            string messagePopup = "" )
        {
            this.message      = message;
            this.messagaPopup = messagePopup;
            this.Port         = port;
        }
    }
}
