using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using GloboClima.Api.Extensions;
using GloboClima.Api.Inputs;
using GloboClima.Api.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Api.Controllers;

[ApiController]
[Route("favorite-locations")]
public class FavoriteLocationsController(IDynamoDBContext context) : ControllerBase
{
    private readonly IDynamoDBContext _context = context;

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreateFavoriteLocationOutput>> CreateFavoriteLocation([FromBody] CreateFavoriteLocationInput input)
    {
        var favoriteLocation = input.GetFavoriteLocation(User);
        await _context.SaveAsync(favoriteLocation);
        return Ok(new CreateFavoriteLocationOutput() { FavoriteLocationId = favoriteLocation.Id });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteFavoriteLocation([FromRoute] Guid id)
    {
        var username = User.GetName();
        var favoriteLocation = await _context.LoadAsync<FavoriteLocation>(username, id);
        if (favoriteLocation is null) return NotFound();
        await _context.DeleteAsync<FavoriteLocation>(username, id);
        return NoContent();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<FavoriteLocation>>> ListFavoriteLocations()
    {
        var favoriteLocations = await _context.QueryAsync<FavoriteLocation>(User.GetName())
            .GetRemainingAsync();
        return Ok(favoriteLocations);
    }
}