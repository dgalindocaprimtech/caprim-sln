using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caprim.API.Models
{
    /// <summary>
    /// Entity for exchange_rates table
    /// </summary>
    [Table("exchange_rates")]
    public class ExchangeRate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BaseAssetId { get; set; }

        [Required]
        public int QuoteAssetId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Rate { get; set; }

        [StringLength(100)]
        public string? Provider { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual Asset? BaseAsset { get; set; }
        public virtual Asset? QuoteAsset { get; set; }
    }
}