using System.Net;
using GloboClima.Api.Configuration;
using GloboClima.Api.DTOs;
using GloboClima.Api.Inputs;
using GloboClima.Api.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GloboClima.Api.Controllers;

[ApiController]
[Route("countries")]
public class CountriesController(IOptions<RestCountriesApiConfiguration> options, IHttpService httpService)
    : ControllerBase
{
    private readonly RestCountriesApiConfiguration _restCountriesApiConfiguration = options.Value;
    private readonly IHttpService _httpService = httpService;

    [HttpGet("{cioc}")]
    public async Task<ActionResult<List<GetCountryOutput>>> GetCountryDemographicInformation([FromRoute] string cioc)
    {
        var httpClient = new HttpClient();
        var uri = $"{_restCountriesApiConfiguration.BaseUrl}/alpha/{cioc}";
        var response = await _httpService.GetAsync(httpClient, uri);
        if (response.StatusCode != HttpStatusCode.OK) return NotFound();
        var country = await response.Content.ReadFromJsonAsync<List<RestCountriesApiResponse>>()
            ?? throw new Exception($"Error to read country demographic information - cioc {cioc}");
        return Ok(country[0].ConvertOutput());
    }
}