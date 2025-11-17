using API_Lab2.DTO.DepartmentDTO;
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
    public class DepartmentsController : ControllerBase
    {
        
            ItiContext db;
            IMapper map;
            public DepartmentsController(ItiContext db, IMapper map)
            {
                this.db = db;
                this.map = map;
            }
            [HttpGet]
            public ActionResult Get()
            {
            var d = db.Departments
    .Include(d => d.Students).ToList();
                List<ReadDepartmentDTO> deptDTO = map.Map<List<ReadDepartmentDTO>>(d);
                if (d.Count == 0) return NotFound();
                else return Ok(deptDTO);
            }
            [HttpGet("{id}")]
            public ActionResult GetById(int id)
            {
                var d = db.Departments
        .Include(d => d.Students)
        .FirstOrDefault(d => d.DeptId == id);


                if (d == null) return NotFound();
            ReadDepartmentDTO deptDTO = map.Map<ReadDepartmentDTO>(d);
                return Ok(deptDTO);
            }

            [HttpPost]
            public ActionResult post(Department d)
            {
                if (d == null) return BadRequest();
                db.Departments.Add(d);
                db.SaveChanges();
                return CreatedAtAction("GetById", new { id = d.DeptId }, d);
            }

            [HttpPut("{id}")]
            public ActionResult Put(int id, [FromBody] Department d)
            {
                if (id != d.DeptId)
                    return BadRequest("Department ID mismatch.");

                var existingDepartment = db.Departments.FirstOrDefault(d => d.DeptId == id);

                if (existingDepartment == null)
                    return NotFound($"Department with ID {id} not found.");

            existingDepartment.DeptName = d.DeptName;
            existingDepartment.DeptDesc = d.DeptDesc;
            existingDepartment.DeptManager = d.DeptManager;

            existingDepartment.DeptLocation = d.DeptLocation;


            db.SaveChanges();
                return NoContent();
            }

            [HttpDelete("deleteDepartment/{id}")]
            public ActionResult DeleteDepartment(int id)
            {
                var d = db.Departments.FirstOrDefault(d => d.DeptId == id);

                if (d == null)
                    return NotFound($"Department with ID {id} not found.");

                db.Departments.Remove(d);
                db.SaveChanges();
                var updatedList = db.Departments.ToList();

                return Ok(updatedList);
            
        }
    }
}
