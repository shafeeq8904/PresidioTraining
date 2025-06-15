import { importProvidersFrom, Injectable } from '@angular/core';
import { User } from '../app/models/user.model';


@Injectable({
  providedIn: 'root'
})
export class AuthService  {

  private dummyUsers: User[] = [
    { username: 'admin', password: 'admin123' },
    { username: 'user', password: 'user123' }
  ];

  // login(user: User): boolean {
  //   const found = this.dummyUsers.find(
  //     u => u.username === user.username && u.password === user.password
  //   );
  //   if (found) {
  //     sessionStorage.setItem('user', JSON.stringify(found)); 
  //     return true;
  //   }
  //   return false;
  // }

  // getLoggedInUser(): User | null {
  //   const userJson = sessionStorage.getItem('user');
  //   return userJson ? JSON.parse(userJson) : null;
  // }

  // logout(): void {
  //   sessionStorage.removeItem('user');
  // }

  login(user: User): boolean {
    const found = this.dummyUsers.find(
      u => u.username === user.username && u.password === user.password
    );
    if (found) {
      localStorage.setItem('user', JSON.stringify(found)); 
      return true;
    }
    return false;
  }

  getLoggedInUser(): User | null {
    const userJson = localStorage.getItem('user');
    return userJson ? JSON.parse(userJson) : null;
  }

  logout(): void {
    localStorage.removeItem('user');
  }


}
