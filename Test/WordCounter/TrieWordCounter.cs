using DanskeBank.WordCounter;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Test.WordCounter
{
    [TestFixture]
    public class TrieWordCounterTests
    {
        [Test]
        public void AddWord_LimitValues_IncrementsCounter()
        {
            // Arrange
            var wordCount = new TrieWordCounter();

            // Act
            wordCount.AddWord("zealots");
            wordCount.AddWord("aaron");

            // Assert
            using (new AssertionScope())
            {
                wordCount.GetWordCount().First(kvp => kvp.Key == "zealots").Value.Should().Be(1);
                wordCount.GetWordCount().First(kvp => kvp.Key == "aaron").Value.Should().Be(1);
            }
        }


        [Test]
        public void GetAllKeyValuePairs_ReturnsAllKeyValuePairs()
        {
            // Arrange
            var wordCount = new TrieWordCounter();
            wordCount.AddWord("hello");
            wordCount.AddWord("world");
            wordCount.AddWord("hello");

            // Act
            var result = wordCount.GetWordCount();

            // Assert
            using (new AssertionScope())
            {
                result.Should().HaveCount(2);
                result.Should().Contain(new KeyValuePair<string, long>("world", 1));
                result.Should().Contain(new KeyValuePair<string, long>("hello", 2));
            }
        }

        [Test]
        public void AddNull_ShouldThrowArgumentException()
        {
            // Arrange
            var wordCount = new TrieWordCounter();

            // Act
            var action = () => wordCount.AddWord(null);

            // Assert
            action.Should().Throw<ArgumentException>();
        }


        [Test]
        public void AddWord_IncrementsCounter()
        {
            // Arrange
            var wordCount = new TrieWordCounter();

            // Act
            wordCount.AddWord("hello");
            wordCount.AddWord("world");
            wordCount.AddWord("hello");

            // Assert
            using (new AssertionScope())
            {
                wordCount.GetWordCount().First(kvp => kvp.Key == "hello").Value.Should().Be(2);
                wordCount.GetWordCount().First(kvp => kvp.Key == "world").Value.Should().Be(1);
            }
        }

        [Test]
        public async Task AddWord_IsThreadSafe()
        {
            // Arrange
            const int iterations = 100_000;
            var wordCount = new TrieWordCounter();

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
}