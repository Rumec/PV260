namespace DataLayer.Models
{
    /// <summary>
    /// These are the data, which do not change in time (at least I think)
    /// </summary>
    public class HoldingBaseInformation : BaseEntity
    {
        public string Fund { get; set; }
        public string Company { get; set; }
        public string Ticker { get; set; }
        public string Cusip { get; set; }

        public static string GetHeaderString(string separator = ",")
        {
            return "fund" + separator + "company" + separator + "ticker" + separator + "cusip";
        }

        public string ToString(string separator = ",")
        {
            return Fund + separator + "\"" + Company + "\"" + separator + Ticker + separator + Cusip;
        }
    }
}