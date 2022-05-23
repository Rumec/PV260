using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.BLTests
{
    public static class TestUtils
    {
        public static HoldingChanges CreateTeslaHoldingChanges()
        {
            HoldingBaseInformation teslaTestHoldingInfo = new HoldingBaseInformation();
            teslaTestHoldingInfo.Company = "TESLA INC";
            teslaTestHoldingInfo.Cusip = "88160R101";
            teslaTestHoldingInfo.Fund = "ARKK";
            teslaTestHoldingInfo.Ticker = "TSLA";

            return new HoldingChanges()
            {
                Holding = teslaTestHoldingInfo,
                NumberOfShares = 1356326,
                DifferenceOfShares = 10,
                DifferenceOfWeight = 9.66,
                MarketValueDifference = 1228003997.14
            };
        }

        public static HoldingChanges CreateHealthHoldingChanges()
        {
            HoldingBaseInformation healthTestHoldingInfo = new HoldingBaseInformation();
            healthTestHoldingInfo.Company = "TELADOC HEALTH INC";
            healthTestHoldingInfo.Cusip = "87918A105";
            healthTestHoldingInfo.Fund = "ARKK";
            healthTestHoldingInfo.Ticker = "TDOC";


            return new HoldingChanges()
            {
                Holding = healthTestHoldingInfo,
                NumberOfShares = 12395542,
                DifferenceOfShares = -20,
                DifferenceOfWeight = -6.72,
                MarketValueDifference = 854796576.32
            };
        }

        public static HoldingChanges CreateRokuHoldingChanges()
        {
            HoldingBaseInformation healthTestHoldingInfo = new HoldingBaseInformation();
            healthTestHoldingInfo.Company = "ROKU INC";
            healthTestHoldingInfo.Cusip = "77543R102";
            healthTestHoldingInfo.Fund = "ARKK";
            healthTestHoldingInfo.Ticker = "ROKU";

            return new HoldingChanges()
            {
                Holding = healthTestHoldingInfo,
                NumberOfShares = 6452422,
                DifferenceOfShares = 5,
                DifferenceOfWeight = 6.34,
                MarketValueDifference = 805584886.70
            };
        }
    }
}
