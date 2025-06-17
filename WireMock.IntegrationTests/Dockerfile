FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

# Copy the solution and project files
COPY ["WireMock.IntegrationTests/WireMock.IntegrationTests.csproj", "WireMock.IntegrationTests/"]
COPY ["WireMock/WireMock.csproj", "WireMock/"]

# Restore dependencies
RUN dotnet restore "WireMock.IntegrationTests/WireMock.IntegrationTests.csproj"

# Copy the rest of the code
COPY . .

# Build and run tests
WORKDIR /app/WireMock.IntegrationTests
RUN dotnet build
ENTRYPOINT ["dotnet", "test"] 