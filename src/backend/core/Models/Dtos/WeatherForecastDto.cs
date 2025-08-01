namespace core.Models.Dtos
{
    public class WeatherForecastDto(Guid id, DateOnly date, int temperatureC, int temperatureF, string? summary)
    {
        public Guid Id { get; set; } = id;

        public DateOnly Date { get; set; } = date;

        public int TemperatureC { get; set; } = temperatureC;

        public int TemperatureF { get; set; } = temperatureF;

        public string? Summary { get; set; } = summary;
    }
}
