namespace core.Models.Dtos
{
    public class CreateWeatherForecastDto(DateOnly date, int temperatureC, int temperatureF, string summary)
    {
        public DateOnly Date { get; set; } = date;

        public int TemperatureC { get; set; } = temperatureC;

        public int TemperatureF { get; set; } = temperatureF;

        public string Summary { get; set; } = summary;
    }
}
