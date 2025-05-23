namespace GloboClima.Tests;

public class FavoriteCountriesTest : BaseTest
{
    protected override string GetTableName() => "favorite-countries";
    protected override string GetKeyName() => "Id";

    [Fact]
    public async Task ShouldCreateFavoriteCountry()
    {
        var input = new CreateFavoriteCountryInput() { Cioc = "BRA" };
        var response = await _httpClient.PostAsJsonAsync("/favorite-countries", input);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<CreateFavoriteCountryOutput>();
        Assert.NotNull(output);
        Assert.NotEqual(default, output.FavoriteCountryId);
        var favoriteCountry = await _dbContext.LoadAsync<FavoriteCountry>(output.FavoriteCountryId);
        Assert.NotNull(favoriteCountry);
        Assert.Equal(input.Cioc, favoriteCountry.Cioc);
    }

    [Fact]
    public async Task ShouldDeleteFavoriteCountry()
    {
        var savedFavoriteCountry = new FavoriteCountry() { Cioc = "BRA" };
        await _dbContext.SaveAsync(savedFavoriteCountry);
        var response = await _httpClient.DeleteAsync($"/favorite-countries/{savedFavoriteCountry.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var favoriteCountry = await _dbContext.LoadAsync<FavoriteCountry>(savedFavoriteCountry.Id);
        Assert.Null(favoriteCountry);
    }

    [Fact]
    public async Task ShouldNotDeleteInexistingFavoriteCountry()
    {
        var favoriteCountryId = Guid.NewGuid();
        var response = await _httpClient.DeleteAsync($"/favorite-countries/{favoriteCountryId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var favoriteCountry = await _dbContext.LoadAsync<FavoriteCountry>(favoriteCountryId);
        Assert.Null(favoriteCountry);
    }

    [Fact]
    public async Task ShouldListFavoriteCountries()
    {
        var brazil = new FavoriteCountry() { Cioc = "BRA" };
        var argentina = new FavoriteCountry() { Cioc = "ARG" };
        await _dbContext.SaveAsync(brazil);
        await _dbContext.SaveAsync(argentina);
        var response = await _httpClient.GetAsync("/favorite-countries");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<List<FavoriteCountry>>();
        Assert.NotNull(output);
        Assert.Equal(2, output.Count);
        var savedBrazil = output.Find(country => country.Id == brazil.Id);
        Assert.NotNull(savedBrazil);
        Assert.Equal(brazil.Cioc, savedBrazil.Cioc);
        var savedArgentina = output.Find(country => country.Id == argentina.Id);
        Assert.NotNull(savedArgentina);
        Assert.Equal(argentina.Cioc, savedArgentina.Cioc);
    }
}