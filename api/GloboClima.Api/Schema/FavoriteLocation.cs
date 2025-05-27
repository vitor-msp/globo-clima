using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Exceptions;

namespace GloboClima.Api.Schema;

[DynamoDBTable("favorite-locations")]
public class FavoriteLocation
{
    [DynamoDBHashKey("username")]
    public required string Username { get; init; }

    [DynamoDBRangeKey("id")]
    public Guid Id { get; init; } = Guid.NewGuid();

    private readonly double _lat;

    [DynamoDBProperty("lat")]
    public required double Lat
    {
        get => _lat;
        init
        {
            if (value < -90 || value > 90)
                throw new DomainException("Lat must be between -90 and 90.");
            _lat = value;
        }
    }

    private readonly double _lon;

    [DynamoDBProperty("lon")]
    public required double Lon
    {
        get => _lon;
        init
        {
            if (value < -180 || value > 180)
                throw new DomainException("Lon must be between -180 and 180.");
            _lon = value;
        }
    }
}