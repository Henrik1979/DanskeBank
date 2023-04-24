using DanskeBank.FileWriter;
using FluentAssertions;
using NUnit.Framework;

namespace Test.FileWriter;

[TestFixture]
public class StreamFileWriter_When_Calling_Constructor
{
    [Test]
    public void Constructor_NullGuard_ThrowsArgumentNullException()
    {
        // Arrange / Act 
        Action action = () => { new StreamFileWriter(null); };

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
