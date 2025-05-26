namespace GloboClima.Tests.Integration;

public class CustomWebApplicationFactory() : WebApplicationFactory<Program>
{
    public Mock<IHttpService> HttpServiceMock { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(descriptor => descriptor.ServiceType == typeof(IHttpService));
            if (descriptor is not null)
                services.Remove(descriptor);
            services.AddSingleton(HttpServiceMock.Object);
        });
    }
}