using GLMS.Web.Services;
using Xunit;

namespace GLMS.Tests
{
    public class CurrencyServiceTests
    {
        // Test 1: Correct USD to ZAR conversion math
        [Fact]
        public void ConvertUSDToZAR_CorrectCalculation()
        {
            // Arrange
            var mockHttp = new System.Net.Http.HttpClient();
            var mockConfig = new Microsoft.Extensions.Configuration
                .ConfigurationBuilder().Build();
            var service = new CurrencyService(mockHttp, mockConfig);

            decimal usdAmount = 100m;
            decimal rate = 18.50m;

            // Act
            decimal result = service.ConvertUSDToZAR(usdAmount, rate);

            // Assert
            Assert.Equal(1850.00m, result);
        }

        // Test 2: Zero USD returns zero ZAR
        [Fact]
        public void ConvertUSDToZAR_ZeroAmount_ReturnsZero()
        {
            var mockHttp = new System.Net.Http.HttpClient();
            var mockConfig = new Microsoft.Extensions.Configuration
                .ConfigurationBuilder().Build();
            var service = new CurrencyService(mockHttp, mockConfig);

            decimal result = service.ConvertUSDToZAR(0m, 18.50m);

            Assert.Equal(0m, result);
        }

        // Test 3: Decimal precision is maintained
        [Fact]
        public void ConvertUSDToZAR_DecimalPrecision_RoundedToTwoPlaces()
        {
            var mockHttp = new System.Net.Http.HttpClient();
            var mockConfig = new Microsoft.Extensions.Configuration
                .ConfigurationBuilder().Build();
            var service = new CurrencyService(mockHttp, mockConfig);

            decimal result = service.ConvertUSDToZAR(10m, 18.333m);

            // 10 * 18.333 = 183.33 (rounded to 2dp)
            Assert.Equal(183.33m, result);
        }
    }
}