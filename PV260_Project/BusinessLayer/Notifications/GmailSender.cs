using System.Net;
using System.Net.Mail;
using BusinessLayer.Exceptions;
using DataLayer.Models;
using Microsoft.Extensions.Options;

namespace BusinessLayer.Notifications;

public class GmailSender : IEmailSender
{
    private readonly IMessageBuilder _messageBuilder;
    private readonly IOptions<SmtpSettings> _smtpSettings;

    public GmailSender(IMessageBuilder messageBuilder, IOptions<SmtpSettings> smtpSettings)
    {
        _messageBuilder = messageBuilder;
        _smtpSettings = smtpSettings;
    }

    public void SendNotification(List<HoldingChanges> holdingChanges, List<Email> recipients)
    {
        var client = new SmtpClient()
        {
            Host = _smtpSettings.Value.Host,
            Port = _smtpSettings.Value.Port,
            EnableSsl = _smtpSettings.Value.EnableSsl,
            Credentials = new NetworkCredential(_smtpSettings.Value.UserName, _smtpSettings.Value.Password)
        };

        var message = _messageBuilder.Build(_smtpSettings.Value.FromAddress, recipients.Select(r => r.Address).ToList(),
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