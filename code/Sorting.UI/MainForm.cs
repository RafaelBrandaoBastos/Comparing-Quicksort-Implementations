using Sorting.Core.Algorithms;
using Sorting.Core.Data;
using Sorting.Core.Models;
using Sorting.Core.Services;
using Sorting.Core.Utils;
using System.Windows.Forms.DataVisualization.Charting;

namespace Sorting.UI;

public partial class MainForm : Form
{
    private const string ChartAreaName = "MainArea";
    private const string ChartLegendName = "MainLegend";
    private static readonly Font ChartTitleFont = new("Segoe UI", 12F, FontStyle.Bold);
    private static readonly Font ChartAxisTitleFont = new("Segoe UI", 11F, FontStyle.Bold);
    private static readonly Font ChartAxisLabelFont = new("Segoe UI", 11F, FontStyle.Regular);
    private static readonly Font ChartLegendFont = new("Segoe UI", 11F, FontStyle.Regular);

    private int[]? _currentArray;
    private DataPattern _currentPattern;
    private int _currentSeed;
    private readonly Dictionary<DataPattern, List<ExperimentResult>> _latestMassChartResultsByPattern = [];
    private readonly Dictionary<DataPattern, (int Size, int Seed)> _latestMassMetaByPattern = [];
    private readonly Dictionary<DataPattern, SafeChart> _patternCharts = [];
    private readonly Dictionary<DataPattern, Label> _patternChartPlaceholders = [];

    private readonly ExperimentRunner _runner = new();
    private readonly ResultsRepository _repository = new();
    private readonly TestDataGenerator _generator = new();

    private readonly ISortingAlgorithm _recursive = new QuickSortRecursive();
    private readonly ISortingAlgorithm _hybrid = new QuickSortHybrid();
    private readonly ISortingAlgorithm _hybridMedian = new QuickSortHybridMedianOfThree();

    // Mapeamento de tooltips para colunas dos DataGridViews de resultados
    private static readonly Dictionary<string, string> ColumnTooltips = new()
    {
        ["RunId"]          = "Identificador único da execução (GUID)",
        ["TimestampUtc"]   = "Data e hora em que o experimento foi realizado (UTC)",
        ["AlgorithmName"]  = "Nome do algoritmo executado",
        ["Size"]           = "Tamanho do vetor (N = número de elementos)",
        ["Pattern"]        = "Padrão de dados do vetor de entrada",
        ["Repetitions"]    = "Reps.: número de repetições realizadas para calcular a média",
        ["Seed"]           = "Semente do gerador pseudoaleatório",
        ["M"]              = "Limiar M: subarrays menores que M são passados ao Insertion Sort (híbridos)",
        ["AvgTimeMs"]      = "T. Médio (ms): média do tempo de execução em milissegundos",
        ["AvgComparisons"] = "Comp. Médias: média do número de comparações entre elementos do vetor",
        ["AvgSwaps"]       = "Trocas Médias: média do número de trocas efetivas de posições distintas",
        ["StdDevTimeMs"]   = "DP Tempo (ms): desvio padrão do tempo — mede a variabilidade entre repetições"
    };

    // Classe auxiliar para exibir elementos do array na aba Massa Ativa
    private sealed class ArrayViewItem
    {
        [System.ComponentModel.DisplayName("Índice")]
        public int Indice { get; init; }

        [System.ComponentModel.DisplayName("Valor")]
        public int Valor { get; init; }
    }

    public MainForm()
    {
        InitializeComponent();
        InitializePatternCharts();
        PopulatePatternCombo();
        LoadDefaults();
        UpdateButtonStates();
        LoadHistory();
    }

    // ─── Inicialização ───────────────────────────────────────────────────────

    private void PopulatePatternCombo()
    {
        cmbPattern.DataSource = Enum.GetValues<DataPattern>();
    }

    private void LoadDefaults()
    {
        numSize.Value = 1000;
        numRepetitions.Value = 10;
        numSeed.Value = 42;
        numM.Value = 16;
        cmbPattern.SelectedIndex = 0;
        chkRecursive.Checked = true;
        chkHybrid.Checked = true;
        chkHybridMedian.Checked = true;
    }

