using Caprim.API.Dtos;
using Caprim.API.Models;
using Caprim.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Caprim.API.Services.Implementations
{
    /// <summary>
    /// Implementation of IExchangeRateService
    /// </summary>
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly StellarDbContext _context;
        private readonly ILogger<ExchangeRateService> _logger;

        public ExchangeRateService(StellarDbContext context, ILogger<ExchangeRateService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRateDto>> GetAllExchangeRatesAsync()
        {
            var rates = await _context.ExchangeRates
                .Include(er => er.BaseAsset)
                .Include(er => er.QuoteAsset)
                .ToListAsync();

            return rates.Select(MapToDto);
        }

        public async Task<ExchangeRateDto?> GetExchangeRateByIdAsync(int id)
        {
            var rate = await _context.ExchangeRates
                .Include(er => er.BaseAsset)
                .Include(er => er.QuoteAsset)
                .FirstOrDefaultAsync(er => er.Id == id);

            return rate == null ? null : MapToDto(rate);
        }

        public async Task<ExchangeRateDto?> GetCurrentExchangeRateAsync(int baseAssetId, int quoteAssetId)
        {
            var rate = await _context.ExchangeRates
                .Include(er => er.BaseAsset)
                .Include(er => er.QuoteAsset)
                .Where(er => er.BaseAssetId == baseAssetId && er.QuoteAssetId == quoteAssetId)
                .OrderByDescending(er => er.Timestamp)
                .FirstOrDefaultAsync();

            return rate == null ? null : MapToDto(rate);
        }

        public async Task<ExchangeRateDto> CreateExchangeRateAsync(CreateExchangeRateDto dto)
        {
            var rate = new ExchangeRate
            {
                BaseAssetId = dto.BaseAssetId,
                QuoteAssetId = dto.QuoteAssetId,
                Rate = dto.Rate,
                Provider = dto.Provider,
                Timestamp = dto.Timestamp ?? DateTime.UtcNow
            };

            _context.ExchangeRates.Add(rate);
            await _context.SaveChangesAsync();

            // Reload with includes
            await _context.Entry(rate).Reference(er => er.BaseAsset).LoadAsync();
            await _context.Entry(rate).Reference(er => er.QuoteAsset).LoadAsync();

            return MapToDto(rate);
        }

        public async Task<ExchangeRateDto?> UpdateExchangeRateAsync(int id, UpdateExchangeRateDto dto)
        {
            var rate = await _context.ExchangeRates.FindAsync(id);
            if (rate == null) return null;

            if (dto.Rate.HasValue) rate.Rate = dto.Rate.Value;
            if (dto.Provider != null) rate.Provider = dto.Provider;
            rate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Reload with includes
            await _context.Entry(rate).Reference(er => er.BaseAsset).LoadAsync();
            await _context.Entry(rate).Reference(er => er.QuoteAsset).LoadAsync();

            return MapToDto(rate);
        }

        public async Task<bool> DeleteExchangeRateAsync(int id)
        {
            var rate = await _context.ExchangeRates.FindAsync(id);
            if (rate == null) return false;

            _context.ExchangeRates.Remove(rate);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ExchangeRateDto MapToDto(ExchangeRate rate)
        {
            return new ExchangeRateDto
            {
                Id = rate.Id,
                BaseAssetId = rate.BaseAssetId,
                BaseAssetCode = rate.BaseAsset?.Code,
                QuoteAssetId = rate.QuoteAssetId,
                QuoteAssetCode = rate.QuoteAsset?.Code,
                Rate = rate.Rate,
                Provider = rate.Provider,
                Timestamp = rate.Timestamp,
                CreatedAt = rate.CreatedAt,
                UpdatedAt = rate.UpdatedAt
            };
        }
    }
}