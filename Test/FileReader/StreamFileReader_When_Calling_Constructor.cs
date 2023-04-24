using DanskeBank.FileReader;
using FluentAssertions;
using NUnit.Framework;

namespace Test.FileReader;

public class StreamFileReader_When_Calling_Constructor
{
    [Test]
    public void Constructor_NullGuard_ThrowsArgumentNullException()
    {
        // Arrange / Act 
        Action action = () => { new StreamFileReader(null); };

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
