using DanskeBank.FileReaderManager;
using DanskeBank.FileWriterManager;
using DanskeBank.Guard;
using DanskeBank.WordCountManager;
using System.Threading.Channels;

namespace DanskeBank.WordCountWorkFlow;

public class WordCountWorkFlow : IWordCountWorkFlow
{
    private readonly IGuard guard;
    private readonly IWordCountManager wordCountManager;
    private readonly IFileReaderManager fileReaderManager;
    private readonly IFileWriterManager fileWriterManager;

    public WordCountWorkFlow(IGuard guard, IWordCountManager wordCountManager, IFileReaderManager fileReaderManager, IFileWriterManager fileWriterManager)
    {
        this.guard = guard ?? throw new ArgumentNullException(nameof(guard));
        this.wordCountManager = guard.GuardAgainstNull(wordCountManager);
        this.fileReaderManager = guard.GuardAgainstNull(fileReaderManager);
        this.fileWriterManager = guard.GuardAgainstNull(fileWriterManager);
    }

    public async Task Execute(Channel<string> channel, Configuration configuration, CancellationToken token, Func<string, string>? wordCorrector = null, Func<string, bool>? channelReaderFilter = null)
    {
        // These checks already exist inside the current interface implementations - but there's no guarantee that new implementations will do the same
        guard.GuardAgainstNull(channel);
        guard.GuardAgainstNull(configuration);
        guard.GuardAgainstNull(configuration.InputFiles);
        guard.GuardAgainstFileSystemInfoNotFound(configuration.OutputPath);

        // Setup Consumer first, this way it will start reading as soon as there's data ready 
        var wordCountManagerTask = wordCountManager.Execute(channel.Reader, token, wordCorrector, channelReaderFilter);

        // Start Producers      
        await fileReaderManager.Execute(channel.Writer, configuration.InputFiles, token);

        // When all writers are done, signal no more input
        channel.Writer.Complete();

        // Wait for the consumer to finish 
        await wordCountManagerTask;

        // Write files to disk
        await fileWriterManager.WriteWordCountFiles(configuration.OutputPath, wordCountManager.GetWordCount(), token);
        await fileWriterManager.WriteWordExcludeFile(configuration.OutputPath, configuration.OutPutExcludeFileName, wordCountManager.GetExcludeWordCount(), token);
    }
}
