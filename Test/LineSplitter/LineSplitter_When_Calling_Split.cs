using DanskeBank.Guard;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Test.LineSplitter;

[TestFixture]
public class Tests
{
    const string NormalInput = "Duis, non. urna id";

    //Don't format this string!
    const string InputWithLinebreaks =
@"Duis, 
non. 
urna 
id";

    [Test]
    public void Split_WithValidInput_ReturnsExpectedWords([ValueSource(nameof(TestCasesForWithValidInput))] (string Input, StringSplitOptions options, List<string> Expected) testCase)
    {
        // Arrange 
        var guard = Substitute.For<IGuard>();
        var lineSplitter = new DanskeBank.LineSplitter.LineSplitter(guard, testCase.options);
        char[] separators = { ',', '.', ' ' };

        // Act 
        var actual = lineSplitter.Split(testCase.Input, separators);

        // Assert
        actual.Should().BeEquivalentTo(testCase.Expected, options => options.WithStrictOrdering());
    }


    [TestCase("  ")]
    [TestCase("")]
    [TestCase(null)]
    public void Split_WithInvalidLineInput_ReturnsNothing(string line)
    {
        // Arrange 
        var guard = Substitute.For<IGuard>();
        var lineSplitter = new DanskeBank.LineSplitter.LineSplitter(guard);
        char[] separators = { ',', '.', ' ' };

        // Act
        var result = lineSplitter.Split(line, separators).ToList();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public void Split_WitNullSeparator_ThrowArgumentNullException()
    {
        // Arrange 
        var guard = Substitute.For<IGuard>();
        var lineSplitter = new DanskeBank.LineSplitter.LineSplitter(guard);

        // Act
        Action action = () => lineSplitter.Split(NormalInput, null).ToList();

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    private static IEnumerable<(string Input, StringSplitOptions Options, List<string> Expected)> TestCasesForWithValidInput()
    {
        yield return (Input: NormalInput, Options: StringSplitOptions.None, Expected: new List<string> { "Duis", "", "non", "", "urna", "id" });
        yield return (Input: InputWithLinebreaks, Options: StringSplitOptions.None, Expected: new List<string> { "Duis", "", $"{Environment.NewLine}non", "", $"{Environment.NewLine}urna", $"{Environment.NewLine}id" });

        yield return (Input: NormalInput, Options: StringSplitOptions.RemoveEmptyEntries, Expected: new List<string> { "Duis", "non", "urna", "id" });
        yield return (Input: InputWithLinebreaks, Options: StringSplitOptions.RemoveEmptyEntries, Expected: new List<string> { "Duis", $"{Environment.NewLine}non", $"{Environment.NewLine}urna", $"{Environment.NewLine}id" });

        yield return (Input: NormalInput, Options: StringSplitOptions.TrimEntries, Expected: new List<string> { "Duis", "", "non", "", "urna", "id" });
        yield return (Input: InputWithLinebreaks, Options: StringSplitOptions.TrimEntries, Expected: new List<string> { "Duis", "", "non", "", "urna", "id" });

        yield return (Input: NormalInput, Options: StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries, Expected: new List<string> { "Duis", "non", "urna", "id" });
        yield return (Input: InputWithLinebreaks, Options: StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries, Expected: new List<string> { "Duis", "non", "urna", "id" });
    }
}