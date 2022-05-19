using DataLayer.Models;

namespace BusinessLayer.DataLoading;

public interface IDataDownloader
{
    public Task<DataSet> LoadCsvFile(string path, string delimiter = ",");
}