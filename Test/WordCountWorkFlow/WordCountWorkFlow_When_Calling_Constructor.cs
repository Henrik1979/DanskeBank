using NSubstitute;
using NUnit.Framework;
using DanskeBank.Guard;
using DanskeBank.FileReaderManager;
using DanskeBank.FileWriterManager;
using DanskeBank.WordCountManager;

namespace Test.WordCountWorkFlow;

[TestFixture]
public class WordCountWorkFlow_When_Calling_Constructor
{
    [Test]
    public void Constructor_NullGuard_ThrowsArgumentNullException()
    {
        // Arrange
        var wordCountManager = Substitute.For<IWordCountManager>();
        var fileReaderManager = Substitute.For<IFileReaderManager>();
        var fileWriterManager = Substitute.For<IFileWriterManager>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.WordCountWorkFlow.WordCountWorkFlow(null, wordCountManager, fileReaderManager, fileWriterManager));
    }

    [Test]
    public void Constructor_NullWordCountManager_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var fileReaderManager = Substitute.For<IFileReaderManager>();
        var fileWriterManager = Substitute.For<IFileWriterManager>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.WordCountWorkFlow.WordCountWorkFlow(guard, null, fileReaderManager, fileWriterManager));
    }

    [Test]
    public void Constructor_NullFileReaderManager_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordCountManager = Substitute.For<IWordCountManager>();
        var fileWriterManager = Substitute.For<IFileWriterManager>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.WordCountWorkFlow.WordCountWorkFlow(guard, wordCountManager, null, fileWriterManager));
    }

    [Test]
    public void Constructor_NullFileWriterManager_ThrowsArgumentNullException()
    {
        // Arrange
        var guard = Substitute.For<IGuard>();
        var wordCountManager = Substitute.For<IWordCountManager>();
        var fileReaderManager = Substitute.For<IFileReaderManager>();
        var fileWriterManager = Substitute.For<IFileWriterManager>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DanskeBank.WordCountWorkFlow.WordCountWorkFlow(guard, wordCountManager, fileReaderManager, null));
    }
}
