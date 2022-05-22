using System.Globalization;
using System.Net;
using BusinessLayer.DataLoading.Utils;
using BusinessLayer.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using DataLayer.Models;

namespace BusinessLayer.DataLoading;

public class CsvFileDownloader : IDataDownloader
{
    public async Task<DataSet> LoadCsvFile(string path, string delimiter = ",")
    {
        var client = new HttpClient();
        // NOTE: There must be User-Agent specified, otherwise the request will be rejected
        client.DefaultRequestHeaders.Add("User-Agent", "Chrome/101.0.4951.67");
        var body = "";

        // File download
        try
        {
            var responseMessage = await client.GetAsync(path);
            body = await responseMessage.Content.ReadAsStringAsync();

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new WebException($"Responded with {responseMessage.StatusCode}");
            }
        }
        catch (Exception e)
        {
            throw new DataLoaderException($"Error while downloading csv file from '{path}'", e);
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            { Delimiter = delimiter, BadDataFound = null, MissingFieldFound = null };
        using var reader = new StringReader(body);
        using var csv = new CsvReader(reader, config);

        return CsvOperations.ParseCsvData(path, csv);
    }
}