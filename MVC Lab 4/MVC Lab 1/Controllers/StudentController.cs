using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Lab_1.DAL;
using MVC_Lab_1.Models;
using MVC_Lab_1.Models.ViewModels;
using MVC_Lab_1.Repo;
using System.Runtime.CompilerServices;

namespace MVC_Lab_1.Controllers
{
    public class StudentController : Controller
    {
        IEntityRepo<Student> studentRepo;
        IEntityRepo<Department> departmentRepo;
        IStudentEmailExist emailExist;

        public StudentController(IEntityRepo<Department> _departmentRepo,IEntityRepo<Student>_studentRepo,IStudentEmailExist _emailExist)
        {
            departmentRepo = _departmentRepo;
            studentRepo = _studentRepo;
            emailExist = _emailExist;
        }
        public IActionResult Index()
        {
            var model = studentRepo.GetAll();
            return View(model);
        }
        public IActionResult Details(int? id)
        {
            ViewBag.depts = departmentRepo.GetAll();

            if (id == null)
            {
                return BadRequest();
            }
            var model = studentRepo.Get(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateStudentVM model = new CreateStudentVM() { Student = new Student()};
            model.Departments = departmentRepo.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(CreateStudentVM s)
        {
            if (ModelState.IsValid)
            {
                studentRepo.Insert(s.Student);
                studentRepo.Save();
                return RedirectToAction("index");
            }
            
            s.Departments = departmentRepo.GetAll();
            return View(s);
            
            
        }
        public IActionResult Edit(int? id)
        {

            if (id == null)
            {
                return BadRequest();
            }
            var s = studentRepo.Get(id.Value);
            if (s == null)
                return NotFound();

            var depts = departmentRepo.GetAll();
            CreateStudentVM model = new CreateStudentVM() { Student = s };
            model.Departments = depts;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(CreateStudentVM s)
        {
            studentRepo.Update(s.Student);
            studentRepo.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            studentRepo.Delete(id);
            studentRepo.Save();
            return RedirectToAction("Index");
        }
        public IActionResult CheckEmail(string Email)
        {
            bool b = emailExist.IsExist(Email);
            if (b)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

    }
}
