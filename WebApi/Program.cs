using Microsoft.EntityFrameworkCore;
using WebApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("WARNING: Connection string is not configured!");
}

builder.Services.AddDbContext<AppDbContext>(
    opt => opt.UseNpgsql(connectionString)
    .EnableSensitiveDataLogging() // Для отладки
    .EnableDetailedErrors() // Для отладки
);

// OpenAPI/Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

// Логируем информацию о портах
var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
Console.WriteLine($"ASPNETCORE_URLS: {urls}");
Console.WriteLine($"DOTNET_RUNNING_IN_CONTAINER: {Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")}");

var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
if (!runningInContainer)
{
    app.UseHttpsRedirection();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();

app.UseAuthorization();
app.MapControllers();

// Проверяем подключение к БД при запуске
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var canConnect = await db.Database.CanConnectAsync();
        Console.WriteLine($"Database connection: {(canConnect ? "OK" : "FAILED")}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"WARNING: Cannot connect to database: {ex.Message}");
    Console.WriteLine("Application will continue, but database operations may fail.");
}

Console.WriteLine("Application is starting...");
Console.WriteLine($"Listening on: {string.Join(", ", app.Urls)}");
app.Run();
