using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Lab_1.DAL;
using MVC_Lab_1.Models;
using MVC_Lab_1.Models.ViewModels;
using MVC_Lab_1.Repo;

namespace MVC_Lab_1.Controllers
{
    public class DepartmentController : Controller
    {
        IEntityRepo<Department> departmentRepo;// = new DepartmentRepo();
        private DepartmentCourseRepo departmentCourseRepo;
        private ITIDbContext db;

        public DepartmentController(IEntityRepo<Department> _departmentRepo, DepartmentCourseRepo _departmentCourseRepo, ITIDbContext _db)
        {
            departmentRepo = _departmentRepo;
            departmentCourseRepo = _departmentCourseRepo;
            db = _db;
        }
        public IActionResult Index()
        {
            var model = departmentRepo.GetAll();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department dept)
        {
            departmentRepo.Insert(dept);
            departmentRepo.Save();
            return RedirectToAction("index");
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = departmentRepo.Get(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = departmentRepo.Get(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Department dept)
        {
            departmentRepo.Update(dept);
            departmentRepo.Save();
            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Delete(Department dept)
        {
            departmentRepo.Delete(dept.DeptId);
            departmentRepo.Save();
            return RedirectToAction("index");
        }
        public IActionResult EditCourses(int id)
        {
            var model = departmentCourseRepo.GetDepartmentCourses(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditCourses(PostDepartmentCouseUpdateVM model)
        {
            departmentCourseRepo.UpdateDepartmentCourses(model);
            departmentCourseRepo.Save();
            return RedirectToAction("index");
        }
        public IActionResult ViewCourses(int id)
        {
            var dept = departmentCourseRepo.GetDepartmentWithCourses(id);
            return View(dept);
        }
        public IActionResult UpdateStudentDegree(int deptid, int crsid)
        {
            var dept = db.Departments
                         .Include(d => d.Students)
                         .FirstOrDefault(d => d.DeptId == deptid);

            var crs = db.Courses.FirstOrDefault(c => c.Id == crsid);

            var degrees = db.StudentCourses
                .Where(sc => sc.CourseId == crsid)
                .GroupBy(sc => sc.StudentId)
                .ToDictionary(g => g.Key, g => g.First().Degree ?? 0);

            ViewBag.crs = crs;
            ViewBag.degrees = degrees;

            return View(dept);
        }






        [HttpPost]

        public IActionResult UpdateStudentDegree(int deptid, int crsid, Dictionary<int, int> degree)
        {

            foreach (var item in degree)
            {
                var existing = db.StudentCourses
                                 .FirstOrDefault(sc => sc.StudentId == item.Key && sc.CourseId == crsid);

                if (existing != null)
                {
                    existing.Degree = item.Value;
                }
                else
                {
                    db.StudentCourses.Add(new StudentCourse()
                    {
                        CourseId = crsid,
                        StudentId = item.Key,
                        Degree = item.Value
                    });
                }
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }

    
}
