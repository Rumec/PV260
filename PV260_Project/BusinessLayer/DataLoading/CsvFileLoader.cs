using System.Globalization;
using BusinessLayer.Extensions;
using BusinessLayer.Exceptions;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using DataLayer.Models;

namespace BusinessLayer.DataLoading;

public class CsvLine
{
    [Name("date")]
    public string Date { get; set; }
    [Name("fund")]
    public string Fund { get; set; }
    [Name("company")]
    public string Company { get; set; }
    [Name("ticker")]
    public string Ticker { get; set; }
    [Name("cusip")]
    public string Cusip { get; set; }
    [Name("shares")]
    public string Shares { get; set; }
    [Name("market value ($)")]
    public string MarketValue { get; set; }
    [Name("weight (%)")]
    public string Weight { get; set; }
}

public class CsvFileLoader : IDataLoader
{
    private readonly string _delimiter;

    public CsvFileLoader(string delimiter)
    {
        _delimiter = delimiter;
    }

    public DataSet LoadCsvFile(string path) {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = _delimiter, BadDataFound = null, MissingFieldFound = null};
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, config);

        try
        {
            csv.Read();
            csv.ReadHeader();
            
            var lines = csv.GetRecords<CsvLine>().Where(x => !string.IsNullOrEmpty(x.Company)).ToList();
            return GetCsvFile(lines);
        }
        catch (Exception e)
        {
            throw new DataLoaderException($"Error while loading csv file '{path}'", e);
        }
    }

    private DataSet GetCsvFile(IEnumerable<CsvLine> dataLines) {
        var file = new DataSet();
        if (dataLines.Any())
            file.Date = dataLines.ElementAt(0).Date.GetDateTime();

        foreach (var csvLine in dataLines) {
            var holding = new Holding();
            holding.Fund = csvLine.Fund;
            holding.Company = csvLine.Company;
            holding.Ticker = csvLine.Ticker;
            holding.Cusip = csvLine.Cusip;
            holding.Shares = long.Parse(csvLine.Shares.Replace(",", ""));
            csvLine.MarketValue.GetCurrencyAndValue().Deconstruct(out var currency, out var value);
            holding.Currency = currency;
            holding.MarketValue = value;
            holding.Weight = double.Parse(csvLine.Weight.Remove(csvLine.Weight.Length - 1), CultureInfo.InvariantCulture);
            file.Holdings.Add(holding);
        }

        return file;
    }
}
