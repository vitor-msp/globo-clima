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

    public async Task InitializeAsync() => await CreateTable(GetTableName(), GetKeyName());

    private async Task CreateTable(string tableName, string keyName)
    {
        try
        {
            await _client.DescribeTableAsync(tableName);
        }
        catch (ResourceNotFoundException)
        {
            await _client.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition(keyName, ScalarAttributeType.S)
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement(keyName, KeyType.HASH)
                },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            });
            await WaitForTableActive(tableName);
        }
    }

    public async Task DisposeAsync() => await _client.DeleteTableAsync(GetTableName());

    private async Task WaitForTableActive(string tableName)
    {
        while (true)
        {
            var resp = await _client.DescribeTableAsync(tableName);
            if (resp.Table.TableStatus == "ACTIVE") break;
            await Task.Delay(500);
        }
    }

    protected abstract string GetTableName();
    protected abstract string GetKeyName();
}