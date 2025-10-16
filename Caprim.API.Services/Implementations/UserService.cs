using Caprim.API.Dtos;
using Caprim.API.Models;
using Caprim.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Caprim.API.Services.Implementations
{
    /// <summary>
    /// Implementation of IUserService
    /// </summary>
    public class UserService : IUserService
    {
        private readonly StellarDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(StellarDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Include(u => u.UserStatus)
                .Include(u => u.KycLevel)
                .ToListAsync();

            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.UserStatus)
                .Include(u => u.KycLevel)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDto?> GetUserByCognitoSubAsync(string cognitoSub)
        {
            var user = await _context.Users
                .Include(u => u.UserStatus)
                .Include(u => u.KycLevel)
                .FirstOrDefaultAsync(u => u.CognitoSub == cognitoSub);

            return user == null ? null : MapToDto(user);
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            var user = new User
            {
                CognitoSub = dto.CognitoSub,
                Email = dto.Email,
                UserStatusId = dto.UserStatusId,
                KycLevelId = dto.KycLevelId,
                KycDate = dto.KycDate
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Reload with includes
            await _context.Entry(user).Reference(u => u.UserStatus).LoadAsync();
            await _context.Entry(user).Reference(u => u.KycLevel).LoadAsync();

            return MapToDto(user);
        }

        public async Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.UserStatusId = dto.UserStatusId;
            user.KycLevelId = dto.KycLevelId;
            user.KycDate = dto.KycDate;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Reload with includes
            await _context.Entry(user).Reference(u => u.UserStatus).LoadAsync();
            await _context.Entry(user).Reference(u => u.KycLevel).LoadAsync();

            return MapToDto(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                CognitoSub = user.CognitoSub,
                Email = user.Email,
                UserStatusId = user.UserStatusId,
                UserStatusName = user.UserStatus?.Name,
                KycLevelId = user.KycLevelId,
                KycLevelName = user.KycLevel?.LevelName,
                KycDate = user.KycDate,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}