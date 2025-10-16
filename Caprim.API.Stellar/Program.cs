using System.Reflection;
using Caprim.API.Services.Interfaces;
using Caprim.API.Services.Implementations;
using Caprim.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Amazon.Lambda.AspNetCoreServer.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configuración de StellarSettings
builder.Services.Configure<StellarSettings>(
    builder.Configuration.GetSection("Stellar"));

// Add DbContext
builder.Services.AddDbContext<StellarDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios
builder.Services.AddScoped<IStellarService, StellarService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStellarAccountService, StellarAccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

// Add services to the container.
builder.Services.AddControllers();

// Configurar autenticación JWT para Cognito
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Leer configuración de Cognito desde appsettings.json
        var cognitoRegion = builder.Configuration["Cognito:Region"];
        var cognitoUserPoolId = builder.Configuration["Cognito:UserPoolId"];
        var cognitoClientId = builder.Configuration["Cognito:ClientId"];

        options.Authority = $"https://cognito-idp.{cognitoRegion}.amazonaws.com/{cognitoUserPoolId}";
        options.Audience = cognitoClientId;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

// Agregar autorización
builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    // Configurar autenticación JWT en Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT obtenido de Cognito. Ejemplo: Bearer {tu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Agregar middleware de autenticación
app.UseAuthentication();

// Agregar middleware de manejo de errores
app.UseErrorHandling();

app.UseAuthorization();

app.MapControllers();

app.Run();