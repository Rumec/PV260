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
    }
}
