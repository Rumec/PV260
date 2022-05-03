using System.Globalization;

namespace DataLayer.Models
{
    public class HoldingChanges
    {
        public HoldingBaseInformation Holding { get; set; }
        public long NumberOfShares { get; set; }
        public double DifferenceOfShares { get; set; }
        public double DifferenceOfWeight { get; set; }
        public double MarketValueDifference { get; set; }

        /// <summary>
        /// new CultureInfo("en-US"))
        /// </summary>
        /// <param name="info"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string ToString(CultureInfo info, string separator = ",")
        {
            string result = Holding.ToString(separator);
            result += separator + "\"" + NumberOfShares.ToString("N0", info) + "\"";
            result += separator + "\"" + DifferenceOfShares.ToString("N2", info) + "\"";
            result += separator + "\"" + "$" + MarketValueDifference.ToString("N2", info) + "\"";
            result += separator + DifferenceOfWeight.ToString("N2", info) + "%";

            return result;
        }

        public static string GetHeaderString(string separator = ",")
        {
            return "date" + separator + HoldingBaseInformation.GetHeaderString(separator) + separator + "shares" +
                   separator +
                   "\"shares difference\"" + separator + "\"market value difference ($)\"" + separator +
                   "\"weight difference (%)\"";
        }
    }
}