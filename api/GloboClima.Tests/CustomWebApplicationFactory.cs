using GloboClima.Api.Extensions;
using GloboClima.Api.Services.Contract;

namespace GloboClima.Tests;

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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });
    }
}