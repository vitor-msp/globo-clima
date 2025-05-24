using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GloboClima.Tests.Integration;

public class CustomWebApplicationFactory(IConfiguration configuration, IAmazonDynamoDB dbClient, IDynamoDBContext dbContext)
    : WebApplicationFactory<Program>
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IAmazonDynamoDB _dbClient = dbClient;
    private readonly IDynamoDBContext _dbContext = dbContext;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ConfigureToken(services);
            services.Configure<TokenConfiguration>(_configuration.GetSection("Token"));
            services.AddControllers();
            services.AddSingleton(_dbClient);
            services.AddSingleton(_dbContext);
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<ITextHasherService, TextHasherService>();
        });

        builder.Configure(app =>
        {
            app.UseExceptionMiddleware();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });
    }

    private void ConfigureToken(IServiceCollection services)
    {
        var key = _configuration.GetSection("Token")["Key"]
            ?? throw new Exception("Missing configure token key.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
        });
        services.AddAuthorization();
    }
}