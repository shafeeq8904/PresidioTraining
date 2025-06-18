import { Component, signal, computed, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService } from '../services/user.service';
import { User } from '../models/user.model';
import { FormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { ChartData, ChartType ,ChartOptions} from 'chart.js';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, NgChartsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  private userService = inject(UserService);
  users = signal<User[]>([]);
  filterTerm = signal<string>('');
  objectKeys = Object.keys;

  filteredUsers = computed(() =>
  this.users().filter(u =>
    u.firstName?.toLowerCase().includes(this.filterTerm().toLowerCase()) ||
    u.lastName?.toLowerCase().includes(this.filterTerm().toLowerCase()) ||
    u.gender?.toLowerCase().includes(this.filterTerm().toLowerCase()) ||
    u.state?.toLowerCase().includes(this.filterTerm().toLowerCase()) ||
    u.role?.toLowerCase().includes(this.filterTerm().toLowerCase())
  )
);

  genderStats = computed(() => {
    const male = this.filteredUsers().filter(u => u.gender === 'male').length;
    const female = this.filteredUsers().filter(u => u.gender === 'female').length;
    return { male, female };
  });

  genderChartData = computed<ChartData<'pie'>>(() => ({
    labels: ['Male', 'Female'],
    datasets: [
      {
        data: [this.genderStats().male, this.genderStats().female],
        backgroundColor: ['#36A2EB', '#FF6384']
      }
    ]
  }));

  genderChartType: ChartType = 'pie';

  roleStats = computed(() => {
    const counts: Record<string, number> = {};
    for (const user of this.filteredUsers()) {
      const role = user.role || 'user';
      counts[role] = (counts[role] || 0) + 1;
    }
    return counts;
  });

  roleChartLabels = computed(() => Object.keys(this.roleStats()));

  roleChartData = computed(() => ({
    labels: this.roleChartLabels(),
    datasets: [
      {
        label: 'Users by Role',
        data: this.roleChartLabels().map(role => this.roleStats()[role]),
        backgroundColor: '#4CAF50'
      }
    ]
  }));

  roleChartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
      x: {},
      y: { beginAtZero: true }
    }
  };

  roleChartType: ChartType = 'bar';


  stateStats = computed(() => {
    const states: Record<string, number> = {};
    for (const user of this.filteredUsers()) {
      const state = user.state || 'Unknown';
      states[state] = (states[state] || 0) + 1;
    }
    return states;
  });

  stateChartLabels = computed(() => Object.keys(this.stateStats()));

  stateChartData = computed(() => ({
    labels: this.stateChartLabels(),
    datasets: [
      {
        label: 'Users by State',
        data: this.stateChartLabels().map(state => this.stateStats()[state]),
        backgroundColor: '#FF9800' 
      }
    ]
  }));

  stateChartOptions: ChartOptions = {
    responsive: true,
    indexAxis: 'y', 
    maintainAspectRatio: false,
    scales: {
      x: { beginAtZero: true },
      y: {}
    }
  };

  stateChartType: ChartType = 'bar';


  ngOnInit() {
    this.userService.getUsers().subscribe((data: any) => {
      const mappedUsers = data.users.map((user: any) => ({
        ...user,
        state: user?.address?.state || 'Unknown',
        role: user?.role || 'user'
      }));
      this.users.set(mappedUsers);
    });
  }
}
