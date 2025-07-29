import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { WeatherBackendModel, WeatherForecast } from '../models/weatherBackendModel';

@Component({
  selector: 'app-weather',
  imports: [CommonModule, HttpClientModule],
  templateUrl: './weather.html',
  styleUrl: './weather.scss'
})
export class Weather implements OnInit {
  weatherForecasts: WeatherForecast[] = [];
  error: string | null = null;

  constructor(private weatherService: WeatherBackendModel) { }

  ngOnInit(): void {
    this.weatherService.get().subscribe({
      next: data => {
        this.weatherForecasts = data;
        this.error = null;
      },
      error: err => {
        this.error = 'Could not fetch weather data. Please try again later.'
      }
    });
  }
}
