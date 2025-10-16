using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Caprim.API.Dtos;
using Caprim.API.Services.Interfaces;

namespace Caprim.API.Stellar.Controllers
{
    /// <summary>
    /// Controller for managing transactions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        /// <summary>
        /// Get all transactions.
        /// </summary>
        /// <returns>A list of transactions.</returns>
        /// <response code="200">Returns the list of transactions.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), 200)]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all transactions.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get transactions by Stellar account ID.
        /// </summary>
        /// <param name="stellarAccountId">The Stellar account ID.</param>
        /// <returns>A list of transactions for the account.</returns>
        /// <response code="200">Returns the list of transactions.</response>
        [HttpGet("account/{stellarAccountId}")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), 200)]
        public async Task<IActionResult> GetTransactionsByStellarAccountId(Guid stellarAccountId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByStellarAccountIdAsync(stellarAccountId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for Stellar account {StellarAccountId}.", stellarAccountId);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get a transaction by ID.
        /// </summary>
        /// <param name="id">The transaction ID.</param>
        /// <returns>The transaction.</returns>
        /// <response code="200">Returns the transaction.</response>
        /// <response code="404">If the transaction is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TransactionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTransactionById(long id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return NotFound();
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transaction with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get a transaction by Stellar transaction hash.
        /// </summary>
        /// <param name="stellarTxHash">The Stellar transaction hash.</param>
        /// <returns>The transaction.</returns>
        /// <response code="200">Returns the transaction.</response>
        /// <response code="404">If the transaction is not found.</response>
        [HttpGet("hash/{stellarTxHash}")]
        [ProducesResponseType(typeof(TransactionDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTransactionByStellarTxHash(string stellarTxHash)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByStellarTxHashAsync(stellarTxHash);
                if (transaction == null)
                {
                    return NotFound();
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transaction with hash {StellarTxHash}.", stellarTxHash);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Create a new transaction.
        /// </summary>
        /// <param name="dto">The transaction data.</param>
        /// <returns>The created transaction.</returns>
        /// <response code="201">Returns the created transaction.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TransactionDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var transaction = await _transactionService.CreateTransactionAsync(dto);
                return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transaction.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Delete a transaction.
        /// </summary>
        /// <param name="id">The transaction ID.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Transaction deleted.</response>
        /// <response code="404">If the transaction is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTransaction(long id)
        {
            try
            {
                var result = await _transactionService.DeleteTransactionAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transaction with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}