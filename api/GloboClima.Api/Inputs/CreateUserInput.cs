using GloboClima.Api.Schema;

namespace GloboClima.Api.Inputs;

public class CreateUserInput
{
    public required string Name { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string PasswordConfirmation { get; init; }

    public User GetUser() => new()
    {
        Name = Name,
        Username = Username,
        Password = Password,
    };
}