using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.DataLoading;
using BusinessLayer.DiffComputing;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using BusinessLayer.Writers;
using CsvHelper;

namespace PresentationLayer.UI
{
    public class DataSetUi : BaseUi, IDataSetUi
    {
        private readonly IDataSetService _dataSetService;
        private readonly IDiffComputer _diffComputer;
        private readonly IDataLoader _dataLoader;

        public DataSetUi(IDataSetService dataSetService, IDiffComputer diffComputer, IDataLoader dataLoader) {
            _dataSetService = dataSetService;
            _diffComputer = diffComputer;
            _dataLoader = dataLoader;
        }

        public async Task Run() {
            await GenerateUi(
                new List<MenuAction>() {
                    new MenuAction() { Identifier = "1", Description = "Load file", Action = LoadFile},
                    new MenuAction() { Identifier = "2", Description = "Download current file", Action = DownloadCurrentFile},
                    new MenuAction() { Identifier = "3", Description = "View all files", Action = ViewFiles},
                    new MenuAction() { Identifier = "4", Description = "Make a diff", Action = MakeDiff},
                    new MenuAction() { Identifier = "5", Description = "Delete file by ID", Action = DeleteFile}
                });
        }

        private async Task LoadFile() {
            Console.WriteLine("Path: ('b' for back)");
            var input = Console.ReadLine();

            while (input! != "b") {
                if (!File.Exists(input)) {
                    Console.WriteLine("File on this path does not exist!");
                    input = Console.ReadLine();
                    continue;
                }

                var file = _dataLoader.LoadCsvFile(input);
                await _dataSetService.CreateDataSet(file);
                return;
            }
        }

        private async Task DownloadCurrentFile() {

        }

        private async Task ViewFiles() {
            var files = await  _dataSetService.GetAllDataSets();
            foreach (var file in files) {
                Console.WriteLine($"File ID: {file.Id}, date of upload: {file.Date}");
            }
        }

        private async Task MakeDiff() {
            Console.WriteLine(
                "Write IDs of two files and path (if you want to store diff in file) separated by comma (like '5,3,../output.csv').\n" +
                "If no path is provided, the diff will be printed in console (type 'b' for back).");

            var input = Console.ReadLine();
            while (input! != "b") {
                var lines = input!.Split(",");
                if (lines.Length is < 2 or > 3) {
                    Console.WriteLine($"Wrong number of arguments: {lines.Length}");
                    input = Console.ReadLine();
                    continue;
                }

                try {
                    var firstFile = await _dataSetService.GetDataSetById(int.Parse(lines[0]));
                    var secondFile = await _dataSetService.GetDataSetById(int.Parse(lines[1]));
                    var diff = _diffComputer.ComputeDiff(firstFile, secondFile);
                    var cultureInfo = new CultureInfo("en-US");
                    IResultWriter writer = lines.Length == 3 ? new FileResultWriter(lines[2], cultureInfo) : new ConsoleResultWriter(cultureInfo);
                    writer.Print(diff);
                    break;
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }

                input = Console.ReadLine();
            }
        }

        private async Task DeleteFile() {
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
