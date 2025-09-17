using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Resize.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<StorageService>();

// Registra o HttpClient como singleton para o PhotoApiClient
builder.Services.AddHttpClient<Resize.FormaiApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7228/api/photos/");
});

builder.Build().Run();
