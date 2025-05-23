namespace GloboClima.Tests;

public abstract class BaseTest : IAsyncLifetime
{
    protected readonly IAmazonDynamoDB _dbClient;
    protected readonly IDynamoDBContext _dbContext;
    protected readonly HttpClient _httpClient;

    protected BaseTest()
    {
        _dbClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig
        {
            ServiceURL = "http://localhost:8000"
        });

#pragma warning disable CS0618 // Type or member is obsolete
        _dbContext = new DynamoDBContext(_dbClient);
#pragma warning restore CS0618 // Type or member is obsolete

        _httpClient = new CustomWebApplicationFactory(_dbClient, _dbContext).CreateClient();
    }

    public async Task InitializeAsync() => await CreateTable(GetTableName(), GetKeyName());

    private async Task CreateTable(string tableName, string keyName)
    {
        try
        {
            await _dbClient.DescribeTableAsync(tableName);
        }
        catch (ResourceNotFoundException)
        {
            await _dbClient.CreateTableAsync(new CreateTableRequest
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

    public async Task DisposeAsync() => await _dbClient.DeleteTableAsync(GetTableName());

    private async Task WaitForTableActive(string tableName)
    {
        while (true)
        {
            var resp = await _dbClient.DescribeTableAsync(tableName);
            if (resp.Table.TableStatus == "ACTIVE") break;
            await Task.Delay(500);
        }
    }

    protected abstract string GetTableName();
    protected abstract string GetKeyName();
}