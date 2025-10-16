using Amazon.Lambda.AspNetCoreServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Caprim.API.Services.Implementations;

namespace Caprim.API.Stellar;

/// <summary>
/// Función Lambda que sirve como punto de entrada para ASP.NET Core en AWS Lambda.
/// Esta clase maneja las solicitudes HTTP desde API Gateway.
/// </summary>
public class LambdaEntryPoint : APIGatewayProxyFunction
{
    /// <summary>
    /// Este método se llama para inicializar la aplicación ASP.NET Core.
    /// </summary>
    protected override void Init(IWebHostBuilder builder)
    {
        builder
            .UseStartup<LambdaStartup>();
    }
}

/// <summary>
/// Clase Startup específica para Lambda que configura la aplicación.
/// </summary>
public class LambdaStartup
{
    public LambdaStartup(Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

    public void ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
    {
        // Configuración de StellarSettings
        services.Configure<Models.StellarSettings>(
            Configuration.GetSection("Stellar"));

        // Add DbContext
        services.AddDbContext<Models.StellarDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        // Registrar servicios
        services.AddScoped<Services.Interfaces.IStellarService, Services.Implementations.StellarService>();
        services.AddScoped<Services.Interfaces.IUserService, Services.Implementations.UserService>();
        services.AddScoped<Services.Interfaces.IStellarAccountService, Services.Implementations.StellarAccountService>();
        services.AddScoped<Services.Interfaces.ITransactionService, Services.Implementations.TransactionService>();
        services.AddScoped<Services.Interfaces.IExchangeRateService, Services.Implementations.ExchangeRateService>();

        // Add services to the container.
        services.AddControllers();

        // Configurar autenticación JWT para Cognito
        services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Leer configuración de Cognito desde appsettings.json
                var cognitoRegion = Configuration["Cognito:Region"];
                var cognitoUserPoolId = Configuration["Cognito:UserPoolId"];
                var cognitoClientId = Configuration["Cognito:ClientId"];

                options.Authority = $"https://cognito-idp.{cognitoRegion}.amazonaws.com/{cognitoUserPoolId}";
                options.Audience = cognitoClientId;

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        // Agregar autorización
        services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFilename));

            // Configurar autenticación JWT en Swagger
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Ingresa el token JWT obtenido de Cognito. Ejemplo: Bearer {tu_token}"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        // Agregar middleware de manejo de errores
        services.AddScoped<Services.Interfaces.IErrorHandlingMiddleware, Services.Implementations.ErrorHandlingMiddleware>();
    }

    public void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        // Agregar middleware de autenticación
        app.UseAuthentication();

        // Agregar middleware de manejo de errores
        app.UseErrorHandling();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}