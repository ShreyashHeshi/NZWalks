import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { RegisterRequestDto } from '../../components/register/RegisterRequestDto';
import { RouterModule, Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule, ReactiveFormsModule],
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    registerForm: FormGroup;
    availableRoles: string[] = [];
    message: string = '';
    isLoadingRoles: boolean = false;
    isLoadingSubmit: boolean = false;

    constructor(
        private authService: AuthService,
        private fb: FormBuilder,
        private router: Router,
        private toastr: ToastrService
    ) {
        this.registerForm = this.fb.group({
            userName: ['', [Validators.required, Validators.email]],
            password: ['', [Validators.required, Validators.minLength(6)]],
            selectedRole: ['', Validators.required]
        });
    }

    ngOnInit(): void {
        this.loadRoles();
    }

    get userName() { return this.registerForm.get('userName'); }
    get password() { return this.registerForm.get('password'); }
    get selectedRole() { return this.registerForm.get('selectedRole'); }

    loadRoles(): void {
        this.isLoadingRoles = true;
        this.authService.getAvailableRoles().pipe(
            finalize(() => this.isLoadingRoles = false)
        ).subscribe({
            next: (roles) => {
                this.availableRoles = roles;
            },
            error: (error) => {
                console.error('Failed to load roles', error);
                this.message = 'Failed to load available roles. Please refresh the page.';
            }
        });
    }

    register(): void {
        if (this.registerForm.invalid) {
            this.registerForm.markAllAsTouched();
            return;
        }

        this.isLoadingSubmit = true;
        const user: RegisterRequestDto = {
            userName: this.registerForm.value.userName,
            password: this.registerForm.value.password,
            roles: [this.registerForm.value.selectedRole]
        };

        this.authService.register(user).pipe(
            finalize(() => this.isLoadingSubmit = false)
        ).subscribe({
            next: () => {
                this.toastr.success('User registered successfully!', 'Success');
                this.registerForm.reset();
                this.router.navigate(['/login']);
            },
            error: (error) => {
                this.toastr.error(error.error?.message || 'Registration failed. Please try again.', 'Error');
                console.error(error);
            }
        });
    }
}