namespace GloboClima.Tests.Integration;

public class FavoriteLocationsTest : BaseTest
{
    protected override string GetTableName() => "favorite-locations";
    protected override string GetKeyName() => "Id";

    [Fact]
    public async Task ShouldCreateFavoriteLocation()
    {
        var input = new CreateFavoriteLocationInput() { Lat = 1, Lon = -1.1 };
        var response = await _httpClient.PostAsJsonAsync("favorite-locations", input);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<CreateFavoriteLocationOutput>();
        Assert.NotNull(output);
        Assert.NotEqual(default, output.FavoriteLocationId);
        var favoriteLocation = await _dbContext.LoadAsync<FavoriteLocation>(output.FavoriteLocationId);
        Assert.NotNull(favoriteLocation);
        Assert.Equal(input.Lat, favoriteLocation.Lat);
        Assert.Equal(input.Lon, favoriteLocation.Lon);
    }

    [Fact]
    public async Task ShouldDeleteFavoriteLocation()
    {
        var savedFavoriteLocation = new FavoriteLocation() { Lat = 1, Lon = -1.1 };
        await _dbContext.SaveAsync(savedFavoriteLocation);
        var response = await _httpClient.DeleteAsync($"favorite-locations/{savedFavoriteLocation.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var favoriteLocation = await _dbContext.LoadAsync<FavoriteLocation>(savedFavoriteLocation.Id);
        Assert.Null(favoriteLocation);
    }

    [Fact]
    public async Task ShouldNotDeleteInexistingFavoriteLocation()
    {
        var favoriteLocationId = Guid.NewGuid();
        var response = await _httpClient.DeleteAsync($"/favorite-locations/{favoriteLocationId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var favoriteLocation = await _dbContext.LoadAsync<FavoriteLocation>(favoriteLocationId);
        Assert.Null(favoriteLocation);
    }

    [Fact]
    public async Task ShouldListFavoriteLocations()
    {
        var location1 = new FavoriteLocation() { Lat = 1, Lon = -1.1 };
        var location2 = new FavoriteLocation() { Lat = -4, Lon = 0.6 };
        await _dbContext.SaveAsync(location1);
        await _dbContext.SaveAsync(location2);
        var response = await _httpClient.GetAsync("/favorite-locations");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<List<FavoriteLocation>>();
        Assert.NotNull(output);
        Assert.Equal(2, output.Count);
        var savedLocation1 = output.Find(location => location.Id == location1.Id);
        Assert.NotNull(savedLocation1);
        Assert.Equal(location1.Lat, savedLocation1.Lat);
        Assert.Equal(location1.Lon, savedLocation1.Lon);
        var savedLocation2 = output.Find(location => location.Id == location2.Id);
        Assert.NotNull(savedLocation2);
        Assert.Equal(location2.Lat, savedLocation2.Lat);
        Assert.Equal(location2.Lon, savedLocation2.Lon);
    }
}