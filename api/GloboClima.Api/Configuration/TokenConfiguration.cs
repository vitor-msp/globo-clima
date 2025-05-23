namespace GloboClima.Api.Configuration;

public class TokenConfiguration
{
    public string? Key { get; set; }
    public long AccessTokenExpiresInSeconds { get; set; }
}