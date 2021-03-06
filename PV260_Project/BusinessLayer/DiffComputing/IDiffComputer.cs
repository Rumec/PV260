using DataLayer.Models;

namespace BusinessLayer.DiffComputing;

public interface IDiffComputer
{
    /// <summary>
    /// Computes the diff between two files
    /// </summary>
    /// <returns>Returns list of changes in holdings</returns>
    public List<HoldingChanges> ComputeDiff(DataSet first, DataSet second);
}