using DanskeBank.FileWriter;
using DanskeBank.Guard;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace Test.FileWriter;

[TestFixture]
public class StreamFileWriter_When_Calling_WriteFileAsync
{
    private const string outputPath = @"./";
    private const string fileName = "test.txt";

    private IFileWriter fileWriter;

    private readonly KeyValuePair<string, long>[] values = new[]
    {
        new KeyValuePair<string, long>("foo", 1),
        new KeyValuePair<string, long>("bar", 2)
    };   


    [SetUp]
    public void SetUp()
    {
        fileWriter = new StreamFileWriter(Guard.Init);

        var filePath = Path.Combine(outputPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [TearDown]
    public void TearDown()
    {
        var filePath = Path.Combine(outputPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
       

    [Test]
    public async Task WriteFileAsync_WithValidArguments_ShouldWriteToFile()
    {
        // Arrange
        var filePath = Path.Combine(outputPath, fileName);

        // Act
        await fileWriter.WriteFileAsync(outputPath, fileName, values, CancellationToken.None);

        // Assert                   
        File.Exists(filePath).Should().BeTrue();

        var fileContents = await File.ReadAllTextAsync(filePath);

        using (new AssertionScope())
        {
            fileContents.Should().Contain("foo 1");
            fileContents.Should().Contain("bar 2");
        }
    }

    [Test]
    public async Task WriteFileAsync_WithNullArguments_ShouldWriteToFile()
    {
        // Arrange
        var filePath = Path.Combine(outputPath, fileName);

        // Act
        var action = async() =>  await fileWriter.WriteFileAsync(outputPath, fileName, null, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task WriteFileAsync_WithEmptyArguments_ShouldNotWriteToFile()
    {
        // Arrange
        var filePath = Path.Combine(outputPath, fileName);

        // Act
        await fileWriter.WriteFileAsync(outputPath, fileName, new List<KeyValuePair<string, long>>(), CancellationToken.None);

        // Assert                   
        File.Exists(filePath).Should().BeTrue();
        var fileContents = await File.ReadAllTextAsync(filePath);
        fileContents.Should().BeEmpty();
    }


    [Test]
    public async Task WriteFileAsync_ShouldThrowArgumentNullException_WhenGivenNullOutputPath()
    {
        // Arrange & Act
        var action = async () => await fileWriter.WriteFileAsync(null, fileName, values, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestCase("")]
    [TestCase("   ")]
    public async Task WriteFileAsync_WithEmptyOutputPath_ShouldThrowIOException(string outputPath)
    {
        // Arrange & Act
        var action = async () => await fileWriter.WriteFileAsync(outputPath, fileName, values, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<IOException>();
    }

    [Test]
    public async Task WriteFileAsync_WithNullOutputPath_ShouldThrowArgumentNullException()
    {
        // Arrange & Act
        var action = async () => await fileWriter.WriteFileAsync(null, fileName, values, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase(null)]
    public async Task WriteFileAsync_WithEmptyFileName_ShouldThrowArgumentArgumentException(string fileName)
    {
        // Arrange & Act
        var action = async () => await fileWriter.WriteFileAsync(outputPath, fileName, values, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }
}