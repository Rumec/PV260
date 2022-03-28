using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    /// <summary>
    /// Represents single line of holding in csv file
    /// </summary>
    public class Holding : HoldingBaseInformation
    {
        public long Shares { get; set; }
        public double MarketValue { get; set; }
        public double Weight { get; set; }
        public string Currency { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Holding) obj;
            var other2 = (HoldingBaseInformation) other;
            
            return base.Equals(other2) &&
                   !(Math.Abs(this.Shares - other.Shares) > 0.000001) &&
                   !(Math.Abs(this.MarketValue - other.MarketValue) > 0.000001) &&
                   !(Math.Abs(this.Weight - other.Weight) > 0.000001) &&
                   this.Currency == other.Currency;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
