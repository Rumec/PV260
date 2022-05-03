namespace BusinessLayer.Logging;

public interface ILogger
{
    /// <summary>
    /// Logs the provided message
    /// </summary>
    /// <param name="message"></param>
    void Log(string message);
}