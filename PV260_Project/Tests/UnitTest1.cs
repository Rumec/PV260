using System;
using System.IO.Enumeration;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using BL.DataLoading;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        private const string BasePath = "../../../";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test() {
            //Arrange
            var loader = new CsvFileLoader(",");

            //Act
            var file = loader.LoadCsvFile($"{BasePath}/TestFiles/test_file_1.csv");

            //Assert
            file.Holdings.Count.Should().Be(36);
        }
    }
}