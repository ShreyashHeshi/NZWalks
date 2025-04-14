import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthResponse, LoginRequestDto } from '../components/login/LoginRequestDto';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = "https://localhost:7084/api/Auth/Login"

  constructor(private http: HttpClient) { }

  public login(user: LoginRequestDto): Observable<AuthResponse> {
    const body = {
      username: user.userName,
      password: user.password,
    };

    console.log("final body", body);

    return this.http.post<AuthResponse>(this.apiUrl, body);

    // //.pipe(tap(r => {
    //   if(r && r.jwtToken)
    //   {
    //     console.log(r.jwtToken);

    //   }
    // }))

  }

}
