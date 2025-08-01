using core.Models.Dtos;
using data;
using data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace core.Services
{
    public class WeatherForecastService(BackendDbContext database, ILogger<WeatherForecastService> logger) : IWeatherForecastService
    {
        private readonly BackendDbContext database = database;
        private readonly ILogger<WeatherForecastService> logger = logger;

        public async Task<WeatherForecastDto> CreateNewWeatherForecastAsync(CreateWeatherForecastDto weatherForecastDto)
        {
            var weatherForecast = new WeatherForecast
            {
                Date = weatherForecastDto.Date,
                TemperatureC = weatherForecastDto.TemperatureC,
                TemperatureF = weatherForecastDto.TemperatureF,
                Summary = weatherForecastDto.Summary,
            };

            await this.database.WeatherForecasts.AddAsync(weatherForecast);
            await this.database.SaveChangesAsync();

            return new WeatherForecastDto(weatherForecast.Id, weatherForecast.Date, weatherForecast.TemperatureC, weatherForecast.TemperatureF, weatherForecast.Summary);
        }

        public async Task DeleteWeatherForecastAsync(Guid id)
        {
            var weatherForecastToDelete = await this.database.WeatherForecasts.FindAsync(id);
            if (weatherForecastToDelete is not null)
            {
                this.database.Remove(weatherForecastToDelete);
                await this.database.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<WeatherForecastDto>> GetAllWeatherForecastsAsync() =>
            await this.database.WeatherForecasts.AsNoTracking()
                                                .Select(weatherforecast => new WeatherForecastDto(
                                                    weatherforecast.Id,
                                                    weatherforecast.Date,
                                                    weatherforecast.TemperatureC,
                                                    weatherforecast.TemperatureF,
                                                    weatherforecast.Summary))
                                                .ToListAsync();

        public async Task<WeatherForecastDto?> GetWeatherForecastByIdAsync(Guid id)
        {
            var weatherForecast = await this.database.WeatherForecasts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (weatherForecast is null)
                return null;

            return new WeatherForecastDto(weatherForecast.Id, weatherForecast.Date, weatherForecast.TemperatureC, weatherForecast.TemperatureF, weatherForecast.Summary);
        }

        public async Task<WeatherForecastDto?> UpdateWeatherForecastAsync(Guid id, UpdateWeatherForecastDto weatherForecast)
        {
            var existingWeatherForecast = await this.database.WeatherForecasts.FindAsync(id);
            if (existingWeatherForecast is null)
                return null;

            existingWeatherForecast.Date = weatherForecast.Date;
            existingWeatherForecast.TemperatureC = weatherForecast.TemperatureC;
            existingWeatherForecast.TemperatureF = weatherForecast.TemperatureF;
            existingWeatherForecast.Summary = weatherForecast.Summary;

            await this.database.SaveChangesAsync();

            // TODO: Use Mapper
            return new WeatherForecastDto(existingWeatherForecast.Id, existingWeatherForecast.Date, existingWeatherForecast.TemperatureC, existingWeatherForecast.TemperatureF, existingWeatherForecast.Summary);
        }
    }
}
