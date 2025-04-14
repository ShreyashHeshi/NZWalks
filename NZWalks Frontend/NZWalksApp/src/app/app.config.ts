import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core'; // ApplicationConfig is an interface introduced in Angular 15
// It is primarily used for standalone applications,also gives providers
// Angular uses Zone.js to detect changes when asynchronous events occur
import { provideRouter } from '@angular/router';
import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { jwtInterceptor } from './auth/jwt.intercepter';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes), provideHttpClient(
    withInterceptors([jwtInterceptor])
  ), provideAnimations(), provideToastr()]
};
