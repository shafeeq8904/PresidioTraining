import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { TaskItemUpdateDto, TaskItemResponseDto, TaskState } from '../create-task/task.types';
import { TaskService } from '../create-task/task.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserService } from '../Create-User/user.service';
import { AbstractControl, ValidationErrors } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { futureDateValidator } from '../date';

@Component({
  selector: 'app-task-edit-modal',
  templateUrl: './task-edit-modal.html',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  styleUrls: ['./task-edit-modal.css']
})
export class TaskEditModalComponent implements OnInit {
  @Input() task!: TaskItemResponseDto;
  @Output() close = new EventEmitter<void>();
  @Output() taskUpdated = new EventEmitter<void>();

  form!: FormGroup;
  loading = false;
  taskState = TaskState;
  users: any[] = []; // Add this

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private userService: UserService,
    private toastr: ToastrService
  ) {}

ngOnInit(): void {
  const safeDueDate = this.task.dueDate
  ? new Date(this.task.dueDate).toISOString().substring(0, 10)
  : ''; 
  
    this.form = this.fb.group({
    title: [this.task.title, Validators.required],
    description: [this.task.description, Validators.required],
    status: [this.task.status, Validators.required],
    dueDate: [safeDueDate, [Validators.required, futureDateValidator]],

    assignedToId: [this.task.assignedToId, Validators.required]

  });

  this.loadUsers();
}
  loadUsers(): void {
  this.userService.getAllUsers().subscribe({
    next: (res) => {
      const allUsers = res.data || res; 
      this.users = allUsers.filter((u: any) => u.role === 'TeamMember');
    },
    error: (err) => {
      console.error('Failed to fetch users:', err);
    }
  });
  }

  submit(): void {
    const dto: TaskItemUpdateDto = this.form.value;
    console.log('Submitting DTO:', dto); 
    this.loading = true;
    this.taskService.updateTask(this.task.id, dto).subscribe({
      next: () => {
        this.toastr.success('Task updated successfully');
        this.taskUpdated.emit();
        this.close.emit();
        this.loading = false;
      },
      error: (err) => {
        this.toastr.error(err?.error?.message || 'Update failed');
        console.error('Update failed', err);
        this.loading = false;
      }
    });
  }
}

