import { Routes } from '@angular/router';
import { Weather } from './weather-list/weather-list';
import { PageNotFound } from './page-not-found/page-not-found';
import { WeatherCreate } from './weather-create/weather-create';
import { WeatherEdit } from './weather-edit/weather-edit';
import { UserLogin } from './user-login/user-login';
import { UserProfile } from './user-profile/user-profile';
import { authGuard } from './authentication/auth.guard';
import { UserOverview } from './user-overview/user-overview';
import { AdminUserOverview } from './admin-user-overview/admin-user-overview';
import { AdminGuard } from './authentication/admin.guard';
import { AdminUserCreate } from './admin-user-create/admin-user-create';
import { AdminUserEdit } from './admin-user-edit/admin-user-edit';

export const routes: Routes = [
    { path: 'weather', component: Weather },
    { path: 'weather/create', component: WeatherCreate },
    { path: 'weather/edit/:id', component: WeatherEdit },
    { path: 'login', component: UserLogin },
    { path: 'user/profile', component: UserProfile, canActivate: [authGuard] },
    { path: 'user/overview', component: UserOverview, canActivate: [authGuard] },
    { path: 'admin/users', component:AdminUserOverview, canActivate: [AdminGuard] },
    { path: 'admin/users/create', component:AdminUserCreate, canActivate: [AdminGuard] },
    { path: 'admin/users/edit', component:AdminUserEdit, canActivate: [AdminGuard] },
    { path: '**', component: PageNotFound }
];
