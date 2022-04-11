using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using BL.DataLoading;
using BL.Writers;
using DAL.Models;
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
        
        // _testFile.Date = new DateTime(2022, 3, 21);
        _changes.Add(CreateTeslaHoldingChanges());
        _changes.Add(CreateHealthHoldingChanges());
        _changes.Add(CreateRokuHoldingChanges());

        _culture = new CultureInfo("en-US");
    }


    [Test]
    public void TestStringGenerator()
    {
        string str = DiffStringGenerator.GenerateSeparatedString(_changes, _culture);
        Assert.AreEqual(str, _holdingsStr);
    }


    private static HoldingChanges CreateTeslaHoldingChanges()
    {
        HoldingBaseInformation teslaTestHoldingInfo = new HoldingBaseInformation();
        teslaTestHoldingInfo.Company = "TESLA INC";
        teslaTestHoldingInfo.Cusip = "88160R101";
        teslaTestHoldingInfo.Fund = "ARKK";
        teslaTestHoldingInfo.Ticker = "TSLA";

        return new HoldingChanges()
        {
            Holding = teslaTestHoldingInfo,
            NumberOfShares = 1356326,
            DifferenceOfShares = 10,
            DifferenceOfWeight = 9.66,
            MarketValueDifference = 1228003997.14
        };
    }

    private static HoldingChanges CreateHealthHoldingChanges()
    {
        HoldingBaseInformation healthTestHoldingInfo = new HoldingBaseInformation();
        healthTestHoldingInfo.Company = "TELADOC HEALTH INC";
        healthTestHoldingInfo.Cusip = "87918A105";
        healthTestHoldingInfo.Fund = "ARKK";
        healthTestHoldingInfo.Ticker = "TDOC";


        return new HoldingChanges()
        {
            Holding = healthTestHoldingInfo,
            NumberOfShares = 12395542,
            DifferenceOfShares = -20,
            DifferenceOfWeight = -6.72,
            MarketValueDifference = 854796576.32
        };
    }

    private static HoldingChanges CreateRokuHoldingChanges()
    {
        HoldingBaseInformation healthTestHoldingInfo = new HoldingBaseInformation();
        healthTestHoldingInfo.Company = "ROKU INC";
        healthTestHoldingInfo.Cusip = "77543R102";
        healthTestHoldingInfo.Fund = "ARKK";
        healthTestHoldingInfo.Ticker = "ROKU";

        return new HoldingChanges()
        {
            Holding = healthTestHoldingInfo,
            NumberOfShares = 6452422,
            DifferenceOfShares = 5,
            DifferenceOfWeight = 6.34,
            MarketValueDifference = 805584886.70
        };
    }
}