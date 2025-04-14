import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RegisterRequestDto } from '../components/register/RegisterRequestDto';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

interface LoginRequestDto {
  userName: string;
  password: string;
}

interface LoginResponseDto {
  jwtToken: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = "https://localhost:7084/api/Auth";
  private tokenSubject = new BehaviorSubject<string | null>(null);
  private tokenExpirationTimer: any;

  constructor(private http: HttpClient, private router: Router) {
    this.initializeAuthState();
  }

  private initializeAuthState(): void {
    const token = localStorage.getItem('jwt_token');
    if (token && !this.isTokenExpired(token)) {
      this.tokenSubject.next(token);
      this.setAutoLogout(this.getTokenExpiration(token));
    }
  }

  register(user: RegisterRequestDto): Observable<any> {
    const body = {
      UserName: user.userName,
      Password: user.password,
      Roles: Array.isArray(user.roles) ? user.roles : [user.roles]
    };
    return this.http.post(`${this.baseUrl}/Register`, body);
  }

  login(credentials: LoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.baseUrl}/Login`, credentials).pipe(
      tap(response => {
        this.storeToken(response.jwtToken);
      })
    );
  }

  getAvailableRoles(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/Roles`);
  }

  storeToken(token: string): void {
    localStorage.setItem('jwt_token', token);
    this.tokenSubject.next(token);
    this.setAutoLogout(this.getTokenExpiration(token));
  }

  getToken(): string | null {
    return this.tokenSubject.value;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token && !this.isTokenExpired(token);
  }

  getUserRoles(): string[] {
    const token = this.getToken();
    if (!token) return [];
    
    try {
      const decoded: any = jwtDecode(token);
      return decoded.roles || [];
    } catch {
      return [];
    }
  }

  logout(): void {
    localStorage.removeItem('jwt_token');
    this.tokenSubject.next(null);
    this.clearAutoLogoutTimer();
    this.router.navigate(['/login']);
  }

  private isTokenExpired(token: string): boolean {
    try {
      const decoded: any = jwtDecode(token);
      return decoded.exp < Date.now() / 1000;
    } catch {
      return true;
    }
  }

  private getTokenExpiration(token: string): number {
    const decoded: any = jwtDecode(token);
    return (decoded.exp * 1000) - Date.now();
  }

  private setAutoLogout(expirationDuration: number): void {
    this.clearAutoLogoutTimer();
    this.tokenExpirationTimer = setTimeout(() => {
      this.logout();
    }, expirationDuration);
  }

  private clearAutoLogoutTimer(): void {
    if (this.tokenExpirationTimer) {
      clearTimeout(this.tokenExpirationTimer);
      this.tokenExpirationTimer = null;
    }
  }
}