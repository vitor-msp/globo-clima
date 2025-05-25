using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Services.Contract;

namespace GloboClima.Api.Schema;

[DynamoDBTable("users")]
public class User
{
    [DynamoDBHashKey("username")]
    public required string Username { get; init; }

    [DynamoDBProperty("name")]
    public required string Name { get; init; }

    [DynamoDBProperty("password_hash")]
    public required string PasswordHash { get; init; }

    public bool PasswordIsCorrect(ITextHasherService textHasher, string password)
        => textHasher.Verify(PasswordHash, password);
}