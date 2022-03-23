﻿using System;
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
    }
}
