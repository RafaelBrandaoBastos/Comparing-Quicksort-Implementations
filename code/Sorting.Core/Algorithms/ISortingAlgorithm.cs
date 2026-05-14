using Sorting.Core.Models;

namespace Sorting.Core.Algorithms;

public interface ISortingAlgorithm
{
    string Name { get; }
    void Sort(int[] array, SortMetrics metrics, int? m = null);
}
