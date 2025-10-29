using API_Lab2.DTO.DepartmentDTO;
using API_Lab2.DTO.StudentDTO;
using API_Lab2.Models;
using AutoMapper;

namespace API_Lab2.MapperConfig
{
    public class MapConfig:Profile
    {
        public MapConfig()
        {
            CreateMap<Student, ReadStudentDTO>()
           .ForMember(dest => dest.DeptName,
                      opt => opt.MapFrom(src => src.Dept != null ? src.Dept.DeptName : null))
           .ForMember(dest => dest.StSuperName,
                      opt => opt.MapFrom(src => src.StSuperNavigation != null ? src.StSuperNavigation.StFname : null));

            CreateMap<Department, ReadDepartmentDTO>()
           .ForMember(dest => dest.Students,
                      opt => opt.MapFrom(src => src.Students != null ? src.Students : null));

        }
    }
    
}
