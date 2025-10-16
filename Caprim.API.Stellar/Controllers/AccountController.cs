using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Caprim.API.Dtos;
using Caprim.API.Services.Interfaces;

namespace Caprim.API.Stellar.Controllers
{
    /// <summary>
    /// Controlador para gestionar cuentas de Stellar.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IStellarService _stellarService;

        public AccountController(ILogger<AccountController> logger, IStellarService stellarService)
        {
            _logger = logger;
            _stellarService = stellarService;
        }

        /// <summary>
        /// Crea una nueva cuenta de Stellar en la red de prueba (TestNet).
        /// </summary>
        /// <remarks>
        /// Este endpoint genera un nuevo par de claves (pública y secreta), financia la nueva cuenta con 10,000 XLM de prueba a través de Friendbot y devuelve las credenciales de la cuenta.
        /// </remarks>
        /// <returns>Un objeto `AccountStellar` con la clave pública (KEY) y la semilla secreta (Secret) de la nueva cuenta.</returns>
        /// <response code="200">Retorna las credenciales de la cuenta recién creada.</response>
        [HttpGet(Name = "CreateAccount")]
        [ProducesResponseType(typeof(AccountStellarDto), 200)]
        public async Task<ActionResult<AccountStellarDto>> CreateAccountAsync()
        {
            try
            {
                var accountDto = await _stellarService.CreateAccountAsync();
                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la cuenta.");
                return BadRequest($"Error al crear la cuenta: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene los balances de una cuenta de Stellar específica.
        /// </summary>
        /// <param name="accountId">La clave pública (AccountId) de la cuenta a consultar.</param>
        /// <returns>Una cadena en formato JSON con la lista de balances de la cuenta.</returns>
        /// <response code="200">Retorna los balances de la cuenta.</response>
        /// <response code="404">Si la cuenta no se encuentra.</response>
        [HttpGet("AccountId", Name = "GetAccountBalances")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAccountBalancesAsync(string accountId)
        {
            try
            {
                var balances = await _stellarService.GetAccountBalancesAsync(accountId);
                return Ok(balances);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"Error al obtener balances de la cuenta {accountId}.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error inesperado al obtener balances de la cuenta {accountId}.");
                return BadRequest($"Error al obtener balances: {ex.Message}");
            }
        }
    }
}