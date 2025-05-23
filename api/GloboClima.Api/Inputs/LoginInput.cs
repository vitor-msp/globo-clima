namespace GloboClima.Api.Inputs;

public class LoginInput
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}