    private void InitializePatternCharts()
    {
        tabPatternCharts.SuspendLayout();
        tabPatternCharts.TabPages.Clear();
        _patternCharts.Clear();
        _patternChartPlaceholders.Clear();

        foreach (var pattern in Enum.GetValues<DataPattern>())
        {
            var tabPage = new TabPage(pattern.ToString())
            {
                Padding = new Padding(3)
            };

            var placeholder = CreateChartPlaceholder(pattern);
            var chart = CreatePatternChart();
            chart.Visible = false;

            tabPage.Controls.Add(placeholder);
            tabPage.Controls.Add(chart);
            tabPatternCharts.TabPages.Add(tabPage);
            _patternCharts[pattern] = chart;
            _patternChartPlaceholders[pattern] = placeholder;
        }

        tabPatternCharts.ResumeLayout();
    }

    private static Label CreateChartPlaceholder(DataPattern pattern)
    {
        return new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.DimGray,
            BackColor = Color.WhiteSmoke,
            Font = new Font("Segoe UI", 10F, FontStyle.Italic),
            Text =
                $"Ainda não há dados para o padrão {pattern}.\n\n" +
                "Execute testes com variação de M e salve os resultados para gerar este gráfico."
        };
    }

    private static SafeChart CreatePatternChart()
    {
        var chart = new SafeChart
        {
            Dock = DockStyle.Fill,
            BorderlineDashStyle = ChartDashStyle.Solid,
            BorderlineColor = Color.Gainsboro,
            BackColor = Color.White,
            Palette = ChartColorPalette.Bright
        };

        var area = new ChartArea(ChartAreaName);
        area.AxisX.Title = "Valores de M";
        area.AxisY.Title = "Tempo médio (ms)";
        area.AxisX.TitleFont = ChartAxisTitleFont;
        area.AxisY.TitleFont = ChartAxisTitleFont;
        area.AxisX.IsLabelAutoFit = false;
        area.AxisY.IsLabelAutoFit = false;
        area.AxisX.LabelStyle.Font = ChartAxisLabelFont;
        area.AxisY.LabelStyle.Font = ChartAxisLabelFont;
        area.AxisX.IsMarginVisible = false;
        area.AxisX.LabelStyle.Format = "0";
        area.AxisY.LabelStyle.Format = "0.####";
        area.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
        area.AxisX.MajorGrid.LineColor = Color.Gainsboro;
        area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
        chart.ChartAreas.Add(area);

        var legend = new Legend(ChartLegendName)
        {
            Docking = Docking.Top,
            Alignment = StringAlignment.Near,
            IsTextAutoFit = false,
            Font = ChartLegendFont
        };
        chart.Legends.Add(legend);
        chart.FormatNumber += Chart_FormatNumber;

        return chart;
    }

    private static void Chart_FormatNumber(object? sender, FormatNumberEventArgs e)
    {
        if (Math.Abs(e.Value) < 0.0000001)
            e.LocalizedValue = "0";
    }

    private void LoadHistory()
    {
        try
        {
            var results = _repository.LoadAllResults().ToList();
            dgvHistory.DataSource = new BindingSource { DataSource = results };
            ApplyColumnTooltips(dgvHistory);
            ApplyResultColumnFormats(dgvHistory);
            Log($"Histórico carregado: {results.Count} registro(s).");
        }
        catch (Exception ex)
        {
            Log($"Erro ao carregar histórico: {ex.Message}");
        }
    }

    // ─── Utilitários ─────────────────────────────────────────────────────────

    private void Log(string message)
    {
        if (txtLog.InvokeRequired)
        {
            txtLog.Invoke(() => Log(message));
            return;
        }
        txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        txtLog.ScrollToCaret();
    }

    /// <summary>
    /// Habilita ou desabilita todos os controles durante operações assíncronas.
    /// Ao habilitar, respeita o estado do array ativo para botões dependentes.
    /// </summary>
    private void SetUiEnabled(bool enabled)
    {
        btnGenerateMass.Enabled = enabled;
        btnFindBestM.Enabled = enabled;
        btnClearHistory.Enabled = enabled;
        numSize.Enabled = enabled;
        numRepetitions.Enabled = enabled;
        numSeed.Enabled = enabled;
        numM.Enabled = enabled;
        numMassViewCount.Enabled = enabled;
        cmbPattern.Enabled = enabled;
        chkRecursive.Enabled = enabled;
        chkHybrid.Enabled = enabled;
        chkHybridMedian.Enabled = enabled;

        if (enabled)
            UpdateButtonStates();
        else
        {
            btnRunSelected.Enabled = false;
            btnCompareThree.Enabled = false;
        }
    }

    /// <summary>
    /// Habilita/desabilita botões que dependem de haver uma Massa Ativa gerada.
    /// </summary>
    private void UpdateButtonStates()
    {
        bool hasArray = _currentArray is not null;
        btnRunSelected.Enabled = hasArray;
        btnCompareThree.Enabled = hasArray;
    }

    private DataPattern GetSelectedPattern()
        => (DataPattern)(cmbPattern.SelectedItem ?? DataPattern.Random);

    private List<ISortingAlgorithm> GetSelectedAlgorithms()
    {
        var list = new List<ISortingAlgorithm>();
        if (chkRecursive.Checked) list.Add(_recursive);
        if (chkHybrid.Checked) list.Add(_hybrid);
        if (chkHybridMedian.Checked) list.Add(_hybridMedian);
        return list;
    }

    private static bool NeedsM(ISortingAlgorithm alg)
        => alg is QuickSortHybrid or QuickSortHybridMedianOfThree;

    private static void ApplyColumnTooltips(DataGridView dgv)
    {
        foreach (DataGridViewColumn col in dgv.Columns)
        {
            if (ColumnTooltips.TryGetValue(col.DataPropertyName, out string? tip))
                col.ToolTipText = tip;
        }
    }

    private static void ApplyResultColumnFormats(DataGridView dgv)
    {
        const string maxFourDecimals = "0.####";

        foreach (DataGridViewColumn col in dgv.Columns)
        {
            if (col.DataPropertyName is nameof(ExperimentResult.AvgTimeMs)
                or nameof(ExperimentResult.AvgComparisons)
                or nameof(ExperimentResult.AvgSwaps)
                or nameof(ExperimentResult.StdDevTimeMs))
            {
                col.DefaultCellStyle.Format = maxFourDecimals;
            }
        }
    }

    private void ShowResults(List<ExperimentResult> results)
    {
        if (dgvCurrentResults.InvokeRequired)
        {
            dgvCurrentResults.Invoke(() => ShowResults(results));
            return;
        }
        dgvCurrentResults.DataSource = new BindingSource { DataSource = results };
        ApplyColumnTooltips(dgvCurrentResults);
        ApplyResultColumnFormats(dgvCurrentResults);
        tabMain.SelectedTab = tabCurrentResults;

        foreach (var r in results)
        {
            string mInfo = r.M.HasValue ? $" M={r.M}" : string.Empty;
            Log($"{r.AlgorithmName}{mInfo} | N={r.Size} | {r.Pattern} | " +
                $"T.Médio={r.AvgTimeMs:F4}ms | Comp.={r.AvgComparisons:F0} | " +
                $"Trocas={r.AvgSwaps:F0} | DP={r.StdDevTimeMs:F4}ms");
        }

        UpdateCurrentMassChart(results);
    }

    private void UpdateCurrentMassChart(IEnumerable<ExperimentResult> latestRunResults)
    {
        var list = latestRunResults.ToList();
        if (list.Count == 0)
            return;

        var pattern = _currentPattern;
        if (!_latestMassChartResultsByPattern.TryGetValue(pattern, out var storedResults))
        {
            storedResults = [];
            _latestMassChartResultsByPattern[pattern] = storedResults;
        }

        foreach (var result in list)
        {
            int existingIndex = storedResults.FindIndex(r =>
                string.Equals(r.AlgorithmName, result.AlgorithmName, StringComparison.OrdinalIgnoreCase)
                && r.M == result.M);

            if (existingIndex >= 0)
                storedResults[existingIndex] = result;
            else
                storedResults.Add(result);
        }

        UpdateMvsTimeChart();
    }

    private void UpdateMvsTimeChart()
    {
        if (tabPatternCharts.InvokeRequired)
        {
            tabPatternCharts.Invoke(UpdateMvsTimeChart);
            return;
        }

        foreach (var (pattern, chart) in _patternCharts)
        {
            string patternName = pattern.ToString();
            var placeholder = _patternChartPlaceholders[pattern];

            chart.Series.Clear();
            chart.Titles.Clear();

            if (!_latestMassChartResultsByPattern.TryGetValue(pattern, out var patternRows))
                patternRows = [];

            var grouped = patternRows
                .Where(r => r.M.HasValue)
                .GroupBy(r => r.AlgorithmName)
                .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
                .Select(g => new
                {
                    Algorithm = g.Key,
                    Points = g
                        .GroupBy(r => r.M!.Value)
                        .Select(mg => new
                        {
                            M = mg.Key,
                            AvgTimeMs = mg.Average(x => x.AvgTimeMs)
                        })
                        .OrderBy(p => p.M)
                        .ToList()
                })
                .Where(x => x.Points.Count > 0)
                .ToList();

            var xValues = grouped
                .SelectMany(g => g.Points)
                .Select(p => (double)p.M)
                .ToList();
            var yValues = grouped
                .SelectMany(g => g.Points)
                .Select(p => p.AvgTimeMs)
                .ToList();

            string titleText = _latestMassMetaByPattern.TryGetValue(pattern, out var massMeta)
                ? $"Tempo de execução por M e algoritmo | {patternName} | N={massMeta.Size:N0} | Seed={massMeta.Seed}"
                : $"Tempo de execução por M e algoritmo | {patternName}";

            chart.Titles.Add(new Title
            {
                Text = titleText,
                Font = ChartTitleFont
            });

            // Recursivo nao usa M; plota como referencia em M=0 — adicionado primeiro para aparecer primeiro na legenda.
            var recursiveRows = patternRows
                .Where(r => string.Equals(r.AlgorithmName, _recursive.Name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (recursiveRows.Count > 0)
            {
                var recursiveSeries = new Series($"Tempo Base: {_recursive.Name}")
                {
                    ChartType = SeriesChartType.Point,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 10,
                    Color = Color.Red,
                    IsXValueIndexed = false,
                    XValueType = ChartValueType.Double,
                    YValueType = ChartValueType.Double,
                    Legend = ChartLegendName,
                    ChartArea = ChartAreaName
                };

                double recursiveAvg = recursiveRows.Average(r => r.AvgTimeMs);
                xValues.Add(0d);
                yValues.Add(recursiveAvg);
                // Workaround para bug do Chart: série Point com 1 único ponto pode ser plotada em X=1.
                recursiveSeries.Points.AddXY(0d, recursiveAvg);
                var hiddenPointIndex = recursiveSeries.Points.AddXY(0.000001d, recursiveAvg);
                var hiddenPoint = recursiveSeries.Points[hiddenPointIndex];
                hiddenPoint.MarkerStyle = MarkerStyle.None;
                hiddenPoint.Color = Color.Transparent;
                chart.Series.Add(recursiveSeries);
            }

            foreach (var alg in grouped)
            {
                var series = new Series(alg.Algorithm)
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 6,
                    IsXValueIndexed = false,
                    XValueType = ChartValueType.Int32,
                    YValueType = ChartValueType.Double,
                    Legend = ChartLegendName,
                    ChartArea = ChartAreaName
                };

                foreach (var p in alg.Points)
                    series.Points.AddXY(p.M, p.AvgTimeMs);

                chart.Series.Add(series);
            }

            string yValueFormat = BuildDynamicNumberFormat(yValues);
            ApplyAdaptiveChartScale(chart, xValues, yValues, yValueFormat);

            bool hasData = chart.Series.Count > 0;
            chart.Legends[ChartLegendName].Enabled = hasData;
            chart.Visible = hasData;
            placeholder.Visible = !hasData;
        }
    }

    private static void ApplyAdaptiveChartScale(
        SafeChart chart,
        IReadOnlyList<double> xValues,
        IReadOnlyList<double> yValues,
        string yAxisFormat)
    {
        var area = chart.ChartAreas[ChartAreaName];
        area.AxisY.LabelStyle.Format = yAxisFormat;

        if (xValues.Count == 0 || yValues.Count == 0)
        {
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = 1;
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 1;
            return;
        }

        double maxX = Math.Max(0, xValues.Max());
        double xPadding = maxX <= 1 ? 1 : Math.Max(1, maxX * 0.05);
        area.AxisX.Minimum = 0;
        area.AxisX.Maximum = maxX + xPadding;

        double minY = Math.Max(0, yValues.Min());
        double maxY = Math.Max(0, yValues.Max());
        double ySpan = maxY - minY;
        double yPadding = ySpan > 0 ? ySpan * 0.15 : Math.Max(0.01, maxY * 0.15);

        area.AxisY.Minimum = Math.Max(0, minY - yPadding);
        area.AxisY.Maximum = maxY + yPadding;
    }



    private static string BuildDynamicNumberFormat(IReadOnlyList<double> values)
    {
        if (values.Count == 0)
            return "0";

        int maxDecimalsNeeded = 0;
        foreach (double value in values)
        {
            maxDecimalsNeeded = Math.Max(maxDecimalsNeeded, GetDecimalsNeeded(value));
            if (maxDecimalsNeeded == 4)
                break;
        }

        return maxDecimalsNeeded switch
        {
            0 => "0",
            1 => "0.#",
            2 => "0.##",
            3 => "0.###",
            _ => "0.####"
        };
    }

    private static int GetDecimalsNeeded(double value)
    {
        double rounded = Math.Round(value, 4, MidpointRounding.AwayFromZero);

        for (int decimals = 0; decimals <= 4; decimals++)
        {
            double test = Math.Round(rounded, decimals, MidpointRounding.AwayFromZero);
            if (Math.Abs(rounded - test) < 1e-9)
                return decimals;
        }

        return 4;
    }

    /// <summary>
    /// Atualiza a aba Massa Ativa com o array atual e o número de elementos solicitado.
    /// </summary>
    private void UpdateMassPreview()
    {
        if (_currentArray is null)
        {
            lblMassStatus.Text = "Nenhuma massa ativa — clique em \"Gerar Massa de Teste\"";
            dgvMassPreview.DataSource = null;
            return;
        }

        int count = (int)Math.Min(numMassViewCount.Value, _currentArray.Length);
        numMassViewCount.Maximum = _currentArray.Length;

        lblMassStatus.Text =
            $"N = {_currentArray.Length:N0} | Padrão: {_currentPattern} | Seed: {_currentSeed} | " +
            $"Exibindo {count:N0} de {_currentArray.Length:N0} elemento(s)";

        var items = _currentArray
            .Take(count)
            .Select((v, i) => new ArrayViewItem { Indice = i, Valor = v })
            .ToList();

        dgvMassPreview.DataSource = new BindingSource { DataSource = items };
    }

    // ─── Handlers dos botões ─────────────────────────────────────────────────

    private void btnGenerateMass_Click(object sender, EventArgs e)
    {
        int size = (int)numSize.Value;
        int seed = (int)numSeed.Value;
        var pattern = GetSelectedPattern();

        try
        {
            _currentArray = _generator.Generate(size, pattern, seed);
            _currentPattern = pattern;
            _currentSeed = seed;
            _latestMassMetaByPattern[pattern] = (size, seed);
            _latestMassChartResultsByPattern[pattern] = [];
            UpdateMvsTimeChart();

            UpdateButtonStates();
            UpdateMassPreview();
            tabMain.SelectedTab = tabActiveMass;

            Log($"Massa gerada: {size:N0} elemento(s) | padrão={pattern} | seed={seed}.");
        }
        catch (Exception ex)
        {
            Log($"Erro ao gerar massa: {ex.Message}");
        }
    }

    private async void btnRunSelected_Click(object sender, EventArgs e)
    {
        var algorithms = GetSelectedAlgorithms();
        if (algorithms.Count == 0)
        {
            MessageBox.Show("Selecione ao menos um algoritmo.", "Aviso",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var array = _currentArray!;
        int repetitions = (int)numRepetitions.Value;
        int seed = _currentSeed;
        int mVal = (int)numM.Value;
        var pattern = _currentPattern;

        try
        {
            Guard.GreaterThanZero(repetitions, "Repetições");
            Guard.GreaterThan(mVal, -1, "M");
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetUiEnabled(false);
        Log($"Executando {algorithms.Count} algoritmo(s) | rep={repetitions} | N={array.Length}...");
        try
        {
            var results = await Task.Run(() =>
            {
                var list = new List<ExperimentResult>();
                foreach (var alg in algorithms)
                {
                    int? m = NeedsM(alg) ? mVal : null;
                    list.Add(_runner.Run(alg, array, repetitions, m, seed, pattern));
                }
                return list;
            });

            ShowResults(results);
            _repository.SaveResults(results);
            Log($"{results.Count} resultado(s) salvos em disco.");
            LoadHistory();
        }
        catch (Exception ex)
        {
            Log($"Erro ao executar: {ex.Message}");
        }
        finally
        {
            SetUiEnabled(true);
        }
    }

    private async void btnCompareThree_Click(object sender, EventArgs e)
    {
        var array = _currentArray!;
        int repetitions = (int)numRepetitions.Value;
        int seed = _currentSeed;
        int mVal = (int)numM.Value;
        var pattern = _currentPattern;

        try
        {
            Guard.GreaterThanZero(repetitions, "Repetições");
            Guard.GreaterThan(mVal, -1, "M");
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetUiEnabled(false);
        Log($"Comparando 3 algoritmos | N={array.Length:N0} | rep={repetitions} | M={mVal}...");
        try
        {
            var results = await Task.Run(() => new List<ExperimentResult>
            {
                _runner.Run(_recursive,    array, repetitions, null, seed, pattern),
                _runner.Run(_hybrid,       array, repetitions, mVal, seed, pattern),
                _runner.Run(_hybridMedian, array, repetitions, mVal, seed, pattern)
            });

            ShowResults(results);
            _repository.SaveResults(results);
            Log("Comparação concluída. Resultados salvos.");
            LoadHistory();
        }
        catch (Exception ex)
        {
            Log($"Erro ao comparar: {ex.Message}");
        }
        finally
        {
            SetUiEnabled(true);
        }
    }

    private async void btnFindBestM_Click(object sender, EventArgs e)
    {
        var pattern = GetSelectedPattern();
        int seed = (int)numSeed.Value;
        int size = (int)numSize.Value;
        int repetitions = (int)numRepetitions.Value;

        try
        {
            Guard.GreaterThanZero(size, "Tamanho");
            Guard.GreaterThanZero(repetitions, "Repetições");
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SetUiEnabled(false);
        tabMain.SelectedTab = tabLog;
        Log($"Buscando melhor M | padrão={pattern} | N={size:N0} | rep={repetitions} por valor de M...");

        try
        {
            var svc = new MParameterSearchService();

            var simpleResult = await Task.Run(() =>
                svc.FindBestM(pattern, seed, size, repetitions));

            Log($"[Híbrido Simples] Melhor M = {simpleResult.BestM}");
            Log("  Ranking top-5 (por T. Médio):");
            foreach (var r in simpleResult.Ranking.Take(5))
                Log($"    M={r.M,3} → {r.AvgTimeMs:0.####} ms | Comp.={r.AvgComparisons:0.####}");

            var medianResult = await Task.Run(() =>
                svc.FindBestM(pattern, seed, size, repetitions, useMedianOfThree: true));

            Log($"[Híbrido Mediana-3] Melhor M = {medianResult.BestM}");
            Log("  Ranking top-5 (por T. Médio):");
            foreach (var r in medianResult.Ranking.Take(5))
                Log($"    M={r.M,3} → {r.AvgTimeMs:0.####} ms | Comp.={r.AvgComparisons:0.####}");
        }
        catch (Exception ex)
        {
            Log($"Erro ao buscar melhor M: {ex.Message}");
        }
        finally
        {
            SetUiEnabled(true);
        }
    }

    private void btnClearHistory_Click(object sender, EventArgs e)
    {
        var confirm = MessageBox.Show(
            "Tem certeza que deseja apagar todo o histórico de resultados?",
            "Confirmar exclusão",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (confirm != DialogResult.Yes) return;

        try
        {
            _repository.ClearAll();
            dgvHistory.DataSource = null;
            _latestMassChartResultsByPattern.Clear();
            _latestMassMetaByPattern.Clear();
            UpdateMvsTimeChart();
            Log("Histórico apagado com sucesso.");
        }
        catch (Exception ex)
        {
            Log($"Erro ao limpar histórico: {ex.Message}");
        }
    }

    private void numMassViewCount_ValueChanged(object sender, EventArgs e)
    {
        UpdateMassPreview();
    }
}
