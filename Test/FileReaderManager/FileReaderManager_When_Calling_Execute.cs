using NSubstitute;
using NUnit.Framework;
using DanskeBank.FileReader;
using DanskeBank.Guard;
using DanskeBank.LineSplitter;
using System.Threading.Channels;
using FluentAssertions;

namespace Test.FileReaderManager;

[TestFixture]
public class FileReaderManager_When_Calling_Execute
{
    private DanskeBank.FileReaderManager.FileReaderManager fileReaderManager;
    private IFileReader fileReaderSubstitute;
    private ILineSplitter lineSplitterSubstitute;
    private Channel<string> channel;
    private char[] separators;

    [SetUp]
    public void Setup()
    {
        var guard = Substitute.For<IGuard>();
        fileReaderSubstitute = Substitute.For<IFileReader>();
        lineSplitterSubstitute = Substitute.For<ILineSplitter>();
        separators = new char[] { ',', '.', ' ' };

        fileReaderManager = new DanskeBank.FileReaderManager.FileReaderManager(guard, fileReaderSubstitute, lineSplitterSubstitute, separators);
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
        var filePath = Array.Empty<string>();

        // Act            
        var action = async () => await fileReaderManager.Execute(null, filePath, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Execute_NullFilePaths_ThrowsArgumentNullException()
    {
        // Arrange & Act            
        var action = async () => await fileReaderManager.Execute(channel.Writer, null, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }


    [Test]
    public async Task Execute_EmptyFilePaths_DoesNothing()
    {
        // Arrange
        var filePath = Array.Empty<string>();

        // Act            
        await fileReaderManager.Execute(channel.Writer, filePath, CancellationToken.None);

        // Assert
        fileReaderSubstitute.DidNotReceive().ReadAllLinesAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        lineSplitterSubstitute.DidNotReceive().Split(Arg.Any<string>(), Arg.Any<char[]>());
    }

    [Test]
    public async Task Execute_WithData_WordsAreWrittenToChannel()
    {
        // Arrange
        const string fileContent = "facilisi. nam rutrum nisl est, et";
        var splitvalues = new List<string>
        {
            "facilisi", "nam", "rutrum", "nisl", "est", "wt"
        };

        var filePath = new string[1];

        fileReaderSubstitute
            .ReadAllLinesAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(x => new[] { fileContent }.ToAsyncEnumerable());

        lineSplitterSubstitute.Split(Arg.Is(fileContent), Arg.Is(separators)).Returns(x => splitvalues);

        // Act            
        await fileReaderManager.Execute(channel.Writer, filePath, CancellationToken.None);
        channel.Writer.Complete();
        var temp = await channel.Reader.ReadAllAsync().ToListAsync();

        // Assert
        fileReaderSubstitute.Received(1).ReadAllLinesAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        lineSplitterSubstitute.Received(1).Split(Arg.Is(fileContent), Arg.Is(separators));
        temp.Should().BeEquivalentTo(splitvalues);
    }
}


