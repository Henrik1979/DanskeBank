namespace DanskeBank.WordExcluder;

public interface IWordExcluder
{
    bool IsExcluded(string word);
    void IncrementExcludeWord(string excludeWord);
    void AddExcludeWord(params string[] excludeWord);
    IEnumerable<KeyValuePair<string, long>> GetExcludeWordCount();
}