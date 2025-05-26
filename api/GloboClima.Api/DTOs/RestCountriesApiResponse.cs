using System.Text.Json.Serialization;
using GloboClima.Api.Inputs;

namespace GloboClima.Api.DTOs;

public class RestCountriesApiResponse
{
    [JsonPropertyName("name")]
    public Name CountryName { get; set; } = new();

    [JsonPropertyName("cioc")]
    public string Cioc { get; set; } = "";

    [JsonPropertyName("currencies")]
    public Dictionary<string, Currency> Currencies { get; set; } = [];

    [JsonPropertyName("languages")]
    public Dictionary<string, string> Languages { get; set; } = [];

    [JsonPropertyName("population")]
    public double Population { get; set; }

    public class Name
    {
        [JsonPropertyName("official")]
        public string OfficialName { get; set; } = "";

        [JsonPropertyName("common")]
        public string CommonName { get; set; } = "";
    }

    public class Currency
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = "";

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }

    public GetCountryOutput ConvertOutput()
        => new()
        {
            Cioc = Cioc,
            CommonName = CountryName.CommonName,
            OfficialName = CountryName.OfficialName,
            Population = Population,
            Languages = Languages.Values.Select(language => language).ToList(),
            Currencies = Currencies.Values.Select(currency => new GetCountryOutput.Currency()
            {
                Name = currency.Name,
                Symbol = currency.Symbol
            }).ToList()
        };
}

