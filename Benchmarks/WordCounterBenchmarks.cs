using BenchmarkDotNet.Attributes;
using DanskeBank.Guard;
using DanskeBank.WordCounter;

namespace Benchmarks;

[MemoryDiagnoser(false)]
public class WordCounterBenchmarks
{
    private readonly string[] Files = { "Sample_10_000.txt", "corncob_lowercase.txt" };
    private static List<string> Data;
    
    [Params(0,1)]
    public int Index { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        char[] separators = { ',', '.', ' ', ';' };

        var fileContents = File.ReadAllLines(Files[Index]);
        Data = fileContents
            .SelectMany(x => x.Split(separators, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Select(z => z.Replace("-", string.Empty))
            .Where(z => !string.IsNullOrEmpty(z))
            .ToList();
        
        if (!Data.Any())
        {
            throw new Exception("No data found.");
        }
    }

    [Benchmark]
    public void DictionaryInsert()
    {
        var wordCount = new ConcurrentDictionaryWordCounter(Guard.Init);

        foreach (var word in Data)
        {
            wordCount.AddWord(word);
        }
    }

    [Benchmark]
    public void TrieInsert()
    {
        var wordCount = new TrieWordCounter();

        foreach (var word in Data)
        {
            wordCount.AddWord(word);
        }
    }
}