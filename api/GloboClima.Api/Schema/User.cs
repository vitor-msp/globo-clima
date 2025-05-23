using Amazon.DynamoDBv2.DataModel;

namespace GloboClima.Api.Schema;

[DynamoDBTable("users")]
public class User
{
    [DynamoDBHashKey]
    public required string Username { get; init; }

    [DynamoDBProperty]
    public required string Name { get; init; }

    [DynamoDBProperty]
    public required string Password { get; init; }
}