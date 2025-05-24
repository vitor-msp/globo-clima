using System.Security.Claims;
using GloboClima.Api.Extensions;
using GloboClima.Api.Schema;

namespace GloboClima.Api.Inputs;

public class CreateFavoriteCountryInput
{
    public required string Cioc { get; init; }

    public FavoriteCountry GetFavoriteCountry(ClaimsPrincipal user) => new()
    {
        Cioc = Cioc,
        Username = user.GetName()
    };
}