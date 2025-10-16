using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Caprim.API.Dtos;
using Caprim.API.Services.Interfaces;

namespace Caprim.API.Stellar.Controllers
{
    /// <summary>
    /// Controlador para gestionar transacciones de Stellar.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ILogger<TransactionController> _logger;
        private readonly IStellarService _stellarService;

        public TransactionController(ILogger<TransactionController> logger, IStellarService stellarService)
        {
            _logger = logger;
            _stellarService = stellarService;
        }

        /// <summary>
        /// Envía XLM de una cuenta a otra, considerando el entorno (DEV o PROD).
        /// </summary>
        /// <param name="sendXlmDto">DTO con los datos de la transacción.</param>
        /// <returns>Hash de la transacción si es exitosa.</returns>
        /// <response code="200">Transacción exitosa.</response>
        /// <response code="400">Error en la transacción.</response>
        [HttpPost("send-xlm")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> SendXlmAsync([FromBody] SendXlmDto sendXlmDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transactionHash = await _stellarService.SendXlmAsync(sendXlmDto);
                return Ok(transactionHash);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de validación en envío de XLM.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error al enviar XLM.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al enviar XLM.");
                return BadRequest($"Error al enviar XLM: {ex.Message}");
            }
        }

        /// <summary>
        /// Envía USDC de una cuenta a otra, validando la existencia de ambas cuentas.
        /// </summary>
        /// <param name="sendUsdcDto">DTO con los datos de la transacción.</param>
        /// <returns>Hash de la transacción si es exitosa.</returns>
        /// <response code="200">Transacción exitosa.</response>
        /// <response code="400">Error en la transacción o cuentas no válidas.</response>
        [HttpPost("send-usdc")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> SendUsdcAsync([FromBody] SendUsdcDto sendUsdcDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transactionHash = await _stellarService.SendUsdcAsync(sendUsdcDto);
                return Ok(transactionHash);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de validación en envío de USDC.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error al enviar USDC.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al enviar USDC.");
                return BadRequest($"Error al enviar USDC: {ex.Message}");
            }
        }
    }
}