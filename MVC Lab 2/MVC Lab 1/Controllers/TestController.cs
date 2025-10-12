using Microsoft.AspNetCore.Mvc;
using MVC_Lab_1.Models;

namespace MVC_Lab_1.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Details()
        {
            List<Student> students = new List<Student>()
    {
        new Student(){Id = 1, Name = "Samy" , Age = 20},
        new Student(){Id = 2, Name = "Sally" , Age = 22},
        new Student(){Id = 3, Name = "Sara" , Age = 21},
        new Student(){Id = 4, Name = "Kara" , Age = 24},

    };
            return View(students);
        }

        public IActionResult Index()
        {
            return View();
        }

    }

}
