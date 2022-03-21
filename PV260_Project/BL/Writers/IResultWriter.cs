using DAL.Models;

namespace BL.Writers;

public interface IResultWriter
{
    /// <summary>
    /// Writes the changes to the output based on the implementation
    /// </summary>
    /// <param name="holdingChanges"></param>
    void Print(List<HoldingChanges> holdingChanges);
}