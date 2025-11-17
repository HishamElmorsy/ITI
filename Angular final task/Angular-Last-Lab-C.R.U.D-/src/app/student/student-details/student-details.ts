import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StudentDto } from '../../interfaces/api-student';
import { StudentService } from '../../services/student-service';

@Component({
  selector: 'app-student-details',
  imports: [CommonModule],
  templateUrl: './student-details.html',
  styleUrl: './student-details.css',
})
export class StudentDetails implements OnInit {
  student?: StudentDto;
  isLoading = false;
  errorMessage = '';

  constructor(private route: ActivatedRoute, private studentService: StudentService) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadStudent(id);
    } else {
      this.errorMessage = 'Invalid student id.';
    }
  }

  private loadStudent(id: number): void {
    this.isLoading = true;
    this.studentService.getStudentById(id).subscribe({
      next: (student) => {
        this.student = student;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Student not found.';
        this.isLoading = false;
      },
    });
  }
}
