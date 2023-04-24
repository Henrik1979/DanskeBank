using DanskeBank.FileWriter;
using DanskeBank.Guard;

namespace DanskeBank.FileWriterManager;

public class FileWriterManager : IFileWriterManager
{
    private readonly IGuard guard;
    private readonly IFileWriter fileWriter;

    public FileWriterManager(IGuard guard, IFileWriter fileWriter)
    {
        this.guard = guard ?? throw new ArgumentNullException(nameof(guard));
        this.fileWriter = guard.GuardAgainstNull(fileWriter);
    }

    public async Task WriteWordExcludeFile(string outputPath, string fileName, IEnumerable<KeyValuePair<string, long>> keyValuePairs, CancellationToken cancellationToken)
    {
        guard.GuardAgainstFileSystemInfoNotFound(outputPath);
        guard.GuardAgainstEmptyOrWhiteSpace(fileName);
        guard.GuardAgainstNull(keyValuePairs);

        await fileWriter.WriteFileAsync(outputPath, fileName, keyValuePairs, cancellationToken);
    }

    public async Task WriteWordCountFiles(string outputPath, IEnumerable<KeyValuePair<string, long>> keyValuePairs, CancellationToken cancellationToken)
    {
        guard.GuardAgainstFileSystemInfoNotFound(outputPath);
        guard.GuardAgainstNull(keyValuePairs);

        Dictionary<char, List<KeyValuePair<string, long>>> temp = new();

        // Populate dictionary to ensure we get a -> z
        for (char c = 'a'; c <= 'z'; c++)
        {
            temp[c] = new List<KeyValuePair<string, long>>();
        }

        // Populate the output data model 
        foreach (var keyValuePair in keyValuePairs)
        {
            temp[keyValuePair.Key[0]].Add(keyValuePair);
        }

        //Write files
        foreach (var keyValuePair in temp)
        {
            var fileName = $"{keyValuePair.Key}.txt";
            var values = keyValuePair.Value;
            await fileWriter.WriteFileAsync(outputPath, fileName, values, cancellationToken);
        }
    }
}