using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caprim.API.Models
{
    /// <summary>
    /// Entity for kyc_levels table
    /// </summary>
    [Table("kyc_levels")]
    public class KycLevel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string LevelName { get; set; } = string.Empty;

        [Required]
        [StringLength(2)]
        public string CountryCode { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}