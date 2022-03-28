using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    /// <summary>
    /// These are the data, which do not change in time (at least I think)
    /// </summary>
    public class HoldingBaseInformation
    {
        public string Fund { get; set; }
        public string Company { get; set; }
        public string Ticker { get; set; }
        public string Cusip { get; set; }
        
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            HoldingBaseInformation other = (HoldingBaseInformation) obj;
            
            return this.Fund == other.Fund && 
                   this.Company == other.Company && 
                   this.Ticker == other.Ticker &&
                   this.Cusip == other.Cusip;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
