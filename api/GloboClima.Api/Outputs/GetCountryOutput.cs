namespace GloboClima.Api.Inputs;

public class GetCountryOutput
{
    public string CommonName { get; set; } = "";
    public string OfficialName { get; set; } = "";
    public string Cioc { get; set; } = "";
    public double Population { get; set; }
    public List<string> Languages { get; set; } = [];
    public List<Currency> Currencies { get; set; } = [];

    public class Currency
    {
        public string Symbol { get; set; } = "";
        public string Name { get; set; } = "";
    }
}