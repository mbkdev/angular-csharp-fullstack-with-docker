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

        var res = this.weatherService.user_LoginUser(inputLoginUserDto);



        var res1 = res.pipe(tap(res => {
            console.log("res1-result: " + res1);


            if (res) {
                console.log("res-result: " + res);
                localStorage.setItem('jwt', res);
            }
        }))

        return res1;
    }

    logout() {
        localStorage.removeItem('jwt');
    }

    isLoggedIn(): boolean {
        return !!localStorage.getItem('jwt');
    }

    getRole(): string | null {
        const token = this.getToken();

        
        if (!token) return null;
        
        const decoded: any = jwtDecode(token);
        const role_key = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        
        console.log(decoded);
        

        return decoded[role_key] || null;
    }

    getToken(): string | null {
        return localStorage.getItem('jwt');
    }
}