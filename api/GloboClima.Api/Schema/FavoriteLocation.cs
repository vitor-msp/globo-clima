using Amazon.DynamoDBv2.DataModel;

namespace GloboClima.Api.Schema;

[DynamoDBTable("favorite-locations")]
public class FavoriteLocation
{
    [DynamoDBHashKey]
    public Guid Id { get; init; } = Guid.NewGuid();

    [DynamoDBProperty]
    public required double Lat { get; init; }

    [DynamoDBProperty]
    public required double Lon { get; init; }
}