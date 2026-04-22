using System.Text.Json;

namespace GLMS.Web.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CurrencyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<decimal> GetUSDToZARRateAsync()
        {
            var apiKey = _configuration["ExchangeRateAPI:ApiKey"];
            var baseUrl = _configuration["ExchangeRateAPI:BaseUrl"];
            var url = $"{baseUrl}{apiKey}/latest/USD";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);

            // Navigate to conversion_rates.ZAR
            var rate = doc.RootElement
                .GetProperty("conversion_rates")
                .GetProperty("ZAR")
                .GetDecimal();

            return rate;
        }

        public decimal ConvertUSDToZAR(decimal usdAmount, decimal rate)
        {
            return Math.Round(usdAmount * rate, 2);
        }
    }
}

/*
* Title: JSON serialization and deserialization in .NET - overview
* Author: Microsoft
* Date: 29 January 2025
* Version: 1
* Availability: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview
*/

/*
* Title: Make HTTP requests with the HttpClient class
* Author: Microsoft
* Date: 05 March 2026
* Version: 1
* Availability: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient
*/