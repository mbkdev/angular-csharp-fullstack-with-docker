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
                return this.NotFound();
            }

            return this.Ok(weatherForecast);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<WeatherForecastDto>>> GetAllWeatherForecastsAsync()
        {
            this.logger.LogInformation("Maax Test");
            var weatherForecasts = await this.weatherForecastService.GetAllWeatherForecastsAsync();

            return this.Ok(weatherForecasts);
        }

        [HttpPost]
        public async Task<ActionResult<WeatherForecastDto>> PostNewWeatherForecastAsync(CreateWeatherForecastDto weatherForecastDto)
        {
            var weatherForecast = await this.weatherForecastService.CreateNewWeatherForecastAsync(weatherForecastDto);

            return this.Ok(weatherForecast);
        }

        [HttpPut]
        public async Task<ActionResult<WeatherForecast>> UpdateWeatherForecastAsync(Guid id, UpdateWeatherForecastDto updateWeatherForecastDto)
        {
            var weatherForecast = await this.weatherForecastService.UpdateWeatherForecastAsync(id, updateWeatherForecastDto);

            if (weatherForecast is null)
            {
                return this.NotFound();
            }

            return this.Ok(weatherForecast);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> RemoveWeatherForecastAsync(Guid id)
        {
            return this.Ok(await this.weatherForecastService.DeleteWeatherForecastAsync(id));
        }
    }
}
