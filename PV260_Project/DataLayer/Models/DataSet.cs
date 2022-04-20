namespace DAL.Models
{
    /// <summary>
    /// Represents a data item, which is downloaded at https://ark-funds.com/funds/arkk/
    /// </summary>
    public class DataSet
    {
        public DateTime Date { get; set; }
        public List<Holding> Holdings { get; set; } = new List<Holding>();
    }
}
