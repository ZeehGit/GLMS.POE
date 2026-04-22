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

/*
* Title: Testing with 'dotnet test'
* Author: Microsoft
* Date: 09 April 2026
* Version: 1
* Availability: https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test
*/

/*
* Title: xUnit.net
* Author: Getting Started with xUnit.net
* Date: 13 August 2025
* Version: 3
* Availability: https://xunit.net/docs/getting-started/v3/getting-started
*/