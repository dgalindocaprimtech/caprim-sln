using Caprim.API.Dtos;

namespace Caprim.API.Services.Interfaces
{
    /// <summary>
    /// Interface for User service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Get all users
        /// </summary>
        Task<IEnumerable<UserDto>> GetAllUsersAsync();

        /// <summary>
        /// Get user by ID
        /// </summary>
        Task<UserDto?> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Get user by Cognito sub
        /// </summary>
        Task<UserDto?> GetUserByCognitoSubAsync(string cognitoSub);

        /// <summary>
        /// Create a new user
        /// </summary>
        Task<UserDto> CreateUserAsync(CreateUserDto dto);

        /// <summary>
        /// Update an existing user
        /// </summary>
        Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto dto);

        /// <summary>
        /// Delete a user
        /// </summary>
        Task<bool> DeleteUserAsync(Guid id);
    }
}