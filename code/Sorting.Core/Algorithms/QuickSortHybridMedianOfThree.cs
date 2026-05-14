using Sorting.Core.Models;

namespace Sorting.Core.Algorithms;

/// <summary>
/// Quicksort híbrido com mediana-de-três para escolha de pivô.
/// A mediana entre array[left], array[mid] e array[right] é calculada,
/// movida para array[right] e usada pela partição Lomuto.
/// </summary>
public class QuickSortHybridMedianOfThree : ISortingAlgorithm
{
    public string Name => "QuickSort Híbrido Mediana-3";

    public void Sort(int[] array, SortMetrics metrics, int? m = null)
    {
        int mVal = m ?? 16;
        QuickSortPartial(array, 0, array.Length - 1, metrics, mVal);
        InsertionSort.SortFull(array, metrics);
    }

    private static void QuickSortPartial(int[] array, int left, int right, SortMetrics metrics, int m)
    {
        if (left >= right) return;

        if (right - left + 1 < m) return;

        PlaceMedianAsPivot(array, left, right, metrics);

        int p = Partition(array, left, right, metrics);
        QuickSortPartial(array, left, p - 1, metrics, m);
        QuickSortPartial(array, p + 1, right, metrics, m);
    }

    /// <summary>
    /// Ordena as posições left, mid e right para que left ≤ mid ≤ right,
    /// depois move a mediana (mid) para a posição right (pivot do Lomuto).
    /// </summary>
    private static void PlaceMedianAsPivot(int[] array, int left, int right, SortMetrics metrics)
    {
        int mid = left + (right - left) / 2;

        // Ordenar as três posições
        metrics.Comparisons++;
        if (array[left] > array[mid])
        {
            (array[left], array[mid]) = (array[mid], array[left]);
            metrics.Swaps++;
        }

        metrics.Comparisons++;
        if (array[left] > array[right])
        {
            (array[left], array[right]) = (array[right], array[left]);
            metrics.Swaps++;
        }

        metrics.Comparisons++;
        if (array[mid] > array[right])
        {
            (array[mid], array[right]) = (array[right], array[mid]);
            metrics.Swaps++;
        }

        // Após as três comparações: array[left] ≤ array[mid] ≤ array[right]
        // Mediana está em mid; movê-la para right para uso pelo Lomuto
        if (mid != right)
        {
            (array[mid], array[right]) = (array[right], array[mid]);
            metrics.Swaps++;
        }
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
