using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PresentationLayer.Utils;

namespace PresentationLayer.UI
{
    public class BaseUi
    {
        private readonly IConsoleWrapper _consoleWrapper;

        protected BaseUi(IConsoleWrapper consoleWrapper)
        {
            _consoleWrapper = consoleWrapper;
        }
        
        protected async Task GenerateUi(List<MenuAction> actions)
        {
            PrintMenu(actions);

            var input = _consoleWrapper.ReadLine();
            while (input != UserInput.Back && input != UserInput.Quit) {
                var action = actions.FirstOrDefault(x => x.Identifier == input);
                if (action == null) {
                    _consoleWrapper.WriteLine("Incorrect input!");
                    continue;
                }

                await action.Action();
                PrintMenu(actions);
                input = _consoleWrapper.ReadLine();
            }
        }

        private void PrintMenu(List<MenuAction> actions) {
            foreach (var action in actions) {
                _consoleWrapper.WriteLine($"{action.Identifier}: {action.Description}");
            }
        }
    }
}
