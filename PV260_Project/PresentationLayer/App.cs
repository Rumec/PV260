using PresentationLayer.UI;

namespace PresentationLayer
{
    public class App : IApp
    {
        private readonly IEmailUi _emailUi;
        private readonly IDataSetUi _dataSetUi;

        public App(IEmailUi emailUi, IDataSetUi dataSetUi) {
            _emailUi = emailUi;
            _dataSetUi = dataSetUi;
        }

        public void Run() {
            PrintMenu();
            var input = Console.ReadLine();
            while (input! != "q") {
                switch (input) {
                    case "1":
                        _dataSetUi.Run();
                        break;
                    case "2":
                        _emailUi.Run();
                        break;
                    default:
                        Console.WriteLine("Incorrect input!");
                        break;
                }
                PrintMenu();
                input = Console.ReadLine();
            }
            Console.WriteLine("Quitting...");
        }

        private void PrintMenu() { 
            Console.WriteLine(
                "What do you want to work with:\n" +
                "1: Data sets\n" +
                "2: Emails\n" +
                "q: Quit");
        }
    }
}
