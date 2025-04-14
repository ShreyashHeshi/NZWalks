import { HttpEvent, HttpHandlerFn, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

export const jwtInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const router = inject(Router);
  const cookieService = inject(CookieService);

  const token = cookieService.get('jwtToken'); // Retrieve JWT from cookies
  console.log('Token from interceptor:', token);

  // Clone request and attach Authorization header if token exists
  const authReq = token
    ? req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    })
    : req;

  return next(authReq).pipe(
    catchError((error) => {
      if (error.status === 401) {
        console.error("Unauthorized! Redirecting to login...");
        router.navigate(['/login']);
      }
      throw error;
    })
  );
};
