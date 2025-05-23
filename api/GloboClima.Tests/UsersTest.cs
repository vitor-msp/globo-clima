namespace GloboClima.Tests;

public class UsersTest : BaseTest
{
    private readonly UsersController _controller;

    public UsersTest() : base()
    {
        _controller = MakeSut();
    }

    private UsersController MakeSut() => new(_context, _client);

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

    [Fact]
    public async Task ShouldNotCreateDuplicatedUser()
    {
        var existingUser = new User()
        {
            Name = "fulano de tal",
            Username = "fulano",
            Password = "fulano0.123@"
        };
        await _context.SaveAsync(existingUser);
        var input = new CreateUserInput()
        {
            Name = "fulano de tal",
            Username = "fulano",
            Password = "fulano0.123@",
            PasswordConfirmation = "fulano0.123@"
        };
        var output = await _controller.CreateUser(input);
        Assert.IsType<ConflictResult>(output);
        var user = await _context.LoadAsync<User>("fulano");
        Assert.NotNull(user);
        Assert.Equal(existingUser.Name, user.Name);
        Assert.Equal(existingUser.Username, user.Username);
        Assert.Equal(existingUser.Password, user.Password);
    }
}