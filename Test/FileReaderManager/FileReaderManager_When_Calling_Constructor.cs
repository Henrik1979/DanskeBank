using NSubstitute;
using NUnit.Framework;
using DanskeBank.FileReader;
using DanskeBank.Guard;
using DanskeBank.LineSplitter;

namespace Test.FileReaderManager;

[TestFixture]
public class FileReaderManager_When_Calling_Constructor
{
    [Test]
    public void Constructor_NullGuard_ThrowsArgumentNullException()
    {
        // Arrange
        var fileReader = Substitute.For<IFileReader>();
        var lineSplitter = Substitute.For<ILineSplitter>();
        char[] separators = Array.Empty<char>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.FileReaderManager.FileReaderManager(null, fileReader, lineSplitter, separators));
    }

    [Test]
    public void Constructor_NullFileReader_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var fileReader = Substitute.For<IFileReader>();
        var lineSplitter = Substitute.For<ILineSplitter>();
        char[] separators = Array.Empty<char>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.FileReaderManager.FileReaderManager(guard, null, lineSplitter, separators));
    }

    [Test]
    public void Constructor_NullLineSplitter_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var fileReader = Substitute.For<IFileReader>();
        var lineSplitter = Substitute.For<ILineSplitter>();
        char[] separators = Array.Empty<char>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.FileReaderManager.FileReaderManager(guard, fileReader, null, separators));
    }

    [Test]
    public void Constructor_NullSeparators_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var fileReader = Substitute.For<IFileReader>();
        var lineSplitter = Substitute.For<ILineSplitter>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.FileReaderManager.FileReaderManager(null, fileReader, lineSplitter, null));
    }
}
