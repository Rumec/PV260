namespace BusinessLayer.Exceptions;

[Serializable]
public class EmailSenderException : Exception
{
    public EmailSenderException() {}

    public EmailSenderException(string message) : base(message) {}
    
    public EmailSenderException(string message, Exception innerException) : base(message, innerException) {}
}