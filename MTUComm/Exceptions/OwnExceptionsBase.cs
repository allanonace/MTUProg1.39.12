using System;

namespace MTUComm.Exceptions
{
    public class OwnExceptionsBase : Exception
    {
        private string message;
        
        public override string Message { get { return message; }  }
        
        public int Port { private set; get; }
        
        public OwnExceptionsBase (
            string message = "",
            int    port    = 1 )
        {
            this.message = message;
            this.Port    = port;
        }
    }
}
