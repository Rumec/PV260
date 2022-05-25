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
            _testFile.Holdings.Add(BLTests.TestUtils.CreateTeslaHolding());
            _testFile.Holdings.Add(BLTests.TestUtils.CreateHealthHolding());
            _testFile.Holdings.Add(BLTests.TestUtils.CreateRokuHolding());
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
    }
}