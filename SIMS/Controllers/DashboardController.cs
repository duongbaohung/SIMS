using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Admin, Teacher, Student")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
