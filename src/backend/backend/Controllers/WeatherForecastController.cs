using core.Models.Dtos;
using core.Services;
using data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService) : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger = logger;
        private readonly IWeatherForecastService weatherForecastService = weatherForecastService;

        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherForecastDto>> GetWeatherForecastByIdAsync(Guid id)
        {
            var weatherForecast = await this.weatherForecastService.GetWeatherForecastByIdAsync(id);

            if (weatherForecast is null)
            {
                return NotFound();
            }

            return Ok(weatherForecast);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<WeatherForecastDto>>> GetAllWeatherForecastsAsync()
        {
            var weatherForecasts = await this.weatherForecastService.GetAllWeatherForecastsAsync();
            if (!weatherForecasts.Any())
            {
                return new EmptyResult();
            }

            return Ok(weatherForecasts);
        }

        [HttpPost]
        public async Task<ActionResult<WeatherForecastDto>> PostNewWeatherForecastAsync(CreateWeatherForecastDto weatherForecastDto)
        {
            var weatherForecast = await weatherForecastService.CreateNewWeatherForecastAsync(weatherForecastDto);

            return Ok(weatherForecast);
        }

        [HttpPut]
        public async Task<ActionResult<WeatherForecast>> UpdateWeatherForecastAsync(Guid id, UpdateWeatherForecastDto updateWeatherForecastDto)
        {
            var weatherForecast = await weatherForecastService.UpdateWeatherForecastAsync(id, updateWeatherForecastDto);

            if (weatherForecast is null)
            {
                return NotFound();
            }

            return Ok(weatherForecast);
        }

        [HttpDelete]
        public async Task RemoveWeatherForecastAsync(Guid id)
        {
            await weatherForecastService.DeleteWeatherForecastAsync(id);
        }
    }
}
