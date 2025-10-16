using Caprim.API.Dtos;

namespace Caprim.API.Services.Interfaces
{
    /// <summary>
    /// Interface for StellarAccount service
    /// </summary>
    public interface IStellarAccountService
    {
        /// <summary>
        /// Get all stellar accounts
        /// </summary>
        Task<IEnumerable<StellarAccountDto>> GetAllStellarAccountsAsync();

        /// <summary>
        /// Get stellar accounts by user ID
        /// </summary>
        Task<IEnumerable<StellarAccountDto>> GetStellarAccountsByUserIdAsync(Guid userId);

        /// <summary>
        /// Get stellar account by ID
        /// </summary>
        Task<StellarAccountDto?> GetStellarAccountByIdAsync(Guid id);

        /// <summary>
        /// Get stellar account by public key
        /// </summary>
        Task<StellarAccountDto?> GetStellarAccountByPublicKeyAsync(string publicKey);

        /// <summary>
        /// Create a new stellar account
        /// </summary>
        Task<StellarAccountDto> CreateStellarAccountAsync(CreateStellarAccountDto dto);

        /// <summary>
        /// Update an existing stellar account
        /// </summary>
        Task<StellarAccountDto?> UpdateStellarAccountAsync(Guid id, UpdateStellarAccountDto dto);

        /// <summary>
        /// Delete a stellar account
        /// </summary>
        Task<bool> DeleteStellarAccountAsync(Guid id);
    }
}