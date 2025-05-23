using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using GloboClima.Api.Inputs;
using GloboClima.Api.Schema;
using GloboClima.Api.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Api.Controllers;

[ApiController]
public class UsersController(IDynamoDBContext context, IAmazonDynamoDB dbClient, ITokenService tokenService)
    : ControllerBase
{
    private readonly IDynamoDBContext _context = context;
    private readonly IAmazonDynamoDB _dbClient = dbClient;
    private readonly ITokenService _tokenService = tokenService;

    [HttpPost("users")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserInput input)
    {
        var user = input.GetUser();
        await _dbClient.PutItemAsync(new PutItemRequest
        {
            TableName = "users",
            Item = _context.ToDocument(user).ToAttributeMap(),
            ConditionExpression = "attribute_not_exists(Username)"
        });
        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginOutput>> Login([FromBody] LoginInput input)
    {
        var user = await _context.LoadAsync<User>(input.Username);
        if (user is null || !user.PasswordIsCorrect(input.Password))
            return UnprocessableEntity();
        var accessToken = _tokenService.GenerateJwt(user);
        return Ok(new LoginOutput() { AccessToken = accessToken });
    }
}