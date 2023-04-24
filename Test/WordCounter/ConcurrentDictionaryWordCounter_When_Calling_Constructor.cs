using DanskeBank.Guard;
using DanskeBank.WordCounter;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Concurrent;

namespace Test.WordCounter;

[TestFixture]
public class ConcurrentDictionaryWordCounter_When_Calling_Constructor
{

    [Test]
    public void Constructor_NullGuard_ThrowsArgumentNullException()
    {
        // Arrange / Act 
        Action action = () => { new ConcurrentDictionaryWordCounter(null); };

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_NullGuardWithDictionary_ThrowsArgumentNullException()
    {
        // Arrange / Act 
        Action action = () =>
        {
            new ConcurrentDictionaryWordCounter(null, new ConcurrentDictionary<string, long>());
        };

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WithGuardNullDictionary_ThrowsArgumentNullException()
    {
        // Arrange       
        var guard = Substitute.For<IGuard>();
        
        //Act 
        Action action = () =>
        {
            new ConcurrentDictionaryWordCounter(guard, null);
        };

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }


    [Test]
    public void Constructor_InitializesCorrectly_WhenGuardIsSet()
    {
        // Arrange       
        var guard = Substitute.For<IGuard>();

        // Act
        var wordCounter = new ConcurrentDictionaryWordCounter(guard);

        // Assert
        wordCounter.Should().NotBeNull();
    }

    [Test]
    public void Constructor_InitializesCorrectly_WhenGuardAndDictionaryIsSet()
    {
        // Arrange       
        var guard = Substitute.For<IGuard>();

        // Act
        var wordCounter = new ConcurrentDictionaryWordCounter(guard, new ConcurrentDictionary<string, long>());

        // Assert
        wordCounter.Should().NotBeNull();
    }
}
