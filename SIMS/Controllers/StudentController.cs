using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }
    }
}
