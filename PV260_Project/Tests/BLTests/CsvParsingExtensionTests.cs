using System;
using System.Collections.Generic;
using BusinessLayer.Extensions;
using DataLayer.Models;
using NUnit.Framework;

namespace Tests.BLTests;

[TestFixture]
public class CsvParsingExtensionTests
{
    static object[] _currencyCases =
    {
        new object[] { "$769,871,740.32", "$", 769871740.32 },
        new object[] { "$758,877,828.96", "$", 758877828.96 },
        new object[] { "£758", "£", 758 }
    };
    
    static object[] _dateCases =
    {
        new object[] { "10/15/2000", new DateTime(2000,10,15)},
        new object[] { "02/06/2022", new DateTime(2022,02,06)},
        new object[] { "04/28/1997", new DateTime(1997,04,28)}
    };

    [Test]
    [TestCaseSource(nameof(_dateCases))]
    public void TestGetDateTime(string input, DateTime date)
    {
        var result = input.GetDateTime();
        Assert.That(Equals(result, date));
    }

    [Test]
    [TestCaseSource(nameof(_currencyCases))]
    public void TestGetCurrencyAndValue(string input, string currency, double value )
    {
        var result = input.GetCurrencyAndValue();
        Assert.That(Equals(new Tuple<string, double>(currency, value), result));
    }
}