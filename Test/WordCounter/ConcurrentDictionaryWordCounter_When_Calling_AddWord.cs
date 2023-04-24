using DanskeBank.Guard;
using DanskeBank.WordCounter;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Concurrent;

namespace Test.WordCounter;

[TestFixture]
public class ConcurrentDictionaryWordCounter_When_Calling_AddWord
{
    [Test]
    public void AddWord_Null_ShouldThrowArgumentException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordCount = new ConcurrentDictionaryWordCounter(guard);

        // Act
        var action = () => wordCount.AddWord(null);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [TestCase("")]
    [TestCase("  ")]
    public void AddWord_EmptyString_ShouldThrowArgumentException(string value)
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordCount = new ConcurrentDictionaryWordCounter(guard);

        // Act
        var action = () => wordCount.AddWord(value);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void AddWord_ValidValue_IncrementsCounter()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();

        var dic = new ConcurrentDictionary<string, long>();
        var wordCount = new ConcurrentDictionaryWordCounter(guard, dic);

        // Act
        wordCount.AddWord("hello");
        wordCount.AddWord("world");
        wordCount.AddWord("hello");

        // Assert
        using (new AssertionScope())
        {
            dic.Keys.Count.Should().Be(2);
            dic["hello"].Should().Be(2);
            dic["world"].Should().Be(1);
        }
    }

    [Test]
    public async Task AddWord_IsThreadSafe()
    {
        // Arrange
        const int iterations = 250_000;

        var guard = Substitute.For<IGuard>();
        var wordCount = new ConcurrentDictionaryWordCounter(guard);

        // Act
        var helloTasks = Enumerable.Range(0, iterations).Select(i => Task.Run(() => wordCount.AddWord("hello"))).ToList();
        var worldTasks = Enumerable.Range(0, iterations).Select(i => Task.Run(() => wordCount.AddWord("world"))).ToList();

        await Task.WhenAll(helloTasks.Concat(worldTasks));

        // Assert
        using (new AssertionScope())
        {
            wordCount.GetWordCount().First(kvp => kvp.Key == "hello").Value.Should().Be(iterations);
            wordCount.GetWordCount().First(kvp => kvp.Key == "world").Value.Should().Be(iterations);
        }
    }
}