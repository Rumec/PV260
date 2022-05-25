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
        public static Holding CreateTeslaHolding()
        {
            return new Holding
            {
                Company = "TESLA INC",
                Currency = "$",
                Cusip = "88160R101",
                Fund = "ARKK",
                MarketValue = 1228003997.14,
                Shares = 1356326,
                Ticker = "TSLA",
                Weight = 9.66
            };
        }
        public static Holding CreateHealthHolding()
        {
            return new Holding
            {
                Company = "TELADOC HEALTH INC",
                Currency = "$",
                Cusip = "87918A105",
                Fund = "ARKK",
                MarketValue = 854796576.32,
                Shares = 12395542,
                Ticker = "TDOC",
                Weight = 6.72
            };
        }
        public static Holding CreateRokuHolding()
        {
            return new Holding
            {
                Company = "ROKU INC",
                Currency = "$",
                Cusip = "77543R102",
                Fund = "ARKK",
                MarketValue = 805584886.70,
                Shares = 6452422,
                Ticker = "ROKU",
                Weight = 6.34
            };
        }
    }
}
