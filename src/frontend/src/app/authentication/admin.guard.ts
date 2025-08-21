import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "./auth.service";

@Injectable({ providedIn: 'root' })

export class AdminGuard implements CanActivate {
    constructor(private auth: AuthService, private router: Router) {}

    canActivate(): boolean {
        var isLoggedIn = this.auth.isLoggedIn();
        var isAdministrator = this.auth.getRole() === 'Admin';

        return isLoggedIn && isAdministrator;
    }
}