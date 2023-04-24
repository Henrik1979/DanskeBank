using BenchmarkDotNet.Attributes;

namespace Benchmarks;

[MemoryDiagnoser(false)]
public class SplitBenchmarks
{
    private const int Iterations = 10_000;
    private const string InputString =
        @"suspendisse nec dolor vehicula, porttitor augue eu, dapibus diam. sed auctor, ligula vitae placerat porttitor, sem mauris facilisis dolor, quis aliquet augue lectus vel nisl. 
nullam elementum justo eu consequat pretium. donec sodales velit turpis, egestas aliquam odio interdum vitae. nulla tempus diam nec eros porttitor vehicula. in quis purus metus. 
lorem ipsum dolor sit amet, consectetur adipiscing elit. integer tortor est, volutpat quis faucibus nec, viverra a nisl. nunc tempus dui eros, vitae ullamcorper est vulputate in. 
fusce ut ligula at libero commodo tempor. quisque quis est scelerisque, dictum est vel, tincidunt velit. maecenas consequat est vel felis malesuada, a 
rhoncus tellus posuere. nunc convallis vitae magna vel luctus. quisque fermentum velit porttitor nisi auctor, id imperdiet nisl rutrum. ut nisl lectus, suscipit ac ante at, egestas venenatis est.";

    [Benchmark]
    public void SplitUsingSpan()
    {
        char[] separators = new[] { ' ', ',' };

        for (int i = 0; i < Iterations; i++)
        {
            SplitAsMemory(InputString, separators);
        }
    }

    [Benchmark]
    public void SplitUsingString()
    {
        char[] separators = new[] { ' ', ',' };

        string inputString = InputString;

        for (int i = 0; i < Iterations; i++)
        {
            Split(inputString, separators);
        }
    }

    private static IEnumerable<ReadOnlyMemory<char>> SplitAsMemory(string line, char[] separators)
    {
        ReadOnlyMemory<char> lineMemory = line.AsMemory();
        int start = 0;
        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];
            if (separators.Contains(c))
            {
                if (i > start)
                {
                    yield return lineMemory.Slice(start, i - start);
                }
                start = i + 1;
            }
        }
        if (start < line.Length)
        {
            yield return lineMemory.Slice(start);
        }
    }


    private static IEnumerable<string> Split(string line, char[] separators)
    {
        foreach (var word in line.Split(separators))
        {
            yield return word;
        }
    }
}
