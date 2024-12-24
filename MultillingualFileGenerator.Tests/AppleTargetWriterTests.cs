using FluentAssertions;
using MultillingualFileGenerator.Targets.Writers;
using Xunit;

namespace MultillingualFileGenerator.Tests
{
    public class AppleTargetWriterTests
    {

        [Theory]
        [InlineData("\"", "\\\"")]                  // Should escape double qoutes
        [InlineData("\"abc\"", "\\\"abc\\\"")]      // Should escape double qoutes
        [InlineData("\\\"", "\\\"")]                // Should not escape double quotes that are already escaped
        [InlineData("\\\"abc\\\"", "\\\"abc\\\"")]  // Should not escape double quotes that are already escaped
        [InlineData("", "")]
        [InlineData(null, null)]
        public void TargetWriter_should_escape_special_characters(string input, string expected)
        {
            var targetWriter = new AppleTargetWriter();

            var result = targetWriter.AppleEscape(input);

            result.Should().Be(expected);
        }

    }
}
