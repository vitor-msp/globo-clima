using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace GloboClima.Tests.Integration;

public class FavoriteCountriesTest : BaseTest
{
    private readonly TokenService _tokenService;
    private readonly TextHasherService _textHasher = new();
    protected override string GetTableName() => "favorite-countries";
    protected override string GetKeyName() => "Id";

    public FavoriteCountriesTest()
    {
        var configuration = new TokenConfiguration();
        _configuration.GetSection("Token").Bind(configuration);
        var options = Options.Create(configuration);
        _tokenService = new TokenService(options);
    }

    [Fact]
    public async Task ShouldCreateFavoriteCountry()
    {
        var input = new CreateFavoriteCountryInput() { Cioc = "BRA" };
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.PostAsJsonAsync("/favorite-countries", input);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<CreateFavoriteCountryOutput>();
        Assert.NotNull(output);
        Assert.NotEqual(default, output.FavoriteCountryId);
        var favoriteCountry = await _dbContext.LoadAsync<FavoriteCountry>(output.FavoriteCountryId);
        Assert.NotNull(favoriteCountry);
        Assert.Equal(input.Cioc, favoriteCountry.Cioc);
        Assert.Equal("fulano", favoriteCountry.Username);
    }

    [Fact]
    public async Task ShouldDeleteFavoriteCountry()
    {
        var savedFavoriteCountry = new FavoriteCountry() { Cioc = "BRA", Username = "fulano" };
        var othersSavedFavoriteCountry = new FavoriteCountry() { Cioc = "BRA", Username = "ciclano" };
        await _dbContext.SaveAsync(savedFavoriteCountry);
        await _dbContext.SaveAsync(othersSavedFavoriteCountry);
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.DeleteAsync($"/favorite-countries/{savedFavoriteCountry.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var favoriteCountry = await _dbContext.LoadAsync<FavoriteCountry>(savedFavoriteCountry.Id);
        Assert.Null(favoriteCountry);
        var othersFavoriteCountry = await _dbContext.LoadAsync<FavoriteCountry>(othersSavedFavoriteCountry.Id);
        Assert.NotNull(othersFavoriteCountry);
    }

    [Fact]
    public async Task ShouldNotDeleteInexistingFavoriteCountry()
    {
        var othersSavedFavoriteCountry = new FavoriteCountry() { Cioc = "BRA", Username = "ciclano" };
        await _dbContext.SaveAsync(othersSavedFavoriteCountry);
        var favoriteCountryId = Guid.NewGuid();
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.DeleteAsync($"/favorite-countries/{favoriteCountryId}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var favoriteCountry = await _dbContext.LoadAsync<FavoriteCountry>(favoriteCountryId);
        Assert.Null(favoriteCountry);
        var othersFavoriteCountry = await _dbContext.LoadAsync<FavoriteCountry>(othersSavedFavoriteCountry.Id);
        Assert.NotNull(othersFavoriteCountry);
    }

    [Fact]
    public async Task ShouldNotDeleteOthersFavoriteCountry()
    {
        var othersSavedFavoriteCountry = new FavoriteCountry() { Cioc = "BRA", Username = "ciclano" };
        await _dbContext.SaveAsync(othersSavedFavoriteCountry);
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.DeleteAsync($"/favorite-countries/{othersSavedFavoriteCountry.Id}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var favoriteCountry = await _dbContext.LoadAsync<FavoriteCountry>(othersSavedFavoriteCountry.Id);
        Assert.NotNull(favoriteCountry);
    }

    [Fact]
    public async Task ShouldListFavoriteCountries()
    {
        var brazil = new FavoriteCountry() { Cioc = "BRA", Username = "fulano" };
        var argentina = new FavoriteCountry() { Cioc = "ARG", Username = "fulano" };
        var othersBrazil = new FavoriteCountry() { Cioc = "BRA", Username = "ciclano" };
        var othersArgentina = new FavoriteCountry() { Cioc = "ARG", Username = "ciclano" };
        await _dbContext.SaveAsync(brazil);
        await _dbContext.SaveAsync(argentina);
        await _dbContext.SaveAsync(othersBrazil);
        await _dbContext.SaveAsync(othersArgentina);
        SetAccessTokenToUsername("fulano");
        var response = await _httpClient.GetAsync("/favorite-countries");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<List<FavoriteCountry>>();
        Assert.NotNull(output);
        Assert.Equal(2, output.Count);
        var savedBrazil = output.Find(country => country.Id == brazil.Id);
        Assert.NotNull(savedBrazil);
        Assert.Equal(brazil.Cioc, savedBrazil.Cioc);
        Assert.Equal("fulano", savedBrazil.Username);
        var savedArgentina = output.Find(country => country.Id == argentina.Id);
        Assert.NotNull(savedArgentina);
        Assert.Equal(argentina.Cioc, savedArgentina.Cioc);
        Assert.Equal("fulano", savedArgentina.Username);
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