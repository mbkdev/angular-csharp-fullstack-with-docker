import { bootstrapApplication, BrowserModule } from '@angular/platform-browser';
import { App } from './app/app';
import { importProvidersFrom } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { API_BASE_URL } from './app/authentication/tokens';
import { WeatherBackendModel } from './app/models/weatherBackendModel';
import { AuthInterceptor } from './app/authentication/auth.interceptor';

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),
    importProvidersFrom(BrowserModule, HttpClientModule),
    {
      provide: WeatherBackendModel,
      useFactory: (http: HttpClient, apiBaseUrl: string) => new WeatherBackendModel(http, apiBaseUrl),
      deps: [HttpClient, API_BASE_URL]
    },
    provideHttpClient(withInterceptorsFromDi()),
    {
       provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true
    }
  ]
});
