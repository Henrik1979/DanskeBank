using DanskeBank.Guard;
using DanskeBank.WordCounter;
using DanskeBank.WordExcluder;
using System.Threading.Channels;

namespace DanskeBank.WordCountManager;

public class WordCountManager : IWordCountManager
{
    private readonly IGuard guard;
    private readonly IWordCounter wordCounter;
    private readonly IWordExcluder wordExcluder;

    public WordCountManager(
        IGuard guard,
        IWordExcluder wordExcluder,
        IWordCounter wordCounter)
    {
        this.guard = guard ?? throw new ArgumentNullException(nameof(guard));
        this.wordExcluder = guard.GuardAgainstNull(wordExcluder);
        this.wordCounter = guard.GuardAgainstNull(wordCounter);
    }

    public async Task Execute(ChannelReader<string> channel, CancellationToken cancel, Func<string, string>? wordCorrector = null, Func<string, bool>? channelFilter = null)
    {
        guard.GuardAgainstNull(channel);

        await foreach (var word in channel.ReadAllAsync(cancel))
        {
            if (channelFilter?.Invoke(word) == true)
            {
                continue;
            }

            // Use the word corrector, words are case insensitive so we can lowercase them to make them match different cases
            var correctedWord = wordCorrector != null ? wordCorrector(word) : word;

            if (wordExcluder.IsExcluded(correctedWord))
            {
                wordExcluder.IncrementExcludeWord(correctedWord);
                continue;
            }

            wordCounter.AddWord(correctedWord);
        }
    }

    public IEnumerable<KeyValuePair<string, long>> GetWordCount()
    {
        return wordCounter.GetWordCount();
    }

    public IEnumerable<KeyValuePair<string, long>> GetExcludeWordCount()
    {
        return wordExcluder.GetExcludeWordCount();
    }
}