``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2)
AMD Ryzen 7 5800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.202
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|                              Method |     Mean |     Error |    StdDev | Allocated |
|------------------------------------ |---------:|----------:|----------:|----------:|
|                  WriteFileAsyncLinq | 5.351 ms | 0.0953 ms | 0.0892 ms |   4.16 MB |
| WriteFileAsyncForeachReadOnlyMemory | 3.455 ms | 0.0687 ms | 0.0869 ms |   3.07 MB |
|               WriteFileAsyncForeach | 3.413 ms | 0.0680 ms | 0.0908 ms |   3.07 MB |
