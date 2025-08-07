import { Routes } from '@angular/router';
import { Weather } from './weather-list/weather-list';
import { PageNotFound } from './page-not-found/page-not-found';
import { WeatherCreate } from './weather-create/weather-create';
import { WeatherEdit } from './weather-edit/weather-edit';

export const routes: Routes = [
    { path: 'weather', component: Weather },
    { path: 'weather/create', component: WeatherCreate },
    { path: 'weather/edit/:id', component: WeatherEdit },
    { path: '**', component: PageNotFound }
];
