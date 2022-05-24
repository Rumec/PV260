using System.Net.Mail;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using BusinessLayer.Exceptions;
using BusinessLayer.Services;
using PresentationLayer.Utils;

namespace PresentationLayer.UI
{
    public class EmailUi : BaseUi, IEmailUi
    {
        private readonly IUserEmailService _emailService;
        private readonly IConsoleWrapper _consoleWrapper;

        public EmailUi(IUserEmailService emailService, IConsoleWrapper consoleWrapper) : base(consoleWrapper)
        {
            _emailService = emailService;
            _consoleWrapper = consoleWrapper;
        }

        public async Task Run() {
            await GenerateUi(new List<MenuAction>() {
                new MenuAction() {
                    Identifier = UserInput.RegisterEmail,
                    Description = "Register new email",
                    Action = RegisterEmail
                },
                new MenuAction() {
                    Identifier = UserInput.ViewEmails,
                    Description = "View all registered emails",
                    Action = ViewEmails
                },
                new MenuAction() {
                    Identifier = UserInput.DeleteEmail,
                    Description = "Delete email by id",
                    Action = DeleteEmail
                }
            });
        }

        // we can use a regex or MailAddress.TryCreate (or both) to check for validity of input email
        private bool IsValidEmail(string email)
        {
            // RFC 5322 email regex (http://emailregex.com/)
            // var emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            // var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            // var isValidEmail = emailRegex.IsMatch(email);
            
            var isValidEmail = MailAddress.TryCreate(email, out _);
            return isValidEmail;
        }
        
        private async Task RegisterEmail() {
            _consoleWrapper.WriteLine($"Which email address you would like to register? ('{UserInput.Back}' for back)");
            var input = _consoleWrapper.ReadLine();

            if (input == null || input == UserInput.Back)
            {
                return;
            }

            while (input != null && input != UserInput.Back)
            {
                if (!IsValidEmail(input))
                {
                    _consoleWrapper.WriteLine($"Invalid email address. Try again. ('{UserInput.Back}' for back)");
                }
                else
                {
                    await _emailService.RegisterNewEmail(input);
                }

                input = _consoleWrapper.ReadLine();
            }
        }

        private async Task ViewEmails() {
            _consoleWrapper.WriteLine("Printing all registered emails:");
            var emails = await _emailService.GetAllRegisteredEmails();
            foreach (var email in emails) {
                _consoleWrapper.WriteLine(email.ToString());
            }
        }

        private async Task DeleteEmail() {
            _consoleWrapper.WriteLine($"Which email would you like to remove? ('{UserInput.Back}' for back)");
            var input = _consoleWrapper.ReadLine();

            while (input! != UserInput.Back) {
                if (!int.TryParse(input, out _))
                {
                    _consoleWrapper.WriteLine("Id has to be a number!");
                    input = _consoleWrapper.ReadLine();
                    continue;
                }

                try
                {
                    await _emailService.RemoveEmail(int.Parse(input));
                }
                catch (EmailDoesNotExistException)
                {
                    _consoleWrapper.WriteLine("Email with this ID does not exist. Try again");
                }

                input = _consoleWrapper.ReadLine();
            }
        }
    }
}
