namespace GloboClima.Tests.Integration;

public abstract class BaseTest : IAsyncLifetime
{
    protected readonly IConfiguration _configuration;
    protected readonly IAmazonDynamoDB _dbClient;
    protected readonly IDynamoDBContext _dbContext;
    protected readonly HttpClient _httpClient;

    protected BaseTest()
    {
        _configuration = LoadConfiguration();
        var dbConnectionString = _configuration.GetConnectionString("DynamoDBConnection")
           ?? throw new Exception("Missing configure db connection string.");
        _dbClient = new AmazonDynamoDBClient(new AmazonDynamoDBConfig { ServiceURL = dbConnectionString });

#pragma warning disable CS0618 // Type or member is obsolete
        _dbContext = new DynamoDBContext(_dbClient);
#pragma warning restore CS0618 // Type or member is obsolete

        _httpClient = new CustomWebApplicationFactory().CreateClient();
    }

    public async Task InitializeAsync() => await CreateTable(GetTableName(), GetPartitionKey(), GetSortKey());

    private async Task CreateTable(string tableName, string partitionKey, string? sortKey = null)
    {
        try
        {
            await _dbClient.DescribeTableAsync(tableName);
        }
        catch (ResourceNotFoundException)
        {
            var attributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition(partitionKey, ScalarAttributeType.S)
            };
            var keySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement(partitionKey, KeyType.HASH)
            };
            if (!string.IsNullOrEmpty(sortKey))
            {
                attributeDefinitions.Add(new AttributeDefinition(sortKey, ScalarAttributeType.S));
                keySchema.Add(new KeySchemaElement(sortKey, KeyType.RANGE));
            }
            await _dbClient.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = attributeDefinitions,
                KeySchema = keySchema,
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
            if (resp.Table.TableStatus == TableStatus.ACTIVE) break;
            await Task.Delay(500);
        }
    }

    protected abstract string GetTableName();
    protected abstract string GetPartitionKey();
    protected abstract string? GetSortKey();

    private static IConfiguration LoadConfiguration()
        => new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .Build();
}