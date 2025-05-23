namespace GloboClima.Tests;

public class UsersTest : BaseTest
{
    private readonly UsersController _controller;

    public UsersTest() : base()
    {
        _controller = MakeSut();
    }

    private UsersController MakeSut() => new(_context);

    [Fact]
    public async Task ShouldCreateUser()
    {
        var input = new CreateUserInput()
        {
            Name = "fulano de tal",
            Username = "fulano",
            Password = "fulanO.123@",
            PasswordConfirmation = "fulanO.123@"
        };
        var output = await _controller.CreateUser(input);
        Assert.IsType<CreatedResult>(output);
        var user = await _context.LoadAsync<User>("fulano");
        Assert.NotNull(user);
        Assert.Equal(input.Name, user.Name);
        Assert.Equal(input.Username, user.Username);
        Assert.Equal(input.Password, user.Password);
    }
}