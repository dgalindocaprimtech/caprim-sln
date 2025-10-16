using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Caprim.API.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el middleware de manejo de errores
    /// </summary>
    public interface IErrorHandlingMiddleware
    {
        Task InvokeAsync(HttpContext context);
    }
}