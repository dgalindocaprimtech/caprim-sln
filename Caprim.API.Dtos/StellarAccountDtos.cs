using System.ComponentModel.DataAnnotations;

namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO for StellarAccount response
    /// </summary>
    public class StellarAccountDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PublicKey { get; set; } = string.Empty;
        public string KmsKeyArn { get; set; } = string.Empty;
        public string? AccountName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for creating a StellarAccount
    /// </summary>
    public class CreateStellarAccountDto
    {
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
    }

    /// <summary>
    /// DTO for updating a StellarAccount
    /// </summary>
    public class UpdateStellarAccountDto
    {
        [StringLength(100)]
        public string? AccountName { get; set; }

        public bool? IsActive { get; set; }
    }
}