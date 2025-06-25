import { Component, OnInit } from '@angular/core';
import { TaskItemResponseDto } from '../create-task/task.types';
import { TaskService } from '../create-task/task.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { TaskEditModalComponent } from '../task-edit-modal/task-edit-modal';
import { TaskUploadModalComponent } from '../task-upload/task-upload-modal.component';
import { TaskFilesModalComponent } from '../view-files/task-files-modal.component';
import { TaskStatusLogModalComponent } from '../task-status-log-modal/task-status-log-modal.component';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  imports: [CommonModule,TaskEditModalComponent,TaskUploadModalComponent,TaskFilesModalComponent,TaskStatusLogModalComponent,FormsModule],
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  tasks: TaskItemResponseDto[] = [];
  loading = false;
  selectedFilter: string = 'All';
  filteredTasks: TaskItemResponseDto[] = [];  
  selectedTaskToEdit?: TaskItemResponseDto; 
  showUploadModal = false;
  selectedTaskId = '';
  selectedTaskToUpload?: TaskItemResponseDto;
  currentUser: any;
  showFilesModal = false;
  showLogModal = false;
  selectedTaskIdForLogs = '';
  searchTitle: string = '';
  searchDueDate: string = '';



  constructor(private taskService: TaskService, private toastr: ToastrService) {}

  ngOnInit(): void {
    const userData = sessionStorage.getItem('user');
    if (userData) {
        this.currentUser = JSON.parse(userData);
    }
    this.fetchTasks();
    
  }

  getInitials(name: string): string {
  const words = name.trim().split(' ');
  return words.map(word => word[0]).join('').toUpperCase();
}


deleteTask(id: string) {
  if (confirm("Are you sure you want to delete this task?")) {
    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.toastr.success("Task deleted successfully");
        this.fetchTasks(); // Refresh task list
      },
      error: (err) => {
        this.toastr.error(err?.error?.message || "Failed to delete task");
      }
    });
  }
}

openEdit(task: TaskItemResponseDto): void {
  this.selectedTaskToEdit = task;
  document.body.classList.add('modal-open');
}

closeEdit(): void {
  this.selectedTaskToEdit = undefined;
   document.body.classList.remove('modal-open');
  this.fetchTasks(); // refresh list
}


openFilesModal(taskId: string) {
  this.selectedTaskId = taskId;
  this.showFilesModal = true;
  document.body.classList.add('modal-open');
}

closeFilesModal(): void {
  this.showFilesModal = false;
  this.selectedTaskId = '';
  document.body.classList.remove('modal-open');
}


openUploadModal(task: TaskItemResponseDto) {
  this.selectedTaskToUpload = task;
  document.body.classList.add('modal-open');
  this.showUploadModal = true;
}

closeUploadModal(): void {
  this.showUploadModal = false;
  this.selectedTaskToUpload = undefined;
  document.body.classList.remove('modal-open');
}


openLogModal(taskId: string): void {
  this.selectedTaskIdForLogs = taskId;
  this.showLogModal = true;
  document.body.style.overflow = 'hidden';
}

closeLogModal(): void {
  this.showLogModal = false;
  this.selectedTaskIdForLogs = '';
  document.body.style.overflow = ''; // Restore scrolling
}

searchTasks(): void {
  this.loading = true;
  this.taskService.searchTasks(this.searchTitle, this.searchDueDate).subscribe({
    next: (res) => {
      this.filteredTasks = res.data || [];
      this.selectedFilter = 'All';
      this.loading = false;
    },
    error: (err) => {
      this.filteredTasks = [];
      this.toastr.error(err?.error?.message || 'Search failed');
      this.loading = false;
    }
  });
}

clearSearch(): void {
  this.searchTitle = '';
  this.searchDueDate = '';
  this.fetchTasks(); // reload original list
}

 filterTasks(state: string) {
  this.selectedFilter = state;
  this.filteredTasks = state === 'All'
    ? this.tasks
    : this.tasks.filter(t => t.status === state);
}

  fetchTasks() {
    this.loading = true;
    this.taskService.getAllTasks().subscribe({
      next: (res) => {
        this.tasks = res.data;
        this.filterTasks(this.selectedFilter);
        this.loading = false;
      },
      error: (err) => {
        this.toastr.error(err?.error?.message || 'Failed to load tasks');
        this.loading = false;
      }
    });
  }
}
