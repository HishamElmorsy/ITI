import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StudentDto, StudentListResponse, StudentPayload } from '../interfaces/api-student';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  private readonly baseUrl = `${environment.apiBaseUrl}/Students`;

  constructor(private http: HttpClient) {}

  getStudents(search?: string, pageNumber = 1, pageSize = 10): Observable<StudentListResponse> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (search) {
      params = params.set('search', search);
    }

    return this.http.get<StudentListResponse>(this.baseUrl, { params });
  }

  getStudentById(id: number): Observable<StudentDto> {
    return this.http.get<StudentDto>(`${this.baseUrl}/${id}`);
  }

  addStudent(payload: StudentPayload): Observable<StudentDto> {
    return this.http.post<StudentDto>(this.baseUrl, payload);
  }

  updateStudent(id: number, payload: StudentPayload): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  deleteStudent(id: number): Observable<StudentDto[]> {
    return this.http.delete<StudentDto[]>(`${this.baseUrl}/deleteStudent/${id}`);
  }
}
