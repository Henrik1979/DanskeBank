using System.Threading.Channels;

namespace DanskeBank.WordCountManager;

public interface IWordCountManager
{
    Task Execute(ChannelReader<string> channel, CancellationToken cancel, Func<string, string>? wordCorrector = null, Func<string, bool>? channelFilter = null);
    IEnumerable<KeyValuePair<string, long>> GetWordCount();
    IEnumerable<KeyValuePair<string, long>> GetExcludeWordCount();
}
