import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CreateWeatherForecastDto, WeatherBackendModel, WeatherForecast } from '../models/weatherBackendModel';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-weather-create',
  imports: [FormsModule, RouterLink],
  templateUrl: './weather-create.html',
  styleUrl: './weather-create.scss'
})
export class WeatherCreate {

  private router = inject(Router);

  date?: string;
  temperatureC?: number;
  temperatureF?: number;
  summary?: string | undefined;

  constructor(private weatherService: WeatherBackendModel) { }

  saveWeatherForecast(){
    var weatherForecast = new CreateWeatherForecastDto();

    weatherForecast.date = this.date;
    weatherForecast.summary = this.summary;
    weatherForecast.temperatureC = this.temperatureC;
    weatherForecast.temperatureF = this.temperatureF;

    this.weatherService.weatherForecast_PostNewWeatherForecast(weatherForecast).subscribe(); 

    this.router.navigateByUrl('/weather');
  }
}
