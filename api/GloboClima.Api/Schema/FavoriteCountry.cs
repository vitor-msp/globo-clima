using Amazon.DynamoDBv2.DataModel;

namespace GloboClima.Api.Schema;

[DynamoDBTable("favorite-countries")]
public class FavoriteCountry
{
    [DynamoDBHashKey]
    public Guid Id { get; init; } = Guid.NewGuid();

    [DynamoDBProperty]
    public required string Cioc { get; init; }
}