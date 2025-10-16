using System.ComponentModel.DataAnnotations;

namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO para establecer un trustline
    /// </summary>
    public class EstablishTrustlineDto
    {
        /// <summary>
        /// La semilla secreta de la cuenta
        /// </summary>
        [Required]
        public string SecretSeed { get; set; } = string.Empty;

        /// <summary>
        /// Código del asset (por ejemplo, USDT)
        /// </summary>
        [Required]
        public string AssetCode { get; set; } = string.Empty;
    }
}
