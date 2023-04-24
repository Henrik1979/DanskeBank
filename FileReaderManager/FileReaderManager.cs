using DanskeBank.FileReader;
using DanskeBank.Guard;
using DanskeBank.LineSplitter;
using System.Threading.Channels;

namespace DanskeBank.FileReaderManager;

public class FileReaderManager : IFileReaderManager
{
    private readonly IGuard guard;
    private readonly IFileReader fileReader;
    private readonly ILineSplitter lineSplitter;
    private readonly char[] separators;

    public FileReaderManager(
        IGuard guard,
        IFileReader fileReader,
        ILineSplitter lineSplitter,
        char[] separators)
    {
        this.guard = guard ?? throw new ArgumentNullException(nameof(guard));
        this.fileReader = guard.GuardAgainstNull(fileReader);
        this.lineSplitter = guard.GuardAgainstNull(lineSplitter);
        this.separators = guard.GuardAgainstNull(separators); ;
    }

    public async Task Execute(ChannelWriter<string> channel, string[] filePaths, CancellationToken cancel)
    {
        guard.GuardAgainstNull(channel);
        guard.GuardAgainstNull(filePaths);

        var tasks = new List<Task>(filePaths.Length);

        foreach (var filePath in filePaths)
        {
            var task = Task.Run(async () =>
            {
                await foreach (var line in fileReader.ReadAllLinesAsync(filePath, cancel))
                {
                    foreach (var word in lineSplitter.Split(line, separators))
                    {
                        await channel.WriteAsync(word, cancel);
                    }
                }
            }, cancel);

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}