using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace BL.Writers
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
