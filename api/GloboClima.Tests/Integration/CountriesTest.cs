using Moq;

namespace GloboClima.Tests.Integration;

public class CountriesTest
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public CountriesTest()
    {
        _factory = new CustomWebApplicationFactory();
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task ShouldGetCountryDemographicInformationByCioc()
    {
        var json = await File.ReadAllTextAsync("Mocks/countries-response.json");
        _factory.HttpServiceMock
            .Setup(x => x.GetAsync(It.IsAny<HttpClient>(), It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        var cioc = "BRA";
        var response = await _httpClient.GetAsync($"/countries/{cioc}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<GetCountryOutput>();
        Assert.NotNull(output);
        Assert.Equal("BRA", output.Cioc);
        Assert.Equal("Brazil", output.CommonName);
        Assert.Equal("Federative Republic of Brazil", output.OfficialName);
        Assert.Equal(212559409, output.Population);
        Assert.Single(output.Languages);
        Assert.Equal("Portuguese", output.Languages[0]);
        Assert.Single(output.Currencies);
        Assert.Equal("R$", output.Currencies[0].Symbol);
        Assert.Equal("Brazilian real", output.Currencies[0].Name);
    }
}