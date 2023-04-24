using DanskeBank;
using DanskeBank.Guard;
using DanskeBank.WordCounter;
using DanskeBank.WordExcluder;
using NSubstitute;
using NUnit.Framework;

namespace Test.WordCountManager;

[TestFixture]
internal class WordCountManager_When_Calling_GetWordCount
{
    [Test]
    public void GetWordCount_ForwardsCallToWordCounter()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordCounterSubstitute = Substitute.For<IWordCounter>();
        var wordExcluderSubstitute = Substitute.For<IWordExcluder>();
        var wordCountManager = new DanskeBank.WordCountManager.WordCountManager(guard, wordExcluderSubstitute, wordCounterSubstitute);

        // Act             
        wordCountManager.GetWordCount();

        // Assert
        wordCounterSubstitute.Received(1).GetWordCount();
    }
}