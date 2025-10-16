using System.ComponentModel.DataAnnotations;

namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO for User response
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string CognitoSub { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int UserStatusId { get; set; }
        public string? UserStatusName { get; set; }
        public int KycLevelId { get; set; }
        public string? KycLevelName { get; set; }
        public DateTime? KycDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for creating a User
    /// </summary>
    public class CreateUserDto
    {
        [Required]
        [StringLength(255)]
        public string CognitoSub { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int UserStatusId { get; set; }

        [Required]
        public int KycLevelId { get; set; }

        public DateTime? KycDate { get; set; }
    }

    /// <summary>
    /// DTO for updating a User
    /// </summary>
    public class UpdateUserDto
    {
        [Required]
        public int UserStatusId { get; set; }

        [Required]
        public int KycLevelId { get; set; }

        public DateTime? KycDate { get; set; }
    }
}