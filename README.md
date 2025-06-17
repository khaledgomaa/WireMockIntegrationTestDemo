# Integration Test Demo with WireMock

This project demonstrates how to use WireMock for integration testing of external APIs in a .NET application. It provides a complete setup with Docker containers, mock API responses, and automated tests.

## 🏗️ Project Structure

```
IntegrationTestDemo/
├── WireMock/                          # Main API application
│   ├── ExternalApiClient.cs          # HTTP client for external API
│   ├── Program.cs                    # API endpoints and configuration
│   └── WireMock.csproj              # Project configuration
├── WireMock.IntegrationTests/        # Integration test project
│   ├── BaseIntegrationTest.cs       # Base class for test setup
│   ├── UserApiTests.cs              # Integration tests
│   ├── mappings/                    # WireMock response mappings
│   │   └── users.json              # Mock user data responses
│   └── WireMock.IntegrationTests.csproj
├── docker-compose.yml               # Production-like environment
├── docker-compose-local.yml         # Local development environment
└── README.md
```

## 🔧 Components

### 1. ExternalApiClient
Located in `WireMock/ExternalApiClient.cs`, this class:
- Handles HTTP communication with external APIs
- Uses dependency injection for configuration
- Configurable timeout and base URL via `ExternalApiOptions`

### 2. WireMock Mappings
Located in `WireMock.IntegrationTests/mappings/users.json`:
- Defines mock responses for external API calls
- Simulates different scenarios (success, failure, etc.)
- Used by WireMock server to return predictable responses

**Example mapping structure:**
```json
{
  "request": {
    "method": "GET",
    "url": "/users"
  },
  "response": {
    "status": 200,
    "body": "[{\"id\": 1, \"name\": \"John Doe\"}]"
  }
}
```

### 3. BaseIntegrationTest
Located in `WireMock.IntegrationTests/BaseIntegrationTest.cs`:
- Provides common setup for all integration tests
- Configures WireMock URL and timeout settings
- Creates configured HTTP clients for testing

### 4. Integration Tests
Located in `WireMock.IntegrationTests/UserApiTests.cs`:
- Tests the complete API flow
- Verifies integration with external services
- Uses WireMock for predictable external API responses

## 🐳 Docker Compose Files

### docker-compose.yml (Production-like)
Used for production-like testing environment:
- Runs WireMock server on port 8080
- Loads mappings from the `mappings/` directory
- Provides stable, predictable external API responses
- Used by CI/CD pipelines and production testing

### docker-compose-local.yml (Local Development)
Used for local development and testing:
- Runs WireMock server on port 8080
- Mounts local mappings directory for easy updates
- Enables hot-reload of mapping changes
- Perfect for development and debugging

**Key differences:**
- **Production**: Uses built-in mappings, stable environment
- **Local**: Mounts local mappings, enables live editing

## 🚀 How to Run

### Prerequisites
- .NET 9.0 SDK
- Docker and Docker Compose
- Your preferred IDE (Visual Studio, Rider, VS Code)

### 1. Start WireMock Server
```bash
# For local development (recommended)
docker-compose -f docker-compose-local.yml up -d

# For production-like environment
docker-compose up -d
```

The API will be available at `https://localhost:5001` (or the configured port).

### 2. Run Integration Tests
```bash
cd WireMock.IntegrationTests
dotnet test
```

## 🧪 Testing

### Manual Testing
1. **Start WireMock**: `docker-compose -f docker-compose-local.yml up -d`
2. **Start API**: `dotnet run` (from WireMock directory)
3. **Test endpoint**: `GET https://localhost:5001/api/users`

### Automated Testing
```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run specific test class
dotnet test --filter "FullyQualifiedName~UserApiTests"
```

### Test Scenarios
- ✅ **Success case**: Returns mock user data
- ❌ **Failure case**: Simulates external API failures
- 🔄 **Integration**: Tests complete request/response flow

## 🔧 Configuration

### Environment Variables
- `WIREMOCK_URL`: WireMock server URL (default: `http://localhost:8080`)
- `ASPNETCORE_ENVIRONMENT`: Environment name (Development/Production)

### ExternalApiOptions
```json
{
  "ExternalApiOptions": {
    "Url": "http://localhost:8080",
    "Timeout": 30
  }
}
```

## 📝 Adding New Tests

1. **Create test class** inheriting from `BaseIntegrationTest`
2. **Add mappings** in `mappings/` directory
3. **Write test methods** using the configured HTTP client
4. **Run tests** with `dotnet test`

### Example Test Structure
```csharp
public class NewApiTests : BaseIntegrationTest
{
    private readonly HttpClient _httpClient;

    public NewApiTests()
    {
        _httpClient = CreateConfiguredClient();
    }

    [Fact]
    public async Task TestNewEndpoint_ReturnsExpectedData()
    {
        // Arrange, Act, Assert
    }
}
```

## 🎯 Benefits

- **Isolated Testing**: No dependency on real external APIs
- **Predictable Results**: WireMock provides consistent responses
- **Fast Execution**: No network calls to external services
- **Reliable CI/CD**: Tests work consistently in all environments
- **Easy Debugging**: Full control over mock responses

## 🔍 Troubleshooting

### Common Issues
1. **WireMock not running**: Check Docker containers with `docker ps`
2. **Port conflicts**: Ensure port 8080 is available
3. **Mapping not loaded**: Verify mapping files are in correct location
4. **Test failures**: Check WireMock logs with `docker-compose logs wiremock`

### Debug Commands
```bash
# Check WireMock status
curl http://localhost:8080/__admin/mappings

# View WireMock logs
docker-compose logs wiremock

# Restart WireMock
docker-compose restart wiremock
```

This setup provides a robust foundation for integration testing with external APIs, ensuring your application works correctly without depending on external services.

