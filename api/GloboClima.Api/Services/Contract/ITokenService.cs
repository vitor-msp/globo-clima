using GloboClima.Api.Schema;

namespace GloboClima.Api.Services.Contract;

public interface ITokenService
{
    public string GenerateJwt(User user);
}