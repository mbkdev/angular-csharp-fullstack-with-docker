using backend.Extensions;
using core.Services;
using data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure services for CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost", "http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

#if !DEBUG
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
#else
var connectionString = builder.Configuration.GetConnectionString("DevelopmentConnection");
#endif

builder.Services.AddDbContext<BackendDbContext>(opt => opt.UseSqlServer(connectionString));
builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();
await app.CheckDatabaseActuatlity(app.Logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.MapControllers();

app.Run();