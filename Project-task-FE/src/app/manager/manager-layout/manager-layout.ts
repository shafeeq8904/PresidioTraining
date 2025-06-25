import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ManagerNavbarComponent } from '../manager-navbar/manager-navbar';

@Component({
  selector: 'app-manager-layout',
  standalone: true,
  imports: [RouterOutlet, ManagerNavbarComponent],
  templateUrl: './manager-layout.html',
  styleUrl: './manager-layout.css'
})
export class ManagerLayoutComponent {}
