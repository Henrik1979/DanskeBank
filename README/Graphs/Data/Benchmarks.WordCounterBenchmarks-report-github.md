``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2)
AMD Ryzen 7 5800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.202
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|           Method | Index |        Mean |     Error |    StdDev |   Allocated |
|----------------- |------ |------------:|----------:|----------:|------------:|
| **DictionaryInsert** |     **0** |    **506.4 μs** |   **4.95 μs** |   **4.39 μs** |    **39.51 KB** |
|       TrieInsert |     0 |    267.0 μs |   1.58 μs |   1.48 μs |   217.39 KB |
| **DictionaryInsert** |     **1** | **14,771.1 μs** | **142.42 μs** | **133.22 μs** |  **8741.06 KB** |
|       TrieInsert |     1 | 40,301.0 μs | 804.64 μs | 752.66 μs | 37032.73 KB |
