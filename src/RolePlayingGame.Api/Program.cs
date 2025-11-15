using RolePlayingGame.Application.Configuration;
using RolePlayingGame.Infrastructure.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load configuration
var configuration = new ConfigurationManager();
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

// Configure logging
LoggingConfiguration.ConfigureLogger(configuration, environment);
builder.Host.UseSerilog();

// ---------------------------
//  Application Layer
// ---------------------------
ApplicationConfiguration.ConfigureServices(builder.Services);

// ---------------------------
//  Infrastructure Layer
// ---------------------------
InfrastructureConfiguration.ConfigureServices(builder.Services, builder.Configuration);


var app = builder.Build();

// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
