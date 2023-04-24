using DanskeBank.Guard;

namespace DanskeBank.LineSplitter;

public class LineSplitter : ILineSplitter
{
    private readonly IGuard guard;
    private readonly StringSplitOptions stringSplitOptions;

    public LineSplitter(IGuard guard, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    {
        this.guard = guard ?? throw new ArgumentNullException(nameof(guard));
        this.stringSplitOptions = stringSplitOptions;
    }

    public IEnumerable<string> Split(string line, char[] separators)
    {
        // If the separator is the empty string (i.e., string.Empty), then whitespace (i.e., Character.IsWhitespace) is used as the separator.
        guard.GuardAgainstNull(separators);

        if (string.IsNullOrWhiteSpace(line))
        {
            yield break;
        }

        foreach (var word in line.Split(separators, stringSplitOptions))
        {
            yield return word;
        }
    }
}