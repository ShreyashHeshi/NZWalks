import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { RegionComponent } from './components/region/region.component';
import { authGuard } from './auth/auth.guard';

export const routes: Routes = [
    {
        path: 'login', component: LoginComponent,
        // Children should be inside an array
    },
    { path: 'register', component: RegisterComponent },
    {
        path: 'region',
        loadComponent: () => import('./components/region/region.component')
            .then(m => m.RegionComponent),
        //canActivate: [authGuard]
    },
    { path: 'home', component: HomeComponent },
    { path: 'region', component: RegionComponent },
    //{ path: 'register', component: RegisterComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' } // Redirect to login by default
];

//     {
//         path: 'login';
//         component: RegisterComponent
//     },
//     { path: 'register', component: RegisterComponent },
//     //{ path: '', redirectTo: '/login', pathMatch: 'full' }
// ];
