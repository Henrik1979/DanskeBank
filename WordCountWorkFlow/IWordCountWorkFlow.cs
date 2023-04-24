using System.Threading.Channels;

namespace DanskeBank.WordCountWorkFlow;

public interface IWordCountWorkFlow
{
    Task Execute(Channel<string> channel, Configuration configuration, CancellationToken token, Func<string, string>? wordCorrector = null, Func<string, bool>? channelReaderFilter = null);
}
