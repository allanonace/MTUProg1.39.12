namespace MTUComm.Exceptions
{
    public class CustomMethodNotExistException : OwnExceptionsBase
    {
        public CustomMethodNotExistException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class MemoryMapParseXmlException : OwnExceptionsBase
    {
        public MemoryMapParseXmlException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class MemoryMapXmlValidationException : OwnExceptionsBase
    {
        public MemoryMapXmlValidationException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class MemoryRegisterNotExistException : OwnExceptionsBase
    {
        public MemoryRegisterNotExistException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class OverloadEmptyCustomException : OwnExceptionsBase
    {
        public OverloadEmptyCustomException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class SetMemoryFormatException : OwnExceptionsBase
    {
        public SetMemoryFormatException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class SetMemoryTypeLimitException : OwnExceptionsBase
    {
        public SetMemoryTypeLimitException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
}
