// app.routes.ts
import { Routes } from '@angular/router';
import { ProductListComponent } from './products/product-list.component';
import { ProductDetailComponent } from './products/product-detail.component';
import { authGuard } from './auth/auth.guard';
import { LoginComponent } from './auth/login.component';

export const routes: Routes = [
  { path: '', redirectTo: 'products', pathMatch: 'full' },
  {
    path: 'products',
    canActivateChild: [authGuard],
    children: [
      { path: '', component: ProductListComponent },
      { path: ':id', component: ProductDetailComponent },
    ],
  },
  { path: 'login', component: LoginComponent },
];
