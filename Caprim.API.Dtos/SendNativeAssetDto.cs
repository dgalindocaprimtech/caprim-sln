namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO para enviar activos nativos.
    /// </summary>
    public class SendNativeAssetDto
    {
        public string SecretSeed { get; set; } = string.Empty;
        public string DestinationAccountId { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
    }
}
