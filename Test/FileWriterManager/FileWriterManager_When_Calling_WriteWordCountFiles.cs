using DanskeBank.FileWriter;
using DanskeBank.Guard;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Test.FileWriterManager;

[TestFixture]
public class FileWriterManager_When_Calling_WriteWordCountFiles
{
    const string outputPath = "./";

    private IGuard guardSubstitute;
    private IFileWriter fileWriterSubstitute;
    private DanskeBank.FileWriterManager.FileWriterManager fileWriterManager;

    [SetUp]
    public void Setup()
    {
        guardSubstitute = Substitute.For<IGuard>();
        fileWriterSubstitute = Substitute.For<IFileWriter>();
        fileWriterManager = new DanskeBank.FileWriterManager.FileWriterManager(guardSubstitute, fileWriterSubstitute);
    }

    [TestCase("")]
    [TestCase("  ")]
    public async Task WriteWordCountFiles_WhiteSpaceOutputPath_ThrowsIOException(string outputPath)
    {
        // Arrange            
        var keyValuePairs = new List<KeyValuePair<string, long>>();

        // Act            
        var action = async () => await fileWriterManager.WriteWordCountFiles(outputPath, keyValuePairs, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<IOException>();
    }

    [Test]
    public async Task WriteWordCountFiles_NullOutputPath_ThrowsArgumentNullException()
    {
        // Arrange
        var keyValuePairs = new List<KeyValuePair<string, long>>();

        // Act            
        var action = async () => await fileWriterManager.WriteWordCountFiles(null, keyValuePairs, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task WriteWordCountFiles_NullKeyValuePairs_ThrowsArgumentNullException()
    {
        // Arrange & Act            
        var action = async () => await fileWriterManager.WriteWordCountFiles(outputPath, null, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task WriteWordCountFiles_EmptyKeyValuePairs_WritesEmptyAZFiles()
    {
        // Arrange
        const int expectedCount = 26; //  [a - z]
        var keyValuePairs = new List<KeyValuePair<string, long>>();

        // Act            
        await fileWriterManager.WriteWordCountFiles(outputPath, keyValuePairs, CancellationToken.None);

        // Assert
        await fileWriterSubstitute
            .Received(expectedCount)
            .WriteFileAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Is<IEnumerable<KeyValuePair<string, long>>>(x => x.ToList().Count == 0),
                Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task WriteWordCountFiles_KeyValuePairs_WritesValuesToAZFiles()
    {
        // Arrange
        const int expectedCount = 26; //  [a - z]
        var keyValuePairs = new List<KeyValuePair<string, long>>
        {
            new KeyValuePair<string, long>("a", 10),
            new KeyValuePair<string, long>("z", 30)
        };

        // Act            
        await fileWriterManager.WriteWordCountFiles(outputPath, keyValuePairs, CancellationToken.None);

        // Assert
        await fileWriterSubstitute
            .Received(expectedCount)
            .WriteFileAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<IEnumerable<KeyValuePair<string, long>>>(),
                Arg.Any<CancellationToken>());

        await fileWriterSubstitute
            .Received(1)
            .WriteFileAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Is<IEnumerable<KeyValuePair<string, long>>>(fileContent => fileContent.Any(keyvaluePair => keyvaluePair.Key == "a" && keyvaluePair.Value == 10)),
                Arg.Any<CancellationToken>());

        await fileWriterSubstitute
            .Received(1)
            .WriteFileAsync(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Is<IEnumerable<KeyValuePair<string, long>>>(fileContent => fileContent.Any(keyvaluePair => keyvaluePair.Key == "z" && keyvaluePair.Value == 30)),
                Arg.Any<CancellationToken>());
    }

}
