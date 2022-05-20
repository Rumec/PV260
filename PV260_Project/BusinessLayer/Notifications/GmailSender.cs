using System.Net;
using System.Net.Mail;
using BusinessLayer.Exceptions;
using DataLayer.Models;

namespace BusinessLayer.Notifications;

public class GmailSender : IEmailSender
{
    private readonly IMessageBuilder _messageBuilder;
    private readonly SmtpSettings _smtpSettings;

    public GmailSender(IMessageBuilder messageBuilder, SmtpSettings smtpSettings)
    {
        _messageBuilder = messageBuilder;
        _smtpSettings = smtpSettings;
    }

    public void SendDailyNotification(List<HoldingChanges> holdingChanges, List<Email> recipients)
    {
        var client = new SmtpClient()
        {
            Host = _smtpSettings.Host,
            Port = _smtpSettings.Port,
            EnableSsl = _smtpSettings.EnableSsl,
            Credentials = new NetworkCredential(_smtpSettings.FromAddress, _smtpSettings.Password)
        };

        var message = _messageBuilder.Build(_smtpSettings.FromAddress, recipients.Select(r => r.Address).ToList(),
            "Daily updates - ARK Funds", holdingChanges);

        try
        {
            client.Send(message);
        }
        catch (Exception e)
        {
            throw new EmailSenderException("Sending notifications failed.", e);
        }
    }
}