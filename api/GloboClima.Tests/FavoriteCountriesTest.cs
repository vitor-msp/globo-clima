namespace GloboClima.Tests;

public class FavoriteCountriesTest : BaseTest
{
    private readonly FavoriteCountriesController _controller;

    public FavoriteCountriesTest() : base()
    {
        _controller = MakeSut();
    }

    private FavoriteCountriesController MakeSut() => new(_context);

    protected override string GetTableName() => "favorite-countries";
    protected override string GetKeyName() => "Id";

    [Fact]
    public async Task ShouldCreateFavoriteCountry()
    {
        var input = new CreateFavoriteCountryInput()
        {
            Cioc = "BRA",
        };
        var output = await _controller.CreateFavoriteCountry(input);
        var outputResult = Assert.IsType<OkObjectResult>(output.Result);
        var outputContent = Assert.IsType<CreateFavoriteCountryOutput>(outputResult.Value);
        Assert.NotEqual(default, outputContent.FavoriteCountryId);
        var favoriteCountry = await _context.LoadAsync<FavoriteCountry>(outputContent.FavoriteCountryId);
        Assert.NotNull(favoriteCountry);
        Assert.Equal(input.Cioc, favoriteCountry.Cioc);
    }
}