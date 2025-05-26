namespace GloboClima.Tests.Integration;

public class LocationsTest
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;

    public LocationsTest()
    {
        _factory = new CustomWebApplicationFactory();
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task ShouldGetLocationWeatherInformationByLatLon()
    {
        var json = await File.ReadAllTextAsync("Mocks/weather-response.json");
        _factory.HttpServiceMock
            .Setup(x => x.GetAsync(It.IsAny<HttpClient>(), It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        var lat = "-19.9191248";
        var lon = "-43.9386291";
        var response = await _httpClient.GetAsync($"/locations/lat/{lat}/lon/{lon}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var output = await response.Content.ReadFromJsonAsync<GetLocationOutput>();
        Assert.NotNull(output);
        Assert.Equal(1748284663, output.CurrentUTCTimeUnix);
        Assert.Equal(23.95, output.TemperatureInCelsius);
        Assert.Equal(23.78, output.FeelsLikeInCelsius);
        Assert.Equal(1019, output.PressureInhPa);
        Assert.Equal(53, output.HumidityPercent);
        Assert.Equal(20, output.CloudsPercent);
        Assert.Equal(2.06, output.WindSpeedInMeterPerSecond);
    }

    [Fact]
    public async Task ShouldNotGetLocationWeatherInformationForInvalidLatLon()
    {
        _factory.HttpServiceMock
            .Setup(x => x.GetAsync(It.IsAny<HttpClient>(), It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.UnprocessableEntity
            });
        var lat = "91.00";
        var lon = "181.00";
        var response = await _httpClient.GetAsync($"/locations/lat/{lat}/lon/{lon}");
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }
}