import { Component } from '@angular/core';
import { User } from '../models/user.model';
import { Router } from '@angular/router';
import { AuthService } from '../auth';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-login',
  imports: [CommonModule,FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  user: User = { username: '', password: '' };
  error = false;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    const success = this.authService.login(this.user);
    if (success) {
      this.router.navigate(['/dashboard']);
    } else {
      this.error = true;
    }
  }

}
