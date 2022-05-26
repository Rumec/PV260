namespace PresentationLayer.Utils;

/// <summary>
/// Serves as a wrapper for IO methods (getting user input and showing/printing messages to the user),
/// allowing these methods to be mocked and tested.
/// </summary>
public interface IIoWrapper
{
    string? GetInput();
    void ShowMessage(string s);
}