using DataLayer.Models;

namespace BusinessLayer.DataLoading;

public interface IDataLoader
{
    /// <summary>
    /// Loads the CSV file
    /// </summary>
    /// <param name="path">Path to the CSV</param>
    /// <returns>Object which represents the file</returns>
    public DataSet LoadCsvFile(string path, string delimiter = ",");
}