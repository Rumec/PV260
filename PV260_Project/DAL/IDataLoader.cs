namespace DAL;

public interface IDataLoader
{
    /// <summary>
    /// Loads the CSV file
    /// </summary>
    /// <param name="path">Path to the CSV</param>
    /// <returns>CSV in a form of 2D array</returns>
    public string[][] LoadCsvFile(string path);
}