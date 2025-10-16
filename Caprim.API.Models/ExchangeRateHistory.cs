using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caprim.API.Models
{
    /// <summary>
    /// Entity for exchange_rates_history table
    /// </summary>
    [Table("exchange_rates_history")]
    public class ExchangeRateHistory
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public int ExchangeRateId { get; set; }

        [Column(TypeName = "decimal(18,8)")]
        public decimal? OldRate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal NewRate { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}