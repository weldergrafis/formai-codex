using FormAI.Api.Data;
using FormAI.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//// Configure Entity Framework Core to use SQL Server
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Entity Framework Core to use SQL Server
builder.Services.AddDbContext<AppDbContext>();

// Service responsible for communication with Azure Blob Storage
builder.Services.AddSingleton<StorageService>();
builder.Services.AddSingleton<ServiceBusService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
