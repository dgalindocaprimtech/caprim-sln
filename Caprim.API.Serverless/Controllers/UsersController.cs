using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Caprim.API.Dtos;
using Caprim.API.Services.Interfaces;

namespace Caprim.API.Serverless.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Re-habilitar autenticación JWT
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>A list of users.</returns>
        /// <response code="200">Returns the list of users.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get a user by ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user.</returns>
        /// <response code="200">Returns the user.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get a user by Cognito sub.
        /// </summary>
        /// <param name="cognitoSub">The Cognito sub.</param>
        /// <returns>The user.</returns>
        /// <response code="200">Returns the user.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpGet("cognito/{cognitoSub}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUserByCognitoSub(string cognitoSub)
        {
            try
            {
                var user = await _userService.GetUserByCognitoSubAsync(cognitoSub);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with Cognito sub {CognitoSub}.", cognitoSub);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="dto">The user data.</param>
        /// <returns>The created user.</returns>
        /// <response code="201">Returns the created user.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="dto">The updated user data.</param>
        /// <returns>The updated user.</returns>
        /// <response code="200">Returns the updated user.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userService.UpdateUserAsync(id, dto);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Delete a user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>No content.</returns>
        /// <response code="204">User deleted.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}