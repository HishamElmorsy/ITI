import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements OnInit {
  credentials = {
    username: '',
    password: '',
  };

  isSubmitting = false;
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/department']);
    }
  }

  submit(): void {
    this.errorMessage = '';

    if (!this.credentials.username || !this.credentials.password) {
      this.errorMessage = 'Username and password are required.';
      return;
    }

    this.isSubmitting = true;

    this.authService.login(this.credentials).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.router.navigate(['/department']);
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage =
          typeof error?.error === 'string'
            ? error.error
            : 'Unable to login. Please try again.';
      },
    });
  }
}
