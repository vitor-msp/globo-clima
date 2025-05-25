using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;

namespace GloboClima.Api.Extensions;

public static class DynamoDbExtension
{
    public static AmazonDynamoDBClient GetDbClient(this IConfiguration configuration)
    {
        var dynamoDbConfig = configuration.GetSection("DynamoDB");
        var accessKeyId = dynamoDbConfig["AccessKeyId"] ?? throw new Exception("Missing configure aws access key id.");
        var secretAccessKey = dynamoDbConfig["SecretAccessKey"] ?? throw new Exception("Missing configure aws secret access key.");
        var region = dynamoDbConfig["Region"];
        var serviceUrl = dynamoDbConfig["ServiceURL"];
        if (region is null && serviceUrl is null)
            throw new Exception("Missing configure aws region or dynamodb service url.");
        var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
        var config = region is null
            ? new AmazonDynamoDBConfig { ServiceURL = serviceUrl }
            : new AmazonDynamoDBConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(region) };
        return new(credentials, config);
    }

#pragma warning disable CS0618 // Type or member is obsolete
    public static DynamoDBContext GetDbContext(this IAmazonDynamoDB dbClient) => new(dbClient);
#pragma warning restore CS0618 // Type or member is obsolete
}