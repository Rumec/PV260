using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Request = WireMock.RequestBuilders.Request;
using BusinessLayer.DataLoading;
using DataLayer.Models;
using FluentAssertions;

namespace Tests.BLTests
{
    public class CsvFileDownloaderTests
    {
        private DataSet _testFile;
        private WireMockServer _server;
        [SetUp]
        public void Setup()
        {
            var body = File.ReadAllText("../../../TestFiles/csv_file_loader_test.csv");
            _server = WireMockServer.Start(3006);
            _server.Given(
                    Request.Create()
                        .UsingGet()
                        .WithPath("/wp-content/uploads/funds-etf-csv"))
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithBody(body));
            
            _testFile = new DataSet();
            _testFile.Date = new DateTime(2022, 3, 21);
            _testFile.Holdings.Add(CreateTeslaHolding());
            _testFile.Holdings.Add(CreateHealthHolding());
            _testFile.Holdings.Add(CreateRokuHolding());
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
        }

        [Test]
        public async Task CsvFileDownloadTest()
        {
            var downloader = new CsvFileDownloader() ;
            var client = downloader.LoadCsvFile("http://localhost:3006/wp-content/uploads/funds-etf-csv", ",");
            await client;
            client.Result.Should().BeEquivalentTo(_testFile);
        }
        
        private static Holding CreateTeslaHolding()
        {
            var teslaTestHolding = new Holding
            {
                Company = "TESLA INC",
                Currency = "$",
                Cusip = "88160R101",
                Fund = "ARKK",
                MarketValue = 1228003997.14,
                Shares = 1356326,
                Ticker = "TSLA",
                Weight = 9.66
            };
            return teslaTestHolding;
        }
    
        private static Holding CreateHealthHolding()
        {
            var healthTestHolding = new Holding
            {
                Company = "TELADOC HEALTH INC",
                Currency = "$",
                Cusip = "87918A105",
                Fund = "ARKK",
                MarketValue = 854796576.32,
                Shares = 12395542,
                Ticker = "TDOC",
                Weight = 6.72
            };
            return healthTestHolding;
        }
        private static Holding CreateRokuHolding()
        {
            var healthTestHolding = new Holding
            {
                Company = "ROKU INC",
                Currency = "$",
                Cusip = "77543R102",
                Fund = "ARKK",
                MarketValue = 805584886.70,
                Shares = 6452422,
                Ticker = "ROKU",
                Weight = 6.34
            };
            return healthTestHolding;
        }
    }
}