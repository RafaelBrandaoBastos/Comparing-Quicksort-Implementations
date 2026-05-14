using Sorting.Core.Models;
using Sorting.Core.Utils;

namespace Sorting.Core.Data;

public class TestDataGenerator
{
    /// <summary>
    /// Gera um vetor de inteiros de acordo com o padrão solicitado.
    /// O mesmo seed sempre produz a mesma sequência aleatória.
    /// </summary>
    public int[] Generate(int size, DataPattern pattern, int seed)
    {
        Guard.GreaterThanZero(size, nameof(size));

        return pattern switch
        {
            DataPattern.Random           => GenerateRandom(size, seed),
            DataPattern.Sorted           => GenerateSorted(size),
            DataPattern.ReverseSorted    => GenerateReverseSorted(size),
            DataPattern.ManyDuplicates   => GenerateManyDuplicates(size, seed),
            DataPattern.WorstCaseQuickSort => GenerateSorted(size),
            _ => throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null)
        };
    }

    private static int[] GenerateRandom(int size, int seed)
    {
        var rng = new Random(seed);
        var arr = new int[size];
        for (int i = 0; i < size; i++)
            arr[i] = rng.Next(0, 1_000_001);
        return arr;
    }

    private static int[] GenerateSorted(int size)
    {
        var arr = new int[size];
        for (int i = 0; i < size; i++)
            arr[i] = i;
        return arr;
    }

    private static int[] GenerateReverseSorted(int size)
    {
        var arr = new int[size];
        for (int i = 0; i < size; i++)
            arr[i] = size - 1 - i;
        return arr;
    }

    private static int[] GenerateManyDuplicates(int size, int seed)
    {
        var rng = new Random(seed);
        var arr = new int[size];
        for (int i = 0; i < size; i++)
            arr[i] = rng.Next(0, 10);
        return arr;
    }
}
