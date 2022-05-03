using System.Globalization;
using DataLayer.Models;

namespace BusinessLayer.Writers
{
    public static class DiffStringGenerator
    {
        public static string GenerateSeparatedString(List<HoldingChanges> holdingChanges,
            CultureInfo info, DateTime? date=null, string separator=",")
        {
            date = date ?? DateTime.Today;
            var strDate = date?.ToString("MM/dd/yyyy", info) + separator;

            string result = HoldingChanges.GetHeaderString(separator) + Environment.NewLine;
            return holdingChanges.Aggregate(result, (current, change) => 
                current + (strDate + change.ToString(info, separator) + Environment.NewLine));
        }
    }
}
