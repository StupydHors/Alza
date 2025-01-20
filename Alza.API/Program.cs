using Api;
using Application;
using Infrastructure;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (bool.TryParse(builder.Configuration.GetSection("TryGenerateBogusData").Value ?? "false", out var seedWithBogusData) && seedWithBogusData)
{
    await app.SeedDataAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();


namespace Api
{
    public static class SeederExtensions
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseMockDataSeeder>();
            await seeder.SeedAsync();
        }
    }
}