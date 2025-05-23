using GloboClima.Api.Schema;

namespace GloboClima.Api.Inputs;

public class CreateFavoriteLocationInput
{
    public required double Lat { get; init; }
    public required double Lon { get; init; }

    public FavoriteLocation GetFavoriteLocation()
        => new() { Lat = Lat, Lon = Lon };
}