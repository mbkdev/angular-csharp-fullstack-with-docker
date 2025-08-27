import { Observable, tap } from "rxjs";
import { InputLoginUserDto, WeatherBackendModel } from "../models/weatherBackendModel";
import { Injectable } from "@angular/core";
import { jwtDecode } from 'jwt-decode';

@Injectable({ providedIn: 'root' })

export class AuthService {
    constructor(private weatherService: WeatherBackendModel) { }

    login(username: string, password: string): Observable<any> {

        var inputLoginUserDto = new InputLoginUserDto();
        inputLoginUserDto.email = username;
        inputLoginUserDto.username = username;
        inputLoginUserDto.password = password;

        var loginResponse = this.weatherService.user_LoginUser(inputLoginUserDto);
        var authenticationResult = loginResponse.pipe(tap(res => {
            if (res) {
                localStorage.setItem('jwt', res);
            }
        }))

        return authenticationResult;
    }

    logout() {
        localStorage.removeItem('jwt');
    }

    isLoggedIn(): boolean {
        return !!localStorage.getItem('jwt');
    }

    isAdministrator(): boolean {
        return this.hasRole("Admin");
    }

    hasRole(role: string): boolean {
        var roles = this.getRole();

        return roles!.includes(role);
    }

    getRole(): string | null {
        const token = this.getToken();

        if (!token) return null;

        try {
            const decoded: any = jwtDecode(token);
            const roles = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

            var roleArray = [];
            var isArray = Array.isArray(roles);
            if (isArray) {
                roleArray = roles;
            } else {
                roleArray = [roles];
            }
        } catch (e) {
            console.error(e);
        }
        
        return roleArray;
    }

    getToken(): string | null {
        return localStorage.getItem('jwt');
    }
}