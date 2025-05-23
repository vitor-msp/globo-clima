using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Inputs;
using GloboClima.Api.Presenters;
using GloboClima.Api.Schema;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Api.Controllers;

[ApiController]
[Route("favorite-locations")]
public class FavoriteLocationsController(IDynamoDBContext context) : ControllerBase
{
    private readonly IDynamoDBContext _context = context;

    [HttpPost]
    public async Task<ActionResult<CreateFavoriteLocationOutput>> CreateFavoriteLocation([FromBody] CreateFavoriteLocationInput input)
    {
        try
        {
            var favoriteLocation = input.GetFavoriteLocation();
            await _context.SaveAsync(favoriteLocation);
            return Ok(new CreateFavoriteLocationOutput() { FavoriteLocationId = favoriteLocation.Id });
        }
        catch (Exception error)
        {
            var output = ErrorPresenter.GenerateJson(error.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, output);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFavoriteLocation([FromRoute] Guid id)
    {
        try
        {
            var favoriteLocation = await _context.LoadAsync<FavoriteLocation>(id);
            if (favoriteLocation is null) return NotFound();
            await _context.DeleteAsync<FavoriteLocation>(id);
            return NoContent();
        }
        catch (Exception error)
        {
            var output = ErrorPresenter.GenerateJson(error.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, output);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<FavoriteLocation>>> ListFavoriteLocations()
    {
        try
        {
            var favoriteLocations = await _context.ScanAsync<FavoriteLocation>(new List<ScanCondition>())
                .GetRemainingAsync();
            return Ok(favoriteLocations);
        }
        catch (Exception error)
        {
            var output = ErrorPresenter.GenerateJson(error.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, output);
        }
    }
}