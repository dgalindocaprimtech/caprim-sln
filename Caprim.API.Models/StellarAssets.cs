using System.Collections.Generic;

namespace Caprim.API.Models
{
    /// <summary>
    /// Información de un asset conocido en Stellar
    /// </summary>
    public class StellarAssetInfo
    {
        public string Issuer { get; set; } = string.Empty;
        public string Limit { get; set; } = "1000000"; // Límite por defecto
    }

    /// <summary>
    /// Diccionario de assets conocidos en Stellar
    /// </summary>
    public static class StellarAssets
    {
        private static readonly Dictionary<string, StellarAssetInfo> _assets = new Dictionary<string, StellarAssetInfo>
        {
            // USDT (Tether)
            ["USDT"] = new StellarAssetInfo
            {
                Issuer = "GDHP3Z3R5Q6Q4LKWMYZ5NSB6HK2UBW7J2RD2BZ5GK37Q6TJQGZUX5X",
                Limit = "1000000"
            },
            // USDC (USD Coin)
            ["USDC"] = new StellarAssetInfo
            {
                Issuer = "GA5ZSEJYB37JRC5AVCIA5MOP4RHTM335X2KGX3IHOJAPP5RE34K4KZVN",
                Limit = "1000000"
            },
            // EURT (Euro Tether)
            ["EURT"] = new StellarAssetInfo
            {
                Issuer = "GAP5LETOV6YIE62YAM56STDANPRDO7ZFDBGSNHJQIYGGKSMOZAHOOS2S",
                Limit = "1000000"
            },
            // BTC (Bitcoin)
            ["BTC"] = new StellarAssetInfo
            {
                Issuer = "GCKFBEIYV2U22IO5KDLO7H6P4CXW4ZBKC7USG5JTCL6S2DCQI3M4R7",
                Limit = "1000000"
            },
            // ETH (Ethereum)
            ["ETH"] = new StellarAssetInfo
            {
                Issuer = "GCKFBEIYV2U22IO5KDLO7H6P4CXW4ZBKC7USG5JTCL6S2DCQI3M4R7",
                Limit = "1000000"
            },
            // Puedes agregar más assets aquí
        };

        /// <summary>
        /// Obtiene la información de un asset por su código
        /// </summary>
        public static StellarAssetInfo? GetAssetInfo(string assetCode)
        {
            return _assets.TryGetValue(assetCode.ToUpper(), out var info) ? info : null;
        }

        /// <summary>
        /// Lista de códigos de assets disponibles
        /// </summary>
        public static IEnumerable<string> GetAvailableAssets()
        {
            return _assets.Keys;
        }
    }
}