namespace GloboClima.Api.Services.Contract;

public interface IHttpService
{
    public Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string uri);
}