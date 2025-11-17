import { Component, OnInit } from '@angular/core';
import { DepartmentService } from '../../services/department-service';
import { Router, RouterLink } from '@angular/router';
import { DepartmentDto } from '../../interfaces/api-department';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-get-department',
  imports: [RouterLink, CommonModule],
  templateUrl: './get-department.html',
  styleUrl: './get-department.css',
})
export class GetDepartment implements OnInit {
  departments: DepartmentDto[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(private deptService: DepartmentService, private router: Router) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  showDetails(dept: DepartmentDto): void {
    this.router.navigate(['/department/details', dept.DeptId]);
  }

  private loadDepartments(): void {
    this.isLoading = true;
    this.deptService.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load departments.';
        this.isLoading = false;
      },
    });
  }
}
