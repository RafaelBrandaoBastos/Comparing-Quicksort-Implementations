using System.ComponentModel;

namespace Sorting.Core.Models;

public class ExperimentResult
{
    private double _avgTimeMs;
    private double _avgComparisons;
    private double _avgSwaps;
    private double _stdDevTimeMs;

    [DisplayName("ID Execução")]
    public string RunId { get; set; } = string.Empty;

    [DisplayName("Data/Hora (UTC)")]
    public DateTime TimestampUtc { get; set; }

    [DisplayName("Algoritmo")]
    public string AlgorithmName { get; set; } = string.Empty;

    [DisplayName("N")]
    public int Size { get; set; }

    [DisplayName("Padrão")]
    public string Pattern { get; set; } = string.Empty;

    [DisplayName("Reps.")]
    public int Repetitions { get; set; }

    [DisplayName("Seed")]
    public int Seed { get; set; }

    [DisplayName("M")]
    public int? M { get; set; }

    [DisplayName("T. Médio (ms)")]
    public double AvgTimeMs
    {
        get => _avgTimeMs;
        set => _avgTimeMs = Round4(value);
    }

    [DisplayName("Comp. Médias")]
    public double AvgComparisons
    {
        get => _avgComparisons;
        set => _avgComparisons = Round4(value);
    }

    [DisplayName("Trocas Médias")]
    public double AvgSwaps
    {
        get => _avgSwaps;
        set => _avgSwaps = Round4(value);
    }

    [DisplayName("DP Tempo (ms)")]
    public double StdDevTimeMs
    {
        get => _stdDevTimeMs;
        set => _stdDevTimeMs = Round4(value);
    }

    private static double Round4(double value)
        => Math.Round(value, 4, MidpointRounding.AwayFromZero);
}
