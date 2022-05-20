using BusinessLayer.Exceptions;
using DataLayer.Models;

namespace BusinessLayer.Notifications;

public interface IEmailSender
{
    /// <summary>
    /// Sends notification emails to the specified recipients regarding actual holding changes
    /// </summary>
    /// <param name="holdingChanges">list of holding changes</param>
    /// <param name="recipients">recipients of given notification</param>
    /// <exception cref="EmailSenderException">when sending notifications has failed</exception>
    void SendNotification(List<HoldingChanges> holdingChanges, List<Email> recipients);
}