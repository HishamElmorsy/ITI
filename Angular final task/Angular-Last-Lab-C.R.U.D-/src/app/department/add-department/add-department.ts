import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DepartmentPayload } from '../../interfaces/api-department';
import { DepartmentService } from '../../services/department-service';

@Component({
  selector: 'app-add-department',
  imports: [FormsModule, CommonModule],
  templateUrl: './add-department.html',
  styleUrl: './add-department.css',
})
export class AddDepartment {
  newDept: DepartmentPayload = {
    DeptName: '',
    DeptDesc: '',
    DeptLocation: '',
    DeptManager: undefined,
  };

  isSubmitting = false;
  errorMessage = '';

  constructor(private deptService: DepartmentService, private router: Router) {}

  addDepartment(): void {
    this.errorMessage = '';
    if (!this.newDept.DeptName) {
      this.errorMessage = 'Department name is required.';
      return;
    }

    this.isSubmitting = true;
    this.deptService.addDepartment(this.newDept).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/department/get-department']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Failed to add department.';
      },
    });
  }
}
