using DetectFaces;
using DetectFaces.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Neurotec.Biometrics.Client;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddSingleton<StorageService>();
builder.Services.AddSingleton<NeurotecService>();

// Registra o HttpClient como singleton para o PhotoApiClient
builder.Services.AddHttpClient<FormaiApiClient>(client =>
{
    //client.BaseAddress = new Uri("https://localhost:7228/api/photos/");
    client.BaseAddress = new Uri("https://formai-api-f4dnhegdfye5eebe.brazilsouth-01.azurewebsites.net/api/photos/");
});

// Comentário: NBiometricClient como SINGLETON
builder.Services.AddSingleton<NBiometricClient>(x=> NeurotecService.CreateBiometricClient());
builder.Services.AddSingleton<NeurotecService>();
builder.Services.AddHostedService<NeurotecHostService>();

builder.Build().Run();
