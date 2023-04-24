using BenchmarkDotNet.Attributes;
using System.Text;

namespace Benchmarks;

[MemoryDiagnoser(false)]
public class WriteFileBenchmark
{
    private readonly string outputPath = ".";
    private readonly string fileName = "fileName";
    private static KeyValuePair<string, long>[] stringValues;
    private static KeyValuePair<ReadOnlyMemory<char>, long>[] memoryValues;
       

    [GlobalSetup]
    public static void GlobalSetup()
    {       
        const int count = 50_000;

        stringValues = new KeyValuePair<string, long>[count];
        memoryValues = new KeyValuePair<ReadOnlyMemory<char>, long>[count];
        
        for (int i = 0; i < count; i++)
        {
            var key = $"Key{i}";
            var value = i;
            stringValues[i] = new KeyValuePair<string, long>(key, value);
            memoryValues[i] = new KeyValuePair<ReadOnlyMemory<char>, long>(key.AsMemory(), value);
        }
    }

    [Benchmark]
    public async Task WriteFileAsyncLinq()
    {
        await WriteFileAsyncLinq(outputPath, fileName, stringValues);
    }

    [Benchmark]
    public async Task WriteFileAsyncForeachReadOnlyMemory()
    {
        await WriteFileAsyncForeachReadOnlyMemory(outputPath, fileName, CancellationToken.None, memoryValues);
    }

    [Benchmark]
    public async Task WriteFileAsyncForeach()
    {
        await WriteFileAsyncForeach(outputPath, fileName, stringValues);
    }

    public static async Task WriteFileAsyncLinq(string outputPath, string fileName, params KeyValuePair<string, long>[] values)
    {
        await using var fileStream = File.Open(Path.Combine(outputPath, fileName), FileMode.Create);
        await using var streamWriter = new StreamWriter(fileStream);

        string lines = string.Join(Environment.NewLine, values.Select(keyPair => $"{keyPair.Key} {keyPair.Value}"));
        await streamWriter.WriteAsync(lines);
    }

    public static async Task WriteFileAsyncForeach(string outputPath, string fileName, params KeyValuePair<string, long>[] values)
    {
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

        await streamWriter.WriteAsync(builder.ToString().AsMemory());
    }

    public static async Task WriteFileAsyncForeachReadOnlyMemory(string outputPath, string fileName, CancellationToken cancellationToken, params KeyValuePair<ReadOnlyMemory<char>, long>[] values)
    {
        await using var fileStream = File.Open(Path.Combine(outputPath, fileName), FileMode.Create);
        await using var streamWriter = new StreamWriter(fileStream);

        var builder = new StringBuilder();
        foreach (var keyPair in values)
        {
            builder.Append(keyPair.Key.Span);
            builder.Append(' ');
            builder.Append(keyPair.Value);
            builder.AppendLine();
        }

        await streamWriter.WriteAsync(builder.ToString().AsMemory(), cancellationToken);
    }
}