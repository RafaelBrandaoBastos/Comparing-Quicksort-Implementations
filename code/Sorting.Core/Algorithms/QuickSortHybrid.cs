using Sorting.Core.Models;

namespace Sorting.Core.Algorithms;

/// <summary>
/// Quicksort híbrido: interrompe a partição para subarrays menores que M
/// e finaliza com Insertion Sort no vetor inteiro.
/// </summary>
public class QuickSortHybrid : ISortingAlgorithm
{
    public string Name => "QuickSort Híbrido";

    public void Sort(int[] array, SortMetrics metrics, int? m = null)
    {
        int mVal = m ?? 16;
        QuickSortPartial(array, 0, array.Length - 1, metrics, mVal);
        InsertionSort.SortFull(array, metrics);
    }

    private static void QuickSortPartial(int[] array, int left, int right, SortMetrics metrics, int m)
    {
        if (left >= right) return;

        // Subarray menor que M: deixa para o InsertionSort final
        if (right - left + 1 < m) return;

        int p = Partition(array, left, right, metrics);
        QuickSortPartial(array, left, p - 1, metrics, m);
        QuickSortPartial(array, p + 1, right, metrics, m);
    }

    private static int Partition(int[] array, int left, int right, SortMetrics metrics)
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
