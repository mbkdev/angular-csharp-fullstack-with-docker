import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { OutputUsersWithRolesDto, WeatherBackendModel } from '../models/weatherBackendModel';
import { HttpClientModule } from '@angular/common/http';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-admin-user-overview',
  imports: [CommonModule, HttpClientModule, RouterLink],
  templateUrl: './admin-user-overview.html',
  styleUrl: './admin-user-overview.scss'
})
export class AdminUserOverview implements OnInit {
  users: OutputUsersWithRolesDto[] = [];
  error: string | null = null;

  constructor(private weatherService: WeatherBackendModel) { }

  ngOnInit(): void {
    this.getAllUsersWithRoles();
  }

  getAllUsersWithRoles() {
    this.weatherService.admin_GetAllUsers().subscribe({
      next: data => {
        this.users = data;
        this.error = null;
      },
      error: err => {
        console.log(err);

        this.error = 'Benutzer können nicht ermittelt werden.';
      }
    });
  }

  deleteUserByMail(email: string) {
    this.weatherService.admin_DeleteUser(email).subscribe(x => {
      this.getAllUsersWithRoles();
    })
  }
}
