using System.Net.Mail;
using System.Text;
using DataLayer.Models;

namespace BusinessLayer.Notifications;

public class HtmlMessageBuilder : IMessageBuilder
{
    public MailMessage Build(string sender, List<string> recipients, string subject,
        List<HoldingChanges> holdingChanges)
    {
        var message = new MailMessage();
        message.From = new MailAddress(sender);
        message.Subject = subject;
        message.Body = BuildBody(holdingChanges);
        message.IsBodyHtml = true;
        recipients.ForEach(r => message.To.Add(r));

        return message;
    }

    private string BuildBody(List<HoldingChanges> holdingChanges)
    {
        var builder = new StringBuilder();

        builder.AppendLine(BuildElement("h1", $"Daily updates ({DateTime.Today:d}) - ARK Funds"));

        builder.AppendLine("<table>");
        builder.AppendLine("<tr>");
        builder.AppendLine(BuildTableHeaderColumn("Fund"));
        builder.AppendLine(BuildTableHeaderColumn("Company"));
        builder.AppendLine(BuildTableHeaderColumn("Ticker"));
        builder.AppendLine(BuildTableHeaderColumn("Cusip"));
        builder.AppendLine(BuildTableHeaderColumn("Shares"));
        builder.AppendLine(BuildTableHeaderColumn("Shares difference"));
        builder.AppendLine(BuildTableHeaderColumn("Market value difference ($)"));
        builder.AppendLine(BuildTableHeaderColumn("Weight difference (%)"));
        builder.AppendLine("</tr>");

        foreach (var change in holdingChanges)
        {
            builder.AppendLine("<tr>");
            builder.AppendLine(BuildTableDataColumn(change.Holding.Fund));
            builder.AppendLine(BuildTableDataColumn(change.Holding.Company));
            builder.AppendLine(BuildTableDataColumn(change.Holding.Ticker));
            builder.AppendLine(BuildTableDataColumn(change.Holding.Cusip));
            builder.AppendLine(BuildTableDataColumn(change.NumberOfShares));
            builder.AppendLine(BuildTableDataColumn(change.DifferenceOfShares));
            builder.AppendLine(BuildTableDataColumn(change.MarketValueDifference));
            builder.AppendLine(BuildTableDataColumn(change.DifferenceOfWeight));
            builder.AppendLine("</tr>");
        }

        builder.AppendLine("</table>");

        return builder.ToString();
    }

    private string BuildTableHeaderColumn(string value)
    {
        return BuildElement("th", value);
    }

    private string BuildTableDataColumn(object value)
    {
        return BuildElement("td", value);
    }

    private string BuildElement(string tag, object value)
    {
        return $"<{tag}>{value}</{tag}>";
    }
}