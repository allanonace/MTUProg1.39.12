namespace Library.Exceptions
{
    public class LexiReadingException : OwnExceptionsBase
    {
        public LexiReadingException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }
    
    public class LexiWritingException : OwnExceptionsBase
    {
        public LexiWritingException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class LexiWritingAckException : OwnExceptionsBase
    {
        public LexiWritingAckException ( string message = "", int port = 1, string messagePopup = "" ) : base ( message, port, messagePopup ) { }
    }

    public class LexiWritingEncryptionException<T> : OwnSpecialExceptionsBase<T>
    {
        public LexiWritingEncryptionException ( T response ) : base ( response ) { }
    }
}
