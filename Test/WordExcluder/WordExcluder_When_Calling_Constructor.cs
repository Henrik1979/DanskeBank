using DanskeBank.Guard;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Concurrent;

namespace Test.WordExcluder;

[TestFixture]
public class WordExcluder_When_Calling_Constructor
{      
    [Test]
    public void Constructor_NullGuard_ThrowsArgumentNullException()
    {
        // Arrange & Act 
        Action action = () => { new DanskeBank.WordExcluder.WordExcluder(null); };

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_NullExcludeWordCount_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        ConcurrentDictionary<string, long> excludeWordCount = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.WordExcluder.WordExcluder(guard, excludeWordCount));
    }

    [Test]
    public void Constructor_GuardAndExcludeWordCountIsSet_InitializesCorrectly()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var excludeWordCount = new ConcurrentDictionary<string, long>();

        // Act
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard, excludeWordCount);

        // Assert
        wordExcluder.Should().NotBeNull();
    }

    [Test]
    public void Constructor_GuardOnly_InitializesCorrectly()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();

        // Act
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard);

        // Assert
        wordExcluder.Should().NotBeNull();
    }
}
