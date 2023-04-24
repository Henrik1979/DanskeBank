using DanskeBank.Guard;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Text;

namespace DanskeBank.FileReader;

public class StreamFileReader : IFileReader
{
    private readonly IGuard guard;

    public StreamFileReader(IGuard guard)
    {
        this.guard = guard ?? throw new ArgumentNullException(nameof(guard));
    }

    public async IAsyncEnumerable<string> ReadAllLinesAsync(string filePath, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        guard.GuardAgainstFileSystemInfoNotFound(filePath);

        // Check if the file has content, if not MemoryMappedFile will blow up
        if (new FileInfo(filePath).Length == 0)
        {
            yield break;
        }

        using var mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open);
        using var stream = mmf.CreateViewStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);

        string? line;

        while ((line = await reader.ReadLineAsync(cancellationToken)) != null)
        {
            // this is needed because of using a MemoryMappedFile, without it we'll get a line filled with \0
            if (reader.EndOfStream)
            {
                yield break;
            }

            yield return line;
        }
    }
}