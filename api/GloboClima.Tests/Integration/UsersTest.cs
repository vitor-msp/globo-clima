namespace GloboClima.Tests.Integration;

public class UsersTest : BaseTest
{
    private readonly TextHasherService _textHasher = new();
    protected override string GetTableName() => "users";
    protected override string GetPartitionKey() => "Username";
    protected override string? GetSortKey() => null;

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
        var response = await _httpClient.PostAsJsonAsync("/users", input);
        response.EnsureSuccessStatusCode();
        var user = await _dbContext.LoadAsync<User>("fulano");
        Assert.NotNull(user);
        Assert.Equal(input.Name, user.Name);
        Assert.Equal(input.Username, user.Username);
        Assert.True(_textHasher.Verify(user.PasswordHash, input.Password));
    }

    [Fact]
    public async Task ShouldNotCreateDuplicatedUser()
    {
        var existingUser = await CreateUser();
        var input = new CreateUserInput()
        {
            Name = "fulano de tal",
            Username = "fulano",
            Password = "fulano0.123@",
            PasswordConfirmation = "fulano0.123@"
        };
        var response = await _httpClient.PostAsJsonAsync("/users", input);
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var user = await _dbContext.LoadAsync<User>("fulano");
        Assert.NotNull(user);
        Assert.Equal(existingUser.Name, user.Name);
        Assert.Equal(existingUser.Username, user.Username);
        Assert.Equal(existingUser.PasswordHash, user.PasswordHash);
    }

    [Fact]
    public async Task ShouldNotCreateUserWithInvalidPasswordConfirmation()
    {
        var input = new CreateUserInput()
        {
            Name = "fulano de tal",
            Username = "fulano",
            Password = "fulanO.123@",
            PasswordConfirmation = "another-password"
        };
        var response = await _httpClient.PostAsJsonAsync("/users", input);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var user = await _dbContext.LoadAsync<User>("fulano");
        Assert.Null(user);
    }

    [Fact]
    public async Task ShouldLoginUser()
    {
        await CreateUser();
        var input = new LoginInput()
        {
            Username = "fulano",
            Password = "fulano0.123@",
        };
        var response = await _httpClient.PostAsJsonAsync("/login", input);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<LoginOutput>();
        Assert.NotNull(output);
        var jwtPattern = @"^[^\.\s]+\.[^\.\s]+\.[^\.\s]+$";
        Assert.True(Regex.Match(output.AccessToken, jwtPattern).Success);
        var claims = ValidateToken(output.AccessToken);
        var username = claims.FindFirstValue(ClaimTypes.Name);
        Assert.Equal("fulano", username);
    }

    [Fact]
    public async Task ShouldNotLoginInexistingUser()
    {
        var input = new LoginInput()
        {
            Username = "fulano",
            Password = "fulano0.123@",
        };
        var response = await _httpClient.PostAsJsonAsync("/login", input);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ShouldNotLoginUserWithInvalidPassword()
    {
        await CreateUser();
        var input = new LoginInput()
        {
            Username = "fulano",
            Password = "incorrect-password",
        };
        var response = await _httpClient.PostAsJsonAsync("/login", input);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    public async Task<User> CreateUser()
    {
        var existingUser = new User()
        {
            Name = "fulano de tal",
            Username = "fulano",
            PasswordHash = _textHasher.Hash("fulano0.123@")
        };
        await _dbContext.SaveAsync(existingUser);
        return existingUser;
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = _configuration.GetSection("Token")["Key"]
            ?? throw new Exception("Missing configure token key.");
        return tokenHandler.ValidateToken(token, new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        }, out _);
    }
}