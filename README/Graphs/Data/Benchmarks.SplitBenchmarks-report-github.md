``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2)
AMD Ryzen 7 5800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.202
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|           Method |     Mean |    Error |   StdDev | Allocated |
|----------------- |---------:|---------:|---------:|----------:|
|   SplitUsingSpan | 60.68 μs | 0.928 μs | 0.823 μs | 937.53 KB |
| SplitUsingString | 59.59 μs | 0.735 μs | 0.652 μs | 781.28 KB |
