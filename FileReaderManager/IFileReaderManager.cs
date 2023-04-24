using System.Threading.Channels;

namespace DanskeBank.FileReaderManager;

public interface IFileReaderManager
{
    Task Execute(ChannelWriter<string> channel, string[] filePaths, CancellationToken cancel);
}
