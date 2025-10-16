using System.ComponentModel.DataAnnotations;

namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO for Transaction response
    /// </summary>
    public class TransactionDto
    {
        public long Id { get; set; }
        public Guid StellarAccountId { get; set; }
        public string StellarTxHash { get; set; } = string.Empty;
        public int AssetId { get; set; }
        public string? AssetCode { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ProcessedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for creating a Transaction
    /// </summary>
    public class CreateTransactionDto
    {
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
        [Range(0.00000001, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime? ProcessedAt { get; set; }
    }
}