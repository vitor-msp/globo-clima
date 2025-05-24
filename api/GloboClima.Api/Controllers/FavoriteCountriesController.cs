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
        var username = User.GetName();
        var favoriteCountry = await _context.LoadAsync<FavoriteCountry>(username, id);
        if (favoriteCountry is null) return NotFound();
        await _context.DeleteAsync<FavoriteCountry>(username, id);
        return NoContent();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<FavoriteCountry>>> ListFavoriteCountries()
    {
        var favoriteCountries = await _context.QueryAsync<FavoriteCountry>(User.GetName())
            .GetRemainingAsync();
        return Ok(favoriteCountries);
    }
}