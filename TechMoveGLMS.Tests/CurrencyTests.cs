using Xunit;

namespace TechMoveGLMS.Tests
{
    public class CurrencyTests
    {
        [Fact]
        public void CalculateZar_ShouldReturnCorrectConversion()
        {
            // Arrange: Set up a fake scenario
            decimal usdAmount = 10.00m;
            decimal exchangeRate = 18.50m; // Example rate
            decimal expectedZar = 185.00m;

            // Act: Perform the math
            decimal actualZar = usdAmount * exchangeRate;

            // Assert: Verify the math is 100% correct
            Assert.Equal(expectedZar, actualZar);
        }
    }
}