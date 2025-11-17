import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DepartmentService } from '../../services/department-service';
import { DepartmentDto } from '../../interfaces/api-department';

@Component({
  selector: 'app-department-details',
  imports: [CommonModule],
  templateUrl: './department-details.html',
  styleUrl: './department-details.css',
})
export class DepartmentDetails implements OnInit {
  dept?: DepartmentDto;
  isLoading = false;
  errorMessage = '';

  constructor(private route: ActivatedRoute, private deptService: DepartmentService) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadDepartment(id);
    } else {
      this.errorMessage = 'Invalid department id.';
    }
  }

  private loadDepartment(id: number): void {
    this.isLoading = true;
    this.deptService.getDepartmentById(id).subscribe({
      next: (dept) => {
        this.dept = dept;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Department not found.';
        this.isLoading = false;
      },
    });
  }
}
