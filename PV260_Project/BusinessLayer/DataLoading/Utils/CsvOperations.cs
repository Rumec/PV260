using System.Globalization;
using BusinessLayer.Exceptions;
using BusinessLayer.Extensions;
using CsvHelper;
using DataLayer.Models;

namespace BusinessLayer.DataLoading.Utils;

public static class CsvOperations
{
    public static DataSet GetCsvFile(IEnumerable<CsvLine> dataLines)
    {
        var file = new DataSet();
        if (dataLines.Any())
            file.Date = dataLines.ElementAt(0).Date.GetDateTime();

        foreach (var csvLine in dataLines)
        {
            var holding = new Holding();
            holding.Fund = csvLine.Fund;
            holding.Company = csvLine.Company;
            holding.Ticker = csvLine.Ticker;
            holding.Cusip = csvLine.Cusip;
            holding.Shares = long.Parse(csvLine.Shares.Replace(",", ""));
            csvLine.MarketValue.GetCurrencyAndValue().Deconstruct(out var currency, out var value);
            holding.Currency = currency;
            holding.MarketValue = value;
            holding.Weight = double.Parse(csvLine.Weight.Remove(csvLine.Weight.Length - 1),
                CultureInfo.InvariantCulture);
            file.Holdings.Add(holding);
        }

        return file;
    }

    public static DataSet ParseCsvData(string path, CsvReader csv)
    {
        try
        {
            csv.Read();
            csv.ReadHeader();

            var lines = csv.GetRecords<CsvLine>().Where(x => !string.IsNullOrEmpty(x.Company)).ToList();
            return GetCsvFile(lines);
        }
        catch (Exception e)
        {
            throw new DataLoaderException($"Error while parsing csv file '{path}'", e);
        }
    }
}
