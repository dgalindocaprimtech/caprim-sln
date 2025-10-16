using Caprim.API.Dtos;
using Caprim.API.Models;

namespace Caprim.API.Services.Interfaces
{
    /// <summary>
    /// Interfaz para servicios de Stellar
    /// </summary>
    public interface IStellarService
    {
        /// <summary>
        /// Crea una nueva cuenta de Stellar
        /// </summary>
        Task<AccountStellarDto> CreateAccountAsync();

        /// <summary>
        /// Obtiene los balances de una cuenta
        /// </summary>
        Task<IEnumerable<AccountBalanceDto>> GetAccountBalancesAsync(string accountId);

        /// <summary>
        /// Establece una trustline para un asset conocido
        /// </summary>
        Task<string> EstablishTrustlineAsync(EstablishTrustlineDto dto);

        /// <summary>
        /// Envía XLM entre cuentas
        /// </summary>
        Task<string> SendXlmAsync(SendXlmDto dto);

        /// <summary>
        /// Envía USDC entre cuentas con validaciones completas
        /// </summary>
        Task<string> SendUsdcAsync(SendUsdcDto dto);
    }
}