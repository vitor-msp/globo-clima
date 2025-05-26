namespace GloboClima.Tests.Integration;

public class FavoriteLocationsTest : BaseTest
{
    private readonly TokenService _tokenService;
    private readonly TextHasherService _textHasher = new();
    protected override string GetTableName() => "favorite-locations";
    protected override string GetPartitionKey() => "username";
    protected override string? GetSortKey() => "id";

    public FavoriteLocationsTest()
    {
        var configuration = new TokenConfiguration();
        _configuration.GetSection("Token").Bind(configuration);
        var options = Options.Create(configuration);
        _tokenService = new TokenService(options);
    }

    [Fact]
    public async Task ShouldNotCreateFavoriteLocation_UserIsNotAutenticated()
    {
        var input = new CreateFavoriteLocationInput() { Lat = 1, Lon = -1 };
        var response = await _httpClient.PostAsJsonAsync("/favorite-locations", input);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ShouldCreateFavoriteLocation()
    {
        var input = new CreateFavoriteLocationInput() { Lat = 1, Lon = -1 };
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.PostAsJsonAsync("favorite-locations", input);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<CreateFavoriteLocationOutput>();
        Assert.NotNull(output);
        Assert.NotEqual(default, output.FavoriteLocationId);
        var favoriteLocation = await _dbContext.LoadAsync<FavoriteLocation>("fulano", output.FavoriteLocationId);
        Assert.NotNull(favoriteLocation);
        Assert.Equal(input.Lat, favoriteLocation.Lat);
        Assert.Equal(input.Lon, favoriteLocation.Lon);
        Assert.Equal("fulano", favoriteLocation.Username);
    }

    [Fact]
    public async Task ShouldNotDeleteFavoriteLocation_UserIsNotAutenticated()
    {
        var savedFavoriteLocation = new FavoriteLocation() { Lat = 1, Lon = -1, Username = "fulano" };
        await _dbContext.SaveAsync(savedFavoriteLocation);
        var response = await _httpClient.DeleteAsync($"/favorite-countries/{savedFavoriteLocation.Id}");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ShouldDeleteFavoriteLocation()
    {
        var savedFavoriteLocation = new FavoriteLocation() { Lat = 1, Lon = -1, Username = "fulano" };
        var othersSavedFavoriteLocation = new FavoriteLocation() { Lat = 1, Lon = -1, Username = "ciclano" };
        await _dbContext.SaveAsync(savedFavoriteLocation);
        await _dbContext.SaveAsync(othersSavedFavoriteLocation);
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.DeleteAsync($"favorite-locations/{savedFavoriteLocation.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var favoriteLocation = await _dbContext.LoadAsync<FavoriteLocation>("fulano", savedFavoriteLocation.Id);
        Assert.Null(favoriteLocation);
        var othersFavoriteLocation = await _dbContext.LoadAsync<FavoriteLocation>("ciclano", othersSavedFavoriteLocation.Id);
        Assert.NotNull(othersFavoriteLocation);
    }

    [Fact]
    public async Task ShouldNotDeleteInexistingFavoriteLocation()
    {
        var othersSavedFavoriteCountry = new FavoriteLocation() { Lat = 1, Lon = -1, Username = "ciclano" };
        await _dbContext.SaveAsync(othersSavedFavoriteCountry);
        var favoriteLocationId = Guid.NewGuid();
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.DeleteAsync($"/favorite-locations/{favoriteLocationId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var favoriteLocation = await _dbContext.LoadAsync<FavoriteLocation>("fulano", favoriteLocationId);
        Assert.Null(favoriteLocation);
        var othersFavoriteLocation = await _dbContext.LoadAsync<FavoriteLocation>("ciclano", othersSavedFavoriteCountry.Id);
        Assert.NotNull(othersFavoriteLocation);
    }

    [Fact]
    public async Task ShouldNotDeleteOthersFavoriteLocation()
    {
        var othersSavedFavoriteLocation = new FavoriteLocation() { Lon = 1, Lat = 0, Username = "ciclano" };
        await _dbContext.SaveAsync(othersSavedFavoriteLocation);
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.DeleteAsync($"/favorite-locations/{othersSavedFavoriteLocation.Id}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var favoriteLocation = await _dbContext.LoadAsync<FavoriteLocation>("ciclano", othersSavedFavoriteLocation.Id);
        Assert.NotNull(favoriteLocation);
    }

    [Fact]
    public async Task ShouldNotListFavoriteLocation_UserIsNotAutenticated()
    {
        var savedFavoriteLocation = new FavoriteLocation() { Lat = 1, Lon = -1, Username = "fulano" };
        await _dbContext.SaveAsync(savedFavoriteLocation);
        var response = await _httpClient.GetAsync("/favorite-locations");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ShouldListFavoriteLocations()
    {
        var location1 = new FavoriteLocation() { Lat = 1, Lon = -1, Username = "fulano" };
        var location2 = new FavoriteLocation() { Lat = -4, Lon = 6, Username = "fulano" };
        var othersLocation1 = new FavoriteLocation() { Lat = 1, Lon = -1, Username = "ciclano" };
        var othersLocation2 = new FavoriteLocation() { Lat = -4, Lon = 6, Username = "ciclano" };
        await _dbContext.SaveAsync(location1);
        await _dbContext.SaveAsync(location2);
        await _dbContext.SaveAsync(othersLocation1);
        await _dbContext.SaveAsync(othersLocation2);
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.GetAsync("/favorite-locations");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<List<FavoriteLocation>>();
        Assert.NotNull(output);
        Assert.Equal(2, output.Count);
        var savedLocation1 = output.Find(location => location.Id == location1.Id);
        Assert.NotNull(savedLocation1);
        Assert.Equal(location1.Lat, savedLocation1.Lat);
        Assert.Equal(location1.Lon, savedLocation1.Lon);
        Assert.Equal(location1.Username, savedLocation1.Username);
        var savedLocation2 = output.Find(location => location.Id == location2.Id);
        Assert.NotNull(savedLocation2);
        Assert.Equal(location2.Lat, savedLocation2.Lat);
        Assert.Equal(location2.Lon, savedLocation2.Lon);
        Assert.Equal(location2.Username, savedLocation2.Username);
    }

    private void SetAccessTokenToUsername(string username)
    {
        var accessToken = _tokenService.GenerateJwt(GetUser(username));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    private User GetUser(string username)
        => new()
        {
            Name = "fulano de tal",
            PasswordHash = _textHasher.Hash("Fulano.12@3"),
            Username = username
        };
}