using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    /// <summary>
    /// Represents csv file, which is downloaded at https://ark-funds.com/funds/arkk/
    /// </summary>
    public class CsvFile
    {
        public DateTime Date { get; set; }
        public List<Holding> Holdings { get; set; } = new List<Holding>();
        
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (CsvFile) obj;
            if (this.Date != other.Date || Holdings.Count != other.Holdings.Count)
            {
                return false;
            }

            return !Holdings.Where((t, i) => !t.Equals(other.Holdings[i])).Any();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
