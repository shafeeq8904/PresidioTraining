import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login';
import { ManagerDashboard } from './manager/manager-dashboard/manager-dashboard';
import { TeamDashboard } from './team/team-dashboard/team-dashboard';
import { authGuard } from './auth/auth.guard';
import { ManagerLayoutComponent } from './manager/manager-layout/manager-layout';
import { CreateUserComponent } from './manager/Create-User/create-user.component';
import { UserListComponent } from './manager/user-list/user-list';
import { CreateTaskComponent } from './manager/create-task/create-task';
import { TaskListComponent } from './manager/task-list/task-list.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'manager',
    component:ManagerLayoutComponent,
    canActivate: [authGuard(['Manager'])],
    children: [
    { path: 'dashboard', component: ManagerDashboard },
    { path: 'users/create',component:CreateUserComponent},
    { path: 'users',component:UserListComponent},
    { path: 'task/create',component:CreateTaskComponent},
    { path: 'tasks',component:TaskListComponent},
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
  ]
    
  },
  {
    path: 'team',
    canActivate: [authGuard(['TeamMember'])],
    component:TeamDashboard
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];