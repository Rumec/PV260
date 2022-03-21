using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CsvFile
    {
        public DateTime Date { get; set; }
        public List<Holding> Holdings { get; set; } = new List<Holding>();

    }
}
