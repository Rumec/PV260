using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class HoldingChanges
    {
        public HoldingBaseInformation Holding { get; set; }
        public long NumberOfShares { get; set; }
        public double DifferenceOfShares { get; set; }
        public double DifferenceOfWeight { get; set; }
    }
}
