using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caprim.API.Models
{
    /// <summary>
    /// Entity for users table
    /// </summary>
    [Table("users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string CognitoSub { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int UserStatusId { get; set; }

        [Required]
        public int KycLevelId { get; set; }

        public DateTime? KycDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("UserStatusId")]
        public UserStatus? UserStatus { get; set; }

        [ForeignKey("KycLevelId")]
        public KycLevel? KycLevel { get; set; }

        public UserProfile? UserProfile { get; set; }
        public ICollection<StellarAccount> StellarAccounts { get; set; } = new List<StellarAccount>();
        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    }
}