import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';

export interface LoginCredentials {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiresInSeconds: number;
  user: {
    username: string;
    role: string;
  };
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly tokenKey = environment.tokenStorageKey;
  private readonly expiryKey = `${this.tokenKey}_expiry`;
  private readonly authState = new BehaviorSubject<boolean>(this.hasValidToken());

  readonly authState$ = this.authState.asObservable();

  constructor(private http: HttpClient) {}

  login(credentials: LoginCredentials): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${environment.apiBaseUrl}/Auth/login`, credentials)
      .pipe(
        tap(response => {
          const expiresAt = Date.now() + response.expiresInSeconds * 1000;
          localStorage.setItem(this.tokenKey, response.token);
          localStorage.setItem(this.expiryKey, expiresAt.toString());
          this.authState.next(true);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.expiryKey);
    this.authState.next(false);
  }

  get token(): string | null {
    if (!this.hasValidToken()) {
      this.logout();
      return null;
    }
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return this.hasValidToken();
  }

  private hasValidToken(): boolean {
    const storedToken = localStorage.getItem(this.tokenKey);
    const expiry = Number(localStorage.getItem(this.expiryKey));

    if (!storedToken || !expiry) {
      return false;
    }

    if (Date.now() > expiry) {
      localStorage.removeItem(this.tokenKey);
      localStorage.removeItem(this.expiryKey);
      return false;
    }

    return true;
  }
}

