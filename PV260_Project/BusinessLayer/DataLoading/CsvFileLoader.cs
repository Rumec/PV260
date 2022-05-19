using System.Globalization;
using BusinessLayer.DataLoading.Utils;
using BusinessLayer.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using DataLayer.Models;

namespace BusinessLayer.DataLoading;

public class CsvFileLoader : IDataLoader
{
    public DataSet LoadCsvFile(string path, string delimiter = ",")
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            { Delimiter = delimiter, BadDataFound = null, MissingFieldFound = null };
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, config);

        return CsvOperations.ParseCsvData(path, csv);
    }
}