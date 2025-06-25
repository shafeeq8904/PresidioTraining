// src/app/auth/auth.guard.ts
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from './auth.service';
import { CanActivateFn } from '@angular/router';

export const authGuard = (allowedRoles: string[] = []): CanActivateFn => {
  return () => {
    const auth = inject(AuthService);
    const router = inject(Router);
    const toastr = inject(ToastrService);

    // Not logged in
    if (!auth.isAuthenticated()) {
      router.navigate(['/login']);
      return false;
    }

    const user = auth.getUser();
    
    // Logged in but not allowed
    if (!user || !allowedRoles.includes(user.role)) {
      toastr.error("Access Denied: You don't have permission to access this page.", 'Unauthorized');
        if (user?.role === 'Manager') 
        {
            router.navigate(['/manager']);
        } else if (user?.role === 'TeamMember') {
            router.navigate(['/team']);
        } else {
            router.navigate(['/login']);
      }
      return false;
    }

    return true;
  };
};
