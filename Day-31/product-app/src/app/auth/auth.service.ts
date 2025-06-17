import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  login(username: string, password: string): boolean {
    if (username === 'admin' && password === 'admin') {
      localStorage.setItem('token', 'mock-token');
      return true;
    }
    return false;
  }

  logout() {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}