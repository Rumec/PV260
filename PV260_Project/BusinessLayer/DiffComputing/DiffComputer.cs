using DataLayer.Models;

namespace BusinessLayer.DiffComputing;

public class DiffComputer : IDiffComputer
{
    public List<HoldingChanges> ComputeDiff(DataSet first, DataSet second)
    {
        var diff = new List<HoldingChanges>();
        AddSymmetricDifference(first, second, ref diff);

        foreach (var holding in first.Holdings)
        {
            if (diff.Any(hc => hc.Holding.Company.Equals(holding.Company)))
                continue;
            var secondHolding = second.Holdings.First(h => h.Company.Equals(holding.Company));

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

    private void AddSymmetricDifference(DataSet first, DataSet second,
        ref List<HoldingChanges> diff)
    {
        diff.AddRange(CalculateRelativeComplement(first, second, false));
        diff.AddRange(CalculateRelativeComplement(second, first, true));
    }

    private IEnumerable<HoldingChanges> CalculateRelativeComplement(DataSet a, DataSet b, bool expectsNegativeDiffs)
    {
        var sign = expectsNegativeDiffs ? -1 : 1;
        return b.Holdings
            .Where(h => !a.Holdings.Select(ho => ho.Company)
                .Contains(h.Company))
            .Select(h => new HoldingChanges
            {
                DifferenceOfShares = sign * h.Shares,
                DifferenceOfWeight = sign * h.Weight,
                NumberOfShares = h.Shares,
                MarketValueDifference = sign * h.MarketValue,
                Holding = h
            });
    }
}