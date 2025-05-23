using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Inputs;
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
        var favoriteCountry = input.GetFavoriteCountry();
        await _context.SaveAsync(favoriteCountry);
        return Ok(new CreateFavoriteCountryOutput() { FavoriteCountryId = favoriteCountry.Id });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteFavoriteCountry([FromRoute] Guid id)
    {
        var favoriteCountry = await _context.LoadAsync<FavoriteCountry>(id);
        if (favoriteCountry is null) return NotFound();
        await _context.DeleteAsync<FavoriteCountry>(id);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<FavoriteCountry>>> ListFavoriteCountries()
    {
        var favoriteCountries = await _context.ScanAsync<FavoriteCountry>(new List<ScanCondition>())
            .GetRemainingAsync();
        return Ok(favoriteCountries);
    }
}