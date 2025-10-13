using Microsoft.AspNetCore.Mvc;
using MVC_Lab_1.DAL;
using MVC_Lab_1.Models;
using MVC_Lab_1.Repo;

namespace MVC_Lab_1.Controllers
{
    public class DepartmentController : Controller
    {
        IEntityRepo<Department> departmentRepo;// = new DepartmentRepo();
        public DepartmentController(IEntityRepo<Department> _departmentRepo)
        {
            departmentRepo = _departmentRepo;
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
    }
}
