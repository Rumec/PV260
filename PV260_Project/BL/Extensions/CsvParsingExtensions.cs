using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extensions
{
    public static class CsvParsingExtensions
    {
        public static DateTime GetDateTime(this string stringDate) {
            string[] formats = { "MM/dd/yyyy" };
            var dateTime = DateTime.ParseExact(stringDate, formats, new CultureInfo("en-US"), DateTimeStyles.None);
            return dateTime;
        }

        public static Tuple<string, double> GetCurrencyAndValue(this string value) {
            var number = double.Parse(value.Remove(0, 1).Replace(",", ""), CultureInfo.InvariantCulture);
            var currency = value[0].ToString();
            return new Tuple<string, double>(currency, number);
        }
    }
}
