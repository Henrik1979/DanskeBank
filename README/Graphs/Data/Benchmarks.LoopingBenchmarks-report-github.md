``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22621.1555/22H2/2022Update/SunValley2)
AMD Ryzen 7 5800X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.202
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|              Method | Capacity |           Mean |         Error |        StdDev | Allocated |
|-------------------- |--------- |---------------:|--------------:|--------------:|----------:|
|             **ForLoop** |       **10** |       **4.014 ns** |     **0.0147 ns** |     **0.0138 ns** |         **-** |
|         ForeachLoop |       10 |       4.972 ns |     0.1573 ns |     0.4639 ns |         - |
|     ForeachLinqLoop |       10 |      14.879 ns |     0.0545 ns |     0.0483 ns |         - |
| ParallelForeachLoop |       10 |   5,039.097 ns |    23.2135 ns |    21.7139 ns |    2224 B |
|    ParallelLinqLoop |       10 |  26,582.789 ns |    91.7405 ns |    81.3255 ns |    7544 B |
|     ForeachSpanLoop |       10 |       2.417 ns |     0.0034 ns |     0.0030 ns |         - |
|         ForSpanLoop |       10 |       2.440 ns |     0.0189 ns |     0.0167 ns |         - |
|             **ForLoop** |     **1000** |     **421.904 ns** |     **1.8320 ns** |     **1.6240 ns** |         **-** |
|         ForeachLoop |     1000 |     427.720 ns |     4.5249 ns |     4.2326 ns |         - |
|     ForeachLinqLoop |     1000 |   1,434.275 ns |    28.5954 ns |    51.5634 ns |         - |
| ParallelForeachLoop |     1000 |  11,770.236 ns |    84.7304 ns |    79.2568 ns |    3055 B |
|    ParallelLinqLoop |     1000 |  27,288.457 ns |   223.2710 ns |   208.8478 ns |    7544 B |
|     ForeachSpanLoop |     1000 |     212.890 ns |     0.9173 ns |     0.7660 ns |         - |
|         ForSpanLoop |     1000 |     212.719 ns |     0.7098 ns |     0.6639 ns |         - |
|             **ForLoop** |    **50000** |  **20,890.427 ns** |    **80.8302 ns** |    **75.6086 ns** |         **-** |
|         ForeachLoop |    50000 |  20,945.145 ns |   131.6343 ns |   123.1308 ns |         - |
|     ForeachLinqLoop |    50000 |  70,871.430 ns | 1,406.8924 ns | 4,126.1720 ns |         - |
| ParallelForeachLoop |    50000 |  75,024.207 ns |   434.7352 ns |   406.6516 ns |    5759 B |
|    ParallelLinqLoop |    50000 |  83,398.231 ns |   201.5404 ns |   178.6604 ns |    7608 B |
|     ForeachSpanLoop |    50000 |  10,411.309 ns |    40.8271 ns |    38.1897 ns |         - |
|         ForSpanLoop |    50000 |  10,451.183 ns |    43.2688 ns |    40.4737 ns |         - |
|             **ForLoop** |   **100000** |  **41,451.245 ns** |    **67.0953 ns** |    **62.7610 ns** |         **-** |
|         ForeachLoop |   100000 |  41,564.594 ns |   138.5270 ns |   129.5782 ns |         - |
|     ForeachLinqLoop |   100000 | 124,572.867 ns |   353.9421 ns |   295.5577 ns |         - |
| ParallelForeachLoop |   100000 | 100,674.273 ns |   389.3081 ns |   325.0899 ns |    5596 B |
|    ParallelLinqLoop |   100000 | 122,826.584 ns | 1,036.1168 ns |   969.1843 ns |    7608 B |
|     ForeachSpanLoop |   100000 |  20,766.917 ns |    45.2626 ns |    37.7963 ns |         - |
|         ForSpanLoop |   100000 |  20,738.600 ns |    23.1752 ns |    18.0937 ns |         - |
|             **ForLoop** |   **500000** | **207,206.376 ns** |   **319.3528 ns** |   **283.0979 ns** |         **-** |
|         ForeachLoop |   500000 | 210,239.417 ns | 1,221.5284 ns | 1,020.0315 ns |         - |
|     ForeachLinqLoop |   500000 | 629,812.513 ns | 3,457.4694 ns | 3,234.1191 ns |         - |
| ParallelForeachLoop |   500000 | 317,029.134 ns | 2,847.3420 ns | 2,663.4054 ns |    5655 B |
|    ParallelLinqLoop |   500000 | 431,952.855 ns |   879.9403 ns |   823.0967 ns |    7608 B |
|     ForeachSpanLoop |   500000 | 104,609.059 ns |   356.5216 ns |   333.4905 ns |         - |
|         ForSpanLoop |   500000 | 104,657.177 ns |   401.9015 ns |   375.9390 ns |         - |
