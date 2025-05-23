using System.Text.RegularExpressions;
using GloboClima.Api.Configuration;
using GloboClima.Api.Services.Implementation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GloboClima.Tests;

public class UsersTest : BaseTest
{
    private readonly UsersController _controller;
    private readonly IConfiguration _configuration;

    public UsersTest() : base()
    {
        _configuration = LoadConfiguration();
        _controller = MakeSut();
    }

    private static IConfiguration LoadConfiguration()
        => new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .Build();

    private UsersController MakeSut()
    {
        var configuration = new TokenConfiguration();
        _configuration.GetSection("Token").Bind(configuration);
        var options = Options.Create(configuration);
        var tokenService = new TokenService(options);
        return new(_context, _client, tokenService);
    }

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

    [Fact]
    public async Task ShouldLoginUser()
    {
        var existingUser = new User()
        {
            Name = "fulano de tal",
            Username = "fulano",
            Password = "fulano0.123@"
        };
        await _context.SaveAsync(existingUser);
        var input = new LoginInput()
        {
            Username = "fulano",
            Password = "fulano0.123@",
        };
        var output = await _controller.Login(input);
        var outputResult = Assert.IsType<OkObjectResult>(output.Result);
        var outputContent = Assert.IsType<LoginOutput>(outputResult.Value);
        var jwtPattern = @"^[^\.\s]+\.[^\.\s]+\.[^\.\s]+$";
        Assert.True(Regex.Match(outputContent.AccessToken, jwtPattern).Success);
    }

    [Fact]
    public async Task ShouldNotLoginInexistingUser()
    {
        var input = new LoginInput()
        {
            Username = "fulano",
            Password = "fulano0.123@",
        };
        var output = await _controller.Login(input);
        Assert.IsType<UnprocessableEntityResult>(output.Result);
    }

    [Fact]
    public async Task ShouldNotLoginUserWithInvalidPassword()
    {
        var existingUser = new User()
        {
            Name = "fulano de tal",
            Username = "fulano",
            Password = "fulano0.123@"
        };
        await _context.SaveAsync(existingUser);
        var input = new LoginInput()
        {
            Username = "fulano",
            Password = "incorrect-password",
        };
        var output = await _controller.Login(input);
        Assert.IsType<UnprocessableEntityResult>(output.Result);
    }
}