using BenchmarkDotNet.Attributes;
using System.Runtime.InteropServices;

namespace Benchmarks;

[MemoryDiagnoser(false)]
public class LoopingBenchmarks
{
    [Params(10, 1_000, 50_000, 100_000, 500_000)]
    public int Capacity { get; set; }

    public List<int> Samples { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Samples = Enumerable.Range(1, Capacity).ToList();
    }

    [Benchmark]
    public void ForLoop()
    {
        for (int i = 0; i < Capacity; i++)
        {
            var sample = Samples[i];
        }
    }

    [Benchmark]
    public void ForeachLoop()
    {
        foreach (var sample in Samples)
        {
        }
    }

    [Benchmark]
    public void ForeachLinqLoop()
    {
        Samples.ForEach(x => { });
    }

    [Benchmark]
    public void ParallelForeachLoop()
    {
        // Set it to the number of cores on the machine
        var option = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        Parallel.ForEach(Samples, option, s => { });
    }

    [Benchmark]
    public void ParallelLinqLoop()
    {
        Samples.AsParallel().ForAll(x => { });
    }

    [Benchmark]
    public void ForeachSpanLoop()
    {
        // VERY fast, BUT items should not be added or removed while the span is in use
        // Don't mutate the list while looping
        foreach (var sample in CollectionsMarshal.AsSpan(Samples))
        {
        }
    }

    [Benchmark]
    public void ForSpanLoop()
    {
        var span = CollectionsMarshal.AsSpan(Samples);

        for (int i = 0; i < span.Length; i++)
        {
            var sample = span[i];
        }
    }
}