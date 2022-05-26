using PresentationLayer.UI;
using PresentationLayer.Utils;

namespace PresentationLayer
{
    public class App : IApp
    {
        private readonly IEmailUi _emailUi;
        private readonly IDataSetUi _dataSetUi;
        private readonly IConsoleIoWrapper _consoleIoWrapper;

        public App(IEmailUi emailUi, IDataSetUi dataSetUi, IConsoleIoWrapper consoleIoWrapper) {
            _emailUi = emailUi;
            _dataSetUi = dataSetUi;
            _consoleIoWrapper = consoleIoWrapper;
        }

        public void Run() {
            PrintMenu();
            var input = _consoleIoWrapper.GetInput();
            while (input! != UserInput.Quit) {
                switch (input) {
                    case UserInput.DataSet:
                        // TODO: should be awaited?
                        _dataSetUi.Run();
                        break;
                    case UserInput.Email:
                        _emailUi.Run();
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
                $"{UserInput.Quit}: Quit");
        }
    }
}
