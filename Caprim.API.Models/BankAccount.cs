using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caprim.API.Models
{
    /// <summary>
    /// Entity for bank_accounts table
    /// </summary>
    [Table("bank_accounts")]
    public class BankAccount
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int BankId { get; set; }

        [Required]
        public int AccountTypeId { get; set; }

        [StringLength(500)]
        public string? EncryptedAccountNumber { get; set; }

        [StringLength(255)]
        public string? HolderName { get; set; }

        [StringLength(255)]
        public string? HolderDocumentId { get; set; }

        public int? HolderDocumentTypeId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}