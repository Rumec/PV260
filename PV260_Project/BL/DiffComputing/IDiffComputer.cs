using DAL.Models;

namespace BL.DiffComputing;

public interface IDiffComputer
{
    /// <summary>
    /// Computes the diff between two files
    /// </summary>
    /// <param name="first">First file</param>
    /// <param name="second">Second file</param>
    /// <returns>Returns list of changes in holdings</returns>
    public List<HoldingChanges> ComputeDiff(CsvFile first, CsvFile second);
}