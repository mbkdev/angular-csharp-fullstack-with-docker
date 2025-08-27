import { Component, OnInit } from '@angular/core';
import { OutputUserDto, WeatherBackendModel } from '../models/weatherBackendModel';

@Component({
  selector: 'app-user-profile',
  imports: [],
  templateUrl: './user-profile.html',
  styleUrl: './user-profile.scss'
})
export class UserProfile implements OnInit {
  outputUserDto: OutputUserDto = null!;
  error: string | null = null;

  constructor(private weatherService: WeatherBackendModel) { }

  ngOnInit(): void {
    this.getUserProfile();
  }

  getUserProfile() {
    this.weatherService.user_GetCurrentUsersProfile().subscribe({
      next: data => {
        this.outputUserDto = data;
        this.error = null;
      },
      error: err => {
        console.log(err);
      }
    });
  }
}
