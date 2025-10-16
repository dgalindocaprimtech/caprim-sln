using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Caprim.API.Dtos;
using Caprim.API.Services.Interfaces;

namespace Caprim.API.Stellar.Controllers
{
    /// <summary>
    /// Controlador para gestionar trustlines de Stellar.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrustlineController : ControllerBase
    {
        private readonly ILogger<TrustlineController> _logger;
        private readonly IStellarService _stellarService;

        public TrustlineController(ILogger<TrustlineController> logger, IStellarService stellarService)
        {
            _logger = logger;
            _stellarService = stellarService;
        }

        /// <summary>
        /// Establece una línea de confianza (trustline) para un asset conocido en Stellar.
        /// </summary>
        /// <param name="dto">DTO con la semilla secreta y código del asset.</param>
        /// <returns>Hash de la transacción si es exitosa.</returns>
        /// <response code="200">Trustline establecida exitosamente.</response>
        /// <response code="400">Error al establecer la trustline o asset no encontrado.</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> EstablishTrustlineAsync([FromBody] EstablishTrustlineDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transactionHash = await _stellarService.EstablishTrustlineAsync(dto);
                return Ok(transactionHash);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de validación en trustline.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error al establecer la trustline.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al establecer la trustline.");
                return BadRequest($"Error al establecer la trustline: {ex.Message}");
            }
        }
    }
}