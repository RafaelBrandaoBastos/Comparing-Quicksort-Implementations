using System.ComponentModel;

namespace Sorting.Core.Models;

public class ExperimentResult
{
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
    public double AvgTimeMs { get; set; }

    [DisplayName("Comp. Médias")]
    public double AvgComparisons { get; set; }

    [DisplayName("Trocas Médias")]
    public double AvgSwaps { get; set; }

    [DisplayName("DP Tempo (ms)")]
    public double StdDevTimeMs { get; set; }
}
