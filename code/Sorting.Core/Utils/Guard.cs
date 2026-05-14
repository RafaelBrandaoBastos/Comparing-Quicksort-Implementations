namespace Sorting.Core.Utils;

public static class Guard
{
    public static void GreaterThanZero(int value, string paramName)
    {
        if (value <= 0)
            throw new ArgumentException(
                $"'{paramName}' deve ser maior que zero. Valor recebido: {value}.", paramName);
    }

    public static void GreaterThan(int value, int minimum, string paramName)
    {
        if (value <= minimum)
            throw new ArgumentException(
                $"'{paramName}' deve ser maior que {minimum}. Valor recebido: {value}.", paramName);
    }

    public static void NotNull<T>(T? value, string paramName) where T : class
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
    }
}
