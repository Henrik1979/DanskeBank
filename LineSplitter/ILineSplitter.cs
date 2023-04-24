namespace DanskeBank.LineSplitter;

public interface ILineSplitter
{
    IEnumerable<string> Split(string line, char[] separators);
}
