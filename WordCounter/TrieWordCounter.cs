using System.Runtime.CompilerServices;
using System.Text;

namespace DanskeBank.WordCounter;

public class TrieWordCounter : IWordCounter
{
    private readonly TrieNode root;

    private readonly object @lock = new();

    public TrieWordCounter()
    {
        root = new TrieNode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddWord(string word)
    {
        ArgumentNullException.ThrowIfNull(word);

        lock (@lock)
        {
            var currentNode = root;

            foreach (var c in word)
            {
                var index = c - 'a';

                if (currentNode.Children[index] == null)
                {
                    currentNode.Children[index] = new TrieNode();
                }

                currentNode = currentNode.Children[index];
            }

            currentNode.Count++;
        }
    }

    public IEnumerable<KeyValuePair<string, long>> GetWordCount()
    {
        lock (@lock)
        {
            var wordCount = new List<KeyValuePair<string, long>>();
            TraverseTrie(root, new StringBuilder(), wordCount);
            return wordCount;
        }
    }

    private static void TraverseTrie(TrieNode node, StringBuilder prefix, List<KeyValuePair<string, long>> wordCount)
    {
        if (node.Count > 0)
        {
            wordCount.Add(new KeyValuePair<string, long>(prefix.ToString(), node.Count));
        }

        for (int i = 0; i < node.Children.Length; i++)
        {
            var child = node.Children[i];

            if (child != null)
            {
                prefix.Append((char)('a' + i));
                TraverseTrie(child, prefix, wordCount);
                prefix.Length--;
            }
        }
    }

    private class TrieNode
    {
        const int TrieNodeSize = 26; // 'a' ->  'z'
        public long Count ;
        public TrieNode[] Children;


        public TrieNode()
        {
            Children = new TrieNode[TrieNodeSize];
        }
    }
}