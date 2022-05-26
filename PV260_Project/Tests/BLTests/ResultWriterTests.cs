using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessLayer.Writers;
using DataLayer.Models;
using NUnit.Framework;

namespace Tests.BLTests;

[TestFixture]
public class ResultWriterTests
{
    private List<HoldingChanges> _changes = new List<HoldingChanges>(); 
    private CultureInfo _culture;
    private DateTime _date;
    private string _holdingsStr;

    [SetUp]
    public void Setup()
    {
        _changes.Add(TestUtils.CreateTeslaHoldingChanges());
        _changes.Add(TestUtils.CreateHealthHoldingChanges());
        _changes.Add(TestUtils.CreateRokuHoldingChanges());

        _culture = TestUtils.HoldingsCultureInfo;
        _date = TestUtils.HoldingsDate;
        _holdingsStr = TestUtils.HoldingsStr;
    }


    [Test]
    public void TestStringGenerator()
    {
        string str = DiffStringGenerator.GenerateSeparatedString(_changes, _culture, _date);
        Assert.AreEqual(str, _holdingsStr);
    }
}
