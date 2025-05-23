using GloboClima.Api.Schema;

namespace GloboClima.Api.Inputs;

public class CreateFavoriteCountryInput
{
    public required string Cioc { get; init; }

    public FavoriteCountry GetFavoriteCountry() => new() { Cioc = Cioc };
}