using GloboClima.Api.Exceptions;
using GloboClima.Api.Schema;
using GloboClima.Api.Services.Contract;

namespace GloboClima.Api.Inputs;

public class CreateUserInput()
{
    public required string Name { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required string PasswordConfirmation { get; init; }

    public User GetUser(ITextHasherService textHasher) => new()
    {
        Name = Name,
        Username = Username,
        PasswordHash = textHasher.Hash(Password),
    };

    public void ValidatePassword()
    {
        if (!Password.Equals(PasswordConfirmation))
            throw new DomainException("Password and confirmation must be equal.");
    }
}