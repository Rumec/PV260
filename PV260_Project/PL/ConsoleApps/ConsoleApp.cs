using System.Globalization;
using BL.DataLoading;
using BL.DiffComputing;
using BL.Writers;
using CommandLine;

namespace PL.ConsoleApps
{
    // > first version of ConsoleApp that doesn't do any 'extensive' logic/parsing in constructor,
    //   which means we can't initialize _dataLoader / _resultWriter (and only do it 'late' after parsing args)
    // > improved (?) version, ConsoleAppImproved, does some parsing in constructor so that we can properly
    //   set _dataLoader / _resultWriter
    public class ConsoleApp : IApp
    {
        private IDataLoader _dataLoader;
        private IResultWriter _resultWriter;
        private readonly IDiffComputer _diffComputer = new DiffComputer();

        public void Run()
        {
            var args = Environment.GetCommandLineArgs();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Execute)
                .WithNotParsed(HandleParseError);
        }

        private void Execute(Options o)
        {
            if (!AreOptionsValid(o))
            {
                return;
            }

            // can't initialize it earlier
            _dataLoader = new CsvFileLoader(o.Delim);
            _resultWriter = (o.OutputFile == null)
                ? new ConsoleResultWriter(new CultureInfo("en-US"), DateTime.Now, o.Delim)
                : new FileResultWriter(o.OutputFile, new CultureInfo("en-US"), DateTime.Now, o.Delim);

            try
            {
                var fstParsedFile = _dataLoader.LoadCsvFile(o.InputFilesList[0]);
                var sndParsedFile = _dataLoader.LoadCsvFile(o.InputFilesList[1]);
                var changes = _diffComputer.ComputeDiff(fstParsedFile, sndParsedFile);
                _resultWriter.Print(changes);
            }
            // TODO: should use 'our' generic exception (that covers errors for reading from disk, loading/fetching from web etc.) ?
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private bool AreOptionsValid(Options o)
        {
            // delimiter can be any string?
            // outputFile can be any string?
            var inputFiles = o.InputFilesList;

            // Count = 2 is assured by Options.InputFiles (otherwise parse error), therefore useless 'if' - should remove ?
            if (inputFiles.Count != 2)
            {
                Console.WriteLine("You need to specify exactly TWO input CSV files.");
                return false;
            }

            var invalidPath = false;
            foreach (var inputFile in inputFiles)
            {
                if (!File.Exists(inputFile))
                {
                    Console.WriteLine($"Input file '{inputFile}' doesn't exist!");
                    invalidPath = true;
                }
            }

            if (invalidPath)
            {
                Console.WriteLine("All provided file paths must be valid!");
                return false;
            }

            return true;
        }

        private void HandleParseError(IEnumerable<Error> errors)
        {
            Console.WriteLine($"errors {errors.Count()}");
        }
    }
}