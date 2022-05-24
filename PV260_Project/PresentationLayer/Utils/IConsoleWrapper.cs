namespace PresentationLayer.Utils;

/// <summary>
/// Serves as a wrapper for System.Console IO methods, ReadLine and WriteLine, allowing them to be mocked.
/// </summary>
public interface IConsoleWrapper
{
    string? ReadLine();
    void WriteLine(string s);
}