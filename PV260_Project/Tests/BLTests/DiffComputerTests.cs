using System;
using System.Collections.Generic;
using BL.DiffComputing;
using DAL.Models;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.BLTests;

[TestFixture]
public class DiffComputerTests
{
    private static readonly List<string> Currencies = new() {"EUR", "USD", "CZK"};

    private static readonly CsvFile DummyCsvFile = CreateDummyCsvFile();
    private static readonly CsvFile DummyCsvFileWithoutHoldings = CreateDummyCsvFile(false);

    private static IEnumerable<TestCaseData> CsvFilesWithEmptyHoldingsTestCases =>
        new[]
        {
            new TestCaseData(DummyCsvFile, DummyCsvFileWithoutHoldings, 1, true)
                .SetName("Only first CsvFile has Holdings")
                .SetDescription("Expected: diffs are negative, since particular holding was only in first CsvFile"),
            new TestCaseData(DummyCsvFileWithoutHoldings, DummyCsvFile, 1, false)
            .SetName("Only second CsvFile has Holdings")
            .SetDescription("Expected: diffs are positive, since particular holding was only in second CsvFile"),
            new TestCaseData(DummyCsvFileWithoutHoldings, DummyCsvFileWithoutHoldings, 0, false)
                .SetName("Both CsvFiles without Holdings")
                .SetDescription("Expected: none HoldingChange"),
        };

    [Test]
    public void TestComputeDiff()
    {
        var holding1 = CreateHolding(1, 1000L, 10.0, 100000);
        var holding2 = CreateHolding(2, 500L, 50.0, 50000);
        var holding3 = CreateHolding(3, 12L, 12.3, 1234567.89);

        var holding1New = CreateHolding(1, 2000L, 2.0, 200000);
        var holding3New = CreateHolding(3, 12L, 12.3, 1234567.89);
        var holding4New = CreateHolding(4, 4444L, 44.44, 1234567.89);
        var holding5New = CreateHolding(5, 555555L, 55, 5555555.5);

        var csv1 = new CsvFile
        {
            Date = new DateTime(2022, 4, 4),
            Holdings = new List<Holding> {holding1, holding2, holding3}
        };
        var csv2 = new CsvFile
        {
            Date = new DateTime(2022, 5, 5),
            Holdings = new List<Holding> {holding1New, holding3New, holding4New, holding5New}
        };

        var changes = new List<HoldingChanges>
        {
            CreateHoldingChanges(holding1New, 1000L, -8.0, 100000),
            CreateHoldingChanges(holding2, -500L, -50.0, -50000),
            CreateHoldingChanges(holding3New, 0L, 0.0, 0.0),
            CreateHoldingChanges(holding4New, 4444L, 44.44, 1234567.89),
            CreateHoldingChanges(holding5New, 555555L, 55, 5555555.5),
        };

        var result = new DiffComputer().ComputeDiff(csv1, csv2);

        result.Should().NotBeEmpty()
            .And.NotContainNulls()
            .And.HaveSameCount(changes)
            .And.OnlyHaveUniqueItems()
            .And.BeEquivalentTo(changes);
    }

    [Test]
    [TestCaseSource(nameof(CsvFilesWithEmptyHoldingsTestCases))]
    public void TestComputeDiff_WhenCsvFileWithoutHoldings(CsvFile first, CsvFile second, int expectedHoldingsCount,
        bool shouldBeNegative)
    {
        var result = new DiffComputer().ComputeDiff(first, second);
        result.Should().HaveCount(expectedHoldingsCount);

        if (expectedHoldingsCount > 0)
            result.Should().AllSatisfy(x =>
            {
                (x.DifferenceOfShares < 0).Should().Be(shouldBeNegative);
                (x.DifferenceOfWeight < 0).Should().Be(shouldBeNegative);
                (x.MarketValueDifference < 0).Should().Be(shouldBeNegative);
            });
    }

    private static CsvFile CreateDummyCsvFile(bool withHolding = true)
    {
        var csvFile = new CsvFile
        {
            Date = new DateTime(2022, 4, 4)
        };
        if (withHolding)
            csvFile.Holdings = new List<Holding> {CreateHolding(1, 1000L, 10.0, 100000)};

        return csvFile;
    }

    private static Holding CreateHolding(int holdingIndex, long shares, double weight, double marketValue)
    {
        return new Holding
        {
            Company = "Company" + holdingIndex,
            Cusip = "Cusip" + holdingIndex,
            Fund = "Fund" + holdingIndex,
            Ticker = "Ticker" + holdingIndex,
            Currency = Currencies[holdingIndex % Currencies.Count],
            Shares = shares,
            Weight = weight,
            MarketValue = marketValue
        };
    }

    private static HoldingChanges CreateHoldingChanges(Holding holding, long sharesDiff, double weightDiff,
        double marketValueDiff)
    {
        return new HoldingChanges
        {
            Holding = holding,
            NumberOfShares = holding.Shares,
            DifferenceOfShares = sharesDiff,
            DifferenceOfWeight = weightDiff,
            MarketValueDifference = marketValueDiff
        };
    }
}