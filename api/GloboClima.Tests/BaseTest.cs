namespace GloboClima.Tests;

public abstract class BaseTest : IAsyncLifetime
{
    protected readonly IAmazonDynamoDB _client;
    protected readonly IDynamoDBContext _context;

    protected BaseTest()
    {
        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:8000"
        };
        _client = new AmazonDynamoDBClient(config);
#pragma warning disable CS0618 // Type or member is obsolete
        _context = new DynamoDBContext(_client);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    public async Task InitializeAsync()
    {
        try
        {
            await _client.DescribeTableAsync("users");
        }
        catch (ResourceNotFoundException)
        {
            await _client.CreateTableAsync(new CreateTableRequest
            {
                TableName = "users",
                AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition("Username", ScalarAttributeType.S)
            },
                KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement("Username", KeyType.HASH)
            },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            });
            await WaitForTableActive();
        }
    }

    public async Task DisposeAsync() => await _client.DeleteTableAsync("users");

    private async Task WaitForTableActive()
    {
        while (true)
        {
            var resp = await _client.DescribeTableAsync("users");
            if (resp.Table.TableStatus == "ACTIVE") break;
            await Task.Delay(500);
        }
    }
}