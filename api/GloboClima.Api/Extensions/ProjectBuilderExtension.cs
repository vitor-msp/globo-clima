using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Configuration;
using GloboClima.Api.Services.Contract;
using GloboClima.Api.Services.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace GloboClima.Api.Extensions;

public static class ProjectBuilderExtension
{
    public static void BuildProject(this IServiceCollection services, IConfiguration configuration)
    {
        var dbClient = configuration.GetDbClient();
        var dbContext = dbClient.GetDbContext();
        services.AddSingleton<IAmazonDynamoDB>(dbClient);
        services.AddSingleton<IDynamoDBContext>(dbContext);
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<ITextHasherService, TextHasherService>();
        services.AddSingleton<IHttpService, HttpService>();
        services.Configure<TokenConfiguration>(configuration.GetSection("Token"));
        services.Configure<RestCountriesApiConfiguration>(configuration.GetSection("RestCountriesApi"));
        ConfigureToken(services, configuration);
    }

    private static void ConfigureToken(IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration.GetSection("Token")["Key"]
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