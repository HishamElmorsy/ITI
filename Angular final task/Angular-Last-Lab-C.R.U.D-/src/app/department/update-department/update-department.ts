import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DepartmentPayload } from '../../interfaces/api-department';
import { DepartmentService } from '../../services/department-service';

@Component({
  selector: 'app-update-department',
  imports: [FormsModule, CommonModule],
  templateUrl: './update-department.html',
  styleUrl: './update-department.css',
})
export class UpdateDepartment implements OnInit {
  deptId = 0;
  deptForm: DepartmentPayload = {
    DeptName: '',
    DeptDesc: '',
    DeptLocation: '',
    DeptManager: undefined,
  };

  isLoading = false;
  isSubmitting = false;
  errorMessage = '';

  constructor(
    private deptService: DepartmentService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.deptId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.deptId) {
      this.loadDepartment();
    } else {
      this.errorMessage = 'Invalid department id.';
    }
  }

  updateDepartment(): void {
    this.errorMessage = '';
    if (!this.deptForm.DeptName) {
      this.errorMessage = 'Name is required.';
      return;
    }

    this.isSubmitting = true;
    this.deptService.updateDepartment(this.deptId, this.deptForm).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/department/get-department']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Failed to update department.';
      },
    });
  }

  private loadDepartment(): void {
    this.isLoading = true;
    this.deptService.getDepartmentById(this.deptId).subscribe({
      next: (dept) => {
        this.deptForm = {
          DeptId: dept.DeptId,
          DeptName: dept.DeptName,
          DeptDesc: dept.DeptDesc,
          DeptLocation: dept.DeptLocation,
          DeptManager: dept.DeptManager,
        };
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Department not found.';
        this.isLoading = false;
      },
    });
  }
}
