namespace PresentationLayer.Utils;

public class ConsoleIoWrapper : IConsoleIoWrapper
{
    public string? GetInput()
    {
        return Console.ReadLine();
    }

    public void ShowMessage(string s)
    {
        Console.WriteLine(s);
    }
}