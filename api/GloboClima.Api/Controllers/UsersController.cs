using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Inputs;
using GloboClima.Api.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Api.Controllers;

[ApiController]
public class UsersController(IDynamoDBContext context) : ControllerBase
{
    private readonly IDynamoDBContext _context = context;

    [HttpPost("users")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserInput input)
    {
        try
        {
            var user = input.GetUser();
            await _context.SaveAsync(user);
            return Created();
        }
        catch (Exception error)
        {
            var output = ErrorPresenter.GenerateJson(error.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, output);
        }
    }
}