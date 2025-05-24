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
        var serviceUrl = dynamoDbConfig["ServiceURL"] ?? throw new Exception("Missing configure dynamodb service url.");
        var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
        var config = new AmazonDynamoDBConfig { ServiceURL = serviceUrl };
        return new(credentials, config);
    }

#pragma warning disable CS0618 // Type or member is obsolete
    public static DynamoDBContext GetDbContext(this IAmazonDynamoDB dbClient) => new(dbClient);
#pragma warning restore CS0618 // Type or member is obsolete
}