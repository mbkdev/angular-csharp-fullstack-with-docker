import { Component, inject } from '@angular/core';
import { UpdateWeatherForecastDto, WeatherBackendModel } from '../models/weatherBackendModel';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-weather-edit',
  imports: [FormsModule, RouterLink],
  templateUrl: './weather-edit.html',
  styleUrl: './weather-edit.scss'
})
export class WeatherEdit {
  id: string | undefined;
  date?: string;
  temperatureC?: number;
  temperatureF?: number;
  summary?: string | undefined;

  private router = inject(Router);

  constructor(private activatedRoute: ActivatedRoute, private weatherService: WeatherBackendModel) { }

  ngOnInit() {
    var id = this.activatedRoute.snapshot.paramMap.get('id');

    if (id != null) {
      this.id = id;
      this.weatherService.getWeatherForecastById(id).subscribe(wf => {
          this.date = wf.date,
          this.summary = wf.summary,
          this.temperatureC = wf.temperatureC,
          this.temperatureF = wf.temperatureF
      });
    } else {
      alert("Id Parameter nicht gefunden");
    }
  }

  saveWeatherForecast() {
    var updateWeatherForecast = new UpdateWeatherForecastDto();

    updateWeatherForecast.date = this.date;
    updateWeatherForecast.summary = this.summary;
    updateWeatherForecast.temperatureC = this.temperatureC;
    updateWeatherForecast.temperatureF = this.temperatureF;

    this.weatherService.updateWeatherForecast(this.id, updateWeatherForecast).subscribe();
    this.router.navigateByUrl('/weather');
  }
}
