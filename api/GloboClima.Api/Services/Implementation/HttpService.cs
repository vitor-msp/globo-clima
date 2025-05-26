using GloboClima.Api.Services.Contract;

namespace GloboClima.Api.Services.Implementation;

public class HttpService : IHttpService
{
    public async Task<HttpResponseMessage> GetAsync(HttpClient httpClient, string uri)
        => await httpClient.GetAsync(uri);
}