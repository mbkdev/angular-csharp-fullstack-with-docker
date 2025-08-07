import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WeatherCreate } from './weather-create';

describe('WeatherCreate', () => {
  let component: WeatherCreate;
  let fixture: ComponentFixture<WeatherCreate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WeatherCreate]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WeatherCreate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
