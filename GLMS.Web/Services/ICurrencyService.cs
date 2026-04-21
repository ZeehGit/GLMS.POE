namespace GLMS.Web.Services
{
    public interface ICurrencyService
    {
        Task<decimal> GetUSDToZARRateAsync();
        decimal ConvertUSDToZAR(decimal usdAmount, decimal rate);
    }
}