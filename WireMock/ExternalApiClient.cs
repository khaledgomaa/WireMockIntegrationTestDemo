using Microsoft.Extensions.Options;

namespace WireMock;

public class ExternalApiClient
{
    private readonly HttpClient _httpClient;

    public ExternalApiClient(HttpClient httpClient, IOptions<ExternalApiOptions> options)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(options.Value.Timeout);
        _httpClient.BaseAddress = new Uri(options.Value.Url);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync("/users");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<User>>();
        }

        return [];
    }
}


public record User(
    int Id,
    string Name,
    string Company,
    string Username,
    string Email,
    string Address,
    string Zip,
    string State,
    string Country,
    string Phone,
    string? Photo
);

public class ExternalApiOptions
{
    public string Url { get; set; }

    public int Timeout { get; set; }
}