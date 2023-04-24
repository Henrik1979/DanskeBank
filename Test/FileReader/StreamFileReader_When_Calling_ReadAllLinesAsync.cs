using DanskeBank.FileReader;
using DanskeBank.Guard;
using FluentAssertions;
using NUnit.Framework;

namespace Test.FileReader;

[TestFixture]
public class StreamFileReader_When_Calling_ReadAllLinesAsync
{
    private static readonly string[] InvalidPaths = { "", " ", "nonexistingfile.txt" };

    private const string ExistingFilePath = "testfile.txt";
    private const string EmptyFilePath = "emptyfile.txt";

    private IFileReader fileReader;

    private static readonly List<string> TestLines = new()
    {
        "Line 1",
        "Line 2",
        "Line 3"
    };

    [SetUp]
    public void Setup()
    {
        fileReader = new StreamFileReader(Guard.Init);

        File.WriteAllLines(ExistingFilePath, TestLines, encoding:System.Text.Encoding.UTF8);
        File.Create(EmptyFilePath).Dispose();
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(ExistingFilePath))
        {
            File.Delete(ExistingFilePath);
        }

        if (File.Exists(EmptyFilePath))
        {
            File.Delete(EmptyFilePath);
        }
    }

    [Test]
    public async Task ReadAllLinesAsync_NullFilePath_ThrowsArgumentNullException()
    {
        // Arrange & Act
        Func<Task> func = async () => await fileReader.ReadAllLinesAsync(null, CancellationToken.None).ToListAsync();

        // Assert
        await func.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task ReadAllLinesAsync_NonExistentFilePath_ThrowsIOException([ValueSource(nameof(InvalidPaths))] string invalidPaths)
    {
        // Arrange & Act
        Func<Task> func = async () => await fileReader.ReadAllLinesAsync(invalidPaths, CancellationToken.None).ToListAsync();

        // Assert
        await func.Should().ThrowAsync<IOException>();
    }

    [Test]
    public async Task ReadAllLinesAsync_ValidFilePath_ReturnsAllLines()
    {
        // Arrange & Act
        var lines = await fileReader.ReadAllLinesAsync(ExistingFilePath, CancellationToken.None).ToListAsync();

        // Assert
        lines.Should().BeEquivalentTo(TestLines, options => options.WithStrictOrdering());
    }

    [Test]
    public async Task ReadAllLinesAsync_EmptyFile_ReturnsEmptyList()
    {
        // Arrange & Act
        var lines = await fileReader.ReadAllLinesAsync(EmptyFilePath, CancellationToken.None).ToListAsync();

        // Assert
        lines.Should().NotBeNull().And.BeEmpty();
    }
}