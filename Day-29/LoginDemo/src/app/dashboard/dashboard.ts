import { Component ,OnInit } from '@angular/core';
import { User } from '../models/user.model';
import { AuthService } from '../auth'

@Component({
  selector: 'app-dashboard',
  imports: [],
  template: `
    <h2>Welcome, {{ user?.username }}</h2>
    <button (click)="logout()">Logout</button>`,
    styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  user: User | null = null;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.user = this.authService.getLoggedInUser();
  }

  logout() {
    this.authService.logout();
    window.location.href = '/login';
  }
}
