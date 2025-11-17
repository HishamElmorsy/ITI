import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { StudentDto } from '../../interfaces/api-student';
import { StudentService } from '../../services/student-service';

@Component({
  selector: 'app-get-student',
  imports: [RouterLink, CommonModule, FormsModule],
  templateUrl: './get-student.html',
  styleUrl: './get-student.css',
})
export class GetStudent implements OnInit {
  students: StudentDto[] = [];
  isLoading = false;
  errorMessage = '';
  searchText = '';

  constructor(private studentService: StudentService, private router: Router) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  showDetails(student: StudentDto): void {
    this.router.navigate(['/student/details', student.StId]);
  }

  refresh(): void {
    this.loadStudents();
  }

  private loadStudents(): void {
    this.errorMessage = '';
    this.isLoading = true;
    const query = this.searchText.trim();
    this.studentService.getStudents(query || undefined).subscribe({
      next: (response) => {
        this.students = response.data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load students.';
        this.isLoading = false;
      },
    });
  }
}
