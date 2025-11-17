export interface StudentDto {
  StId: number;
  StFname?: string;
  StLname?: string;
  StAddress?: string;
  StAge?: number;
  DeptName?: string;
  StSuperName?: string;
  DeptId?: number;
  StSuper?: number;
}

export interface StudentPayload {
  StId?: number;
  StFname: string;
  StLname?: string;
  StAddress?: string;
  StAge?: number;
  DeptId?: number;
  StSuper?: number;
}

export interface StudentListResponse {
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  data: StudentDto[];
}

