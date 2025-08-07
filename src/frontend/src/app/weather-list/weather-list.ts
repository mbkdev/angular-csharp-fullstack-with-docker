import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { WeatherBackendModel, WeatherForecast } from '../models/weatherBackendModel';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-weather-list',
  imports: [CommonModule, HttpClientModule, RouterLink],
  templateUrl: './weather-list.html',
  styleUrl: './weather-list.scss'
})
export class Weather implements OnInit {
  weatherForecasts: WeatherForecast[] = [];
  error: string | null = null;

  constructor(private weatherService: WeatherBackendModel) { }

  ngOnInit(): void {
    console.log("init");
    
    this.getAllWeatherForecasts();
  }

  getAllWeatherForecasts() {
    this.weatherService.getAllWeatherForecasts().subscribe({
      next: data => {
        this.weatherForecasts = data;
        this.error = null;
      },
      error: err => {
        this.error = 'Could not fetch weather data. Please try again later.'
      }
    });
  }

  deleteWeatherForecast(weatherForecastId: undefined | string) {
    this.weatherService.removeWeatherForecast(weatherForecastId).subscribe(x => {
      this.getAllWeatherForecasts();
    });
  }
}
