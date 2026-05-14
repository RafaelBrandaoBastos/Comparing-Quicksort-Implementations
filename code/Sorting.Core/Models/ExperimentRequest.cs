namespace Sorting.Core.Models;

public class ExperimentRequest
{
    public string AlgorithmName { get; set; } = string.Empty;
    public int Size { get; set; }
    public DataPattern Pattern { get; set; }
    public int Repetitions { get; set; }
    public int Seed { get; set; }
    public int? M { get; set; }
    public DateTime StartedAtUtc { get; set; }
}
