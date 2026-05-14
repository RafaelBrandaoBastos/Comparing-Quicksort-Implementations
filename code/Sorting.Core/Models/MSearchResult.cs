namespace Sorting.Core.Models;

public class MSearchResult
{
    public int BestM { get; set; }
    public List<ExperimentResult> Ranking { get; set; } = new();
}
