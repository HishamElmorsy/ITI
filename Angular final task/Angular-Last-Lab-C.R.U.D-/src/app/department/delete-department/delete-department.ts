import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DepartmentService } from '../../services/department-service';

@Component({
  selector: 'app-delete-department',
  imports: [FormsModule, CommonModule],
  templateUrl: './delete-department.html',
  styleUrl: './delete-department.css',
})
export class DeleteDepartment implements OnInit {
  deptId = 0;
  isDeleting = false;
  errorMessage = '';

  constructor(
    private deptService: DepartmentService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.deptId = Number(this.route.snapshot.paramMap.get('id'));
  }

  deleteDepartment(): void {
    this.errorMessage = '';
    if (!this.deptId) {
      this.errorMessage = 'Invalid department id.';
      return;
    }

    this.isDeleting = true;
    this.deptService.deleteDepartment(this.deptId).subscribe({
      next: () => {
        this.isDeleting = false;
        this.router.navigate(['/department/get-department']);
      },
      error: () => {
        this.isDeleting = false;
        this.errorMessage = 'Unable to delete department.';
      },
    });
  }
}
