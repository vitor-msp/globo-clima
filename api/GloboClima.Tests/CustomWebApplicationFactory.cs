using GloboClima.Api.Services.Contract;

namespace GloboClima.Tests;

public class CustomWebApplicationFactory(IAmazonDynamoDB dbClient, IDynamoDBContext dbContext)
    : WebApplicationFactory<Program>
{
    private readonly IAmazonDynamoDB _dbClient = dbClient;
    private readonly IDynamoDBContext _dbContext = dbContext;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var configuration = LoadConfiguration();
        builder.ConfigureServices(services =>
        {
            services.Configure<TokenConfiguration>(configuration.GetSection("Token"));
            services.AddControllers();
            services.AddSingleton(_dbClient);
            services.AddSingleton(_dbContext);
            services.AddSingleton<ITokenService, TokenService>();
        });

        builder.Configure(app =>
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });
    }

    private static IConfiguration LoadConfiguration()
        => new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .Build();
}