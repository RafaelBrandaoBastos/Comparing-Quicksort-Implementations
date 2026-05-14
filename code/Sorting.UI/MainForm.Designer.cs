namespace Sorting.UI;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    // Layout principal
    private System.Windows.Forms.TableLayoutPanel tlpMain;
    private System.Windows.Forms.Panel pnlLeft;
    private System.Windows.Forms.TabControl tabMain;
    private System.Windows.Forms.TabPage tabActiveMass;
    private System.Windows.Forms.TabPage tabCurrentResults;
    private System.Windows.Forms.TabPage tabHistory;
    private System.Windows.Forms.TabPage tabLog;

    // Aba Massa Ativa
    private System.Windows.Forms.Panel pnlMassControl;
    private System.Windows.Forms.Label lblMassStatus;
    private System.Windows.Forms.Label lblExibir;
    private System.Windows.Forms.NumericUpDown numMassViewCount;
    private System.Windows.Forms.DataGridView dgvMassPreview;

    // Configurações
    private System.Windows.Forms.Label lblSize;
    private System.Windows.Forms.NumericUpDown numSize;
    private System.Windows.Forms.Label lblRepetitions;
    private System.Windows.Forms.NumericUpDown numRepetitions;
    private System.Windows.Forms.Label lblSeed;
    private System.Windows.Forms.NumericUpDown numSeed;
    private System.Windows.Forms.Label lblM;
    private System.Windows.Forms.NumericUpDown numM;
    private System.Windows.Forms.Label lblPattern;
    private System.Windows.Forms.ComboBox cmbPattern;

    // Seleção de algoritmos
    private System.Windows.Forms.GroupBox grpAlgorithms;
    private System.Windows.Forms.CheckBox chkRecursive;
    private System.Windows.Forms.CheckBox chkHybrid;
    private System.Windows.Forms.CheckBox chkHybridMedian;

    // Botões
    private System.Windows.Forms.Button btnGenerateMass;
    private System.Windows.Forms.Button btnRunSelected;
    private System.Windows.Forms.Button btnCompareThree;
    private System.Windows.Forms.Button btnFindBestM;
    private System.Windows.Forms.Button btnClearHistory;

    // Grids e log
    private System.Windows.Forms.DataGridView dgvCurrentResults;
    private System.Windows.Forms.DataGridView dgvHistory;
    private System.Windows.Forms.TextBox txtLog;

    // Tooltips
    private System.Windows.Forms.ToolTip toolTipMain;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);

        this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
        this.pnlLeft = new System.Windows.Forms.Panel();
        this.tabMain = new System.Windows.Forms.TabControl();

        this.tabActiveMass = new System.Windows.Forms.TabPage();
        this.pnlMassControl = new System.Windows.Forms.Panel();
        this.lblMassStatus = new System.Windows.Forms.Label();
        this.lblExibir = new System.Windows.Forms.Label();
        this.numMassViewCount = new System.Windows.Forms.NumericUpDown();
        this.dgvMassPreview = new System.Windows.Forms.DataGridView();

        this.tabCurrentResults = new System.Windows.Forms.TabPage();
        this.tabHistory = new System.Windows.Forms.TabPage();
        this.tabLog = new System.Windows.Forms.TabPage();
        this.dgvCurrentResults = new System.Windows.Forms.DataGridView();
        this.dgvHistory = new System.Windows.Forms.DataGridView();
        this.txtLog = new System.Windows.Forms.TextBox();

        this.lblSize = new System.Windows.Forms.Label();
        this.numSize = new System.Windows.Forms.NumericUpDown();
        this.lblRepetitions = new System.Windows.Forms.Label();
        this.numRepetitions = new System.Windows.Forms.NumericUpDown();
        this.lblSeed = new System.Windows.Forms.Label();
        this.numSeed = new System.Windows.Forms.NumericUpDown();
        this.lblM = new System.Windows.Forms.Label();
        this.numM = new System.Windows.Forms.NumericUpDown();
        this.lblPattern = new System.Windows.Forms.Label();
        this.cmbPattern = new System.Windows.Forms.ComboBox();
        this.grpAlgorithms = new System.Windows.Forms.GroupBox();
        this.chkRecursive = new System.Windows.Forms.CheckBox();
        this.chkHybrid = new System.Windows.Forms.CheckBox();
        this.chkHybridMedian = new System.Windows.Forms.CheckBox();
        this.btnGenerateMass = new System.Windows.Forms.Button();
        this.btnRunSelected = new System.Windows.Forms.Button();
        this.btnCompareThree = new System.Windows.Forms.Button();
        this.btnFindBestM = new System.Windows.Forms.Button();
        this.btnClearHistory = new System.Windows.Forms.Button();

        this.tlpMain.SuspendLayout();
        this.pnlLeft.SuspendLayout();
        this.tabMain.SuspendLayout();
        this.tabActiveMass.SuspendLayout();
        this.pnlMassControl.SuspendLayout();
        this.tabCurrentResults.SuspendLayout();
        this.tabHistory.SuspendLayout();
        this.tabLog.SuspendLayout();
        this.grpAlgorithms.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numMassViewCount)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvMassPreview)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numSize)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numRepetitions)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numSeed)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.numM)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentResults)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
        this.SuspendLayout();

        // tlpMain
        this.tlpMain.ColumnCount = 2;
        this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 292F));
        this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tlpMain.RowCount = 1;
        this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tlpMain.Controls.Add(this.pnlLeft, 0, 0);
        this.tlpMain.Controls.Add(this.tabMain, 1, 0);
        this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tlpMain.Padding = new System.Windows.Forms.Padding(6);

        // pnlLeft
        this.pnlLeft.AutoScroll = true;
        this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
        this.pnlLeft.Controls.Add(this.lblSize);
        this.pnlLeft.Controls.Add(this.numSize);
        this.pnlLeft.Controls.Add(this.lblRepetitions);
        this.pnlLeft.Controls.Add(this.numRepetitions);
        this.pnlLeft.Controls.Add(this.lblSeed);
        this.pnlLeft.Controls.Add(this.numSeed);
        this.pnlLeft.Controls.Add(this.lblM);
        this.pnlLeft.Controls.Add(this.numM);
        this.pnlLeft.Controls.Add(this.lblPattern);
        this.pnlLeft.Controls.Add(this.cmbPattern);
        this.pnlLeft.Controls.Add(this.grpAlgorithms);
        this.pnlLeft.Controls.Add(this.btnGenerateMass);
        this.pnlLeft.Controls.Add(this.btnRunSelected);
        this.pnlLeft.Controls.Add(this.btnCompareThree);
        this.pnlLeft.Controls.Add(this.btnFindBestM);
        this.pnlLeft.Controls.Add(this.btnClearHistory);

        // lblSize
        this.lblSize.AutoSize = true;
        this.lblSize.Location = new System.Drawing.Point(8, 10);
        this.lblSize.Text = "Tamanho do vetor:";

        // numSize
        this.numSize.Location = new System.Drawing.Point(8, 28);
        this.numSize.Size = new System.Drawing.Size(268, 26);
        this.numSize.Minimum = 1M;
        this.numSize.Maximum = 10_000_000M;
        this.numSize.Value = 1000M;
        this.numSize.ThousandsSeparator = true;
        this.toolTipMain.SetToolTip(this.numSize, "Quantidade de elementos no vetor de teste.");

        // lblRepetitions
        this.lblRepetitions.AutoSize = true;
        this.lblRepetitions.Location = new System.Drawing.Point(8, 64);
        this.lblRepetitions.Text = "Repetições:";

        // numRepetitions
        this.numRepetitions.Location = new System.Drawing.Point(8, 82);
        this.numRepetitions.Size = new System.Drawing.Size(268, 26);
        this.numRepetitions.Minimum = 1M;
        this.numRepetitions.Maximum = 1000M;
        this.numRepetitions.Value = 10M;
        this.toolTipMain.SetToolTip(this.numRepetitions, "Número de vezes que cada algoritmo é executado para calcular a média.");

        // lblSeed
        this.lblSeed.AutoSize = true;
        this.lblSeed.Location = new System.Drawing.Point(8, 118);
        this.lblSeed.Text = "Seed (pseudoaleatório):";

        // numSeed
        this.numSeed.Location = new System.Drawing.Point(8, 136);
        this.numSeed.Size = new System.Drawing.Size(268, 26);
        this.numSeed.Minimum = 0M;
        this.numSeed.Maximum = 2_147_483_647M;
        this.numSeed.Value = 42M;
        this.toolTipMain.SetToolTip(this.numSeed, "Semente do gerador pseudoaleatório. Mesmo seed sempre produz o mesmo vetor.");

        // lblM
        this.lblM.AutoSize = true;
        this.lblM.Location = new System.Drawing.Point(8, 172);
        this.lblM.Text = "M – limiar híbrido (> 1):";

        // numM
        this.numM.Location = new System.Drawing.Point(8, 190);
        this.numM.Size = new System.Drawing.Size(268, 26);
        this.numM.Minimum = 2M;
        this.numM.Maximum = 512M;
        this.numM.Value = 16M;
        this.toolTipMain.SetToolTip(this.numM,
            "Limiar M: subarrays com menos de M elementos são deixados para o Insertion Sort final (algoritmos híbridos).");

        // lblPattern
        this.lblPattern.AutoSize = true;
        this.lblPattern.Location = new System.Drawing.Point(8, 226);
        this.lblPattern.Text = "Padrão de dados:";

        // cmbPattern
        this.cmbPattern.Location = new System.Drawing.Point(8, 244);
        this.cmbPattern.Size = new System.Drawing.Size(268, 26);
        this.cmbPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.toolTipMain.SetToolTip(this.cmbPattern,
            "Padrão de geração do vetor:\n" +
            "Random = aleatório, Sorted = crescente, ReverseSorted = decrescente,\n" +
            "ManyDuplicates = muitos repetidos (0-9), WorstCaseQuickSort = crescente (pior caso para pivô no fim).");

        // grpAlgorithms
        this.grpAlgorithms.Location = new System.Drawing.Point(8, 282);
        this.grpAlgorithms.Size = new System.Drawing.Size(268, 100);
        this.grpAlgorithms.Text = "Algoritmos";
        this.grpAlgorithms.Controls.Add(this.chkRecursive);
        this.grpAlgorithms.Controls.Add(this.chkHybrid);
        this.grpAlgorithms.Controls.Add(this.chkHybridMedian);

        // chkRecursive
        this.chkRecursive.AutoSize = true;
        this.chkRecursive.Location = new System.Drawing.Point(10, 22);
        this.chkRecursive.Text = "QuickSort Recursivo";
        this.chkRecursive.Checked = true;
        this.toolTipMain.SetToolTip(this.chkRecursive, "Quicksort clássico com pivô no último elemento (partição Lomuto).");

        // chkHybrid
        this.chkHybrid.AutoSize = true;
        this.chkHybrid.Location = new System.Drawing.Point(10, 46);
        this.chkHybrid.Text = "QuickSort Híbrido";
        this.chkHybrid.Checked = true;
        this.toolTipMain.SetToolTip(this.chkHybrid,
            "Quicksort que interrompe a recursão para subarrays < M e finaliza com Insertion Sort.");

        // chkHybridMedian
        this.chkHybridMedian.AutoSize = true;
        this.chkHybridMedian.Location = new System.Drawing.Point(10, 70);
        this.chkHybridMedian.Text = "QuickSort Híbrido Mediana-3";
        this.chkHybridMedian.Checked = true;
        this.toolTipMain.SetToolTip(this.chkHybridMedian,
            "Híbrido com escolha de pivô pela mediana entre o primeiro, meio e último elemento.");

        // btnGenerateMass
        this.btnGenerateMass.Location = new System.Drawing.Point(8, 394);
        this.btnGenerateMass.Size = new System.Drawing.Size(268, 32);
        this.btnGenerateMass.Text = "Gerar Massa de Teste";
        this.btnGenerateMass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnGenerateMass.BackColor = System.Drawing.Color.SteelBlue;
        this.btnGenerateMass.ForeColor = System.Drawing.Color.White;
        this.btnGenerateMass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.toolTipMain.SetToolTip(this.btnGenerateMass,
            "Gera o vetor de teste com os parâmetros atuais e o mantém em memória como Massa Ativa.");
        this.btnGenerateMass.Click += new System.EventHandler(this.btnGenerateMass_Click);

        // btnRunSelected
        this.btnRunSelected.Location = new System.Drawing.Point(8, 434);
        this.btnRunSelected.Size = new System.Drawing.Size(268, 32);
        this.btnRunSelected.Text = "Executar Selecionados";
        this.btnRunSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnRunSelected.BackColor = System.Drawing.Color.SeaGreen;
        this.btnRunSelected.ForeColor = System.Drawing.Color.White;
        this.btnRunSelected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.btnRunSelected.Enabled = false;
        this.toolTipMain.SetToolTip(this.btnRunSelected,
            "Executa os algoritmos marcados na Massa Ativa e salva os resultados automaticamente.\n" +
            "Requer uma Massa Ativa gerada.");
        this.btnRunSelected.Click += new System.EventHandler(this.btnRunSelected_Click);

        // btnCompareThree
        this.btnCompareThree.Location = new System.Drawing.Point(8, 474);
        this.btnCompareThree.Size = new System.Drawing.Size(268, 32);
        this.btnCompareThree.Text = "Comparar os 3 Algoritmos";
        this.btnCompareThree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnCompareThree.BackColor = System.Drawing.Color.DarkOrange;
        this.btnCompareThree.ForeColor = System.Drawing.Color.White;
        this.btnCompareThree.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.btnCompareThree.Enabled = false;
        this.toolTipMain.SetToolTip(this.btnCompareThree,
            "Executa os 3 algoritmos na mesma Massa Ativa, garantindo comparação justa.\n" +
            "Requer uma Massa Ativa gerada.");
        this.btnCompareThree.Click += new System.EventHandler(this.btnCompareThree_Click);

        // btnFindBestM
        this.btnFindBestM.Location = new System.Drawing.Point(8, 514);
        this.btnFindBestM.Size = new System.Drawing.Size(268, 32);
        this.btnFindBestM.Text = "Buscar Melhor M";
        this.btnFindBestM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnFindBestM.BackColor = System.Drawing.Color.MediumPurple;
        this.btnFindBestM.ForeColor = System.Drawing.Color.White;
        this.btnFindBestM.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.toolTipMain.SetToolTip(this.btnFindBestM,
            "Testa empiricamente valores de M de 2 a 64 (passo 2) e exibe o ranking no Log.\n" +
            "Usa os parâmetros Tamanho, Seed e Padrão atuais para gerar os vetores de teste.");
        this.btnFindBestM.Click += new System.EventHandler(this.btnFindBestM_Click);

        // btnClearHistory
        this.btnClearHistory.Location = new System.Drawing.Point(8, 554);
        this.btnClearHistory.Size = new System.Drawing.Size(268, 32);
        this.btnClearHistory.Text = "Limpar Histórico";
        this.btnClearHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnClearHistory.BackColor = System.Drawing.Color.IndianRed;
        this.btnClearHistory.ForeColor = System.Drawing.Color.White;
        this.btnClearHistory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.toolTipMain.SetToolTip(this.btnClearHistory, "Apaga os arquivos de histórico (JSONL e CSV) após confirmação.");
        this.btnClearHistory.Click += new System.EventHandler(this.btnClearHistory_Click);

        // tabMain
        this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tabMain.Controls.Add(this.tabActiveMass);
        this.tabMain.Controls.Add(this.tabCurrentResults);
        this.tabMain.Controls.Add(this.tabHistory);
        this.tabMain.Controls.Add(this.tabLog);
        this.tabMain.Font = new System.Drawing.Font("Segoe UI", 9.5F);

        // ── tabActiveMass ──────────────────────────────────────────────────

        this.tabActiveMass.Text = "Massa Ativa";
        this.tabActiveMass.Padding = new System.Windows.Forms.Padding(0);
        this.tabActiveMass.Controls.Add(this.dgvMassPreview);
        this.tabActiveMass.Controls.Add(this.pnlMassControl);

        // pnlMassControl (Dock=Top: controla a barra superior da aba)
        this.pnlMassControl.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlMassControl.Height = 50;
        this.pnlMassControl.BackColor = System.Drawing.SystemColors.ControlLight;
        this.pnlMassControl.Controls.Add(this.lblMassStatus);
        this.pnlMassControl.Controls.Add(this.lblExibir);
        this.pnlMassControl.Controls.Add(this.numMassViewCount);

        // lblMassStatus
        this.lblMassStatus.AutoSize = true;
        this.lblMassStatus.Location = new System.Drawing.Point(10, 16);
        this.lblMassStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
        this.lblMassStatus.Text = "Nenhuma massa ativa — clique em \"Gerar Massa de Teste\"";
        this.lblMassStatus.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;

        // numMassViewCount (ancorado à direita)
        this.numMassViewCount.Location = new System.Drawing.Point(800, 13);
        this.numMassViewCount.Size = new System.Drawing.Size(90, 26);
        this.numMassViewCount.Minimum = 1M;
        this.numMassViewCount.Maximum = 10_000_000M;
        this.numMassViewCount.Value = 100M;
        this.numMassViewCount.ThousandsSeparator = true;
        this.numMassViewCount.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
        this.toolTipMain.SetToolTip(this.numMassViewCount, "Número de elementos do array a exibir na tabela abaixo.");
        this.numMassViewCount.ValueChanged += new System.EventHandler(this.numMassViewCount_ValueChanged);

        // lblExibir (ancorado à direita, à esquerda do NumericUpDown)
        this.lblExibir.AutoSize = true;
        this.lblExibir.Location = new System.Drawing.Point(680, 16);
        this.lblExibir.Text = "Exibir primeiros:";
        this.lblExibir.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;

        // dgvMassPreview
        this.dgvMassPreview.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgvMassPreview.ReadOnly = true;
        this.dgvMassPreview.AllowUserToAddRows = false;
        this.dgvMassPreview.AllowUserToDeleteRows = false;
        this.dgvMassPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        this.dgvMassPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvMassPreview.RowHeadersVisible = false;
        this.dgvMassPreview.BackgroundColor = System.Drawing.SystemColors.Window;
        this.dgvMassPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;

        // ── tabCurrentResults ──────────────────────────────────────────────

        this.tabCurrentResults.Text = "Resultados Atuais";
        this.tabCurrentResults.Padding = new System.Windows.Forms.Padding(3);
        this.tabCurrentResults.Controls.Add(this.dgvCurrentResults);

        // dgvCurrentResults
        this.dgvCurrentResults.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgvCurrentResults.ReadOnly = true;
        this.dgvCurrentResults.AllowUserToAddRows = false;
        this.dgvCurrentResults.AllowUserToDeleteRows = false;
        this.dgvCurrentResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        this.dgvCurrentResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvCurrentResults.RowHeadersVisible = false;
        this.dgvCurrentResults.BackgroundColor = System.Drawing.SystemColors.Window;
        this.dgvCurrentResults.BorderStyle = System.Windows.Forms.BorderStyle.None;

        // ── tabHistory ─────────────────────────────────────────────────────

        this.tabHistory.Text = "Histórico";
        this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
        this.tabHistory.Controls.Add(this.dgvHistory);

        // dgvHistory
        this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Fill;
        this.dgvHistory.ReadOnly = true;
        this.dgvHistory.AllowUserToAddRows = false;
        this.dgvHistory.AllowUserToDeleteRows = false;
        this.dgvHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
        this.dgvHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvHistory.RowHeadersVisible = false;
        this.dgvHistory.BackgroundColor = System.Drawing.SystemColors.Window;
        this.dgvHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;

        // ── tabLog ─────────────────────────────────────────────────────────

        this.tabLog.Text = "Log";
        this.tabLog.Padding = new System.Windows.Forms.Padding(3);
        this.tabLog.Controls.Add(this.txtLog);

        // txtLog
        this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
        this.txtLog.Multiline = true;
        this.txtLog.ReadOnly = true;
        this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.txtLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        this.txtLog.BackColor = System.Drawing.Color.FromArgb(20, 20, 20);
        this.txtLog.ForeColor = System.Drawing.Color.LightGreen;
        this.txtLog.WordWrap = false;
        this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;

        // ── Form ───────────────────────────────────────────────────────────

        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1200, 700);
        this.Controls.Add(this.tlpMain);
        this.MinimumSize = new System.Drawing.Size(920, 600);
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Comparador de Quicksort — PAA";
        this.Font = new System.Drawing.Font("Segoe UI", 9F);

        // ResumeLayout
        this.grpAlgorithms.ResumeLayout(false);
        this.grpAlgorithms.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.numMassViewCount)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvMassPreview)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numSize)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numRepetitions)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numSeed)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.numM)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvCurrentResults)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
        this.pnlMassControl.ResumeLayout(false);
        this.pnlMassControl.PerformLayout();
        this.tabActiveMass.ResumeLayout(false);
        this.tabLog.ResumeLayout(false);
        this.tabLog.PerformLayout();
        this.tabHistory.ResumeLayout(false);
        this.tabCurrentResults.ResumeLayout(false);
        this.tabMain.ResumeLayout(false);
        this.pnlLeft.ResumeLayout(false);
        this.pnlLeft.PerformLayout();
        this.tlpMain.ResumeLayout(false);
        this.ResumeLayout(false);
    }
}
