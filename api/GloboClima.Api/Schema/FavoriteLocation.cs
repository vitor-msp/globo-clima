using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Exceptions;

namespace GloboClima.Api.Schema;

[DynamoDBTable("favorite-locations")]
public class FavoriteLocation
{
    [DynamoDBHashKey]
    public Guid Id { get; init; } = Guid.NewGuid();

    private readonly int _lat;

    [DynamoDBProperty]
    public required int Lat
    {
        get => _lat;
        init
        {
            if (value < -90 || value > 90)
                throw new DomainException("Lat must be between -90 and 90.");
            _lat = value;
        }
    }

    private readonly int _lon;

    [DynamoDBProperty]
    public required int Lon
    {
        get => _lon;
        init
        {
            if (value < -180 || value > 180)
                throw new DomainException("Lon must be between -180 and 180.");
            _lon = value;
        }
    }

    [DynamoDBProperty]
    public required string Username { get; init; }
}