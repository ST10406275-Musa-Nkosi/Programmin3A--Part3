using Xunit;
using System.IO;

namespace TechMoveGLMS.Tests
{
    public class FileValidationTests
    {
        [Theory]
        [InlineData("contract.pdf", true)]   // Valid
        [InlineData("hacker.exe", false)]    // Invalid
        [InlineData("image.png", false)]     // Invalid
        public void ValidateFileType_OnlyAllowsPdf(string fileName, bool expectedResult)
        {
            // Arrange: Get the file extension
            var extension = Path.GetExtension(fileName).ToLower();

            // Act: Check if it is a .pdf
            bool isValid = (extension == ".pdf");

            // Assert: Does it match our expectation?
            Assert.Equal(expectedResult, isValid);
        }
    }
}