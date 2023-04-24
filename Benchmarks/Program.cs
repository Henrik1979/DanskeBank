
using BenchmarkDotNet.Running;
using Benchmarks;
using DanskeBank.Guard;

namespace FileReadBenchmark;

class Program
{
    static void Main(string[] args)
    {
        //var summary = BenchmarkRunner.Run<SplitBenchmarks>();
        //  var summary = BenchmarkRunner.Run<LoopingBenchmarks>();
        //  var summary = BenchmarkRunner.Run<FileReadBenchmark>();
        // var summary = BenchmarkRunner.Run<WriteFileBenchmark>();
        //  var summary = BenchmarkRunner.Run<WordCounterBenchmarks>();

        var switcher = new BenchmarkSwitcher(typeof(Program).Assembly);
        switcher.Run(args);
    }
}

