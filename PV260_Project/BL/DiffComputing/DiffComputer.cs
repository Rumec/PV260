using DAL.Models;

namespace BL.DiffComputing;

public class DiffComputer : IDiffComputer
{
    private readonly CsvFile _first, _second;

    public DiffComputer(CsvFile first, CsvFile second)
    {
        _first = first;
        _second = second;
    }

    public List<HoldingChanges> ComputeDiff()
    {
        var diff = new List<HoldingChanges>();
        AddSymmetricDifference(_first, _second, ref diff);

        foreach (var holding in _first.Holdings)
        {
            if (diff.Any(hc => hc.Holding.Company.Equals(holding.Company)))
                continue;
            var secondHolding = _second.Holdings.First(h => h.Company.Equals(holding.Company));

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

    private void AddSymmetricDifference(CsvFile first, CsvFile second,
        ref List<HoldingChanges> diff)
    {
        diff.AddRange(CalculateRelativeComplement(first, second, false));
        diff.AddRange(CalculateRelativeComplement(second, first, true));
    }

    private IEnumerable<HoldingChanges> CalculateRelativeComplement(CsvFile a, CsvFile b, bool expectsNegativeDiffs)
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