using System.Text.RegularExpressions;
using Amazon.DynamoDBv2.DataModel;
using GloboClima.Api.Exceptions;

namespace GloboClima.Api.Schema;

[DynamoDBTable("favorite-countries")]
public partial class FavoriteCountry
{
    private const string CiocPattern = @"^[A-Z|a-z]{3}$";

    [DynamoDBHashKey("username")]
    public required string Username { get; init; }

    [DynamoDBRangeKey("id")]
    public Guid Id { get; init; } = Guid.NewGuid();

    private readonly string _cioc = "";

    [DynamoDBProperty("cioc")]
    public required string Cioc
    {
        get => _cioc;
        init
        {
            if (!Regex.Match(value, CiocPattern).Success)
                throw new DomainException("Cioc must be a valid country IOC.");
            _cioc = value;
        }
    }
}