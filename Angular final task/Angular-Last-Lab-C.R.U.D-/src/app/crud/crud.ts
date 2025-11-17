import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { StudentDto, StudentPayload } from '../interfaces/api-student';
import { StudentService } from '../services/student-service';

@Component({
  selector: 'app-crud',
  imports: [FormsModule, CommonModule],
  templateUrl: './crud.html',
  styleUrl: './crud.css',
})
export class Crud implements OnInit {
  students: StudentDto[] = [];
  searchText = '';
  isLoading = false;
  isSubmitting = false;
  isEditing = false;
  editingId: number | null = null;
  errorMessage = '';
  successMessage = '';

  newStudent: StudentPayload = this.createEmptyStudent();

  constructor(private studentService: StudentService) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  refresh(): void {
    this.loadStudents();
  }

  submitStudent(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.newStudent.StFname?.trim()) {
      this.errorMessage = 'First name is required.';
      return;
    }

    this.isSubmitting = true;
    if (this.isEditing && this.editingId) {
      this.studentService.updateStudent(this.editingId, this.newStudent).subscribe({
        next: () => {
          this.successMessage = 'Student updated successfully.';
          this.resetForm();
          this.loadStudents();
        },
        error: () => {
          this.errorMessage = 'Failed to update student.';
          this.isSubmitting = false;
        },
      });
    } else {
      this.studentService.addStudent(this.newStudent).subscribe({
        next: () => {
          this.successMessage = 'Student added successfully.';
          this.resetForm();
          this.loadStudents();
        },
        error: () => {
          this.errorMessage = 'Failed to add student.';
          this.isSubmitting = false;
        },
      });
    }
  }

  startEdit(student: StudentDto): void {
    this.isEditing = true;
    this.editingId = student.StId;
    this.newStudent = {
      StFname: student.StFname ?? '',
      StLname: student.StLname,
      StAddress: student.StAddress,
      StAge: student.StAge,
      DeptId: student.DeptId,
      StSuper: student.StSuper,
    };
    this.successMessage = '';
    this.errorMessage = '';
  }

  cancelEdit(): void {
    this.resetForm();
  }

  deleteStudent(student: StudentDto): void {
    if (!confirm(`Delete ${student.StFname}?`)) {
      return;
    }
    this.errorMessage = '';
    this.successMessage = '';

    this.studentService.deleteStudent(student.StId).subscribe({
      next: () => {
        this.successMessage = 'Student deleted.';
        this.loadStudents();
      },
      error: () => {
        this.errorMessage = 'Failed to delete student.';
      },
    });
  }

  private loadStudents(): void {
    this.errorMessage = '';
    this.isLoading = true;
    const query = this.searchText.trim() || undefined;
    this.studentService.getStudents(query).subscribe({
      next: (response) => {
        this.students = response.data;
        this.isLoading = false;
        this.isSubmitting = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load students.';
        this.isLoading = false;
        this.isSubmitting = false;
      },
    });
  }

  private resetForm(): void {
    this.isSubmitting = false;
    this.isEditing = false;
    this.editingId = null;
    this.newStudent = this.createEmptyStudent();
  }

  private createEmptyStudent(): StudentPayload {
    return {
      StFname: '',
      StLname: '',
      StAddress: '',
      StAge: undefined,
      DeptId: undefined,
      StSuper: undefined,
    };
  }
}