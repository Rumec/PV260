using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using BusinessLayer.Writers;
using DataLayer.Models;
using NUnit.Framework;

namespace Tests.BLTests;

public class FileResultWriterTests
{
    private List<HoldingChanges> _changes = new List<HoldingChanges>(); 
    private CultureInfo _culture;
    private DateTime _date;
    private string _holdingsStr;
    private string _filePath;
    private string _separator;

    [SetUp]
    public void Setup()
    {
        _changes.Add(TestUtils.CreateTeslaHoldingChanges());
        _changes.Add(TestUtils.CreateHealthHoldingChanges());
        _changes.Add(TestUtils.CreateRokuHoldingChanges());

        _culture = TestUtils.HoldingsCultureInfo;
        _date = TestUtils.HoldingsDate;
        _holdingsStr = TestUtils.HoldingsStr;
        _filePath = "../../../TestFiles/mock_test_file.csv";
        _separator = ",";
        
        if (File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        File.Delete(_filePath);
    }
    
    
    [Test]
    public void TestFileIsCreated()
    {
        var testFile = new FileResultWriter(_filePath, _culture, _date, _separator);
        testFile.Print(_changes);
        
        Assert.True(File.Exists(_filePath));
    }
        
    [Test]
    public void TestFileContent()
    {
        var testFile = new FileResultWriter(_filePath, _culture, _date, _separator);
        testFile.Print(_changes);
        var contents = File.ReadAllText(_filePath);

        Assert.AreEqual(contents, _holdingsStr);
    }
}
