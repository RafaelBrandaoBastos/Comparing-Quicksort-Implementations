using System.Diagnostics;
using Sorting.Core.Algorithms;
using Sorting.Core.Models;
using Sorting.Core.Utils;

namespace Sorting.Core.Services;

public class ExperimentRunner
{
    /// <summary>
    /// Executa o algoritmo sobre clones de baseArray, repetindo conforme solicitado,
    /// e retorna as médias e desvio padrão das métricas coletadas.
    /// </summary>
    public ExperimentResult Run(
        ISortingAlgorithm algorithm,
        int[] baseArray,
        int repetitions,
        int? m,
        int seed,
        DataPattern pattern)
    {
        Guard.NotNull(algorithm, nameof(algorithm));
        Guard.NotNull(baseArray, nameof(baseArray));
        Guard.GreaterThanZero(repetitions, nameof(repetitions));
        Guard.GreaterThanZero(baseArray.Length, nameof(baseArray));

        var times = new double[repetitions];
        var comparisons = new long[repetitions];
        var swaps = new long[repetitions];

        var metrics = new SortMetrics();
        var sw = new Stopwatch();

        for (int i = 0; i < repetitions; i++)
        {
            var copy = (int[])baseArray.Clone();
            metrics.Reset();

            sw.Restart();
            algorithm.Sort(copy, metrics, m);
            sw.Stop();

            metrics.ElapsedTicks = sw.ElapsedTicks;
            metrics.ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds;

            times[i] = metrics.ElapsedMilliseconds;
            comparisons[i] = metrics.Comparisons;
            swaps[i] = metrics.Swaps;
        }

        double avgTime = times.Average();
        double avgComp = comparisons.Average(x => (double)x);
        double avgSwap = swaps.Average(x => (double)x);
        double stdDev = Math.Sqrt(times.Average(t => Math.Pow(t - avgTime, 2)));

        return new ExperimentResult
        {
            RunId = Guid.NewGuid().ToString(),
            TimestampUtc = DateTime.UtcNow,
            AlgorithmName = algorithm.Name,
            Size = baseArray.Length,
            Pattern = pattern.ToString(),
            Repetitions = repetitions,
            Seed = seed,
            M = m,
            AvgTimeMs = avgTime,
            AvgComparisons = avgComp,
            AvgSwaps = avgSwap,
            StdDevTimeMs = stdDev
        };
    }
}
