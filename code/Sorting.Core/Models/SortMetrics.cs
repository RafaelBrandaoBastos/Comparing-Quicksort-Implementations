namespace Sorting.Core.Models;

public class SortMetrics
{
    public long Comparisons { get; set; }
    public long Swaps { get; set; }
    public long ElapsedTicks { get; set; }
    public double ElapsedMilliseconds { get; set; }

    public void Reset()
    {
        Comparisons = 0;
        Swaps = 0;
        ElapsedTicks = 0;
        ElapsedMilliseconds = 0;
    }

    public void Add(SortMetrics other)
    {
        Comparisons += other.Comparisons;
        Swaps += other.Swaps;
        ElapsedTicks += other.ElapsedTicks;
        ElapsedMilliseconds += other.ElapsedMilliseconds;
    }
}
