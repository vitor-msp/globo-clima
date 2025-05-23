using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Configuration;
using GloboClima.Api.Services.Contract;
using GloboClima.Api.Services.Implementation;

namespace GloboClima.Api.Extensions;

public static class ProjectBuilderExtension
{
    public static void BuildProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TokenConfiguration>(configuration.GetSection("Token"));
        var dbConnectionString = configuration.GetConnectionString("DynamoDBConnection")
            ?? throw new Exception("Missing configure db connection string.");
        var dbClient = GetDbClient(dbConnectionString);
        services.AddSingleton(dbClient);
        services.AddSingleton(GetDbContext(dbClient));
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<ITextHasherService, TextHasherService>();
    }

    private static IAmazonDynamoDB GetDbClient(string connectionString)
        => new AmazonDynamoDBClient(new AmazonDynamoDBConfig { ServiceURL = connectionString });

#pragma warning disable CS0618 // Type or member is obsolete
    private static IDynamoDBContext GetDbContext(IAmazonDynamoDB dbClient)
        => new DynamoDBContext(dbClient);
#pragma warning restore CS0618 // Type or member is obsolete
}