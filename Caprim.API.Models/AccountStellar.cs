namespace Caprim.API.Models
{
    /// <summary>
    /// Representa una cuenta de Stellar con su clave pública y secreta
    /// </summary>
    public class AccountStellar
    {
        /// <summary>
        /// Clave pública de la cuenta (KEY)
        /// </summary>
        public string? KEY { get; set; }

        /// <summary>
        /// Semilla secreta de la cuenta
        /// </summary>
        public string? Secret { get; set; }
    }
}