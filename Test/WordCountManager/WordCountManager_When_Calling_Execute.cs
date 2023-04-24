using DanskeBank.Guard;
using DanskeBank.WordCounter;
using DanskeBank.WordExcluder;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Channels;

namespace Test.WordCountManager;

[TestFixture]
internal class WordCountManager_When_Calling_Execute
{
    private DanskeBank.WordCountManager.WordCountManager wordCountManager;
    private IWordExcluder wordExcluderSubstitute;
    private IWordCounter wordCounterSubstitute;

    [SetUp]
    public void Setup()
    {
        var guard = Substitute.For<IGuard>();
        wordCounterSubstitute = Substitute.For<IWordCounter>();
        wordExcluderSubstitute = Substitute.For<IWordExcluder>();
        wordCountManager = new DanskeBank.WordCountManager.WordCountManager(guard, wordExcluderSubstitute, wordCounterSubstitute);
    }


    [Test]
    public async Task Execute_NullChannel_ThrowsArgumentNullException()
    {
        // Arrange & Act             
        var action = async () => await wordCountManager.Execute(null, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Execute_ChannelContainsValidNoneExcludedWords_AddWordGetsCalled()
    {
        // Arrange
        wordExcluderSubstitute.IsExcluded(Arg.Any<string>()).Returns(x => false);

        var channel = Channel.CreateUnbounded<string>();
        await channel.Writer.WriteAsync("1");
        await channel.Writer.WriteAsync("2");
        await channel.Writer.WriteAsync("3");
        channel.Writer.Complete();  // signal no more input

        // Act             
        await wordCountManager.Execute(channel, CancellationToken.None);

        // Assert
        wordCounterSubstitute.Received(3).AddWord(Arg.Any<string>());
        wordExcluderSubstitute.Received(0).IncrementExcludeWord(Arg.Any<string>());
        wordExcluderSubstitute.Received(3).IsExcluded(Arg.Any<string>());
    }

    [Test]
    public async Task Execute_ChannelContainsValidExcludedAnddNoneExcludedWords_AddWordAndAddIncrementExcludedWordsGetsCalled()
    {
        // Arrange
        wordExcluderSubstitute.IsExcluded(Arg.Any<string>()).Returns(x => true, x => false, x => false);

        var channel = Channel.CreateUnbounded<string>();
        await channel.Writer.WriteAsync("1");
        await channel.Writer.WriteAsync("2");
        await channel.Writer.WriteAsync("3");
        channel.Writer.Complete();  // signal no more input

        // Act
        await wordCountManager.Execute(channel, CancellationToken.None);

        // Assert
        wordCounterSubstitute.Received(2).AddWord(Arg.Any<string>());
        wordExcluderSubstitute.Received(1).IncrementExcludeWord(Arg.Any<string>());
        wordExcluderSubstitute.Received(3).IsExcluded(Arg.Any<string>());
    }

    [Test]
    public async Task Execute_ChannelContainsValidExcludedWords_IncrementExcludedWordsGetsCalled()
    {
        // Arrange
        wordExcluderSubstitute.IsExcluded(Arg.Any<string>()).Returns(x => true, x => true, x => true);

        var channel = Channel.CreateUnbounded<string>();
        await channel.Writer.WriteAsync("1");
        await channel.Writer.WriteAsync("2");
        await channel.Writer.WriteAsync("3");
        channel.Writer.Complete();  // signal no more input

        // Act
        await wordCountManager.Execute(channel, CancellationToken.None);

        // Assert
        wordCounterSubstitute.Received(0).AddWord(Arg.Any<string>());
        wordExcluderSubstitute.Received(3).IncrementExcludeWord(Arg.Any<string>());
        wordExcluderSubstitute.Received(3).IsExcluded(Arg.Any<string>());
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public async Task Execute_IsNullOrWhiteSpaceFilterIsApplied_IgnoresNullAndWhiteSpaceValues(string word)
    {
        // Arrange
        wordExcluderSubstitute.IsExcluded(Arg.Any<string>()).Returns(x => false);

        var channel = Channel.CreateUnbounded<string>();
        await channel.Writer.WriteAsync(word);
        channel.Writer.Complete();  // signal no more input

        // Act             
        await wordCountManager.Execute(channel, CancellationToken.None, channelFilter: string.IsNullOrWhiteSpace);

        // Assert
        wordCounterSubstitute.Received(0).AddWord(Arg.Any<string>());
        wordExcluderSubstitute.Received(0).IncrementExcludeWord(Arg.Any<string>());
        wordExcluderSubstitute.Received(0).IsExcluded(Arg.Any<string>());
    }

    [Test]
    public async Task Execute_LowerCaseWordCorrector_ChangesUpperCaseToLowerCase()
    {
        // Arrange
        wordExcluderSubstitute.IsExcluded(Arg.Any<string>()).Returns(x => false);

        static string wordCorrector(string word) => word.ToLower();

        var channel = Channel.CreateUnbounded<string>();
        await channel.Writer.WriteAsync("HELLO WORLD!");
        channel.Writer.Complete();  // signal no more input

        // Act             
        await wordCountManager.Execute(channel, CancellationToken.None, wordCorrector: wordCorrector);

        // Assert
        wordCounterSubstitute.Received(1).AddWord(Arg.Is("hello world!"));
        wordExcluderSubstitute.Received(0).IncrementExcludeWord(Arg.Any<string>());
        wordExcluderSubstitute.Received(1).IsExcluded(Arg.Any<string>());
    }
}