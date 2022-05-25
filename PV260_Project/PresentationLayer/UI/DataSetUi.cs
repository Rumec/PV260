﻿using System.Globalization;
using BusinessLayer.DataLoading;
using BusinessLayer.DiffComputing;
using BusinessLayer.Exceptions;
using BusinessLayer.Notifications;
using BusinessLayer.Services;
using BusinessLayer.Writers;
using PresentationLayer.Utils;

namespace PresentationLayer.UI
{
    public class DataSetUi : BaseUi, IDataSetUi
    {
        private readonly IDataSetService _dataSetService;
        private readonly IUserEmailService _userEmailService;
        private readonly IDiffComputer _diffComputer;
        private readonly IDataLoader _dataLoader;
        private readonly IDataDownloader _dataDownloader;
        private readonly IEmailSender _emailSender;
        private readonly IConsoleWrapper _consoleWrapper;

        public DataSetUi(
            IDataSetService dataSetService,
            IUserEmailService userEmailService,
            IDiffComputer diffComputer,
            IDataLoader dataLoader,
            IDataDownloader dataDownloader,
            IEmailSender emailSender,
            IConsoleWrapper consoleWrapper) : base(consoleWrapper)
        {
            _dataSetService = dataSetService;
            _userEmailService = userEmailService;
            _diffComputer = diffComputer;
            _dataLoader = dataLoader;
            _dataDownloader = dataDownloader;
            _emailSender = emailSender;
            _consoleWrapper = consoleWrapper;
        }

        public async Task Run()
        {
            await GenerateUi(
                new List<MenuAction>
                {
                    new() { Identifier = UserInput.LoadFile, Description = "Load file", Action = LoadFile },
                    new() { Identifier = UserInput.DownloadCurrentFile, Description = "Download current file", Action = DownloadCurrentFile },
                    new() { Identifier = UserInput.ViewFiles, Description = "View all files", Action = ViewFiles },
                    new() { Identifier = UserInput.MakeDiff, Description = "Make a diff", Action = MakeDiff },
                    new() { Identifier = UserInput.DeleteFile, Description = "Delete file by ID", Action = DeleteFile },
                    new() { Identifier = UserInput.SendNotification, Description = "Send email notification", Action = SendNotification }
            });
        }

        private async Task LoadFile()
        {
            var input = "";

            while (input! != UserInput.Back)
            {
                _consoleWrapper.WriteLine($"Path: ('{UserInput.Back}' for back)");
                input = _consoleWrapper.ReadLine();

                if (!File.Exists(input))
                {
                    _consoleWrapper.WriteLine("File on this path does not exist!");
                    input = _consoleWrapper.ReadLine();
                    continue;
                }

                try
                {
                    var file = _dataLoader.LoadCsvFile(input);
                    await _dataSetService.CreateDataSet(file);
                }
                catch (DataSetAlreadyExistsException e)
                {
                    _consoleWrapper.WriteLine("File from this date was already loaded!");
                    continue;
                }

                return;
            }
        }

        private async Task DownloadCurrentFile()
        {
            var input = "";

            while (input! != UserInput.Back)
            {
                _consoleWrapper.WriteLine($"Path: ('{UserInput.Back}' for back)");
                input = _consoleWrapper.ReadLine();
                try
                {
                    var file = await _dataDownloader.LoadCsvFile(input!);
                    await _dataSetService.CreateDataSet(file);
                    return;
                }
                catch (DataSetAlreadyExistsException e)
                {
                    _consoleWrapper.WriteLine("File from this date was already downloaded!");
                }
                catch (Exception e)
                {
                    _consoleWrapper.WriteLine("Unable to download the file!");
                }
            }
        }

        private async Task ViewFiles()
        {
            var files = await _dataSetService.GetAllDataSets();
            foreach (var file in files)
            {
                _consoleWrapper.WriteLine($"File ID: {file.Id}, date of upload: {file.Date}");
            }
        }

        private async Task MakeDiff()
        {
            _consoleWrapper.WriteLine(
                "Write IDs of two files and path (if you want to store diff in file) separated by comma (like '5,3,../output.csv').\n" +
                "If no path is provided, the diff will be printed in console (type 'b' for back).");

            var input = _consoleWrapper.ReadLine();
            while (input! != UserInput.Back)
            {
                var lines = input!.Split(",");
                if (lines.Length is < 2 or > 3)
                {
                    _consoleWrapper.WriteLine($"Wrong number of arguments: {lines.Length}");
                    input = _consoleWrapper.ReadLine();
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
                    _consoleWrapper.WriteLine(e.Message);
                }

                input = _consoleWrapper.ReadLine();
            }
        }

        private async Task DeleteFile()
        {
            _consoleWrapper.WriteLine($"Which file would you like to remove? ('{UserInput.Back}' for back)");
            var input = _consoleWrapper.ReadLine();

            while (input! != UserInput.Back)
            {
                if (!int.TryParse(input, out _))
                {
                    _consoleWrapper.WriteLine("Id has to be a number!");
                    input = _consoleWrapper.ReadLine();
                    continue;
                }

                try
                {
                    await _dataSetService.DeleteDataSet(int.Parse(input));
                    return;
                }
                catch (DataSetDoesNotExistException)
                {
                    _consoleWrapper.WriteLine("File with this ID does not exist. Try again");
                    input = _consoleWrapper.ReadLine();
                }
            }
        }
        
        private async Task SendNotification() {
            var emails = await _userEmailService.GetAllRegisteredEmails();

            if (!emails.Any()) return;

            _consoleWrapper.WriteLine("Daily notification email are being sent to the following emails:");
            emails.ForEach(e => _consoleWrapper.WriteLine(e.Address));
                
            var dataSets = await _dataSetService.GetAllDataSets();
            var holdingChanges = _diffComputer.ComputeDiff(dataSets[0], dataSets[1]);

            try
            {
                _emailSender.SendNotification(holdingChanges, emails);
                _consoleWrapper.WriteLine("Emails were sent successfully.");
            }
            catch (EmailSenderException e)
            {
                _consoleWrapper.WriteLine(e.Message);
            }

            _consoleWrapper.WriteLine($"(type '{UserInput.Back}' for back)");
            var input = _consoleWrapper.ReadLine();
            while (input! != UserInput.Back) {
                input = _consoleWrapper.ReadLine();
            }
        }

    }
}