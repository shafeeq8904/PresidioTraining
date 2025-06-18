import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../services/user.service';
import { User } from '../models/user.model';

@Component({
  selector: 'app-add-user',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-user.html',
  styleUrl: './add-user.css'
})
export class AddUserComponent {
  user: Partial<User> = {
    firstName: '',
    lastName: '',
    gender: 'male',
    role: '',
    state: ''
  };

  submitted = false;
  constructor(private userService: UserService) {}

  addUser() {
    this.userService.addUser(this.user as User).subscribe(res => {
      console.log('User added:', res);
      this.submitted = true;
    });
  }
}
