import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { RegisterComponent } from './components/register/register.component';

import { NavbarComponent } from './components/navbar/navbar.component';
import { LoginComponent } from './components/login/login.component';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RegisterComponent, LoginComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'NZWalksApp';
}
