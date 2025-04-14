// navbar.component.ts
import { Component } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.component.html'
})
export class NavbarComponent {
  constructor(private router: Router) { }

  logout() {
    try {
      // Add your actual logout logic here
      console.log('Logout initiated'); // Test if this appears in console

      // Example logout flow:
      // 1. Clear authentication tokens
      // 2. Navigate to login
      this.router.navigate(['/login'])
        .then(() => console.log('Navigation successful'))
        .catch(err => console.error('Navigation failed:', err));

    } catch (error) {
      console.error('Logout error:', error);
    }
  }
}