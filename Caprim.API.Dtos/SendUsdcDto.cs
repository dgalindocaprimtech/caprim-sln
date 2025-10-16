using System.ComponentModel.DataAnnotations;

namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO para enviar USDC entre cuentas
    /// </summary>
    public class SendUsdcDto
    {
        /// <summary>
        /// Semilla secreta de la cuenta remitente
        /// </summary>
        [Required]
        public string SourceSecretSeed { get; set; } = string.Empty;

        /// <summary>
        /// Cuenta de destino (clave pública)
        /// </summary>
        [Required]
        public string DestinationAccountId { get; set; } = string.Empty;

        /// <summary>
        /// Monto de USDC a enviar
        /// </summary>
        [Required]
        public string Amount { get; set; } = string.Empty;
    }
}