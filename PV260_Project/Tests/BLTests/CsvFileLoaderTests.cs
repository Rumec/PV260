using System;
using BusinessLayer.DataLoading;
using DataLayer.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.BLTests;

[TestFixture]
public class CsvFileLoaderTests
{
    private DataSet _testFile;
    private CsvFileLoader _testLoader;
    private string _pathToFile;
    
    [SetUp]
    public void Setup()
    {
        _testLoader = new CsvFileLoader();
        _pathToFile = "../../../TestFiles/csv_file_loader_test.csv";
        _testFile = new DataSet();
        _testFile.Date = new DateTime(2022, 3, 21);
        _testFile.Holdings.Add(BLTests.TestUtils.CreateTeslaHolding());
        _testFile.Holdings.Add(BLTests.TestUtils.CreateHealthHolding());
        _testFile.Holdings.Add(BLTests.TestUtils.CreateRokuHolding());
    }


    [Test]
    public void TestLoadCsvFile()
    {
        var file = _testLoader.LoadCsvFile(_pathToFile, ",");
        file.Should().BeEquivalentTo(_testFile);
    }
}