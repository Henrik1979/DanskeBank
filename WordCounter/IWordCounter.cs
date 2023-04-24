namespace DanskeBank.WordCounter;
public interface IWordCounter
{
    public void AddWord(string word);
    IEnumerable<KeyValuePair<string, long>> GetWordCount();
}