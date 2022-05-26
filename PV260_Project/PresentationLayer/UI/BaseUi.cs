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
        private readonly IConsoleIoWrapper _consoleIoWrapper;

        protected BaseUi(IConsoleIoWrapper consoleIoWrapper)
        {
            _consoleIoWrapper = consoleIoWrapper;
        }
        
        protected async Task GenerateUi(List<MenuAction> actions)
        {
            PrintMenu(actions);

            var input = _consoleIoWrapper.GetInput();
            while (input != UserInput.Back && input != UserInput.Quit) {
                var action = actions.FirstOrDefault(x => x.Identifier == input);
                if (action == null) {
                    _consoleIoWrapper.ShowMessage(Messages.InvalidInput);
                    continue;
                }

                await action.Action();
                PrintMenu(actions);
                input = _consoleIoWrapper.GetInput();
            }
        }

        private void PrintMenu(List<MenuAction> actions) {
            foreach (var action in actions) {
                _consoleIoWrapper.ShowMessage($"{action.Identifier}: {action.Description}");
            }
        }
    }
}
