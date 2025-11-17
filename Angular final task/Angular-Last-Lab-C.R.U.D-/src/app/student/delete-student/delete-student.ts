import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { StudentService } from '../../services/student-service';

@Component({
  selector: 'app-delete-student',
  imports: [CommonModule, RouterLink],
  templateUrl: './delete-student.html',
  styleUrl: './delete-student.css',
})
export class DeleteStudent implements OnInit {
  studentId = 0;
  isDeleting = false;
  errorMessage = '';

  constructor(
    private studentService: StudentService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.studentId = Number(this.route.snapshot.paramMap.get('id'));
  }

  deleteStudent(): void {
    this.errorMessage = '';
    if (!this.studentId) {
      this.errorMessage = 'Invalid student id.';
      return;
    }

    this.isDeleting = true;
    this.studentService.deleteStudent(this.studentId).subscribe({
      next: () => {
        this.isDeleting = false;
        this.router.navigate(['/student/get-student']);
      },
      error: () => {
        this.isDeleting = false;
        this.errorMessage = 'Unable to delete student.';
      },
    });
  }
}
