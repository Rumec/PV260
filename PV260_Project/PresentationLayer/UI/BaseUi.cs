using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.UI
{
    public abstract class BaseUi
    {
        protected async Task GenerateUi(List<MenuAction> actions)
        {
            PrintMenu(actions);

            var input = Console.ReadLine();
            while (input != "b" && input != "q") {
                var action = actions.FirstOrDefault(x => x.Identifier == input);
                if (action == null) {
                    Console.WriteLine("Incorrect input!");
                    continue;
                }

                await action.Action();
                PrintMenu(actions);
                input = Console.ReadLine();
            }
        }

        private void PrintMenu(List<MenuAction> actions) {
            foreach (var action in actions) {
                Console.WriteLine($"{action.Identifier}: {action.Description}");
            }
        }
    }
}
