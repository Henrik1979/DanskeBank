namespace DanskeBank.FileWriterManager;

public interface IFileWriterManager
{
    Task WriteWordCountFiles(string outputPath, IEnumerable<KeyValuePair<string, long>> keyValuePairs, CancellationToken cancellationToken);
    Task WriteWordExcludeFile(string outputPath, string fileName, IEnumerable<KeyValuePair<string, long>> keyValuePairs, CancellationToken cancellationToken);
}