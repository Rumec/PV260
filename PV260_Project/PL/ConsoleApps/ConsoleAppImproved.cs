using System.Globalization;
using BL.DataLoading;
using BL.DiffComputing;using BL.Writers;
using CommandLine;

namespace PL.ConsoleApps
{
    public class ConsoleAppImproved : IApp
    {
        private readonly IDataLoader? _dataLoader;
        private readonly IResultWriter? _resultWriter;
        private readonly IDiffComputer _diffComputer = new DiffComputer();
        private readonly Options? _options;
        
        public ConsoleAppImproved()
        {
            var args = Environment.GetCommandLineArgs();
            _options = Parser.Default.ParseArguments<Options>(args).MapResult(ValidateOptions, HandleParseError);
            if (_options == null)
            {
                return;
            }

            _dataLoader = new CsvFileLoader(_options.Delim);
            _resultWriter = (_options.OutputFile == null)
                ? new ConsoleResultWriter(new CultureInfo("en-US"), DateTime.Now, _options.Delim)
                : new FileResultWriter(_options.OutputFile, new CultureInfo("en-US"), DateTime.Now, _options.Delim);
        }
        
        public void Run()
        {
            if (_options == null || _dataLoader == null || _resultWriter == null)
            {
                return;
            }
            
            try
            {
                var fstParsedFile = _dataLoader.LoadCsvFile(_options.InputFilesList[0]);
                var sndParsedFile = _dataLoader.LoadCsvFile(_options.InputFilesList[1]);
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
        
        private Options? ValidateOptions(Options o)
        {
            return AreOptionsValid(o) ? o : null;
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
        
        private Options? HandleParseError(IEnumerable<Error> errors)
        {
            Console.WriteLine($"errors {errors.Count()}");
            return null;
        }
    }
}
