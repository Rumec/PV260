using PresentationLayer.UI;
using PresentationLayer.Utils;

namespace PresentationLayer
{
    public class App : IApp
    {
        private readonly IEmailUi _emailUi;
        private readonly IDataSetUi _dataSetUi;
        private readonly IConsoleWrapper _consoleWrapper;

        public App(IEmailUi emailUi, IDataSetUi dataSetUi, IConsoleWrapper consoleWrapper) {
            _emailUi = emailUi;
            _dataSetUi = dataSetUi;
            _consoleWrapper = consoleWrapper;
        }

        public void Run() {
            PrintMenu();
            var input = _consoleWrapper.ReadLine();
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
                        _consoleWrapper.WriteLine(Messages.InvalidInput);
                        break;
                }
                PrintMenu();
                input = _consoleWrapper.ReadLine();
            }
            _consoleWrapper.WriteLine(Messages.Quitting);
        }

        private void PrintMenu() { 
            _consoleWrapper.WriteLine(
                "What do you want to work with:\n" +
                $"{UserInput.DataSet}: Data sets\n" +
                $"{UserInput.Email}: Emails\n" +
                $"{UserInput.Quit}: Quit");
        }
    }
}
