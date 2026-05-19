using System.Windows.Forms.DataVisualization.Charting;

namespace Sorting.UI;

// Evita excecao intermitente no controle Chart durante layout inicial
// quando o TabControl cria paginas com largura/altura temporariamente zero.
public class SafeChart : Chart
{
    protected override void OnResize(EventArgs e)
    {
        if (IsDisposed || Width <= 0 || Height <= 0)
            return;

        try
        {
            base.OnResize(e);
        }
        catch (ArgumentException ex) when (
            ex.Message.Contains("greater than 0px", StringComparison.OrdinalIgnoreCase))
        {
            // Ignora resize invalido transitório; no proximo resize valido o chart renderiza normalmente.
        }
    }
}
