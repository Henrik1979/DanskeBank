using BenchmarkDotNet.Attributes;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace FileReadBenchmark;

[MemoryDiagnoser(false)]
public class FileReadBenchmark
{
    private readonly string filePath = "Sample_10_000.txt";

    [Benchmark]
    public async Task Benchmark_ReadAllLinesAsync_String()
    {
        await foreach (var _ in ReadAllLinesAsync_String()) { }
    }

    [Benchmark]
    public async Task Benchmark_ReadAllLinesAsyncMemoryMappedFileReader()
    {
        await foreach (var _ in MemoryMappedFileReader()) { }
    }

    [Benchmark]
    public async Task Benchmark_ReadAllLinesAsync_Span()
    {
        await foreach (ReadOnlyMemory<char> _ in ReadAllLinesAsync_Span()) { }
    }


    private async IAsyncEnumerable<string> ReadAllLinesAsync_String()
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);

        string line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            yield return line;
        }
    }

    public async IAsyncEnumerable<string> MemoryMappedFileReader()
    {        
        using var mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open);
        using var stream = mmf.CreateViewStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);

        string line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (reader.EndOfStream)
            {
                yield break;
            }

            yield return line;
        }
    }

    public async IAsyncEnumerable<ReadOnlyMemory<char>> ReadAllLinesAsync_Span()
    {
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);

        string line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            yield return line.AsMemory();
        }
    }
}