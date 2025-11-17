import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DepartmentDto } from '../../interfaces/api-department';
import { StudentPayload } from '../../interfaces/api-student';
import { DepartmentService } from '../../services/department-service';
import { StudentService } from '../../services/student-service';

@Component({
  selector: 'app-add-student',
  imports: [FormsModule, CommonModule],
  templateUrl: './add-student.html',
  styleUrl: './add-student.css',
})
export class AddStudent implements OnInit {
  newStudent: StudentPayload = {
    StFname: '',
    StLname: '',
    StAge: undefined,
    StAddress: '',
    DeptId: undefined,
    StSuper: undefined,
  };

  departments: DepartmentDto[] = [];
  isSubmitting = false;
  errorMessage = '';

  constructor(
    private studentService: StudentService,
    private deptService: DepartmentService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.deptService.getDepartments().subscribe({
      next: (depts) => (this.departments = depts),
      error: () => (this.errorMessage = 'Failed to load departments.'),
    });
  }

  addStudent(): void {
    this.errorMessage = '';
    if (!this.newStudent.StFname) {
      this.errorMessage = 'First name is required.';
      return;
    }

    this.isSubmitting = true;
    this.studentService.addStudent(this.newStudent).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/student/get-student']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Unable to add student.';
      },
    });
  }
}
