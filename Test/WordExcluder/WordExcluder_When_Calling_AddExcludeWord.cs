using DanskeBank.Guard;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Concurrent;

namespace Test.WordExcluder;

[TestFixture]
public class WordExcluder_When_Calling_AddExcludeWord
{
    const string excludeWord = "exclude";

    [Test]
    public void AddExcludeWord_ValidString_WordIsAddedToModel()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        ConcurrentDictionary<string, long> excludeWordCount = new();
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard, excludeWordCount);

        // Act
        wordExcluder.AddExcludeWord(excludeWord);

        // Assert
        using (new AssertionScope())
        {
            excludeWordCount.Keys.Count.Should().Be(1);

            var result = excludeWordCount.TryGetValue(excludeWord, out var value);

            result.Should().BeTrue();
            value.Should().Be(0);
        }
    }

    [Test]
    public void AddExcludeWord_DuplicatedString_ThrowArgumentException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => wordExcluder.AddExcludeWord(excludeWord, excludeWord));
    }

    [Test]
    public void AddExcludeWord_NoWordIsAdded_ReturnsEmptyCollection()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        ConcurrentDictionary<string, long> excludeWordCount = new();
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard, excludeWordCount);

        // Act
        wordExcluder.AddExcludeWord();

        // Assert
        excludeWordCount.Should().BeEmpty();
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public void AddExcludeWord_NullOrWhiteSpaceWord_ThrowArgumentException(string word)
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        ConcurrentDictionary<string, long> excludeWordCount = new();
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard, excludeWordCount);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => wordExcluder.AddExcludeWord(word));
    }
}