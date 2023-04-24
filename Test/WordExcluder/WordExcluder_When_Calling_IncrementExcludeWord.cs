using DanskeBank.Guard;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Concurrent;

namespace Test.WordExcluder;

[TestFixture]
public class WordExcluder_When_Calling_IncrementExcludeWord
{
    const string excludeWord = "exclude";

    [Test]
    public void IncrementExcludeWord_ValidString_IncrementsCount()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        ConcurrentDictionary<string, long> excludeWordCount = new();
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard, excludeWordCount);

        // Act
        wordExcluder.IncrementExcludeWord(excludeWord);
        wordExcluder.IncrementExcludeWord(excludeWord);
        wordExcluder.IncrementExcludeWord(excludeWord);

        // Assert
        using (new AssertionScope())
        {
            excludeWordCount.Keys.Count.Should().Be(1);

            var result = excludeWordCount.TryGetValue(excludeWord, out var value);
            result.Should().BeTrue();
            value.Should().Be(3);
        }
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void IncrementExcludeWord_NullOrWhiteSpaceWord_ThrowArgumentException(string excludeWord)
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        ConcurrentDictionary<string, long> excludeWordCount = new();
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard, excludeWordCount);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => wordExcluder.IncrementExcludeWord(excludeWord));
        excludeWordCount.Keys.Count.Should().Be(0);
    }
}