using System.Net.Mail;
using DataLayer.Models;

namespace BusinessLayer.Notifications;

public interface IMessageBuilder
{
    /// <summary>
    /// Returns generated MailMessage from parameters based on the implementation
    /// </summary>
    /// <param name="sender">email of a sender</param>
    /// <param name="recipients">emails of recipients</param>
    /// <param name="subject">subject of email</param>
    /// <param name="holdingChanges">list of holding changes to be used in email body</param>
    MailMessage Build(string sender, List<string> recipients, string subject, List<HoldingChanges> holdingChanges);

}