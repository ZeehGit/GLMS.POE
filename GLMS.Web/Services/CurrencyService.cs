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