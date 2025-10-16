using System.ComponentModel.DataAnnotations;

namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO para enviar XLM
    /// </summary>
    public class SendXlmDto
    {
        /// <summary>
        /// Semilla secreta de la cuenta fuente
        /// </summary>
        [Required]
        public string SourceSecretSeed { get; set; } = string.Empty;

        /// <summary>
        /// Cuenta de destino (clave pública)
        /// </summary>
        [Required]
        public string DestinationAccountId { get; set; } = string.Empty;

        /// <summary>
        /// Monto a enviar (en XLM)
        /// </summary>
        [Required]
        public string Amount { get; set; } = string.Empty;
    }
}