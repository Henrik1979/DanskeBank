using DanskeBank.Guard;
using System.Text;

namespace DanskeBank.FileWriter;

public class StreamFileWriter : IFileWriter
{
    private readonly IGuard guard;

    public StreamFileWriter(IGuard guard)
    {
        this.guard = guard ?? throw new ArgumentNullException(nameof(guard));
    }

    public async Task WriteFileAsync(string outputPath, string fileName, IEnumerable<KeyValuePair<string, long>> values, CancellationToken cancellationToken)
    {
        guard.GuardAgainstFileSystemInfoNotFound(outputPath);
        guard.GuardAgainstEmptyOrWhiteSpace(fileName);
        guard.GuardAgainstNull(values);

        await using var fileStream = File.Open(Path.Combine(outputPath, fileName), FileMode.Create);
        await using var streamWriter = new StreamWriter(fileStream);

        var builder = new StringBuilder();
        foreach (var keyPair in values)
        {
            builder.Append(keyPair.Key);
            builder.Append(' ');
            builder.Append(keyPair.Value);
            builder.AppendLine();
        }

        await streamWriter.WriteAsync(builder.ToString().AsMemory(), cancellationToken);
    }
}