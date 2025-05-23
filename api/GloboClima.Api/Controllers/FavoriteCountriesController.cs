using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Inputs;
using GloboClima.Api.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Api.Controllers;

[ApiController]
public class FavoriteCountriesController(IDynamoDBContext context) : ControllerBase
{
    private readonly IDynamoDBContext _context = context;

    [HttpPost("countries/favorites")]
    public async Task<ActionResult<CreateFavoriteCountryOutput>> CreateFavoriteCountry([FromBody] CreateFavoriteCountryInput input)
    {
        try
        {
            var favoriteCountry = input.GetFavoriteCountry();
            await _context.SaveAsync(favoriteCountry);
            return Ok(new CreateFavoriteCountryOutput() { FavoriteCountryId = favoriteCountry.Id });
        }
        catch (Exception error)
        {
            var output = ErrorPresenter.GenerateJson(error.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, output);
        }
    }
}