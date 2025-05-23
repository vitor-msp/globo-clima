using GloboClima.Api.Schema;

namespace GloboClima.Api.Inputs;

public class CreateFavoriteLocationInput
{
    public required int Lat { get; init; }
    public required int Lon { get; init; }

    public FavoriteLocation GetFavoriteLocation()
        => new() { Lat = Lat, Lon = Lon };
}