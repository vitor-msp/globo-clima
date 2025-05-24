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
        var favoriteLocation = await _context.LoadAsync<FavoriteLocation>(id);
        if (favoriteLocation is null || !favoriteLocation.Username.Equals(User.GetName()))
            return NotFound();
        await _context.DeleteAsync<FavoriteLocation>(id);
        return NoContent();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<FavoriteLocation>>> ListFavoriteLocations()
    {
        var conditions = new List<ScanCondition>()
        {
            new ScanCondition("Username", ScanOperator.Equal, User.GetName())
        };
        var favoriteLocations = await _context.ScanAsync<FavoriteLocation>(conditions)
            .GetRemainingAsync();
        return Ok(favoriteLocations);
    }
}