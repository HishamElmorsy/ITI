using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Lab_1.DAL;
using MVC_Lab_1.Models;

namespace MVC_Lab_1.Controllers
{
    public class StudentController : Controller
    {
        ITIDbContext dbContext = new ITIDbContext();
        public IActionResult Index()
        {
            var model = dbContext.Students.Include(d=>d.Department).ToList();
            return View(model);
        }
        public IActionResult Details(int? id)
        {
            ViewBag.depts = dbContext.Departments.ToList();

            if (id == null)
            {
                return BadRequest();
            }
            var model = dbContext.Students.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.depts = dbContext.Departments.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student s)
        {
            dbContext.Students.Add(s);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Edit(int? id)
        {
            ViewBag.depts = dbContext.Departments.ToList();

            if (id == null)
            {
                return BadRequest();
            }
            var model = dbContext.Students.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Student s)
        {
            dbContext.Students.Update(s);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Delete(Student s)
        {
            dbContext.Students.Remove(s);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
