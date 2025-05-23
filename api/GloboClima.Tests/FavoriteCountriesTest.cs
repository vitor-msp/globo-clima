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

    [Fact]
    public async Task ShouldDeleteFavoriteCountry()
    {
        var savedFavoriteCountry = new FavoriteCountry() { Cioc = "BRA" };
        await _context.SaveAsync(savedFavoriteCountry);
        var output = await _controller.DeleteFavoriteCountry(savedFavoriteCountry.Id);
        Assert.IsType<NoContentResult>(output);
        var favoriteCountry = await _context.LoadAsync<FavoriteCountry>(savedFavoriteCountry.Id);
        Assert.Null(favoriteCountry);
    }

    [Fact]
    public async Task ShouldNotDeleteInexistingFavoriteCountry()
    {
        var favoriteCountryId = Guid.NewGuid();
        var output = await _controller.DeleteFavoriteCountry(favoriteCountryId);
        Assert.IsType<NotFoundResult>(output);
        var favoriteCountry = await _context.LoadAsync<FavoriteCountry>(favoriteCountryId);
        Assert.Null(favoriteCountry);
    }

    [Fact]
    public async Task ShouldListFavoriteCountries()
    {
        var brazil = new FavoriteCountry() { Cioc = "BRA" };
        var argentina = new FavoriteCountry() { Cioc = "ARG" };
        await _context.SaveAsync(brazil);
        await _context.SaveAsync(argentina);
        var output = await _controller.ListFavoriteCountries();
        var outputResult = Assert.IsType<OkObjectResult>(output.Result);
        var outputContent = Assert.IsType<List<FavoriteCountry>>(outputResult.Value);
        Assert.Equal(2, outputContent.Count);
        var savedBrazil = outputContent.Find(country => country.Id == brazil.Id);
        Assert.NotNull(savedBrazil);
        Assert.Equal(brazil.Cioc, savedBrazil.Cioc);
        var savedArgentina = outputContent.Find(country => country.Id == argentina.Id);
        Assert.NotNull(savedArgentina);
        Assert.Equal(argentina.Cioc, savedArgentina.Cioc);
    }
}