import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  username = '';
  password = '';
  error = false;

  constructor(private auth: AuthService, private router: Router) {}

  login() {
    if (this.auth.login(this.username, this.password)) {
      this.router.navigate(['/products']);
    } else {
      this.error = true;
    }
  }
}
