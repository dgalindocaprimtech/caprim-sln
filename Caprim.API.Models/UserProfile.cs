using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caprim.API.Models
{
    /// <summary>
    /// Entity for user_profiles table
    /// </summary>
    [Table("user_profiles")]
    public class UserProfile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [StringLength(500)]
        public string? EncryptedFirstName { get; set; }

        [StringLength(500)]
        public string? EncryptedLastName { get; set; }

        [StringLength(1000)]
        public string? EncryptedAddress { get; set; }

        [StringLength(500)]
        public string? EncryptedPhone { get; set; }

        [StringLength(500)]
        public string? EncryptedDocumentNumber { get; set; }

        public int? DocumentTypeId { get; set; }

        [StringLength(100)]
        public string? Nationality { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}