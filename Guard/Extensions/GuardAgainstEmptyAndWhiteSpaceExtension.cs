using System.Runtime.CompilerServices;

namespace DanskeBank.Guard;

internal static class GuardAgainstEmptyAndWhiteSpaceExtension
{
    public static ReadOnlySpan<char> GuardAgainstEmptyOrWhiteSpace(
        this IGuard guard,
        ReadOnlySpan<char> input,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null,
        string? message = null)
    {
        // "null" string will be converted to an empty ReadOnlySpan<char> value type which will be chaught in this check
        if (input.IsEmpty)
        {
            throw new ArgumentException(parameterName, message ?? $"Input: '{parameterName}' is required but was empty.");
        }

        if (input.IsWhiteSpace())
        {
            throw new ArgumentException(message ?? $"Input: '{parameterName}' is required but was white space.", parameterName);
        }

        return input;
    }
}