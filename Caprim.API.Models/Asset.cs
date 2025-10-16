using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Caprim.API.Models
{
    /// <summary>
    /// Entity for assets table
    /// </summary>
    [Table("assets")]
    public class Asset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(12)]
        public string Code { get; set; } = string.Empty;

        [StringLength(56)]
        public string? Issuer { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}