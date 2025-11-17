import { StudentDto } from './api-student';

export interface DepartmentDto {
  DeptId: number;
  DeptName?: string;
  DeptDesc?: string;
  DeptLocation?: string;
  DeptManager?: number;
  Students?: StudentDto[];
}

export interface DepartmentPayload {
  DeptId?: number;
  DeptName?: string;
  DeptDesc?: string;
  DeptLocation?: string;
  DeptManager?: number;
}

