# Comparador de Quicksort — Trabalho Prático PAA

Estudo comparativo empírico de três versões de Quicksort, medindo tempo de execução (relógio da máquina), número de comparações e número de trocas sobre cinco padrões distintos de entrada. Feito em C#.


## Pré-requisitos

- Ter o .NET 8 SDK instalado [Download Aqui!](https://dotnet.microsoft.com/pt-br/download/dotnet/thank-you/runtime-desktop-8.0.27-windows-x64-installer?cid=getdotnetcore)


## Como rodar

1. Abrir o projeto na sua IDE (neste exemplo é utilizado o VS Code)

2. Entrar na pasta /code

3. Rodar o comando `dotnet run --project Sorting.UI`

4. Ou se estiver no Visual Studio 2022+ abra `Comparing-Quicksort.sln` no e pressione **F5**.

5. Será aberta uma janela contendo as opções de execução

<img width="2535" height="1307" alt="{66E28F2D-EED6-49FD-A15C-054E3783FE55}" src="https://github.com/user-attachments/assets/c7b2ff17-d34e-4f1c-aa38-f6f8f9718867" />


# Estrutura da solução

```
code/
├── Comparing-Quicksort.sln
├── Sorting.Core/               # Biblioteca de lógica pura (.NET 8)
│   ├── Algorithms/
│   │   ├── ISortingAlgorithm.cs
│   │   ├── InsertionSort.cs
│   │   ├── QuickSortRecursive.cs
│   │   ├── QuickSortHybrid.cs
│   │   └── QuickSortHybridMedianOfThree.cs
│   ├── Data/
│   │   └── TestDataGenerator.cs
│   ├── Models/
│   │   ├── DataPattern.cs
│   │   ├── ExperimentRequest.cs
│   │   ├── ExperimentResult.cs
│   │   ├── MSearchResult.cs
│   │   └── SortMetrics.cs
│   ├── Services/
│   │   ├── ExperimentRunner.cs
│   │   ├── MParameterSearchService.cs
│   │   └── ResultsRepository.cs
│   └── Utils/
│       └── Guard.cs
└── Sorting.UI/                 # Interface Windows Forms (.NET 8)
    ├── MainForm.cs
    ├── MainForm.Designer.cs
    └── Program.cs
```

## Descrição dos algoritmos

### 1. QuickSort Recursivo

Implementação padrão com **pivô no último elemento** e **partição Lomuto**.

```
Sort(A, left, right):
    if left >= right: return
    p = Partition(A, left, right)
    Sort(A, left, p-1)
    Sort(A, p+1, right)

Partition(A, left, right):
    pivot = A[right]
    i = left - 1
    for j = left to right-1:
        if A[j] <= pivot:
            i++; swap(A[i], A[j])
    swap(A[i+1], A[right])
    return i+1
```

Pior caso: vetor crescente (pivô = máximo em cada passo) → O(n²).

---

### 2. QuickSort Híbrido

Igual ao recursivo, mas **interrompe a recursão** quando o subvetor tem menos de **M** elementos. Após o Quicksort parcial, um único passe de **Insertion Sort** sobre o vetor inteiro finaliza a ordenação (os blocos pequenos ficam quase ordenados pelo Quicksort).

```
Sort(A, left, right, M):
    if left >= right: return
    if (right - left + 1) < M: return   ← sai sem particionar
    p = Partition(A, left, right)
    Sort(A, left, p-1, M)
    Sort(A, p+1, right, M)
InsertionSort(A)                        ← passe final no vetor inteiro
```

---

### 3. QuickSort Híbrido Mediana-de-3

Idêntico ao híbrido, com escolha de pivô aprimorada: a **mediana** entre `A[left]`, `A[mid]` e `A[right]` é calculada antes de cada partição e movida para `A[right]`, onde a partição Lomuto a utiliza normalmente.

Reduz o número de comparações em vetores ordenados e quase-ordenados.

---

### 4. Insertion Sort

Usado internamente pelos híbridos. Implementado por **trocas adjacentes** para que a métrica de Trocas seja comparável com a do Quicksort.

---

## Como as métricas são contadas

| Campo na UI          | Propriedade interna | Descrição |
|----------------------|---------------------|-----------|
| **T. Médio (ms)**    | `AvgTimeMs`         | Média do tempo de execução em milissegundos sobre todas as repetições. |
| **Comp. Médias**     | `AvgComparisons`    | Média do número de comparações entre elementos (`A[j] <= pivot` e comparações do Insertion Sort). |
| **Trocas Médias**    | `AvgSwaps`          | Média do número de trocas efetivas de posições distintas. |
| **DP Tempo (ms)**    | `StdDevTimeMs`      | Desvio padrão do tempo — mede a variabilidade entre repetições. Valor baixo = medição confiável. |
| **N**                | `Size`              | Tamanho do vetor (número de elementos). |
| **Reps.**            | `Repetitions`       | Número de repetições realizadas para calcular a média. |
| **M**                | `M`                 | Limiar usado pelos algoritmos híbridos. |

> Todos os cabeçalhos das colunas têm **tooltip**: passe o mouse sobre o cabeçalho para ver a descrição completa.

---

## Padrões de dados gerados

| Padrão              | Descrição |
|---------------------|-----------|
| `Random`            | Valores pseudoaleatórios 0–1.000.000 com seed fixo. |
| `Sorted`            | Vetor crescente 0, 1, 2, … n-1. |
| `ReverseSorted`     | Vetor decrescente n-1, n-2, … 0. |
| `ManyDuplicates`    | Valores pseudoaleatórios restritos a 0–9. |
| `WorstCaseQuickSort`| Vetor crescente — força O(n²) no QuickSort Recursivo (pivô = máximo). |

---

## Onde os resultados ficam salvos

Após cada execução, os resultados são persistidos **automaticamente** em:

```
<diretório do executável>\resultados\
    historico_resultados.jsonl   ← append, 1 JSON por linha
    historico_resultados.csv     ← append, com cabeçalho na primeira linha
```

Ao rodar com `dotnet run`, esse diretório fica em:

```
Sorting.UI\bin\Debug\net8.0-windows\resultados\
```

Colunas do CSV:
```
RunId, TimestampUtc, AlgorithmName, Size, Pattern, Repetitions, Seed, M,
AvgTimeMs, AvgComparisons, AvgSwaps, StdDevTimeMs
```

Ao reiniciar o aplicativo, o histórico é carregado automaticamente na aba **Histórico**.

---

## Interface da aplicação

### Painel esquerdo — Parâmetros

| Controle | Descrição |
|---|---|
| **Tamanho do vetor** | Número de elementos (N) a gerar. |
| **Repetições** | Quantas vezes cada algoritmo é executado para calcular a média. |
| **Seed** | Semente do gerador pseudoaleatório — mesmo seed = mesmo vetor. |
| **M – limiar híbrido** | Subarrays menores que M são deixados para o Insertion Sort (mín. 2). |
| **Padrão de dados** | Forma como o vetor de teste é gerado (ver tabela acima). |
| **Algoritmos** | Checkboxes para selecionar quais algoritmos serão executados. |

> Todos os controles têm **tooltip** descritivo ao passar o mouse.

### Painel direito — Abas

| Aba | Conteúdo |
|---|---|
| **Massa Ativa** | Exibe o vetor gerado atualmente. O campo **"Exibir primeiros:"** permite escolher quantos elementos mostrar (1 a N). Atualiza em tempo real ao mudar o valor. |
| **Resultados Atuais** | Tabela com os resultados da última execução da sessão. |
| **Histórico** | Tabela com todos os resultados carregados do disco. |
| **Log** | Registro cronológico de todas as ações, com horário. |

### Botões

| Botão | Ação |
|---|---|
| **Gerar Massa de Teste** | Cria o vetor em memória (Massa Ativa) e abre a aba correspondente. Habilita os botões de execução. |
| **Executar Selecionados** | Executa os algoritmos marcados na Massa Ativa. *Requer massa gerada.* |
| **Comparar os 3 Algoritmos** | Executa os 3 algoritmos na mesma massa. *Requer massa gerada.* |
| **Buscar Melhor M** | Testa M de 2 a 64 (passo 2) e exibe o ranking no Log. Usa os parâmetros atuais para gerar vetores próprios. |
| **Limpar Histórico** | Apaga os arquivos de histórico após confirmação. |

> **"Executar Selecionados"** e **"Comparar os 3"** ficam **desabilitados** enquanto não houver uma Massa Ativa gerada.

---

## Fluxo de uso típico

```
1. Configure os parâmetros no painel esquerdo
   (ex.: Tamanho=1000, Repetições=10, Seed=42, M=16, Padrão=Random)

2. Clique [Gerar Massa de Teste]
   → Aba "Massa Ativa" abre com o vetor gerado
   → Use "Exibir primeiros:" para inspecionar os valores
   → Botões de execução são habilitados

3. Clique [Comparar os 3 Algoritmos]
   → Os 3 algoritmos rodam sobre a mesma massa
   → Resultados aparecem na aba "Resultados Atuais"
   → Salvos automaticamente em JSONL e CSV
   → Aba "Histórico" é recarregada

4. (Opcional) Clique [Buscar Melhor M]
   → Exibe no Log qual valor de M minimiza o tempo
     para o Híbrido Simples e para o Híbrido Mediana-3

5. Repita com outros padrões (Sorted, ReverseSorted, etc.)
   para comparar o comportamento em diferentes entradas
```
