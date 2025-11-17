using API_Lab2.DTO.StudentDTO;
using API_Lab2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Lab2.DTO.DepartmentDTO
{
    public class ReadDepartmentDTO 
    {
        public int DeptId { get; set; }

        public string? DeptName { get; set; }

        public string? DeptDesc { get; set; }

        public string? DeptLocation { get; set; }

        public int? DeptManager { get; set; }

        public virtual ICollection<ReadStudentDTO> Students { get; set; } = new List<ReadStudentDTO>();


    }
}
