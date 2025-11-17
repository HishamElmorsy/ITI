import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DepartmentDto } from '../../interfaces/api-department';
import { StudentPayload } from '../../interfaces/api-student';
import { DepartmentService } from '../../services/department-service';
import { StudentService } from '../../services/student-service';

@Component({
  selector: 'app-update-student',
  imports: [FormsModule, CommonModule],
  templateUrl: './update-student.html',
  styleUrl: './update-student.css',
})
export class UpdateStudent implements OnInit {
  studentId = 0;
  studentForm: StudentPayload = {
    StFname: '',
    StLname: '',
    StAge: undefined,
    StAddress: '',
    DeptId: undefined,
    StSuper: undefined,
  };

  departments: DepartmentDto[] = [];
  isLoading = false;
  isSubmitting = false;
  errorMessage = '';

  constructor(
    private studentService: StudentService,
    private deptService: DepartmentService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.studentId = Number(this.route.snapshot.paramMap.get('id'));
    if (!this.studentId) {
      this.errorMessage = 'Invalid student id.';
      return;
    }

    this.loadDepartments();
    this.loadStudent();
  }

  updateStudent(): void {
    this.errorMessage = '';
    if (!this.studentForm.StFname) {
      this.errorMessage = 'First name is required.';
      return;
    }

    this.isSubmitting = true;
    this.studentService.updateStudent(this.studentId, this.studentForm).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/student/get-student']);
      },
      error: () => {
        this.isSubmitting = false;
        this.errorMessage = 'Unable to update student.';
      },
    });
  }

  private loadDepartments(): void {
    this.deptService.getDepartments().subscribe({
      next: (depts) => (this.departments = depts),
      error: () => (this.errorMessage = 'Failed to load departments.'),
    });
  }

  private loadStudent(): void {
    this.isLoading = true;
    this.studentService.getStudentById(this.studentId).subscribe({
      next: (student) => {
        this.studentForm = {
          StId: student.StId,
          StFname: student.StFname ?? '',
          StLname: student.StLname,
          StAge: student.StAge,
          StAddress: student.StAddress,
          DeptId: student.DeptId,
          StSuper: student.StSuper,
        };
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Student not found.';
        this.isLoading = false;
      },
    });
  }
}
