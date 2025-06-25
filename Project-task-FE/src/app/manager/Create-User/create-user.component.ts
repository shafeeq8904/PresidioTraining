import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HttpClient } from '@angular/common/http';
import { UserRequestDto } from './user.types';
import { UserService } from './user.service';

@Component({
  selector: 'app-create-user',
  standalone: true,
  templateUrl: './create-user.component.html',
  styleUrls: ['./create-user.component.css'],
  imports: [CommonModule, ReactiveFormsModule],
})
export class CreateUserComponent {
  userForm: FormGroup;
  loading = false;

  roles = ['Manager', 'TeamMember'];

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private toastr: ToastrService,
    private userService: UserService,
    private router: Router
  ) {
    this.userForm = this.fb.group({
      fullName: [''],
      email: [''],
      role: [''],
      password: ['']
    });
  }

  submit() {
  if (this.userForm.invalid) return;

  const dto: UserRequestDto = this.userForm.value as UserRequestDto;
  this.loading = true;

  this.userService.createUser(dto).subscribe({
    next: res => {
      this.toastr.success('User created successfully');
      this.userForm.reset(); // ✅ Clear the form
      this.loading = false;
    },
    error: err => {
      if (err.status === 409) {
        this.toastr.error('Email already exists.');
        this.userForm.get('email')?.setErrors({ conflict: true });
      } else {
        this.toastr.error(err?.error?.message || 'Something went wrong');
      }

      this.loading = false; // ✅ Reset even on error
    }
  });
}

  
}



