using Sorting.Core.Models;

namespace Sorting.Core.Algorithms;

public class InsertionSort : ISortingAlgorithm
{
    public string Name => "Insertion Sort";

    public void Sort(int[] array, SortMetrics metrics, int? m = null)
        => SortFull(array, metrics);

    /// <summary>
    /// Versão por trocas adjacentes para manter métricas comparáveis com Quicksort.
    /// Cada deslocamento é contabilizado como uma troca explícita.
    /// </summary>
    public static void SortFull(int[] array, SortMetrics metrics)
    {
        for (int i = 1; i < array.Length; i++)
        {
            int j = i;
            while (j > 0)
            {
                metrics.Comparisons++;
                if (array[j] < array[j - 1])
                {
                    (array[j], array[j - 1]) = (array[j - 1], array[j]);
                    metrics.Swaps++;
                    j--;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
