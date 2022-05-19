namespace BusinessLayer.Exceptions;

public class DataSetAlreadyExistsException : Exception
{
    public DataSetAlreadyExistsException() : base("Data set already exists.")
    {
    }
}