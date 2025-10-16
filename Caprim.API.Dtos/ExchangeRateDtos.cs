using System.ComponentModel.DataAnnotations;

namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO for ExchangeRate response
    /// </summary>
    public class ExchangeRateDto
    {
        public int Id { get; set; }
        public int BaseAssetId { get; set; }
        public string? BaseAssetCode { get; set; }
        public int QuoteAssetId { get; set; }
        public string? QuoteAssetCode { get; set; }
        public decimal Rate { get; set; }
        public string? Provider { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for creating an ExchangeRate
    /// </summary>
    public class CreateExchangeRateDto
    {
        [Required]
        public int BaseAssetId { get; set; }

        [Required]
        public int QuoteAssetId { get; set; }

        [Required]
        [Range(0.00000001, double.MaxValue)]
        public decimal Rate { get; set; }

        [StringLength(100)]
        public string? Provider { get; set; }

        public DateTime? Timestamp { get; set; }
    }

    /// <summary>
    /// DTO for updating an ExchangeRate
    /// </summary>
    public class UpdateExchangeRateDto
    {
        [Range(0.00000001, double.MaxValue)]
        public decimal? Rate { get; set; }

        [StringLength(100)]
        public string? Provider { get; set; }
    }
}