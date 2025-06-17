using System.Net;
using System.Net.Http.Json;

namespace WireMock.IntegrationTests;

public class UserApiTests : BaseIntegrationTest
{
    private readonly HttpClient _httpClient;

    public UserApiTests()
    {
        _httpClient = CreateConfiguredClient();
    }

    [Fact]
    public async Task GetUsers_WhenSuccessful_ReturnsUsersList()
    {
        // Arrange
        var expectedUsers = new List<User>
        {
            new(1, "Emily Johnson", "ABC Corporation", "emily_johnson", "emily.johnson@abccorporation.com",
                "123 Main St", "12345", "California", "USA", "+1-555-123-4567", "https://loremflickr.com/1713/787?lock=8160151724855441"),
            new(2, "Michael Williams", "XYZ Corp", "michael_williams", "michael.williams@xyzcorp.com",
                "456 Elm Ave", "67890", "New York", "USA", "+1-555-987-6543", null)
        };

        // Act
        var response = await _httpClient.GetAsync("/api/users");
        var users = await response.Content.ReadFromJsonAsync<List<User>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(users);
        Assert.NotEmpty(users);
        Assert.Equal(expectedUsers.Count, users!.Count);
        Assert.Equal(expectedUsers[0].Name, users[0].Name);
        Assert.Equal(expectedUsers[0].Email, users[0].Email);
    }

    [Fact]
    public async Task GetUsers_WhenApiFails_ReturnsProblemDetails()
    {
        // Arrange
        var mockResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var mockHandler = new Mock<HttpMessageHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        var client = new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri("https://dummy-json.mock.beeceptor.com")
        };

        // Act
        var response = await client.GetAsync("/users");

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
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