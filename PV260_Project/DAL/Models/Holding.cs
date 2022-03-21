using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Holding
    {
        public string Fund { get; set; }
        public string Company { get; set; }
        public string Ticker { get; set; }
        public string Shares { get; set; }
        public double MarketValue { get; set; }
        public double Weight { get; set; }
    }
}
