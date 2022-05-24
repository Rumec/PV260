namespace PresentationLayer.Utils;

public class ConsoleWrapper : IConsoleWrapper
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(string s)
    {
        Console.WriteLine(s);
    }
}