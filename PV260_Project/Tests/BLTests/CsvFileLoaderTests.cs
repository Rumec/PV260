using System;
using BL.DataLoading;
using DAL.Models;
using NUnit.Framework;

namespace Tests.BLTests;

[TestFixture]
public class CsvFileLoaderTests
{
    private CsvFile _testFile;
    private CsvFileLoader _testLoader;
    private string _pathToFile;
    
    [SetUp]
    public void Setup()
    {
        _testLoader = new CsvFileLoader(",");
        _pathToFile = "../../../TestFiles/csv_file_loader_test.csv";
        _testFile = new CsvFile();
        _testFile.Date = new DateTime(2022, 3, 21);
        _testFile.Holdings.Add(CreateTeslaHolding());
        _testFile.Holdings.Add(CreateHealthHolding());
        _testFile.Holdings.Add(CreateRokuHolding());
    }


    [Test]
    public void TestLoadCsvFile()
    {
        var file = _testLoader.LoadCsvFile(_pathToFile);
        
        Assert.That(file.Equals(_testFile));
    }
    

    private static Holding CreateTeslaHolding()
    {
        Holding teslaTestHolding = new Holding();
        teslaTestHolding.Company = "TESLA INC";
        teslaTestHolding.Currency = "$";
        teslaTestHolding.Cusip = "88160R101";
        teslaTestHolding.Fund = "ARKK";
        teslaTestHolding.MarketValue = 1228003997.14;
        teslaTestHolding.Shares = 1356326;
        teslaTestHolding.Ticker = "TSLA";
        teslaTestHolding.Weight = 9.66;
        return teslaTestHolding;
    }
    
    private static Holding CreateHealthHolding()
    {
        Holding healthTestHolding = new Holding();
        healthTestHolding.Company = "TELADOC HEALTH INC";
        healthTestHolding.Currency = "$";
        healthTestHolding.Cusip = "87918A105";
        healthTestHolding.Fund = "ARKK";
        healthTestHolding.MarketValue = 854796576.32;
        healthTestHolding.Shares = 12395542;
        healthTestHolding.Ticker = "TDOC";
        healthTestHolding.Weight = 6.72;
        return healthTestHolding;
    }
    private static Holding CreateRokuHolding()
    {
        Holding healthTestHolding = new Holding();
        healthTestHolding.Company = "ROKU INC";
        healthTestHolding.Currency = "$";
        healthTestHolding.Cusip = "77543R102";
        healthTestHolding.Fund = "ARKK";
        healthTestHolding.MarketValue = 805584886.70;
        healthTestHolding.Shares = 6452422;
        healthTestHolding.Ticker = "ROKU";
        healthTestHolding.Weight = 6.34;
        return healthTestHolding;
    }
}