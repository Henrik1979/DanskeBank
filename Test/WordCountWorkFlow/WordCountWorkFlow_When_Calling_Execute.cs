using DanskeBank;
using NSubstitute;
using NUnit.Framework;
using DanskeBank.Guard;
using System.Threading.Channels;
using FluentAssertions;
using DanskeBank.FileReaderManager;
using DanskeBank.FileWriterManager;
using DanskeBank.WordCountManager;
using DanskeBank.WordCountWorkFlow;

namespace Test.WordCountWorkFlow;

[TestFixture]
public class WordCountWorkFlow_When_Calling_Execute
{
    private DanskeBank.WordCountWorkFlow.WordCountWorkFlow wordCountWorkFlow;
    private Channel<string> channel;
    private IWordCountManager wordCountManager;
    private IFileReaderManager fileReaderManager;
    private IFileWriterManager fileWriterManager;

    [SetUp]
    public void SetUp()
    {
        var guard = Substitute.For<IGuard>();
        wordCountManager = Substitute.For<IWordCountManager>();
        fileReaderManager = Substitute.For<IFileReaderManager>();
        fileWriterManager = Substitute.For<IFileWriterManager>();
        wordCountWorkFlow = new DanskeBank.WordCountWorkFlow.WordCountWorkFlow(guard, wordCountManager, fileReaderManager, fileWriterManager);

        channel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions
        {
            SingleWriter = false,
            SingleReader = true,
            AllowSynchronousContinuations = true
        });
    }

    [Test]
    public async Task Execute_NullChannel_ThrowsArgumentNullException()
    {
        // Arrange

        Configuration configuration = new()
        {
            InputFiles = Array.Empty<string>(),
            OutputPath = "."
        };

        // Act       
        var action = async () => await wordCountWorkFlow.Execute(null, configuration, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Execute_NullConfiguration_ThrowsArgumentNullException()
    {
        // Arrange & Act       
        var action = async () => await wordCountWorkFlow.Execute(channel, null, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }


    [Test]
    public async Task Execute_NullFiles_ThrowsArgumentNullException()
    {
        // Arrange
        Configuration configuration = new()
        {
            InputFiles = null,
            OutputPath = "."
        };

        // Act       
        var action = async () => await wordCountWorkFlow.Execute(channel, configuration, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Execute_NullOutputPath_ThrowsArgumentNullException()
    {
        // Arrange
        Configuration configuration = new()
        {
            InputFiles = Array.Empty<string>(),
            OutputPath = null
        };


        // Act       
        var action = async () => await wordCountWorkFlow.Execute(channel, configuration, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Execute_NoneExistingOutputPath_ThrowsIOException()
    {
        // Arrange            
        Configuration configuration = new()
        {
            InputFiles = Array.Empty<string>(),
            OutputPath = "QWERTY:/test"
        };

        // Act       
        var action = async () => await wordCountWorkFlow.Execute(channel, configuration, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<IOException>();
    }

    [Test]
    public async Task Execute_WithValidDate_WritesWordCountToFiles()
    {
        // Arrange            
        var outputPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFileOutput");
        Directory.CreateDirectory(outputPath);

        Configuration configuration = new()
        {
            InputFiles = Array.Empty<string>(),
            OutputPath = outputPath
        };

        Func<string, string> wordCorrector = null;
        Func<string, bool> channelFilter = null;

        IEnumerable<KeyValuePair<string, long>> keyValuePairs = new List<KeyValuePair<string, long>>
        {
            new KeyValuePair<string, long>("foo" , 1 ),
            new KeyValuePair<string, long>("bar" , 2 ),
            new KeyValuePair<string, long>("bazz" , 3 )
        };

        wordCountManager.GetWordCount().Returns(keyValuePairs);

        // Act       
        await wordCountWorkFlow.Execute(channel, configuration, CancellationToken.None, wordCorrector, channelFilter);

        // Assert
        await fileReaderManager.Received(1).Execute(Arg.Is(channel.Writer), Arg.Is(configuration.InputFiles), Arg.Any<CancellationToken>());
        await fileWriterManager.Received(1).WriteWordCountFiles(Arg.Is(configuration.OutputPath), Arg.Is(keyValuePairs), Arg.Any<CancellationToken>());
        await wordCountManager.Received(1).Execute(Arg.Is(channel.Reader), Arg.Any<CancellationToken>(), Arg.Is(wordCorrector), Arg.Is(channelFilter));
    }

}