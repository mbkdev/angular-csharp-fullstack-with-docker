import { bootstrapApplication, BrowserModule } from '@angular/platform-browser';
import { App } from './app/app';
import { importProvidersFrom } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { API_BASE_URL } from './app/tokens';
import { WeatherBackendModel } from './app/models/weatherBackendModel';

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),
    importProvidersFrom(BrowserModule, HttpClientModule),
    {
      provide: WeatherBackendModel,
      useFactory: (http: HttpClient, apiBaseUrl: string) => new WeatherBackendModel(http, apiBaseUrl),
      deps: [HttpClient, API_BASE_URL]
    }
  ]
});
