using System.Globalization;
using BL.Extensions;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using DAL.Models;

namespace BL.DataLoading;

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

    public CsvFile LoadCsvFile(string path) {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = _delimiter, BadDataFound = null, MissingFieldFound = null};
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, config);

        csv.Read();
        csv.ReadHeader();

        var lines = csv.GetRecords<CsvLine>().Where(x => !string.IsNullOrEmpty(x.Company)).ToList();
        return GetCsvFile(lines);
    }

    private CsvFile GetCsvFile(IEnumerable<CsvLine> csvLines) {
        var file = new CsvFile();
        if (csvLines.Any())
            file.Date = csvLines.ElementAt(0).Date.GetDateTime();

        foreach (var csvLine in csvLines) {
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
