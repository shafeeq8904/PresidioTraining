import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-manager-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink,RouterModule],
  templateUrl: './manager-navbar.html',
  styleUrl: './manager-navbar.css'
})
export class ManagerNavbarComponent {
  user = JSON.parse(sessionStorage.getItem('user') || '{}');

  constructor(private auth: AuthService, private router: Router) {}

  isSidebarOpen = false;

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  logout() {
    this.auth.logout();
  }
}
