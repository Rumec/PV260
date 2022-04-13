// See https://aka.ms/new-console-template for more information

using System.Globalization;
using BL.DataLoading;
using BL.DiffComputing;using BL.Writers;


if (Environment.GetCommandLineArgs().Length < 4 || Environment.GetCommandLineArgs().Length > 5)
{
    Console.WriteLine("Invalid number of arguments, must be 4 or 5!");
    return;
}

string inputFile = Environment.GetCommandLineArgs()[1];
string sndFile = Environment.GetCommandLineArgs()[2];
string delim = Environment.GetCommandLineArgs()[3];
string? outputFile = Environment.GetCommandLineArgs().Length == 5 ?
    Environment.GetCommandLineArgs()[4] : null;


if (!File.Exists(inputFile) || !File.Exists(sndFile))
{
    Console.WriteLine("Files don't exist!");
    return;
}

// TODO Take this from cmd line
var csvFileLoader = new CsvFileLoader(delim);
var fstParsedFile = csvFileLoader.LoadCsvFile(inputFile);
var sndParsedFile = csvFileLoader.LoadCsvFile(sndFile);

var diffComputer = new DiffComputer();
var changes = diffComputer.ComputeDiff(fstParsedFile, sndParsedFile);

new ConsoleResultWriter(new CultureInfo("en-US"), DateTime.Now, delim).Print(changes);
