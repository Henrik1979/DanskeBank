using NSubstitute;
using NUnit.Framework;
using DanskeBank.FileWriter;
using DanskeBank.Guard;

namespace Test.FileWriterManager;

[TestFixture]
public class FileWriterManager_When_Calling_Constructor
{
    [Test]
    public void Constructor_NullGuard_ThrowsArgumentNullException()
    {
        // Arrange
        var fileWriter = Substitute.For<IFileWriter>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.FileWriterManager.FileWriterManager(null, fileWriter));
    }

    [Test]
    public void Constructor_NullFileWriter_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.FileWriterManager.FileWriterManager(guard, null));
    }
}
