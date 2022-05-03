namespace BusinessLayer.Exceptions;

[Serializable]
public class DataWriterException : Exception
{
    public DataWriterException() {}

    public DataWriterException(string message) : base(message) {}
    
    public DataWriterException(string message, Exception innerException) : base(message, innerException) {}
}