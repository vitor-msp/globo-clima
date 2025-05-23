using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using GloboClima.Api.Inputs;
using GloboClima.Api.Presenters;
using Microsoft.AspNetCore.Mvc;

namespace GloboClima.Api.Controllers;

[ApiController]
public class UsersController(IDynamoDBContext context, IAmazonDynamoDB dbClient) : ControllerBase
{
    private readonly IDynamoDBContext _context = context;
    private readonly IAmazonDynamoDB _dbClient = dbClient;

    [HttpPost("users")]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserInput input)
    {
        try
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
        catch (ConditionalCheckFailedException)
        {
            return Conflict();
        }
        catch (Exception error)
        {
            var output = ErrorPresenter.GenerateJson(error.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, output);
        }
    }
}