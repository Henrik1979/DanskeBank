``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2)
AMD Ryzen 7 5800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.202
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|                                            Method |     Mean |   Error |  StdDev | Allocated |
|-------------------------------------------------- |---------:|--------:|--------:|----------:|
|                Benchmark_ReadAllLinesAsync_String | 409.2 μs | 2.58 μs | 2.41 μs |  287.6 KB |
| Benchmark_ReadAllLinesAsyncMemoryMappedFileReader | 225.9 μs | 1.18 μs | 1.10 μs | 276.36 KB |
|                  Benchmark_ReadAllLinesAsync_Span | 431.0 μs | 3.85 μs | 3.60 μs | 287.59 KB |
