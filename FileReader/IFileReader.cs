namespace DanskeBank.FileReader;
public interface IFileReader
{
    IAsyncEnumerable<string> ReadAllLinesAsync(string filePath, CancellationToken cancellationToken);
}
