using System.Globalization;
using BusinessLayer.DataLoading;
using BusinessLayer.DiffComputing;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using BusinessLayer.Writers;

namespace PresentationLayer.UI
{
    public class DataSetUi : BaseUi, IDataSetUi
    {
        private readonly IDataSetService _dataSetService;
        private readonly IDiffComputer _diffComputer;
        private readonly IDataLoader _dataLoader;
        private readonly IDataDownloader _dataDownloader;

        public DataSetUi(IDataSetService dataSetService, IDiffComputer diffComputer, IDataLoader dataLoader,
            IDataDownloader dataDownloader)
        {
            _dataSetService = dataSetService;
            _diffComputer = diffComputer;
            _dataLoader = dataLoader;
            _dataDownloader = dataDownloader;
        }

        public async Task Run()
        {
            await GenerateUi(
                new List<MenuAction>
                {
                    new() { Identifier = "1", Description = "Load file", Action = LoadFile },
                    new() { Identifier = "2", Description = "Download current file", Action = DownloadCurrentFile },
                    new() { Identifier = "3", Description = "View all files", Action = ViewFiles },
                    new() { Identifier = "4", Description = "Make a diff", Action = MakeDiff },
                    new() { Identifier = "5", Description = "Delete file by ID", Action = DeleteFile }
                });
        }

        private async Task LoadFile()
        {
            var input = "";

            while (input! != "b")
            {
                Console.WriteLine("Path: ('b' for back)");
                input = Console.ReadLine();

                if (!File.Exists(input))
                {
                    Console.WriteLine("File on this path does not exist!");
                    input = Console.ReadLine();
                    continue;
                }

                try
                {
                    var file = _dataLoader.LoadCsvFile(input);
                    await _dataSetService.CreateDataSet(file);
                }
                catch (DataSetAlreadyExistsException e)
                {
                    Console.WriteLine("File from this date was already downloaded!");
                    continue;
                }

                return;
            }
        }

        private async Task DownloadCurrentFile()
        {
            var input = "";

            while (input! != "b")
            {
                Console.WriteLine("Path: ('b' for back)");
                input = Console.ReadLine();
                try
                {
                    var file = await _dataDownloader.LoadCsvFile(input!);
                    await _dataSetService.CreateDataSet(file);
                    return;
                }
                catch (DataSetAlreadyExistsException e)
                {
                    Console.WriteLine("File from this date was already downloaded!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to download the file!");
                }
            }
        }

        private async Task ViewFiles()
        {
            var files = await _dataSetService.GetAllDataSets();
            foreach (var file in files)
            {
                Console.WriteLine($"File ID: {file.Id}, date of upload: {file.Date}");
            }
        }

        private async Task MakeDiff()
        {
            Console.WriteLine(
                "Write IDs of two files and path (if you want to store diff in file) separated by comma (like '5,3,../output.csv').\n" +
                "If no path is provided, the diff will be printed in console (type 'b' for back).");

            var input = Console.ReadLine();
            while (input! != "b")
            {
                var lines = input!.Split(",");
                if (lines.Length is < 2 or > 3)
                {
                    Console.WriteLine($"Wrong number of arguments: {lines.Length}");
                    input = Console.ReadLine();
                    continue;
                }

                try
                {
                    var firstFile = await _dataSetService.GetDataSetById(int.Parse(lines[0]));
                    var secondFile = await _dataSetService.GetDataSetById(int.Parse(lines[1]));
                    var diff = _diffComputer.ComputeDiff(firstFile, secondFile);
                    var cultureInfo = new CultureInfo("en-US");
                    IResultWriter writer = lines.Length == 3
                        ? new FileResultWriter(lines[2], cultureInfo)
                        : new ConsoleResultWriter(cultureInfo);
                    writer.Print(diff);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                input = Console.ReadLine();
            }
        }

        private async Task DeleteFile()
        {
            Console.WriteLine("Which file would you like to remove? ('b' for back)");
            var input = Console.ReadLine();

            while (input! != "b")
            {
                while (!int.TryParse(input, out _))
                {
                    Console.WriteLine("Id has to be a number!");
                }

                try
                {
                    await _dataSetService.DeleteDataSet(int.Parse(input));
                    return;
                }
                catch (DataSetDoesNotExistException)
                {
                    Console.WriteLine("File with this ID does not exist. Try again");
                    input = Console.ReadLine();
                }
            }
        }
    }
}