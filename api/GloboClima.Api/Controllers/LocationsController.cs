using System.Net;
using GloboClima.Api.Configuration;
using GloboClima.Api.DTOs;
using GloboClima.Api.Inputs;
using GloboClima.Api.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace GloboClima.Api.Controllers;

[ApiController]
[Route("locations")]
public class LocationsController(IOptions<OpenWeatherMapConfiguration> options, IHttpService httpService)
    : ControllerBase
{
    private readonly OpenWeatherMapConfiguration _openWeatherMapConfiguration = options.Value;
    private readonly IHttpService _httpService = httpService;

    [HttpGet("lat/{lat}/lon/{lon}")]
    public async Task<ActionResult<List<GetLocationOutput>>> GetLocationWeatherInformation(
        [FromRoute] string lat, [FromRoute] string lon)
    {
        var httpClient = new HttpClient();
        var route = $"{_openWeatherMapConfiguration.BaseUrl}/onecall";
        var query = GetQuery(lat, lon);
        var uri = QueryHelpers.AddQueryString(route, query);
        var response = await _httpService.GetAsync(httpClient, uri);
        if (response.StatusCode != HttpStatusCode.OK) return UnprocessableEntity();
        var location = await response.Content.ReadFromJsonAsync<OpenWeatherMapResponse>()
            ?? throw new Exception($"Error to read location weather information - lat {lat}, lon {lon}");
        return Ok(location.ConvertOutput());
    }

    private Dictionary<string, string?> GetQuery(string lat, string lon)
        => new()
        {
            { "lat", lat },
            { "lon", lon },
            { "units", "metric" },
            { "exclude", "minutely,hourly,daily,alerts" },
            { "lang", "pt_br" },
            { "appid", _openWeatherMapConfiguration.ApiKey },
        };
}