using Caprim.API.Dtos;
using Caprim.API.Models;
using Caprim.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Caprim.API.Services.Implementations
{
    /// <summary>
    /// Implementation of IStellarAccountService
    /// </summary>
    public class StellarAccountService : IStellarAccountService
    {
        private readonly StellarDbContext _context;
        private readonly ILogger<StellarAccountService> _logger;

        public StellarAccountService(StellarDbContext context, ILogger<StellarAccountService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<StellarAccountDto>> GetAllStellarAccountsAsync()
        {
            var accounts = await _context.StellarAccounts.ToListAsync();
            return accounts.Select(MapToDto);
        }

        public async Task<IEnumerable<StellarAccountDto>> GetStellarAccountsByUserIdAsync(Guid userId)
        {
            var accounts = await _context.StellarAccounts
                .Where(sa => sa.UserId == userId)
                .ToListAsync();

            return accounts.Select(MapToDto);
        }

        public async Task<StellarAccountDto?> GetStellarAccountByIdAsync(Guid id)
        {
            var account = await _context.StellarAccounts.FindAsync(id);
            return account == null ? null : MapToDto(account);
        }

        public async Task<StellarAccountDto?> GetStellarAccountByPublicKeyAsync(string publicKey)
        {
            var account = await _context.StellarAccounts
                .FirstOrDefaultAsync(sa => sa.PublicKey == publicKey);

            return account == null ? null : MapToDto(account);
        }

        public async Task<StellarAccountDto> CreateStellarAccountAsync(CreateStellarAccountDto dto)
        {
            var account = new StellarAccount
            {
                UserId = dto.UserId,
                PublicKey = dto.PublicKey,
                KmsKeyArn = dto.KmsKeyArn,
                AccountName = dto.AccountName,
                IsActive = dto.IsActive
            };

            _context.StellarAccounts.Add(account);
            await _context.SaveChangesAsync();

            return MapToDto(account);
        }

        public async Task<StellarAccountDto?> UpdateStellarAccountAsync(Guid id, UpdateStellarAccountDto dto)
        {
            var account = await _context.StellarAccounts.FindAsync(id);
            if (account == null) return null;

            if (dto.AccountName != null) account.AccountName = dto.AccountName;
            if (dto.IsActive.HasValue) account.IsActive = dto.IsActive.Value;
            account.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToDto(account);
        }

        public async Task<bool> DeleteStellarAccountAsync(Guid id)
        {
            var account = await _context.StellarAccounts.FindAsync(id);
            if (account == null) return false;

            _context.StellarAccounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        private static StellarAccountDto MapToDto(StellarAccount account)
        {
            return new StellarAccountDto
            {
                Id = account.Id,
                UserId = account.UserId,
                PublicKey = account.PublicKey,
                KmsKeyArn = account.KmsKeyArn,
                AccountName = account.AccountName,
                IsActive = account.IsActive,
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt
            };
        }
    }
}