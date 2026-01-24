using Microsoft.EntityFrameworkCore;
using EmployeeApi.Converters;
using EmployeeApi.Data;
using EmployeeApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Используем имена из JsonPropertyName
        options.JsonSerializerOptions.WriteIndented = true; // Для читаемости в Development
        // Настройка парсинга даты для формата "dd.MM.yyyy"
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(
    opt => opt.UseNpgsql(connectionString)
);

builder.Services.AddHttpClient("Service1Api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Service1:BaseUrl"]);
});

builder.Services.AddHostedService<EmployeePostSyncService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var runningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
if (!runningInContainer)
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
