using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Inputs;
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
        var favoriteLocation = input.GetFavoriteLocation();
        await _context.SaveAsync(favoriteLocation);
        return Ok(new CreateFavoriteLocationOutput() { FavoriteLocationId = favoriteLocation.Id });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFavoriteLocation([FromRoute] Guid id)
    {
        var favoriteLocation = await _context.LoadAsync<FavoriteLocation>(id);
        if (favoriteLocation is null) return NotFound();
        await _context.DeleteAsync<FavoriteLocation>(id);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<FavoriteLocation>>> ListFavoriteLocations()
    {
        var favoriteLocations = await _context.ScanAsync<FavoriteLocation>(new List<ScanCondition>())
            .GetRemainingAsync();
        return Ok(favoriteLocations);
    }
}