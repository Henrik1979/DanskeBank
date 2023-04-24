using DanskeBank.Guard;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Concurrent;
using FluentAssertions.Execution;

namespace Test.WordExcluder;

public class WordExcluder_When_Calling_GetExcludeWordCount
{ 

    [Test]
    public void GetExcludeWordCount_NoExcludeWordHasBeenAdded_ReturnEmptyKetvaluePair()
    {
        // Arrange
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(Guard.Init);

        // Act
        var result = wordExcluder.GetExcludeWordCount().ToList();

        // Assert
        result.Should().HaveCount(0);
    }

    [Test]
    public void GetExcludeWordCount_ExcludeWordsHasBeenAdde_ReturnCorrectCount()
    {
        // Arrange
        var excludeWordCount = new ConcurrentDictionary<string, long>(new List<KeyValuePair<string, long>>
        {
            new KeyValuePair<string, long>("exclude1", 1),
            new KeyValuePair<string, long>("exclude2", 2),
            new KeyValuePair<string, long>("exclude3", 3)
        });

        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(Guard.Init, excludeWordCount);

        // Act
        var result = wordExcluder.GetExcludeWordCount().ToList();

        // Assert
        using (new AssertionScope())
        {
            excludeWordCount.Should().HaveCount(3);
            excludeWordCount.Should().Contain(x => x.Key == "exclude1" && x.Value == 1);
            excludeWordCount.Should().Contain(x => x.Key == "exclude2" && x.Value == 2);
            excludeWordCount.Should().Contain(x => x.Key == "exclude3" && x.Value == 3);
        }
    }
}