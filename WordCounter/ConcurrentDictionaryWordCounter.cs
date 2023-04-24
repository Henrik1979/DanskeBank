using DanskeBank.Guard;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace DanskeBank.WordCounter;

public class ConcurrentDictionaryWordCounter : IWordCounter
{
    private readonly IGuard guard;
    private readonly ConcurrentDictionary<string, long> dictionary;

    public ConcurrentDictionaryWordCounter(IGuard guard) : this(guard, new ConcurrentDictionary<string, long>())
    {
    }

    public ConcurrentDictionaryWordCounter(IGuard guard, ConcurrentDictionary<string, long> dictionary)
    {
        this.guard = guard ?? throw new ArgumentNullException(nameof(guard));
        this.dictionary = guard.GuardAgainstNull(dictionary);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddWord(string word)
    {
        guard.GuardAgainstEmptyOrWhiteSpace(word);
        dictionary.AddOrUpdate(word, 1, (key, oldValue) => oldValue + 1);
    }

    public IEnumerable<KeyValuePair<string, long>> GetWordCount()
    {
        foreach (var kvp in dictionary)
        {
            yield return kvp;
        }
    }
}