using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Caprim.API.Dtos;
using Caprim.API.Services.Interfaces;

namespace Caprim.API.Stellar.Controllers
{
    /// <summary>
    /// Controller for managing exchange rates.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger<ExchangeRatesController> _logger;

        public ExchangeRatesController(IExchangeRateService exchangeRateService, ILogger<ExchangeRatesController> logger)
        {
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }

        /// <summary>
        /// Get all exchange rates.
        /// </summary>
        /// <returns>A list of exchange rates.</returns>
        /// <response code="200">Returns the list of exchange rates.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExchangeRateDto>), 200)]
        public async Task<IActionResult> GetAllExchangeRates()
        {
            try
            {
                var rates = await _exchangeRateService.GetAllExchangeRatesAsync();
                return Ok(rates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all exchange rates.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get an exchange rate by ID.
        /// </summary>
        /// <param name="id">The exchange rate ID.</param>
        /// <returns>The exchange rate.</returns>
        /// <response code="200">Returns the exchange rate.</response>
        /// <response code="404">If the exchange rate is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExchangeRateDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetExchangeRateById(int id)
        {
            try
            {
                var rate = await _exchangeRateService.GetExchangeRateByIdAsync(id);
                if (rate == null)
                {
                    return NotFound();
                }
                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving exchange rate with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Get the current exchange rate between two assets.
        /// </summary>
        /// <param name="baseAssetId">The base asset ID.</param>
        /// <param name="quoteAssetId">The quote asset ID.</param>
        /// <returns>The current exchange rate.</returns>
        /// <response code="200">Returns the exchange rate.</response>
        /// <response code="404">If the exchange rate is not found.</response>
        [HttpGet("current/{baseAssetId}/{quoteAssetId}")]
        [ProducesResponseType(typeof(ExchangeRateDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCurrentExchangeRate(int baseAssetId, int quoteAssetId)
        {
            try
            {
                var rate = await _exchangeRateService.GetCurrentExchangeRateAsync(baseAssetId, quoteAssetId);
                if (rate == null)
                {
                    return NotFound();
                }
                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving current exchange rate for assets {BaseAssetId} and {QuoteAssetId}.", baseAssetId, quoteAssetId);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Create a new exchange rate.
        /// </summary>
        /// <param name="dto">The exchange rate data.</param>
        /// <returns>The created exchange rate.</returns>
        /// <response code="201">Returns the created exchange rate.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ExchangeRateDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateExchangeRate([FromBody] CreateExchangeRateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var rate = await _exchangeRateService.CreateExchangeRateAsync(dto);
                return CreatedAtAction(nameof(GetExchangeRateById), new { id = rate.Id }, rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exchange rate.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Update an existing exchange rate.
        /// </summary>
        /// <param name="id">The exchange rate ID.</param>
        /// <param name="dto">The updated exchange rate data.</param>
        /// <returns>The updated exchange rate.</returns>
        /// <response code="200">Returns the updated exchange rate.</response>
        /// <response code="404">If the exchange rate is not found.</response>
        /// <response code="400">If the data is invalid.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ExchangeRateDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateExchangeRate(int id, [FromBody] UpdateExchangeRateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var rate = await _exchangeRateService.UpdateExchangeRateAsync(id, dto);
                if (rate == null)
                {
                    return NotFound();
                }
                return Ok(rate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exchange rate with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Delete an exchange rate.
        /// </summary>
        /// <param name="id">The exchange rate ID.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Exchange rate deleted.</response>
        /// <response code="404">If the exchange rate is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteExchangeRate(int id)
        {
            try
            {
                var result = await _exchangeRateService.DeleteExchangeRateAsync(id);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting exchange rate with ID {Id}.", id);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}