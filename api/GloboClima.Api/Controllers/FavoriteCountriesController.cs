using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Inputs;
using GloboClima.Api.Schema;
using GloboClima.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Amazon.DynamoDBv2.DocumentModel;

namespace GloboClima.Api.Controllers;

[ApiController]
[Route("favorite-countries")]
public class FavoriteCountriesController(IDynamoDBContext context) : ControllerBase
{
    private readonly IDynamoDBContext _context = context;

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreateFavoriteCountryOutput>> CreateFavoriteCountry([FromBody] CreateFavoriteCountryInput input)
    {
        var favoriteCountry = input.GetFavoriteCountry(User);
        await _context.SaveAsync(favoriteCountry);
        return Ok(new CreateFavoriteCountryOutput() { FavoriteCountryId = favoriteCountry.Id });
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<ActionResult> DeleteFavoriteCountry([FromRoute] Guid id)
    {
        var favoriteCountry = await _context.LoadAsync<FavoriteCountry>(id);
        if (favoriteCountry is null || !favoriteCountry.Username.Equals(User.GetName()))
            return NotFound();
        await _context.DeleteAsync<FavoriteCountry>(id);
        return NoContent();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<FavoriteCountry>>> ListFavoriteCountries()
    {
        var conditions = new List<ScanCondition>()
        {
            new ScanCondition("Username", ScanOperator.Equal, User.GetName())
        };
        var favoriteCountries = await _context.ScanAsync<FavoriteCountry>(conditions)
            .GetRemainingAsync();
        return Ok(favoriteCountries);
    }
}