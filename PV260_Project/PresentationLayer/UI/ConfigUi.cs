using BusinessLayer.DataLoading;
using BusinessLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PresentationLayer.Utils;

namespace PresentationLayer.UI
{
    internal class ConfigUi : BaseUi, IConfigUi
    {
        private readonly IFileUrlService _fileUrlService;
        private readonly IConsoleIoWrapper _consoleIoWrapper;

        public ConfigUi(IFileUrlService fileUrlService, IConsoleIoWrapper consoleIoWrapper) : base(consoleIoWrapper)
        {
            _fileUrlService = fileUrlService;
            _consoleIoWrapper = consoleIoWrapper;
        }

        public async Task Run()
        {
            await GenerateUi(
                new List<MenuAction>
                {
                    new() { Identifier = UserInput.ShowCurrentFileUrl, Description = "Show current file url used for automatic downloads", Action = ShowCurrentFileUrl },
                    new() { Identifier = UserInput.SetFileUrl, Description = "Set file url for automatic downloads", Action = SetFileUrl },
                    new() { Identifier = UserInput.ListFileUrls, Description = "List all file urls", Action = ListFileUrls },
                });
        }

        private async Task ShowCurrentFileUrl()
        {
            var fileUrl = await _fileUrlService.GetLatest();
            _consoleIoWrapper.ShowMessage(fileUrl?.Url != null ? $"Currently set file url: {fileUrl.Url}" : "Url is not set yet");
        }

        private async Task SetFileUrl()
        {
            _consoleIoWrapper.ShowMessage($"Enter the url for automatic file downloads ('{UserInput.Back}' for back)");
            var input = _consoleIoWrapper.GetInput();

            var urlRegex = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");

            if (input == UserInput.Back) 
                return;

            while (input is null || !urlRegex.IsMatch(input))
            {
                _consoleIoWrapper.ShowMessage("Invalid url. Try again");
                input = _consoleIoWrapper.GetInput();

                if (input == UserInput.Back) 
                    return;

                continue;
            }

            try
            {
                var fileUrl = await _fileUrlService.SetNewFileUrl(input);
                _consoleIoWrapper.ShowMessage($"Url set to: {fileUrl.Url}");
            }
            catch (Exception e)
            {
                _consoleIoWrapper.ShowMessage(e.Message);
            }
        }

        private async Task ListFileUrls()
        {
            var fileUrls = await _fileUrlService.GetAll();
            fileUrls.ForEach(fileUrl => _consoleIoWrapper.ShowMessage($"id: {fileUrl.Id}, url: {fileUrl.Url}, createdAt: {fileUrl.CreatedAt}, validTo: {fileUrl?.ValidTo?.ToString() ?? "Null"}"));
        }
    }
}
