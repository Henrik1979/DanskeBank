using DanskeBank.Guard;
using System.Collections.Concurrent;

namespace DanskeBank.WordExcluder;

public class WordExcluder : IWordExcluder
{
    private readonly IGuard guard;
    private readonly ConcurrentDictionary<string, long> excludeWordCount;

    public WordExcluder(IGuard guard, ConcurrentDictionary<string, long> excludeWordCount)
    {
        this.guard = guard ?? throw new ArgumentNullException(nameof(guard));
        this.excludeWordCount = guard.GuardAgainstNull(excludeWordCount);
    }

    public WordExcluder(IGuard guard) : this(guard, new ConcurrentDictionary<string, long>())
    {
    }

    public bool IsExcluded(string word)
    {
        guard.GuardAgainstEmptyOrWhiteSpace(word);
        return excludeWordCount.ContainsKey(word);
    }

    public void IncrementExcludeWord(string excludeWord)
    {
        guard.GuardAgainstEmptyOrWhiteSpace(excludeWord);
        excludeWordCount.AddOrUpdate(excludeWord, 1, (key, oldValue) => oldValue + 1);
    }

    public void AddExcludeWord(params string[] excludeWord)
    {
        foreach (var word in excludeWord)
        {
            guard.GuardAgainstEmptyOrWhiteSpace(word);

            if (excludeWordCount.TryGetValue(word, out _))
            {
                throw new ArgumentException($"'{word}' is already an excluded word");
            }

            excludeWordCount.TryAdd(word, 0);
        }
    }

    public IEnumerable<KeyValuePair<string, long>> GetExcludeWordCount()
    {
        foreach (var kvp in excludeWordCount)
        {
            yield return kvp;
        }
    }
}
