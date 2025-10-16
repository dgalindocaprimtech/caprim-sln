namespace Caprim.API.Dtos
{
    /// <summary>
    /// DTO para los balances de la cuenta.
    /// </summary>
    public class AccountBalanceDto
    {
        public string AssetType { get; set; } = string.Empty;
        public string? AssetCode { get; set; }
        public string Balance { get; set; } = string.Empty;
    }
}
