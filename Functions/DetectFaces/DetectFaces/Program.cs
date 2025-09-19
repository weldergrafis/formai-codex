using DetectFaces;
using DetectFaces.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neurotec.Biometrics.Client;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// Registra o HttpClient como singleton para o PhotoApiClient
builder.Services.AddHttpClient<FormaiApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7228/api/photos/");
    //client.BaseAddress = new Uri("https://formai-api-f4dnhegdfye5eebe.brazilsouth-01.azurewebsites.net/api/photos/");
});

builder.Services.AddSingleton<StorageService>();

// Comentário: NBiometricClient como SINGLETON
builder.Services.AddSingleton<NBiometricClient>(x => NeurotecService.CreateBiometricClient());
builder.Services.AddSingleton<NeurotecService>();
builder.Services.AddHostedService<NeurotecHostService>();

//// Program.cs
//builder.Services.AddSingleton<NBiometricClient>(sp =>
//{
//    var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
//    var pid = Environment.ProcessId; // Comentário: ajuda a ver reinícios de processo
//    logger.LogInformation("NEUROTEC_INIT_START pid={Pid}", pid);

//    try
//    {
//        NeurotecService.Initialize(); // Comentário: garante licença 1x por processo
//        logger.LogInformation("NEUROTEC_INIT_END pid={Pid}", pid);
//    }
//    catch (Exception ex)
//    {
//        logger.LogError($"NEUROTEC_INIT_ERROR pid={pid} - Exception: {ex.Message}", pid);
//        throw;
//    }
//    return NeurotecService.CreateBiometricClient(); // Comentário: só cria após licenciar
//});

//builder.Services.AddSingleton<NeurotecService>();

builder.Build().Run();
