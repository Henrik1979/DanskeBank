using System.Runtime.CompilerServices;

namespace DanskeBank.Guard;

internal static class GuardAgainstFileSystemInfoNotFoundExtension
{
    public static string GuardAgainstFileSystemInfoNotFound(
        this IGuard guard,
        string input,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null,
        string? message = null)
    {
        guard.GuardAgainstNull(input, parameterName, message);

        if (!File.Exists(input) && !Directory.Exists(input))
        {
            throw new IOException(message ?? $"'{parameterName}' No FileSystemInfo found in path: '{input}'");
        }

        return input;
    }
}