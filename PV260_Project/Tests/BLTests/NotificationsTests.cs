using System;
using System.Collections.Generic;
using System.Globalization;
using BusinessLayer.DiffComputing;
using BusinessLayer.Notifications;
using DataLayer.Models;
using FluentAssertions;
using NUnit.Framework;
using BusinessLayer.Notifications;
using Microsoft.Extensions.Options;
using HtmlAgilityPack;

using System.Linq;
namespace Tests.BLTests;

[TestFixture]
public class NotificationsTests
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

    // The password must be filled in!!!
    // You can find it in our discord
    private SmtpSettings _mailSettings = new SmtpSettings()
    {
        Host = "smtp.gmail.com",
        Port = 587,
        FromAddress = "pv260.teamOrange@gmail.com",
        UserName = "pv260.teamOrange@gmail.com",
        Password = "",
        EnableSsl = true
    };

    private IOptions<SmtpSettings> _mailOptions;
    private IMessageBuilder _msgBuilder;
    private IEmailSender _sender;

    [SetUp]
    public void Setup()
    {
        _changes.Add(TestUtils.CreateTeslaHoldingChanges());
        _changes.Add(TestUtils.CreateHealthHoldingChanges());
        _changes.Add(TestUtils.CreateRokuHoldingChanges());

        _culture = new CultureInfo("en-US");

        _mailOptions = Options.Create(_mailSettings);
        _msgBuilder = new HtmlMessageBuilder();
        _sender = new GmailSender(_msgBuilder, _mailOptions);
    }

    [Test]
    [TestCase("469372@mail.muni.cz")]
    [TestCase("485628@mail.muni.cz")]
    public void Should_NotThrow_When_SingleEmailSent(string emailAddress)
    {
        Assert.DoesNotThrow(() =>
            _sender.SendNotification(_changes, new List<Email>() { new Email() { Address = emailAddress } })
        );
    }

    [Test]
    [TestCase("469372@mail.muni.cz", "485628@mail.muni.cz")]
    public void Should_NotThrow_When_MultipleEmailSent(string emailAddress1, string emailAddress2)
    {
        Assert.DoesNotThrow(() =>
            _sender.SendNotification(_changes, new List<Email>() { new Email() { Address = emailAddress1 },
            new Email() { Address = emailAddress2 }})
        );
    }

    [Test]
    [TestCase(0, "485628@mail.muni.cz", "Delete later")]
    [TestCase(1, "485628@mail.muni.cz", "Stuff \t hmm")]
    [TestCase(2, "469372@mail.muni.cz", "Subject")]
    public void When_CreatingHtml_Then_HtmlIsValid(int index, string recipient, string subject)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(
            _msgBuilder.Build(_mailSettings.FromAddress, 
            new List<string>() { recipient },
            subject, 
            new List<HoldingChanges>() { _changes[index] }
        ).Body);

        Assert.AreEqual(doc.ParseErrors.Count(), 0);
    }
}
