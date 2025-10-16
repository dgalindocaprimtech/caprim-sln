namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO para la respuesta de creación de cuenta.
    /// </summary>
    public class AccountStellarDto
    {
        public string PublicKey { get; set; } = string.Empty;
        public string SecretSeed { get; set; } = string.Empty;
    }
}
