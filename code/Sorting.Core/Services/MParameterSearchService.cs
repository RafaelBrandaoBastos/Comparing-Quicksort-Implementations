using Sorting.Core.Algorithms;
using Sorting.Core.Data;
using Sorting.Core.Models;

namespace Sorting.Core.Services;

public class MParameterSearchService
{
    private readonly ExperimentRunner _runner = new();
    private readonly TestDataGenerator _generator = new();

    /// <summary>
    /// Busca empiricamente o melhor valor de M testando cada valor no intervalo
    /// [mStart, mEnd] com passo mStep e ordenando pelo menor AvgTimeMs.
    /// </summary>
    public MSearchResult FindBestM(
        DataPattern pattern,
        int seed,
        int size = 1000,
        int repetitions = 20,
        int mStart = 2,
        int mEnd = 64,
        int mStep = 2,
        bool useMedianOfThree = false)
    {
        var baseArray = _generator.Generate(size, pattern, seed);

        ISortingAlgorithm algorithm = useMedianOfThree
            ? new QuickSortHybridMedianOfThree()
            : new QuickSortHybrid();

        var results = new List<ExperimentResult>();

        for (int m = mStart; m <= mEnd; m += mStep)
        {
            var result = _runner.Run(algorithm, baseArray, repetitions, m, seed, pattern);
            results.Add(result);
        }

        var ranking = results.OrderBy(r => r.AvgTimeMs).ToList();

        return new MSearchResult
        {
            BestM = ranking[0].M ?? mStart,
            Ranking = ranking
        };
    }
}
