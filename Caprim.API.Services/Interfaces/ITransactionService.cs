using Caprim.API.Dtos;

namespace Caprim.API.Services.Interfaces
{
    /// <summary>
    /// Interface for Transaction service
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Get all transactions
        /// </summary>
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();

        /// <summary>
        /// Get transactions by stellar account ID
        /// </summary>
        Task<IEnumerable<TransactionDto>> GetTransactionsByStellarAccountIdAsync(Guid stellarAccountId);

        /// <summary>
        /// Get transaction by ID
        /// </summary>
        Task<TransactionDto?> GetTransactionByIdAsync(long id);

        /// <summary>
        /// Get transaction by stellar tx hash
        /// </summary>
        Task<TransactionDto?> GetTransactionByStellarTxHashAsync(string stellarTxHash);

        /// <summary>
        /// Create a new transaction
        /// </summary>
        Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto dto);

        /// <summary>
        /// Delete a transaction
        /// </summary>
        Task<bool> DeleteTransactionAsync(long id);
    }
}