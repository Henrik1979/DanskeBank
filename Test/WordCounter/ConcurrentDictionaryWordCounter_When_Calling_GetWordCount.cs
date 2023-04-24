using DanskeBank.Guard;
using DanskeBank.WordCounter;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Concurrent;

namespace Test.WordCounter;


[TestFixture]
public class ConcurrentDictionaryWordCounter_When_Calling_GetWordCount
{
    [Test]
    public void GetWordCount_WithValues_ReturnsAllKeyValuePairs()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var dic = new ConcurrentDictionary<string, long>(new List<KeyValuePair<string, long>>
        {
            new KeyValuePair<string, long>("key1", 1),
            new KeyValuePair<string, long>("key2", 2),
            new KeyValuePair<string, long>("key3", 3)
        });

        var wordCount = new ConcurrentDictionaryWordCounter(guard, dic);

        // Act
        var result = wordCount.GetWordCount();

        // Assert
        using (new AssertionScope())
        {
            result.Should().HaveCount(3);
            result.Should().Contain(new KeyValuePair<string, long>("key1", 1));
            result.Should().Contain(new KeyValuePair<string, long>("key2", 2));
            result.Should().Contain(new KeyValuePair<string, long>("key3", 3));
        }
    }
}
