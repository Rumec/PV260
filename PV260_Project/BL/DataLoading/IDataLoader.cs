using DAL.Models;

namespace BL.DataLoading;

public interface IDataLoader
{
    /// <summary>
    /// Loads the CSV file
    /// </summary>
    /// <param name="path">Path to the CSV</param>
    /// <returns>Object which represents the file</returns>
    public CsvFile LoadCsvFile(string path);
}