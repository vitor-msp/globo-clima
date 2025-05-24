using System.Security.Claims;
using GloboClima.Api.Schema;
using GloboClima.Api.Extensions;

namespace GloboClima.Api.Inputs;

public class CreateFavoriteLocationInput
{
    public required int Lat { get; init; }
    public required int Lon { get; init; }

    public FavoriteLocation GetFavoriteLocation(ClaimsPrincipal user)
        => new()
        {
            Lat = Lat,
            Lon = Lon,
            Username = user.GetName()
        };
}