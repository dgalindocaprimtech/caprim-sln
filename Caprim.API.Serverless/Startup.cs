using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Caprim.API.Services;
using Caprim.API.Services.Interfaces;
using Caprim.API.Services.Implementations;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Agregar autenticación JWT con Cognito
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://cognito-idp.us-east-1.amazonaws.com/us-east-1_lvRYtEIyu";
                options.Audience = "20hlo41tc8dgbfshpil3ps3lc2";
            });

        services.AddAuthorization();

        // Registrar servicios (ajusta según tus implementaciones)
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStellarService, StellarService>();
        services.AddScoped<IStellarAccountService, StellarAccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
            });
        });
    }
}