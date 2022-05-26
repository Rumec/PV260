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
        private readonly IConsoleIoWrapper _consoleIoWrapper;

        public EmailUi(IUserEmailService emailService, IConsoleIoWrapper consoleIoWrapper) : base(consoleIoWrapper)
        {
            _emailService = emailService;
            _consoleIoWrapper = consoleIoWrapper;
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

        // we can use a regex or MailAddress.TryCreate (or both) to check the validity of input email
        private bool IsValidEmailAddress(string address)
        {
            // RFC 5322 email regex (http://emailregex.com/)
            // var emailRegex = new Regex(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
            // var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            // var isValidEmail = emailRegex.IsMatch(email);
            
            return MailAddress.TryCreate(address, out _);
        }
        
        private async Task RegisterEmail() {
            _consoleIoWrapper.ShowMessage(Messages.RegisterEmail);
            var input = _consoleIoWrapper.GetInput();

            if (input == null || input == UserInput.Back)
            {
                return;
            }

            while (input != null && input != UserInput.Back)
            {
                if (!IsValidEmailAddress(input))
                {
                    _consoleIoWrapper.ShowMessage(Messages.InvalidEmailAddress);
                }
                else
                {
                    await _emailService.RegisterNewEmail(input);
                    _consoleIoWrapper.ShowMessage(Messages.InputAnotherEmailAddress);
                }
                input = _consoleIoWrapper.GetInput();
            }
        }

        private async Task ViewEmails() {
            _consoleIoWrapper.ShowMessage(Messages.ViewEmails);
            var emails = await _emailService.GetAllRegisteredEmails();
            foreach (var email in emails) {
                _consoleIoWrapper.ShowMessage(Messages.PrintEmail(email));
            }
        }

        private async Task DeleteEmail()
        {
            _consoleIoWrapper.ShowMessage(Messages.DeleteEmail);
            var input = _consoleIoWrapper.GetInput();
            
            while (input! != UserInput.Back) {

                if (!int.TryParse(input, out _))
                {
                    _consoleIoWrapper.ShowMessage(Messages.InvalidIdFormat);
                    input = _consoleIoWrapper.GetInput();
                    continue;
                }

                try
                {
                    await _emailService.RemoveEmail(int.Parse(input));
                    _consoleIoWrapper.ShowMessage(Messages.InputAnotherEmailId);
                }
                catch (EmailDoesNotExistException)
                {
                    _consoleIoWrapper.ShowMessage(Messages.EmailDoesNotExist);
                }
                
                input = _consoleIoWrapper.GetInput();
            }
        }
    }
}
