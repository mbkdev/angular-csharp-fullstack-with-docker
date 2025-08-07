import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WeatherEdit } from './weather-edit';

describe('WeatherEdit', () => {
  let component: WeatherEdit;
  let fixture: ComponentFixture<WeatherEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WeatherEdit]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WeatherEdit);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
