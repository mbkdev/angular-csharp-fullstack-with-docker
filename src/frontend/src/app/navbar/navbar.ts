import { Component, inject } from '@angular/core';
import { AuthService } from '../authentication/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule], // [CommonModule, HttpClientModule, RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.scss'
})
export class Navbar {
  constructor(private auth: AuthService) { }

  private router = inject(Router);

  isLoggedIn(): boolean {
    return this.auth.isLoggedIn();
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']); // nach Logout zurück zur Login-Seite
  }
}
