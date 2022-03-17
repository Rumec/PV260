namespace DAL;

public class DataLoader : IDataLoader
{
    private readonly string _delimiter;

    public DataLoader(string delimiter)
    {
        _delimiter = delimiter;
    }

    public string[][] LoadCsvFile(string path)
    {
        var lines = File
            .ReadLines(path)
            .Select(line => line.Split(_delimiter))
            .ToArray();
        return lines;
    }
}
