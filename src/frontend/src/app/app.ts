import { Component } from '@angular/core';
import { Weather } from './weather/weather';

@Component({
  selector: 'app-root',
  template: '<app-weather></app-weather>',
  imports: [Weather],
  standalone: true,
})
export class App {
  protected title = 'frontend';
}
