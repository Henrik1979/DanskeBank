using DanskeBank.FileWriter;
using DanskeBank.Guard;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

namespace Test.FileWriterManager;

[TestFixture]
public class FileWriterManager_When_Calling_WriteWordExcludeFile
{
    const string outputPath = "./";
    const string fileName = "myFile";

    private IGuard guardSubstitute;
    private IFileWriter fileWriterSubstitute;
    private DanskeBank.FileWriterManager.FileWriterManager fileWriterManager;
    private IEnumerable<KeyValuePair<string, long>> keyValuePairs;

    [SetUp]
    public void Setup()
    {
        guardSubstitute = Substitute.For<IGuard>();
        fileWriterSubstitute = Substitute.For<IFileWriter>();
        fileWriterManager = new DanskeBank.FileWriterManager.FileWriterManager(guardSubstitute, fileWriterSubstitute);
        keyValuePairs = new List<KeyValuePair<string, long>>();
    }

    [TestCase("")]
    [TestCase("  ")]
    public async Task WriteWordExcludeFile_WhiteSpaceOutputPath_ThrowsIOException(string path)
    {
        // Arrange & Act            
        var action = async () => await fileWriterManager.WriteWordExcludeFile(path, fileName, keyValuePairs, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<IOException>();
    }

    [Test]
    public async Task WriteWordExcludeFile_NullOutputPath_ThrowsArgumentNullException()
    {
        // Arrange & Act          
        var action = async () => await fileWriterManager.WriteWordExcludeFile(null, fileName, keyValuePairs, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("  ")]
    public async Task WriteWordExcludeFile_NullOrWhiteSpaceFileName_ThrowsArgumentException(string fileName)
    {
        // Arrange & Act           
        var action = async () => await fileWriterManager.WriteWordExcludeFile(outputPath, fileName, keyValuePairs, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WriteWordExcludeFile_NullKeyValuePairs_ThrowsArgumentException()
    {
        // Arrange            
        var action = async () => await fileWriterManager.WriteWordExcludeFile(outputPath, fileName, null, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WriteWordExcludeFile_ValidData_ForwardsCallToFileWriterSubstitute()
    {
        // Arrange & Act           
        await fileWriterManager.WriteWordExcludeFile(outputPath, fileName, keyValuePairs, CancellationToken.None);

        // Assert
        await fileWriterSubstitute.Received(1).WriteFileAsync(Arg.Is(outputPath), Arg.Is(fileName), Arg.Is(keyValuePairs), Arg.Any<CancellationToken>());
    }
}
