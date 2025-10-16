using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Caprim.API.Dtos;
using Caprim.API.Services.Interfaces;

namespace Caprim.API.Serverless.Controllers
{
    /// <summary>
    /// Controller for managing Stellar accounts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StellarAccountsController : ControllerBase
    {
        private readonly IStellarAccountService _stellarAccountService;
        private readonly ILogger<StellarAccountsController> _logger;

        public StellarAccountsController(IStellarAccountService stellarAccountService, ILogger<StellarAccountsController> logger)
        {
            _stellarAccountService = stellarAccountService;
            _logger = logger;
        }

        /// <summary>
        /// Get all Stellar accounts.
        /// </summary>
        /// <returns>A list of Stellar accounts.</returns>
        /// <response code="200">Returns the list of Stellar accounts.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StellarAccountDto>), 200)]
        public async Task<IActionResult> GetAllStellarAccounts()
        {
            try
            {
                var accounts = await _stellarAccountService.GetAllStellarAccountsAsync();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all Stellar accounts.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get Stellar accounts by user ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>A list of Stellar accounts for the user.</returns>
        /// <response code="200">Returns the list of Stellar accounts.</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<StellarAccountDto>), 200)]
        public async Task<IActionResult> GetStellarAccountsByUserId(Guid userId)
        {
            try
            {
                var accounts = await _stellarAccountService.GetStellarAccountsByUserIdAsync(userId);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Stellar accounts for user {UserId}.", userId);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get a Stellar account by ID.
        /// </summary>
        /// <param name="id">The Stellar account ID.</param>
        /// <returns>The Stellar account.</returns>
        /// <response code="200">Returns the Stellar account.</response>
        /// <response code="404">If the account is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StellarAccountDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStellarAccountById(Guid id)
        {
            try
            {
                var account = await _stellarAccountService.GetStellarAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound();
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Stellar account with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get a Stellar account by public key.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <returns>The Stellar account.</returns>
        /// <response code="200">Returns the Stellar account.</response>
        /// <response code="404">If the account is not found.</response>
        [HttpGet("publickey/{publicKey}")]
        [ProducesResponseType(typeof(StellarAccountDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStellarAccountByPublicKey(string publicKey)
        {
            try
            {
                var account = await _stellarAccountService.GetStellarAccountByPublicKeyAsync(publicKey);
                if (account == null)
                {
                    return NotFound();
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Stellar account with public key {PublicKey}.", publicKey);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Create a new Stellar account.
        /// </summary>
        /// <param name="dto">The Stellar account data.</param>
        /// <returns>The created Stellar account.</returns>
        /// <response code="201">Returns the created Stellar account.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(StellarAccountDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateStellarAccount([FromBody] CreateStellarAccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var account = await _stellarAccountService.CreateStellarAccountAsync(dto);
                return CreatedAtAction(nameof(GetStellarAccountById), new { id = account.Id }, account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Stellar account.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Update an existing Stellar account.
        /// </summary>
        /// <param name="id">The Stellar account ID.</param>
        /// <param name="dto">The updated Stellar account data.</param>
        /// <returns>The updated Stellar account.</returns>
        /// <response code="200">Returns the updated Stellar account.</response>
        /// <response code="404">If the account is not found.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StellarAccountDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateStellarAccount(Guid id, [FromBody] UpdateStellarAccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var account = await _stellarAccountService.UpdateStellarAccountAsync(id, dto);
                if (account == null)
                {
                    return NotFound();
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Stellar account with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Delete a Stellar account.
        /// </summary>
        /// <param name="id">The Stellar account ID.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Account deleted.</response>
        /// <response code="404">If the account is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteStellarAccount(Guid id)
        {
            try
            {
                var result = await _stellarAccountService.DeleteStellarAccountAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Stellar account with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}