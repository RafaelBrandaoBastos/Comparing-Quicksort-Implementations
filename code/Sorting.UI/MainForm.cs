using Sorting.Core.Algorithms;
using Sorting.Core.Data;
using Sorting.Core.Models;
using Sorting.Core.Services;
using Sorting.Core.Utils;

namespace Sorting.UI;

public partial class MainForm : Form
{
    private int[]? _currentArray;
    private DataPattern _currentPattern;
    private int _currentSeed;

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

    private void LoadHistory()
    {
        try
        {
            var results = _repository.LoadAllResults().ToList();
            dgvHistory.DataSource = new BindingSource { DataSource = results };
            ApplyColumnTooltips(dgvHistory);
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

    private void ShowResults(List<ExperimentResult> results)
    {
        if (dgvCurrentResults.InvokeRequired)
        {
            dgvCurrentResults.Invoke(() => ShowResults(results));
            return;
        }
        dgvCurrentResults.DataSource = new BindingSource { DataSource = results };
        ApplyColumnTooltips(dgvCurrentResults);
        tabMain.SelectedTab = tabCurrentResults;

        foreach (var r in results)
        {
            string mInfo = r.M.HasValue ? $" M={r.M}" : string.Empty;
            Log($"{r.AlgorithmName}{mInfo} | N={r.Size} | {r.Pattern} | " +
                $"T.Médio={r.AvgTimeMs:F4}ms | Comp.={r.AvgComparisons:F0} | " +
                $"Trocas={r.AvgSwaps:F0} | DP={r.StdDevTimeMs:F4}ms");
        }
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
        int seed = (int)numSeed.Value;
        int mVal = (int)numM.Value;
        var pattern = GetSelectedPattern();

        try
        {
            Guard.GreaterThanZero(repetitions, "Repetições");
            Guard.GreaterThan(mVal, 1, "M");
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
        int seed = (int)numSeed.Value;
        int mVal = (int)numM.Value;
        var pattern = GetSelectedPattern();

        try
        {
            Guard.GreaterThanZero(repetitions, "Repetições");
            Guard.GreaterThan(mVal, 1, "M");
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
                Log($"    M={r.M,3} → {r.AvgTimeMs:F4} ms | Comp.={r.AvgComparisons:F0}");

            var medianResult = await Task.Run(() =>
                svc.FindBestM(pattern, seed, size, repetitions, useMedianOfThree: true));

            Log($"[Híbrido Mediana-3] Melhor M = {medianResult.BestM}");
            Log("  Ranking top-5 (por T. Médio):");
            foreach (var r in medianResult.Ranking.Take(5))
                Log($"    M={r.M,3} → {r.AvgTimeMs:F4} ms | Comp.={r.AvgComparisons:F0}");
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
