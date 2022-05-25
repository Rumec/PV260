using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BusinessLayer.DataLoading;
using BusinessLayer.Notifications;
using BusinessLayer.Writers;
using DataLayer.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.BLTests;

[TestFixture]
public class ResultWriterTests
{
    private List<HoldingChanges> _changes = new List<HoldingChanges>(); 
    private CultureInfo _culture;

    private string _holdingsStr =
        "date,fund,company,ticker,cusip,shares,\"shares difference\",\"market value difference ($)\",\"weight difference (%)\"" +
        Environment.NewLine +
        "04/12/2022,ARKK,\"TESLA INC\",TSLA,88160R101,\"1,356,326\",\"10.00\",\"$1,228,003,997.14\",9.66%" +
        Environment.NewLine +
        "04/12/2022,ARKK,\"TELADOC HEALTH INC\",TDOC,87918A105,\"12,395,542\",\"-20.00\",\"$854,796,576.32\",-6.72%" +
        Environment.NewLine +
        "04/12/2022,ARKK,\"ROKU INC\",ROKU,77543R102,\"6,452,422\",\"5.00\",\"$805,584,886.70\",6.34%" +
        Environment.NewLine;
    // TODO: Add the disclaimer

    [SetUp]
    public void Setup()
    {
        _changes.Add(TestUtils.CreateTeslaHoldingChanges());
        _changes.Add(TestUtils.CreateHealthHoldingChanges());
        _changes.Add(TestUtils.CreateRokuHoldingChanges());

        _culture = new CultureInfo("en-US");
    }


    [Test]
    public void TestStringGenerator()
    {
        string str = DiffStringGenerator.GenerateSeparatedString(_changes, _culture, new DateTime(2022, 4, 12));
        Assert.AreEqual(str, _holdingsStr);
    }
}