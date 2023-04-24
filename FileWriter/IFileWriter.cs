namespace DanskeBank.FileWriter;

public interface IFileWriter
{
    Task WriteFileAsync(string outputPath, string fileName, IEnumerable<KeyValuePair<string, long>> values, CancellationToken cancellationToken);
}