import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { LoginService } from '../../services/login.service';
import { LoginRequestDto } from './LoginRequestDto';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CookieService } from 'ngx-cookie-service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  message: string = ""; // Define message property

  loginForm = new FormGroup({
    userName: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.email,
        Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$') // Ensures email format
      ]
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.minLength(6)] // Min length 6 characters
    }),
  });

  constructor(
    private loginService: LoginService,
    private cookieService: CookieService,
    private router: Router,
    private toastr: ToastrService
  ) { }

  login(): void {
    if (this.loginForm.invalid) return; // Prevent submission if form is invalid

    const user: LoginRequestDto = {
      userName: this.loginForm.value.userName || '',
      password: this.loginForm.value.password || '',
    };

    console.log("User on console", user);

    this.loginService.login(user).subscribe({
      next: (response) => {
        console.log("Login API Response:", response); // Debugging

        const JwtToken = response.jwtToken; // ✅ Match exactly with backend DTO

        if (!JwtToken) {
          console.error("JWT Token is missing in response!");
          this.toastr.error("Login failed: No token received.", "Error");
          return;
        }

        // Store JWT Token in cookies with security settings
        this.cookieService.set('jwtToken', JwtToken, { path: '/', secure: true, sameSite: 'Lax' });

        console.log("Stored JWT in cookies:", this.cookieService.get('jwtToken')); // Verify token storage

        this.toastr.success('Login successful!', 'Success');
        this.router.navigate(['/home']);
        this.loginForm.reset();
      },
      error: (error) => {
        this.message = 'Login failed!';
        this.toastr.error('Invalid credentials, please try again.', 'Error');
        console.error("Login error:", error);
      }
    });
  }

  get userName() {
    return this.loginForm.get('userName');
  }

  get password() {
    return this.loginForm.get('password');
  }
}
