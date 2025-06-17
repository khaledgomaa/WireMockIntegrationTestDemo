using WireMock;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient<ExternalApiClient>();

builder.Services.Configure<ExternalApiOptions>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/users", async (ExternalApiClient externalApiClient) =>
{
    var users = await externalApiClient.GetUsersAsync();
    return Results.Ok(users);
})
.WithName("GetUsers");

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }