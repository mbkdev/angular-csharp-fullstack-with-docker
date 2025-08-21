import { Component, inject, input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputLoginUserDto, WeatherBackendModel } from '../models/weatherBackendModel';
import { AuthService } from '../authentication/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './user-login.html',
  styleUrl: './user-login.scss'
})
export class UserLogin {
  username?: string;
  password?: string;
  errorMessage: string = '';

  private router = inject(Router);


  constructor(private weatherService: WeatherBackendModel, private auth: AuthService) { }

  loginUser() {
    var inputLoginUserDto = new InputLoginUserDto();
    inputLoginUserDto.email = this.username;
    inputLoginUserDto.password = this.password;
    inputLoginUserDto.username = this.username;

    console.log(inputLoginUserDto);


    this.auth.login(this.username!, this.password!).subscribe({
      next: () => {
        this.router.navigate(['/user/overview']); // nach Logout zurück zur Login-Seite
      },
      error: err => {
        if(err.status === 401){
          this.errorMessage = "Benutzername oder Passwort falsch!";
        }else{
          this.errorMessage = "Ein unbekannter Feheler ist aufgetreten!";
        }
      }
    });



    // this.weatherService.user_LoginUser(inputLoginUserDto).subscribe({
    //   next: (res: any) => {
    //     console.log(res);
    //   },
    //   error: err => {
    //     console.log(err);
    //   }
    // })
  }

  logoutUser() {
    this.weatherService.user_LogoutUser();
  }
}
