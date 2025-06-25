import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../Create-User/user.service';
import { UserResponseDto } from '../Create-User/user.types';
import { FormsModule } from '@angular/forms';
import { PagedResponse } from '../Create-User/user.types';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { EditUserModalComponent } from "../EditUserModalComponent/edit-user-modal.component";

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, FormsModule, EditUserModalComponent],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css'],
})
export class UserListComponent implements OnInit {
  users: UserResponseDto[] = [];
  loading = false;
  searchTerm: string = '';
  roleFilter = '';
  showEditModal = false;
  selectedUser?: UserResponseDto;

  // Pagination
  page = 1;
  pageSize = 5;
  totalPages = 0;

  constructor(private userService: UserService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.fetchUsers();
  }

  setRoleFilter(role: string) {
  this.roleFilter = role;
}

getPageRange(): number[] {
  const range: number[] = [];
  for (let i = 1; i <= this.totalPages; i++) {
    range.push(i);
  }
  return range;
}



 filteredUsers(): UserResponseDto[] {
  const term = this.searchTerm.toLowerCase();

  return this.users.filter(user => {
    const matchesSearch = user.fullName.toLowerCase().includes(term) || user.email.toLowerCase().includes(term);
    const matchesRole = !this.roleFilter || user.role === this.roleFilter;
    return matchesSearch && matchesRole;
  });
}

  fetchUsers() {
  this.loading = true;

  this.userService.getAllUsers(this.page, this.pageSize).subscribe({
    next: res => {
      console.log('API Response:', res);
      console.log('res.data type:', typeof res.data);
      console.log('res.data instanceof Array:', res.data instanceof Array);
      
      this.users = res.data; // This line is fine IF data is an array
      this.totalPages = res.pagination.totalPages;
      this.loading = false;
    },
    error: err => {
      this.toastr.error(err?.error?.message || 'Failed to load users');
      this.loading = false;
    }
  });
}

  getInitials(name: string): string {
    const parts = name.split(' ');
    const first = parts[0]?.charAt(0).toUpperCase() || '';
    const last = parts[1]?.charAt(0).toUpperCase() || '';
    return first + last;
  }

  deleteUser(id: string) {
     if (!confirm('Are you sure you want to delete this task?')) return;
    this.userService.deleteUser(id).subscribe({
      next: () => {
        this.toastr.success('User deleted');
        this.fetchUsers();
      },
      error: (err: HttpErrorResponse) => {
        this.toastr.error(err.error?.message || 'Delete failed');
      }
    });
  }

  goToPage(page: number) {
    this.page = page;
    this.fetchUsers();
  }

  
openEditModal(user: UserResponseDto) {
  this.selectedUser = user;
  this.showEditModal = true;
  document.body.style.overflow = 'hidden'; 
}

onModalClose() {
  this.showEditModal = false;
  this.selectedUser = undefined;
  document.body.style.overflow = 'auto'; 
}

onUserUpdated() {
  this.fetchUsers();
}
}
