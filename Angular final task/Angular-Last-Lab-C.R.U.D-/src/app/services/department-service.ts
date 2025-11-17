import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DepartmentDto, DepartmentPayload } from '../interfaces/api-department';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private readonly baseUrl = `${environment.apiBaseUrl}/Departments`;

  constructor(private http: HttpClient) {}

  getDepartments(): Observable<DepartmentDto[]> {
    return this.http.get<DepartmentDto[]>(this.baseUrl);
  }

  getDepartmentById(id: number): Observable<DepartmentDto> {
    return this.http.get<DepartmentDto>(`${this.baseUrl}/${id}`);
  }

  addDepartment(department: DepartmentPayload): Observable<DepartmentDto> {
    return this.http.post<DepartmentDto>(this.baseUrl, department);
  }

  updateDepartment(id: number, department: DepartmentPayload): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, department);
  }

  deleteDepartment(id: number): Observable<DepartmentDto[]> {
    return this.http.delete<DepartmentDto[]>(`${this.baseUrl}/deleteDepartment/${id}`);
  }
}
