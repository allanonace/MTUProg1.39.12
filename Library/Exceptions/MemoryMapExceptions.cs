namespace Library.Exceptions
{
    public class CustomMethodNotExistException : OwnExceptionsBase
    {
        public CustomMethodNotExistException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when an error occurs during dynamic generation of a <see cref="MTUComm.MemoryMap.MemoryMap"/>.
    /// </summary>
    public class MemoryMapParseXmlException : OwnExceptionsBase
    {
        public MemoryMapParseXmlException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class MemoryMapXmlValidationException : OwnExceptionsBase
    {
        public MemoryMapXmlValidationException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    /// <summary>
    /// Exception thrown when trying to work with a dynamic member not registered in the <see cref="MTUComm.MemoryMap.MemoryMap"/>.
    /// <para>
    /// See <see cref="MTUComm.MemoryMap.AMemoryMap.TrySetMember ()"/>.
    /// </para>
    /// <para>
    /// See <see cref="MTUComm.MemoryMap.AMemoryMap.TryGetMember ()"/>.
    /// </para>
    /// </summary>
    public class MemoryRegisterNotExistException : OwnExceptionsBase
    {
        public MemoryRegisterNotExistException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    /// <summary>
    /// Exception thrown when trying modify the value of a <see cref="MTUComm.MemoryMap.MemoryOverload"/>, that are always read-only.
    /// <para>
    /// See <see cref="MTUComm.MemoryMap.AMemoryMap.TrySetMember ()"/>.
    /// </para>
    /// </summary>
    public class MemoryOverloadsAreReadOnly : OwnExceptionsBase
    {
        public MemoryOverloadsAreReadOnly ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
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

    public class MemoryRegisterSetValueException : OwnExceptionsBase
    {
        public MemoryRegisterSetValueException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
}
