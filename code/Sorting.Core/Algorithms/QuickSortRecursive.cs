using Sorting.Core.Models;

namespace Sorting.Core.Algorithms;

/// <summary>
/// Quicksort recursivo padrão com pivô no último elemento (partição Lomuto).
/// </summary>
public class QuickSortRecursive : ISortingAlgorithm
{
    public string Name => "QuickSort Recursivo";

    public void Sort(int[] array, SortMetrics metrics, int? m = null)
        => QuickSort(array, 0, array.Length - 1, metrics);

    private static void QuickSort(int[] array, int left, int right, SortMetrics metrics)
    {
        if (left >= right) return;

        int p = Partition(array, left, right, metrics);
        QuickSort(array, left, p - 1, metrics);
        QuickSort(array, p + 1, right, metrics);
    }

    internal static int Partition(int[] array, int left, int right, SortMetrics metrics)
    {
        int pivot = array[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            metrics.Comparisons++;
            if (array[j] <= pivot)
            {
                i++;
                if (i != j)
                {
                    (array[i], array[j]) = (array[j], array[i]);
                    metrics.Swaps++;
                }
            }
        }

        if (i + 1 != right)
        {
            (array[i + 1], array[right]) = (array[right], array[i + 1]);
            metrics.Swaps++;
        }

        return i + 1;
    }
}
