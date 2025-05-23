using GloboClima.Api.Schema;
using GloboClima.Api.Services.Contract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using GloboClima.Api.Configuration;

namespace GloboClima.Api.Services.Implementation;

public class TokenService : ITokenService
{
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly TokenConfiguration _configuration;
    private readonly byte[] _key;

    public TokenService(IOptions<TokenConfiguration> options)
    {
        _configuration = options.Value;
        if (_configuration.Key is null)
            throw new Exception("Missing configure token key.");
        _key = Encoding.UTF8.GetBytes(_configuration.Key);
    }

    public string GenerateJwt(User user)
    {
        var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature);
        var descriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddSeconds(_configuration.AccessTokenExpiresInSeconds),
            Subject = GenerateClaims(user)
        };
        var token = _tokenHandler.CreateToken(descriptor);
        return _tokenHandler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Username));
        return ci;
    }
}