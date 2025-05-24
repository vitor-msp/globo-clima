using System.Security.Claims;
using GloboClima.Api.Exceptions;

namespace GloboClima.Api.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string GetName(this ClaimsPrincipal claims)
        => claims.Identity?.Name ?? throw new UnauthorizeException("User is not authenticated.");
}