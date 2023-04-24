using System.Runtime.CompilerServices;

namespace DanskeBank.Guard;

internal static class GuardAgainstNullExtensions
{
    public static T GuardAgainstNull<T>(
        this IGuard guard,
        T input,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null,
        string? message = null) where T : class
    {
        if (input == null)
        {
            throw new ArgumentNullException(parameterName, message ?? $"Input: '{parameterName}' is required but was null.");
        }

        return input;
    }
}
