using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caprim.API.Models
{
    /// <summary>
    /// Entity for transactions table
    /// </summary>
    [Table("transactions")]
    public class Transaction
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid StellarAccountId { get; set; }

        [Required]
        [StringLength(64)]
        public string StellarTxHash { get; set; } = string.Empty;

        [Required]
        public int AssetId { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Amount { get; set; }

        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual StellarAccount? StellarAccount { get; set; }
        public virtual Asset? Asset { get; set; }
    }
}