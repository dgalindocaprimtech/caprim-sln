using Caprim.API.Dtos;

namespace Caprim.API.Services.Interfaces
{
    /// <summary>
    /// Interface for ExchangeRate service
    /// </summary>
    public interface IExchangeRateService
    {
        /// <summary>
        /// Get all exchange rates
        /// </summary>
        Task<IEnumerable<ExchangeRateDto>> GetAllExchangeRatesAsync();

        /// <summary>
        /// Get exchange rate by ID
        /// </summary>
        Task<ExchangeRateDto?> GetExchangeRateByIdAsync(int id);

        /// <summary>
        /// Get current exchange rate between two assets
        /// </summary>
        Task<ExchangeRateDto?> GetCurrentExchangeRateAsync(int baseAssetId, int quoteAssetId);

        /// <summary>
        /// Create a new exchange rate
        /// </summary>
        Task<ExchangeRateDto> CreateExchangeRateAsync(CreateExchangeRateDto dto);

        /// <summary>
        /// Update an existing exchange rate
        /// </summary>
        Task<ExchangeRateDto?> UpdateExchangeRateAsync(int id, UpdateExchangeRateDto dto);

        /// <summary>
        /// Delete an exchange rate
        /// </summary>
        Task<bool> DeleteExchangeRateAsync(int id);
    }
}