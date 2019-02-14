using System;

namespace MTUComm.Exceptions
{
    public class OwnExceptionsBase : Exception
    {
        public string Message { private set; get; }

        public OwnExceptionsBase () { }
        
        public OwnExceptionsBase ( string message )
        {
            this.Message = message;
        }
    }
}
