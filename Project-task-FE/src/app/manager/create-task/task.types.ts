export enum TaskState {
  ToDo = 'ToDo',
  InProgress = 'InProgress',
  Done = 'Done'
}

export interface TaskItemRequestDto {
  title: string;
  description: string;
  status: TaskState;
  dueDate?: string;
  assignedToId: string;
  
}


export interface TaskStatusLogResponseDto {
  id: string;
  previousStatus: TaskState;
  newStatus: TaskState;
  changedAt: string;
  changedById: string;
  changedByName: string;
}

export interface TaskItemUpdateDto {
    title?: string;
    description?: string;
    status?: TaskState;
    dueDate?: string;
    assignedToId?: string;
}

export interface TaskItemResponseDto {
  id: string;
  title: string;
  description?: string;
  status: TaskState;
  dueDate?: string;

  createdById: string;
  updatedById?: string;
  assignedToId?: string;
  assignedToName?: string;

  createdAt: string;
  updatedAt?: string;
}


export interface TaskFileDto {
  id: string;
  fileName: string;
  uploadedAt: string;
  taskTitle: string;
}