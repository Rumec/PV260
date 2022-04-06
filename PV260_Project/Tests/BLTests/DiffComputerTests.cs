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
    private readonly List<string> _currencies = new() {"EUR", "USD", "CZK"};

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

        var result = new DiffComputer(csv1, csv2).ComputeDiff();

        result.Should().NotBeEmpty()
            .And.NotContainNulls()
            .And.HaveSameCount(changes)
            .And.OnlyHaveUniqueItems()
            .And.BeEquivalentTo(changes);
    }

    private Holding CreateHolding(int holdingIndex, long shares, double weight, double marketValue)
    {
        return new Holding
        {
            Company = "Company" + holdingIndex,
            Cusip = "Cusip" + holdingIndex,
            Fund = "Fund" + holdingIndex,
            Ticker = "Ticker" + holdingIndex,
            Currency = _currencies[holdingIndex % _currencies.Count],
            Shares = shares,
            Weight = weight,
            MarketValue = marketValue
        };
    }

    private HoldingChanges CreateHoldingChanges(Holding holding, long sharesDiff, double weightDiff,
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