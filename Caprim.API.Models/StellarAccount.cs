using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caprim.API.Models
{
    /// <summary>
    /// Entity for stellar_accounts table
    /// </summary>
    [Table("stellar_accounts")]
    public class StellarAccount
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(56)]
        public string PublicKey { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string KmsKeyArn { get; set; } = string.Empty;

        [StringLength(100)]
        public string? AccountName { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}