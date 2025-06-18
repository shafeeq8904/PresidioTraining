// src/app/app.routes.ts
import { Routes } from '@angular/router';
import { UserListComponent } from './user-list/user-list'; // adjust the path
import { DashboardComponent } from './dashboard/dashboard';
import { AddUserComponent } from './add-user/add-user';

export const routes: Routes = [
  {
    path: '',
    component: UserListComponent
  },
  {
    path: 'dashboard',
    component:DashboardComponent
  }
  ,
  {
    path: 'add-user',
    component:AddUserComponent
  }
];
