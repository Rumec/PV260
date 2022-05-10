using System.Net.Sockets;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;

namespace PresentationLayer.UI
{
    public class EmailUi : BaseUi, IEmailUi
    {
        private readonly IUserEmailService _emailService;

        public EmailUi(IUserEmailService emailService) {
            _emailService = emailService;
        }

        public async Task Run() {
            await GenerateUi(new List<MenuAction>() {
                new MenuAction() {
                    Identifier = "1",
                    Description = "Register new email",
                    Action = RegisterEmail
                },
                new MenuAction() {
                    Identifier = "2",
                    Description = "View all registered emails",
                    Action = ViewEmails
                },
                new MenuAction() {
                    Identifier = "3",
                    Description = "Delete email by id",
                    Action = DeleteEmail
                }
            });
        }

        private async Task RegisterEmail() {
            Console.WriteLine("Which email address you would like to register?");
            var address = Console.ReadLine();

            await _emailService.RegisterNewEmail(address!);
        }

        private async Task ViewEmails() {
            Console.WriteLine("Printing all registered emails:");
            var emails = await _emailService.GetAllRegisteredEmails();
            foreach (var email in emails) {
                Console.WriteLine($"Id: {email.Id}, address: {email.Address}");
            }
        }

        private async Task DeleteEmail() {
            Console.WriteLine("Which email would you like to remove? ('b' for back)");
            var input = Console.ReadLine();

            while (input! != "b") {
                while (!int.TryParse(input, out _))
                {
                    Console.WriteLine("Id has to be a number!");
                }

                try
                {
                    await _emailService.RemoveEmail(int.Parse(input));
                }
                catch (EmailDoesNotExistException)
                {
                    Console.WriteLine("Email with this ID does not exist. Try again");
                }

                input = Console.ReadLine();
            }
        }
    }
}
