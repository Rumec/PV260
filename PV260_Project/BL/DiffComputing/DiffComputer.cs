using DAL.Models;

namespace BL.DiffComputing;

public class DiffComputer : IDiffComputer
{
    private CsvFile _first, _second;

    public DiffComputer(CsvFile first, CsvFile second)
    {
        _first = first;
        _second = second;
    }

    private void AddSymmetricDifference(CsvFile first, CsvFile second,
        ref List<HoldingChanges> diff)
    {
        var firstMinusSecond = first.Holdings
            .Where(h => !second.Holdings.Select(ho => ho.Company)
                .Contains(h.Company))
            .Select(h => new HoldingChanges
            {
                DifferenceOfShares = -h.Shares,
                DifferenceOfWeight = -h.Weight,
                NumberOfShares = h.Shares,
                MarketValueDifference = - h.MarketValue,
                Holding = h
            });

        var secondMinusFirst = second.Holdings
            .Where(h => !first.Holdings.Select(ho => ho.Company)
                .Contains(h.Company))
            .Select(h => new HoldingChanges
            {
                DifferenceOfShares = h.Shares,
                DifferenceOfWeight = h.Weight,
                NumberOfShares = h.Shares,
                MarketValueDifference = h.MarketValue,
                Holding = h
            });
        diff.AddRange(secondMinusFirst);
        diff.AddRange(firstMinusSecond);
    }

    public List<HoldingChanges> ComputeDiff()
    {
        var diff = new List<HoldingChanges>();
        AddSymmetricDifference(_first, _second, ref diff);

        foreach (var holding in _first.Holdings)
        {
            if (diff.Any(hc => hc.Holding.Company.Equals(holding.Company)))
                continue;
            var secondHolding = _second.Holdings.First(h => h.Company.Equals(h.Company));

            diff.Add(new HoldingChanges
            {
                DifferenceOfShares = secondHolding.Shares - holding.Shares,
                DifferenceOfWeight = secondHolding.Weight - holding.Weight,
                NumberOfShares = secondHolding.Shares,
                MarketValueDifference = secondHolding.MarketValue - holding.MarketValue,
                Holding = secondHolding
            });
        }

        return diff;
    }
}