using Caprim.API.Dtos;
using Caprim.API.Models;
using Caprim.API.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StellarDotnetSdk;
using StellarDotnetSdk.Accounts;
using StellarDotnetSdk.Assets;
using StellarDotnetSdk.Operations;
using StellarDotnetSdk.Responses;
using StellarDotnetSdk.Transactions;

namespace Caprim.API.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de Stellar
    /// </summary>
    public class StellarService : IStellarService
    {
        private readonly ILogger<StellarService> _logger;
        private readonly StellarSettings _stellarSettings;
        private readonly Server _server;

        public StellarService(ILogger<StellarService> logger, IOptions<StellarSettings> stellarOptions)
        {
            _logger = logger;
            _stellarSettings = stellarOptions.Value;

            // Selección de red y URL según ambiente
            if (_stellarSettings.Environment?.ToUpper() == "DEV")
            {
                Network.UseTestNetwork();
                _server = new Server(_stellarSettings.HorizonUrlDev);
            }
            else
            {
                Network.UsePublicNetwork();
                _server = new Server(_stellarSettings.HorizonUrl);
            }
        }

        /// <summary>
        /// Crea una nueva cuenta de Stellar
        /// </summary>
        public async Task<AccountStellarDto> CreateAccountAsync()
        {
            _logger.LogInformation("Creando un nuevo par de claves de Stellar...");
            var keyPair = GenerateKeyPair();
            _logger.LogInformation($"Clave pública generada: {keyPair.AccountId}");

            if (_stellarSettings.Environment?.ToUpper() == "DEV")
            {
                _logger.LogInformation($"Financiando cuenta {keyPair.AccountId} usando Friendbot...");
                await _server.TestNetFriendBot.FundAccount(keyPair.AccountId).Execute();
                _logger.LogInformation($"Cuenta {keyPair.AccountId} financiada exitosamente.");
            }

            var accountDto = new AccountStellarDto
            {
                PublicKey = keyPair.AccountId ?? string.Empty,
                SecretSeed = keyPair.SecretSeed ?? string.Empty
            };

            return accountDto;
        }

        /// <summary>
        /// Obtiene los balances de una cuenta
        /// </summary>
        public async Task<IEnumerable<AccountBalanceDto>> GetAccountBalancesAsync(string accountId)
        {
            try
            {
                _logger.LogInformation($"Cargando la cuenta {accountId}...");
                var accountResponse = await _server.Accounts.Account(accountId);
                var balances = accountResponse.Balances.Select(b => new AccountBalanceDto
                {
                    AssetType = b.AssetType,
                    AssetCode = b.AssetCode,
                    Balance = b.BalanceString
                }).ToList();
                _logger.LogInformation($"Balances para la cuenta {accountId} obtenidos exitosamente.");
                return balances;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener la cuenta {accountId}.");
                throw new InvalidOperationException($"La cuenta {accountId} no fue encontrada o ocurrió un error.", ex);
            }
        }

        /// <summary>
        /// Establece una trustline para un asset conocido
        /// </summary>
        public async Task<string> EstablishTrustlineAsync(EstablishTrustlineDto dto)
        {
            var assetInfo = StellarAssets.GetAssetInfo(dto.AssetCode);
            if (assetInfo == null)
            {
                throw new ArgumentException($"Asset '{dto.AssetCode}' no encontrado. Assets disponibles: {string.Join(", ", StellarAssets.GetAvailableAssets())}");
            }

            var keyPair = KeyPair.FromSecretSeed(dto.SecretSeed);
            var account = await _server.Accounts.Account(keyPair.AccountId);
            var asset = StellarDotnetSdk.Assets.Asset.CreateNonNativeAsset(dto.AssetCode, assetInfo.Issuer);
            var changeTrustOp = new ChangeTrustOperation(asset, assetInfo.Limit);

            var tx = new TransactionBuilder(account)
                .AddOperation(changeTrustOp)
                .Build();
            tx.Sign(keyPair);

            var response = await SubmitTransactionAsync(tx);
            if (response.IsSuccess)
            {
                _logger.LogInformation($"Trustline para {dto.AssetCode}:{assetInfo.Issuer} establecida. Hash: {response.Hash}");
                return response.Hash;
            }

            _logger.LogError($"Error en la trustline: {response?.ResultXdr}");
            throw new InvalidOperationException($"Error en la trustline: {response?.ResultXdr}");
        }

        /// <summary>
        /// Envía XLM entre cuentas
        /// </summary>
        public async Task<string> SendXlmAsync(SendXlmDto dto)
        {
            var sourceKeyPair = KeyPair.FromSecretSeed(dto.SourceSecretSeed);
            var destinationKeyPair = KeyPair.FromAccountId(dto.DestinationAccountId);

            // Validar existencia de cuenta destino en PROD
            if (_stellarSettings.Environment?.ToUpper() != "DEV")
            {
                try
                {
                    await _server.Accounts.Account(destinationKeyPair.AccountId);
                }
                catch
                {
                    throw new ArgumentException($"La cuenta de destino {destinationKeyPair.AccountId} no existe en la red pública.");
                }
            }

            var sourceAccount = await _server.Accounts.Account(sourceKeyPair.AccountId);
            var paymentOp = new PaymentOperation(destinationKeyPair, new AssetTypeNative(), dto.Amount);

            var tx = new TransactionBuilder(sourceAccount)
                .AddOperation(paymentOp)
                .Build();

            tx.Sign(sourceKeyPair);

            var response = await SubmitTransactionAsync(tx);

            if (response.IsSuccess)
            {
                _logger.LogInformation($"Transacción XLM exitosa. Hash: {response.Hash}");
                return response.Hash;
            }

            _logger.LogError($"Error en la transacción: {response?.ResultXdr}");
            throw new InvalidOperationException($"Error en la transacción: {response?.ResultXdr}");
        }

        /// <summary>
        /// Envía USDC entre cuentas con validaciones completas
        /// </summary>
        public async Task<string> SendUsdcAsync(SendUsdcDto dto)
        {
            // Obtener información del asset USDC
            var assetInfo = StellarAssets.GetAssetInfo("USDC");
            if (assetInfo == null)
            {
                throw new InvalidOperationException("Información de USDC no encontrada en el sistema.");
            }

            // Crear keypairs
            var sourceKeyPair = KeyPair.FromSecretSeed(dto.SourceSecretSeed);
            var destinationKeyPair = KeyPair.FromAccountId(dto.DestinationAccountId);

            // Validar existencia de cuenta remitente
            AccountResponse sourceAccount;
            try
            {
                sourceAccount = await _server.Accounts.Account(sourceKeyPair.AccountId);
                _logger.LogInformation($"Cuenta remitente {sourceKeyPair.AccountId} validada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cuenta remitente {sourceKeyPair.AccountId} no existe o no es accesible.");
                throw new ArgumentException($"La cuenta remitente {sourceKeyPair.AccountId} no existe o no es accesible.", ex);
            }

            // Validar existencia de cuenta destino
            try
            {
                await _server.Accounts.Account(destinationKeyPair.AccountId);
                _logger.LogInformation($"Cuenta destino {destinationKeyPair.AccountId} validada.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Cuenta destino {destinationKeyPair.AccountId} no existe o no es accesible.");
                throw new ArgumentException($"La cuenta destino {destinationKeyPair.AccountId} no existe o no es accesible.", ex);
            }

            // Verificar que la cuenta remitente tenga trustline para USDC
            var hasUsdcTrustline = sourceAccount.Balances.Any(b =>
                b.AssetCode == "USDC" &&
                b.AssetIssuer == assetInfo.Issuer);

            if (!hasUsdcTrustline)
            {
                throw new InvalidOperationException("La cuenta remitente no tiene trustline establecida para USDC. Establezca la trustline primero.");
            }

            // Verificar que la cuenta destino tenga trustline para USDC
            try
            {
                var destAccount = await _server.Accounts.Account(destinationKeyPair.AccountId);
                var destHasUsdcTrustline = destAccount.Balances.Any(b =>
                    b.AssetCode == "USDC" &&
                    b.AssetIssuer == assetInfo.Issuer);

                if (!destHasUsdcTrustline)
                {
                    throw new InvalidOperationException("La cuenta destino no tiene trustline establecida para USDC.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar trustline de cuenta destino.");
                throw new InvalidOperationException($"Error al verificar trustline de cuenta destino: {ex.Message}", ex);
            }

            // Verificar balance suficiente
            var usdcBalance = sourceAccount.Balances.FirstOrDefault(b =>
                b.AssetCode == "USDC" &&
                b.AssetIssuer == assetInfo.Issuer);

            if (usdcBalance == null || decimal.Parse(usdcBalance.BalanceString) < decimal.Parse(dto.Amount))
            {
                throw new InvalidOperationException($"Balance insuficiente de USDC. Balance actual: {usdcBalance?.BalanceString ?? "0"}");
            }

            // Crear la transacción
            var usdcAsset = StellarDotnetSdk.Assets.Asset.CreateNonNativeAsset("USDC", assetInfo.Issuer);
            var paymentOp = new PaymentOperation(destinationKeyPair, usdcAsset, dto.Amount);

            var tx = new TransactionBuilder(sourceAccount)
                .AddOperation(paymentOp)
                .Build();

            tx.Sign(sourceKeyPair);

            var response = await SubmitTransactionAsync(tx);

            if (response.IsSuccess)
            {
                _logger.LogInformation($"Transacción USDC exitosa. Hash: {response.Hash}");
                return response.Hash;
            }

            _logger.LogError($"Error en la transacción USDC: {response?.ResultXdr}");
            throw new InvalidOperationException($"Error en la transacción USDC: {response?.ResultXdr}");
        }

        /// <summary>
        /// Envía una transacción a la red Stellar
        /// </summary>
        private async Task<SubmitTransactionResponse> SubmitTransactionAsync(StellarDotnetSdk.Transactions.Transaction transaction)
        {
            try
            {
                _logger.LogInformation($"Enviando transacción: {transaction.ToEnvelopeXdrBase64()}");
                var response = await _server.SubmitTransaction(transaction);
                if (response == null)
                {
                    _logger.LogError($"La transacción falló. Result XDR: {response?.ResultXdr}");
                    throw new InvalidOperationException("No se recibió respuesta de la red Stellar.");
                }
                _logger.LogInformation($"Transacción exitosa. Hash: {response.Hash}");
                return response;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Excepción al enviar la transacción.");
                throw;
            }
        }

        /// <summary>
        /// Genera un nuevo par de claves
        /// </summary>
        private static KeyPair GenerateKeyPair()
        {
            var keyPair = KeyPair.Random();
            // No mostrar la clave secreta en logs por seguridad
            return keyPair;
        }
    }
}