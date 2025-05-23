namespace GloboClima.Tests;

public class FavoriteLocationsTest : BaseTest
{
    private readonly FavoriteLocationsController _controller;

    public FavoriteLocationsTest() : base()
    {
        _controller = MakeSut();
    }

    private FavoriteLocationsController MakeSut() => new(_context);

    protected override string GetTableName() => "favorite-locations";
    protected override string GetKeyName() => "Id";

    [Fact]
    public async Task ShouldCreateFavoriteLocation()
    {
        var input = new CreateFavoriteLocationInput() { Lat = 1, Lon = -1.1 };
        var output = await _controller.CreateFavoriteLocation(input);
        var outputResult = Assert.IsType<OkObjectResult>(output.Result);
        var outputContent = Assert.IsType<CreateFavoriteLocationOutput>(outputResult.Value);
        Assert.NotEqual(default, outputContent.FavoriteLocationId);
        var favoriteLocation = await _context.LoadAsync<FavoriteLocation>(outputContent.FavoriteLocationId);
        Assert.NotNull(favoriteLocation);
        Assert.Equal(input.Lat, favoriteLocation.Lat);
        Assert.Equal(input.Lon, favoriteLocation.Lon);
    }

    [Fact]
    public async Task ShouldDeleteFavoriteLocation()
    {
        var savedFavoriteLocation = new FavoriteLocation() { Lat = 1, Lon = -1.1 };
        await _context.SaveAsync(savedFavoriteLocation);
        var output = await _controller.DeleteFavoriteLocation(savedFavoriteLocation.Id);
        Assert.IsType<NoContentResult>(output);
        var favoriteLocation = await _context.LoadAsync<FavoriteLocation>(savedFavoriteLocation.Id);
        Assert.Null(favoriteLocation);
    }

    [Fact]
    public async Task ShouldNotDeleteInexistingFavoriteLocation()
    {
        var favoriteLocationId = Guid.NewGuid();
        var output = await _controller.DeleteFavoriteLocation(favoriteLocationId);
        Assert.IsType<NotFoundResult>(output);
        var favoriteLocation = await _context.LoadAsync<FavoriteLocation>(favoriteLocationId);
        Assert.Null(favoriteLocation);
    }

    [Fact]
    public async Task ShouldListFavoriteLocations()
    {
        var location1 = new FavoriteLocation() { Lat = 1, Lon = -1.1 };
        var location2 = new FavoriteLocation() { Lat = -4, Lon = 0.6 };
        await _context.SaveAsync(location1);
        await _context.SaveAsync(location2);
        var output = await _controller.ListFavoriteLocations();
        var outputResult = Assert.IsType<OkObjectResult>(output.Result);
        var outputContent = Assert.IsType<List<FavoriteLocation>>(outputResult.Value);
        Assert.Equal(2, outputContent.Count);
        var savedLocation1 = outputContent.Find(location => location.Id == location1.Id);
        Assert.NotNull(savedLocation1);
        Assert.Equal(location1.Lat, savedLocation1.Lat);
        Assert.Equal(location1.Lon, savedLocation1.Lon);
        var savedLocation2 = outputContent.Find(location => location.Id == location2.Id);
        Assert.NotNull(savedLocation2);
        Assert.Equal(location2.Lat, savedLocation2.Lat);
        Assert.Equal(location2.Lon, savedLocation2.Lon);
    }
}