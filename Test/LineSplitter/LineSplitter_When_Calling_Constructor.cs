using FluentAssertions;
using NUnit.Framework;

namespace Test.LineSplitter;

[TestFixture]
public class LineSplitter_When_Calling_Constructor
{
    [Test]
    public void Constructor_NullGuard_ThrowsArgumentNullException()
    {
        // Arrange / Act 
        Action action = () => { new DanskeBank.LineSplitter.LineSplitter(null); };

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
