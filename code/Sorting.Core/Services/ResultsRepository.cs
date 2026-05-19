using System.Text.Json;
using Sorting.Core.Models;

namespace Sorting.Core.Services;

public class ResultsRepository
{
    private static readonly string BaseDir =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resultados");

    private static readonly string JsonlPath =
        Path.Combine(BaseDir, "historico_resultados.jsonl");

    private static readonly string CsvPath =
        Path.Combine(BaseDir, "historico_resultados.csv");

    private const string CsvHeader =
        "RunId,TimestampUtc,AlgorithmName,Size,Pattern,Repetitions,Seed,M," +
        "AvgTimeMs,AvgComparisons,AvgSwaps,StdDevTimeMs";

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = false
    };

    public void SaveResult(ExperimentResult result) => SaveResults([result]);

    public void SaveResults(IEnumerable<ExperimentResult> results)
    {
        Directory.CreateDirectory(BaseDir);

        var list = results.ToList();

        using (var jsonlWriter = File.AppendText(JsonlPath))
        {
            foreach (var r in list)
                jsonlWriter.WriteLine(JsonSerializer.Serialize(r, JsonOpts));
        }

        bool needsHeader = !File.Exists(CsvPath) || new FileInfo(CsvPath).Length == 0;
        using var csvWriter = File.AppendText(CsvPath);
        if (needsHeader)
            csvWriter.WriteLine(CsvHeader);

        foreach (var r in list)
            csvWriter.WriteLine(ToCsvLine(r));
    }

    public IReadOnlyList<ExperimentResult> LoadAllResults()
    {
        if (!File.Exists(JsonlPath)) return [];

        var results = new List<ExperimentResult>();
        foreach (var line in File.ReadLines(JsonlPath))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            try
            {
                var r = JsonSerializer.Deserialize<ExperimentResult>(line, JsonOpts);
                if (r is not null) results.Add(r);
            }
            catch
            {
                // linha malformada; ignorar
            }
        }
        return results;
    }

    public void ClearAll()
    {
        if (File.Exists(JsonlPath)) File.Delete(JsonlPath);
        if (File.Exists(CsvPath)) File.Delete(CsvPath);
    }

    private static string ToCsvLine(ExperimentResult r) =>
        string.Join(",",
            r.RunId,
            r.TimestampUtc.ToString("o"),
            $"\"{r.AlgorithmName}\"",
            r.Size,
            r.Pattern,
            r.Repetitions,
            r.Seed,
            r.M?.ToString() ?? string.Empty,
            r.AvgTimeMs.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture),
            r.AvgComparisons.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture),
            r.AvgSwaps.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture),
            r.StdDevTimeMs.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture));
}
