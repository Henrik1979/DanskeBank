using DanskeBank.Guard;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Test.WordExcluder;

[TestFixture]
public class WordExcluder_When_Calling_IsExcluded
{
    const string ExcludeWord = "exclude";

    [Test]
    public void IsExcluded_ExcludeWorSet_ReturnsTrue()
    {
        // Arrange            
        var guard = Substitute.For<IGuard>();
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard);
        wordExcluder.AddExcludeWord(ExcludeWord);

        // Act
        var result = wordExcluder.IsExcluded(ExcludeWord);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsExcluded_ExcludeWordNotSet_ReturnsFalse()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard);

        // Act
        var result = wordExcluder.IsExcluded(ExcludeWord);

        // Assert
        result.Should().BeFalse();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public void IsExcluded_NullAndWhiteSpace_ThrowsArgumentException(string word)
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordExcluder = new DanskeBank.WordExcluder.WordExcluder(guard);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => wordExcluder.IsExcluded(word));   
    }
}