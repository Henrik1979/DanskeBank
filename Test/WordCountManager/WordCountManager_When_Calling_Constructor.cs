using DanskeBank.Guard;
using DanskeBank.WordCounter;
using DanskeBank.WordExcluder;
using NSubstitute;
using NUnit.Framework;

namespace Test.WordCountManager;

[TestFixture]
public class WordCountManager_When_Calling_Constructor
{
    [Test]
    public void Constructor_NullGuard_ThrowsArgumentNullException()
    {
        // Arrange
        var wordExcluder = Substitute.For<IWordExcluder>();
        var wordCounter = Substitute.For<IWordCounter>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.WordCountManager.WordCountManager(null, wordExcluder, wordCounter));
    }

    [Test]
    public void Constructor_NullWordExcluder_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordCounter = Substitute.For<IWordCounter>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.WordCountManager.WordCountManager(guard, null, wordCounter));
    }

    [Test]
    public void Constructor_NullWordCounter_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordExcluder = Substitute.For<IWordExcluder>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.WordCountManager.WordCountManager(guard, wordExcluder, null));
    }
}