using System.Globalization;
using BusinessLayer.DataLoading;
using BusinessLayer.DiffComputing;
using BusinessLayer.Exceptions;
using BusinessLayer.Logging;
using BusinessLayer.Writers;
using CommandLine;

namespace PresentationLayer.ConsoleApps
{
    public class ConsoleApp : IApp
    {
        private readonly IDataLoader? _dataLoader;
        private readonly IResultWriter? _resultWriter;
        private readonly IDiffComputer _diffComputer = new DiffComputer();
        private readonly ILogger _logger = new ConsoleLogger();
        private readonly Options? _options;
        
        public ConsoleApp()
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
            catch (DataLoaderException e)
            {
                Console.WriteLine($"Couldn't load a file: {e.Message}.");
                _logger.Log(e.Message);
            }
            catch (DataWriterException e)
            {
                Console.WriteLine($"Couldn't write to a file: {e.Message}.");
                _logger.Log(e.Message);
            }
        }
        
        private Options? ValidateOptions(Options o)
        {
            return AreOptionsValid(o) ? o : null;
        }
        
        private bool AreOptionsValid(Options o)
        {
            var inputFiles = o.InputFilesList;
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
        
        private Options? HandleParseError(IEnumerable<Error> errs)
        {
            var errors = errs.ToList();
            Console.WriteLine($"errors: {errors.Count}");
            _logger.Log($"CommandLineParser - {errors.Count} errors:");
            foreach (var e in errors)
            {
                _logger.Log($" - Error: {e.ToString() ?? "CommandLine." + e.Tag}");
            }
            
            return null;
        }
    }
}
