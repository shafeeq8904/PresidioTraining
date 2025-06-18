import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user.model';
import { Observable ,map } from 'rxjs';
import { CommonModule } from '@angular/common';

@Injectable({ providedIn: 'root' })
export class UserService {
  private baseUrl = 'https://dummyjson.com/users';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<any> {
    return this.http.get(`${this.baseUrl}`);
  }

  getUsersList(): Observable<User[]> {
  return this.http.get<any>(this.baseUrl).pipe(
    map(res =>
      res.users.map((user: any) => ({
        id: user.id,
        image:user.image,
        firstName: user.firstName,
        lastName: user.lastName,
        gender: user.gender,
        role: user.company?.title || 'User',
        state: user.address?.state || 'Unknown'
      }))
    )
  );
}

  addUser(user: User): Observable<any> {
    return this.http.post(`${this.baseUrl}/add`, user);
  }
}


