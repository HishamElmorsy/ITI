using API_Lab2.DTO.StudentDTO;
using API_Lab2.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        ItiContext db;
        IMapper map;
        public StudentsController(ItiContext db,IMapper map)
        {
            this.db = db;
            this.map = map;
        }
        [HttpGet]
        public ActionResult Get([FromQuery] string? search,[FromQuery] int pageNumber = 1 , [FromQuery]int pageSize = 10)
        {
            var query = db.Students
    .Include(st => st.Dept)
    .Include(st => st.StSuperNavigation).AsQueryable();

            if(!string.IsNullOrEmpty(search))
            {
                query = query.Where(st =>
                    (st.StFname != null && st.StFname.Contains(search)) ||
                    (st.StLname != null && st.StLname.Contains(search)));
            }
            var totalStudents = query.Count();

            var students = query.OrderBy(st => st.StId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            
            List<ReadStudentDTO> stdDTO = map.Map<List<ReadStudentDTO>>(students);
            var response = new
            {
                totalCount = totalStudents,
                pageNumber,
                pageSize,
                totalPages = (int)Math.Ceiling(totalStudents / (double)pageSize),
                data = stdDTO
            };
             return Ok(response);


        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var s = db.Students
    .Include(st => st.Dept)
    .Include(st => st.StSuperNavigation)
    .FirstOrDefault(st => st.StId == id);


            if (s == null) return NotFound();
            ReadStudentDTO stdDTO =  map.Map<ReadStudentDTO>(s);
             return Ok(stdDTO);
        }

        [HttpPost]
        public ActionResult post(Student s)
        {
            if (s == null) return BadRequest();
            db.Students.Add(s);
            db.SaveChanges();
            return CreatedAtAction("GetById", new { id = s.StId }, s);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Student s)
        {
            if (id != s.StId)
                return BadRequest("Student ID mismatch.");

            var existingStudent = db.Students.FirstOrDefault(s => s.StId == id);

            if (existingStudent == null)
                return NotFound($"Course with ID {id} not found.");

            existingStudent.StFname = s.StFname;
            existingStudent.StLname = s.StLname;
            existingStudent.Dept = s.Dept;

            db.SaveChanges();
            return NoContent();
        }

        [HttpDelete("deleteStudent/{id}")]
        public ActionResult DeleteStudent(int id)
        {
            var s = db.Students.FirstOrDefault(s => s.StId == id);

            if (s == null)
                return NotFound($"Student with ID {id} not found.");

            db.Students.Remove(s);
            db.SaveChanges();
            var updatedList = db.Students.ToList();

            return Ok(updatedList);
        }
    }
}
