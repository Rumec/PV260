using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationLayer.UI;

namespace PresentationLayer.ConsoleApps
{
    public class App : IApp
    {
        private readonly IEmailUi _emailUi;

        public App(IEmailUi emailUi) {
            _emailUi = emailUi;
        }

        public void Run() {
            PrintMenu();
            var input = Console.ReadLine();
            while (input! != "q") {
                switch (input) {
                    case "1":
                        break;
                    case "2":
                        _emailUi.Run();
                        break;
                    default:
                        Console.WriteLine("Incorrect input!");
                        break;
                }
                input = Console.ReadLine();
                PrintMenu();
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
