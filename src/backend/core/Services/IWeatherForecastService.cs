using core.Models.Dtos;

namespace core.Services
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecastDto> CreateNewWeatherForecastAsync(CreateWeatherForecastDto weatherForecast);
        Task DeleteWeatherForecastAsync(Guid id);
        Task<IEnumerable<WeatherForecastDto>> GetAllWeatherForecastsAsync();
        Task<WeatherForecastDto?> GetWeatherForecastByIdAsync(Guid id);
        Task<WeatherForecastDto?> UpdateWeatherForecastAsync(Guid id, UpdateWeatherForecastDto weatherForecast);
    }
}
