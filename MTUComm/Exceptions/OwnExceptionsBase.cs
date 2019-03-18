using System;

namespace MTUComm.Exceptions
{
    public class OwnExceptionsBase : Exception
    {
        private string message;
        public override string Message { get { return message; }  }
        
        public int Port { private set; get; }

        public OwnExceptionsBase ()
        {
            this.message = string.Empty;
            this.Port    = 1;
        }
        
        public OwnExceptionsBase (
            string message,
            int    port )
        {
            this.message = message;
            this.Port    = port;
        }
    }
}
