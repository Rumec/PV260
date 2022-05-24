using BusinessLayer.DataLoading;
using BusinessLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PresentationLayer.UI
{
    internal class ConfigUi : BaseUi, IConfigUi
    {
        private readonly IDataDownloader _dataDownloader;
        private readonly IFileUrlService _fileUrlService;

        public ConfigUi(IDataDownloader dataDownloader, IFileUrlService fileUrlService)
        {
            _fileUrlService = fileUrlService;
            _dataDownloader = dataDownloader;
        }

        public async Task Run()
        {
            await GenerateUi(
                new List<MenuAction>
                {
                    new() { Identifier = "1", Description = "Show current file url used for automatic downloads", Action = ShowCurrentFileUrl },
                    new() { Identifier = "2", Description = "Set file url for automatic downloads", Action = SetFileUrl },
                    new() { Identifier = "3", Description = "List all file urls", Action = ListFileUrls },
                });
        }

        private async Task ShowCurrentFileUrl()
        {
            var fileUrl = await _fileUrlService.GetLatest();
            Console.WriteLine(fileUrl?.Url != null ? $"Currently set file url: {fileUrl.Url}" : "Url is not set yet");
        }

        private async Task SetFileUrl()
        {
            Console.WriteLine("Enter the url for automatic file downloads ('b' for back)");
            var input = Console.ReadLine();

            var urlRegex = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");

            if (input == "b") 
                return;

            while (input is null || !urlRegex.IsMatch(input))
            {
                Console.WriteLine("Invalid url. Try again");
                input = Console.ReadLine();

                if (input == "b") 
                    return;

                continue;
            }

            try
            {
                var fileUrl = await _fileUrlService.SetNewFileUrl(input);
                Console.WriteLine($"Url set to: {fileUrl.Url}");
                var res = await _fileUrlService.GetAll();
                res.ForEach(fileUrl => Console.WriteLine($"{fileUrl.Id}, {fileUrl.Url}, {fileUrl.CreatedAt}, {fileUrl.ValidTo}"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task ListFileUrls()
        {
            var fileUrls = await _fileUrlService.GetAll();
            fileUrls.ForEach(fileUrl => Console.WriteLine($"id: {fileUrl.Id}, url: {fileUrl.Url}, createdAt: {fileUrl.CreatedAt}, validTo: {fileUrl?.ValidTo?.ToString() ?? "Null"}"));
        }
    }
}
