using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Inputs;
using GloboClima.Api.Presenters;
using GloboClima.Api.Schema;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Api.Controllers;

[ApiController]
[Route("favorite-countries")]
public class FavoriteCountriesController(IDynamoDBContext context) : ControllerBase
{
    private readonly IDynamoDBContext _context = context;

    [HttpPost]
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFavoriteCountry([FromRoute] Guid id)
    {
        try
        {
            var favoriteCountry = await _context.LoadAsync<FavoriteCountry>(id);
            if (favoriteCountry is null) return NotFound();
            await _context.DeleteAsync<FavoriteCountry>(id);
            return NoContent();
        }
        catch (Exception error)
        {
            var output = ErrorPresenter.GenerateJson(error.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, output);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<FavoriteCountry>>> ListFavoriteCountries()
    {
        try
        {
            var favoriteCountries = await _context.ScanAsync<FavoriteCountry>(new List<ScanCondition>())
                .GetRemainingAsync();
            return Ok(favoriteCountries);
        }
        catch (Exception error)
        {
            var output = ErrorPresenter.GenerateJson(error.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, output);
        }
    }
}