using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace WireMock.IntegrationTests;

public abstract class BaseIntegrationTest
{
    protected readonly string WiremockUrl;
    public WebApplicationFactory<Program> Factory { get; }

    protected BaseIntegrationTest()
    {
        WiremockUrl = Environment.GetEnvironmentVariable("WIREMOCK_URL") ?? "http://localhost:8080";
        Factory = new WebApplicationFactory<Program>();
    }

    protected WebApplicationFactory<Program> CreateConfiguredFactory()
    {
        return Factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Mock the IOptions<ExternalApiOptions>
                var mockOptions = new ExternalApiOptions
                {
                    Url = WiremockUrl,
                    Timeout = 30
                };
                
                services.Configure<ExternalApiOptions>(options =>
                {
                    options.Url = mockOptions.Url;
                    options.Timeout = mockOptions.Timeout;
                });
            });
        });
    }

    protected HttpClient CreateConfiguredClient()
    {
        var configuredFactory = CreateConfiguredFactory();
        return configuredFactory.CreateClient();
    }
} 