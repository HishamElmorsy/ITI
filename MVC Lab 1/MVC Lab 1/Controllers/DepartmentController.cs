using Microsoft.AspNetCore.Mvc;
using MVC_Lab_1.DAL;
using MVC_Lab_1.Models;

namespace MVC_Lab_1.Controllers
{
    public class DepartmentController : Controller
    {
        ITIDbContext dbContext = new ITIDbContext();
        public IActionResult Index()
        {
            var model = dbContext.Departments.ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department dept)
        {
            dbContext.Departments.Add(dept);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
           var model = dbContext.Departments.Find(id);
            if(model==null)
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
            var model = dbContext.Departments.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Department dept)
        {
            dbContext.Departments.Update(dept);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Delete(Department dept)
        {
            dbContext.Departments.Remove(dept);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
