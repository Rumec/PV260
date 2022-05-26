using PresentationLayer.UI;
using PresentationLayer.Utils;
using BusinessLayer.Jobs;

namespace PresentationLayer
{
    public class App : IApp
    {
        private readonly IEmailUi _emailUi;
        private readonly IDataSetUi _dataSetUi;
        private readonly IConsoleIoWrapper _consoleIoWrapper;
        private readonly IConfigUi _configUi;
        private readonly IDataSyncJob _dataSyncJob;

        public App(IEmailUi emailUi, IDataSetUi dataSetUi, IDataSyncJob dataSyncJob, IConfigUi configUi, IConsoleIoWrapper consoleIoWrapper) {
            _emailUi = emailUi;
            _dataSetUi = dataSetUi;
            _configUi = configUi;
            _dataSyncJob = dataSyncJob;
            _consoleIoWrapper = consoleIoWrapper;
        }

        public async Task Run() {
            _dataSyncJob.Run();
            PrintMenu();
            var input = _consoleIoWrapper.GetInput();
            while (input! != UserInput.Quit) {
                switch (input) {
                    case UserInput.DataSet:
                        await _dataSetUi.Run();
                        break;
                    case UserInput.Email:
                        await _emailUi.Run();
                        break;
                    case "3":
                        await _configUi.Run();
                        break;
                    default:
                        _consoleIoWrapper.ShowMessage(Messages.InvalidInput);
                        break;
                }
                PrintMenu();
                input = _consoleIoWrapper.GetInput();
            }
            _consoleIoWrapper.ShowMessage(Messages.Quitting);
        }

        private void PrintMenu() { 
            _consoleIoWrapper.ShowMessage(
                "What do you want to work with:\n" +
                $"{UserInput.DataSet}: Data sets\n" +
                $"{UserInput.Email}: Emails\n" +
                $"{UserInput.Config}: Config\n" +
                $"{UserInput.Quit}: Quit");
        }
    }
}
