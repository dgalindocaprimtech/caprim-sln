using Caprim.API.Dtos;
using Caprim.API.Models;
using Caprim.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Caprim.API.Services.Implementations
{
    /// <summary>
    /// Implementation of ITransactionService
    /// </summary>
    public class TransactionService : ITransactionService
    {
        private readonly StellarDbContext _context;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(StellarDbContext context, ILogger<TransactionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync()
        {
            var transactions = await _context.Transactions
                .Include(t => t.StellarAccount)
                .Include(t => t.Asset)
                .ToListAsync();

            return transactions.Select(MapToDto);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByStellarAccountIdAsync(Guid stellarAccountId)
        {
            var transactions = await _context.Transactions
                .Include(t => t.StellarAccount)
                .Include(t => t.Asset)
                .Where(t => t.StellarAccountId == stellarAccountId)
                .ToListAsync();

            return transactions.Select(MapToDto);
        }

        public async Task<TransactionDto?> GetTransactionByIdAsync(long id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.StellarAccount)
                .Include(t => t.Asset)
                .FirstOrDefaultAsync(t => t.Id == id);

            return transaction == null ? null : MapToDto(transaction);
        }

        public async Task<TransactionDto?> GetTransactionByStellarTxHashAsync(string stellarTxHash)
        {
            var transaction = await _context.Transactions
                .Include(t => t.StellarAccount)
                .Include(t => t.Asset)
                .FirstOrDefaultAsync(t => t.StellarTxHash == stellarTxHash);

            return transaction == null ? null : MapToDto(transaction);
        }

        public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionDto dto)
        {
            var transaction = new Transaction
            {
                StellarAccountId = dto.StellarAccountId,
                StellarTxHash = dto.StellarTxHash,
                AssetId = dto.AssetId,
                Type = dto.Type,
                Amount = dto.Amount,
                ProcessedAt = dto.ProcessedAt ?? DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Reload with includes
            await _context.Entry(transaction).Reference(t => t.StellarAccount).LoadAsync();
            await _context.Entry(transaction).Reference(t => t.Asset).LoadAsync();

            return MapToDto(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(long id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        private static TransactionDto MapToDto(Transaction transaction)
        {
            return new TransactionDto
            {
                Id = transaction.Id,
                StellarAccountId = transaction.StellarAccountId,
                StellarTxHash = transaction.StellarTxHash,
                AssetId = transaction.AssetId,
                AssetCode = transaction.Asset?.Code,
                Type = transaction.Type,
                Amount = transaction.Amount,
                ProcessedAt = transaction.ProcessedAt,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }
    }
}